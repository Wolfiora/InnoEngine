namespace Inno.UI;

/// <summary>
/// Distinguishes a missing declaration from a hidden element and measurable visual content.
/// </summary>
public enum UiElementLayoutStatus
{
    /// <summary>
    /// The document does not contain the requested authored element ID.
    /// </summary>
    Missing,
    /// <summary>
    /// The element exists but does not currently produce visible layout geometry.
    /// </summary>
    Hidden,
    /// <summary>
    /// The snapshot contains the element's current visual geometry.
    /// </summary>
    Available
}
