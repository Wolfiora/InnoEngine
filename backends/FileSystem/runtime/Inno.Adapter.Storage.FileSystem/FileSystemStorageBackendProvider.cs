using System;
using System.IO;
using Inno.Storage;

namespace Inno.Adapter.Storage.FileSystem;

/// <summary>
/// Supplies the FileSystem implementation through the neutral storage creation boundary.
/// </summary>
public sealed class FileSystemStorageBackendProvider : StorageBackendProvider
{
    private readonly string m_rootDirectory;

    /// <summary>
    /// Captures the host-selected root beneath which application namespaces are isolated.
    /// </summary>
    /// <param name="rootDirectory">
    /// The absolute writable data root owned by the host.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The root is absent or not fully qualified.
    /// </exception>
    public FileSystemStorageBackendProvider(string rootDirectory) : base(StorageBackendId.fileSystem)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        if (!Path.IsPathFullyQualified(rootDirectory))
            throw new ArgumentException("A filesystem storage provider requires an absolute host root.", nameof(rootDirectory));
        m_rootDirectory = Path.GetFullPath(rootDirectory);
    }

    /// <inheritdoc />
    public override IApplicationStorage CreateStorage(StorageScope scope)
    {
        if (!scope.isValid)
            throw new ArgumentException("Storage requires an assigned application namespace.", nameof(scope));
        return new FileSystemApplicationStorage(Path.Combine(m_rootDirectory, scope.value!, "Storage"));
    }
}

