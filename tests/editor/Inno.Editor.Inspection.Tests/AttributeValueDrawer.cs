using System.Collections.Generic;
using Inno.Editor.Inspection;

namespace Inno.Editor.Inspection.Tests;

[PropertyDrawer(typeof(AttributeValue), priority: 10_000)]
internal sealed class AttributeValueDrawer : IPropertyDrawer
{
    public void Draw(PropertyDrawContext context)
        => observations.Add($"{context.label}:{context.isReadOnly}:{context.minimum}:{context.maximum}");

    internal static List<string> observations { get; } = [];
}
