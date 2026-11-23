using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(TooltipAttribute))]
internal sealed class TooltipInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Applies tooltip text to the annotated property row.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void Update(InspectorAttributeDrawContext context) => context.tooltip = ((TooltipAttribute)context.attribute).text;
}
