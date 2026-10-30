using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Serialization;
using Inno.Core.Logging;
using Inno.Editor.Core;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Scene;
using Inno.Editor.Settings;
using Inno.Scene;
using Inno.Scene.Components;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
[InspectionDrawer(typeof(GameObject))]
internal sealed class GameObjectInspectionDrawer : InspectionDrawer<GameObject>
{
    private const nuint C_SEARCH_BUFFER_SIZE = 256;
    private readonly InspectorCardControls m_cardControls;
    private readonly SceneEdits m_edits;
    private readonly SerializationRegistry m_serialization;
    private readonly GameObjectTagSelector m_tagSelector;
    private readonly GameObjectLayerSelector m_layerSelector;
    private readonly EditorSettings m_settings;
    private string m_componentSearch = string.Empty;
    /// <summary>
    /// Creates a GameObject drawer backed by the current project classification settings.
    /// </summary>
    /// <param name="edits">
    /// The Scene editing service used for compact Undo/Redo records.
    /// </param>
    /// <param name="classificationSettings">
    /// The project tag and layer catalogs displayed in the target header.
    /// </param>
    /// <param name="settings">
    /// The project Settings service that owns semantic icon values.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that describes component properties in the active generation.
    /// </param>
    /// <param name="logs">
    /// The application log router used by component card controls.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name = "edits"/>, <paramref name="classificationSettings"/>, or
    /// <paramref name="settings"/> is <see langword="null"/>.
    /// </exception>
    internal GameObjectInspectionDrawer(
        SceneEdits edits,
        SceneProjectSettingsModule classificationSettings,
        EditorSettings settings,
        SerializationRegistry serialization,
        LogRouter logs
    ) {
        m_edits = edits ?? throw new ArgumentNullException(nameof(edits));
        ArgumentNullException.ThrowIfNull(classificationSettings);
        m_tagSelector = new GameObjectTagSelector(classificationSettings, edits);
        m_layerSelector = new GameObjectLayerSelector(classificationSettings, edits);
        m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
        m_serialization = serialization ?? throw new ArgumentNullException(nameof(serialization));
        m_cardControls = new InspectorCardControls(logs);
    }

    /// <summary>
    /// Gets the icon glyph used to represent this item in the editor.
    /// </summary>
    public override string icon => m_settings.Get("Editor/Appearance/Icons/GameObject").GetAsString("value", ImGuiIcon.Cube)!;

    /// <summary>
    /// Binds a caller-visible label to the current inspection target.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    /// <returns>
    /// The validated (string name, actionstring? setter) that represents the completed operation.
    /// </returns>
    protected override (string name, Action<string>? setter) BindName(
        InspectionDrawContext context,
        GameObject target
    ) => (target.name, name => m_edits.RenameGameObject(target, name));
    /// <summary>
    /// Renders the header presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="target">
    /// The existing target that receives the validated result.
    /// </param>
    protected override void DrawHeader(
        InspectionDrawContext context,
        GameObject target
    ) {
        bool active = target.activeSelf;
        if (EditorUI.current.widgets.CompactCheckbox($"target_active_{target.identity.persistentId:N}", ref active, tooltip: "Enable or disable this GameObject."))
        {
            m_edits.SetGameObjectActive(target, active);
        }

        Ui.SameLine(0f, -1f);
        Ui.Text("Active");
        Ui.SameLine(0f, EditorUI.current.widgets.style.headerSectionSpacing);
        float available = Ui.GetContentRegionAvailable().X;
        float tagLabelWidth = EditorUI.current.widgets.GetLabelChipSize("Tag").X;
        float layerLabelWidth = EditorUI.current.widgets.GetLabelChipSize("Layer").X;
        float controlWidth = MathF.Max(1f, (available - tagLabelWidth - layerLabelWidth - EditorUI.current.widgets.style.headerSectionSpacing) * 0.5f);
        m_tagSelector.Draw([target], context.interactions, controlWidth);
        Ui.SameLine(0f, EditorUI.current.widgets.style.headerSectionSpacing);
        m_layerSelector.Draw([target], context.interactions, controlWidth);
    }

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="gameObject">
    /// The scene object captured by this structural snapshot.
    /// </param>
    protected override void Draw(
        InspectionDrawContext context,
        GameObject gameObject
    ) {
        if (!gameObject.isRuntimeValid || !gameObject.scene.isLoaded)
        {
            _ = context.interactions.For(context.interactions.focusedArea).Select();
            Ui.Text("Selected GameObject is not available in the active scene.");
            return;
        }

        if (!m_layerSelector.IsLayerDefined(gameObject.layer))
        {
            Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.error);
            EditorUI.current.widgets.WrappedText($"Layer slot {gameObject.layer.index} is not defined in the current project settings. " + "Choose Default or restore that layer definition.");
            Ui.PopStyleColor();
            Ui.Spacing();
        }

