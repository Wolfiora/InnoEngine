using Inno.Editor.Annotations;

namespace Inno.Editor.Inspection;

[InspectorAttributeDrawer(typeof(ShowIfAttribute))]
internal sealed class ShowIfInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    /// <summary>
    /// Hides the annotated property unless its sibling condition succeeds.
    /// </summary>
    /// <param name="context">
    /// Active attribute drawing context.
    /// </param>
    public void Update(InspectorAttributeDrawContext context)
    {
        ShowIfAttribute condition = (ShowIfAttribute)context.attribute;
        context.isVisible &= InspectorConditionEvaluator.Evaluate(
            context.GetSiblingValue(condition.memberName),
            condition.condition,
            condition.expectedValue
        );
    }
}
