using System;
using System.Linq;
using Inno.Rendering;

namespace Inno.Build.Toolchains.Bgfx.Tools;

/// <summary>
/// Supplies one consistent offline capability profile to all BGFX target compilers.
/// </summary>
public static class BgfxTargetCapabilities
{
    /// <summary>
    /// Creates the limits and format set used to validate one target's shader output.
    /// </summary>
    /// <param name="platform">
    /// The distribution platform whose limits constrain generated artifacts.
    /// </param>
    /// <param name="backend">
    /// The renderer whose shader dialect is being compiled.
    /// </param>
    /// <returns>
    /// The immutable offline validation profile for the selected target.
    /// </returns>
    public static GraphicsCapabilities Create(
        BgfxShaderTargetPlatform platform,
        GraphicsApi backend
    ) {
        if (platform == BgfxShaderTargetPlatform.BrowserWasm)
            return CreateBrowser(backend);

        GraphicsCapability features = Enum.GetValues<GraphicsCapability>()
            .Aggregate(GraphicsCapability.None, static (
                current,
                value
            ) => current | value);
        RenderTextureFormat[] formats = Enum.GetValues<RenderTextureFormat>();
        return new GraphicsCapabilities(
            backend,
            features,
            new GraphicsLimits(256, 8, 16384, 16),
            formats,
            formats,
            formats,
            formats,
            originBottomLeft: backend == GraphicsApi.OpenGL || backend == GraphicsApi.OpenGLES,
            homogeneousDepth: backend == GraphicsApi.OpenGL || backend == GraphicsApi.OpenGLES,
            formats,
            formats,
            formats);
    }

    private static GraphicsCapabilities CreateBrowser(GraphicsApi backend)
    {
        RenderTextureFormat[] colorFormats =
        [
            RenderTextureFormat.R8,
            RenderTextureFormat.RG8,
            RenderTextureFormat.RGBA8,
            RenderTextureFormat.RGBA8Srgb,
            RenderTextureFormat.RGB10A2
        ];
        RenderTextureFormat[] sampledFormats =
        [
            .. colorFormats,
            RenderTextureFormat.Depth24Stencil8
        ];
        GraphicsCapability features = GraphicsCapability.Index32 |
            GraphicsCapability.Instancing |
            GraphicsCapability.Texture2DArray |
            GraphicsCapability.Texture3D |
            GraphicsCapability.FragmentDepth;
        return new GraphicsCapabilities(
            backend,
            features,
            new GraphicsLimits(256, 4, 2048, 0),
            sampledFormats,
            sampledFormats,
            [],
            [],
            originBottomLeft: true,
            homogeneousDepth: true,
            colorFormats,
            colorFormats,
            sampledFormats);
    }
}
