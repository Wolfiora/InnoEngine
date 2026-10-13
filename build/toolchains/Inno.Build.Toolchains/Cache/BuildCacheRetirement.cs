using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

internal static class BuildCacheRetirement
{
    internal static async ValueTask<BuildCacheCleanupResult> ExecuteAsync(
        string workspace,
        IReadOnlyList<BuildCacheOwnerDescriptor> owners,
        BuildCachePolicy policy,
        IReadOnlyList<BuildCacheArtifactDescriptor> candidates,
        CancellationToken cancellation
    ) {
        var retired = new List<string>();
        var skipped = new HashSet<string>(BuildCacheInventory.pathComparer);
        var failures = new Dictionary<string, string>(BuildCacheInventory.pathComparer);
        var ownerLeases = new List<FileLease>();
        var artifactLeases = new Dictionary<string, FileLease>(BuildCacheInventory.pathComparer);
        var proposed = candidates.ToDictionary(static entry => entry.directory, BuildCacheInventory.pathComparer);
        if (proposed.Count == 0)
            return new([], [], failures, 0, false);
        long reclaimed = 0;
        Stopwatch elapsed = Stopwatch.StartNew();
        bool limited = false;
        using var bounded = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
        bounded.CancelAfter(policy.timeBudget);
        try
        {
            foreach (BuildCacheOwnerDescriptor owner in owners.OrderBy(static entry => entry.coordinationPath, StringComparer.Ordinal))
            {
                cancellation.ThrowIfCancellationRequested();
                if (elapsed.Elapsed >= policy.timeBudget)
                    return new([], proposed.Keys, failures, 0, true);
                try
                {
                    BuildCacheCleanupPlan.RequireChild(owner.directory, owner.coordinationPath);
                    ownerLeases.Add(await FileLease.AcquireAsync(owner.coordinationPath, TimeSpan.Zero, cancellation).ConfigureAwait(false));
                }
                catch (TimeoutException)
                {
                    // A changing owner may introduce cross-owner dependencies; defer the complete operation.
                    return new([], proposed.Keys, failures, 0, false);
                }
            }
            BuildCacheInventory fresh = BuildCacheInventory.Read(workspace, owners, bounded.Token);
            HashSet<string> protectedPaths = fresh.Protect(fresh.roots);
            protectedPaths.UnionWith(fresh.Protect(fresh.artifacts.Keys.Except(proposed.Keys, BuildCacheInventory.pathComparer)));
            var active = new List<string>();
            foreach (string path in fresh.artifacts.Keys.Where(path => proposed.ContainsKey(path) && !protectedPaths.Contains(path)).Order(StringComparer.Ordinal))
            {
                cancellation.ThrowIfCancellationRequested();
                if (elapsed.Elapsed >= policy.timeBudget)
                    return new([], proposed.Keys, failures, 0, true);
                try
                {
                    BuildCacheCleanupPlan.RequireChild(fresh.artifacts[path].owner.directory, path);
                    artifactLeases.Add(path, await FileLease.AcquireAsync(fresh.artifacts[path].artifact.leasePath,
                        TimeSpan.Zero, cancellation).ConfigureAwait(false));
                }
                catch (TimeoutException)
                {
                    active.Add(path);
                }
                catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
                {
                    failures[path] = failure.Message;
                    active.Add(path);
                }
            }
            protectedPaths.UnionWith(fresh.Protect(active));
            // An active parent keeps its dependencies available to the original owner.
            foreach (string path in protectedPaths)
                if (artifactLeases.Remove(path, out FileLease? lease))
                    lease.Dispose();
            foreach (string path in fresh.retirementOrder.Where(proposed.ContainsKey))
            {
                cancellation.ThrowIfCancellationRequested();
                if (elapsed.Elapsed >= policy.timeBudget)
                {
                    limited = true;
                    break;
                }
                var current = fresh.artifacts[path];
                BuildCacheArtifactDescriptor original = proposed[path];
                if (protectedPaths.Contains(path) || !artifactLeases.ContainsKey(path)
                    || original.bytes != current.artifact.bytes || original.lastUseUtc != current.artifact.lastUseUtc
                    || original.targetId != current.artifact.targetId
                    || !BuildCacheInventory.pathComparer.Equals(original.leasePath, current.artifact.leasePath)
                    || !original.dependencies.SequenceEqual(current.artifact.dependencies, BuildCacheInventory.pathComparer))
                {
                    skipped.Add(path);
                    continue;
                }
                try
                {
                    BuildCacheCleanupPlan.RequireChild(workspace, current.owner.directory);
                    BuildCacheCleanupPlan.RequireChild(current.owner.directory, path);
                    if (!Directory.Exists(path))
                    {
                        skipped.Add(path);
                        continue;
                    }
                    // Inspect every nested entry before recursive removal; do not follow junctions.
                    foreach (string file in PathBoundary.EnumerateFiles(path))
                        cancellation.ThrowIfCancellationRequested();
                    Directory.Delete(path, recursive: true);
                    reclaimed = checked(reclaimed + current.artifact.bytes);
                    retired.Add(path);
                }
                catch (Exception failure) when (failure is IOException or UnauthorizedAccessException)
                {
                    failures[path] = failure.Message;
                    protectedPaths.UnionWith(fresh.Protect([path]));
                }
            }
            skipped.UnionWith(proposed.Keys.Except(retired, BuildCacheInventory.pathComparer));
            return new(retired, skipped, failures, reclaimed, limited);
        }
        catch (OperationCanceledException) when (!cancellation.IsCancellationRequested)
        {
            return new(retired, proposed.Keys.Except(retired, BuildCacheInventory.pathComparer), failures, reclaimed, true);
        }
        finally
        {
            foreach (FileLease lease in artifactLeases.Values.Reverse())
                lease.Dispose();
            for (int index = ownerLeases.Count - 1; index >= 0; index--)
                ownerLeases[index].Dispose();
        }
    }
}
