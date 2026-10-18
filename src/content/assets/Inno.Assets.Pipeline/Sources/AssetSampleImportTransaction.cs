using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

using Inno.Assets;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.References;

namespace Inno.Assets.Pipeline;

/// <summary>
/// Owns a cancellable sample clone, its pinned generation and its owner-thread publication.
/// </summary>
/// <remarks>
/// Advance and Commit run on the AssetPipeline initialization thread. A prepared copy is indexed
/// in an isolated catalog for validation without publishing Changed; ordinary mutations and generation changes are deferred.
/// Rollback must finish before the pipeline or any validation dependency can be released.
/// </remarks>
public sealed class AssetSampleImportTransaction : IDisposable
{
    private readonly AssetPipeline m_owner;
    private readonly LifetimeScope m_lifetime = new();
    private readonly LifetimeScope m_preparationResources = new();
    private readonly LifetimeScope m_cleanupResources = new();
    private readonly RetirementBarrier m_retirement = new("Asset sample import");
    private Task<Preparation>? m_preparation;
    private Task<Exception?>? m_validation;
    private Task<Indexing>? m_indexing;
    private Task<Exception?>? m_cleanup;
    private bool m_finished;

    internal AssetSampleImportTransaction(
        AssetPipeline owner,
        AssetPath source,
        AssetPath target,
        string absoluteSource,
        string transactionRoot,
        AssetSourcePolicy policy,
        IDisposable generation,
        SerializationGeneration serialization,
        IAssetSampleSourceRewriter[] rewriters
    ) {
        m_owner = owner;
        this.target = target;
        this.transactionRoot = transactionRoot;
        m_lifetime.Own(generation);
        m_lifetime.Own(m_cleanupResources);
        m_lifetime.Own(m_preparationResources);
        m_preparationResources.Own(serialization);
        foreach (IAssetSampleSourceRewriter rewriter in rewriters)
        {
            if (rewriter is IDisposable disposable)
                m_preparationResources.Own(disposable);
        }
        m_preparation = m_lifetime.RunAsync<Preparation>(async cancellationToken =>
        {
            try
            {
                return await Task.Run(() =>
                {
                    List<string> entries = AssetSampleSnapshot.Capture(
                        absoluteSource, stagedSource, target.localPath, policy, cancellationToken);
                    AssetSampleSnapshot.RemapIdentities(
                        stagedSource, source, target, serialization, context =>
                        {
                            foreach (IAssetSampleSourceRewriter rewriter in rewriters)
                            {
                                cancellationToken.ThrowIfCancellationRequested();
                                rewriter.Transform(context);
                            }
                        }, cancellationToken);
                    return new Preparation(entries, null);
                }, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                EnsureFailureDrained(exception);
                return new Preparation([], exception);
            }
        });
    }

    /// <summary>
    /// Gets the destination path with the original sample directory name preserved.
    /// </summary>
    public AssetPath target { get; }

    /// <summary>
    /// Gets whether failed publication or retirement has faulted the host and requires a restart.
    /// </summary>
    public bool isFaulted => m_owner.isSampleImportFaulted;

    /// <summary>
    /// Gets whether background validation has drained and Commit can inspect its outcome.
    /// </summary>
    public bool isValidationComplete => m_validation?.IsCompleted == true;

    /// <summary>
    /// Starts background indexing after cloning, then adopts its completed catalog without waiting.
    /// </summary>
    /// <returns>
    /// True when the candidate is indexed and ready for BeginValidation; false while preparation is pending.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// The transaction is finished or the caller is not its initialization thread.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation or indexing observed cancellation before a candidate could be adopted.
    /// </exception>
    public bool Advance()
    {
        EnsureOpen();
        m_owner.EnsureSampleImportOwner(this);
        if (isIndexed)
            return true;
        Task<Preparation> preparation = m_preparation!;
        if (!preparation.IsCompleted)
            return false;
        Preparation outcome = preparation.GetAwaiter().GetResult();
        m_preparationResources.Dispose();
        if (outcome.failure is not null)
            ExceptionDispatchInfo.Capture(outcome.failure).Throw();
        entries = outcome.entries;
        if (m_indexing is null)
        {
            m_indexing = m_lifetime.RunAsync<Indexing>(async cancellationToken =>
            {
                try
                {
                    AssetPipeline.PreparedSourceMounts prepared = await m_owner.PrepareSampleCatalogAsync(
                        this, cancellationToken).ConfigureAwait(false);
                    return new Indexing(prepared, null);
                }
                catch (Exception failure)
                {
                    EnsureFailureDrained(failure);
                    return new Indexing(null, failure);
                }
            });
            return false;
        }
        if (!m_indexing.IsCompleted)
            return false;
        Indexing indexed = m_indexing.GetAwaiter().GetResult();
        if (indexed.failure is not null)
            ExceptionDispatchInfo.Capture(indexed.failure).Throw();
        m_owner.AdoptSampleCatalog(this, indexed.prepared!);
        isIndexed = true;
        return true;
    }

    /// <summary>
    /// Starts one authoring preflight after indexing, retaining its dependencies until it drains.
    /// </summary>
    /// <param name="validate">
    /// Owner-thread capture followed by asynchronous validation. Continuations must not mutate live Assets.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The validation callback is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The candidate is not indexed, validation already started, or the transaction is finished.
    /// </exception>
    public void BeginValidation(Func<IAssetSourceSnapshot, CancellationToken, ValueTask> validate)
    {
        ArgumentNullException.ThrowIfNull(validate);
        EnsureOpen();
        m_owner.EnsureSampleImportOwner(this);
        if (!isIndexed || m_validation is not null)
            throw new InvalidOperationException("Sample validation requires one indexed, unvalidated candidate.");
        m_validation = m_lifetime.RunAsync<Exception?>(async cancellationToken =>
        {
            try
            {
                await validate(new SourceSnapshot(candidate!), cancellationToken).ConfigureAwait(false);
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }
            catch (Exception exception)
            {
                EnsureFailureDrained(exception);
                return exception;
            }
        });
    }

    /// <summary>
    /// Publishes a successfully validated import exactly once, without waiting for unfinished work.
    /// </summary>
    /// <param name="beforePublish">
    /// Optional owner-thread finalization, such as recording neutral History data. Failure leaves the
    /// transaction open for Rollback and no Changed notification is published.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Validation is unfinished, the transaction is finished, or the caller is not its owner thread.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The import or its validation was canceled before publication.
    /// </exception>
    public void Commit(Action<AssetPath>? beforePublish = null)
    {
        EnsureOpen();
        m_owner.EnsureSampleImportOwner(this);
        if (!isValidationComplete)
            throw new InvalidOperationException("Sample validation has not completed.");
        Exception? failure = m_validation!.GetAwaiter().GetResult();
        if (failure is not null)
            ExceptionDispatchInfo.Capture(failure).Throw();
        m_lifetime.cancellationToken.ThrowIfCancellationRequested();
        m_owner.CommitSampleImport(this, beforePublish);
    }

    /// <summary>
    /// Requests cancellation of copying and validation while retaining all dependencies.
    /// </summary>
    /// <remarks>
    /// Cancellation does not wait. Continue owner-thread retirement until Rollback or Dispose completes.
    /// </remarks>
    public void Cancel() => m_lifetime.Cancel();

    /// <summary>
    /// Cancels work and removes an uncommitted copy after all background consumers have drained.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Background work is still draining; retain the transaction and retry on its owner thread.
    /// </exception>
    public void Rollback()
    {
        if (m_finished)
            return;
        m_owner.EnsureSampleImportOwner(this);
        Cancel();
        try
        {
            if (!m_retirement.TryComplete(() =>
                {
                    if (!m_lifetime.isQuiescent)
                        throw new RetirementPendingException("Sample import work must drain before rollback releases its generation.");
                    if (candidate is null && m_indexing is { IsCompletedSuccessfully: true })
                    {
                        Indexing indexed = m_indexing.GetAwaiter().GetResult();
                        if (indexed.prepared is not null)
                            m_owner.DiscardSampleCatalog(indexed.prepared);
                        m_indexing = null;
                    }
                    m_owner.RollbackSampleImport(this);
                }))
                throw new RetirementPendingException("Sample import retirement is waiting for canceled work.");
        }
        catch (Exception pending) when (RetirementPendingException.Find(pending) is not RetirementTimeoutException
            && RetirementPendingException.Find(pending) is not null)
        {
            throw;
        }
        catch (Exception failure)
        {
            m_owner.FaultSampleImport(failure);
            throw;
        }
    }

    /// <summary>
    /// Retires an uncommitted import on its owner thread after cancellation has drained.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Canceled work has not drained; disposal must be retried without releasing its owner.
    /// </exception>
    public void Dispose() => Rollback();

    internal IReadOnlyList<SerializedMissingState> recovery { get; set; } = [];
    internal AssetSourceMountTransaction? candidate { get; set; }
    internal string transactionRoot { get; }
    internal string stagedSource => System.IO.Path.Combine(transactionRoot, "sample");
    internal List<string> entries { get; private set; } = [];
    internal bool isIndexed { get; private set; }
    internal bool sourceMoved { get; set; }
    internal bool metaMoved { get; set; }
    internal bool restartWatcher { get; set; }

    internal void Finish()
    {
        m_lifetime.Dispose();
        candidate = null;
        recovery = [];
        m_indexing = null;
        m_preparation = null;
        m_validation = null;
        m_cleanup = null;
        entries.Clear();
        m_finished = true;
    }

    internal bool TryCleanPrivateCopy()
    {
        // Cleanup is admitted separately because cancellation has already closed the work lifetime.
        m_cleanup ??= m_cleanupResources.RunAsync<Exception?>(async _ =>
        {
            try
            {
                await m_owner.CleanSampleStorageAsync(transactionRoot).ConfigureAwait(false);
                return null;
            }
            catch (Exception failure)
            {
                return failure;
            }
        });
        if (!m_cleanup.IsCompleted)
            return false;
        Exception? failure = m_cleanup.GetAwaiter().GetResult();
        if (failure is not null)
            ExceptionDispatchInfo.Capture(failure).Throw();
        return true;
    }

    private void EnsureOpen()
    {
        if (m_finished)
            throw new InvalidOperationException("The sample import transaction is finished.");
    }

    private void EnsureFailureDrained(Exception failure)
    {
        if (RetirementPendingException.Find(failure) is null)
            return;
        // A completed task reporting pending ownership must stay retained by the Core lifetime.
        m_owner.FaultSampleImport(failure);
        ExceptionDispatchInfo.Capture(failure).Throw();
    }

    private sealed class SourceSnapshot(AssetSourceMountTransaction candidate) : IAssetSourceSnapshot
    {
        IReadOnlyList<AssetSourceMount> IAssetSourceSnapshot.sourceMounts => candidate.sourceMounts;

        IReadOnlyList<AssetFileEntry> IAssetSourceSnapshot.GetFileSystemEntries(bool includeDirectories)
            => candidate.GetFileSystemEntries(includeDirectories);

        TAsset IAssetSourceSnapshot.Load<TAsset>(AssetPath path) => candidate.Load<TAsset>(path);

        bool IAssetSourceSnapshot.TryGetInfo(
            AssetPath path,
            out AssetInfo? info
        ) => candidate.TryGetInfo(path, out info);

        ArtifactLease IAssetArtifactLookup.AcquireArtifact(
            Guid persistentId,
            string outputName
        ) => candidate.AcquireArtifact(persistentId, outputName);

        bool IAssetArtifactLookup.TryGetArtifact(
            Guid persistentId,
            string outputName,
            out AssetArtifactInfo? artifact
        ) => candidate.TryGetArtifact(persistentId, outputName, out artifact);
    }

    private sealed record Indexing(
        AssetPipeline.PreparedSourceMounts? prepared,
        Exception? failure
    );

    private sealed record Preparation(
        List<string> entries,
        Exception? failure
    );
}
