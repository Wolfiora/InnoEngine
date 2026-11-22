namespace Inno.Rendering;

/// <summary>
/// Describes one compiled raster attachment.
/// </summary>
public sealed class CompiledRenderAttachment
{
    internal CompiledRenderAttachment(RenderAttachment attachment)
    {
        this.texture = attachment.texture;
        this.slot = attachment.slot;
        this.isDepth = attachment.isDepth;
        this.mipLevel = attachment.mipLevel;
        this.arrayLayer = attachment.arrayLayer;
        this.loadAction = attachment.loadAction;
        this.storeAction = attachment.storeAction;
        this.clearColor = attachment.clearColor;
        this.clearDepth = attachment.clearDepth;
        this.clearStencil = attachment.clearStencil;
    }

    /// <summary>
    /// Gets the attached graph texture.
    /// </summary>
    public RenderTextureHandle texture { get; }

    /// <summary>
    /// Gets the zero-based color slot, or zero for a depth attachment.
    /// </summary>
    public int slot { get; }

    /// <summary>
    /// Gets whether this is a depth or depth-stencil attachment.
    /// </summary>
    public bool isDepth { get; }

    /// <summary>
    /// Gets the attached mip level.
    /// </summary>
    public int mipLevel { get; }

    /// <summary>
    /// Gets the attached texture-array layer.
    /// </summary>
    public int arrayLayer { get; }

    /// <summary>
    /// Gets initial content behavior.
    /// </summary>
    public RenderLoadAction loadAction { get; }

    /// <summary>
    /// Gets final content behavior.
    /// </summary>
    public RenderStoreAction storeAction { get; }

    /// <summary>
    /// Gets the linear color clear value.
    /// </summary>
    public RenderClearColor clearColor { get; }

    /// <summary>
    /// Gets the depth clear value.
    /// </summary>
    public float clearDepth { get; }

    /// <summary>
    /// Gets the stencil clear value.
    /// </summary>
    public byte clearStencil { get; }
}
