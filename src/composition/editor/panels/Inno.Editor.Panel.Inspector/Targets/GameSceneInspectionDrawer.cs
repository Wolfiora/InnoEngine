using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Logging;
using Inno.Core.Serialization;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Editor.Settings;
using Inno.Scene;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;

[InspectionDrawer(typeof(GameScene))]
internal sealed class GameSceneInspectionDrawer : InspectionDrawer<GameScene>
{
    private const nuint C_SEARCH_BUFFER_SIZE = 256;
    private readonly InspectorCardControls m_cardControls;
    private readonly SceneEdits m_edits;
    private readonly EditorSettings m_settings;
    private readonly SerializationRegistry m_serialization;
    private string m_systemSearch = string.Empty;

    /// <summary>
    /// Creates a Scene drawer backed by the Scene editing service.
    /// </summary>
    /// <param name="edits">
    /// The Scene editing service used for compact Undo/Redo records.
    /// </param>
    /// <param name="settings">
    /// The project Settings service that owns semantic icon values.
    /// </param>
    /// <param name="serialization">
    /// The serialization registry that describes system properties in the active generation.
    /// </param>
    /// <param name="logs">
    /// The application log router used by component card controls.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name = "edits"/> or <paramref name="settings"/> is <see langword="null"/>.
    /// </exception>
    internal GameSceneInspectionDrawer(
        SceneEdits edits,
        EditorSettings settings,
        SerializationRegistry serialization,
        LogRouter logs
    ) {
        ArgumentNullException.ThrowIfNull(edits);
        ArgumentNullException.ThrowIfNull(settings);
        ArgumentNullException.ThrowIfNull(serialization);
        m_edits = edits;
        m_settings = settings;
        m_serialization = serialization;
        m_cardControls = new InspectorCardControls(logs);
    }

    /// <summary>
    /// Gets the icon glyph used to represent this item in the editor.
    /// </summary>
    public override string icon => m_settings.Get("Editor/Appearance/Icons/Scene").GetAsString("value", ImGuiIcon.C_CUBES)!;

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
        GameScene target
    ) => (target.name, name => m_edits.RenameScene(target, name));

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
        GameScene target
    ) => Gui.Text(target.isLoaded ? "Loaded Scene" : "Scene");

    /// <summary>
    /// Renders the value presentation for the current editor frame.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <param name="scene">
    /// The scene consumed by draw; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    protected override void Draw(
        InspectionDrawContext context,
        GameScene scene
    ) {
        if (!scene.isLoaded || scene.isDestroyed)
        {
            _ = context.interactions.For(context.interactions.focusedArea).Select();
            Gui.Text("Selected scene is no longer loaded.");
            return;
        }

        Gui.PushStyleVar((ImGuiStyleVar.ItemSpacing), EditorUI.current.widgets.style.compactItemSpacing);
        Gui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.compactFramePadding);
        try
        {
            DrawSystems(context, scene);
            DrawAddSystem(context, scene);
        }
        finally
        {
            Gui.PopStyleVar(2);
        }
    }

    private void DrawSystems(
        InspectionDrawContext context,
        GameScene scene
    ) {
        IReadOnlyList<GameSystem> systems = scene.GetSystems();
        for (int i = 0; i < systems.Count; i++)
        {
            GameSystem system = systems[i];
            MissingGameSystem? missing = system as MissingGameSystem;
            IReadOnlyList<SerializedProperty> properties = missing is null
                                                           ? m_serialization.GetProperties(system)
                                                           : [];
            string systemID = system.identity.persistentID.ToString("N");
            GameSystem editorTarget = system;
            string title = missing?.missingTypeName ?? system.GetType().Name;
            bool dimmed = !system.enabled;
            Vector2 cardMinimum = default;
            Vector2 headerMaximum = default;
            bool open = EditorUI.current.widgets.CollapsingCard(
                systemID,
                title,
                missing is null
                ? () =>
                {
                    bool enabled = system.enabled;
                    if (EditorUI.current.widgets.CompactCheckbox(
                        $"enabled_{systemID}",
                        ref enabled,
                        tooltip: "Enable or disable this system."
                    )) {
                        _ = m_edits.ChangeProperty(
                            system,
                            "enabled",
                            () => system.enabled = enabled,
                            enabled ? "Enable System" : "Disable System",
                            mergeKey: null
                        );
                    }
                }
                : null,
                () => m_cardControls.DrawSystem(
                    m_edits,
                    scene,
                    system,
                    () => context.interactions
                        .For(InspectorInteractionIDs.C_SYSTEM_AREA, editorTarget)
                        .Enqueue(InspectorInteractionIDs.C_REMOVE_SYSTEM)
                ),
                dimmed: dimmed,
                trailingControlWidth: m_cardControls.GetWidth(canRemove: true),
                drawContextMenu: () =>
                {
                    cardMinimum = Gui.GetItemMinimum();
                    headerMaximum = Gui.GetItemMaximum();
                    _ = EditorUI.current.menus.ContextMenu(
                        $"##system_menu_{systemID}",
                        context.interactions.For(InspectorInteractionIDs.C_SYSTEM_AREA, editorTarget)
                    );
                    m_cardControls.DrawSystemDragSource(system, title, dimmed);
                }
            );
            Vector2 cardMaximum = headerMaximum;
            if (open)
            {
                Gui.Unindent();
                EditorUI.current.widgets.CardBody(systemID, () => EditorUI.current.widgets.SectionLayout(() =>
                {
                    if (missing is not null)
                    {
                        Gui.PushStyleColor((ImGuiCol.Text), EditorPalette.error);
                        EditorUI.current.widgets.WrappedText(
                            $"Missing system script ({missing.missingType.stableID:D}). " + "Its serialized state is preserved and will recover automatically when the type returns."
                        );
                        Gui.PopStyleColor();
                        return;
                    }

                    if (properties.Count == 0)
                    {
                        InspectorTypeOrigin.Draw(system.GetType());
                        return;
                    }

                    for (int propertyIndex = 0; propertyIndex < properties.Count; propertyIndex++)
                    {
                        context.properties.Draw(
                            context.editorContext,
                            system,
                            $"scene.{scene.identity.persistentID:N}.{systemID}",
                            properties[propertyIndex]
                        );
                    }
                }), dimmed: dimmed);
                cardMaximum = Gui.GetItemMaximum();
                Gui.Indent();
                Gui.TreePop();
            }

            m_cardControls.DrawSystemDropTarget(context.interactions, m_edits, scene, system, i, cardMinimum, cardMaximum);
            Gui.Dummy(new Vector2(0f, EditorUI.current.widgets.style.cardSpacing));
        }
    }

    private void DrawAddSystem(
        InspectionDrawContext context,
        GameScene scene
    ) {
        if (EditorUI.current.widgets.CenteredButton("Add System", EditorUI.current.widgets.style.addButtonTopPadding))
        {
            m_systemSearch = string.Empty;
            Gui.OpenPopup("##add_system_popup");
        }

        if (!EditorUI.current.widgets.BeginSearchPopup("##add_system_popup", ref m_systemSearch, "Search systems...", C_SEARCH_BUFFER_SIZE))
        {
            return;
        }
        try
        {
            EditorInteraction interaction = context.interactions.For(InspectorInteractionIDs.C_SYSTEM_AREA, scene);
            if (EditorUI.current.menus.DrawSearchItems(interaction, interaction.BuildMenu().items, m_systemSearch))
            {
                Gui.CloseCurrentPopup();
            }
        }
        finally
        {
            EditorUI.current.widgets.EndSearchPopup();
        }
    }
}
