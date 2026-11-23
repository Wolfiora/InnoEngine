using System.Collections.Generic;
using Inno.Editor.Inspection;

namespace Inno.Editor.Inspection.Native.Tests;

[PropertyDrawer(typeof(TextStateValue), priority: 10_000)]
internal sealed class TextStateDrawer : IPropertyDrawer
{
    public void Draw(PropertyDrawContext context)
    {
        TextStateValue value = (TextStateValue)context.GetValue()!;
        bool hasState = context.TryGetTextState("editing", out string? state);
        observations.Add($"{value.id}:{(hasState ? state : "<new>")}");
        context.SetTextState("editing", value.id);
    }

    internal static List<string> observations { get; } = [];
}
