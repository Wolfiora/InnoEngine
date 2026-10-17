using System;

namespace Inno.Adapter.Presentation;

/// <summary>
/// Describes an explicitly composed host presentation implementation and its creation boundary.
/// </summary>
public abstract class PresentationBackendProvider
{
    /// <summary>
    /// Captures the implementation identity assigned by the composition owner.
    /// </summary>
    /// <param name="id">
    /// The assigned, immutable implementation identifier.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The identifier is unassigned.
    /// </exception>
    protected PresentationBackendProvider(PresentationBackendId id)
    {
        if (!id.isValid)
            throw new ArgumentException("A provider requires an assigned backend ID.", nameof(id));
        this.id = id;
    }

    /// <summary>
    /// Gets this registration's immutable implementation identity.
    /// </summary>
    public PresentationBackendId id { get; }

    /// <summary>
    /// Creates a new caller-owned presentation context over the supplied host resources.
    /// </summary>
    /// <param name="options">
    /// Externally owned platform and rendering resources, together with the requested presentation policy.
    /// </param>
    /// <returns>
    /// A new context whose caller must dispose it before releasing the supplied resources.
    /// Providers must not return null.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The host resources or requested features are incompatible with this implementation.
    /// </exception>
    public abstract IPresentationContext CreateContext(PresentationBackendOptions options);
}
