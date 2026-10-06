using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Inno.Assets;
using Inno.Core.Execution;
using Inno.References;
using System.IO;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Extensibility.Modules;
using Inno.Core.Identity;
using Inno.Core.Diagnostics;
using Inno.Core.Logging;
using Inno.Extensibility.Types;
using Inno.Scripting.Api;
using Inno.Core.Serialization;
using Inno.Extensibility.Reload;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Prepares isolated source catalogs and adopts them at the pipeline's owner-thread safe point.
/// </summary>
public sealed partial class AssetPipeline
{
    private AssetSourceMountTransaction PrepareSourceMountsCore(IReadOnlyList<AssetSourceMount> mounts)
    {
        using IDisposable operationScope = AcquireOperation();
        lock (m_lifecycleLock)
        {
            m_generations.EnsureRetirementSafe();
            if (m_failedPreparation is not null || m_shutdown is not null)
                throw new InvalidOperationException("The Asset Pipeline is retiring and cannot prepare a source candidate.");
            if (m_sourceMountCandidate is not null)
                throw new InvalidOperationException("Another source-mount candidate is already pending.");
            AssetSourceMount[] snapshot = mounts.ToArray();
            AssetSourceMount projectMount = snapshot.SingleOrDefault(static mount => mount.id == AssetSourceId.project)
                ?? throw new ArgumentException("A project asset source mount is required.", nameof(mounts));
            if (projectMount.isReadOnly)
                throw new ArgumentException("The project asset source mount must be writable.", nameof(mounts));
            if (snapshot.Select(static mount => mount.id).Distinct().Count() != snapshot.Length)
                throw new ArgumentException("Asset source mount IDs must be unique.", nameof(mounts));
            PreparedSourceMounts prepared = PrepareSourceMountCatalog(GetLoader(), snapshot, CancellationToken.None);
            try
            {
                return AdoptSourceMountCatalog(prepared, GetLoader().CaptureRecoveryStates());
            }
            catch (Exception failure)
            {
                try
                {
                    DiscardSourceMountCatalog(prepared);
                }
                catch (Exception cleanup)
                {
                    throw new AggregateException("Asset source adoption and retirement failed.", failure, cleanup);
                }
                throw;
            }
        }
    }

