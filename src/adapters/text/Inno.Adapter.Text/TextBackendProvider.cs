using System;
using Inno.Text;

namespace Inno.Adapter.Text;

/// <summary>
/// Describes one explicitly composed text implementation and its creation boundary.
/// </summary>
/// <remarks>
/// The composition owns this provider and its immutable registration identity.
/// Products are owned by their callers. This registration does not perform type discovery.
/// </remarks>
public abstract class TextBackendProvider
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
    protected TextBackendProvider(TextBackendId id)
    {
        if (!id.isValid)
            throw new ArgumentException("A provider requires an assigned backend ID.", nameof(id));
        this.id = id;
    }

    /// <summary>
    /// Gets this registration's immutable implementation identity.
    /// </summary>
    public TextBackendId id { get; }

    /// <summary>
    /// Creates a caller-owned text service using this implementation.
    /// </summary>
    /// <returns>
    /// A new service owned by the caller; providers must not return null.
    /// </returns>
    public abstract ITextBackend CreateBackend();
}

