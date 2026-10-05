using System;
using Inno.Platform;

namespace Inno.Adapter.Input;

/// <summary>
/// Describes one explicitly composed input implementation and its creation boundary.
/// </summary>
/// <remarks>
/// The composition owns this provider and its immutable registration identity.
/// Products are owned by their callers. This registration does not perform type discovery.
/// </remarks>
public abstract class InputBackendProvider
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
    protected InputBackendProvider(InputBackendId id)
    {
        if (!id.isValid)
            throw new ArgumentException("A provider requires an assigned backend ID.", nameof(id));
        this.id = id;
    }

    /// <summary>
    /// Gets this registration's immutable implementation identity.
    /// </summary>
    public InputBackendId id { get; }

    /// <summary>
    /// Creates a caller-owned input service using this implementation.
    /// </summary>
    /// <param name="window">
    /// The window supplying accepted input events.
    /// </param>
    /// <param name="acceptAllWindows">
    /// Whether events from every application window are accepted.
    /// </param>
    /// <returns>
    /// A new service owned by the caller; providers must not return null.
    /// </returns>
    public abstract IInputEventSource CreateEventSource(
        IPlatformWindow window,
        bool acceptAllWindows
    );
}

