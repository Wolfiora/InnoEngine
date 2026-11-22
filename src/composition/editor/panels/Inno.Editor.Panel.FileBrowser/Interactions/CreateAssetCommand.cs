using Inno.Assets.Pipeline;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.FileBrowser;

[EditorAction(FileBrowserInteractionIDs.C_CREATE_ASSET, FileBrowserInteractionIDs.C_AREA)]
internal sealed class CreateAssetCommand(AssetBrowserModule assets) : EditorAction<string, string>
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
    protected override EditorActionState Query(EditorActionContext<string, string> context)
        => assets.edits.CanCreateAsset(
            context.target,
            context.argument
        ) ? EditorActionState.enabled : EditorActionState.disabled;

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<string, string> context)
    {
        AssetFileEntry created = assets.edits.CreateAsset(context.target, context.argument);
        _ = context.interactions.For(FileBrowserInteractionIDs.C_AREA, created).Select();
        _ = context.interactions.For(FileBrowserInteractionIDs.C_AREA, created).Execute(FileBrowserInteractionIDs.C_RENAME);
        context.interactions.OpenPanel("scene.inspector");
    }
}
