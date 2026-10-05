using System;
using System.IO;
using System.Collections.Generic;

namespace Inno.Core.IO;

/// <summary>
/// Resolves paths while enforcing an explicit filesystem ownership boundary.
/// </summary>
public static class PathBoundary
{
    /// <summary>
    /// Resolves a relative path beneath a root and rejects traversal outside that root.
    /// </summary>
    /// <param name="root">
    /// The owning root directory.
    /// </param>
    /// <param name="relativePath">
    /// The relative path to resolve.
    /// </param>
    /// <returns>
    /// The normalized absolute contained path.
    /// </returns>
    public static string Resolve(
        string root,
        string relativePath
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        ArgumentNullException.ThrowIfNull(relativePath);
        if (Path.IsPathRooted(relativePath))
            throw new ArgumentException("A contained path must be relative.", nameof(relativePath));
        string normalizedRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        string candidate = Path.GetFullPath(relativePath.Length == 0 ? "." : relativePath, normalizedRoot);
        EnsureContains(normalizedRoot, candidate);
        return candidate;
    }

    /// <summary>
    /// Validates and normalizes an absolute path beneath a root.
    /// </summary>
    /// <param name="root">
    /// The owning root directory.
    /// </param>
    /// <param name="path">
    /// The path to validate.
    /// </param>
    /// <returns>
    /// The normalized absolute contained path.
    /// </returns>
    public static string RequireContained(
        string root,
        string path
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string normalizedRoot = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        string candidate = Path.GetFullPath(path);
        EnsureContains(normalizedRoot, candidate);
        return candidate;
    }

    /// <summary>
    /// Enumerates regular files in an owned tree without following filesystem links.
    /// </summary>
    /// <param name="root">
    /// The existing physical root whose files and subdirectories belong to the caller.
    /// </param>
    /// <returns>
    /// Absolute file paths in unspecified order; enumeration does not retain open file handles.
    /// </returns>
    /// <exception cref="IOException">
    /// The root or any entry is a symbolic link, junction or other reparse point, or cannot be inspected.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The root does not exist.
    /// </exception>
    public static IEnumerable<string> EnumerateFiles(string root)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        string normalizedRoot = Path.GetFullPath(root);
        if (!Directory.Exists(normalizedRoot))
            throw new DirectoryNotFoundException($"Owned directory '{normalizedRoot}' does not exist.");
        var pending = new Stack<string>();
        pending.Push(normalizedRoot);
        while (pending.Count > 0)
        {
            string directory = pending.Pop();
            RequireRegularEntry(directory);
            foreach (string entry in Directory.EnumerateFileSystemEntries(directory))
            {
                FileAttributes attributes = RequireRegularEntry(entry);
                if ((attributes & FileAttributes.Directory) != 0)
                    pending.Push(entry);
                else
                    yield return entry;
            }
        }
    }

    private static FileAttributes RequireRegularEntry(string path)
    {
        FileAttributes attributes = File.GetAttributes(path);
        if ((attributes & FileAttributes.ReparsePoint) != 0)
            throw new IOException($"Owned filesystem entry '{path}' cannot be a link or reparse point.");
        return attributes;
    }

    private static void EnsureContains(
        string root,
        string candidate
    ) {
        string prefix = Path.EndsInDirectorySeparator(root) ? root : root + Path.DirectorySeparatorChar;
        StringComparison comparison = OperatingSystem.IsWindows() || OperatingSystem.IsMacOS()
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (!string.Equals(candidate, root, comparison)
            && !candidate.StartsWith(prefix, comparison))
        {
            throw new IOException($"Path '{candidate}' escapes filesystem boundary '{root}'.");
        }
    }
}
