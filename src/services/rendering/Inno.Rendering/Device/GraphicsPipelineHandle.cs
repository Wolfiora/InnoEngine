namespace Inno.Rendering;

/// <summary>
/// Identifies a backend-neutral graphics pipeline object.
/// </summary>
public readonly record struct GraphicsPipelineHandle
{
    internal GraphicsPipelineHandle(
        ulong value,
        uint deviceGeneration
    ) {
        this.value = value;
        this.deviceGeneration = deviceGeneration;
    }

    internal ulong value { get; }
    internal uint deviceGeneration { get; }

    /// <summary>
    /// Gets whether the handle identifies a graphics pipeline.
    /// </summary>
    public bool isValid => value != 0 && deviceGeneration != 0;
}
