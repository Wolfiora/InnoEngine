using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Core.IO;

/// <summary>
/// Claims a new temporary directory before payload writes and preserves failed cleanup for later retirement.
/// </summary>
public sealed class ProcessOwnedDirectory : IDisposable
{
    private readonly string m_root;
    private readonly string m_record;
    private readonly ProcessOwnerStamp m_owner;
    private FileLease? m_lease;

    private ProcessOwnedDirectory(
        string root,
        string path,
        string record,
        ProcessOwnerStamp owner,
        FileLease lease
    ) {
        m_root = root;
        directory = path;
        m_record = record;
        m_owner = owner;
        m_lease = lease;
    }

    /// <summary>
    /// Gets the newly claimed absolute directory; its parent and persistent lease are not payloads.
    /// </summary>
    public string directory { get; }

    /// <summary>
    /// Creates a directory protected by this process lifetime when the caller can retain only file metadata.
    /// Payload consumers in other processes must register their lifetimes before using it.
    /// </summary>
    /// <param name="root">
    /// The absolute producer root; existing directories are never adopted.
    /// </param>
    /// <param name="identity">
    /// A current operation's explicit 32-digit hexadecimal directory identity shared by its process participants.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels before the process claim is transferred.
    /// </param>
    /// <returns>
    /// The new directory, preserved until every recorded process exits and the producer retires it.
    /// No managed lease object is retained by the caller.
    /// </returns>
    /// <exception cref="IOException">
    /// Ownership cannot be established within the boundary.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// The producer root or current operation identity is invalid.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// An existing operation claim is malformed, inactive, or cannot authorize this process.
    /// </exception>
    public static async ValueTask<string> CreateForProcessAsync(
        string root,
        string identity,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        if (!Path.IsPathFullyQualified(root) || identity is null || identity.Length != 32
            || identity.Any(static character => !Uri.IsHexDigit(character)))
            throw new ArgumentException("Process storage requires an absolute producer root and current operation identity.");
        root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        if (root == Path.GetPathRoot(root))
            throw new ArgumentException("Process storage cannot manage a filesystem root.", nameof(root));
        string path = PathBoundary.RequireUnlinkedPath(root, PathBoundary.Resolve(root, identity));
        string record = RecordPath(root, identity);
        using FileLease lease = await FileLease.AcquireAsync(path + ".work.lock", TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false);
        if (File.Exists(record))
        {
            Claim claim = ReadClaim(record);
            if (claim.directory != identity || QueryOwners(record, null) != ProcessOwnerState.Active)
                throw new InvalidDataException("Process storage cannot reuse an exited or unverifiable operation claim.");
            RegisterProcess(root, path);
        }
        else
        {
            if (Directory.Exists(path))
                throw new InvalidDataException("An existing unregistered process directory cannot be adopted.");
            ProcessOwnerStamp owner = ProcessOwnerStamp.CaptureCurrent();
            WriteClaim(record, new Claim(identity, owner.processId, owner.startedUtcTicks));
            Directory.CreateDirectory(path);
        }
        return path;
    }

