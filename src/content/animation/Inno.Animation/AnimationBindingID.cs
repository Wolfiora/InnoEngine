using System;

namespace Inno.Animation;

/// <summary>
/// Identifies one backend-neutral value destination inside an animation binding scope.
/// </summary>
public readonly record struct AnimationBindingID
{
    /// <summary>
    /// Creates a stable animation binding identifier.
    /// </summary>
    /// <param name="value">
    /// The non-empty binding protocol value.
    /// </param>
    public AnimationBindingID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value;
    }

    /// <summary>
    /// Gets the stable binding protocol value.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether this identifier contains a usable value.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this identifier for diagnostics and persistence.
    /// </summary>
    /// <returns>
    /// The protocol value, or an empty string for an uninitialized identifier.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}
