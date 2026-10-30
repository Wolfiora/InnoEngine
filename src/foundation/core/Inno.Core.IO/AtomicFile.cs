using System;
using System.IO;

namespace Inno.Core.IO;

/// <summary>
/// Provides durable same-directory file replacement without exposing partial destination content.
/// </summary>
public static class AtomicFile
{
    private const string C_CANDIDATE_MARKER = ".staging-";

    /// <summary>
    /// Recognizes the reserved temporary file names created by atomic byte publication.
    /// </summary>
    /// <param name="path">
    /// The file name or path to inspect without opening the file.
    /// </param>
    /// <returns>
    /// True for a nonempty destination name followed by the reserved marker and a complete hexadecimal operation identity.
    /// </returns>
    /// <remarks>
    /// Recognition excludes transient payloads from source discovery. It does not establish ownership or authorize deletion.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// The path is null.
    /// </exception>
    public static bool IsCandidatePath(string path)
    {
        ArgumentNullException.ThrowIfNull(path);
        ReadOnlySpan<char> name = Path.GetFileName(path.AsSpan());
        int marker = name.Length - C_CANDIDATE_MARKER.Length - 32;
        return marker > 0 && name.Slice(marker, C_CANDIDATE_MARKER.Length).SequenceEqual(C_CANDIDATE_MARKER)
            && Guid.TryParseExact(name[(marker + C_CANDIDATE_MARKER.Length)..], "N", out _);
    }

    /// <summary>
    /// Writes a complete byte payload and atomically installs it at the destination.
    /// </summary>
    /// <param name="path">
    /// The destination file path.
    /// </param>
    /// <param name="data">
    /// The complete file payload.
    /// </param>
    /// <param name="overwrite">
    /// Whether an existing destination may be replaced; false atomically rejects name collisions.
    /// </param>
    /// <exception cref="IOException">
    /// Publication or cleanup fails, the destination already exists in create-only mode, or the
    /// filesystem cannot atomically publish a complete file. No copy fallback exposes partial content.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// The host does not provide an atomic create-only filesystem primitive.
    /// </exception>
    public static void WriteAllBytes(
        string path,
        ReadOnlySpan<byte> data,
        bool overwrite = true
    ) {
        string destination = NormalizeDestination(path);
        string candidate = destination + C_CANDIDATE_MARKER + Guid.NewGuid().ToString("N");
        try
        {
            using (var stream = new FileStream(
                       candidate,
                       FileMode.CreateNew,
                       FileAccess.Write,
                       FileShare.None,
                       32 * 1024,
                       FileOptions.WriteThrough))
            {
                stream.Write(data);
                stream.Flush(flushToDisk: true);
            }
            Install(candidate, destination, overwrite);
        }
        finally
        {
            if (File.Exists(candidate))
                File.Delete(candidate);
        }
    }

    /// <summary>
    /// Atomically installs an existing same-directory candidate file.
    /// </summary>
    /// <param name="source">
    /// The complete candidate file beside the destination.
    /// </param>
    /// <param name="destination">
    /// The destination file path beside the candidate.
    /// </param>
    /// <param name="overwrite">
    /// Whether an existing destination may be replaced; false never overwrites an external creator.
    /// </param>
    /// <remarks>
    /// A failed create-only publication preserves the candidate. After successful publication the
    /// candidate is removed; a subsequent cleanup failure reports that the complete target exists.
    /// Unix create-only publication requires same-directory hard-link support. The caller must own
    /// the candidate and parent directory until the operation completes.
    /// </remarks>
    /// <exception cref="IOException">
    /// Publication fails, create-only mode finds an existing destination, or candidate retirement
    /// fails after the complete destination has been published.
    /// </exception>
    /// <exception cref="PlatformNotSupportedException">
    /// The host does not provide an atomic create-only filesystem primitive.
    /// </exception>
    public static void Install(
        string source,
        string destination,
        bool overwrite = true
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(source);
        string candidate = Path.GetFullPath(source);
        string target = NormalizeDestination(destination);
        if (!File.Exists(candidate))
            throw new FileNotFoundException("The atomic file candidate does not exist.", candidate);
        if (string.Equals(candidate, target, StringComparison.OrdinalIgnoreCase))
            throw new ArgumentException("The atomic file candidate and destination must be different paths.", nameof(source));

        string candidateDirectory = Path.GetDirectoryName(candidate)!;
        string targetDirectory = Path.GetDirectoryName(target)!;
        if (!string.Equals(candidateDirectory, targetDirectory, StringComparison.Ordinal))
            throw new ArgumentException(
                "Atomic file installation requires the candidate and destination to share a directory.",
                nameof(source));

        FileSystemRename.MoveFile(candidate, target, overwrite);
    }

    private static string NormalizeDestination(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        string destination = Path.GetFullPath(path);
        string? directory = Path.GetDirectoryName(destination);
        if (string.IsNullOrEmpty(directory))
            throw new IOException($"File path '{destination}' has no owning directory.");
        Directory.CreateDirectory(directory);
        return destination;
    }

}
