using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;

namespace Inno.Editor.Panel.Hierarchy;
[EditorDrop(HierarchyInteractionIds.C_AREA)]
internal sealed class MoveGameObjectToSceneDropHandler(SceneEdits edits) : EditorDrop<GameObject, HierarchySceneDropTarget>
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
    protected override EditorDropStatus Query(EditorDropContext<GameObject, HierarchySceneDropTarget> context)
    {
        GameObject[] sources = SelectedSources(context);
        GameScene target = context.target.scene;
        return sources.Length > 0
            && sources.All(value => value.isRuntimeValid && edits.CanEdit(value))
            && edits.CanEdit(target)
            && target is { isLoaded: true, isDestroyed: false } ? EditorDropStatus.Accept() : EditorDropStatus.rejected;
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
    protected override EditorDropResult Drop(EditorDropContext<GameObject, HierarchySceneDropTarget> context)
    {
        if (!Query(context).canDrop)
            return EditorDropResult.rejected;
        edits.MoveGameObjects(SelectedSources(context), context.target.scene);
        return EditorDropResult.Accepted(context.source, context.target);
    }

    private static GameObject[] SelectedSources(EditorDropContext<GameObject, HierarchySceneDropTarget> context)
    {
        if (!context.interactions.selection.Contains(context.source))
            return[context.source];
        var values = new List<GameObject>();
        context.interactions.selection.CopyTargets(values);
        return values.Count == context.interactions.selection.count ? values.ToArray() : [];
    }
}
