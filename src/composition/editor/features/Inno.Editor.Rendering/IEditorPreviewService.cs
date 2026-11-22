using System.Numerics;
using Inno.Rendering.Assets;

namespace Inno.Editor.Rendering;

/// <summary>
/// Provides one shared preview texture bridge for browsers, inspectors, and document canvases.
/// </summary>
public interface IEditorPreviewService
{
    /// <summary>
    /// Gets the active rendering-device generation.
    /// </summary>
    uint deviceGeneration { get; }

    /// <summary>
    /// Submits an isolated rendering composition through the shared offscreen viewport bridge.
    /// </summary>
    /// <param name="composition">
    /// Invocation-local layers; the host retains no contributor instances after the frame.
    /// </param>
    /// <param name="handle">
    /// Receives a current-generation preview once its target is resident.
    /// </param>
    /// <returns>
    /// True when the completed previous output can be drawn; false during initial allocation or resize.
    /// </returns>
    bool TryRender(
        EditorViewportComposition composition,
        out EditorPreviewHandle handle
    );

    /// <summary>
    /// Releases an offscreen preview, including one which has not produced its first handle.
    /// </summary>
    /// <param name="viewportID">
    /// The same consumer-qualified identity submitted in the composition.
    /// </param>
    void ReleaseRendered(string viewportID);

    /// <summary>
    /// Tries to resolve a standalone texture preview without blocking target compilation.
    /// </summary>
    /// <param name="texture">
    /// Texture asset to preview.
    /// </param>
    /// <param name="handle">
    /// Receives a generation-scoped handle when ready.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the preview is ready.
    /// </returns>
    bool TryGetTexture(
        TextureAsset texture,
        out EditorPreviewHandle handle
    );

    /// <summary>
    /// Tries to resolve a named texture artifact preview without blocking target compilation.
    /// </summary>
    /// <param name="texture">
    /// Stable named texture artifact reference.
    /// </param>
    /// <param name="pixelWidth">
    /// Positive source width.
    /// </param>
    /// <param name="pixelHeight">
    /// Positive source height.
    /// </param>
    /// <param name="handle">
    /// Receives a generation-scoped handle when ready.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the preview is ready.
    /// </returns>
    bool TryGetTextureArtifact(
        RenderTextureArtifactReference texture,
        int pixelWidth,
        int pixelHeight,
        out EditorPreviewHandle handle
    );

    /// <summary>
    /// Draws one current-generation preview into the active presentation surface.
    /// </summary>
    /// <param name="handle">
    /// Preview handle returned by this service.
    /// </param>
    /// <param name="logicalSize">
    /// Positive destination size in logical pixels.
    /// </param>
    void Draw(
        EditorPreviewHandle handle,
        Vector2 logicalSize
    );

    /// <summary>
    /// Releases one cached preview registration.
    /// </summary>
    /// <param name="handle">
    /// Preview handle returned by this service.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the handle was active and released.
    /// </returns>
    bool Release(EditorPreviewHandle handle);

    /// <summary>
    /// Releases every cached preview registration.
    /// </summary>
    void ReleaseAll();
}
