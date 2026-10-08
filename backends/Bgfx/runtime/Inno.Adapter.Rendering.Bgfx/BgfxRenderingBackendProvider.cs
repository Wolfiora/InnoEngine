using System;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Supplies BGFX rendering devices and compatible layer composition programs.
/// </summary>
public sealed class BgfxRenderingBackendProvider : RenderingBackendProvider
{
    /// <summary>
    /// Creates a composition-owned registration for the bundled implementation.
    /// </summary>
    public BgfxRenderingBackendProvider() : base(RenderingBackendId.bgfx) { }

    /// <inheritdoc />
    public override IRenderDevice CreateDevice(RenderingBackendOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new BgfxDevice(new BgfxDeviceOptions
        {
            window = options.window,
            preferredBackend = options.preferredGraphicsApi,
            verticalSync = options.verticalSync,
            sRgbBackbuffer = options.sRgbBackbuffer,
            forceSingleThreaded = options.forceSingleThreaded
        });
    }

    /// <inheritdoc />
    public override IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider()
        => new BgfxCompositionProgramProvider();
}
