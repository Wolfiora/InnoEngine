using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Identity;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Editor.Settings;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Scene;
using Inno.Scene.Components;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Hierarchy;
/// <summary>
/// Edits the active scene object hierarchy.
/// </summary>
[EditorPanel("scene.hierarchy", "Hierarchy", order: 100, menuPath: "Workspace")]
internal sealed partial class HierarchyPanel : EditorPanel
{
    private const nuint C_NAME_BUFFER_SIZE = 512;

    private readonly List<RuntimeIdentity> m_visibleSelection = [];
    private RuntimeIdentity? m_pendingSelection;
    private RuntimeIdentity? m_keyboardFocus;
    private RuntimeIdentity? m_requestedKeyboardFocus;
    private RuntimeIdentity? m_drawKeyboardFocus;
    private int m_keyboardPageSize = 1;
    private readonly IEditorSceneWorkspace m_workspace;
    private readonly EditorInteractions m_interactions;
    private readonly SceneEdits m_edits;
    private readonly HierarchySelection m_selection;
    private readonly EditorSettings m_settings;
    private readonly HierarchyDropVisual m_dropVisual = new();
    private readonly HashSet<Guid> m_forceOpenIds = [];
    private readonly HashSet<Guid> m_forceOpenSceneIds = [];
    private readonly HashSet<Guid> m_initializedSceneIds = [];
    private ulong m_lastRevealedSelectionVersion = ulong.MaxValue;
    private int m_visibleRowIndex;
    /// <summary>
    /// Gets whether use window padding is enabled for this implementation.
    /// </summary>
    public override bool useWindowPadding => false;
    /// <summary>
    /// Lets the hierarchy content child own both scroll directions.
    /// </summary>
    public override bool allowScrolling => false;

