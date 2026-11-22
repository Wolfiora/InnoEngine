using System;

namespace Inno.Audio;

/// <summary>
/// Describes one immutable listener candidate produced by a content provider.
/// </summary>
public readonly record struct AudioListenerSnapshot
{
    /// <summary>
    /// Creates an immutable listener snapshot.
    /// </summary>
    /// <param name="id">
    /// Stable listener identity across updates.
    /// </param>
    /// <param name="priority">
    /// Selection priority; larger values win.
    /// </param>
    /// <param name="state">
    /// Current listener transform.
    /// </param>
    /// <param name="active">
    /// Whether the listener is eligible for selection.
    /// </param>
    public AudioListenerSnapshot(
        Guid id,
        int priority,
        AudioListenerState state,
        bool active
    ) {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("A listener identity cannot be empty.", nameof(id));
        }
        this.id = id;
        this.priority = priority;
        this.state = state;
        this.active = active;
    }

    /// <summary>
    /// Gets the stable listener identity.
    /// </summary>
    public Guid id { get; }

    /// <summary>
    /// Gets the selection priority.
    /// </summary>
    public int priority { get; }

    /// <summary>
    /// Gets the current listener transform.
    /// </summary>
    public AudioListenerState state { get; }

    /// <summary>
    /// Gets whether the listener is eligible for selection.
    /// </summary>
    public bool active { get; }
}
