using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(RangeAttribute))]
internal sealed class RangeInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Applies inclusive numeric slider bounds to the annotated property.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void Update(InspectorAttributeDrawContext context)
    {
        RangeAttribute range = (RangeAttribute)context.attribute;
        context.minimum = range.minimum;
        context.maximum = range.maximum;
    }
}
