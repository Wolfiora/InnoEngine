using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class BuildCacheFileInventory
{
    internal static long Measure(
        string directory,
        BuildCacheArtifactRole role,
        CancellationToken cancellation
    ) {
        PathBoundary.RequireUnlinkedPath(directory, directory);
        long bytes = 0;
        var pending = new Stack<string>();
        pending.Push(directory);
        while (pending.TryPop(out string? current))
        {
            cancellation.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(directory, current);
            foreach (string entry in Directory.EnumerateFileSystemEntries(current))
            {
                cancellation.ThrowIfCancellationRequested();
                FileAttributes attributes = File.GetAttributes(entry);
                if ((attributes & FileAttributes.ReparsePoint) != 0)
                {
                    if (role != BuildCacheArtifactRole.ReusableWorkspace || (attributes & FileAttributes.Directory) != 0)
                        throw new IOException($"Cache entry '{entry}' cannot contain this filesystem link.");
                    ValidateFileLink(directory, entry, cancellation);
                    // A compiler's library-name alias owns no second copy of its target's bytes.
                    continue;
                }
                if ((attributes & FileAttributes.Directory) != 0)
                    pending.Push(entry);
                else
                    bytes = checked(bytes + new FileInfo(entry).Length);
            }
        }
        return bytes;
    }

    private static void ValidateFileLink(
        string directory,
        string entry,
        CancellationToken cancellation
    ) {
        var visited = new HashSet<string>(StringComparer.Ordinal);
        while (true)
        {
            cancellation.ThrowIfCancellationRequested();
            entry = PathBoundary.RequireContained(directory, entry);
            PathBoundary.RequireUnlinkedPath(directory, Path.GetDirectoryName(entry)!);
            if (!visited.Add(entry))
                throw new IOException($"Workspace file link '{entry}' forms a cycle.");
            var file = new FileInfo(entry);
            string? target = file.LinkTarget;
            if (target is null)
            {
                PathBoundary.RequireUnlinkedPath(directory, entry);
                if (!file.Exists || (file.Attributes & FileAttributes.Directory) != 0)
                    throw new IOException($"Workspace file link '{entry}' has no regular owned target.");
                return;
            }
            entry = Path.GetFullPath(target, Path.GetDirectoryName(entry)!);
        }
    }
}
