
namespace Inno.UI;

/// <summary>
/// Describes an explicit focus request without treating an ineligible element as a backend failure.
/// </summary>
public enum UiFocusResult
{
    /// <summary>
    /// The requested element acquired focus.
    /// </summary>
    Focused,
    /// <summary>
    /// The requested focus state was already active.
    /// </summary>
    Unchanged,
    /// <summary>
    /// The live document contains no element with the requested ID.
    /// </summary>
    NotFound,
    /// <summary>
    /// Visibility, enabled state, focus policy or a modal prevents the request.
    /// </summary>
    NotFocusable
}

