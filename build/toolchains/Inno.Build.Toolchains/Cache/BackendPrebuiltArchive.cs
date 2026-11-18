using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;

namespace Inno.Build.Toolchains;

/// <summary>
/// Transfers the certified files for one backend target. Each archive contains exactly one current inventory.
/// </summary>
public static class BackendPrebuiltArchive
{
    private const string C_MANIFEST = "manifest.json";
    private const long C_MAX_BYTES = 8L * 1024 * 1024 * 1024;
    private static readonly DateTimeOffset S_EPOCH = new(1980, 1, 1, 0, 0, 0, TimeSpan.Zero);
    private sealed record FileRecord(string owner, string configuration, string input, string path, string sha256);
    private sealed record Inventory(int version, string target, FileRecord[] files);

    /// <summary>
    /// Creates a fresh platform ZIP from an explicit inventory and an owned staging directory.
    /// Additional files in staging are never included. Every selected digest is checked while copying.
    /// </summary>
    /// <param name="staging">Root of the exact logical files selected for this target.</param>
    /// <param name="inventoryPath">JSON with version, target and current owner, configuration, input, path and SHA-256 records.</param>
    /// <param name="archivePath">Destination for the completed platform ZIP.</param>
    /// <param name="cancellationToken">Cancels copying without replacing the previous destination.</param>
    public static void Create(
        string staging,
        string inventoryPath,
        string archivePath,
        CancellationToken cancellationToken = default
    ) {
        using FileStream manifest = File.OpenRead(inventoryPath);
        Inventory inventory = Read(manifest);
        string destination = Path.GetFullPath(archivePath);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        string candidate = destination + ".staging-" + Guid.NewGuid().ToString("N");
        try
        {
            using (var stream = new FileStream(candidate, FileMode.CreateNew, FileAccess.Write, FileShare.None))
            using (var zip = new ZipArchive(stream, ZipArchiveMode.Create))
            {
                using (Stream output = Entry(C_MANIFEST).Open())
                    JsonSerializer.Serialize(output, inventory);
                long total = 0;
                foreach (FileRecord file in inventory.files)
                {
                    string source = PathBoundary.RequireUnlinkedPath(staging, PathBoundary.Resolve(staging, file.path));
                    using FileStream input = File.OpenRead(source);
                    total = checked(total + input.Length);
                    if (total > C_MAX_BYTES)
                        throw new InvalidDataException("Backend archive exceeds its size limit.");
                    using Stream output = Entry(file.path).Open();
                    CopyVerified(input, output, file.sha256, input.Length, cancellationToken);
                }

                ZipArchiveEntry Entry(string path)
                {
                    ZipArchiveEntry entry = zip.CreateEntry(path, path.EndsWith(".zip", StringComparison.Ordinal) ? CompressionLevel.NoCompression : CompressionLevel.Fastest);
                    entry.LastWriteTime = S_EPOCH;
                    entry.ExternalAttributes = 0;
                    return entry;
                }
            }
            cancellationToken.ThrowIfCancellationRequested();
            AtomicFile.Install(candidate, destination);
        }
        finally
        {
            File.Delete(candidate);
        }
    }

