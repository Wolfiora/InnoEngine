using System;

namespace Inno.UI;

/// <summary>
/// Publishes one immutable premultiplied RGBA8 texture generation.
/// </summary>
public sealed class UITextureUpdate
{
    private readonly byte[] m_pixels;

    internal UITextureUpdate(
        UITextureHandle texture,
        ulong revision,
        int width,
        int height,
        byte[] pixels
    ) {
        this.texture = texture;
        this.revision = revision;
        this.width = width;
        this.height = height;
        m_pixels = pixels;
    }

    /// <summary>
    /// Gets the generation-scoped texture handle.
    /// </summary>
    public UITextureHandle texture { get; }

    /// <summary>
    /// Gets the monotonically increasing content revision.
    /// </summary>
    public ulong revision { get; }

    /// <summary>
    /// Gets the texture width.
    /// </summary>
    public int width { get; }

    /// <summary>
    /// Gets the texture height.
    /// </summary>
    public int height { get; }

    /// <summary>
    /// Gets immutable tightly packed premultiplied RGBA8 pixels.
    /// </summary>
    public ReadOnlyMemory<byte> pixels => m_pixels;
}
