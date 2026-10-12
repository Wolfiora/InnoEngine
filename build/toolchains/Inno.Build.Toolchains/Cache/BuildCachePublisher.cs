using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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

    /// <summary>
    /// Creates a publisher for one absolute, non-overlapping cache boundary.
    /// </summary>
    /// <param name="directory">
    /// The cache root exclusively controlled by this publisher.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The directory is blank or relative.
    /// </exception>
    public BuildCachePublisher(string directory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(directory);
        if (!Path.IsPathFullyQualified(directory))
            throw new ArgumentException("A cache publisher requires an absolute root.", nameof(directory));
        m_root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(directory));
    }

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
    /// <exception cref="IOException">
    /// An owned path escapes the boundary or crosses a link.
    /// </exception>
    /// <exception cref="DirectoryNotFoundException">
    /// The completed generation is absent.
    /// </exception>
    public void Record(
        string directory,
        string leasePath,
        IReadOnlyList<string> dependencies,
        string? currentKey,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(dependencies);
        BuildCacheCleanupPlan.RequireChild(m_root, directory);
        BuildCacheCleanupPlan.RequireChild(m_root, leasePath);
        if (!Directory.Exists(directory))
            throw new DirectoryNotFoundException(directory);
        cancellationToken.ThrowIfCancellationRequested();
        string relative = Path.GetRelativePath(m_root, directory).Replace('\\', '/');
        long bytes = 0;
        foreach (string file in PathBoundary.EnumerateFiles(directory))
        {
            cancellationToken.ThrowIfCancellationRequested();
            bytes = checked(bytes + new FileInfo(file).Length);
        }
        var metadata = new Entry(relative, Path.GetRelativePath(m_root, leasePath), DateTime.UtcNow,
            dependencies.Select(Path.GetFullPath).ToArray(), bytes);
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
        string process = Environment.ProcessId.ToString(System.Globalization.CultureInfo.InvariantCulture);
        AtomicFile.WriteAllBytes(Path.Combine(owners, process + ".pid"), Encoding.UTF8.GetBytes(process));
        Record(directory, leasePath, dependencies, null, cancellationToken);
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
            result.Add(new(directory, lease, entry.bytes, entry.lastUseUtc, entry.dependencies));
        }
        return result;
    }

    private IReadOnlyList<string> ReadRoots(CancellationToken cancellation)
    {
        List<string> result = [];
        if (!Directory.Exists(m_root))
            return result;
        foreach (string path in Directory.EnumerateFiles(m_root, "*.current").Order(StringComparer.Ordinal))
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
            if (Directory.Exists(owners) && PathBoundary.EnumerateFiles(owners)
                .Where(static file => Path.GetExtension(file) == ".pid").Any(IsProcessAlive))
                result.Add(directory);
        }
        return result;
    }

    private static bool IsProcessAlive(string marker)
    {
        if (!int.TryParse(File.ReadAllText(marker), out int processId) || processId <= 0)
            return true;
        try
        {
            using Process process = Process.GetProcessById(processId);
            return !process.HasExited;
        }
        catch (ArgumentException) { return false; }
        catch (InvalidOperationException) { return false; }
        catch (Win32Exception) { return true; }
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
            .SequenceEqual(new[] { "bytes", "dependencies", "directory", "lastUseUtc", "leasePath" }, StringComparer.Ordinal))
            throw new InvalidDataException("Cache ownership requires one complete current record.");
        long bytes = root.GetProperty("bytes").GetInt64();
        if (bytes < 0)
            throw new InvalidDataException("Recorded cache size cannot be negative.");
        return new(root.GetProperty("directory").GetString()!, root.GetProperty("leasePath").GetString()!,
            root.GetProperty("lastUseUtc").GetDateTime(), root.GetProperty("dependencies").EnumerateArray()
                .Select(static dependency => dependency.GetString()!).ToArray(), bytes);
    }

    private sealed record Entry(
        string directory,
        string leasePath,
        DateTime lastUseUtc,
        string[] dependencies,
        long bytes
    );
}
