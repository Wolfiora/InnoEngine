using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Freezes a reviewable retirement proposal for explicitly registered cache owners in one checkout.
/// </summary>
public sealed class BuildCacheCleanupPlan
{
    private readonly BuildCacheOwnerDescriptor[] m_owners;
    private readonly BuildCachePolicy m_policy;
    private readonly string m_workspace;
    private readonly Stopwatch m_elapsed;

    private BuildCacheCleanupPlan(
        string workspace,
        BuildCacheOwnerDescriptor[] owners,
        BuildCachePolicy policy,
        IReadOnlyList<BuildCacheArtifactDescriptor> candidates,
        long totalBytes,
        long protectedBytes,
        Stopwatch elapsed
    ) {
        m_workspace = workspace;
        m_owners = owners;
        m_policy = policy;
        m_elapsed = elapsed;
        this.candidates = candidates;
        this.totalBytes = totalBytes;
        this.protectedBytes = protectedBytes;
    }

    /// <summary>
    /// Gets the original eligible candidates shared by preview and execution.
    /// </summary>
    public IReadOnlyList<BuildCacheArtifactDescriptor> candidates { get; }

    /// <summary>
    /// Gets the measured total of explicitly inventoried artifact files.
    /// </summary>
    public long totalBytes { get; }

    /// <summary>
    /// Gets the size protected by current roots and their transitive dependencies when planning.
    /// </summary>
    public long protectedBytes { get; }

    /// <summary>
    /// Plans retirement by budget pressure or an explicit request to retire every unreferenced artifact.
    /// </summary>
    /// <param name="workspace">
    /// The absolute checkout boundary; no owner may manage it or its parent.
    /// </param>
    /// <param name="owners">
    /// Explicit registrations; callbacks remain borrowed until execution completes.
    /// </param>
    /// <param name="policy">
    /// The immutable size, free-space and duration budgets.
    /// </param>
    /// <param name="retireAll">
    /// Whether to propose all unreferenced generations regardless of size pressure.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels planning before any directory is removed.
    /// </param>
    /// <param name="targetId">
    /// An exact producer-declared target filter, or null to manage all registered scopes.
    /// Outside-scope generations and their dependencies remain protected.
    /// </param>
    /// <returns>
    /// A fixed proposal; execution rechecks ownership, roots and active readers before deletion.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Owners overlap, identities repeat, or dependency metadata forms a cycle.
    /// </exception>
    /// <exception cref="IOException">
    /// An owner, artifact or lease escapes its boundary or traverses a link.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// An inventory has ambiguous ownership or undeclared dependencies.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Planning was canceled.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Inventory exceeded the complete operation budget; no artifact was removed.
    /// </exception>
    public static BuildCacheCleanupPlan Create(
        string workspace,
        IReadOnlyList<BuildCacheOwnerDescriptor> owners,
        BuildCachePolicy policy,
        bool retireAll = false,
        string? targetId = null,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(policy);
        using var bounded = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        bounded.CancelAfter(policy.timeBudget);
        Stopwatch elapsed = Stopwatch.StartNew();
        try
        {
            BuildCacheCleanupPlan plan = CreateCore(workspace, owners, policy, retireAll, targetId, bounded.Token, elapsed);
            if (elapsed.Elapsed >= policy.timeBudget)
                throw new TimeoutException("Cache inventory exceeded its operation time budget; no artifact was retired.");
            return plan;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            throw new TimeoutException("Cache inventory exceeded its operation time budget; no artifact was retired.");
        }
    }

