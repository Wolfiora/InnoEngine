using Inno.Rendering;
using System;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Accepts rendering-model-neutral requests without exposing runtime or backend ownership.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("53812496-61bc-5128-8cf9-2865a9319980")]
public interface IRenderRequestSink
{
    /// <summary>
    /// Queues one immutable view request for the current or next render frame.
    /// </summary>
    /// <param name="request">
    /// Immutable pipeline-defined request.
    /// </param>
    void Submit(RenderRequest request);
}

