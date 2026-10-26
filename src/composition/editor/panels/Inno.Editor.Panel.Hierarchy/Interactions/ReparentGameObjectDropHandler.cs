using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;
using Inno.Scene.Components;

namespace Inno.Editor.Panel.Hierarchy;
[EditorDrop(HierarchyInteractionIds.C_AREA)]
internal sealed class ReparentGameObjectDropHandler(SceneEdits edits) : EditorDrop<GameObject, HierarchyObjectDropTarget>
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor drop status that represents the completed operation.
    /// </returns>
    protected override EditorDropStatus Query(EditorDropContext<GameObject, HierarchyObjectDropTarget> context)
    {
        GameObject target = context.target.gameObject;
        GameObject[] sources = SelectedSources(context);
        if (sources.Length == 0 || !target.isRuntimeValid || !edits.CanEdit(target) || sources.Any(value => !value.isRuntimeValid || !edits.CanEdit(value)))
            return EditorDropStatus.rejected;
        Transform? parent = context.placement == EditorDropPlacement.Into ? target.transform : target.transform.parent;
        for (Transform? ancestor = parent; ancestor is not null; ancestor = ancestor.parent)
            if (sources.Any(value => ReferenceEquals(value.transform, ancestor)))
                return EditorDropStatus.rejected;
        if (sources.Contains(target))
            return EditorDropStatus.rejected;
        return EditorDropStatus.Accept(context.placement switch
        {
            EditorDropPlacement.Before => EditorDropVisual.InsertBefore,
            EditorDropPlacement.After => EditorDropVisual.InsertAfter,
            _ => EditorDropVisual.Highlight
        });
    }

    /// <summary>
    /// Validates and applies the current editor drag-and-drop interaction atomically.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor drop result that represents the completed operation.
    /// </returns>
    protected override EditorDropResult Drop(EditorDropContext<GameObject, HierarchyObjectDropTarget> context)
    {
        if (!Query(context).canDrop)
            return EditorDropResult.rejected;
        GameObject target = context.target.gameObject;
        GameObject[] sources = SelectedSources(context);
        Transform? parent = context.placement == EditorDropPlacement.Into ? target.transform : target.transform.parent;
        int insertionIndex = int.MaxValue;
        if (context.placement != EditorDropPlacement.Into)
        {
            insertionIndex = target.transform.siblingIndex;
            insertionIndex -= sources.Count(value => ReferenceEquals(value.transform.parent, parent) && ReferenceEquals(value.scene, target.scene) && value.transform.siblingIndex < target.transform.siblingIndex);
            if (context.placement == EditorDropPlacement.After)
                insertionIndex++;
        }

        edits.MoveGameObjects(sources, target.scene, parent?.gameObject, insertionIndex);
        return EditorDropResult.Accepted(context.source, context.target);
    }

    private static GameObject[] SelectedSources(EditorDropContext<GameObject, HierarchyObjectDropTarget> context)
    {
        if (!context.interactions.selection.Contains(context.source))
            return[context.source];
        var values = new List<GameObject>();
        context.interactions.selection.CopyTargets(values);
        return values.Count == context.interactions.selection.count ? values.ToArray() : [];
    }
}
