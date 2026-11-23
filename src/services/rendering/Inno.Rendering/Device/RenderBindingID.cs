using System;

namespace Inno.Rendering;

/// <summary>
/// Identifies a shader binding by stable manifest name.
/// </summary>
public readonly record struct RenderBindingID
{
    /// <summary>
    /// Creates a stable shader binding identifier.
    /// </summary>
    /// <param name="value">
    /// Stable manifest binding name.
    /// </param>
    public RenderBindingID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the stable manifest binding name.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether the identifier contains a stable manifest name.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);
}
