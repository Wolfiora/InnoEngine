using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal sealed class BuildCacheInventoryProgress(
    string workspace,
    bool readOnly
)
{
    internal static void PruneRetired(
        string workspace,
        IReadOnlyList<BuildCacheOwnerDescriptor> owners,
        CancellationToken cancellation
    ) {
        string root = PathBoundary.RequireUnlinkedPath(workspace, Path.Combine(workspace, "artifacts", "cache-owners", "inventory"));
        if (!Directory.Exists(root))
            return;
        // The retirement caller retains every selected publisher gate. Progress never authorizes payload removal.
        using FileLease writing = FileLease.AcquireAsync(Path.Combine(root, "progress.lock"), TimeSpan.Zero, cancellation)
            .AsTask().GetAwaiter().GetResult();
        foreach (string checkpoint in Directory.EnumerateFiles(root, "*.inventory").Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(PathBoundary.RequireUnlinkedPath(root, checkpoint)));
            JsonElement saved = document.RootElement;
            string relative = saved.GetProperty("directory").GetString() ?? throw new InvalidDataException("Inventory owner is absent.");
            var role = (BuildCacheArtifactRole)saved.GetProperty("role").GetInt32();
            string key = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(relative + "\0" + role)));
            if (Path.GetFileName(checkpoint) != key + ".inventory")
                throw new InvalidDataException("Inventory checkpoint does not match its registered path identity.");
            string directory = PathBoundary.Resolve(workspace, relative);
            if (!owners.Any(owner => owner.directory != directory && BuildCacheCleanupPlan.IsContained(owner.directory, directory))
                || Directory.Exists(directory))
                continue;
            File.Delete(checkpoint);
        }
    }

    internal long Measure(
        string directory,
        string metadata,
        string leasePath,
        BuildCacheArtifactRole role,
        CancellationToken cancellation
    ) {
        FileLease reading;
        try
        {
            reading = FileLease.AcquireSharedAsync(leasePath, TimeSpan.Zero, cancellation).AsTask().GetAwaiter().GetResult();
        }
        catch (TimeoutException)
        {
            // A live writer's changing size is never persisted as completed inventory.
            return BuildCacheFileInventory.Measure(directory, role, cancellation);
        }
        using (reading)
        {
            string root = PathBoundary.RequireUnlinkedPath(workspace, Path.Combine(workspace, "artifacts", "cache-owners", "inventory"));
            string identity = Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(PathBoundary.RequireUnlinkedPath(workspace, metadata))));
            string relative = Path.GetRelativePath(workspace, directory).Replace('\\', '/');
            string key = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(relative + "\0" + role)));
            string checkpoint = Path.Combine(root, key + ".inventory");
            if (File.Exists(checkpoint))
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(PathBoundary.RequireUnlinkedPath(workspace, checkpoint)));
                JsonElement saved = document.RootElement;
                if (saved.GetProperty("directory").GetString() != relative || saved.GetProperty("role").GetInt32() != (int)role
                    || saved.GetProperty("bytes").GetInt64() < 0)
                    throw new InvalidDataException("Cache inventory checkpoint has inconsistent ownership.");
                if (saved.GetProperty("identity").GetString() == identity)
                    return saved.GetProperty("bytes").GetInt64();
            }
            long bytes = BuildCacheFileInventory.Measure(directory, role, cancellation);
            cancellation.ThrowIfCancellationRequested();
            if (!readOnly)
            {
                // Only completed measurements are checkpoints. Dependencies, roots and retirement authority
                // are always read again from the publisher; the checkpoint never authorizes deletion.
                using FileLease writing = FileLease.AcquireAsync(Path.Combine(root, "progress.lock"), TimeSpan.Zero, cancellation)
                    .AsTask().GetAwaiter().GetResult();
                AtomicFile.WriteAllBytes(checkpoint, JsonSerializer.SerializeToUtf8Bytes(new { directory = relative, role = (int)role, identity, bytes }));
            }
            return bytes;
        }
    }
}
