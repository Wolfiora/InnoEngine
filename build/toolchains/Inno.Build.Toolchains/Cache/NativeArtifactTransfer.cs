using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Transports verified native payloads without copying publisher registrations, leases or writable workspaces.
/// </summary>
public static class NativeArtifactTransfer
{
    /// <summary>
    /// Exports only locally registered immutable native and binding generations under their existing read leases.
    /// </summary>
    /// <param name="engineRoot">
    /// The checkout whose publishers have already verified the selected generations.
    /// </param>
    /// <param name="archivePath">
    /// The explicit transport file, atomically replaced after complete export.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels copying before the completed archive replaces its previous version.
    /// </param>
    /// <returns>
    /// The number of verified generations exported.
    /// </returns>
    /// <exception cref="IOException">
    /// A registered artifact is damaged, linked, inaccessible or outside its publisher boundary.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Export was canceled before installation.
    /// </exception>
    public static async Task<int> ExportAsync(
        string engineRoot,
        string archivePath,
        CancellationToken cancellationToken = default
    ) {
        string root = Path.GetFullPath(engineRoot);
        string destination = PathBoundary.RequireUnlinkedPath(root, Path.GetFullPath(archivePath));
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        string candidate = destination + ".staging-" + Guid.NewGuid().ToString("N");
        int count = 0;
        try
        {
            using (var stream = new FileStream(candidate, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.None))
            {
                using (var zip = new ZipArchive(stream, ZipArchiveMode.Create, leaveOpen: true))
                foreach (BuildCacheOwnerDescriptor owner in BuildCacheOwnerCatalog.ReadOwners(root, cancellationToken)
                    .Where(static owner => owner.id is "native" or "bindings"))
                {
                    if (owner.directory != Path.Combine(root, "artifacts", owner.id))
                        throw new InvalidDataException("Native transfer encountered a foreign publisher registration.");
                    using FileLease activity = await FileLease.AcquireSharedAsync(owner.coordinationPath,
                        TimeSpan.FromSeconds(30), cancellationToken).ConfigureAwait(false);
                    foreach (BuildCacheArtifactDescriptor artifact in owner.ReadArtifacts(cancellationToken)
                        .Where(static artifact => artifact.role == BuildCacheArtifactRole.ImmutableArtifact).OrderBy(static artifact => artifact.directory, StringComparer.Ordinal))
                    {
                        string relative = Path.GetRelativePath(Path.Combine(root, "artifacts"), artifact.directory).Replace('\\', '/');
                        string[] parts = RequireArtifactPath(relative);
                        using BuildArtifactReadLease reading = await BuildArtifactReadLease.AcquireAsync(artifact.directory, owner.directory,
                            parts[3], OutputRoots(parts[0]), cancellationToken).ConfigureAwait(false);
                        foreach (string file in PathBoundary.EnumerateFiles(artifact.directory).Order(StringComparer.Ordinal))
                        {
                            cancellationToken.ThrowIfCancellationRequested();
                            ZipArchiveEntry entry = zip.CreateEntry(relative + "/" + Path.GetRelativePath(artifact.directory, file).Replace('\\', '/'), CompressionLevel.Fastest);
                            entry.LastWriteTime = new DateTimeOffset(1980, 1, 1, 0, 0, 0, TimeSpan.Zero);
                            if (!OperatingSystem.IsWindows())
                                entry.ExternalAttributes = ((int)File.GetUnixFileMode(file) | 0x8000) << 16;
                            using Stream output = entry.Open();
                            using FileStream input = File.OpenRead(file);
                            await input.CopyToAsync(output, cancellationToken).ConfigureAwait(false);
                        }
                        count++;
                    }
                }
                stream.Flush(flushToDisk: true);
            }
            if (count == 0)
                throw new InvalidDataException("No registered immutable native generations are available for export.");
            cancellationToken.ThrowIfCancellationRequested();
            AtomicFile.Install(candidate, destination);
            return count;
        }
        finally
        {
            if (File.Exists(candidate))
                File.Delete(candidate);
        }
    }

