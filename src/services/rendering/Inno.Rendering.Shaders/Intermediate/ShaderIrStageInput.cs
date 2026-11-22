using System;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Describes a stage input without embedding native declarations or expressions.
/// </summary>
public sealed class ShaderIrStageInput
{
    /// <summary>
    /// Creates an immutable stage input binding.
    /// </summary>
    /// <param name="id">
    /// Stable binding identity, also used by the block's named Input instruction.
    /// </param>
    /// <param name="type">
    /// Complete non-void type.
    /// </param>
    /// <param name="kind">
    /// Source of the GPU value.
    /// </param>
    /// <param name="semantic">
    /// Target semantic, such as position, texcoord, color, instance-data or fragment-coordinate.
    /// </param>
    /// <param name="location">
    /// Attribute/semantic index or assigned texture slot, never a native handle.
    /// </param>
    public ShaderIrStageInput(
        string id,
        ShaderSourceType type,
        ShaderIrInputKind kind,
        string semantic = "",
        int location = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(type);
        ArgumentOutOfRangeException.ThrowIfNegative(location);
        if (type.id == "void" || !Enum.IsDefined(kind))
        {
            throw new ArgumentException("A stage input requires a value type and defined interface kind.");
        }
        if (kind == ShaderIrInputKind.SampledTexture
            && (type.fields.Count != 0 || type.elementType is not null || type.storage is not null)
        ) {
            throw new ArgumentException("A sampled texture binding requires an opaque atomic resource type.", nameof(type));
        }
        if (kind is ShaderIrInputKind.VertexAttribute or ShaderIrInputKind.Varying or ShaderIrInputKind.Builtin)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(semantic);
        }
        this.id = id;
        this.type = type;
        this.kind = kind;
        this.semantic = semantic;
        this.location = location;
    }

    /// <summary>
    /// Gets the logical binding identity.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the complete typed value received by the stage.
    /// </summary>
    public ShaderSourceType type { get; }

    /// <summary>
    /// Gets the GPU input category.
    /// </summary>
    public ShaderIrInputKind kind { get; }

    /// <summary>
    /// Gets the target semantic; an adapter must reject unsupported semantics.
    /// </summary>
    public string semantic { get; }

    /// <summary>
    /// Gets the target-assigned interface location or sampled-texture slot.
    /// </summary>
    public int location { get; }
}
