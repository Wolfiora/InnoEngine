using System;

namespace Inno.UI;

/// <summary>
/// Stores an immutable control value without backend objects or document ownership.
/// </summary>
public readonly record struct UiControlValue
{
    private UiControlValue(
        UiControlValueKind kind,
        string? text,
        bool boolean,
        double number
    ) {
        this.kind = kind;
        this.text = text;
        this.boolean = boolean;
        this.number = number;
    }

    /// <summary>
    /// Gets the active value kind; the default value represents no control.
    /// </summary>
    public UiControlValueKind kind { get; }
    /// <summary>
    /// Gets text or the selected option value; null for all other kinds.
    /// </summary>
    public string? text { get; }
    /// <summary>
    /// Gets the checked value when kind is Boolean; otherwise false.
    /// </summary>
    public bool boolean { get; }
    /// <summary>
    /// Gets the finite value when kind is Number; otherwise zero.
    /// </summary>
    public double number { get; }

    /// <summary>
    /// Captures an immutable text value.
    /// </summary>
    /// <param name="text">
    /// The non-null text, which may be empty.
    /// </param>
    /// <returns>
    /// A text control value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Text is null.
    /// </exception>
    public static UiControlValue Text(string text) =>
        new(UiControlValueKind.Text, text ?? throw new ArgumentNullException(nameof(text)), false, 0);

    /// <summary>
    /// Captures a checked state.
    /// </summary>
    /// <param name="value">
    /// Whether the control is checked.
    /// </param>
    /// <returns>
    /// A boolean control value.
    /// </returns>
    public static UiControlValue Boolean(bool value) => new(UiControlValueKind.Boolean, null, value, 0);

    /// <summary>
    /// Captures a finite numeric value.
    /// </summary>
    /// <param name="value">
    /// The requested finite value.
    /// </param>
    /// <returns>
    /// A numeric control value.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The value is not finite.
    /// </exception>
    public static UiControlValue Number(double value)
    {
        if (!double.IsFinite(value))
            throw new ArgumentOutOfRangeException(nameof(value));
        return new(UiControlValueKind.Number, null, false, value);
    }

    /// <summary>
    /// Captures the authored value of a selected option.
    /// </summary>
    /// <param name="value">
    /// The non-null option value, which may be empty.
    /// </param>
    /// <returns>
    /// A selection control value.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The option value is null.
    /// </exception>
    public static UiControlValue Selection(string value) =>
        new(UiControlValueKind.Selection, value ?? throw new ArgumentNullException(nameof(value)), false, 0);
}
