using System;
using Inno.Storage;

namespace Inno.Adapter.Storage.FileSystem;

/// <summary>
/// Supplies the FileSystem implementation through the neutral storage creation boundary.
/// </summary>
public sealed class FileSystemStorageBackendProvider : StorageBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public FileSystemStorageBackendProvider() : base(StorageBackendId.fileSystem) { }

    /// <inheritdoc />
    public override IApplicationStorage CreateStorage(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        return new FileSystemApplicationStorage(rootDirectory);
    }
}

