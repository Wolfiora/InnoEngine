using Inno.Native.Bgfx;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

internal sealed class BgfxBufferResource
{
    private BgfxBufferResource(
        RenderBufferDescriptor descriptor,
        RenderVertexLayout? vertexLayout,
        RenderIndexFormat indexFormat,
        BgfxBufferKind kind,
        ushort nativeIndex
    ) {
        this.descriptor = descriptor;
        this.vertexLayout = vertexLayout;
        this.indexFormat = indexFormat;
        this.kind = kind;
        this.nativeIndex = nativeIndex;
    }

    /// <summary>
    /// Gets the backend-neutral descriptor used to validate and recreate this resource.
    /// </summary>
    public RenderBufferDescriptor descriptor { get; }

    /// <summary>
    /// Gets the optional vertex layout required by this resource or pipeline.
    /// </summary>
    public RenderVertexLayout? vertexLayout { get; }

    /// <summary>
    /// Gets the element width used by this index buffer.
    /// </summary>
    public RenderIndexFormat indexFormat { get; }

    /// <summary>
    /// Gets the operation kind that determines how this value is interpreted.
    /// </summary>
    public BgfxBufferKind kind { get; }

    /// <summary>
    /// Gets the BGFX dynamic-buffer index used by deferred destruction.
    /// </summary>
    public ushort nativeIndex { get; }

    /// <summary>
    /// Creates the target representation from the supplied vertex value.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by from vertex; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="layout">
    /// The layout consumed by from vertex; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <returns>
    /// The validated bgfx buffer resource that represents the completed operation.
    /// </returns>
    public static BgfxBufferResource FromVertex(
        RenderBufferDescriptor descriptor,
        RenderVertexLayout? layout,
        bgfx.VertexBufferHandle handle
    ) => new(descriptor, layout, RenderIndexFormat.UInt32, BgfxBufferKind.Vertex, handle.idx);

    /// <summary>
    /// Creates the target representation from the supplied index value.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by from index; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="format">
    /// The format consumed by from index; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <returns>
    /// The validated bgfx buffer resource that represents the completed operation.
    /// </returns>
    public static BgfxBufferResource FromIndex(
        RenderBufferDescriptor descriptor,
        RenderIndexFormat format,
        bgfx.IndexBufferHandle handle
    ) => new(descriptor, null, format, BgfxBufferKind.Index, handle.idx);

    /// <summary>
    /// Creates the target representation from the supplied dynamic vertex value.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by from dynamic vertex; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="layout">
    /// The layout consumed by from dynamic vertex; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <returns>
    /// The validated bgfx buffer resource that represents the completed operation.
    /// </returns>
    public static BgfxBufferResource FromDynamicVertex(
        RenderBufferDescriptor descriptor,
        RenderVertexLayout? layout,
        bgfx.DynamicVertexBufferHandle handle
    ) => new(descriptor, layout, RenderIndexFormat.UInt32, BgfxBufferKind.DynamicVertex, handle.idx);

    /// <summary>
    /// Creates the target representation from the supplied dynamic index value.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by from dynamic index; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="format">
    /// The format consumed by from dynamic index; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <returns>
    /// The validated bgfx buffer resource that represents the completed operation.
    /// </returns>
    public static BgfxBufferResource FromDynamicIndex(
        RenderBufferDescriptor descriptor,
        RenderIndexFormat format,
        bgfx.DynamicIndexBufferHandle handle
    ) => new(descriptor, null, format, BgfxBufferKind.DynamicIndex, handle.idx);

    /// <summary>
    /// Creates the target representation from the supplied indirect value.
    /// </summary>
    /// <param name="descriptor">
    /// The descriptor consumed by from indirect; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="handle">
    /// The opaque handle validated by this operation.
    /// </param>
    /// <returns>
    /// The validated bgfx buffer resource that represents the completed operation.
    /// </returns>
    public static BgfxBufferResource FromIndirect(
        RenderBufferDescriptor descriptor,
        bgfx.IndirectBufferHandle handle
    ) => new(descriptor, null, RenderIndexFormat.UInt32, BgfxBufferKind.Indirect, handle.idx);
}
