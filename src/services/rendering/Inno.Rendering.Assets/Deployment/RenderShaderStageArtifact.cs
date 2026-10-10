using Inno.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace Inno.Rendering.Assets;

/// <summary>
/// Stores one immutable target shader stage without retaining authoring source or compiler state.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("617ca942-9bab-528c-8052-e9cfb2ae0df5")]
public sealed class RenderShaderStageArtifact
{
    private readonly byte[] m_bytes;

    /// <summary>
    /// Creates a deployed shader stage.
    /// </summary>
    /// <param name="stage">
    /// The single programmable stage represented by the target binary.
    /// </param>
    /// <param name="bytes">
    /// The non-empty target backend program bytes.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="stage"/> is not a single stage or <paramref name="bytes"/> is empty.
    /// </exception>
    public RenderShaderStageArtifact(
        ShaderStage stage,
        ReadOnlySpan<byte> bytes
    ) {
        if (stage is not ShaderStage.Vertex and not ShaderStage.Fragment and not ShaderStage.Compute)
            throw new ArgumentException("A deployed shader stage must identify one programmable stage.", nameof(stage));
        if (bytes.IsEmpty)
            throw new ArgumentException("A deployed shader stage cannot be empty.", nameof(bytes));
        this.stage = stage;
        m_bytes = bytes.ToArray();
    }

    /// <summary>
    /// Gets the programmable stage represented by this artifact.
    /// </summary>
    public ShaderStage stage { get; }

    /// <summary>
    /// Gets the immutable target backend program bytes.
    /// </summary>
    public ReadOnlyMemory<byte> bytes => m_bytes;
}

