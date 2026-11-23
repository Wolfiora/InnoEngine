using Inno.Editor.Rendering;

namespace Inno.Editor.Shaders;

/// <summary>
/// Builds domain-owned preview geometry and pass inputs without changing a scene or canonical asset.
/// </summary>
public abstract class ShaderPreviewProvider
{
    /// <summary>
    /// Creates one frame-local rendering layer through the ordinary Render Graph.
    /// </summary>
    /// <param name="context">
    /// Detached Material, immutable candidate and isolated diagnostics for this invocation.
    /// </param>
    /// <returns>
    /// A domain-selected pipeline and frame payload; no instance may be retained beyond the submitted frame.
    /// </returns>
    public abstract EditorViewportLayer CreateLayer(ShaderPreviewContext context);
}
