using System;

namespace Inno.Core.Input;

/// <summary>
/// Captures committed Unicode text at its original position among other input changes.
/// </summary>
public sealed record TextInputChange : InputChange
{
    /// <summary>
    /// Creates a completed text-input fact.
    /// </summary>
    /// <param name="text">
    /// Nonempty committed Unicode text.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the commit.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The text is empty.
    /// </exception>
    public TextInputChange(
        string text,
        KeyModifier modifiers
    ) : base(modifiers) {
        ArgumentException.ThrowIfNullOrEmpty(text);
        this.text = text;
    }

    /// <summary>
    /// Gets committed Unicode text without changing its normalization.
    /// </summary>
    public string text { get; }
}
