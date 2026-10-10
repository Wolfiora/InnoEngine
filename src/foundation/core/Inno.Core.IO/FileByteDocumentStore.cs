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
    public byte[]? Read()
    {
        try
        {
            return File.ReadAllBytes(m_path);
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
