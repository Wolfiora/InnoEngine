namespace Inno.UI;

/// <summary>
/// Identifies the neutral value carried by a UI control or event.
/// </summary>
public enum UiControlValueKind
{
    /// <summary>
    /// The element has no supported control value.
    /// </summary>
    None,
    /// <summary>
    /// A Unicode text value.
    /// </summary>
    Text,
    /// <summary>
    /// A checked or unchecked value.
    /// </summary>
    Boolean,
    /// <summary>
    /// A finite numeric value.
    /// </summary>
    Number,
    /// <summary>
    /// The authored value of a selected option.
    /// </summary>
    Selection
}
