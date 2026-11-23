using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Storage;

namespace Inno.Adapter.Storage;

/// <summary>
/// Resolves storage providers from one immutable, composition-owned registration snapshot.
/// </summary>
public sealed class StorageBackendCatalog : IStorageBackendFactory
{
    private readonly Dictionary<StorageBackendID, StorageBackendProvider> m_providers = [];

    /// <summary>
    /// Validates and captures a complete provider set without creating any service.
    /// </summary>
    /// <param name="providers">
    /// Providers whose lifetime remains with the composition owner.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The provider sequence is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A provider is null, unassigned, or duplicates a registration identity.
    /// </exception>
    public StorageBackendCatalog(IEnumerable<StorageBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (StorageBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
            {
                throw new ArgumentException("Providers require unique, assigned backend IDs.", nameof(providers));
            }
        }
        this.supportedBackends = new ReadOnlyCollection<StorageBackendID>([.. m_providers.Keys]);
    }

    /// <inheritdoc/>
    public IReadOnlyList<StorageBackendID> supportedBackends { get; }

    /// <exception cref="InvalidOperationException">
    /// The selected provider returned no service.
    /// </exception>
    /// <inheritdoc/>
    public IApplicationStorage CreateStorage(
        StorageBackendID backend,
        StorageScope scope
    ) {
        if (!scope.isValid)
        {
            throw new ArgumentException("Storage requires an assigned application namespace.", nameof(scope));
        }
        if (!m_providers.TryGetValue(backend, out StorageBackendProvider? provider))
        {
            throw new NotSupportedException("Storage backend '" + backend + "' is not registered.");
        }
        return provider.CreateStorage(scope)
               ?? throw new InvalidOperationException("Storage provider '" + backend + "' returned no service.");
    }
}
