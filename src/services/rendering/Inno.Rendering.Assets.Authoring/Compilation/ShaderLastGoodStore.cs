using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Atomically preserves last-good CPU shader artifacts by asset, target and variant.
/// </summary>
[StableTypeID("b238055f-335a-528f-bdee-cf0044f8e75e")]
public sealed class ShaderLastGoodStore
{
    private readonly object m_gate = new();
    private readonly Dictionary<ArtifactKey, CompiledShaderArtifact> m_artifacts = [];

    /// <summary>
    /// Commits a complete candidate or returns the current last-good artifact after failure.
    /// </summary>
    /// <param name="shaderID">
    /// Persistent shader asset identity.
    /// </param>
    /// <param name="targetKey">
    /// Stable target cache key.
    /// </param>
    /// <param name="variant">
    /// Static keyword variant.
    /// </param>
    /// <param name="candidate">
    /// Candidate compilation result.
    /// </param>
    /// <returns>
    /// The artifact safe to use for rendering.
    /// </returns>
    public ShaderArtifactSelection Select(
        Guid shaderID,
        string targetKey,
        RenderShaderVariant variant,
        ShaderCompilationResult candidate
    ) {
        if (shaderID == Guid.Empty)
        {
            throw new ArgumentException("A persistent shader identity is required.", nameof(shaderID));
        }

        ArgumentException.ThrowIfNullOrWhiteSpace(targetKey);
        ArgumentNullException.ThrowIfNull(candidate);
        var key = new ArtifactKey(shaderID, targetKey, variant.value);
        lock (m_gate)
        {
            if (candidate.succeeded)
            {
                m_artifacts[key] = candidate.artifact!;
                return new ShaderArtifactSelection(
                    candidate.artifact,
                    candidateSucceeded: true,
                    usingLastGood: false,
                    candidate.diagnostics
                );
            }

            m_artifacts.TryGetValue(key, out CompiledShaderArtifact? lastGood);
            return new ShaderArtifactSelection(
                lastGood,
                candidateSucceeded: false,
                usingLastGood: lastGood is not null,
                candidate.diagnostics
            );
        }
    }

    /// <summary>
    /// Removes all CPU artifacts associated with one persistent shader.
    /// </summary>
    /// <param name="shaderID">
    /// Persistent shader asset identity.
    /// </param>
    /// <returns>
    /// The number of removed variants.
    /// </returns>
    public int Remove(Guid shaderID)
    {
        if (shaderID == Guid.Empty)
        {
            return 0;
        }

        lock (m_gate)
        {
            int removed = 0;
            foreach (ArtifactKey key in new List<ArtifactKey>(m_artifacts.Keys))
            {
                if (key.shaderID == shaderID && m_artifacts.Remove(key))
                {
                    removed++;
                }
            }

            return removed;
        }
    }

    private readonly record struct ArtifactKey(
        Guid shaderID,
        string targetKey,
        string variantKey
    );
}
