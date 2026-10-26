using System.Numerics;
using Inno.Native.ImGui;
using RawImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Presents tables through the active Editor UI context.
/// </summary>
public static partial class ImGui
{
    /// <summary>
    /// Selects a declared table column for subsequent controls.
    /// </summary>
    /// <param name="index">
    /// The zero-based item index.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool TableSetColumnIndex(int index)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.TableSetColumnIndex(index);
    }

    /// <summary>
    /// Sets a table row or cell background through the current table scope.
    /// </summary>
    /// <param name="target">
    /// The table background role.
    /// </param>
    /// <param name="color">
    /// The packed RGBA color or semantic style slot.
    /// </param>
    /// <param name="column">
    /// The target column, or -1 for the current row.
    /// </param>
    public static void TableSetBgColor(
        ImGuiTableBgTarget target,
        uint color,
        int column = -1
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.TableSetBgColor((int)target, color, column);
    }
}