        if (!m_tagSelector.IsTagDefined(gameObject.tag))
        {
            Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.error);
            EditorUI.current.widgets.WrappedText($"Tag '{gameObject.tag}' is not defined in the current Project Settings. " + "Choose another tag or restore that tag definition.");
            Ui.PopStyleColor();
            Ui.Spacing();
        }

        Ui.PushStyleVar((ImGuiStyleVar.ItemSpacing), EditorUI.current.widgets.style.compactItemSpacing);
        Ui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.compactFramePadding);
        try
        {
            DrawComponents(context, gameObject);
            DrawAddComponent(context, gameObject);
        }
        finally
        {
            Ui.PopStyleVar(2);
        }
    }

    private void DrawComponents(
        InspectionDrawContext context,
        GameObject gameObject
    ) {
        IReadOnlyList<GameComponent> components = gameObject.GetComponents();
        for (int i = 0; i < components.Count; i++)
        {
            GameComponent component = components[i];
            Type componentType = component.GetType();
            MissingGameComponent? missing = component as MissingGameComponent;
            IReadOnlyList<SerializedProperty> properties = missing is null ? m_serialization.GetProperties(component) : Array.Empty<SerializedProperty>();
            string componentId = component.identity.persistentId.ToString("N");
            GameBehavior? behavior = component as GameBehavior;
            var editorTarget = component;
            string title = missing?.missingTypeName ?? componentType.Name;
            bool canRemove = componentType != typeof(Transform);
            bool dimmed = behavior is { enabled: false };
            Vector2 cardMinimum = default;
            Vector2 headerMaximum = default;
            bool open = EditorUI.current.widgets.CollapsingCard(componentId, title, behavior is not null ? () =>
            {
                bool enabled = behavior.enabled;
                if (EditorUI.current.widgets.CompactCheckbox($"enabled_{componentId}", ref enabled, tooltip: "Enable or disable this component."))
                {
                    _ = m_edits.ChangeProperty(behavior, "enabled", () => behavior.enabled = enabled, enabled ? "Enable Component" : "Disable Component", mergeKey: null);
                }
            } : null, () => m_cardControls.DrawComponent(
                m_edits,
                component,
                canRemove,
                () => context.interactions.For(InspectorInteractionIds.C_COMPONENT_AREA, editorTarget).Enqueue(InspectorInteractionIds.C_REMOVE_COMPONENT)
            ), dimmed: dimmed, trailingControlWidth: m_cardControls.GetWidth(canRemove), drawContextMenu: () =>
            {
                cardMinimum = Ui.GetItemMinimum();
                headerMaximum = Ui.GetItemMaximum();
                _ = EditorUI.current.menus.ContextMenu($"##component_menu_{componentId}", context.interactions.For(InspectorInteractionIds.C_COMPONENT_AREA, editorTarget));
                m_cardControls.DrawComponentDragSource(component, title, dimmed);
            });
            Vector2 cardMaximum = headerMaximum;
            if (open)
            {
                Ui.Unindent();
                EditorUI.current.widgets.CardBody(componentId, () => EditorUI.current.widgets.SectionLayout(() =>
                {
                    if (missing is not null)
                    {
                        Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.error);
                        EditorUI.current.widgets.WrappedText($"Missing component script ({missing.missingType.stableId:D}). " + "Its serialized state is preserved and will recover automatically when the type returns.");
                        Ui.PopStyleColor();
                        return;
                    }

                    if (context.TryDrawInline(component))
                        return;
                    if (properties.Count == 0)
                    {
                        InspectorTypeOrigin.Draw(componentType);
                        return;
                    }

                    for (int propertyIndex = 0; propertyIndex < properties.Count; propertyIndex++)
                    {
                        context.properties.Draw(context.editorContext, component, $"gameObject.{gameObject.identity.persistentId:N}.{componentId}", properties[propertyIndex]);
                    }
                }), dimmed: dimmed);
                cardMaximum = Ui.GetItemMaximum();
                Ui.Indent();
                Ui.TreePop();
            }

            m_cardControls.DrawComponentDropTarget(context.interactions, m_edits, component, i, cardMinimum, cardMaximum);
            Ui.Dummy(new Vector2(0f, EditorUI.current.widgets.style.cardSpacing));
        }
    }

    private void DrawAddComponent(
        InspectionDrawContext context,
        GameObject gameObject
    ) {
        if (EditorUI.current.widgets.CenteredButton("Add Component", EditorUI.current.widgets.style.addButtonTopPadding))
        {
            m_componentSearch = string.Empty;
            Ui.OpenPopup("##add_component_popup");
        }

        if (!EditorUI.current.widgets.BeginSearchPopup("##add_component_popup", ref m_componentSearch, "Search components...", C_SEARCH_BUFFER_SIZE))
        {
            return;
        }

        try
        {
            EditorInteraction interaction = context.interactions.For(InspectorInteractionIds.C_COMPONENT_AREA, gameObject);
            if (EditorUI.current.menus.DrawSearchItems(interaction, interaction.BuildMenu().items, m_componentSearch))
                Ui.CloseCurrentPopup();
        }
        finally
        {
            EditorUI.current.widgets.EndSearchPopup();
        }
    }
}
