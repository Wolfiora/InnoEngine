using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Describes a named block output's GPU destination.
/// </summary>
public sealed class ShaderIrStageOutput
{
    /// <summary>
    /// Creates an immutable stage output binding.
    /// </summary>
    /// <param name="id">
    /// The exact name in the block's output map.
    /// </param>
    /// <param name="kind">
    /// GPU output category.
    /// </param>
    /// <param name="semantic">
    /// Varying semantic matched by the next stage; otherwise empty.
    /// </param>
    /// <param name="location">
    /// Varying index or color attachment index.
    /// </param>
    public ShaderIrStageOutput(
        string id,
        ShaderIrOutputKind kind,
        string semantic = "",
        int location = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentOutOfRangeException.ThrowIfNegative(location);
        if (!Enum.IsDefined(kind))
        {
            throw new ArgumentException("A stage output requires a defined destination kind.", nameof(kind));
        }
        if (kind == ShaderIrOutputKind.Varying)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(semantic);
        }
        this.id = id;
        this.kind = kind;
        this.semantic = semantic;
        this.location = location;
    }

    /// <summary>
    /// Gets the exact block output identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the GPU output category.
    /// </summary>
    public ShaderIrOutputKind kind { get; }

    /// <summary>
    /// Gets the varying semantic, not native assignment syntax.
    /// </summary>
    public string semantic { get; }

    /// <summary>
    /// Gets the target-assigned varying or attachment index.
    /// </summary>
    public int location { get; }
}
