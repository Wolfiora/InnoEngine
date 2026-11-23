using System;

namespace Inno.Audio;

/// <summary>
/// Marks a reloadable extension that creates the base audio mixer graph.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AudioMixerExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a mixer extension declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable mixer extension identifier.
    /// </param>
    public AudioMixerExtensionAttribute(string id)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
    }

    /// <summary>
    /// Gets the globally stable mixer extension identifier.
    /// </summary>
    public string id { get; }
}
