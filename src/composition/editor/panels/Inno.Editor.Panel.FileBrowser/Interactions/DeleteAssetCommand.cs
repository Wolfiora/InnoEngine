using System.Collections.Generic;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;
using Inno.Core.Input;

namespace Inno.Editor.Panel.FileBrowser;
[EditorAction(FileBrowserInteractionIds.C_DELETE, priority: 100)]
[EditorMenu(FileBrowserInteractionIds.C_AREA, "Delete", order: 200)]
[EditorShortcut(FileBrowserInteractionIds.C_AREA, KeyCode.Delete)]
internal sealed class DeleteAssetCommand(AssetBrowserModule assets) : EditorAction<AssetFileEntry>
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
    protected override EditorActionState Query(EditorActionContext<AssetFileEntry> context)
    {
        AssetOperationValidation validation = assets.edits.ValidateDeleteSelection(context.editor, Targets(context));
        return validation.isValid ? EditorActionState.enabled : new EditorActionState(true, false, unavailableReason: validation.message);
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<AssetFileEntry> context)
    {
        assets.edits.DeleteSelection(context.editor, Targets(context));
        context.interactions.SetSelection((object? )null);
    }

    private static IReadOnlyList<AssetFileEntry> Targets(EditorActionContext<AssetFileEntry> context)
    {
        if (!context.interactions.selection.Contains(context.target))
            return[context.target];
        var values = new List<AssetFileEntry>();
        context.interactions.selection.CopyTargets(values);
        return values.Count == context.interactions.selection.count ? values : [];
    }
}
