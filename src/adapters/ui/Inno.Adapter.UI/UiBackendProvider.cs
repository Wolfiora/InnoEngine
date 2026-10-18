using System;
using Inno.UI;

namespace Inno.Adapter.UI;

/// <summary>
/// Creates one concrete UI backend registered by a composition root.
/// </summary>
public abstract class UiBackendProvider
{
    /// <summary>
    /// Captures the registration identity assigned by the composition owner.
    /// </summary>
    /// <param name="id">
    /// The assigned implementation identity.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identity is unassigned.
    /// </exception>
    protected UiBackendProvider(UiBackendId id)
    {
        if (!id.isValid)
            throw new ArgumentException("A provider requires an assigned backend ID.", nameof(id));
        this.id = id;
    }

    /// <summary>
    /// Gets this provider's stable implementation identity.
    /// </summary>
    public UiBackendId id { get; }

    /// <summary>
    /// Creates one caller-owned backend generation.
    /// </summary>
    /// <returns>
    /// A new backend.
    /// </returns>
    public abstract IUiBackend CreateBackend();
}
