using System;

namespace Inno.Audio;

/// <summary>
/// Associates an stream decoder with its stable codec protocol ID.
/// </summary>
/// <param name="id">
/// The case-sensitive codec identifier used by imported clips.
/// </param>
[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class AudioStreamDecoderAttribute(string id) : Attribute
{
    /// <summary>
    /// Gets the codec protocol ID.
    /// </summary>
    public string id { get; } = new AudioCodecID(id).value;
}
