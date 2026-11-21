using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Text;

namespace Inno.Adapter.Text;

/// <summary>
/// Resolves text providers from one immutable, composition-owned registration snapshot.
/// </summary>
public sealed class TextBackendCatalog : ITextBackendFactory
{
    private readonly Dictionary<TextBackendId, TextBackendProvider> m_providers = [];

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
    public TextBackendCatalog(IEnumerable<TextBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (TextBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
            {
                throw new ArgumentException("Providers require unique, assigned backend IDs.", nameof(providers));
            }
        }
        this.supportedBackends = new ReadOnlyCollection<TextBackendId>([.. m_providers.Keys]);
    }

    /// <inheritdoc/>
    public IReadOnlyList<TextBackendId> supportedBackends { get; }

    /// <exception cref="InvalidOperationException">
    /// The selected provider returned no service.
    /// </exception>
    /// <inheritdoc/>
    public ITextBackend CreateBackend(TextBackendId backend)
    {
        if (!m_providers.TryGetValue(backend, out TextBackendProvider? provider))
        {
            throw new NotSupportedException("Text backend '" + backend + "' is not registered.");
        }
        return provider.CreateBackend()
               ?? throw new InvalidOperationException("Text provider '" + backend + "' returned no service.");
    }
}
