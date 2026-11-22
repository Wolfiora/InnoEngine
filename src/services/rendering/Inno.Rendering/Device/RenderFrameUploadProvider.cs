namespace Inno.Rendering;

/// <summary>
/// Provides protected slice construction for replaceable frame-upload service implementations.
/// </summary>
public abstract class RenderFrameUploadProvider
{
    /// <summary>
    /// Creates a frame-scoped slice over a provider-owned persistent upload buffer.
    /// </summary>
    /// <param name="buffer">
    /// The persistent upload page backing the slice.
    /// </param>
    /// <param name="firstElement">
    /// The zero-based first uploaded element.
    /// </param>
    /// <param name="elementCount">
    /// The positive number of uploaded elements.
    /// </param>
    /// <param name="usage">
    /// The GPU uses supported by the uploaded range.
    /// </param>
    /// <param name="frameIndex">
    /// The render frame during which the slice remains valid.
    /// </param>
    /// <returns>
    /// An opaque frame-scoped buffer slice.
    /// </returns>
    protected static RenderBufferSlice CreateBufferSlice(
        PersistentBufferHandle buffer,
        int firstElement,
        int elementCount,
        RenderBufferUsage usage,
        ulong frameIndex
    ) => new(buffer, firstElement, elementCount, usage, frameIndex);
}
