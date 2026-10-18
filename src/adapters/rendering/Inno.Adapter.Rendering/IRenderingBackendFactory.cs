using System;
using System.Collections.Generic;
using Inno.Rendering;

namespace Inno.Adapter.Rendering;

/// <summary>
/// Creates runtime rendering devices from one backend selection.
/// </summary>
public interface IRenderingBackendFactory
{
    /// <summary>
    /// Gets the exact runtime backend identities available in this composition generation.
    /// </summary>
    IReadOnlyList<RenderingBackendId> supportedBackends { get; }

    /// <summary>
    /// Creates a rendering device for the supplied primary presentation surface.
    /// </summary>
    /// <param name="backend">
    /// Stable rendering backend selected by the composition root.
    /// </param>
    /// <param name="options">
    /// Backend-neutral rendering options.
    /// </param>
    /// <returns>
    /// A caller-owned backend-neutral rendering device.
    /// </returns>
    IRenderDevice CreateDevice(
        RenderingBackendId backend,
        RenderingBackendOptions options
    );

    /// <summary>
    /// Creates the composition program provider owned by the selected rendering backend.
    /// </summary>
    /// <param name="backend">
    /// Stable identity of the selected rendering backend.
    /// </param>
    /// <returns>
    /// A provider whose target programs are compatible with that backend.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// No provider is registered for the requested backend identity.
    /// </exception>
    IRenderLayerCompositionProgramProvider CreateCompositionProgramProvider(RenderingBackendId backend);
}
