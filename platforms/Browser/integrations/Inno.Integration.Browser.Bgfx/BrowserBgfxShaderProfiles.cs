using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;

using Inno.Build.Browser;

namespace Inno.Integration.Browser.Bgfx;

/// <summary>
/// Owns explicit Browser shader dialects and offline product capabilities.
/// </summary>
public static class BrowserBgfxShaderProfiles
{
    /// <summary>
    /// Gets the immutable configuration shared by this platform's products and publication module.
    /// </summary>
    public static BgfxShaderTargetProfile target { get; } = new(BrowserBuildModule.target.id.value,
    [
        new(CreateCapabilities(), "asm.js", "300_es", "300_es", "")
    ]);

    private static GraphicsCapabilities CreateCapabilities()
    {
        RenderTextureFormat[] color = [RenderTextureFormat.R8, RenderTextureFormat.RG8,
            RenderTextureFormat.RGBA8, RenderTextureFormat.RGBA8Srgb, RenderTextureFormat.RGB10A2];
        RenderTextureFormat[] sampled = [.. color, RenderTextureFormat.Depth24Stencil8];
        return new(GraphicsApi.OpenGLES, GraphicsCapability.Index32 | GraphicsCapability.Instancing
            | GraphicsCapability.Texture2DArray | GraphicsCapability.Texture3D | GraphicsCapability.FragmentDepth,
            new GraphicsLimits(256, 4, 2048, 0), sampled, sampled, [], [], true, true,
            color, color, sampled);
    }
}
