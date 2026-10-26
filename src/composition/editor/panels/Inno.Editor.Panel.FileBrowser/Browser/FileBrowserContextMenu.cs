using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;
internal sealed class FileBrowserContextMenu(
    AssetBrowserModule assets,
    FileBrowserRename rename
)
{
    internal void DrawEntry(
        EditorContext context,
        string id,
        string relativePath,
        FileBrowserPresentation presentation
    ) {
        if (!assets.edits.pipeline.TryGetFileSystemEntry(AssetPath.Parse(relativePath), out AssetFileEntry entry))
            return;
        if (Ui.IsItemClicked(ImGuiMouseButton.Right) && !assets.edits.interactions.selection.Contains(entry))
            assets.edits.interactions.SetSelection(entry);
        string creationDirectory = entry.isDirectory ? entry.assetPath.ToString() : GetParentDirectory(entry.assetPath.ToString());
        bool isOpen = EditorUI.current.menus.ContextMenu(id, CreateDirectoryInteraction(creationDirectory), assets.edits.interactions.For(FileBrowserInteractionIds.C_AREA, entry));
        if (isOpen)
            rename.MarkInteraction(presentation);
    }

    internal void DrawDirectory(
        EditorContext context,
        string id,
        string relativePath,
        FileBrowserPresentation presentation
    ) {
        if (IsReadOnlySource(assets.edits.pipeline, relativePath))
            return;
        bool isOpen = EditorUI.current.menus.ContextMenu(id, CreateDirectoryInteraction(relativePath));
        if (isOpen)
            rename.MarkInteraction(presentation);
    }

    internal void DrawBackground(
        EditorContext context,
        string id,
        FileBrowserPresentation presentation
    ) {
        if (IsReadOnlyLocation(assets.edits.pipeline, assets.browser))
            return;
        bool isOpen = EditorUI.current.menus.WindowContextMenu(id, CreateDirectoryInteraction(assets.browser.currentDirectory));
        if (isOpen)
            rename.MarkInteraction(presentation);
    }

    private Inno.Editor.Interactions.EditorInteraction CreateDirectoryInteraction(string relativePath) => assets.edits.interactions.For(
        FileBrowserInteractionIds.C_AREA,
        NormalizePath(relativePath)
    );
}
