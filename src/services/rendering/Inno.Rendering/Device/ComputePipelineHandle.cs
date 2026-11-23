namespace Inno.Rendering;

/// <summary>
/// Identifies a backend-neutral compute pipeline object.
/// </summary>
public readonly record struct ComputePipelineHandle
{
    internal ComputePipelineHandle(
        ulong value,
        uint deviceGeneration
    ) {
        this.value = value;
        this.deviceGeneration = deviceGeneration;
    }

    internal ulong value { get; }
    internal uint deviceGeneration { get; }

    /// <summary>
    /// Gets whether the handle identifies a compute pipeline.
    /// </summary>
    public bool isValid => value != 0 && deviceGeneration != 0;
}
