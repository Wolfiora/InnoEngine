using Inno.Editor.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUiSession;

namespace Inno.Editor.Inspection;

internal sealed class UnsupportedPropertyDrawer : IPropertyDrawer
{
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

    internal static UnsupportedPropertyDrawer instance { get; } = new();
}
