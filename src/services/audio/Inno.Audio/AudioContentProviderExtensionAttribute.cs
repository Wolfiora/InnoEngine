using System;

namespace Inno.Audio;

/// <summary>
/// Marks a reloadable provider that converts host content into immutable audio snapshots.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class AudioContentProviderExtensionAttribute : Attribute
{
    /// <summary>
    /// Creates an audio content provider declaration.
    /// </summary>
    /// <param name="id">
    /// Globally stable provider identifier.
    /// </param>
    /// <param name="priority">
    /// Provider invocation priority; lower values run first.
    /// </param>
    public AudioContentProviderExtensionAttribute(
        string id,
        int priority = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        this.id = id;
        this.priority = priority;
    }

    /// <summary>
    /// Gets the globally stable provider identifier.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the provider invocation priority.
    /// </summary>
    public int priority { get; }
}
