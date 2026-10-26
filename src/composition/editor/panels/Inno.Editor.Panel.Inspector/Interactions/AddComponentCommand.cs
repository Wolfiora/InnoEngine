using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.Editor.Interactions;
using Inno.Editor.Scene;
using Inno.Scene;

namespace Inno.Editor.Panel.Inspector;
[EditorAction(InspectorInteractionIds.C_ADD_COMPONENT, InspectorInteractionIds.C_COMPONENT_AREA)]
internal sealed class AddComponentCommand(
    SceneEdits edits,
    TypeCatalog types
) : EditorAction<GameObject, TypeRef>
{
    /// <summary>
    /// Evaluates whether the requested change can be applied to the current generation.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected override EditorActionState Query(EditorActionContext<GameObject, TypeRef> context) => Targets(context).Count > 0
        && Targets(context).All(value => value.isRuntimeValid && edits.CanEdit(value))
        && context.argument.IsValid(types) ? EditorActionState.enabled : EditorActionState.hidden;
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<GameObject, TypeRef> context)
    {
        using EditorHistoryTransaction transaction = context.interactions.history.BeginTransaction("Add Components");
        foreach (GameObject target in Targets(context))
            _ = edits.AddComponent(target, context.argument.Resolve(types));
        transaction.Commit();
    }

    private static IReadOnlyList<GameObject> Targets(EditorActionContext<GameObject, TypeRef> context)
    {
        if (!context.interactions.selection.Contains(context.target))
            return[context.target];
        var targets = new List<GameObject>();
        context.interactions.selection.CopyTargets(targets);
        return targets.Count == context.interactions.selection.count ? targets : [];
    }
}
