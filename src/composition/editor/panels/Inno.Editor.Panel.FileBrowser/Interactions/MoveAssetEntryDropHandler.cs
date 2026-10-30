using System.Collections.Generic;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.FileBrowser;
/// <summary>
/// Moves directory drag payloads into File Browser directory targets.
/// </summary>
/// <param name="assets">
/// The assets used to initialize this instance.
/// </param>
[EditorDrop(FileBrowserInteractionIds.C_AREA, priority: 200)]
internal sealed class MoveAssetEntryDropHandler(AssetBrowserModule assets) : EditorDrop<AssetFileEntry, string>
{
    /// <summary>
    /// Evaluates the operation's current availability and presentation state.
    /// </summary>
    /// <returns>
    /// The validated editor drop status that represents the completed operation.
    /// </returns>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override EditorDropStatus Query(EditorDropContext<AssetFileEntry, string> context) => assets.edits.CanMoveSelection(
        SelectedSources(context),
        context.target
    ) ? EditorDropStatus.Accept() : EditorDropStatus.rejected;
    /// <summary>
    /// Validates and applies the current editor drag-and-drop interaction atomically.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated editor drop result that represents the completed operation.
    /// </returns>
    protected override EditorDropResult Drop(EditorDropContext<AssetFileEntry, string> context)
    {
        if (!Query(context).canDrop)
            return EditorDropResult.rejected;
        assets.edits.MoveSelection(SelectedSources(context), context.target);
        return EditorDropResult.Accepted(context.source, context.target);
    }

    private static IReadOnlyList<AssetFileEntry> SelectedSources(EditorDropContext<AssetFileEntry, string> context)
    {
        if (!context.interactions.selection.Contains(context.source))
            return[context.source];
        var values = new List<AssetFileEntry>();
        context.interactions.selection.CopyTargets(values);
        return values.Count == context.interactions.selection.count ? values : [];
    }
}
