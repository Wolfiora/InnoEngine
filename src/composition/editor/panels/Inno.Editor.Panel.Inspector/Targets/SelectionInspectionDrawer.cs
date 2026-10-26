using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Editor.ImGui;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;

namespace Inno.Editor.Panel.Inspector;
[InspectionDrawer(typeof(EditorSelectionSnapshot))]
internal sealed class SelectionInspectionDrawer(
    SceneEdits edits,
    SerializationRegistry serialization,
    SceneProjectSettingsModule settings
) : InspectionDrawer<EditorSelectionSnapshot>
{
    private readonly GameObjectTagSelector m_tags = new(settings, edits);
    private readonly GameObjectLayerSelector m_layers = new(settings, edits);
    private string m_componentSearch = "";
    /// <inheritdoc/>
    public override string icon => ImGuiIcon.LayerGroup;

    /// <inheritdoc/>
    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        EditorSelectionSnapshot target
    ) => ($"{target.identities.Count} Selected", null);
    /// <inheritdoc/>
    protected override void Draw(
        InspectionDrawContext context,
        EditorSelectionSnapshot target
    ) {
        var values = new List<EngineObject>(target.identities.Count);
        target.CopyTargets(context.interactions, values);
        if (values.Count != target.identities.Count)
        {
            EditorUiSession.current.widgets.Hint("The selection includes unavailable or different content. Select compatible scene objects or assets to edit common values.");
            return;
        }

        if (values.Count == 0)
            return;
        int primaryIndex = values.FindIndex(value => value.identity.runtimeIdentity == target.primary);
        if (primaryIndex > 0)
        {
            EngineObject primary = values[primaryIndex];
            values.RemoveAt(primaryIndex);
            values.Insert(0, primary);
        }
        bool readOnly = values.Any(value => !edits.CanEdit(value));
        if (readOnly)
            EditorUiSession.current.widgets.HelpBox("The selection includes a read-only owner. No batch changes will be applied.", ImGuiIcon.Lock, EditorPalette.warning);
        EditorUiSession.current.widgets.Disabled(readOnly, () =>
        {
            if (values.All(value => value is GameObject))
                DrawGameObjects(context, target, values.Cast<GameObject>().ToArray());
            else
                DrawCommon(context, target, values, "selection");
        });
    }

    private void DrawGameObjects(
        InspectionDrawContext context,
        EditorSelectionSnapshot state,
        IReadOnlyList<GameObject> targets
    ) {
        bool active = targets[0].activeSelf;
        bool mixed = targets.Any(value => value.activeSelf != active);
        EditorUiSession.current.widgets.MixedValue(mixed, () =>
        {
            if (!EditorUiSession.current.widgets.CompactCheckbox("selection.active", ref active, tooltip: "Set the active state of every selected object."))
                return;
            using EditorHistoryTransaction transaction = context.interactions.history.BeginTransaction("Set Selected Active");
            foreach (GameObject target in targets)
                edits.SetGameObjectActive(target, active);
            transaction.Commit();
        });
        EditorUiSession.current.widgets.Hint(mixed ? "Active \u00b7 Mixed values" : "Active");
        float width = MathF.Max(1, Inno.Editor.ImGui.ImGui.GetContentRegionAvailable().X);
        m_tags.Draw(targets, context.interactions, width);
        m_layers.Draw(targets, context.interactions, width);
        IReadOnlyList<GameComponent> primary = targets[0].GetComponents();
        var occurrences = new Dictionary<Type, int>();
        foreach (GameComponent component in primary)
        {
            Type type = component.GetType();
            int occurrence = occurrences.GetValueOrDefault(type);
            occurrences[type] = occurrence + 1;
            GameComponent? [] matches = targets.Select(value => value.GetComponents().Where(candidate => candidate.GetType() == type).Skip(occurrence).FirstOrDefault()).ToArray();
            if (matches.Any(value => value is null))
                continue;
            if (EditorUiSession.current.widgets.SectionHeader($"{type.Name} ({occurrence + 1})", "Common component values. Changes apply atomically to the entire selection."))
            {
                string id = $"selection.{type.FullName}.{occurrence}";
                DrawCommon(context, state, matches.Cast<EngineObject>().ToArray(), id);
                if (Inno.Editor.ImGui.ImGui.Button("Reset Selected##" + id))
                {
                    using EditorHistoryTransaction transaction = context.interactions.history.BeginTransaction("Reset Components");
                    foreach (GameComponent match in matches.Cast<GameComponent>())
                        edits.ResetComponent(match);
                    transaction.Commit();
                }

                if (matches.All(value => value is not Inno.Scene.Components.Transform))
                {
                    Inno.Editor.ImGui.ImGui.SameLine(0, -1);
                    if (Inno.Editor.ImGui.ImGui.Button("Remove Selected##" + id))
                    {
                        using EditorHistoryTransaction transaction = context.interactions.history.BeginTransaction("Remove Components");
                        foreach (GameComponent match in matches.Cast<GameComponent>())
                            edits.RemoveComponent(match);
                        transaction.Commit();
                        return;
                    }
                }
            }
        }

        DrawAddComponents(context, targets[0]);
    }

    private void DrawAddComponents(
        InspectionDrawContext context,
        GameObject primary
    ) {
        var widgets = EditorUiSession.current.widgets;
        if (widgets.CenteredButton("Add Component to Selection", widgets.style.inspectorAddButtonTopPadding))
        {
            m_componentSearch = "";
            Inno.Editor.ImGui.ImGui.OpenPopup("selection.add-component");
        }

        if (!widgets.BeginSearchPopup("selection.add-component", ref m_componentSearch, "Search components...", 256))
            return;
        try
        {
            EditorInteraction interaction = context.interactions.For(InspectorInteractionIds.C_COMPONENT_AREA, primary);
            if (EditorUiSession.current.menus.DrawSearchItems(interaction, interaction.BuildMenu().items, m_componentSearch))
                Inno.Editor.ImGui.ImGui.CloseCurrentPopup();
        }
        finally
        {
            widgets.EndSearchPopup();
        }
    }

    private void DrawCommon(
        InspectionDrawContext context,
        EditorSelectionSnapshot state,
        IReadOnlyList<EngineObject> targets,
        string path
    ) {
        if (targets.Any(value => value is not ISerializable))
        {
            EditorUiSession.current.widgets.Hint("These objects expose no common serialized properties.");
            return;
        }

        IReadOnlyList<SerializedProperty>[] properties = targets.Select(value => serialization.GetProperties((ISerializable)value)).ToArray();
        int common = 0;
        foreach (SerializedProperty property in properties[0])
        {
            if (properties.Any(values => !values.Any(value => value.name == property.name && value.propertyType == property.propertyType)))
                continue;
            var availability = properties.Select((
                values,
                index
            ) => context.properties.GetPropertyAvailability(
                targets[index], values.Single(value => value.name == property.name && value.propertyType == property.propertyType))).ToArray();
            if (availability.Any(value => !value.isVisible))
                continue;
            bool readOnly = availability.Any(value => value.isReadOnly);
            common++;
            bool mixed = !edits.HaveSamePropertyValues(targets, property.name);
            EditorUiSession.current.widgets.MixedValue(
                mixed,
                () => context.properties.DrawDraftProperty(context.editorContext, state, targets[0], path, property, new SelectionEdits(edits, targets), readOnly)
            );
            if (mixed)
                EditorUiSession.current.widgets.Hint("Mixed values");
        }

        if (common == 0)
            EditorUiSession.current.widgets.Hint("No common editable properties.");
    }

    private sealed class SelectionEdits(
        SceneEdits edits,
        IReadOnlyList<EngineObject> targets
    ) : IInspectionPropertyEditService
    {
        /// <inheritdoc />
        public bool ChangeProperty(
            object owner,
            string propertyName,
            Action mutation,
            string historyName,
            string? mergeKey,
            string? componentName
        ) => edits.ChangeProperties(targets, propertyName, mutation, historyName, mergeKey, componentName);
    }
}
