using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Audio;

namespace Inno.Adapter.Audio;

/// <summary>
/// Resolves audio providers from one immutable, composition-owned registration snapshot.
/// </summary>
public sealed class AudioBackendCatalog : IAudioBackendFactory
{
    private readonly Dictionary<AudioBackendId, AudioBackendProvider> m_providers = [];

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
    public AudioBackendCatalog(IEnumerable<AudioBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (AudioBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
                throw new ArgumentException("Providers require unique, assigned backend IDs.", nameof(providers));
        }
        supportedBackends = new ReadOnlyCollection<AudioBackendId>(new List<AudioBackendId>(m_providers.Keys));
    }

    /// <inheritdoc />
    public IReadOnlyList<AudioBackendId> supportedBackends { get; }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// The selected provider returned no service.
    /// </exception>
    public IAudioDevice CreateDevice(
        AudioBackendId backend,
        AudioBackendOptions options
    ) {
        if (!m_providers.TryGetValue(backend, out AudioBackendProvider? provider))
            throw new NotSupportedException("Audio backend '" + backend + "' is not registered.");
        return provider.CreateDevice(options)
            ?? throw new InvalidOperationException("Audio provider '" + backend + "' returned no service.");
    }
}

