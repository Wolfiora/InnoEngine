using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Adapter.Content.FileSystem;

internal static class ContentCacheRetirement
{
    internal static async ValueTask<IReadOnlyList<string>> RetireAsync(
        string cache,
        string current
    ) {
        List<string> failures = [];
        string root = Path.Combine(cache, "generations");
        foreach (string directory in Directory.EnumerateDirectories(root))
        {
            string name = Path.GetFileName(directory);
            if (name == current)
                continue;
            try
            {
                if (!Guid.TryParseExact(name, "N", out _))
                    throw new IOException("An unrecognized directory cannot be retired as a cache generation.");
                PathBoundary.RequireUnlinkedPath(cache, directory);
                string lockPath = PathBoundary.RequireUnlinkedPath(cache, Path.Combine(cache, "leases", name + ".lock"));
                if (!File.Exists(lockPath))
                    throw new IOException("The generation lease is missing; existing reader ownership cannot be proven.");
                using FileLease lease = await FileLease.AcquireAsync(lockPath,
                    TimeSpan.Zero).ConfigureAwait(false);
                foreach (string file in PathBoundary.EnumerateFiles(directory))
                    _ = file;
                Directory.Delete(directory, recursive: true);
            }
            catch (TimeoutException)
            {
                // An existing reader owns this generation; a later preparation retries retirement.
            }
            catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
            {
                failures.Add($"Published content remains valid; retirement of generation '{name}' failed: {failure.Message}");
            }
        }
        return failures.AsReadOnly();
    }
}
