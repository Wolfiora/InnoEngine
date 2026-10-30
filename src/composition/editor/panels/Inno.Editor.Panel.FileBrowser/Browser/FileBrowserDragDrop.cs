using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using System;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;
/// <summary>
/// Binds managed File Browser drag sessions to native entry and directory items.
/// </summary>
/// <param name="assets">
/// The assets used to initialize this instance.
/// </param>
internal sealed class FileBrowserDragDrop(AssetBrowserModule assets)
{
    /// <summary>
    /// Publishes the supplied source entry as a managed File Browser drag payload.
    /// </summary>
    /// <param name="context">
    /// The current Editor context.
    /// </param>
    /// <param name="entry">
    /// The source entry bound to the last native item.
    /// </param>
    internal void DrawAssetSource(
        EditorContext context,
        AssetFileEntry entry
    ) {
        if (!assets.edits.TryCreateContext(
            context,
            entry.assetPath.ToString(),
            out AssetEditorContext? assetContext
        )
            || assetContext is null
            || !assets.edits.TryCreateDragData(
            assetContext,
            out EditorDragData? data
        )
            || data is null)
            return;
        _ = EditorDragDropRenderer.Source(assets.edits.interactions.For(FileBrowserInteractionIds.C_AREA, entry), data, () => Ui.Text(data.label));
    }

    /// <summary>
    /// Accepts compatible File Browser payloads into a directory bound to the last native item.
    /// </summary>
    /// <param name="context">
    /// The current Editor context.
    /// </param>
    /// <param name="relativePath">
    /// The explicit target directory, or an empty string for the Assets root.
    /// </param>
    internal void DrawDirectoryTarget(
        EditorContext context,
        string relativePath
    ) {
        if (FileBrowserUtility.IsReadOnlySource(assets.edits.pipeline, relativePath))
            return;
        System.Numerics.Vector2 minimum = Ui.GetItemMinimum();
        System.Numerics.Vector2 maximum = Ui.GetItemMaximum();
        EditorDropWidgetResult result = EditorDragDropRenderer.Target(assets.edits.interactions.For(FileBrowserInteractionIds.C_AREA, relativePath));
        if (!result.isPreviewing || !result.status.canDrop)
            return;
        EditorUI.current.widgets.DropTargetHighlight(minimum, maximum);
    }
}
