using System;
using Inno.Storage;

namespace Inno.Adapter.Storage.Browser;

/// <summary>
/// Binds the standard application-storage selection to browser-origin persistence.
/// </summary>
public sealed class BrowserStorageBackendFactory : IStorageBackendFactory
{
    /// <summary>
    /// Creates origin-persistent application storage for the standard storage backend.
    /// </summary>
    /// <param name="backend">
    /// The requested application storage backend.
    /// </param>
    /// <param name="rootDirectory">
    /// The application-owned storage directory used to isolate browser keys.
    /// </param>
    /// <returns>
    /// The origin-scoped application storage implementation.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The requested backend is unavailable in a browser.
    /// </exception>
    public IApplicationStorage CreateStorage(
        StorageBackend backend,
        string rootDirectory
    ) {
        if (backend != StorageBackend.FileSystem)
            throw new NotSupportedException($"Storage backend '{backend}' is unavailable in the browser.");
        return new BrowserApplicationStorage(rootDirectory);
    }
}