    /// <summary>
    /// Imports complete payload candidates without adopting them; each consumer's publisher must still verify its requested inputs.
    /// </summary>
    /// <param name="engineRoot">
    /// The consumer checkout whose existing native publishers own the destination layout.
    /// </param>
    /// <param name="archivePath">
    /// The restored transport archive, which remains available for diagnosis after rejection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels extraction or acquisition without replacing any existing generation.
    /// </param>
    /// <returns>
    /// The number of newly imported candidates; existing destinations remain under their publisher's repair policy.
    /// </returns>
    /// <exception cref="IOException">
    /// The archive is malformed, contains invalid ownership paths or fails complete payload validation.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Import was canceled before further candidates could be installed.
    /// </exception>
    public static async Task<int> ImportAsync(
        string engineRoot,
        string archivePath,
        CancellationToken cancellationToken = default
    ) {
        string root = Path.GetFullPath(engineRoot);
        string stagingRoot = Path.Combine(root, "artifacts", "native-transfer");
        BuildCacheOwnerCatalog.Register(root, stagingRoot, "native-transfer", BuildCacheArtifactRole.ProcessWorkingDirectory, cancellationToken);
        using ProcessOwnedDirectory staging = await ProcessOwnedDirectory.CreateAsync(stagingRoot, cancellationToken).ConfigureAwait(false);
        var artifacts = new HashSet<string>(StringComparer.Ordinal);
        var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        using (ZipArchive zip = ZipFile.OpenRead(archivePath))
        {
            long expanded = 0;
            if (zip.Entries.Count == 0 || zip.Entries.Count > 200000)
                throw new InvalidDataException("Native transfer contains an empty or excessive file inventory.");
            foreach (ZipArchiveEntry entry in zip.Entries)
            {
                cancellationToken.ThrowIfCancellationRequested();
                string[] parts = entry.FullName.Split('/');
                if (parts.Length < 5 || parts.Any(static part => part is "" or "." or ".." || part.Contains('\\') || part.Contains(':'))
                    || !names.Add(entry.FullName) || ((entry.ExternalAttributes >> 16) & 0xf000) is not (0 or 0x8000)
                    || (expanded = checked(expanded + entry.Length)) > 32L * 1024 * 1024 * 1024)
                    throw new InvalidDataException("Native transfer contains an invalid, linked, duplicate or excessive entry.");
                string artifact = string.Join('/', parts.Take(4));
                _ = RequireArtifactPath(artifact);
                artifacts.Add(artifact);
                string output = PathBoundary.Resolve(staging.directory, entry.FullName);
                Directory.CreateDirectory(Path.GetDirectoryName(output)!);
                using (Stream input = entry.Open())
                using (var destination = new FileStream(output, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                    await input.CopyToAsync(destination, cancellationToken).ConfigureAwait(false);
                if (!OperatingSystem.IsWindows() && entry.ExternalAttributes != 0)
                    File.SetUnixFileMode(output, (UnixFileMode)((entry.ExternalAttributes >> 16) & 0x1ff));
            }
        }
        foreach (string artifact in artifacts)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string[] parts = RequireArtifactPath(artifact);
            if (!BuildArtifactManifest.IsComplete(PathBoundary.Resolve(staging.directory, artifact), parts[3], OutputRoots(parts[0])))
                throw new InvalidDataException("Native transfer failed exact manifest and payload verification: " + artifact);
        }
        int imported = 0;
        foreach (string artifact in artifacts.Order(StringComparer.Ordinal))
        {
            string[] parts = RequireArtifactPath(artifact);
            string destination = PathBoundary.RequireUnlinkedPath(root, Path.Combine(root, "artifacts", artifact));
            // An existing generation is never replaced by transport, including while a consumer reads it.
            // Its actual publisher remains responsible for completeness, repair and adoption.
            if (Directory.Exists(destination))
                continue;
            using CoordinatedFileLease writing = await CoordinatedFileLease.AcquireAsync(Path.Combine(root, "artifacts", parts[0], "owner.lock"),
                destination + ".read.lock", TimeSpan.FromSeconds(30), shared: false, cancellationToken: cancellationToken).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            if (Directory.Exists(destination))
                continue;
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            Directory.Move(PathBoundary.Resolve(staging.directory, artifact), destination);
            imported++;
        }
        return imported;
    }

    private static string[] RequireArtifactPath(string relative)
    {
        string[] parts = relative.Split('/');
        if (parts.Length != 4 || parts[0] is not ("native" or "bindings") || parts.Any(static part =>
                part is "" or "." or ".." || part.Any(static character => !char.IsAsciiLetterOrDigit(character) && character is not ('-' or '_' or '.')))
            || parts[3].Length != 64 || parts[3].Any(static character => !char.IsAsciiHexDigitLower(character)))
            throw new InvalidDataException("Native transfer has an invalid publisher or content identity: " + relative);
        return parts;
    }

    private static string[] OutputRoots(string owner) => owner == "native" ? ["Outputs", "Metadata"] : ["Generated", "Native"];
}
