using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
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
    /// The absolute installed pack directory after successful validation.
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
    public async ValueTask<string> PublishAsync(
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
        string output = Path.GetFullPath(outputRoot);
        Directory.CreateDirectory(output);
        using FileLease ownership = await FileLease.AcquireAsync(
            Path.Combine(output, target.value + ".lock"), Timeout.InfiniteTimeSpan, cancellationToken)
            .ConfigureAwait(false);
        string transaction = Path.Combine(output, ".support-pack-" + Guid.NewGuid().ToString("N"));
        string staging = Path.Combine(transaction, target.value);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            Directory.CreateDirectory(staging);
            await plan.PrepareAsync(staging, cancellationToken)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            return await new PlayerSupportPackCatalog(output).PublishAsync(
                target, staging, source, cancellationToken).ConfigureAwait(false);
        }
        finally
        {
            if (Directory.Exists(transaction))
                Directory.Delete(transaction, recursive: true);
        }
    }
}
