using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Builds view requests from host output sessions without owning a host window.
/// </summary>
[StableTypeID("52f335d4-64e4-5a11-96f3-c52e44295830")]
public interface IRenderModel : IDisposable
{
    /// <summary>
    /// Determines whether this model accepts the session's content.
    /// </summary>
    /// <param name="session">
    /// Host output session.
    /// </param>
    /// <returns>
    /// Whether the model can render the selected content.
    /// </returns>
    bool CanRender(RenderOutputSession session);

    /// <summary>
    /// Builds one model output after acceptance.
    /// </summary>
    /// <param name="session">
    /// Host output session.
    /// </param>
    /// <returns>
    /// Prepared frame data and pipeline.
    /// </returns>
    RenderModelOutput Build(RenderOutputSession session);
}
