using System;

namespace Inno.Core.Events;

/// <summary>
/// Raised when the platform commits UTF-8 text through keyboard, IME, or virtual-keyboard input.
/// </summary>
/// <param name="windowID">
/// The source window identifier.
/// </param>
/// <param name="text">
/// The committed Unicode text.
/// </param>
public sealed class TextInputEvent(
    uint windowID,
    string text
) : Event
{
    /// <summary>
    /// Gets the source window identifier.
    /// </summary>
    public uint windowID { get; } = windowID;

    /// <summary>
    /// Gets the committed Unicode text.
    /// </summary>
    public string text { get; } = string.IsNullOrEmpty(text)
                                  ? throw new ArgumentException("Committed text cannot be empty.", nameof(text))
                                  : text;
}