    private static BuildCacheCleanupPlan CreateCore(
        string workspace,
        IReadOnlyList<BuildCacheOwnerDescriptor> owners,
        BuildCachePolicy policy,
        bool retireAll,
        string? targetId,
        CancellationToken cancellationToken,
        Stopwatch elapsed
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(workspace);
        ArgumentNullException.ThrowIfNull(owners);
        ArgumentNullException.ThrowIfNull(policy);
        if (!Path.IsPathFullyQualified(workspace))
            throw new ArgumentException("Cache cleanup requires an absolute checkout boundary.", nameof(workspace));
        cancellationToken.ThrowIfCancellationRequested();
        string root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(workspace));
        BuildCacheOwnerDescriptor[] frozen = owners.ToArray();
        ValidateOwners(root, frozen);
        BuildCacheInventory inventory = BuildCacheInventory.Read(root, frozen, cancellationToken);
        HashSet<string> protectedPaths = inventory.Protect(inventory.roots);
        if (targetId is not null)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
            protectedPaths.UnionWith(inventory.Protect(inventory.artifacts.Values
                .Where(entry => entry.artifact.targetId != targetId).Select(static entry => entry.artifact.directory)));
        }
        long total = inventory.artifacts.Values.Sum(static entry => entry.artifact.bytes);
        long protectedSize = protectedPaths.Sum(path => inventory.artifacts[path].artifact.bytes);
        long free = new DriveInfo(Path.GetPathRoot(root)!).AvailableFreeSpace;
        long requested = retireAll ? long.MaxValue : Math.Max(Math.Max(0, total - policy.maximumBytes),
            Math.Max(0, policy.minimumFreeBytes - free));
        var selected = new List<BuildCacheArtifactDescriptor>();
        long proposed = 0;
        foreach (BuildCacheArtifactDescriptor artifact in inventory.artifacts.Values.Select(static entry => entry.artifact)
            .Where(entry => !protectedPaths.Contains(entry.directory))
            .OrderBy(static entry => entry.lastUseUtc).ThenBy(static entry => entry.directory, StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (proposed >= requested && artifact.role != BuildCacheArtifactRole.ProcessWorkingDirectory)
                continue;
            selected.Add(artifact);
            proposed = checked(proposed + artifact.bytes);
        }
        return new(root, frozen, policy, Array.AsReadOnly(selected.ToArray()), total, protectedSize, elapsed);
    }

    /// <summary>
    /// Executes the original proposal under fresh owner and artifact leases without waiting for active readers.
    /// </summary>
    /// <param name="dryRun">
    /// Whether to report the proposal without acquiring mutation leases or deleting files.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels remaining work; successful earlier retirements are not rolled back.
    /// </param>
    /// <returns>
    /// Actual retirement, activity skips and IO failures; protected generations are never forced out.
    /// </returns>
    /// <exception cref="InvalidDataException">
    /// Fresh ownership metadata violates the registered boundary.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The caller canceled; no later candidate is processed.
    /// </exception>
    public ValueTask<BuildCacheCleanupResult> ExecuteAsync(
        bool dryRun = false,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        TimeSpan remaining = m_policy.timeBudget - m_elapsed.Elapsed;
        if (!dryRun && remaining <= TimeSpan.Zero)
            return ValueTask.FromResult(new BuildCacheCleanupResult([], candidates.Select(static entry => entry.directory),
                new Dictionary<string, string>(), 0, true));
        return dryRun
            ? ValueTask.FromResult(new BuildCacheCleanupResult([], candidates.Select(static entry => entry.directory),
                new Dictionary<string, string>(), 0, false))
            : BuildCacheRetirement.ExecuteAsync(m_workspace, m_owners,
                new BuildCachePolicy(m_policy.maximumBytes, m_policy.minimumFreeBytes, remaining), candidates, cancellationToken);
    }

    private static void ValidateOwners(
        string workspace,
        IReadOnlyList<BuildCacheOwnerDescriptor> owners
    ) {
        var identities = new HashSet<string>(StringComparer.Ordinal);
        for (int index = 0; index < owners.Count; index++)
        {
            BuildCacheOwnerDescriptor owner = owners[index] ?? throw new ArgumentException("A cache owner is null.", nameof(owners));
            if (!identities.Add(owner.id))
                throw new ArgumentException("Cache owner identities must be unique.", nameof(owners));
            RequireChild(workspace, owner.directory);
            RequireChild(owner.directory, owner.coordinationPath);
            for (int previous = 0; previous < index; previous++)
                if (IsContained(owners[previous].directory, owner.directory) || IsContained(owner.directory, owners[previous].directory))
                    throw new ArgumentException("Registered cache boundaries must not overlap.", nameof(owners));
        }
    }

    internal static void RequireChild(
        string root,
        string path
    ) {
        if (!Path.IsPathFullyQualified(path) || !IsContained(root, path)
            || BuildCacheInventory.pathComparer.Equals(Path.TrimEndingDirectorySeparator(Path.GetFullPath(root)),
                Path.TrimEndingDirectorySeparator(Path.GetFullPath(path))))
            throw new IOException($"Cache path '{path}' is not a child of owner '{root}'.");
        PathBoundary.RequireUnlinkedPath(root, path);
    }

    internal static bool IsContained(
        string root,
        string path
    ) {
        string relative = Path.GetRelativePath(root, path);
        return !Path.IsPathRooted(relative) && relative != ".."
            && !relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal);
    }
}