    /// <summary>
    /// Creates a fresh directory and ownership record under an explicitly supplied file boundary.
    /// </summary>
    /// <param name="root">
    /// The absolute producer-controlled root; existing unrecorded directories are never adopted.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels before payload ownership is returned.
    /// </param>
    /// <returns>
    /// An owned directory lease; dispose only after its subprocesses and consumers finish.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The root is relative or is a filesystem root.
    /// </exception>
    /// <exception cref="IOException">
    /// The boundary crosses a link or ownership cannot be recorded.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Creation was canceled.
    /// </exception>
    public static async ValueTask<ProcessOwnedDirectory> CreateAsync(
        string root,
        CancellationToken cancellationToken = default
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(root);
        if (!Path.IsPathFullyQualified(root) || Path.TrimEndingDirectorySeparator(root) == Path.GetPathRoot(root))
            throw new ArgumentException("Temporary ownership requires an explicit child root.", nameof(root));
        root = Path.TrimEndingDirectorySeparator(Path.GetFullPath(root));
        PathBoundary.RequireUnlinkedPath(root, root);
        string relative = Guid.NewGuid().ToString("N");
        string path = Path.Combine(root, relative);
        string record = RecordPath(root, relative);
        FileLease lease = await FileLease.AcquireAsync(path + ".work.lock", TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false);
        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (Directory.Exists(path) || File.Exists(record))
                throw new IOException("A fresh temporary identity already exists and cannot be adopted.");
            ProcessOwnerStamp owner = ProcessOwnerStamp.CaptureCurrent();
            WriteClaim(record, new Claim(relative, owner.processId, owner.startedUtcTicks));
            Directory.CreateDirectory(path);
            lease.Dispose();
            FileLease reading = await FileLease.AcquireSharedAsync(path + ".work.lock", TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false);
            return new(root, path, record, owner, reading);
        }
        catch
        {
            lease.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Records a subprocess lifetime while a previously claimed parent directory remains protected.
    /// </summary>
    /// <param name="root">
    /// The producer root containing the existing current claim.
    /// </param>
    /// <param name="directory">
    /// The claimed directory borrowed by this process.
    /// </param>
    /// <exception cref="IOException">
    /// The directory is unrecorded, escapes the root, or its ownership cannot be read.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// The existing claim is malformed or does not match this directory.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// An exclusive owner retains retirement rights beyond the registration budget.
    /// </exception>
    public static void RegisterCurrentProcess(
        string root,
        string directory
    ) {
        PathBoundary.RequireUnlinkedPath(root, directory);
        using FileLease reading = FileLease.AcquireSharedAsync(directory + ".work.lock", TimeSpan.FromSeconds(30))
            .AsTask().GetAwaiter().GetResult();
        RegisterProcess(root, directory);
    }

    /// <summary>
    /// Reads only current explicit claims, without inferring ownership from arbitrary directory names.
    /// </summary>
    /// <param name="root">
    /// The producer-controlled absolute root.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels claim inspection.
    /// </param>
    /// <returns>
    /// Existing directories with exact lease paths and conservative combined owner states.
    /// </returns>
    /// <exception cref="IOException">
    /// A claim cannot be read or its directory escapes the producer boundary.
    /// </exception>
    /// <exception cref="InvalidDataException">
    /// A claim is malformed or its recorded identity does not match its location.
    /// </exception>
    public static IReadOnlyList<ProcessOwnedDirectoryDescriptor> ReadClaims(
        string root,
        CancellationToken cancellationToken = default
    ) {
        var result = new List<ProcessOwnedDirectoryDescriptor>();
        if (!Directory.Exists(root))
            return result;
        foreach (string record in Directory.EnumerateFiles(root, "*.work.json"))
        {
            cancellationToken.ThrowIfCancellationRequested();
            Claim claim = ReadClaim(record);
            string path = PathBoundary.Resolve(root, claim.directory);
            if (record != RecordPath(root, claim.directory))
                throw new InvalidDataException("Temporary ownership has an inconsistent identity.");
            if (Directory.Exists(path))
                result.Add(new(path, path + ".work.lock", QueryOwners(record, null)));
        }
        return result;
    }

    /// <summary>
    /// Releases only this directory after its other recorded process lifetimes have exited.
    /// </summary>
    /// <exception cref="IOException">
    /// Removal failed; its explicit claim remains available for later retirement.
    /// </exception>
    public void Dispose()
    {
        FileLease? lease = m_lease;
        if (lease is null)
            return;
        m_lease = null;
        lease.Dispose();
        try
        {
            using FileLease retirement = FileLease.AcquireAsync(directory + ".work.lock", TimeSpan.FromSeconds(5))
                .AsTask().GetAwaiter().GetResult();
            if (QueryOwners(m_record, m_owner) != ProcessOwnerState.Exited)
                throw new IOException("A temporary directory still has an active or unverifiable subprocess owner.");
            if (Directory.Exists(directory))
                Directory.Delete(PathBoundary.RequireUnlinkedPath(m_root, directory), recursive: true);
            foreach (string marker in Directory.EnumerateFiles(m_root, Path.GetFileName(m_record) + ".*.process"))
                File.Delete(marker);
            File.Delete(m_record);
        }
        catch (TimeoutException failure)
        {
            throw new IOException("A temporary directory still has a registering subprocess reader.", failure);
        }
    }

    private static void RegisterProcess(
        string root,
        string directory
    ) {
        string relative = RequireClaim(root, directory);
        ProcessOwnerStamp owner = ProcessOwnerStamp.CaptureCurrent();
        string record = RecordPath(root, relative);
        WriteClaim(record + "." + owner.processId + ".process",
            new Claim(relative, owner.processId, owner.startedUtcTicks));
    }

    private static string RequireClaim(
        string root,
        string directory
    ) {
        PathBoundary.RequireUnlinkedPath(root, directory);
        string relative = Path.GetRelativePath(root, directory);
        Claim claim = ReadClaim(RecordPath(root, relative));
        if (claim.directory != relative || !Directory.Exists(directory))
            throw new InvalidDataException("The temporary directory has no matching current owner claim.");
        return relative;
    }

    private static string RecordPath(
        string root,
        string relative
    ) => Path.Combine(root, Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(relative))) + ".work.json");

    private static Claim ReadClaim(string path)
    {
        using JsonDocument document = JsonDocument.Parse(File.ReadAllBytes(path));
        JsonElement root = document.RootElement;
        if (root.ValueKind != JsonValueKind.Object
            || !root.TryGetProperty("directory", out JsonElement directory)
            || directory.ValueKind != JsonValueKind.String
            || !root.TryGetProperty("processId", out JsonElement processId)
            || processId.ValueKind != JsonValueKind.Number
            || !processId.TryGetInt32(out int id)
            || !root.TryGetProperty("startedUtcTicks", out JsonElement startedUtcTicks)
            || startedUtcTicks.ValueKind != JsonValueKind.Number
            || !startedUtcTicks.TryGetInt64(out long ticks))
            throw new InvalidDataException("Temporary ownership is incomplete.");
        var claim = new Claim(directory.GetString()!, id, ticks);
        if (claim.directory is null || claim.directory.Length != 32 || claim.directory.Any(static character => !Uri.IsHexDigit(character)))
            throw new InvalidDataException("Temporary ownership requires its current explicit directory identity.");
        _ = new ProcessOwnerStamp(claim.processId, claim.startedUtcTicks);
        return claim;
    }

    private static void WriteClaim(
        string path,
        Claim claim
    ) {
        using var output = new MemoryStream();
        using (var writer = new Utf8JsonWriter(output))
        {
            writer.WriteStartObject();
            writer.WriteString("directory", claim.directory);
            writer.WriteNumber("processId", claim.processId);
            writer.WriteNumber("startedUtcTicks", claim.startedUtcTicks);
            writer.WriteEndObject();
        }
        AtomicFile.WriteAllBytes(path, output.ToArray());
    }

    private static ProcessOwnerState QueryOwners(
        string record,
        ProcessOwnerStamp? excluded
    ) {
        ProcessOwnerState combined = ProcessOwnerState.Exited;
        foreach (string marker in new[] { record }.Concat(Directory.EnumerateFiles(Path.GetDirectoryName(record)!, Path.GetFileName(record) + ".*.process")))
        {
            try
            {
                Claim claim = ReadClaim(marker);
                var stamp = new ProcessOwnerStamp(claim.processId, claim.startedUtcTicks);
                if (stamp == excluded)
                    continue;
                ProcessOwnerState state = stamp.Query();
                if (state == ProcessOwnerState.Active)
                    return state;
                if (state == ProcessOwnerState.Unknown)
                    combined = state;
            }
            catch (Exception exception) when (exception is IOException or InvalidDataException or JsonException or ArgumentException or UnauthorizedAccessException)
            {
                combined = ProcessOwnerState.Unknown;
            }
        }
        return combined;
    }

    private sealed record Claim(
        string directory,
        int processId,
        long startedUtcTicks
    );
}
