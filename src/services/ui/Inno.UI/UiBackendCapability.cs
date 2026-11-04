using System;

namespace Inno.UI;

/// <summary>
/// Identifies an optional backend behavior using an open, provider-qualified stable value.
/// </summary>
public readonly record struct UiBackendCapability
{
    /// <summary>
    /// Selecting an explicit nonzero face from a font collection.
    /// </summary>
    public static readonly UiBackendCapability FontCollectionFaceSelection = new("inno.ui.font-collection-face");
    /// <summary>
    /// Seeking context time without replacing the document or losing interaction state.
    /// </summary>
    public static readonly UiBackendCapability TimeSeek = new("inno.ui.time-seek");

    /// <summary>
    /// Creates an optional behavior identity.
    /// </summary>
    /// <param name="value">
    /// A nonempty provider-qualified value without whitespace.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The value is empty or contains whitespace.
    /// </exception>
    public UiBackendCapability(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        foreach (char character in value)
            if (char.IsWhiteSpace(character))
                throw new ArgumentException("A UI capability cannot contain whitespace.", nameof(value));
        this.value = value;
    }

    /// <summary>
    /// Gets the stable capability value.
    /// </summary>
    public string value { get; } = string.Empty;
    /// <summary>
    /// Gets whether this identity is assigned.
    /// </summary>
    public bool isValid => !string.IsNullOrWhiteSpace(value);
    /// <summary>
    /// Returns the assigned identity, or an empty string for the default value.
    /// </summary>
    /// <returns>
    /// The stable textual identity.
    /// </returns>
    public override string ToString() => value ?? string.Empty;
}
