using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Audio;

/// <summary>
/// Builds and validates an open backend-neutral mixer graph.
/// </summary>
public sealed class AudioMixerBuilder
{
    private readonly Dictionary<AudioBusID, AudioBusDefinition> m_buses = new()
    {
        [AudioBusID.master] = new(AudioBusID.master, null, 1f, false)
    };

    /// <summary>
    /// Adds one semantic bus to the graph.
    /// </summary>
    /// <param name="id">
    /// Stable bus identifier.
    /// </param>
    /// <param name="parent">
    /// Parent bus identifier.
    /// </param>
    /// <param name="volume">
    /// Initial non-negative linear gain.
    /// </param>
    /// <param name="muted">
    /// Whether output is initially silenced.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the bus identifier is invalid or duplicated.
    /// </exception>
    public void AddBus(
        AudioBusID id,
        AudioBusID parent,
        float volume = 1f,
        bool muted = false
    ) {
        if (!id.isValid || id == AudioBusID.master)
        {
            throw new ArgumentException("A valid non-master bus identifier is required.", nameof(id));
        }
        if (m_buses.ContainsKey(id))
        {
            throw new ArgumentException($"Audio bus '{id}' is already defined.", nameof(id));
        }
        m_buses.Add(id, new AudioBusDefinition(id, parent, volume, muted));
    }

    /// <summary>
    /// Appends one processor to an existing bus chain.
    /// </summary>
    /// <param name="bus">
    /// Existing bus identifier.
    /// </param>
    /// <param name="processor">
    /// Immutable processor configuration to append.
    /// </param>
    /// <exception cref="KeyNotFoundException">
    /// Thrown when the bus is not defined.
    /// </exception>
    public void AddProcessor(
        AudioBusID bus,
        AudioProcessorConfiguration processor
    ) {
        ArgumentNullException.ThrowIfNull(processor);
        if (!m_buses.TryGetValue(bus, out AudioBusDefinition? definition))
        {
            throw new KeyNotFoundException($"Audio bus '{bus}' is not defined.");
        }
        m_buses[bus] = new AudioBusDefinition(
            definition.id,
            definition.parent,
            definition.volume,
            definition.muted,
            definition.processors.Append(processor)
        );
    }

    /// <summary>
    /// Validates the graph and returns an immutable parent-before-child snapshot.
    /// </summary>
    /// <returns>
    /// A validated mixer graph containing the mandatory master bus.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Thrown when a parent is missing or bus routing contains a cycle.
    /// </exception>
    public AudioMixer Build()
    {
        var order = new List<AudioBusDefinition>(m_buses.Count);
        var visiting = new HashSet<AudioBusID>();
        var visited = new HashSet<AudioBusID>();
        Visit(AudioBusID.master, order, visiting, visited);
        foreach (AudioBusID id in m_buses.Keys.OrderBy(static id => id.value, StringComparer.Ordinal))
        {
            Visit(id, order, visiting, visited);
        }
        return new AudioMixer(order);
    }

    private void Visit(
        AudioBusID id,
        ICollection<AudioBusDefinition> order,
        ISet<AudioBusID> visiting,
        ISet<AudioBusID> visited
    ) {
        if (visited.Contains(id))
        {
            return;
        }
        if (!m_buses.TryGetValue(id, out AudioBusDefinition? definition))
        {
            throw new InvalidOperationException($"Audio mixer references missing bus '{id}'.");
        }
        if (!visiting.Add(id))
        {
            throw new InvalidOperationException($"Audio mixer bus routing contains a cycle at '{id}'.");
        }
        if (definition.parent is AudioBusID parent)
        {
            Visit(parent, order, visiting, visited);
        }
        visiting.Remove(id);
        visited.Add(id);
        order.Add(definition);
    }
}
