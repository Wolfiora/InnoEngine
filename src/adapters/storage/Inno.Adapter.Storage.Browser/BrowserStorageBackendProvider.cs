using System;
using Inno.Storage;

namespace Inno.Adapter.Storage.Browser;

/// <summary>
/// Supplies the Browser implementation through the neutral storage creation boundary.
/// </summary>
public sealed class BrowserStorageBackendProvider : StorageBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public BrowserStorageBackendProvider() : base(StorageBackendId.browser) { }

    /// <inheritdoc />
    public override IApplicationStorage CreateStorage(string rootDirectory)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(rootDirectory);
        return new BrowserApplicationStorage(rootDirectory);
    }
}

