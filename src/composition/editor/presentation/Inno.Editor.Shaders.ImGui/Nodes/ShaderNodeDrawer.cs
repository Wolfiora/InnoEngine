namespace Inno.Editor.Shaders.ImGui;

/// <summary>
/// Provides reloadable Inspector controls; instances must not retain frame contexts or asset objects.
/// </summary>
public abstract class ShaderNodeDrawer
{
    /// <summary>
    /// Draws the selected node's controls using the shared Inspector styling.
    /// </summary>
    /// <param name="context">
    /// Frame-scoped value access and undoable writes.
    /// </param>
    public abstract void Draw(ShaderNodeDrawContext context);
}
