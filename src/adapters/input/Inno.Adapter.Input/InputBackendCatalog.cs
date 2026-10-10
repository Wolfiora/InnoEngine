using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Platform;

namespace Inno.Adapter.Input;

/// <summary>
/// Resolves input providers from one immutable, composition-owned registration snapshot.
/// </summary>
public sealed class InputBackendCatalog : IInputBackendFactory
{
    private readonly Dictionary<InputBackendId, InputBackendProvider> m_providers = [];

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
    public InputBackendCatalog(IEnumerable<InputBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (InputBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
                throw new ArgumentException("Providers require unique, assigned backend IDs.", nameof(providers));
        }
        supportedBackends = new ReadOnlyCollection<InputBackendId>(new List<InputBackendId>(m_providers.Keys));
    }

    /// <inheritdoc />
    public IReadOnlyList<InputBackendId> supportedBackends { get; }

    /// <inheritdoc />
    /// <exception cref="InvalidOperationException">
    /// The selected provider returned no service.
    /// </exception>
    public IInputEventSource CreateEventSource(
        InputBackendId backend,
        IPlatformWindow window,
        bool acceptAllWindows
    ) {
        ArgumentNullException.ThrowIfNull(window);
        if (!m_providers.TryGetValue(backend, out InputBackendProvider? provider))
            throw new NotSupportedException("Input backend '" + backend + "' is not registered.");
        return provider.CreateEventSource(window, acceptAllWindows)
            ?? throw new InvalidOperationException("Input provider '" + backend + "' returned no service.");
    }
}

