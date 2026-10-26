using Inno.Editor.ImGui;
using System;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Assets;
using Inno.Editor.Core;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;
using Inno.Editor.Settings;
using Inno.Extensibility.Types;

namespace Inno.Editor.Assets.ImGui;
/// <summary>
/// Owns asset navigation and icon presentation independently from source mutations.
/// </summary>
[EditorModule("asset-browser", order: 100)]
public sealed class AssetBrowserModule : EditorModule, IInspectionIconProvider<AssetFileEntry>
{
    private readonly EditorSettings m_settings;
    private readonly AssetIconRegistry m_icons;
    /// <summary>
    /// Creates the asset presentation module for the current editor generation.
    /// </summary>
    /// <param name="edits">
    /// The independent authoritative asset editing service.
    /// </param>
    /// <param name="interactions">
    /// The editor selection and action owner.
    /// </param>
    /// <param name="settings">
    /// The editor settings that supply semantic icon choices.
    /// </param>
    /// <param name="types">
    /// The active extension type catalog.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// A required owner is null.
    /// </exception>
    [Inno.Scripting.Api.ScriptingApiIgnore]
    public AssetBrowserModule(
        AssetEditingModule edits,
        EditorInteractions interactions,
        EditorSettings settings,
        TypeCatalog types
    ) {
        this.edits = edits ?? throw new ArgumentNullException(nameof(edits));
        m_settings = settings ?? throw new ArgumentNullException(nameof(settings));
        m_icons = new AssetIconRegistry(settings, types);
        browser = new AssetBrowserState(interactions, edits.pipeline);
    }

    /// <summary>
    /// Gets the source editing service; this module does not implement source transactions.
    /// </summary>
    public AssetEditingModule edits { get; }
    /// <summary>
    /// Gets navigation state; object selection belongs to the shared interaction owner.
    /// </summary>
    public AssetBrowserState browser { get; }
    /// <summary>
    /// Gets the configured folder glyph used by asset navigation.
    /// </summary>
    public string folderIcon => m_settings.Get("Editor/Appearance/Icons/Folder").GetAsString("value", Inno.Editor.ImGui.ImGuiIcon.Folder)!;

    /// <summary>
    /// Captures an immutable snapshot of the current observable state.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Capture(EditorState state)
    {
        state.Set("root", browser.root.ToString());
        state.Set("assetsDirectory", browser.GetDirectory(AssetBrowserRoot.Assets));
        state.Set("pluginsDirectory", browser.GetDirectory(AssetBrowserRoot.Plugins));
    }

    /// <summary>
    /// Restores the supplied snapshot while preserving current invariants.
    /// </summary>
    /// <param name="state">
    /// The lifecycle or domain state applied by this operation.
    /// </param>
    protected override void Restore(EditorState state)
    {
        AssetBrowserRoot restoredRoot = Enum.TryParse(state.Get("root", string.Empty), out AssetBrowserRoot parsedRoot) ? parsedRoot : AssetBrowserRoot.Assets;
        string assetsDirectory = RestoreDirectory(AssetBrowserRoot.Assets, state.Get("assetsDirectory", string.Empty));
        string pluginsDirectory = RestoreDirectory(AssetBrowserRoot.Plugins, state.Get("pluginsDirectory", string.Empty));
        browser.Restore(restoredRoot, assetsDirectory, pluginsDirectory);
    }

    private string RestoreDirectory(
        AssetBrowserRoot root,
        string path
    ) {
        string directory = AssetPath.Parse(path).ToString();
        if (string.IsNullOrEmpty(directory))
            return string.Empty;
        AssetPath isolated = AssetPath.Parse(directory);
        if ((root == AssetBrowserRoot.Assets) != (isolated.source == AssetSourceId.project))
            return string.Empty;
        while (!IsAvailableDirectory(directory))
        {
            string parent = ParentDirectory(directory);
            if (string.Equals(parent, directory, StringComparison.Ordinal))
                return string.Empty;
            directory = parent;
        }

        return directory;
    }

    private bool IsAvailableDirectory(string path) => edits.pipeline.TryGetFileSystemEntry(AssetPath.Parse(path), out AssetFileEntry entry) && entry.isDirectory;
    /// <summary>
    /// Resolves the presentation icon registered for an asset type or source extension.
    /// </summary>
    /// <param name="entry">
    /// The source entry whose presentation icon should be resolved.
    /// </param>
    /// <returns>
    /// The most specific registered icon, or the built-in directory or file icon when no
    /// registration matches the entry.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="entry"/> is <see langword="null"/>.
    /// </exception>
    public string GetIcon(AssetFileEntry entry)
    {
        ArgumentNullException.ThrowIfNull(entry);
        if (entry.isDirectory)
            return folderIcon;
        _ = edits.pipeline.TryGetAssetType(entry.assetPath, out Type? assetType);
        if (m_icons.TryResolve(assetType, entry.assetPath.ToString(), out string icon))
        {
            return icon;
        }

        return m_settings.Get("Editor/Appearance/Icons/File").GetAsString("value", Inno.Editor.ImGui.ImGuiIcon.File)!;
    }

    /// <inheritdoc/>
    protected override void OnDispose() => m_icons.Dispose();
    private static string ParentDirectory(string value)
    {
        AssetPath path = AssetPath.Parse(value);
        string local = path.localPath;
        int slash = local.LastIndexOf('/');
        return new AssetPath(path.source, slash < 0 ? string.Empty : local[..slash]).ToString();
    }
}
