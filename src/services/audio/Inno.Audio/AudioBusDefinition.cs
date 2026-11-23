using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Audio;

/// <summary>
/// Describes one bus and its ordered processor chain in a compiled mixer graph.
/// </summary>
public sealed class AudioBusDefinition
{
    private readonly AudioProcessorConfiguration[] m_processors;

    /// <summary>
    /// Creates an immutable mixer bus definition.
    /// </summary>
    /// <param name="id">
    /// Stable bus identifier.
    /// </param>
    /// <param name="parent">
    /// Parent bus identifier, or <see langword="null"/> only for the master bus.
    /// </param>
    /// <param name="volume">
    /// Non-negative linear bus gain.
    /// </param>
    /// <param name="muted">
    /// Whether output from the bus is initially silenced.
    /// </param>
    /// <param name="processors">
    /// Ordered processor chain.
    /// </param>
    public AudioBusDefinition(
        AudioBusID id,
        AudioBusID? parent,
        float volume,
        bool muted,
        IEnumerable<AudioProcessorConfiguration>? processors = null
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A valid bus identifier is required.", nameof(id));
        }
        if (volume < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(volume));
        }
        if (id == AudioBusID.master && parent is not null)
        {
            throw new ArgumentException("The master bus cannot have a parent.", nameof(parent));
        }
        if (id != AudioBusID.master && (parent is null || !parent.Value.isValid))
        {
            throw new ArgumentException("A non-master bus requires a valid parent.", nameof(parent));
        }
        this.id = id;
        this.parent = parent;
        this.volume = volume;
        this.muted = muted;
        m_processors = processors?.ToArray() ?? [];
    }

    /// <summary>
    /// Gets the stable bus identifier.
    /// </summary>
    public AudioBusID id { get; }

    /// <summary>
    /// Gets the parent bus, or <see langword="null"/> for the master bus.
    /// </summary>
    public AudioBusID? parent { get; }

    /// <summary>
    /// Gets the initial linear bus gain.
    /// </summary>
    public float volume { get; }

    /// <summary>
    /// Gets whether output from the bus is initially silenced.
    /// </summary>
    public bool muted { get; }

    /// <summary>
    /// Gets the ordered immutable processor chain.
    /// </summary>
    public IReadOnlyList<AudioProcessorConfiguration> processors => Array.AsReadOnly(m_processors);
}
