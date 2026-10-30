using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Numerics;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;
internal static class FileBrowserUtility
{
    internal static AssetBrowserStyle CurrentBrowserStyle => new(EditorUI.current.widgets.style);

    internal readonly record struct BreadcrumbPart(
        string label,
        AssetBrowserRoot root,
        string directory
    );
    internal static void PushBrowserStyle()
    {
        Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.contentText);
        Ui.PushStyleColor((ImGuiCol.WindowBg), EditorPalette.collectionHeader);
        Ui.PushStyleColor((ImGuiCol.ChildBg), EditorPalette.collectionHeader);
        Ui.PushStyleColor((ImGuiCol.Border), EditorPalette.contentBorder);
        Ui.PushStyleColor((ImGuiCol.TableHeaderBg), EditorPalette.collectionHeader);
        Ui.PushStyleColor((ImGuiCol.TableBorderStrong), EditorPalette.contentBorder);
        Ui.PushStyleColor((ImGuiCol.TableBorderLight), EditorPalette.contentBorderSoft);
        Ui.PushStyleColor((ImGuiCol.TableRowBg), EditorPalette.collectionRow);
        Ui.PushStyleColor((ImGuiCol.TableRowBgAlt), EditorPalette.collectionRowAlternate);
        Ui.PushStyleColor((ImGuiCol.Header), EditorPalette.contentAccent);
        Ui.PushStyleColor((ImGuiCol.HeaderHovered), EditorPalette.contentAccent);
        Ui.PushStyleColor((ImGuiCol.HeaderActive), EditorPalette.contentAccent);
        Ui.PushStyleVar((ImGuiStyleVar.WindowPadding), CurrentBrowserStyle.windowPadding);
        Ui.PushStyleVar((ImGuiStyleVar.ChildBorderSize), EditorUI.current.widgets.style.borderSize);
        Ui.PushStyleVar((ImGuiStyleVar.CellPadding), CurrentBrowserStyle.cellPadding);
        Ui.PushStyleVar((ImGuiStyleVar.ItemSpacing), CurrentBrowserStyle.itemSpacing);
        Ui.PushStyleVar((ImGuiStyleVar.FrameRounding), CurrentBrowserStyle.frameRounding);
    }

    internal static void PopBrowserStyle()
    {
        Ui.PopStyleVar(5);
        Ui.PopStyleColor(12);
    }

    internal static void PushButtonColors(Vector4 color)
    {
        Ui.PushStyleColor((ImGuiCol.Button), color);
        Ui.PushStyleColor((ImGuiCol.ButtonHovered), EditorPalette.GetHovered(color));
        Ui.PushStyleColor((ImGuiCol.ButtonActive), EditorPalette.GetActive(color));
    }

    internal static bool IsDirectoryPath(
        AssetPipeline assets,
        string relativePath
    ) => assets.TryGetFileSystemEntry(AssetPath.Parse(relativePath), out AssetFileEntry entry) && entry.isDirectory;
    internal static string GetDirectoryLabel(
        AssetBrowserRoot root,
        string relativePath
    ) {
        if (root == AssetBrowserRoot.Plugins && string.IsNullOrEmpty(relativePath))
            return "Plugins";
        AssetPath path = AssetPath.Parse(NormalizePath(relativePath));
        if (string.IsNullOrEmpty(path.localPath))
            return GetSourceRootLabel(path.source);
        string name = Path.GetFileName(path.localPath);
        return string.IsNullOrEmpty(name) ? GetSourceRootLabel(path.source) : name;
    }

    internal static string GetSourceText(
        AssetFileEntry entry,
        string currentDirectory
    ) {
        AssetPath current = AssetPath.Parse(NormalizePath(currentDirectory));
        AssetPath entryPath = entry.assetPath;
        string directory = GetLocalParent(entryPath.localPath);
        string result;
        if (entryPath.source != current.source)
            result = FormatSourcePath(entryPath.source, directory);
        else if (string.Equals(directory, current.localPath, StringComparison.Ordinal))
            result = "~";
        else if (string.IsNullOrEmpty(current.localPath))
            result = string.IsNullOrEmpty(directory) ? "~" : $"~/{directory}";
        else
        {
            string prefix = current.localPath + "/";
            if (directory.StartsWith(prefix, StringComparison.Ordinal))
            {
                string relativeSource = directory[prefix.Length..];
                result = string.IsNullOrEmpty(relativeSource) ? "~" : $"~/{relativeSource}";
            }
            else
            {
                result = string.IsNullOrEmpty(directory) ? "~" : $"~/{directory}";
            }
        }

        return entry.isReadOnly ? $"{result} (read-only)" : result;
    }

    internal static string GetTypeText(AssetFileEntry entry)
    {
        if (entry.isDirectory && AssetSample.HasSampleDirectoryName(entry.assetPath))
            return AssetSample.fileType.TrimStart('.').ToUpperInvariant();
        if (entry.isDirectory)
            return "FOLDER";
        string extension = string.IsNullOrEmpty(entry.extension) ? Path.GetExtension(entry.assetPath.localPath) : entry.extension;
        return string.IsNullOrEmpty(extension) ? "FILE" : extension.TrimStart('.').ToUpperInvariant();
    }

    internal static IReadOnlyList<BreadcrumbPart> BuildBreadcrumbParts(
        AssetBrowserRoot root,
        string relativePath
    ) {
        if (root == AssetBrowserRoot.Plugins && string.IsNullOrEmpty(relativePath))
            return[new BreadcrumbPart("Plugins", AssetBrowserRoot.Plugins, string.Empty)];
        AssetPath sourcePath = AssetPath.Parse(NormalizePath(relativePath));
        string sourceRoot = new AssetPath(sourcePath.source, string.Empty).ToString();
        var parts = new List<BreadcrumbPart>();
        if (root == AssetBrowserRoot.Plugins)
        {
            parts.Add(new BreadcrumbPart("Plugins", AssetBrowserRoot.Plugins, string.Empty));
            parts.Add(new BreadcrumbPart(GetSourceRootLabel(sourcePath.source), AssetBrowserRoot.Plugins, sourceRoot));
        }
        else
        {
            parts.Add(new BreadcrumbPart("Assets", AssetBrowserRoot.Assets, sourceRoot));
        }

        if (string.IsNullOrEmpty(sourcePath.localPath))
            return parts;
        string[] segments = sourcePath.localPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        string path = string.Empty;
        for (int i = 0; i < segments.Length; i++)
        {
            path = string.IsNullOrEmpty(path) ? segments[i] : $"{path}/{segments[i]}";
            parts.Add(new BreadcrumbPart(segments[i], root, new AssetPath(sourcePath.source, path).ToString()));
        }

        return parts;
    }

    internal static bool IsReadOnlyLocation(
        AssetPipeline assets,
        AssetBrowserState browser
    ) => browser.root == AssetBrowserRoot.Plugins || IsReadOnlySource(assets, browser.currentDirectory);
    internal static string NormalizePath(string? path) => AssetPath.Parse(string.IsNullOrWhiteSpace(path) ? string.Empty : path.Replace('\\', '/').Trim()).ToString();
    internal static string GetParentDirectory(string relativePath)
    {
        AssetPath path = AssetPath.Parse(NormalizePath(relativePath));
        return new AssetPath(path.source, GetLocalParent(path.localPath)).ToString();
    }

    internal static bool IsReadOnlySource(
        AssetPipeline assets,
        string relativePath
    ) {
        AssetSourceId source = AssetPath.Parse(NormalizePath(relativePath)).source;
        for (int i = 0; i < assets.sourceMounts.Count; i++)
        {
            AssetSourceMount mount = assets.sourceMounts[i];
            if (mount.id == source)
                return mount.isReadOnly;
        }

        return true;
    }

    /// <summary>
    /// Gets the editable entry name while excluding a file's final extension.
    /// </summary>
    /// <param name="name">
    /// The final source path segment.
    /// </param>
    /// <param name="isDirectory">
    /// Whether the entry is a directory.
    /// </param>
    /// <returns>
    /// The complete directory name, or the file name without its final extension.
    /// </returns>
    internal static string GetEditableName(
        string name,
        bool isDirectory
    ) {
        if (isDirectory)
            return name;
        string extension = Path.GetExtension(name);
        return string.IsNullOrEmpty(extension) ? name : name[..^extension.Length];
    }

    /// <summary>
    /// Combines an edited entry name with the original file's final extension.
    /// </summary>
    /// <param name="sourcePath">
    /// The current normalized source-relative path.
    /// </param>
    /// <param name="editedName">
    /// The user-edited name that excludes the protected extension.
    /// </param>
    /// <param name="isDirectory">
    /// Whether the entry is a directory.
    /// </param>
    /// <returns>
    /// The renamed final path segment with the original final extension preserved for files.
    /// </returns>
    internal static string ComposeRenamedEntryName(
        string sourcePath,
        string editedName,
        bool isDirectory
    ) => isDirectory ? editedName : editedName + Path.GetExtension(sourcePath);
    internal static string[] FitTextToLines(
        string text,
        float maxWidth,
        int maxLines
    ) {
        ArgumentOutOfRangeException.ThrowIfLessThan(maxLines, 1);
        if (string.IsNullOrEmpty(text))
            return[string.Empty];
        List<string> elements = [];
        TextElementEnumerator enumerator = StringInfo.GetTextElementEnumerator(text);
        while (enumerator.MoveNext())
            elements.Add(enumerator.GetTextElement());
        List<string> lines = new(maxLines);
        int offset = 0;
        for (int lineIndex = 0; lineIndex < maxLines && offset < elements.Count; lineIndex++)
        {
            string remaining = string.Concat(elements.GetRange(offset, elements.Count - offset));
            if (Ui.MeasureText(remaining).X <= maxWidth)
            {
                lines.Add(remaining);
                break;
            }

            bool isLastLine = lineIndex == maxLines - 1;
            string suffix = isLastLine ? "..." : string.Empty;
            int count = 0;
            string candidate = string.Empty;
            while (offset + count < elements.Count)
            {
                string next = candidate + elements[offset + count];
                if (Ui.MeasureText(next + suffix).X > maxWidth)
                    break;
                candidate = next;
                count++;
            }

            if (count == 0)
            {
                lines.Add(isLastLine && Ui.MeasureText("...").X <= maxWidth ? "..." : elements[offset]);
                offset++;
                continue;
            }

            lines.Add(candidate + suffix);
            offset += count;
        }

        return lines.Count == 0 ? [string.Empty] : lines.ToArray();
    }

    internal static bool IsAncestorOrSelf(
        string candidateAncestor,
        string path
    ) {
        AssetPath ancestor = AssetPath.Parse(NormalizePath(candidateAncestor));
        AssetPath descendant = AssetPath.Parse(NormalizePath(path));
        if (ancestor.source != descendant.source)
            return false;
        if (ancestor.localPath.Length == 0 || string.Equals(ancestor.localPath, descendant.localPath, StringComparison.Ordinal))
            return true;
        return descendant.localPath.Length > ancestor.localPath.Length
            && descendant.localPath.StartsWith(
            ancestor.localPath,
            StringComparison.Ordinal
        )
            && descendant.localPath[ancestor.localPath.Length] == '/';
    }

    private static string GetSourceRootLabel(AssetSourceId source) => source == AssetSourceId.project ? "Assets" : source.value;
    private static string FormatSourcePath(
        AssetSourceId source,
        string localPath
    ) {
        string root = source == AssetSourceId.project ? GetSourceRootLabel(source) : $"Plugins/{GetSourceRootLabel(source)}";
        return string.IsNullOrEmpty(localPath) ? root : $"{root}/{localPath}";
    }

    private static string GetLocalParent(string localPath)
    {
        int separator = localPath.LastIndexOf('/');
        return separator < 0 ? string.Empty : localPath[..separator];
    }
}
