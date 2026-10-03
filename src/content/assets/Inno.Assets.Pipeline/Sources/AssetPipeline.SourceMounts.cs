using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Inno.Assets;
using Inno.Core.Execution;
using Inno.References;

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
}
