using System;
using Inno.Storage;

namespace Inno.Adapter.Storage.BrowserWasm32;

/// <summary>
/// Supplies the Browser implementation through the neutral storage creation boundary.
/// </summary>
public sealed class BrowserWasm32StorageBackendProvider : StorageBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public BrowserWasm32StorageBackendProvider() : base(StorageBackendId.browser) { }

    /// <inheritdoc />
    public override IApplicationStorage CreateStorage(StorageScope scope)
    {
        return new BrowserWasm32ApplicationStorage(scope);
    }
}

