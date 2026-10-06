using System;

namespace Inno.Rendering;

/// <summary>
/// Identifies whether a request renders to the main swapchain or an offscreen target.
/// </summary>
public enum RenderTargetKind
{
    /// <summary>
    /// Main application window swapchain.
    /// </summary>
    Backbuffer,
    /// <summary>
    /// Persistent offscreen render texture.
    /// </summary>
    Texture
}

/// <summary>
/// Selects one render destination without exposing a swapchain or framebuffer handle.
/// </summary>
public readonly record struct RenderTarget
{
    private RenderTarget(
        RenderTargetKind kind,
        RenderTexture? texture
    ) {
        this.kind = kind;
        this.texture = texture;
    }

    /// <summary>
    /// Gets a target representing the main application backbuffer.
    /// </summary>
    public static RenderTarget backbuffer { get; } = new(RenderTargetKind.Backbuffer, null);

    /// <summary>
    /// Gets the target kind.
    /// </summary>
    public RenderTargetKind kind { get; }

    /// <summary>
    /// Gets the offscreen texture, or <see langword="null"/> for the backbuffer.
    /// </summary>
    public RenderTexture? texture { get; }

    /// <summary>
    /// Creates an offscreen render target.
    /// </summary>
    /// <param name="texture">
    /// Persistent offscreen texture description.
    /// </param>
    /// <returns>
    /// A target referencing <paramref name="texture"/>.
    /// </returns>
    public static RenderTarget FromTexture(RenderTexture texture)
    {
        ArgumentNullException.ThrowIfNull(texture);
        return new RenderTarget(RenderTargetKind.Texture, texture);
    }
}

