using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Audio;

/// <summary>
/// Describes one ordered processor instance attached to a mixer bus.
/// </summary>
public sealed class AudioProcessorConfiguration
{
    private readonly AudioProcessorParameter[] m_parameters;

    /// <summary>
    /// Creates an immutable processor configuration.
    /// </summary>
    /// <param name="id">
    /// Open processor protocol identifier.
    /// </param>
    /// <param name="parameters">
    /// Complete processor parameter set.
    /// </param>
    public AudioProcessorConfiguration(
        AudioProcessorID id,
        IEnumerable<AudioProcessorParameter>? parameters = null
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A valid processor identifier is required.", nameof(id));
        }
        this.id = id;
        m_parameters = parameters?.ToArray() ?? [];
        if (m_parameters.Select(static parameter => parameter.id).Distinct().Count() != m_parameters.Length)
        {
            throw new ArgumentException("Processor parameter identifiers must be unique.", nameof(parameters));
        }
    }

    /// <summary>
    /// Gets the open processor protocol identifier.
    /// </summary>
    public AudioProcessorID id { get; }

    /// <summary>
    /// Gets the immutable processor parameter set.
    /// </summary>
    public IReadOnlyList<AudioProcessorParameter> parameters => Array.AsReadOnly(m_parameters);
}
