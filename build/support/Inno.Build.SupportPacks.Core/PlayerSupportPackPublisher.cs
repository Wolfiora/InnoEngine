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
            if (source is null || !registered.TryAdd(source.target, source))
                throw new ArgumentException("Support Pack sources must be non-null and have unique target identities.", nameof(sources));
        }
        m_sources = registered;
    }

    /// <summary>
    /// Prepares a target closure in isolation, validates it and replaces the installed pack.
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
    /// The host must coordinate readers and writers targeting the same installed pack.
    /// Installation uses two directory moves; backup cleanup failures preserve the committed candidate.
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
    /// Installation fails, or the installed candidate's old backup cannot be removed.
    /// A cleanup failure preserves the complete installed pack and identifies the remaining backup.
    /// </exception>
    /// <exception cref="AggregateException">
    /// Installation and backup restoration both fail; retained directories are identified by the shared IO primitive.
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
        string output = Path.GetFullPath(outputRoot);
        Directory.CreateDirectory(output);
        string transaction = Path.Combine(output, ".support-pack-" + Guid.NewGuid().ToString("N"));
        string staging = Path.Combine(transaction, target.value);
        string destination = Path.Combine(output, target.value);
        Directory.CreateDirectory(staging);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            await source.PrepareAsync(new PlayerSupportPackBuildContext(root, staging, dotnetHost), cancellationToken)
                .ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            _ = new PlayerSupportPackCatalog(transaction).Resolve(target, source);
            cancellationToken.ThrowIfCancellationRequested();
            AtomicDirectory.Install(staging, destination);
            return destination;
        }
        finally
        {
            if (Directory.Exists(transaction))
                Directory.Delete(transaction, recursive: true);
        }
    }
}
