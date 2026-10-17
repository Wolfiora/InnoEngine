using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Platform;

namespace Inno.Adapter.Platform;

/// <summary>
/// Resolves platform providers from one immutable, composition-owned registration snapshot.
/// </summary>
public sealed class PlatformBackendCatalog : IPlatformBackendFactory
{
    private readonly Dictionary<PlatformBackendId, PlatformBackendProvider> m_providers = [];

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
    public PlatformBackendCatalog(IEnumerable<PlatformBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (PlatformBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
                throw new ArgumentException("Providers require unique, assigned backend IDs.", nameof(providers));
        }
        supportedBackends = new ReadOnlyCollection<PlatformBackendId>(new List<PlatformBackendId>(m_providers.Keys));
    }

    /// <inheritdoc />
    public IReadOnlyList<PlatformBackendId> supportedBackends { get; }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// The selected provider returned no service.
    /// </exception>
    public IPlatformApplication CreateApplication(PlatformBackendId backend) {
        if (!m_providers.TryGetValue(backend, out PlatformBackendProvider? provider))
            throw new NotSupportedException("Platform backend '" + backend + "' is not registered.");
        return provider.CreateApplication()
            ?? throw new InvalidOperationException("Platform provider '" + backend + "' returned no service.");
    }
}

