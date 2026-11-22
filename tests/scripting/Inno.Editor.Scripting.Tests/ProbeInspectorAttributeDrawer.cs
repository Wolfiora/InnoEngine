using System.Collections.Generic;
using Inno.Editor.Inspection;

namespace Inno.Editor.Scripting.Tests;

[InspectorAttributeDrawer(typeof(ProbeInspectorAttribute), priority: 10_000)]
internal sealed class ProbeInspectorAttributeDrawer : IInspectorAttributeDrawer
{
    public void Update(InspectorAttributeDrawContext context)
        => observations.Add($"update:{context.label}:{context.isReadOnly}:{context.minimum}:{context.maximum}");

    public void DrawBefore(InspectorAttributeDrawContext context)
    {
        _ = context;
        observations.Add("before");
    }

    public void DrawAfter(InspectorAttributeDrawContext context)
    {
        _ = context;
        observations.Add("after");
    }

    internal static List<string> observations { get; } = [];
}
