using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Inno.Rendering.Shaders;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Carries validated inputs and outputs for one shader compilation stage.
/// </summary>
public sealed partial class ShaderCompiler
{
    private readonly object m_stageCacheLock = new();
    private readonly Dictionary<(string stage, string target, GraphicsCapabilities capabilities), ShaderStageToolResult> m_stageCache = [];
    private long m_stageCacheBytes;

    /// <summary>
    /// Compiles a typed stage through the configured adapter without an intermediate complete-source authoring asset.
    /// </summary>
    /// <param name="stage">
    /// Typed stage and frozen function modules.
    /// </param>
    /// <param name="target">
    /// Configured target capability/profile snapshot.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation before publishing the candidate.
    /// </param>
    /// <returns>
    /// Compiled stage bytes, exact generated binding names and structured diagnostics.
    /// </returns>
    public async ValueTask<ShaderStageToolResult> CompileAsync(
        ShaderIrStage stage,
        ShaderCompileTarget target,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(stage);
        ArgumentNullException.ThrowIfNull(target);
        cancellationToken.ThrowIfCancellationRequested();
        (string contentHash, string key, GraphicsCapabilities capabilities) key = (stage.contentHash, target.key, target.capabilities);
        lock (m_stageCacheLock)
        {
            if (m_stageCache.TryGetValue(key, out ShaderStageToolResult? previous))
            {
                return previous;
            }
        }
        ShaderStageToolResult result = await m_toolchain.CompileAsync(
            new ShaderStageToolRequest(stage, target),
            cancellationToken
        ).ConfigureAwait(false);
        cancellationToken.ThrowIfCancellationRequested();
        // The bounded cache contains immutable backend results only, never IR, providers or source assets.
        // Failed and cancelled requests are not cached, and layout-only graph edits reuse the semantic key.
        if (result.succeeded && result.bytes.Length <= 32 * 1024 * 1024)
        {
            lock (m_stageCacheLock)
            {
                if (m_stageCache.Count >= 256 || m_stageCacheBytes + result.bytes.Length > 32 * 1024 * 1024)
                {
                    m_stageCache.Clear();
                    m_stageCacheBytes = 0;
                }
                if (m_stageCache.TryAdd(key, result))
                {
                    m_stageCacheBytes += result.bytes.Length;
                }
            }
        }
        return result;
    }
}
