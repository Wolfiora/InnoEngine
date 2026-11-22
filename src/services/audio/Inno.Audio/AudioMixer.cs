using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Audio;

/// <summary>
/// Contains one validated backend-neutral mixer graph.
/// </summary>
public sealed class AudioMixer
{
    private readonly AudioBusDefinition[] m_buses;

    internal AudioMixer(IEnumerable<AudioBusDefinition> buses)
    {
        m_buses = buses.ToArray();
    }

    /// <summary>
    /// Gets buses in parent-before-child creation order.
    /// </summary>
    public IReadOnlyList<AudioBusDefinition> buses => Array.AsReadOnly(m_buses);
}
