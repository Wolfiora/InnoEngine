using Inno.Editor.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using Inno.Extensibility.Types;
using Inno.Core.Serialization;
using Inno.Editor.ImGui.ImGuiWidget;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Inspection;
[PropertyDrawer(typeof(ISerializable), useForChildren: true, priority: 20)]
internal sealed class SerializableObjectPropertyDrawer : IPropertyDrawer
{
    private readonly SerializationRegistry m_serialization;
    private readonly TypeCacheSnapshot m_types;
    internal SerializableObjectPropertyDrawer(
        TypeCacheSnapshot types,
        SerializationRegistry serialization
    ) {
        m_types = types ?? throw new ArgumentNullException(nameof(types));
        m_serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
    }

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    public void Draw(PropertyDrawContext context)
    {
        object? value = context.GetValue();
        Type runtimeType = value?.GetType() ?? context.propertyType;
        DrawTypeSelector(context, runtimeType, value);
        value = context.GetValue();
        if (value is not ISerializable serializable)
        {
            Ui.Text("Null");
            return;
        }

        string header = $"{value.GetType().Name}##{context.path}_foldout";
        if (!Ui.TreeNode(header, (ImGuiTreeNodeFlags.SpanAvailWidth)))
        {
            return;
        }

        IReadOnlyList<SerializedProperty> properties = m_serialization.GetProperties(serializable);
        EditorUI.current.widgets.SectionLayout(() =>
        {
            for (int i = 0; i < properties.Count; i++)
            {
                context.DrawChild(serializable, properties[i]);
            }
        });
        Ui.TreePop();
    }

    private void DrawTypeSelector(
        PropertyDrawContext context,
        Type runtimeType,
        object? value
    ) {
        if (!EditorUI.current.widgets.BeginBoundedCombo($"##{context.path}_runtime_type", value is null ? "Null" : runtimeType.Name))
        {
            return;
        }

        if (Ui.Selectable("Null", value is null))
        {
            context.SetValue(null);
        }

        IReadOnlyList<TypeRef> candidates = m_types.GetTypesImplementing<ISerializable>();
        for (int i = 0; i < candidates.Count; i++)
        {
            Type candidate = candidates[i].Resolve(m_types);
            if (!context.propertyType.IsAssignableFrom(candidate))
            {
                continue;
            }

            if (Ui.Selectable(candidate.Name, candidate == runtimeType && value is not null))
            {
                context.SetValue(Activator.CreateInstance(candidate, nonPublic: true));
            }
        }

        EditorUI.current.widgets.EndBoundedCombo();
    }
}
