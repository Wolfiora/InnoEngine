using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Scripting.Api;
using Inno.Extensibility.Reload;
using Inno.References;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Owns cancellable Sample preparation and owner-thread publication through the shared source transaction.
/// </summary>
public sealed partial class AssetPipeline
{
    private AssetSampleImportTransaction? m_sampleImport;
    private bool m_reconcileSampleImport;

    internal bool isSampleImportFaulted => m_generations.state == GenerationState.Faulted;

    internal void FaultSampleImport(Exception failure) => m_generations.Fault(failure);

    /// <summary>
    /// Starts a private background sample clone without waiting for file copying or transformation.
    /// </summary>
    /// <param name="source">
    /// An indexed installed Plugin sample directory whose final segment starts with <c>~</c>.
    /// </param>
    /// <returns>
    /// The generation-pinned transaction to advance, validate and commit on this pipeline's owner thread.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The source is not an installed sample directory.
    /// </exception>
    /// <exception cref="IOException">
    /// The destination name already exists.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Another mutation or generation transaction is active, or this is not the initialization thread.
    /// </exception>
    [ScriptingApiIgnore]
    public AssetSampleImportTransaction PrepareSampleImport(AssetPath source)
    {
        EnsureOwnerThread();
        m_generations.EnsureReady("import an Asset sample");
        if (m_sourceMountCandidate is not null)
            throw new InvalidOperationException("A source-mount candidate is already pending.");
        AssetFileSystem fileSystem = GetFileSystem();
        if (!fileSystem.TryGetEntry(source, out AssetFileEntry entry) || !entry.isDirectory || !entry.isSample)
            throw new ArgumentException($"Asset source '{source}' is not an installed Plugin sample directory.", nameof(source));
        AssetPath target = AssetPath.Project(AssetSample.GetImportName(source));
        RequireSampleDestination(target);
        AssetSourceMount sourceMount = sourceMounts.Single(mount => mount.id == source.source);
        string transactionRoot = Path.Combine(libraryRoot, "AssetDatabase", "Transactions", Guid.NewGuid().ToString("N"));
        IDisposable generation = m_generations.AcquireRead("import an Asset sample");
        SerializationGeneration? serialization = null;
        var rewriters = new List<IAssetSampleSourceRewriter>();
        try
        {
            serialization = m_serialization.CaptureGeneration();
            foreach (Type type in m_types.GetTypesWithAttribute<AssetSampleSourceRewriterAttribute>()
                         .Select(reference => reference.Resolve(m_types))
                         .OrderBy(type => type.GetCustomAttributes(typeof(AssetSampleSourceRewriterAttribute), false)
                             .Cast<AssetSampleSourceRewriterAttribute>().Single().id, StringComparer.Ordinal))
            {
                if (type.IsAbstract || !typeof(IAssetSampleSourceRewriter).IsAssignableFrom(type)
                    || Activator.CreateInstance(type) is not IAssetSampleSourceRewriter rewriter)
                    throw new InvalidOperationException($"Sample transformer '{type.FullName}' is not a concrete source rewriter.");
                rewriters.Add(rewriter);
            }
            m_sampleImport = new AssetSampleImportTransaction(
                this, source, target, sourceMount.Resolve(source.localPath), transactionRoot,
                m_options.sourcePolicy ?? AssetSourcePolicy.defaultPolicy,
                generation, serialization, rewriters.ToArray());
            return m_sampleImport;
        }
        catch (Exception failure)
        {
            var cleanup = new LifetimeScope();
            cleanup.Own(generation);
            if (serialization is not null)
                cleanup.Own(serialization);
            foreach (IDisposable rewriter in rewriters.OfType<IDisposable>())
                cleanup.Own(rewriter);
            try
            {
                cleanup.Dispose();
            }
            catch (Exception retirement)
            {
                throw new AggregateException("Sample capture and resource retirement failed.", failure, retirement);
            }
            throw;
        }
    }

    internal void EnsureSampleImportOwner(AssetSampleImportTransaction transaction)
    {
        EnsureInitializationThread();
        if (!ReferenceEquals(m_sampleImport, transaction))
            throw new InvalidOperationException("The sample transaction no longer belongs to this pipeline.");
    }

