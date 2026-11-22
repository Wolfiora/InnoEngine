namespace Inno.Rendering;

/// <summary>
/// Identifies one range in a frame upload page without exposing its persistent backing buffer.
/// </summary>
public readonly record struct RenderBufferSlice
{
    internal RenderBufferSlice(
        PersistentBufferHandle buffer,
        int firstElement,
        int elementCount,
        RenderBufferUsage usage,
        ulong frameIndex
    ) {
        this.buffer = buffer;
        this.firstElement = firstElement;
        this.elementCount = elementCount;
        this.usage = usage;
        this.frameIndex = frameIndex;
    }

    internal PersistentBufferHandle buffer { get; }
    internal ulong frameIndex { get; }

    /// <summary>
    /// Gets the first uploaded element in the backing frame page.
    /// </summary>
    public int firstElement { get; }

    /// <summary>
    /// Gets the number of uploaded elements.
    /// </summary>
    public int elementCount { get; }

    /// <summary>
    /// Gets permitted GPU uses for this slice.
    /// </summary>
    public RenderBufferUsage usage { get; }

    /// <summary>
    /// Gets whether this slice was produced by a frame upload service.
    /// </summary>
    public bool isValid => buffer.isValid && elementCount > 0;
}
