using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Identifies one open Editor viewport purpose without prescribing rendering semantics.
/// </summary>
public readonly record struct EditorViewportKindID
{
    /// <summary>
    /// Creates a stable viewport purpose identifier.
    /// </summary>
    /// <param name="value">
    /// Globally stable viewport purpose.
    /// </param>
    public EditorViewportKindID(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        this.value = value.Trim();
    }

    /// <summary>
    /// Gets the stable viewport purpose.
    /// </summary>
    public string value { get; }

    /// <summary>
    /// Gets whether this identifier is usable.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);

    /// <summary>
    /// Formats this value as a human-readable representation.
    /// </summary>
    /// <returns>
    /// The human-readable representation of this value.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}
