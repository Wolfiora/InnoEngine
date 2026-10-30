using System;
using System.IO;

namespace Inno.Core.IO;

/// <summary>
/// Stores one file document with atomic replacement using the common filesystem boundary.
/// </summary>
public sealed class FileByteDocumentStore : IByteDocumentStore
{
    private readonly string m_path;

    /// <summary>
    /// Selects one host-owned absolute document location.
    /// </summary>
    /// <param name="path">
    /// The absolute file path; its parent directory is prepared only when writing.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The path is empty or relative.
    /// </exception>
    public FileByteDocumentStore(string path)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("A file document requires an absolute path.", nameof(path));
        m_path = Path.GetFullPath(path);
    }

    /// <inheritdoc />
    public string documentName => m_path;

    /// <inheritdoc />
    public bool canWrite => true;

    /// <inheritdoc />
    public bool exists => File.Exists(m_path);

    /// <inheritdoc />
    /// <remarks>
    /// One rename-compatible handle pins the selected complete publication until its owned bytes are read.
    /// Writers must atomically replace the document rather than modify the selected file in place.
    /// </remarks>
    /// <exception cref="IOException">
    /// The document cannot be read completely or exceeds the maximum supported byte-array size.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The selected location denies reading or names a directory instead of a document.
    /// </exception>
    public byte[]? Read()
    {
        try
        {
            using var stream = new FileStream(m_path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Delete);
            long length = stream.Length;
            if (length > Array.MaxLength)
                throw new IOException("The file document exceeds the maximum supported byte-array size.");
            byte[] data = GC.AllocateUninitializedArray<byte>((int)length);
            stream.ReadExactly(data);
            return data;
        }
        catch (FileNotFoundException)
        {
            return null;
        }
        catch (DirectoryNotFoundException)
        {
            return null;
        }
    }

    /// <inheritdoc />
    public void Write(ReadOnlySpan<byte> data) => AtomicFile.WriteAllBytes(m_path, data);
}
