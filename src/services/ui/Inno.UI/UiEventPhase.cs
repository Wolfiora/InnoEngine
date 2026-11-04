namespace Inno.UI;

/// <summary>
/// Distinguishes ongoing value editing from a completed control interaction.
/// </summary>
public enum UiEventPhase
{
    /// <summary>
    /// The event does not describe a value edit.
    /// </summary>
    None,
    /// <summary>
    /// The user is editing a value and may continue changing it.
    /// </summary>
    Editing,
    /// <summary>
    /// The control interaction submitted or finished its current value.
    /// </summary>
    Committed
}
