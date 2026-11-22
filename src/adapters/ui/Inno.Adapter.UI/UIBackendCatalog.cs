using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.UI;

namespace Inno.Adapter.UI;

/// <summary>
/// Resolves UI providers from one immutable composition-owned registration snapshot.
/// </summary>
public sealed class UIBackendCatalog : IUIBackendFactory
{
    private readonly Dictionary<UIBackendID, UIBackendProvider> m_providers = [];

    /// <summary>
    /// Validates and captures a complete provider set.
    /// </summary>
    /// <param name="providers">
    /// Providers owned by this composition generation.
    /// </param>
    public UIBackendCatalog(IEnumerable<UIBackendProvider> providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        foreach (UIBackendProvider provider in providers)
        {
            if (provider is null || !provider.id.isValid || !m_providers.TryAdd(provider.id, provider))
            {
                throw new ArgumentException("UI providers require unique, assigned backend IDs.", nameof(providers));
            }
        }
        this.supportedBackends = new ReadOnlyCollection<UIBackendID>([.. m_providers.Keys]);
    }

    /// <summary>
    /// Gets backend registrations available in this type generation.
    /// </summary>
    public IReadOnlyList<UIBackendID> supportedBackends { get; }

    /// <summary>
    /// Creates a backend using this implementation's validated inputs.
    /// </summary>
    /// <param name="backend">
    /// The backend consumed by create backend; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated iui backend that represents the completed operation.
    /// </returns>
    public IUIBackend CreateBackend(UIBackendID backend)
    {
        if (!m_providers.TryGetValue(backend, out UIBackendProvider? provider))
        {
            throw new NotSupportedException($"UI backend '{backend}' is not registered.");
        }
        return provider.CreateBackend()
               ?? throw new InvalidOperationException($"UI provider '{backend}' returned no backend.");
    }
}
