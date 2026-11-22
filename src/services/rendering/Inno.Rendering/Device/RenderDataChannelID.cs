using System;

namespace Inno.Rendering;

/// <summary>
/// Identifies one pipeline-owned frame-data channel without constraining its payload model.
/// </summary>
public readonly record struct RenderDataChannelID
{
    /// <summary>
    /// Creates an open frame-data channel identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable channel protocol value.
    /// </param>
    public RenderDataChannelID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the globally stable protocol value.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether the identifier contains a protocol value.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override string ToString() => value;
}
