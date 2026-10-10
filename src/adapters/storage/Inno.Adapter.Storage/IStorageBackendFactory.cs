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
}
