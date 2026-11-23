using System;

namespace Inno.Audio;

/// <summary>
/// Marks a reloadable extension that contributes to an audio mixer graph.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AudioMixerFeatureExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates a mixer feature declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable feature extension identifier.
    /// </param>
    /// <param name="priority">
    /// Feature invocation priority; lower values build first.
    /// </param>
    public AudioMixerFeatureExtensionAttribute(
        string id,
        int priority = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
        this.priority = priority;
    }

    /// <summary>
    /// Gets the globally stable feature extension identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the feature invocation priority.
    /// </summary>
    public int priority { get; }
}
