namespace Inno.UI;

/// <summary>
/// Identifies the operation that caused a document event.
/// </summary>
public enum UiEventOrigin
{
    /// <summary>
    /// The backend updated document state independently of input.
    /// </summary>
    Backend,
    /// <summary>
    /// An explicitly routed user input sample caused the event.
    /// </summary>
    UserInput,
    /// <summary>
    /// An application API call caused the event.
    /// </summary>
    Programmatic
}