    /// <summary>
    /// Checks a trusted archive digest, its declared target, inventory and payload before publishing a fresh consumer directory.
    /// The caller owns the destination and serializes its acquisition for the job.
    /// </summary>
    /// <param name="archivePath">Downloaded or restored platform archive.</param>
    /// <param name="sha256">Expected archive digest from the authenticated release or candidate receipt.</param>
    /// <param name="target">Target selected by the consumer.</param>
    /// <param name="destination">An absent directory under a consumer-owned staging parent.</param>
    /// <param name="cancellationToken">Cancels extraction before publication.</param>
    public static async Task ExtractAsync(
        string archivePath,
        string sha256,
        string target,
        string destination,
        CancellationToken cancellationToken = default
    ) {
        string output = Path.GetFullPath(destination);
        string parent = Path.GetDirectoryName(output)!;
        using FileStream stream = File.OpenRead(archivePath);
        if (!IsDigest(sha256) || Convert.ToHexStringLower(await SHA256.HashDataAsync(stream, cancellationToken).ConfigureAwait(false)) != sha256)
            throw new InvalidDataException("The backend platform ZIP differs from its receipt.");
        stream.Position = 0;
        using var zip = new ZipArchive(stream, ZipArchiveMode.Read);
        ZipArchiveEntry metadata = zip.GetEntry(C_MANIFEST) ?? throw new InvalidDataException("Missing backend inventory.");
        if (metadata.Length > 16 * 1024 * 1024 || zip.Entries.Count > 50001)
            throw new InvalidDataException("Backend ZIP inventory exceeds its limit.");
        Inventory inventory;
        using (Stream input = metadata.Open())
            inventory = Read(input);
        if (inventory.target != target || zip.Entries.Count != inventory.files.Length + 1)
            throw new InvalidDataException("Backend target or inventory does not match the request.");
        using ProcessOwnedDirectory staging = await ProcessOwnedDirectory.CreateAsync(parent, cancellationToken).ConfigureAwait(false);
        string payload = Path.Combine(staging.directory, "payload");
        Directory.CreateDirectory(payload);
        var expected = inventory.files.ToDictionary(static file => file.path, StringComparer.Ordinal);
        long total = 0;
        foreach (ZipArchiveEntry entry in zip.Entries)
        {
            if (entry == metadata)
                continue;
            if (!expected.Remove(entry.FullName, out FileRecord? file) || ((entry.ExternalAttributes >> 16) & 0xf000) is not (0 or 0x8000)
                || (entry.ExternalAttributes & (int)FileAttributes.ReparsePoint) != 0 || (total = checked(total + entry.Length)) > C_MAX_BYTES)
                throw new InvalidDataException("Unexpected, duplicate, linked or excessive backend ZIP entry: " + entry.FullName);
            string path = PathBoundary.Resolve(payload, entry.FullName);
            Directory.CreateDirectory(Path.GetDirectoryName(path)!);
            using Stream input = entry.Open();
            using var targetFile = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None);
            CopyVerified(input, targetFile, file.sha256, entry.Length, cancellationToken);
        }
        if (expected.Count != 0)
            throw new InvalidDataException("Missing backend ZIP entries.");
        AtomicFile.WriteAllBytes(Path.Combine(payload, C_MANIFEST), JsonSerializer.SerializeToUtf8Bytes(inventory));
        AtomicDirectory.Publish(payload, output, cancellationToken);
    }

    private static Inventory Read(Stream input)
    {
        Inventory inventory = JsonSerializer.Deserialize<Inventory>(input) ?? throw new InvalidDataException("Missing backend inventory.");
        if (inventory.version != 1 || string.IsNullOrWhiteSpace(inventory.target) || inventory.files is not { Length: > 0 and <= 50000 })
            throw new InvalidDataException("Unsupported or empty backend inventory.");
        var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (FileRecord file in inventory.files)
        {
            if (file is null || string.IsNullOrWhiteSpace(file.owner) || string.IsNullOrWhiteSpace(file.configuration) || !IsDigest(file.input)
                || !IsDigest(file.sha256) || string.IsNullOrEmpty(file.path) || !paths.Add(file.path) || file.path.Equals(C_MANIFEST, StringComparison.OrdinalIgnoreCase)
                || file.path.Split('/').Any(static part => part is "" or "." or ".." || part.EndsWith('.') || part.EndsWith(' ')
                    || part.Any(static c => !char.IsAsciiLetterOrDigit(c) && c is not ('.' or '_' or '-'))
                    || IsReservedName(part)))
                throw new InvalidDataException("Invalid or duplicated backend file ownership: " + file?.path);
        }
        return inventory with { files = inventory.files.OrderBy(static file => file.path, StringComparer.Ordinal).ToArray() };
    }

    private static bool IsReservedName(string part)
    {
        string name = part.Split('.')[0].ToUpperInvariant();
        return name is "CON" or "PRN" or "AUX" or "NUL"
            || name.Length == 4 && name[..3] is ("LPT" or "COM") && name[3] is >= '1' and <= '9';
    }

    private static bool IsDigest(string? value) => value is { Length: 64 } && value.All(static c => char.IsAsciiDigit(c) || c is >= 'a' and <= 'f');

    private static void CopyVerified(
        Stream input,
        Stream output,
        string digest,
        long length,
        CancellationToken cancellation
    ) {
        using var hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
        byte[] buffer = new byte[81920];
        long copied = 0;
        int read;
        while ((read = input.Read(buffer)) > 0)
        {
            cancellation.ThrowIfCancellationRequested();
            if ((copied += read) > length)
                throw new InvalidDataException("Backend payload exceeds its declared length.");
            hash.AppendData(buffer, 0, read);
            output.Write(buffer, 0, read);
        }
        if (copied != length || Convert.ToHexStringLower(hash.GetHashAndReset()) != digest)
            throw new InvalidDataException("Backend payload does not match its inventory.");
    }
}
