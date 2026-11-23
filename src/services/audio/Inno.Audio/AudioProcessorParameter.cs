using System;

namespace Inno.Audio;

/// <summary>
/// Stores one neutral numeric parameter assigned to an audio processor.
/// </summary>
public readonly record struct AudioProcessorParameter
{
    /// <summary>
    /// Creates a processor parameter value.
    /// </summary>
    /// <param name="id">
    /// Open parameter identifier understood by the processor protocol.
    /// </param>
    /// <param name="value">
    /// Neutral numeric value interpreted by the processor.
    /// </param>
    public AudioProcessorParameter(
        AudioParameterID id,
        float value
    ) {
        if (!id.isValid)
        {
            throw new ArgumentException("A valid parameter identifier is required.", nameof(id));
        }
        this.id = id;
        this.value = value;
    }

    /// <summary>
    /// Gets the open parameter identifier.
    /// </summary>
    public AudioParameterID id { get; }

    /// <summary>
    /// Gets the neutral numeric value.
    /// </summary>
    public float value { get; }
}
