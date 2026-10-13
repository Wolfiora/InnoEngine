using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Installs verified Support Packs with rollback protection using explicitly registered platform sources.
/// </summary>
public sealed class PlayerSupportPackPublisher
{
    private readonly IReadOnlyDictionary<BuildTargetId, IPlayerSupportPackSource> m_sources;

    /// <summary>
    /// Freezes the platform sources used by this publisher.
    /// </summary>
    /// <param name="sources">
    /// Exactly one source for each supported target identity.
    /// </param>
    /// <exception cref="ArgumentException">
    /// A source is null or a target is registered more than once.
    /// </exception>
    public PlayerSupportPackPublisher(IEnumerable<IPlayerSupportPackSource> sources)
    {
        ArgumentNullException.ThrowIfNull(sources);
        var registered = new Dictionary<BuildTargetId, IPlayerSupportPackSource>();
        foreach (IPlayerSupportPackSource source in sources)
        {
            if (source is null || string.IsNullOrWhiteSpace(source.target.value)
                || !registered.TryAdd(source.target, source))
                throw new ArgumentException("Support Pack sources must be non-null and have unique target identities.", nameof(sources));
        }
        m_sources = registered;
    }

    /// <summary>
    /// Prepares a target closure in isolation and publishes its immutable deployment inputs.
    /// </summary>
    /// <param name="engineRoot">
    /// The source checkout containing InnoEngine.sln.
    /// </param>
    /// <param name="outputRoot">
    /// The directory owning installed target packs.
    /// </param>
    /// <param name="target">
    /// The registered target identity.
    /// </param>
    /// <param name="dotnetHost">
    /// The SDK executable selected by the host.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation before directory installation begins.
    /// </param>
    /// <returns>
    /// Reading ownership of the selected Pack and any post-commit diagnostics. Dispose after consuming its files.
    /// </returns>
    /// <remarks>
    /// Writers targeting the same pack share an exclusive, cancelable preparation lease.
    /// Publication atomically selects a fingerprint directory; previous generations remain valid for readers.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// A required path is blank or the target has no registered source.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The engine checkout is unavailable.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The prepared closure is incomplete or invalid.
    /// </exception>
    /// <exception cref="IOException">
    /// The staging tree, immutable artifact or atomic current index cannot be installed.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Preparation was canceled; the installed pack is unchanged.
    /// </exception>
    public async ValueTask<PlayerSupportPackPublication> PublishAsync(
        string engineRoot,
        string outputRoot,
        BuildTargetId target,
        string dotnetHost,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(engineRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(outputRoot);
        ArgumentException.ThrowIfNullOrWhiteSpace(dotnetHost);
        if (!m_sources.TryGetValue(target, out IPlayerSupportPackSource? source))
            throw new ArgumentException($"No Support Pack source is registered for '{target}'.", nameof(target));
        string root = Path.GetFullPath(engineRoot);
        if (!File.Exists(Path.Combine(root, "InnoEngine.sln")))
            throw new DirectoryNotFoundException($"Engine root '{root}' has no InnoEngine.sln.");
        cancellationToken.ThrowIfCancellationRequested();
        PlayerSupportPackPlan plan = await source.CreatePlanAsync(
            new PlayerSupportPackPlanningContext(root, dotnetHost), cancellationToken).ConfigureAwait(false);
        if (plan is null || plan.target != target)
            throw new InvalidDataException("The Support Pack plan does not match its registered target.");
        cancellationToken.ThrowIfCancellationRequested();
        var cache = new BuildCachePublisher(Path.GetFullPath(outputRoot));
        using FileLease cacheUse = await cache.AcquireUseAsync(cancellationToken).ConfigureAwait(false);
        string output = Path.GetFullPath(outputRoot);
        Directory.CreateDirectory(output);
        using FileLease ownership = await FileLease.AcquireAsync(
            Path.Combine(output, target.value + ".lock"), Timeout.InfiniteTimeSpan, cancellationToken)
            .ConfigureAwait(false);
        var catalog = new PlayerSupportPackCatalog(output);
        string? prepared = PlayerSupportPackPreparationIndex.Read(output, target.value, plan.preparationIdentity);
        if (prepared is not null)
        {
            try
            {
                PlayerSupportPackLease selected = await catalog.SelectAsync(target, prepared, source,
                    plan.VerifyInputsAsync, cancellationToken).ConfigureAwait(false);
                return Complete(selected, []);
            }
            catch (InvalidDataException)
            {
                // Missing or damaged output must be prepared again under the same publication owner.
            }
        }
        // A hit verifies fresh inputs inside SelectAsync after output verification and before
        // selection. No mutable source is consumed between ownership acquisition and that gate.
        await plan.VerifyInputsAsync(cancellationToken).ConfigureAwait(false);
        string transaction = Path.Combine(output, ".support-pack-" + Guid.NewGuid().ToString("N"));
        string staging = Path.Combine(transaction, target.value);
        PlayerSupportPackLease? published = null;
        List<string> diagnostics = [];
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(staging);
            await plan.PrepareAsync(staging, cancellationToken)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            published = await catalog.PublishAsync(target, staging, source, cancellationToken,
                plan.VerifyInputsAsync).ConfigureAwait(false);
            try
            {
                PlayerSupportPackPreparationIndex.Write(output, target.value, plan.preparationIdentity,
                    Path.GetFileName(published.directory));
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
            {
                diagnostics.Add($"Pack '{target}' was committed, but its preparation index could not be updated: {failure.Message}");
            }
        }
        finally
        {
            try
            {
                if (Directory.Exists(transaction))
                    Directory.Delete(transaction, recursive: true);
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
            {
                diagnostics.Add($"Support Pack staging '{transaction}' could not be removed: {failure.Message}");
            }
        }
        return Complete(published!, diagnostics);

        PlayerSupportPackPublication Complete(
            PlayerSupportPackLease pack,
            List<string> messages
        ) {
            try
            {
                cache.Record(pack.directory, pack.directory + ".read.lock", [], target.value, target.value);
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
            {
                messages.Add($"Pack '{target}' was committed, but its cache ownership record could not be updated: {failure.Message}");
            }
            return new PlayerSupportPackPublication(pack, messages);
        }
    }
}
