using System;
using System.Collections.Generic;
using Inno.Core.Identity;
using Inno.Editor.Core;
using Inno.Scene;
using Inno.Scene.Components;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Hierarchy;

internal sealed partial class HierarchyPanel
{
    private readonly record struct HierarchyRow(
        RuntimeIdentity identity,
        int depth
    );

    private readonly List<HierarchyRow> m_rows = [];
    private readonly List<(RuntimeIdentity identity, ulong revision)> m_sceneInputs = [];
    private ulong m_expansionRevision = ulong.MaxValue;

    private void RefreshRows(IReadOnlyList<GameScene> scenes)
    {
        var widgets = EditorUI.current.widgets;
        foreach (GameScene scene in scenes)
            if (m_initializedSceneIds.Add(scene.identity.persistentId))
                widgets.SetTreeNodeOpen("scene_" + scene.identity.persistentId.ToString("N"), true);
        foreach (Guid id in m_forceOpenSceneIds)
            widgets.SetTreeNodeOpen("scene_" + id.ToString("N"), true);
        foreach (Guid id in m_forceOpenIds)
            widgets.SetTreeNodeOpen("hierarchy_" + id.ToString("N"), true);
        m_forceOpenSceneIds.Clear();
        m_forceOpenIds.Clear();

        bool changed = m_expansionRevision != widgets.treeExpansionRevision || m_sceneInputs.Count != scenes.Count;
        for (int index = 0; !changed && index < scenes.Count; index++)
            changed = m_sceneInputs[index] != (RequireIdentity(scenes[index]), scenes[index].hierarchyRevision);
        if (!changed)
            return;

        var rows = new List<HierarchyRow>();
        var inputs = new List<(RuntimeIdentity, ulong)>();
        var pending = new Stack<(GameObject target, int depth)>();
        var visited = new HashSet<RuntimeIdentity>();
        foreach (GameScene scene in scenes)
        {
            RuntimeIdentity sceneId = RequireIdentity(scene);
            inputs.Add((sceneId, scene.hierarchyRevision));
            rows.Add(new(sceneId, 0));
            if (!widgets.IsTreeNodeOpen("scene_" + scene.identity.persistentId.ToString("N")))
                continue;
            IReadOnlyList<GameObject> roots = m_selection.GetRootObjects(scene);
            for (int index = roots.Count - 1; index >= 0; index--)
                pending.Push((roots[index], 1));
            while (pending.TryPop(out var next))
            {
                if (!next.target.isRuntimeValid)
                    continue;
                RuntimeIdentity identity = RequireIdentity(next.target);
                if (!visited.Add(identity))
                    throw new InvalidOperationException("A hierarchy view encountered a duplicate object identity.");
                rows.Add(new(identity, next.depth));
                if (!widgets.IsTreeNodeOpen("hierarchy_" + next.target.identity.persistentId.ToString("N")))
                    continue;
                IReadOnlyList<Transform> children = next.target.transform.children;
                for (int index = children.Count - 1; index >= 0; index--)
                    pending.Push((children[index].gameObject, next.depth + 1));
            }
        }

        m_rows.Clear();
        m_rows.AddRange(rows);
        m_sceneInputs.Clear();
        m_sceneInputs.AddRange(inputs);
        m_visibleSelection.Clear();
        foreach (HierarchyRow row in rows)
            m_visibleSelection.Add(row.identity);
        m_expansionRevision = widgets.treeExpansionRevision;
    }

    private void DrawRows(EditorContext context)
    {
        RuntimeIdentity? selected = (m_interactions.selection.selectedTarget as IdentityObject)?.identity.runtimeIdentity;
        RuntimeIdentity? dragging = m_interactions.TryGetActiveDragIdentity(out RuntimeIdentity source) ? source : null;
        Span<int> preserved = stackalloc int[3];
        int count = 0;
        for (int index = 0; index < m_rows.Count; index++)
            if (m_rows[index].identity == selected || m_rows[index].identity == dragging || m_rows[index].identity == m_drawKeyboardFocus)
                preserved[count++] = index;
        EditorUI.current.widgets.ClippedRows(m_rows.Count, -1f, (panel: this, context), static (
            state,
            start,
            end
        ) =>
        {
            for (int index = start; index < end; index++)
            {
                state.panel.m_visibleRowIndex = index;
                HierarchyRow row = state.panel.m_rows[index];
                if (state.panel.m_interactions.TryResolveIdentity(row.identity, out IdentityObject? target))
                {
                    if (target is GameScene scene)
                        state.panel.DrawScene(state.context, scene, row.depth);
                    else if (target is GameObject gameObject)
                        state.panel.DrawObject(state.context, gameObject, row.depth);
                    else
                        Ui.Dummy(new(1f, Ui.GetFrameHeight()));
                }
                else
                    Ui.Dummy(new(1f, Ui.GetFrameHeight()));
            }
        }, preserved[..count]);
    }

    private static RuntimeIdentity RequireIdentity(IdentityObject target)
        => target.identity.runtimeIdentity ?? throw new InvalidOperationException("A visible hierarchy row requires a registered identity.");
}
