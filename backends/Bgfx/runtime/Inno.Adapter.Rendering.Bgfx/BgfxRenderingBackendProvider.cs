using System;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Supplies BGFX rendering devices and compatible layer composition programs.
/// </summary>
public sealed class BgfxRenderingBackendProvider : RenderingBackendProvider
{
    private readonly IBgfxSurfaceIntegration m_surfaceIntegration;

    /// <summary>
    /// Registers BGFX with an explicitly selected, borrowed host surface integration.
    /// </summary>
    /// <param name="surfaceIntegration">
    /// Immutable host configuration shared by devices created from this provider.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The integration is null.
    /// </exception>
    public BgfxRenderingBackendProvider(IBgfxSurfaceIntegration surfaceIntegration) : base(RenderingBackendId.bgfx)
    {
        ArgumentNullException.ThrowIfNull(surfaceIntegration);
        m_surfaceIntegration = surfaceIntegration;
    }

    /// <inheritdoc />
    public override IRenderDevice CreateDevice(RenderingBackendOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        return new BgfxDevice(new BgfxDeviceOptions
        {
            window = options.window,
            surfaceIntegration = m_surfaceIntegration,
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
