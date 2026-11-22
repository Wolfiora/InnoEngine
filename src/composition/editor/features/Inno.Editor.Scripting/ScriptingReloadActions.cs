using Inno.Editor.Interactions;

namespace Inno.Editor.Scripting;

[EditorAction(ScriptingInteractionIDs.C_RELOAD_SCRIPTING, ScriptingInteractionIDs.C_MAIN_MENU_AREA)]
[EditorMenu(ScriptingInteractionIDs.C_MAIN_MENU_AREA, "Tools/Scripting/Reload Scripts", order: 100)]
internal sealed class ReloadScriptingAction(EditorScripting scripting) : EditorAction
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
    protected override EditorActionState Query(EditorActionContext context)
        => scripting.canReload ? EditorActionState.enabled : EditorActionState.disabled;

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext context) => scripting.ReloadScripting();
}

[EditorAction(ScriptingInteractionIDs.C_RELOAD_PLUGINS, ScriptingInteractionIDs.C_MAIN_MENU_AREA)]
[EditorMenu(ScriptingInteractionIDs.C_MAIN_MENU_AREA, "Tools/Scripting/Reload Plugins", order: 120)]
internal sealed class ReloadPluginsAction(EditorScripting scripting) : EditorAction
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
    protected override EditorActionState Query(EditorActionContext context)
        => scripting.canReload ? EditorActionState.enabled : EditorActionState.disabled;

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext context) => scripting.ReloadPlugins();
}
