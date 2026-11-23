using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(InspectorNameAttribute))]
internal sealed class NameInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Replaces the default serialized member label.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void Update(InspectorAttributeDrawContext context) => context.label = ((InspectorNameAttribute)context.attribute).name;
}
