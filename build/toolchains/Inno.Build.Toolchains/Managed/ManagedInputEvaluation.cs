using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal sealed class ManagedInputEvaluation : IDisposable
{
    private readonly CoordinatedFileLease? m_ownership;
    private readonly BuildCachePublisher? m_cache;
    private readonly string m_directory;
    private readonly string? m_targetId;

    private ManagedInputEvaluation(
        IReadOnlyDictionary<string, string> properties,
        CoordinatedFileLease? ownership,
        BuildCachePublisher? cache,
        string directory,
        string? targetId
    ) {
        this.properties = properties;
        m_ownership = ownership;
        m_cache = cache;
        m_directory = directory;
        m_targetId = targetId;
    }

    /// <inheritdoc />
    public void Dispose() => m_ownership?.Dispose();

    internal IReadOnlyDictionary<string, string> properties { get; }

    internal static async ValueTask<ManagedInputEvaluation> OpenAsync(
        NativeBuildContext context,
        DotNetSdkDescriptor sdk,
        string project,
        IReadOnlyDictionary<string, string> properties,
        CancellationToken cancellation
    ) {
        // An explicitly supplied SDK layout remains the caller's owned concurrency boundary.
        if (properties.Keys.Any(static name => name.Equals("ArtifactsPath", StringComparison.OrdinalIgnoreCase)))
            return new(properties, null, null, string.Empty, null);
        string identity = NativeBuildFingerprint.Create(new[] { "managed-input-layout", context.engineRoot,
            project, sdk.hostPath, sdk.cliPath, sdk.sdkIdentity }
            .Concat(properties.OrderBy(static pair => pair.Key, StringComparer.OrdinalIgnoreCase)
                .Select(static pair => pair.Key + "=" + pair.Value)), []);
        string root = Path.Combine(context.engineRoot, "artifacts", "managed", "evaluations");
        string directory = Path.Combine(root, identity);
        var cache = new BuildCachePublisher(root, BuildCacheArtifactRole.ReusableWorkspace);
        BuildCacheOwnerCatalog.Register(context.engineRoot, root, "managed-evaluations", cache.role, cancellation);
        using FileLease activity = await cache.AcquireUseAsync(cancellation).ConfigureAwait(false);
        CoordinatedFileLease ownership = await CoordinatedFileLease.AcquireAsync(Path.Combine(root, "owner.lock"),
            directory + ".lock", Timeout.InfiniteTimeSpan, cancellationToken: cancellation).ConfigureAwait(false);
        try
        {
            string? target = properties.TryGetValue("InnoNativeTarget", out string? selected) ? selected : context.toolchain?.targetId;
            cache.BeginWorkingDirectory(directory, directory + ".lock", target, cancellation);
            var isolated = properties.ToDictionary(static pair => pair.Key, static pair => pair.Value, StringComparer.OrdinalIgnoreCase);
            isolated.Add("ArtifactsPath", directory);
            return new(new ReadOnlyDictionary<string, string>(isolated), ownership, cache, directory, target);
        }
        catch
        {
            ownership.Dispose();
            throw;
        }
    }

    internal void Complete(CancellationToken cancellation)
        => m_cache?.Record(m_directory, m_directory + ".lock", [], null, m_targetId, cancellation);
}
