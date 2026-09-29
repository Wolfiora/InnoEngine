using Inno.Rendering;

namespace Inno.Adapter.Rendering;

/// <summary>
/// Supplies one rendering implementation registered by a composition owner.
/// </summary>
public abstract class RenderingBackendProvider
{
    /// <summary>
    /// Gets the stable implementation identity paired with its authoring tools.
    /// </summary>
    public abstract RenderingBackendId id { get; }

    /// <summary>
    /// Creates a caller-owned device using backend-neutral surface options.
    /// </summary>
    /// <param name="options">
    /// Validated surface and device creation options.
    /// </param>
    /// <returns>
    /// A new device owned by the caller.
    /// </returns>
    public abstract IRenderDevice CreateDevice(RenderingBackendOptions options);

    /// <summary>
    /// Creates the backend-owned program provider used when the host composites render layers.
    /// </summary>
    /// <returns>
    /// A provider compatible with devices created by this rendering implementation.
    /// </returns>
    public abstract IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider();
}
