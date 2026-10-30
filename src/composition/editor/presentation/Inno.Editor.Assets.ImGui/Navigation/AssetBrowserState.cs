using Inno.Editor.Assets;
using System;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Editor.Inspection;

namespace Inno.Editor.Assets.ImGui;
/// <summary>
/// Identifies the authoring or installed-content root displayed by the Asset Browser.
/// </summary>
public enum AssetBrowserRoot
{
    /// <summary>
    /// The writable project <c>Assets</c> authoring root.
    /// </summary>
    Assets,
    /// <summary>
    /// The read-only <c>Plugins</c> installation root.
    /// </summary>
    Plugins
}

/// <summary>
/// Stores asset browser navigation independently from global object selection.
/// </summary>
public sealed class AssetBrowserState
{
    private readonly EditorInteractions m_interactions;
    private readonly AssetPipeline m_assets;
    private string m_assetsDirectory = string.Empty;
    private string m_pluginsDirectory = string.Empty;
    /// <summary>
    /// Creates Asset Browser navigation state.
    /// </summary>
    /// <param name="interactions">
    /// The active editor interaction entry point.
    /// </param>
    /// <param name="assets">
    /// The isolated authoring asset pipeline resolved by selections.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="interactions"/> is <see langword="null"/>.
    /// </exception>
    internal AssetBrowserState(
        EditorInteractions interactions,
        AssetPipeline assets
    ) {
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
        m_assets = assets ?? throw new ArgumentNullException(nameof(assets));
    }

    /// <summary>
    /// Gets the root currently displayed by the Asset Browser.
    /// </summary>
    public AssetBrowserRoot root { get; private set; }
    /// <summary>
    /// Gets the current directory inside <see cref="root"/>. An empty value identifies that root's overview.
    /// </summary>
    public string currentDirectory => root == AssetBrowserRoot.Assets ? m_assetsDirectory : m_pluginsDirectory;
    /// <summary>
    /// Gets the most recently visited writable project directory, independently of the displayed root.
    /// </summary>
    public string projectDirectory => m_assetsDirectory;

    /// <summary>
    /// Gets the selected asset path when the editor-wide target belongs to the Asset Browser.
    /// </summary>
    /// <param name="context">
    /// The shared editor context containing the global selection.
    /// </param>
    /// <returns>
    /// The normalized source-relative path, or <see langword="null"/> when another target type is selected.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context"/> is <see langword="null"/>.
    /// </exception>
    public string? GetSelectedPath(EditorContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return (m_interactions.selection.selectedTarget as Inno.Assets.Pipeline.AssetFileEntry)?.assetPath.ToString();
    }

    /// <summary>
    /// Checks whether the source entry belongs to the current asset selection.
    /// </summary>
    /// <param name="context">
    /// The current editor callback context.
    /// </param>
    /// <param name="entry">
    /// The indexed source identity to inspect.
    /// </param>
    /// <returns>
    /// True when the entry is selected; otherwise false.
    /// </returns>
    public bool IsSelected(
        EditorContext context,
        AssetFileEntry entry
    ) => m_interactions.selection.Contains(entry);
    /// <summary>
    /// Adds or removes an asset from the current selection without retaining runtime asset objects.
    /// </summary>
    /// <param name="context">
    /// The current editor callback context.
    /// </param>
    /// <param name="entry">
    /// The indexed source identity to toggle.
    /// </param>
    public void ToggleSelection(
        EditorContext context,
        AssetFileEntry entry
    ) {
        m_interactions.ToggleSelection(entry);
    }

    /// <summary>
    /// Switches the displayed root while preserving the last directory visited in each root.
    /// </summary>
    /// <param name="value">
    /// The authoring or installed-content root to display.
    /// </param>
    public void SetRoot(AssetBrowserRoot value) => root = value;
    /// <summary>
    /// Sets the current Asset Browser directory and infers its root from the isolated source identity.
    /// </summary>
    /// <param name="relativePath">
    /// The isolated source-relative directory path, or an empty value for the currently displayed root.
    /// </param>
    public void SetCurrentDirectory(string relativePath)
    {
        string normalized = Normalize(relativePath);
        if (string.IsNullOrEmpty(normalized))
        {
            SetDirectory(root, string.Empty);
            return;
        }

        AssetPath path = AssetPath.Parse(normalized);
        AssetBrowserRoot targetRoot = path.source == AssetSourceId.project ? AssetBrowserRoot.Assets : AssetBrowserRoot.Plugins;
        SetDirectory(targetRoot, normalized);
        root = targetRoot;
    }

    /// <summary>
    /// Selects an asset path through the editor-wide selection state, or clears Asset selection.
    /// </summary>
    /// <param name="context">
    /// The shared editor context whose selection should be updated.
    /// </param>
    /// <param name="relativePath">
    /// The source-relative path to select, or an empty value to clear selection.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="context"/> is <see langword="null"/>.
    /// </exception>
    public void Select(
        EditorContext context,
        string? relativePath
    ) {
        ArgumentNullException.ThrowIfNull(context);
        object? target = null;
        if (!string.IsNullOrWhiteSpace(relativePath))
        {
            if (m_assets.TryGetFileSystemEntry(Inno.Assets.AssetPath.Parse(Normalize(relativePath)), out Inno.Assets.Pipeline.AssetFileEntry entry))
            {
                target = entry;
            }
        }

        m_interactions.SetSelection(target);
    }

    /// <summary>
    /// Gets the last visited isolated directory for a navigation root.
    /// </summary>
    /// <param name="targetRoot">
    /// The root whose independent navigation position is requested.
    /// </param>
    /// <returns>
    /// The source-relative directory, or an empty string for the root.
    /// </returns>
    public string GetDirectory(AssetBrowserRoot targetRoot) => targetRoot == AssetBrowserRoot.Assets ? m_assetsDirectory : m_pluginsDirectory;
    internal void Restore(
        AssetBrowserRoot restoredRoot,
        string assetsDirectory,
        string pluginsDirectory
    ) {
        m_assetsDirectory = Normalize(assetsDirectory);
        m_pluginsDirectory = Normalize(pluginsDirectory);
        root = restoredRoot;
    }

    /// <summary>
    /// Changes the displayed source root and its isolated navigation directory.
    /// </summary>
    /// <param name="targetRoot">
    /// The navigation root that owns the requested directory.
    /// </param>
    /// <param name="directory">
    /// The isolated source-relative directory; empty selects the root.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The directory belongs to a different source root.
    /// </exception>
    public void SetLocation(
        AssetBrowserRoot targetRoot,
        string directory
    ) {
        string normalized = Normalize(directory);
        if (!string.IsNullOrEmpty(normalized))
        {
            AssetPath path = AssetPath.Parse(normalized);
            bool isProject = path.source == AssetSourceId.project;
            if ((targetRoot == AssetBrowserRoot.Assets) != isProject)
            {
                throw new ArgumentException($"Directory '{directory}' does not belong to the '{targetRoot}' root.", nameof(directory));
            }
        }

        SetDirectory(targetRoot, normalized);
        root = targetRoot;
    }

    private void SetDirectory(
        AssetBrowserRoot targetRoot,
        string directory
    ) {
        if (targetRoot == AssetBrowserRoot.Assets)
            m_assetsDirectory = directory;
        else
            m_pluginsDirectory = directory;
    }

    private static string Normalize(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return string.Empty;
        return AssetPath.Parse(relativePath.Replace('\\', '/').Trim()).ToString();
    }
}
