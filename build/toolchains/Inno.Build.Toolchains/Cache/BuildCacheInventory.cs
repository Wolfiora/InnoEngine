using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace Inno.Build.Toolchains;

internal sealed class BuildCacheInventory
{
    internal static readonly StringComparer pathComparer = OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;

    private BuildCacheInventory()
    {
    }

    internal Dictionary<string, (BuildCacheOwnerDescriptor owner, BuildCacheArtifactDescriptor artifact)> artifacts { get; } = new(pathComparer);
    internal List<string> roots { get; } = [];
    internal List<string> retirementOrder { get; } = [];

    internal static BuildCacheInventory Read(
        string workspace,
        IReadOnlyList<BuildCacheOwnerDescriptor> owners,
        CancellationToken cancellation
    ) {
        var result = new BuildCacheInventory();
        foreach (BuildCacheOwnerDescriptor owner in owners)
        {
            cancellation.ThrowIfCancellationRequested();
            BuildCacheCleanupPlan.RequireChild(workspace, owner.directory);
            BuildCacheCleanupPlan.RequireChild(owner.directory, owner.coordinationPath);
            foreach (BuildCacheArtifactDescriptor artifact in owner.ReadArtifacts(cancellation))
            {
                if (artifact is null)
                    throw new InvalidDataException($"Cache owner '{owner.id}' returned a null artifact.");
                BuildCacheCleanupPlan.RequireChild(owner.directory, artifact.directory);
                BuildCacheCleanupPlan.RequireChild(owner.directory, artifact.leasePath);
                if (BuildCacheCleanupPlan.IsContained(artifact.directory, owner.coordinationPath))
                    throw new InvalidDataException("An artifact cannot contain its owner's persistent coordination file.");
                if (!result.artifacts.TryAdd(artifact.directory, (owner, artifact)))
                    throw new InvalidDataException("A cache artifact has repeated ownership.");
            }
            foreach (string path in owner.ReadProtectedArtifacts(cancellation))
            {
                BuildCacheCleanupPlan.RequireChild(owner.directory, path);
                result.roots.Add(Path.TrimEndingDirectorySeparator(Path.GetFullPath(path)));
            }
        }
        bool HasArtifactAncestor(string path)
        {
            for (string? current = path; current is not null; current = Path.GetDirectoryName(current))
                if (result.artifacts.ContainsKey(current))
                    return true;
            return false;
        }
        foreach (string directory in result.artifacts.Keys)
            if (Path.GetDirectoryName(directory) is string parent && HasArtifactAncestor(parent))
                throw new InvalidDataException("Cache artifacts must have disjoint ownership boundaries.");
        var leasePaths = new HashSet<string>(pathComparer);
        foreach (var entry in result.artifacts.Values)
        {
            if (!leasePaths.Add(entry.artifact.leasePath)
                || owners.Any(owner => pathComparer.Equals(owner.coordinationPath, entry.artifact.leasePath))
                || HasArtifactAncestor(entry.artifact.leasePath))
                throw new InvalidDataException("Artifact leases must be distinct persistent files outside all retired directories.");
        }
        foreach (string root in result.roots)
            if (!result.artifacts.ContainsKey(root))
                throw new InvalidDataException($"Protected cache root '{root}' has no registered artifact owner.");
        var incoming = result.artifacts.Keys.ToDictionary(static path => path, static _ => 0, pathComparer);
        foreach (var entry in result.artifacts.Values)
            foreach (string dependency in entry.artifact.dependencies.Distinct(pathComparer))
            {
                if (!result.artifacts.ContainsKey(dependency))
                    throw new InvalidDataException($"Cache dependency '{dependency}' has no registered artifact owner.");
                incoming[dependency]++;
            }
        var ready = new Queue<string>(incoming.Where(static entry => entry.Value == 0)
            .OrderBy(entry => result.artifacts[entry.Key].artifact.lastUseUtc)
            .ThenBy(static entry => entry.Key, StringComparer.Ordinal).Select(static entry => entry.Key));
        while (ready.TryDequeue(out string? path))
        {
            cancellation.ThrowIfCancellationRequested();
            result.retirementOrder.Add(path);
            foreach (string dependency in result.artifacts[path].artifact.dependencies.Distinct(pathComparer))
                if (--incoming[dependency] == 0)
                    ready.Enqueue(dependency);
        }
        if (result.retirementOrder.Count != result.artifacts.Count)
            throw new ArgumentException("Cache dependency metadata must not contain cycles.");
        return result;
    }

    internal HashSet<string> Protect(IEnumerable<string> roots)
    {
        var protectedPaths = new HashSet<string>(pathComparer);
        var pending = new Stack<string>(roots);
        while (pending.TryPop(out string? path))
        {
            if (!artifacts.TryGetValue(path, out var entry))
                throw new InvalidDataException($"Protected cache path '{path}' has no registered artifact owner.");
            if (!protectedPaths.Add(path))
                continue;
            foreach (string dependency in entry.artifact.dependencies)
                pending.Push(dependency);
        }
        return protectedPaths;
    }
}
