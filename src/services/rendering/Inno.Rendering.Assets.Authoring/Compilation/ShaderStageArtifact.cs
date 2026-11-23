using System;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Stores one immutable target stage binary.
/// </summary>
[StableTypeID("7de46626-7dd7-5112-b098-ef3eb53a0cda")]
public sealed class ShaderStageArtifact
{
    private readonly byte[] m_bytes;

    /// <summary>
    /// Creates a stage artifact.
    /// </summary>
    /// <param name="stage">
    /// Single compiled shader stage.
    /// </param>
    /// <param name="bytes">
    /// Target binary bytes.
    /// </param>
    /// <param name="sourceLocation">
    /// Original source mapping.
    /// </param>
    public ShaderStageArtifact(
        ShaderStage stage,
        ReadOnlySpan<byte> bytes,
        ShaderSourceLocation sourceLocation
    ) {
        if (bytes.IsEmpty)
        {
            throw new ArgumentException("A compiled shader stage cannot be empty.", nameof(bytes));
        }

        this.stage = stage;
        m_bytes = bytes.ToArray();
        this.sourceLocation = sourceLocation;
    }

    /// <summary>
    /// Gets the compiled shader stage.
    /// </summary>
    public ShaderStage stage { get; }

    /// <summary>
    /// Gets immutable target binary bytes.
    /// </summary>
    public ReadOnlyMemory<byte> bytes => m_bytes;

    /// <summary>
    /// Gets the original source mapping.
    /// </summary>
    public ShaderSourceLocation sourceLocation { get; }
}