    /// <summary>
    /// Creates the hierarchy panel.
    /// </summary>
    internal HierarchyPanel(
        IEditorSceneWorkspace workspace,
        EditorInteractions interactions,
        SceneEdits edits,
        EditorSettings settings
    ) {
        m_workspace = workspace ?? throw new ArgumentNullException(nameof(workspace));
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
        m_edits = edits ?? throw new ArgumentNullException(nameof(edits));
        m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
        m_selection = new HierarchySelection(workspace, interactions, m_settings);
    }

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        bool visible = Ui.BeginChild("##HierarchyContent", Vector2.Zero, (ImGuiChildFlags.None), (ImGuiWindowFlags.NoSavedSettings | ImGuiWindowFlags.HorizontalScrollbar));
        try
        {
            if (visible)
            {
                m_keyboardPageSize = Math.Max(1, (int)(Ui.GetContentRegionAvailable().Y / Math.Max(1f, Ui.GetFrameHeight())));
                DrawContent(context);
            }
        }
        finally
        {
            Ui.EndChild();
        }
    }

    private static HierarchyStyle hierarchyStyle => new(EditorUI.current.widgets.style);

    private void DrawContent(EditorContext context)
    {
        m_drawKeyboardFocus = m_requestedKeyboardFocus;
        m_requestedKeyboardFocus = null;
        m_pendingSelection = null;
        m_visibleRowIndex = 0;
        m_selection.Prune();
        RevealSelection();
        Ui.PushStyleVar((ImGuiStyleVar.ItemSpacing), hierarchyStyle.itemSpacing);
        Ui.PushStyleVar((ImGuiStyleVar.FramePadding), EditorUI.current.widgets.style.compactFramePadding);
        try
        {
            IReadOnlyList<GameScene> scenes = m_workspace.scenes;
            if (scenes.Count > 0)
            {
                Vector2 contentOrigin = Ui.GetCursorScreenPosition();
                Ui.SetCursorScreenPosition(new Vector2(contentOrigin.X, contentOrigin.Y - Ui.GetWindowBounds().padding.Y));
            }

            RefreshRows(scenes);
            DrawRows(context);

            DrawBlankArea(context);
            if (m_pendingSelection is RuntimeIdentity pending && m_interactions.TryResolveIdentity(pending, out IdentityObject? target) && target is not null)
            {
                EditorUI.current.widgets.SelectTarget(m_interactions, target, m_visibleSelection);
                m_keyboardFocus = pending;
                m_interactions.For(HierarchyInteractionIds.C_AREA, target).Focus();
            }
            HandleKeyboardNavigation();
        }
        finally
        {
            m_pendingSelection = null;
            Ui.PopStyleVar(2);
        }
    }

    private void HandleKeyboardNavigation()
    {
        int focused = -1;
        int selected = -1;
        RuntimeIdentity? selectedTarget = (m_interactions.selection.selectedTarget as IdentityObject)?.identity.runtimeIdentity;
        for (int index = 0; index < m_visibleSelection.Count; index++)
        {
            RuntimeIdentity item = m_visibleSelection[index];
            if (item == m_keyboardFocus)
                focused = index;
            if (item == selectedTarget)
                selected = index;
        }
        if (focused < 0)
            focused = selected;
        ListNavigationResult navigation = EditorUI.current.widgets.NavigateList(m_interactions, m_visibleSelection.Count, focused, m_keyboardPageSize);
        if (navigation.focusedIndex >= 0 && (navigation.focusChanged || navigation.selectionRequested || navigation.activate))
        {
            RuntimeIdentity identity = m_visibleSelection[navigation.focusedIndex];
            if (!m_interactions.TryResolveIdentity(identity, out IdentityObject? target) || target is null)
                return;
            m_keyboardFocus = identity;
            if (navigation.focusChanged)
                m_requestedKeyboardFocus = m_keyboardFocus;
            if (navigation.selectionRequested)
            {
                if (navigation.extendRange)
                    m_interactions.SelectRange(m_visibleSelection, target, navigation.additive);
                else if (navigation.toggle)
                    m_interactions.ToggleSelection(target);
                else
                    m_interactions.SetSelection(target);
            }
            m_interactions.For(HierarchyInteractionIds.C_AREA, m_interactions.selection.selectedTarget).Focus();
            if (navigation.activate)
            {
                if (!m_interactions.selection.Contains(target))
                    m_interactions.SetSelection(target);
                _ = m_interactions.For(HierarchyInteractionIds.C_AREA, target).Execute(HierarchyInteractionIds.C_RENAME);
            }
        }
        if (navigation.cancel)
            m_keyboardFocus = null;
    }

    private void DrawScene(
        EditorContext context,
        GameScene scene,
        int depth
    ) {
        string id = scene.identity.persistentId.ToString("N");
        bool selected = m_interactions.selection.Contains(scene);
        TreeNodeResult result = EditorUI.current.widgets.TreeNode(
            $"scene_{id}",
            drawContext => DrawSceneRowContent(scene, id, drawContext.rowHeight),
            new TreeNodeOptions {
                depth = depth, selected = selected, requestFocus = scene.identity.runtimeIdentity == m_drawKeyboardFocus,
                keepContentActive = m_interactions.For(HierarchyInteractionIds.C_AREA, scene).IsActive(HierarchyInteractionIds.C_RENAME),
                showBackground = true, backgroundColor = EditorPalette.title
            });
        if (!result.isSubmitted)
            return;
        if (result.isClicked || result.isDoubleClicked)
            m_pendingSelection = scene.identity.runtimeIdentity;
        if (result.isDoubleClicked)
            _ = m_interactions.For(HierarchyInteractionIds.C_AREA, scene).Execute(HierarchyInteractionIds.C_RENAME);
        _ = EditorDragDropRenderer.Source(m_interactions.For(HierarchyInteractionIds.C_AREA, scene),
            new EditorDragData(scene, scene.name), () => Ui.Text(scene.name));
        EditorDropPlacement placement = m_dropVisual.GetScenePlacement(result, Ui.GetMousePos().Y);
        EditorDropWidgetResult drop = EditorDragDropRenderer.Target(
            m_interactions.For(HierarchyInteractionIds.C_AREA, new HierarchySceneDropTarget(scene)), placement);
        if (drop.isPreviewing && drop.status.canDrop)
            m_dropVisual.Draw(result, drop.status.visual);
        ApplyDropResult(drop.result);
        DrawSceneContextMenu(context, scene, id);
    }

    private void DrawObject(
        EditorContext context,
        GameObject gameObject,
        int depth
    ) {
        if (!gameObject.isRuntimeValid)
            return;
        string id = gameObject.identity.persistentId.ToString("N");
        TreeNodeResult result = EditorUI.current.widgets.TreeNode(
            $"hierarchy_{id}",
            drawContext => DrawRowContent(context, gameObject, drawContext.rowHeight),
            new TreeNodeOptions {
                depth = depth, selected = m_interactions.selection.Contains(gameObject),
                requestFocus = gameObject.identity.runtimeIdentity == m_drawKeyboardFocus,
                keepContentActive = m_interactions.For(HierarchyInteractionIds.C_AREA, gameObject).IsActive(HierarchyInteractionIds.C_RENAME),
                isLeaf = gameObject.transform.children.Count == 0, showBackground = true,
                backgroundColor = m_visibleRowIndex % 2 == 0 ? EditorPalette.collectionRow : EditorPalette.collectionRowAlternate,
                drawViewportOverlay = () => DrawVisibilityButton(gameObject, id)
            });
        if (!result.isSubmitted)
            return;
        if (result.isClicked || result.isDoubleClicked)
            m_pendingSelection = gameObject.identity.runtimeIdentity;
        if (result.isDoubleClicked)
            _ = m_interactions.For(HierarchyInteractionIds.C_AREA, gameObject).Execute(HierarchyInteractionIds.C_RENAME);
        _ = EditorDragDropRenderer.Source(m_interactions.For(HierarchyInteractionIds.C_AREA, gameObject),
            new EditorDragData(gameObject, gameObject.name), () => Ui.Text(gameObject.name));
        EditorDropPlacement placement = m_dropVisual.GetObjectPlacement(result, Ui.GetMousePos().Y);
        EditorDropWidgetResult drop = EditorDragDropRenderer.Target(
            m_interactions.For(HierarchyInteractionIds.C_AREA, new HierarchyObjectDropTarget(gameObject)), placement);
        if (drop.isPreviewing && drop.status.canDrop)
            m_dropVisual.Draw(result, drop.status.visual);
        ApplyDropResult(drop.result);
        DrawObjectContextMenu(context, gameObject, id);
    }

    private void DrawRowContent(
        EditorContext context,
        GameObject gameObject,
        float rowHeight
    ) {
        string id = gameObject.identity.persistentId.ToString("N");
        bool dimmed = !gameObject.activeInHierarchy;
        if (dimmed)
        {
            Ui.PushStyleColor((ImGuiCol.Text), HierarchyStyle.inactiveText);
        }

        try
        {
            EditorInteraction interaction = m_interactions.For(HierarchyInteractionIds.C_AREA, gameObject);
            bool isRenaming = interaction.IsActive(HierarchyInteractionIds.C_RENAME);
            if (isRenaming)
            {
                EditorUI.current.widgets.IconText(m_settings.Get("Editor/Appearance/Icons/GameObject").GetAsString("value", ImGuiIcon.Cube)!, string.Empty, false);
                Ui.SameLine(0f, 0f);
                float visibilityWidth = GetVisibilityButtonWidth();
                float renameWidth = MathF.Max(
                    hierarchyStyle.renameMinimumWidth,
                    Ui.GetContentRegionAvailable().X - visibilityWidth - hierarchyStyle.renameTrailingGap
                );
                _ = interaction.Present(HierarchyInteractionIds.C_RENAME, new InlineRenamePresentation($"hierarchy_{id}", renameWidth, rowHeight, C_NAME_BUFFER_SIZE));
            }
            else
            {
                EditorUI.current.widgets.IconText(m_settings.Get("Editor/Appearance/Icons/GameObject").GetAsString("value", ImGuiIcon.Cube)!, gameObject.name, false);
            }
        }
        finally
        {
            if (dimmed)
                Ui.PopStyleColor();
        }
    }

    private void DrawSceneRowContent(
        GameScene scene,
        string id,
        float rowHeight
    ) {
        EditorInteraction interaction = m_interactions.For(HierarchyInteractionIds.C_AREA, scene);
        if (!interaction.IsActive(HierarchyInteractionIds.C_RENAME))
        {
            m_selection.DrawSceneRowContent(scene);
            return;
        }

        EditorUI.current.widgets.IconText(
            m_settings.Get("Editor/Appearance/Icons/Scene").GetAsString("value", ImGuiIcon.Cubes)!,
            string.Empty,
            ReferenceEquals(scene, m_workspace.activeScene)
        );
        Ui.SameLine(0f, 0f);
        float renameWidth = MathF.Max(
            hierarchyStyle.renameMinimumWidth,
            Ui.GetContentRegionAvailable().X - hierarchyStyle.renameTrailingGap
        );
        _ = interaction.Present(HierarchyInteractionIds.C_RENAME, new InlineRenamePresentation($"scene_{id}", renameWidth, rowHeight, C_NAME_BUFFER_SIZE));
    }

    private void DrawVisibilityButton(
        GameObject gameObject,
        string id
    ) {
        string icon = gameObject.activeSelf ? ImGuiIcon.Eye : ImGuiIcon.EyeSlash;
        float buttonWidth = GetVisibilityButtonWidth();
        float right = Ui.GetWindowBounds().workMaximum.X - EditorUI.current.widgets.style.windowPadding.X - buttonWidth;
        Ui.SameLine(0f, -1f);
        Vector2 cursor = Ui.GetCursorScreenPosition();
        Ui.SetCursorScreenPosition(new Vector2(MathF.Max(cursor.X, right), cursor.Y));
        if (EditorUI.current.widgets.ClickableIcon($"hierarchy_visibility_{id}", icon, gameObject.activeSelf ? "Deactivate" : "Activate"))
        {
            m_edits.SetGameObjectActive(gameObject, !gameObject.activeSelf);
        }
    }

    private static float GetVisibilityButtonWidth() => EditorUI.current.widgets.GetCompactIconSize().X;
    private void DrawSceneContextMenu(
        EditorContext context,
        GameScene scene,
        string id
    ) {
        if (Ui.IsItemClicked(ImGuiMouseButton.Right) && !m_interactions.selection.Contains(scene))
            m_interactions.SetSelection(scene);
        _ = EditorUI.current.menus.ContextMenu($"##scene_context_{id}", m_interactions.For(HierarchyInteractionIds.C_AREA, scene));
    }

    private void DrawObjectContextMenu(
        EditorContext context,
        GameObject gameObject,
        string id
    ) {
        if (Ui.IsItemClicked(ImGuiMouseButton.Right) && !m_interactions.selection.Contains(gameObject))
            m_interactions.SetSelection(gameObject);
        _ = EditorUI.current.menus.ContextMenu($"##hierarchy_context_{id}", m_interactions.For(HierarchyInteractionIds.C_AREA, gameObject));
    }

    private void DrawBlankArea(EditorContext context)
    {
        Vector2 available = Ui.GetContentRegionAvailable();
        Vector2 size = new(MathF.Max(1f, available.X), MathF.Max(hierarchyStyle.blankMinimumHeight, available.Y));
        _ = Ui.InvisibleButton("##hierarchy_blank", size);
        if (Ui.IsItemClicked((ImGuiMouseButton.Left)))
        {
            _ = m_interactions.For(HierarchyInteractionIds.C_AREA).Select();
        }

        GameScene? activeScene = m_workspace.activeScene;
        if (activeScene is not null)
        {
            EditorDropWidgetResult drop = EditorDragDropRenderer.Target(
                m_interactions.For(HierarchyInteractionIds.C_AREA, new HierarchySceneDropTarget(activeScene)),
                EditorDropPlacement.Into
            );
            ApplyDropResult(drop.result);
        }

        _ = EditorUI.current.menus.ContextMenu(
            "##hierarchy_blank_context",
            activeScene is null ? m_interactions.For(HierarchyInteractionIds.C_AREA) : m_interactions.For(HierarchyInteractionIds.C_AREA, activeScene)
        );
    }

    private void RevealSelection()
    {
        ulong selectionVersion = m_interactions.selection.version;
        if (m_lastRevealedSelectionVersion == selectionVersion)
            return;
        m_lastRevealedSelectionVersion = selectionVersion;
        if (!m_interactions.selection.TryGet(out GameObject? gameObject) || !gameObject.isRuntimeValid)
            return;
        m_forceOpenSceneIds.Add(gameObject.scene.identity.persistentId);
        for (Transform? current = gameObject.transform.parent; current is not null; current = current.parent)
        {
            if (current.gameObject is not null)
                m_forceOpenIds.Add(current.gameObject.identity.persistentId);
        }
    }

    private void ApplyDropResult(EditorDropResult result)
    {
        if (!result.accepted)
            return;
        if (result.revealTarget is GameScene scene)
        {
            m_forceOpenSceneIds.Add(scene.identity.persistentId);
        }
        else if (result.revealTarget is GameObject gameObject)
        {
            m_forceOpenIds.Add(gameObject.identity.persistentId);
        }
    }
}
