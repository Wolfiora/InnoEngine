using System.Numerics;
using Inno.Rendering.Runtime;

namespace Inno.Editor.Rendering;

/// <summary>
/// Bridges editor extensions to a host-owned render request sink and opaque texture presenter.
/// </summary>
public interface IEditorRenderingHost
{
    /// <summary>
    /// Gets the active generation's model-independent world-content collector.
    /// </summary>
    IViewContentCollector viewContent { get; }

    /// <summary>
    /// Gets the current output frame index shared by Editor views.
    /// </summary>
    ulong currentFrameIndex { get; }

    /// <summary>
    /// Submits or updates one composed offscreen editor viewport.
    /// </summary>
    /// <param name="composition">
    /// Complete ordered model composition for the current frame.
    /// </param>
    /// <returns>
    /// The current presentation output, which can be warming up for one frame.
    /// </returns>
    EditorViewportOutput Submit(EditorViewportComposition composition);

    /// <summary>
    /// Draws a ready viewport output inside the current ImGui window.
    /// </summary>
    /// <param name="output">
    /// Output returned by <see cref="Submit"/>.
    /// </param>
    /// <param name="logicalSize">
    /// Destination size in logical UI pixels.
    /// </param>
    void Draw(
        EditorViewportOutput output,
        Vector2 logicalSize
    );

    /// <summary>
    /// Releases one viewport and queues its GPU target for frame-safe destruction.
    /// </summary>
    /// <param name="viewportID">
    /// Stable viewport identity.
    /// </param>
    void Release(string viewportID);

    /// <summary>
    /// Releases every viewport owned by this editor host service.
    /// </summary>
    void ReleaseAll();
}
