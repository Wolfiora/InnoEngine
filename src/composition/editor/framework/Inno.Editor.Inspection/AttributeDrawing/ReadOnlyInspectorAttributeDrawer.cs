using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(InspectorReadOnlyAttribute))]
internal sealed class ReadOnlyInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Disables assignment for the annotated property.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void Update(InspectorAttributeDrawContext context) => context.isReadOnly = true;
}
