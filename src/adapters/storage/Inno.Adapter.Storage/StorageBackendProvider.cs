using System;
using Inno.Storage;

namespace Inno.Adapter.Storage;

/// <summary>
/// Describes one explicitly composed storage implementation and its creation boundary.
/// </summary>
/// <remarks>
/// The composition owns this provider and its immutable registration identity.
/// Products are owned by their callers. This registration does not perform type discovery.
/// </remarks>
public abstract class StorageBackendProvider
{
    /// <summary>
    /// Captures the identity assigned by the composition owner.
    /// </summary>
    /// <param name="id">
    /// The assigned implementation identity.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity is unassigned.
    /// </exception>
    protected StorageBackendProvider(StorageBackendId id)
    {
        if (!id.isValid)
            throw new ArgumentException("A provider requires an assigned backend ID.", nameof(id));
        this.id = id;
    }

    /// <summary>
    /// Gets this registration's immutable implementation identity.
    /// </summary>
    public StorageBackendId id { get; }

    /// <summary>
    /// Creates a caller-owned storage service using this implementation.
    /// </summary>
    /// <param name="rootDirectory">
    /// The application-owned root or origin namespace for stored data.
    /// </param>
    /// <returns>
    /// A new service owned by the caller; providers must not return null.
    /// </returns>
    public abstract IApplicationStorage CreateStorage(string rootDirectory);
}

