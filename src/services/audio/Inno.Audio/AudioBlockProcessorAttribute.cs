using System;

namespace Inno.Audio;

/// <summary>
/// Associates an external sample processor with its stable mixer protocol ID.
/// </summary>
/// <param name="id">
/// The case-sensitive processor identifier used by mixer configurations.
/// </param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AudioBlockProcessorAttribute(string id) : Attribute
{
    /// <summary>
    /// Gets the processor protocol ID.
    /// </summary>
    public string id { get; } = new AudioProcessorID(id).value;
}