    private PreparedSourceMounts PrepareSourceMountCatalog(
        AssetLoader activeLoader,
        AssetSourceMount[] mounts,
        CancellationToken cancellationToken
    ) {
        AssetCatalogCandidate? catalog = null;
        AssetFileSystem? files = null;
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            catalog = activeLoader.PrepareCatalogCandidate(mounts, m_options.sourcePolicy);
            AssetLoader loader = catalog.loader;
            files = new AssetFileSystem(
                mounts, autoStart: false, m_options.fileWatcherFlushDelayMs, m_options.sourcePolicy,
                requireWritableProject: true, m_identities,
                persistentIdentityResolver: path => loader.TryGetPersistentId(path, out Guid id) ? id : null,
                activateIdentities: false);
            loader.Rescan(cancellationToken);
            files.Refresh();
            cancellationToken.ThrowIfCancellationRequested();
            return new PreparedSourceMounts(mounts, catalog, files);
        }
        catch (Exception failure)
        {
            try
            {
                RetireSourcePreparation(catalog, files);
            }
            catch (Exception cleanup)
            {
                throw new AggregateException("Asset source preparation and retirement failed.", failure, cleanup);
            }
            throw;
        }
    }

    private AssetSourceMountTransaction AdoptSourceMountCatalog(
        PreparedSourceMounts prepared,
        IReadOnlyList<SerializedMissingState> recovery
    ) {
        EnsureInitializationThread();
        if (m_sourceMountCandidate is not null)
            throw new InvalidOperationException("Another source-mount candidate is already pending.");
        var transaction = new AssetSourceMountTransaction(
            this, prepared.mounts, prepared.catalog, prepared.files, recovery, Math.Max(1, m_revision + 1));
        m_sourceMountCandidate = transaction;
        return transaction;
    }

    private void DiscardSourceMountCatalog(PreparedSourceMounts prepared)
        => RetireSourcePreparation(prepared.catalog, prepared.files);

    private void RetireSourcePreparation(
        AssetCatalogCandidate? catalog,
        AssetFileSystem? files
    ) {
        var resources = new LifetimeScope();
        if (catalog is not null)
        {
            resources.Own(catalog);
            resources.Own(catalog.loader);
        }
        if (files is not null)
            resources.Own(files);
        try
        {
            Retire(resources, new RetirementBarrier("Asset source preparation"));
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            lock (m_lifecycleLock)
                m_failedPreparation = resources;
            throw;
        }
    }

    internal sealed record PreparedSourceMounts(
        AssetSourceMount[] mounts,
        AssetCatalogCandidate catalog,
        AssetFileSystem files
    );

    /// <summary>
    /// Validates and atomically replaces the complete source-mount generation while preserving the active
    /// generation after any candidate failure.
    /// </summary>
    /// <param name="mounts">
    /// A writable project source followed by zero or more read-only sources.
    /// </param>
    [ScriptingApiIgnore]
    public void ReplaceSourceMounts(IReadOnlyList<AssetSourceMount> mounts)
    {
        using AssetSourceMountTransaction transaction = PrepareSourceMounts(mounts);
        transaction.Activate();
        transaction.Complete();
    }

    /// <summary>
    /// Builds and validates an isolated source-mount candidate without changing active AssetPipeline state.
    /// </summary>
    /// <param name="mounts">
    /// A writable project source followed by zero or more read-only sources.
    /// </param>
    /// <returns>
    /// A transaction that can be inspected, activated, completed, or rolled back.
    /// </returns>
    [ScriptingApiIgnore]
    public AssetSourceMountTransaction PrepareSourceMounts(IReadOnlyList<AssetSourceMount> mounts)
    {
        ArgumentNullException.ThrowIfNull(mounts);
        EnsureOwnerThread();
        return PrepareSourceMountsCore(mounts);
    }

    internal void ActivatePreparedSourceMounts(AssetSourceMountTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        EnsureInitializationThread();
        using IDisposable operationScope = AcquireOperation();
        lock (m_lifecycleLock)
        {
            EnsurePendingSourceMountTransaction(transaction);
            if (transaction.isActivated)
                return;
            transaction.candidateLoader.ActivateExtensionDiscovery();
            _ = transaction.candidateLoader.RefreshRegistries();
            transaction.candidateFileSystem.Refresh();
            transaction.previousLoader = m_loader;
            transaction.previousFileSystem = m_fileSystem;
            transaction.previousMounts = sourceMounts;
            transaction.previousOptions = m_options;
            SwitchSourceIdentities(transaction.previousLoader!, transaction.previousFileSystem!,
                transaction.candidateLoader, transaction.candidateFileSystem);
            transaction.candidateLoader.AssetReloaded += OnAssetReloaded;
            m_loader = transaction.candidateLoader;
            m_fileSystem = transaction.candidateFileSystem;
            sourceMounts = transaction.sourceMounts;
            AssetSourceMount project = transaction.sourceMounts.Single(
                static mount => mount.id == AssetSourceId.project);
            assetRoot = project.rootPath;
            m_options = m_options with { assetRoot = assetRoot, sourceMounts = transaction.sourceMounts };
            m_revision++;
            transaction.isActivated = true;
        }
    }

    internal void CompletePreparedSourceMounts(AssetSourceMountTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        EnsureInitializationThread();
        using IDisposable operationScope = AcquireOperation();
        Action? changed;
        lock (m_lifecycleLock)
        {
            m_generations.EnsureRetirementSafe();
            EnsurePendingSourceMountTransaction(transaction);
            if (!transaction.isActivated)
                throw new InvalidOperationException("A source-mount candidate must be activated before completion.");
            try
            {
                transaction.catalogCandidate.Commit();
                if (m_options.enableFileSystemWatcher)
                    transaction.candidateFileSystem.Start();
            }
            catch (Exception failure)
            {
                m_generations.Fault(failure);
                throw;
            }
            transaction.retirement = new LifetimeScope();
            transaction.retirement.Own(transaction.catalogCandidate);
            if (transaction.previousLoader is not null)
            {
                transaction.previousLoader.AssetReloaded -= OnAssetReloaded;
                transaction.retirement.Own(transaction.previousLoader);
            }
            if (transaction.previousFileSystem is not null)
                transaction.retirement.Own(transaction.previousFileSystem);
            RetireSourceMounts(transaction);
            changed = SourceMountsChanged;
        }
        InvokeObservers(changed);
    }

    internal void RollbackPreparedSourceMounts(AssetSourceMountTransaction transaction)
    {
        ArgumentNullException.ThrowIfNull(transaction);
        if (transaction.isFinished)
            return;
        EnsureInitializationThread();
        using IDisposable operationScope = AcquireOperation();
        lock (m_lifecycleLock)
        {
            m_generations.EnsureRetirementSafe();
            EnsurePendingSourceMountTransaction(transaction);
            if (transaction.isActivated)
            {
                transaction.candidateLoader.AssetReloaded -= OnAssetReloaded;
                SwitchSourceIdentities(transaction.candidateLoader, transaction.candidateFileSystem,
                    transaction.previousLoader!, transaction.previousFileSystem!);
                m_loader = transaction.previousLoader;
                m_fileSystem = transaction.previousFileSystem;
                sourceMounts = transaction.previousMounts ?? [];
                m_options = transaction.previousOptions;
                AssetSourceMount? project = sourceMounts.SingleOrDefault(
                    static mount => mount.id == AssetSourceId.project);
                assetRoot = project?.rootPath ?? string.Empty;
                m_revision++;
                transaction.isActivated = false;
            }
            transaction.retirement = new LifetimeScope();
            transaction.retirement.Own(transaction.catalogCandidate);
            transaction.retirement.Own(transaction.candidateLoader);
            transaction.retirement.Own(transaction.candidateFileSystem);
            RetireSourceMounts(transaction);
        }
    }

    private void RetireSourceMounts(AssetSourceMountTransaction transaction)
    {
        try
        {
            Retire(transaction.retirement!, transaction.retirementBarrier);
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            Finish();
            throw;
        }
        Finish();

        void Finish()
        {
            transaction.previousLoader = null;
            transaction.previousFileSystem = null;
            transaction.previousMounts = null;
            transaction.isFinished = true;
            m_sourceMountCandidate = null;
        }
    }

    private void Retire(
        LifetimeScope lifetime,
        RetirementBarrier barrier
    ) {
        m_generations.EnsureRetirementSafe();
        try
        {
            barrier.Wait(lifetime.Dispose);
        }
        catch (Exception failure)
        {
            m_generations.Fault(failure);
            throw;
        }
    }

    private void DeleteCandidateCatalogRoots(string activeLibraryRoot)
    {
        string root = Path.Combine(activeLibraryRoot, "AssetDatabase", "Candidates");
        if (Directory.Exists(root))
            Directory.Delete(root, recursive: true);
    }

    private void SwitchSourceIdentities(
        AssetLoader previousLoader,
        AssetFileSystem previousFiles,
        AssetLoader candidateLoader,
        AssetFileSystem candidateFiles
    ) {
        var rollback = new Stack<Action>();
        try
        {
            Apply(() => previousLoader.SetIdentitiesActive(false), () => previousLoader.SetIdentitiesActive(true));
            Apply(previousFiles.DeactivateIdentities, previousFiles.ActivateIdentities);
            Apply(() => candidateLoader.SetIdentitiesActive(true), () => candidateLoader.SetIdentitiesActive(false));
            Apply(candidateFiles.ActivateIdentities, candidateFiles.DeactivateIdentities);
        }
        catch (Exception failure) when (RetirementPendingException.Find(failure) is not null)
        {
            m_generations.Fault(failure);
            throw;
        }
        catch (Exception failure)
        {
            List<Exception> failures = [failure];
            while (rollback.TryPop(out Action? restore))
            {
                try
                {
                    restore();
                }
                catch (Exception pending) when (RetirementPendingException.Find(pending) is not null)
                {
                    m_generations.Fault(pending);
                    throw;
                }
                catch (Exception compensation)
                {
                    failures.Add(compensation);
                }
            }
            if (failures.Count > 1)
            {
                var aggregate = new AggregateException("Asset identity publication could not restore its previous domain.", failures);
                m_generations.Fault(aggregate);
                throw aggregate;
            }
            throw;
        }

        void Apply(
            Action apply,
            Action compensate
        ) {
            rollback.Push(compensate);
            apply();
        }
    }

    private void EnsurePendingSourceMountTransaction(AssetSourceMountTransaction transaction)
    {
        if (!ReferenceEquals(m_sourceMountCandidate, transaction))
            throw new InvalidOperationException("The source-mount transaction is not the current candidate.");
    }
}
