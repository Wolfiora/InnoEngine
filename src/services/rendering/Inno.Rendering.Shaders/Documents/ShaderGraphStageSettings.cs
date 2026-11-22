using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Stores one stage's explicit output interface and compute dimensions inside its output node.
/// </summary>
public sealed class ShaderGraphStageSettings : ISerializable
{
    /// <summary>
    /// Gets or sets the programmable stage.
    /// </summary>
    [SerializableProperty]
    public ShaderStage stage { get; set; } = ShaderStage.Fragment;

    /// <summary>
    /// Gets or sets output ports and their GPU destinations.
    /// </summary>
    [SerializableProperty]
    public ShaderGraphOutput[] outputs { get; set; } = [];

    /// <summary>
    /// Gets or sets compute workgroup width.
    /// </summary>
    [SerializableProperty]
    public int threadsX { get; set; } = 1;

    /// <summary>
    /// Gets or sets compute workgroup height.
    /// </summary>
    [SerializableProperty]
    public int threadsY { get; set; } = 1;

    /// <summary>
    /// Gets or sets compute workgroup depth.
    /// </summary>
    [SerializableProperty]
    public int threadsZ { get; set; } = 1;
}
