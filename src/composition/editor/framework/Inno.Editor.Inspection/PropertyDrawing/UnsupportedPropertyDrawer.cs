using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.ImGui;

namespace Inno.Editor.Inspection;
internal sealed class UnsupportedPropertyDrawer : IPropertyDrawer
{
    internal static UnsupportedPropertyDrawer instance { get; } = new();

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        object? value = context.GetValue();
        EditorUI.current.widgets.ColoredText(
            EditorPalette.warning,
            value is null ? $"Unsupported {context.propertyType.Name} (null)" : $"Unsupported {context.propertyType.Name}: {value}"
        );
    }
}