    internal Task<PreparedSourceMounts> PrepareSampleCatalogAsync(
        AssetSampleImportTransaction transaction,
        CancellationToken cancellationToken
    ) {
        EnsureSampleImportOwner(transaction);
        AssetFileSystem fileSystem = GetFileSystem();
        RequireSampleDestination(transaction.target);
        // The candidate scans current sources; completion reconciles events missed while watching is paused.
        transaction.restartWatcher = fileSystem.isWatching;
        fileSystem.Stop();
        string destination = Path.Combine(assetRoot, transaction.target.localPath);
        Directory.Move(transaction.stagedSource, destination);
        transaction.sourceMoved = true;
        if (System.IO.File.Exists(transaction.stagedSource + ".imeta"))
        {
            System.IO.File.Move(transaction.stagedSource + ".imeta", destination + ".imeta");
            transaction.metaMoved = true;
        }
        AssetLoader activeLoader = GetLoader();
        transaction.recovery = activeLoader.CaptureRecoveryStates();
        AssetSourceMount[] mounts = sourceMounts.ToArray();
        return Task.Run(() =>
        {
            PreparedSourceMounts prepared = PrepareSourceMountCatalog(activeLoader, mounts, cancellationToken);
            try
            {
                AssetLoader loader = prepared.catalog.loader;
                foreach (string localPath in transaction.entries)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    AssetPath candidate = AssetPath.Project(localPath);
                    if (!loader.TryGetAssetType(candidate, out Type? assetType) || assetType is null)
                        continue;
                    AssetObject? imported = loader.Load(candidate, assetType);
                    if (imported is null || imported.isMissing)
                        throw new InvalidDataException($"Sample asset '{candidate}' failed pre-publication import validation.");
                }
                cancellationToken.ThrowIfCancellationRequested();
                return prepared;
            }
            catch (Exception failure)
            {
                try
                {
                    DiscardSourceMountCatalog(prepared);
                }
                catch (Exception cleanup)
                {
                    throw new AggregateException("Sample validation and candidate retirement failed.", failure, cleanup);
                }
                throw;
            }
        }, cancellationToken);
    }

    internal void AdoptSampleCatalog(
        AssetSampleImportTransaction transaction,
        PreparedSourceMounts prepared
    ) {
        EnsureSampleImportOwner(transaction);
        transaction.candidate = AdoptSourceMountCatalog(prepared, transaction.recovery);
    }

    internal void DiscardSampleCatalog(PreparedSourceMounts prepared) => DiscardSourceMountCatalog(prepared);

    internal void CommitSampleImport(
        AssetSampleImportTransaction transaction,
        Action<AssetPath>? beforePublish
    ) {
        EnsureSampleImportOwner(transaction);
        var changes = new List<AssetChangedEvent>(transaction.entries.Count + 1)
        {
            new(transaction.target.localPath, WatcherChangeTypes.Created)
        };
        changes.AddRange(transaction.entries.Select(static path => new AssetChangedEvent(path, WatcherChangeTypes.Created)));
        AssetChange[] committed = CreateCommittedChanges(
            transaction.candidate!.candidateLoader, changes, new Dictionary<string, Guid>(StringComparer.OrdinalIgnoreCase), requiresFullRescan: false);
        transaction.candidate!.Activate();
        beforePublish?.Invoke(transaction.target);
        transaction.candidate.Complete();
        FinishSampleImport(transaction);
        InvokeObservers(Changed, new AssetChangeSet(m_revision, committed));
    }

    internal void RollbackSampleImport(AssetSampleImportTransaction transaction)
    {
        EnsureSampleImportOwner(transaction);
        transaction.candidate?.Rollback();
        string destination = Path.Combine(assetRoot, transaction.target.localPath);
        if (transaction.metaMoved && System.IO.File.Exists(destination + ".imeta"))
        {
            System.IO.File.Move(destination + ".imeta", transaction.stagedSource + ".imeta");
            transaction.metaMoved = false;
        }
        if (transaction.sourceMoved && Directory.Exists(destination))
        {
            Directory.Move(destination, transaction.stagedSource);
            transaction.sourceMoved = false;
        }
        if (!transaction.TryCleanPrivateCopy())
            throw new RetirementPendingException("Sample import private storage is still being removed.");
        FinishSampleImport(transaction);
    }

    internal Task CleanSampleStorageAsync(string transactionRoot)
        => Task.Run(() => DeleteTransactionDirectorySafely(transactionRoot));

    private void FinishSampleImport(AssetSampleImportTransaction transaction)
    {
        transaction.Finish();
        DeleteTransactionDirectorySafely(transaction.transactionRoot);
        if (transaction.restartWatcher)
        {
            GetFileSystem().Start();
            m_reconcileSampleImport = true;
        }
        m_sampleImport = null;
    }

    private void RequireSampleDestination(AssetPath target)
    {
        string destination = Path.Combine(assetRoot, target.localPath);
        if (Directory.Exists(destination) || System.IO.File.Exists(destination) || System.IO.File.Exists(destination + ".imeta"))
            throw new IOException($"Sample import target '{target}' already exists.");
    }
}
