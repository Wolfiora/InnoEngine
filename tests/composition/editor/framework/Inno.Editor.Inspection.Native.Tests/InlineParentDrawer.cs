using Inno.Editor.Inspection;

namespace Inno.Editor.Inspection.Native.Tests;

[PropertyDrawer(typeof(InlineParent), priority: 10_000)]
internal sealed class InlineParentDrawer : IPropertyDrawer
{
    internal static bool drewAfterFailure;

    public void Draw(PropertyDrawContext context)
    {
        InlineParent parent = (InlineParent)context.GetValue()!;
        context.DrawInlineChild("child", typeof(InlineFailure), () => parent.child, static _ => {}, readOnly: true);
        drewAfterFailure = true;
    }
}
