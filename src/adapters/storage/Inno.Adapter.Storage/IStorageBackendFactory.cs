using System.Collections.Generic;
using Inno.Storage;

namespace Inno.Adapter.Storage;

/// <summary>
/// Creates isolated application-storage instances from explicit backend selections.
/// </summary>
public interface IStorageBackendFactory
{
    /// <summary>
    /// Gets the exact registrations available in this composition snapshot.
    /// </summary>
    IReadOnlyList<StorageBackendId> supportedBackends { get; }

    /// <summary>
    /// Creates isolated storage for a logical application namespace.
    /// </summary>
    /// <param name="backend">
    /// storage implementation selected by the composition root.
    /// </param>
    /// <param name="scope">
    /// The application namespace mapped to a location by the selected provider.
    /// </param>
    /// <returns>
    /// A caller-owned backend-neutral application storage service.
    /// </returns>
    /// <exception cref="System.NotSupportedException">
    /// Thrown when the catalog does not contain the selected backend.
    /// </exception>
    IApplicationStorage CreateStorage(
        StorageBackendId backend,
        StorageScope scope
    );

    /// <summary>
    /// Creates a fresh temporary sandbox through the explicitly selected provider.
    /// </summary>
    /// <param name="backend">
    /// The implementation selected by the host; unsupported temporary storage never falls back to persistent data.
    /// </param>
    /// <param name="scope">
    /// The producer namespace, distinct from the identity of each independently created sandbox.
    /// </param>
    /// <returns>
    /// A caller-owned service implementing <see cref="System.IDisposable"/>. Drain accepted operations before
    /// disposing it; successful disposal retires its temporary payload without affecting other sandboxes.
    /// </returns>
    /// <exception cref="System.ArgumentException">
    /// The scope is unassigned.
    /// </exception>
    /// <exception cref="System.NotSupportedException">
    /// The backend is unregistered or does not support temporary storage.
    /// </exception>
    /// <exception cref="System.InvalidOperationException">
    /// The provider violates the disposable ownership contract.
    /// </exception>
    IApplicationStorage CreateTemporaryStorage(
        StorageBackendId backend,
        StorageScope scope
    );
}
