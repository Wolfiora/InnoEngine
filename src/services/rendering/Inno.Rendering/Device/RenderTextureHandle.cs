namespace Inno.Rendering;

/// <summary>
/// Identifies a frame-scoped render-graph texture.
/// </summary>
public readonly record struct RenderTextureHandle
{
    internal RenderTextureHandle(
        int index,
        uint generation,
        ulong allocationID
    ) {
        this.index = index;
        this.generation = generation;
        this.allocationID = allocationID;
    }

    internal int index { get; }
    internal uint generation { get; }
    internal ulong allocationID { get; }

    /// <summary>
    /// Gets whether the handle was created by a render graph.
    /// </summary>
    public bool isValid => index >= 0 && generation != 0 && allocationID != 0;
}
