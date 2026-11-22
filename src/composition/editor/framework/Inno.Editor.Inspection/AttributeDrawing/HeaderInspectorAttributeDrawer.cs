using Inno.Editor.Annotations;

using EditorUI = Inno.Editor.ImGui.EditorUISession;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(HeaderAttribute))]
internal sealed class HeaderInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Renders the section heading before its annotated property.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void DrawBefore(InspectorAttributeDrawContext context)
    {
        HeaderAttribute header = (HeaderAttribute)context.attribute;
        EditorUI.current.widgets.SectionHeader(header.title, header.description);
    }
}
