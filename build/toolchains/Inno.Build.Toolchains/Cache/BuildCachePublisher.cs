using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Records exact publisher-owned generations and coordinates them with bounded cache retirement.
/// </summary>
public sealed class BuildCachePublisher
{
    private readonly string m_root;
    private readonly BuildCacheArtifactRole m_role;

    /// <summary>
    /// Creates a publisher for one absolute, non-overlapping cache boundary.
    /// </summary>
    /// <param name="directory">
    /// The cache root exclusively controlled by this publisher.
    /// </param>
    /// <param name="role">
    /// The lifetime declared for this publisher's completed entries.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The directory is blank or relative.
    /// </exception>
    public BuildCachePublisher(
        string directory,
        BuildCacheArtifactRole role
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        if (!Path.IsPathFullyQualified(directory))
            throw new ArgumentException("A cache publisher requires an absolute root.", nameof(directory));
        m_root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
        if (!Enum.IsDefined(role))
            throw new ArgumentOutOfRangeException(nameof(role));
        m_role = role;
    }

    /// <summary>
    /// Gets the declared lifecycle used by this publisher's inventory and maintenance registration.
    /// </summary>
    public BuildCacheArtifactRole role => m_role;

    /// <summary>
    /// Pins publisher activity until publication, reading handoff and current-root updates finish.
    /// </summary>
    /// <param name="cancellationToken">
    /// Cancels this wait without disturbing other owners.
    /// </param>
    /// <returns>
    /// Caller-owned shared activity; dispose after the final artifact reading lease is acquired.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// Acquisition was canceled.
    /// </exception>
    public async ValueTask<FileLease> AcquireUseAsync(CancellationToken cancellationToken = default)
    {
        string path = Path.Combine(m_root, "owner.lock");
        if (!File.Exists(path))
            using (await FileLease.AcquireAsync(path, TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false)) { }
        return await FileLease.AcquireSharedAsync(path, TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Records a completed generation and optionally selects it as a protected current root.
    /// The caller must hold this publisher's activity lease and the artifact's producer or reading lease.
    /// </summary>
    /// <param name="directory">
    /// An existing artifact within this publisher's exact boundary.
    /// </param>
    /// <param name="leasePath">
    /// The persistent lease used by its producer and readers.
    /// </param>
    /// <param name="dependencies">
    /// Exact immutable generations required while this generation is protected.
    /// </param>
    /// <param name="currentKey">
    /// A product/target/configuration root identity, or null for an unselected generation.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels before metadata selection.
    /// </param>
    /// <param name="targetId">
    /// The producer's product target, or null for a shared host generation.
    /// </param>
    /// <exception cref="IOException">
    /// An owned path escapes the boundary, crosses a directory link, or contains an invalid file link.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The completed generation is absent.
    /// </exception>
    /// <remarks>
    /// Reusable workspaces may contain compiler-created file aliases only when their complete link chain
    /// resolves to a regular file within the same workspace. Aliases are not counted as duplicate payloads.
    /// Immutable artifacts and process working directories reject all links.
    /// </remarks>
    public void Record(
        string directory,
        string leasePath,
        IReadOnlyList<string> dependencies,
        string? currentKey,
        string? targetId,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(dependencies);
        if (targetId is not null)
            ArgumentException.ThrowIfNullOrWhiteSpace(targetId);
        BuildCacheCleanupPlan.RequireChild(m_root, directory);
        BuildCacheCleanupPlan.RequireChild(m_root, leasePath);
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException(directory);
        cancellationToken.ThrowIfCancellationRequested();
        string relative = Path.GetRelativePath(m_root, directory).Replace('\\', '/');
        long bytes = BuildCacheFileInventory.Measure(directory, m_role, cancellationToken);
        var metadata = new Entry(relative, Path.GetRelativePath(m_root, leasePath), DateTime.UtcNow,
            dependencies.Select(Path.GetFullPath).ToArray(), bytes, targetId);
        AtomicFile.WriteAllBytes(Path.Combine(m_root, Key(relative) + ".cache.json"), WriteEntry(metadata));
        if (currentKey is not null)
            AtomicFile.WriteAllBytes(Path.Combine(m_root, Key(currentKey) + ".current"), Encoding.UTF8.GetBytes(relative));
    }

    /// <summary>
    /// Registers only explicitly recorded ownership, without discovering artifacts by extension or directory age.
    /// </summary>
    /// <param name="id">
    /// The unique publisher registration identity.
    /// </param>
    /// <returns>
    /// Borrowed inventory callbacks for this publisher's lifetime.
    /// </returns>
    public BuildCacheOwnerDescriptor Describe(string id)
        => new(id, m_root, Path.Combine(m_root, "owner.lock"), ReadArtifacts, ReadRoots);

    /// <summary>
    /// Records a reusable workspace before its first payload write while the caller holds activity and producer leases.
    /// Existing unregistered directories are rejected, rather than adopted by cleanup authority.
    /// </summary>
    /// <param name="directory">
    /// The new or previously registered workspace within this publisher root.
    /// </param>
    /// <param name="leasePath">
    /// The caller's persistent exclusive producer lease outside the workspace.
    /// </param>
    /// <param name="targetId">
    /// The explicit product target, or null for shared host work.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels registration before payload writes.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The publisher does not manage reusable workspaces or an existing directory has no current registration.
    /// </exception>
    /// <exception cref="IOException">
    /// The workspace escapes its boundary or ownership cannot be written.
    /// </exception>
    public void BeginWorkingDirectory(
        string directory,
        string leasePath,
        string? targetId,
        CancellationToken cancellationToken = default
    ) {
        if (m_role != BuildCacheArtifactRole.ReusableWorkspace)
            throw new InvalidOperationException("Working registration requires a reusable workspace publisher.");
        BuildCacheCleanupPlan.RequireChild(m_root, directory);
        BuildCacheCleanupPlan.RequireChild(m_root, leasePath);
        cancellationToken.ThrowIfCancellationRequested();
        string relative = Path.GetRelativePath(m_root, directory).Replace('\\', '/');
        string record = Path.Combine(m_root, Key(relative) + ".cache.json");
        if (Directory.Exists(directory) && !File.Exists(record))
            throw new InvalidOperationException($"An existing unregistered workspace cannot be adopted: '{directory}'.");
        if (File.Exists(record) && ReadEntry(record).directory != relative)
            throw new InvalidDataException("Working ownership has an inconsistent directory identity.");
        AtomicFile.WriteAllBytes(record, WriteEntry(new(relative, Path.GetRelativePath(m_root, leasePath).Replace('\\', '/'), DateTime.UtcNow, [], 0, targetId)));
        Directory.CreateDirectory(directory);
        string owners = Path.Combine(directory, "owners");
        Directory.CreateDirectory(owners);
        ProcessOwnerStamp stamp = ProcessOwnerStamp.CaptureCurrent();
        AtomicFile.WriteAllBytes(Path.Combine(owners, stamp.processId + ".pid"), JsonSerializer.SerializeToUtf8Bytes(stamp));
    }

    /// <summary>
    /// Registers a process-owned loader directory and its dependencies before returning it to a loader.
    /// Retirement preserves the directory until every recorded process has exited.
    /// </summary>
    /// <param name="directory">
    /// The completed directory within this publisher's boundary.
    /// </param>
    /// <param name="leasePath">
    /// The persistent producer and retirement lease outside the directory.
    /// </param>
    /// <param name="dependencies">
    /// Immutable generations required by this loader directory.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels ownership acquisition before the process claim is recorded.
    /// </param>
    /// <exception cref="IOException">
    /// The directory escapes ownership or its registration cannot be written.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Ownership acquisition was canceled.
    /// </exception>
    public void RecordProcessUse(
        string directory,
        string leasePath,
        IReadOnlyList<string> dependencies,
        CancellationToken cancellationToken = default
    ) {
        using FileLease activity = AcquireUseAsync(cancellationToken).AsTask().GetAwaiter().GetResult();
        BuildCacheCleanupPlan.RequireChild(m_root, directory);
        BuildCacheCleanupPlan.RequireChild(m_root, leasePath);
        using FileLease writing = FileLease.AcquireAsync(leasePath, TimeSpan.FromSeconds(30), cancellationToken)
            .AsTask().GetAwaiter().GetResult();
        string owners = Path.Combine(directory, "owners");
        Directory.CreateDirectory(owners);
        ProcessOwnerStamp stamp = ProcessOwnerStamp.CaptureCurrent();
        string process = stamp.processId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        AtomicFile.WriteAllBytes(Path.Combine(owners, process + ".pid"), JsonSerializer.SerializeToUtf8Bytes(stamp));
        Record(directory, leasePath, dependencies, null, null, cancellationToken);
    }

    private IReadOnlyList<BuildCacheArtifactDescriptor> ReadArtifacts(CancellationToken cancellation)
    {
        List<BuildCacheArtifactDescriptor> result = [];
        if (!Directory.Exists(m_root))
            return result;
        foreach (string path in Directory.EnumerateFiles(m_root, "*.cache.json").Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(m_root, path);
            Entry entry = ReadEntry(path);
            string directory = PathBoundary.Resolve(m_root, entry.directory);
            string lease = PathBoundary.Resolve(m_root, entry.leasePath);
            if (Path.GetFileName(path) != Key(entry.directory) + ".cache.json")
                throw new InvalidDataException("Cache ownership metadata does not match its identity.");
            if (!Directory.Exists(directory))
                continue;
            // Completed workspaces are measured by their sole writer. An unfinished registration
            // has zero bytes and must be inspected; this metadata never validates Native contents.
            long bytes = m_role == BuildCacheArtifactRole.ImmutableArtifact
                || m_role == BuildCacheArtifactRole.ReusableWorkspace && entry.bytes > 0
                ? entry.bytes : BuildCacheFileInventory.Measure(directory, m_role, cancellation);
            result.Add(new(directory, lease, entry.targetId, bytes, entry.lastUseUtc, entry.dependencies, m_role));
        }
        foreach (ProcessOwnedDirectoryDescriptor working in ProcessOwnedDirectory.ReadClaims(m_root, cancellation))
        {
            if (result.Any(entry => BuildCacheInventory.pathComparer.Equals(entry.directory, working.directory)))
                continue;
            long bytes = BuildCacheFileInventory.Measure(working.directory, BuildCacheArtifactRole.ProcessWorkingDirectory, cancellation);
            result.Add(new(working.directory, working.leasePath, null, bytes, Directory.GetLastWriteTimeUtc(working.directory), [], BuildCacheArtifactRole.ProcessWorkingDirectory));
        }
        return result;
    }

    private IReadOnlyList<string> ReadRoots(CancellationToken cancellation)
    {
        List<string> result = [];
        if (!Directory.Exists(m_root))
            return result;
        foreach (string path in m_role == BuildCacheArtifactRole.ImmutableArtifact
            ? Directory.EnumerateFiles(m_root, "*.current").Order(StringComparer.Ordinal) : Enumerable.Empty<string>())
        {
            cancellation.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(m_root, path);
            result.Add(PathBoundary.Resolve(m_root, File.ReadAllText(path)));
        }
        foreach (string path in Directory.EnumerateFiles(m_root, "*.cache.json").Order(StringComparer.Ordinal))
        {
            cancellation.ThrowIfCancellationRequested();
            PathBoundary.RequireUnlinkedPath(m_root, path);
            Entry entry = ReadEntry(path);
            string directory = PathBoundary.Resolve(m_root, entry.directory);
            string owners = Path.Combine(directory, "owners");
            if (m_role != BuildCacheArtifactRole.ReusableWorkspace && Directory.Exists(owners) && PathBoundary.EnumerateFiles(owners)
                .Where(static file => Path.GetExtension(file) == ".pid").Any(IsProcessAlive))
                result.Add(directory);
        }
        foreach (ProcessOwnedDirectoryDescriptor working in ProcessOwnedDirectory.ReadClaims(m_root, cancellation))
            if (working.ownerState != ProcessOwnerState.Exited)
                result.Add(working.directory);
        return result;
    }

    private static bool IsProcessAlive(string marker)
    {
        try
        {
            using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(marker));
            JsonElement root = document.RootElement;
            if (!root.EnumerateObject().Select(static property => property.Name).Order(StringComparer.Ordinal)
                .SequenceEqual(new[] { "processId", "startedUtcTicks" }, StringComparer.Ordinal))
                return true;
            var stamp = new ProcessOwnerStamp(root.GetProperty("processId").GetInt32(), root.GetProperty("startedUtcTicks").GetInt64());
            return stamp.Query() != ProcessOwnerState.Exited;
        }
        catch (Exception exception) when (exception is JsonException or ArgumentException or InvalidOperationException
            or IOException or UnauthorizedAccessException or FormatException or OverflowException)
        {
            return true;
        }
    }

    private static string Key(string value) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(value)));

    private static byte[] WriteEntry(Entry entry)
    {
        using var output = new MemoryStream();
        using (var writer = new Utf8JsonWriter(output))
        {
            writer.WriteStartObject();
            writer.WriteString("directory", entry.directory);
            writer.WriteString("leasePath", entry.leasePath);
            writer.WriteString("lastUseUtc", entry.lastUseUtc);
            writer.WriteNumber("bytes", entry.bytes);
            writer.WriteString("targetId", entry.targetId);
            writer.WriteStartArray("dependencies");
            foreach (string dependency in entry.dependencies)
                writer.WriteStringValue(dependency);
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        return output.ToArray();
    }

    private static Entry ReadEntry(string path)
    {
        using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(path));
        JsonElement root = document.RootElement;
        if (!root.EnumerateObject().Select(static property => property.Name).Order(StringComparer.Ordinal)
            .SequenceEqual(new[] { "bytes", "dependencies", "directory", "lastUseUtc", "leasePath", "targetId" }, StringComparer.Ordinal))
            throw new InvalidDataException("Cache ownership requires one complete current record.");
        long bytes = root.GetProperty("bytes").GetInt64();
        if (bytes < 0)
            throw new InvalidDataException("Recorded cache size cannot be negative.");
        return new(root.GetProperty("directory").GetString()!, root.GetProperty("leasePath").GetString()!,
            root.GetProperty("lastUseUtc").GetDateTime(), root.GetProperty("dependencies").EnumerateArray()
                .Select(static dependency => dependency.GetString()!).ToArray(), bytes, root.GetProperty("targetId").GetString());
    }

    private sealed record Entry(
        string directory,
        string leasePath,
        DateTime lastUseUtc,
        string[] dependencies,
        long bytes,
        string? targetId
    );
}
