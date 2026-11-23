using System.Numerics;
using Inno.Editor.Annotations;
using Inno.Editor.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUISession;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(HelpBoxAttribute))]
internal sealed class HelpBoxInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Renders a conditional diagnostic message before its annotated property.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void DrawBefore(InspectorAttributeDrawContext context)
    {
        HelpBoxAttribute help = (HelpBoxAttribute)context.attribute;
        if (!string.IsNullOrWhiteSpace(help.conditionMember)
            && !InspectorConditionEvaluator.Evaluate(
                context.GetSiblingValue(help.conditionMember),
                help.condition,
                help.expectedValue
            )
        ) {
            return;
        }

        Vector4 color = help.messageType switch
        {
            InspectorMessageType.Warning => EditorPalette.warning,
            InspectorMessageType.Error => EditorPalette.error,
            _ => new Vector4(0.42f, 0.66f, 0.88f, EditorPalette.C_OPACITY_OPAQUE)
        };
        string icon = help.messageType switch
        {
            InspectorMessageType.Warning => ImGuiIcon.C_TRIANGLE_EXCLAMATION,
            InspectorMessageType.Error => ImGuiIcon.C_CIRCLE_XMARK,
            _ => ImGuiIcon.C_CIRCLE_INFO
        };
        EditorUI.current.widgets.HelpBox(help.text, icon, color);
    }
}
