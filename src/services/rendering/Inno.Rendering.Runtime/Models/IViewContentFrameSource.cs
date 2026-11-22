using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Completes frame-local input after every output has routed its views.
/// </summary>
[StableTypeID("9242bf1f-a6dc-555f-ab73-494c7a7acc70")]
public interface IViewContentFrameSource
{
    /// <summary>
    /// Advances retained content once after all output views have supplied input.
    /// </summary>
    /// <param name="frameIndex">
    /// The shared render frame whose input routing has completed.
    /// </param>
    void CompleteFrame(ulong frameIndex);
}
