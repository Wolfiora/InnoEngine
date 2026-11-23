using System;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Inspection;

[PropertyDrawer(typeof(Nullable<>))]
internal sealed class NullablePropertyDrawer : IPropertyDrawer
{
    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        Type underlyingType = Nullable.GetUnderlyingType(context.propertyType)
                              ?? throw new InvalidOperationException("Nullable drawer requires a nullable property type.");
        object? value = context.GetValue();
        bool hasValue = value is not null;
        bool previousHasValue = hasValue;
        _ = EditorUI.current.widgets.Checkbox($"##{context.path}_has_value", ref hasValue, context.tooltip);
        if (hasValue != previousHasValue)
        {
            value = hasValue ? Activator.CreateInstance(underlyingType) : null;
            context.SetValue(value);
        }

        if (!hasValue)
        {
            Gui.SameLine(0f, -1f);
            Gui.Text("Null");
            return;
        }

        Gui.SameLine(0f, -1f);
        context.DrawInlineChild("Value", underlyingType, context.GetValue, value => context.SetValue(value));
    }
}
