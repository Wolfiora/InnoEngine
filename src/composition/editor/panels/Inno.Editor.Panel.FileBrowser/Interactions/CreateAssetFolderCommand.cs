using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using System;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Interactions;

namespace Inno.Editor.Panel.FileBrowser;
[EditorAction(FileBrowserInteractionIds.C_CREATE_FOLDER, FileBrowserInteractionIds.C_AREA)]
[EditorMenu(FileBrowserInteractionIds.C_AREA, "Create/Folder", order: 100)]
internal sealed class CreateAssetFolderCommand(AssetBrowserModule assets) : EditorAction<string>
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
    protected override EditorActionState Query(EditorActionContext<string> context) => assets.edits.pipeline.isInitialized
        && IsWritableDirectory(context.target) ? EditorActionState.enabled : EditorActionState.disabled;
    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="context">
    /// The operation scope that provides state, services, and ownership boundaries.
    /// </param>
    protected override void Execute(EditorActionContext<string> context)
    {
        AssetFileEntry target = assets.edits.CreateDirectory(AssetPath.Parse(context.target));
        EditorInteraction interaction = context.interactions.For(FileBrowserInteractionIds.C_AREA, target);
        _ = interaction.Select();
        _ = interaction.Execute(FileBrowserInteractionIds.C_RENAME);
    }

    private static string Combine(
        string parent,
        string name
    ) => string.IsNullOrEmpty(parent) ? name : $"{parent}/{name}";
    private static string Normalize(string? path) => string.IsNullOrWhiteSpace(path) ? string.Empty : path.Replace('\\', '/').Trim('/');
    private bool IsWritableDirectory(string relativePath)
    {
        string normalized = Normalize(relativePath);
        AssetSourceId source = AssetPath.Parse(normalized).source;
        AssetSourceMount? mount = assets.edits.pipeline.sourceMounts.FirstOrDefault(candidate => candidate.id == source);
        if (mount is null || mount.isReadOnly)
            return false;
        return string.IsNullOrEmpty(AssetPath.Parse(normalized).localPath)
            || assets.edits.pipeline.TryGetFileSystemEntry(
            AssetPath.Parse(normalized),
            out AssetFileEntry entry
        )
            && entry.isDirectory;
    }
}
