using Inno.Editor.Annotations;
using Inno.Editor.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(TextAttribute))]
internal sealed class TextInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Renders persistent explanatory text before its annotated property.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void DrawBefore(InspectorAttributeDrawContext context)
    {
        string text = ((TextAttribute)context.attribute).text;
        Gui.PushStyleColor((ImGuiCol.Text), EditorPalette.textDisabled);
        try
        {
            EditorUI.current.widgets.WrappedText(text);
        }
        finally
        {
            Gui.PopStyleColor();
        }
    }
}
