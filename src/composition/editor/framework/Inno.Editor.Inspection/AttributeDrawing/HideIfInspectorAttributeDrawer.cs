using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(HideIfAttribute))]
internal sealed class HideIfInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Hides the annotated property when its sibling condition succeeds.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void Update(InspectorAttributeDrawContext context)
    {
        HideIfAttribute condition = (HideIfAttribute)context.attribute;
        context.isVisible &= !InspectorConditionEvaluator.Evaluate(
            context.GetSiblingValue(condition.memberName),
            condition.condition,
            condition.expectedValue
        );
    }
}
