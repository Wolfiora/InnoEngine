using System;
using Inno.Editor.Inspection;

namespace Inno.Editor.Inspection.Native.Tests;

[PropertyDrawer(typeof(InlineFailure), priority: 10_000)]
internal sealed class InlineFailureDrawer : IPropertyDrawer
{
    public void Draw(PropertyDrawContext context) => throw new InvalidOperationException("Inline drawer failure.");
}
