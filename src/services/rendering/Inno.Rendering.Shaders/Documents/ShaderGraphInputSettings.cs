using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores a stage input node's logical interface without native names or GPU handles.
/// </summary>
public sealed class ShaderGraphInputSettings : ISerializable
{
    /// <summary>
    /// Gets or sets the stable logical binding identity.
    /// </summary>
    [SerializableProperty]
    public string id { get; set; } = "input";

    /// <summary>
    /// Gets or sets the complete neutral type descriptor.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphType type { get; set; } = new();

    /// <summary>
    /// Gets or sets the stage input category.
    /// </summary>
    [SerializableProperty]
    public ShaderIrInputKind kind { get; set; }

    /// <summary>
    /// Gets or sets the target semantic, not a native expression.
    /// </summary>
    [SerializableProperty]
    public string semantic { get; set; } = "position";

    /// <summary>
    /// Gets or sets the interface index.
    /// </summary>
    [SerializableProperty]
    public int location { get; set; }

    /// <summary>
    /// Validates the persisted descriptor and freezes its stage binding.
    /// </summary>
    /// <returns>
    /// An immutable backend-neutral input binding.
    /// </returns>
    public ShaderIrStageInput CreateBinding() => new(id, type.CreateType(), kind, semantic, location);
}
