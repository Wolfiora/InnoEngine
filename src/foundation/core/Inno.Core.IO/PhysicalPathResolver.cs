using System;
using System.Collections.Generic;
using System.IO;

namespace Inno.Core.IO;

/// <summary>
/// Resolves filesystem aliases within one caller-owned reading phase without granting write authority.
/// </summary>
/// <remarks>
/// Instances are not thread-safe. Create a fresh instance when validating filesystem changes.
/// Resolved paths still require an explicit PathBoundary check before ownership or mutation.
/// </remarks>
public sealed class PhysicalPathResolver
{
    private readonly Dictionary<string, string> m_paths;
    private readonly HashSet<string> m_resolving;

    /// <summary>
    /// Creates a phase-local resolver with the caller's known path-comparison semantics.
    /// </summary>
    /// <param name="comparer">
    /// The known filesystem comparison, or null to retain ordinal spelling without assuming volume semantics.
    /// </param>
    public PhysicalPathResolver(StringComparer? comparer = null)
    {
        m_paths = new(comparer ?? StringComparer.Ordinal);
        m_resolving = new(comparer ?? StringComparer.Ordinal);
    }

    /// <summary>
    /// Resolves existing link components and retains their observed physical spelling for this phase.
    /// </summary>
    /// <param name="path">
    /// The file or directory to inspect; relative paths use the current working directory.
    /// </param>
    /// <returns>
    /// The observed physical path, without creating files or relaxing an ownership boundary.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The path is empty or invalid.
    /// </exception>
    /// <exception cref="FileNotFoundException">
    /// A required filesystem entry is missing.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// A required parent directory is missing.
    /// </exception>
    /// <exception cref="IOException">
    /// A link is cyclic or cannot be resolved.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// Filesystem metadata or a link target cannot be read with the current permissions.
    /// </exception>
    public string Resolve(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        return ResolveCore(Path.TrimEndingDirectorySeparator(Path.GetFullPath(path)));
    }

    private string ResolveCore(string path)
    {
        if (m_paths.TryGetValue(path, out string? cached))
            return cached;
        if (!m_resolving.Add(path))
            throw new IOException($"Filesystem alias '{path}' forms a resolution cycle.");
        try
        {
            string? parent = Path.GetDirectoryName(path);
            if (parent is null)
            {
                _ = File.GetAttributes(path);
                m_paths.Add(path, path);
                return path;
            }
            string candidate = Path.Combine(ResolveCore(parent), Path.GetFileName(path));
            FileAttributes attributes = File.GetAttributes(candidate);
            string resolved = candidate;
            if ((attributes & FileAttributes.ReparsePoint) != 0)
            {
                FileSystemInfo entry = (attributes & FileAttributes.Directory) != 0
                    ? new DirectoryInfo(candidate) : new FileInfo(candidate);
                resolved = ResolveCore(Path.GetFullPath(entry.ResolveLinkTarget(returnFinalTarget: true)?.FullName
                    ?? throw new IOException($"Filesystem alias '{candidate}' has no resolvable target.")));
            }
            m_paths.Add(path, resolved);
            return resolved;
        }
        finally
        {
            m_resolving.Remove(path);
        }
    }
}
