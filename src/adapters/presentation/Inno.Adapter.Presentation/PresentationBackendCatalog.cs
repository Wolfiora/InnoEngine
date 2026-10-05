using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.Adapter.Presentation;

/// <summary>
/// Resolves presentation providers from one immutable, composition-owned registration snapshot.
/// </summary>
public sealed class PresentationBackendCatalog : IPresentationBackendFactory
{
    private readonly Dictionary<PresentationBackendId, PresentationBackendProvider> m_providers = [];

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
    public PresentationBackendCatalog(IEnumerable<PresentationBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (PresentationBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
                throw new ArgumentException("Providers require unique, assigned backend IDs.", nameof(providers));
        }
        supportedBackends = new ReadOnlyCollection<PresentationBackendId>(new List<PresentationBackendId>(m_providers.Keys));
    }

    /// <inheritdoc />
    public IReadOnlyList<PresentationBackendId> supportedBackends { get; }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// The selected provider returned no service.
    /// </exception>
    public IPresentationContext CreateContext(
        PresentationBackendId backend,
        PresentationBackendOptions options
    ) {
        ArgumentNullException.ThrowIfNull(options);
        if (!m_providers.TryGetValue(backend, out PresentationBackendProvider? provider))
            throw new NotSupportedException("Presentation backend '" + backend + "' is not registered.");
        return provider.CreateContext(options)
            ?? throw new InvalidOperationException("Presentation provider '" + backend + "' returned no context.");
    }
}
