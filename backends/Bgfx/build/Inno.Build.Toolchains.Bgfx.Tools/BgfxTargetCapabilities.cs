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
    /// <param name="backend">
    /// The renderer whose shader dialect is being compiled.
    /// </param>
    /// <returns>
    /// The immutable offline validation profile for the selected target.
    /// </returns>
    public static GraphicsCapabilities Create(
        GraphicsApi backend
    ) {
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

}
