namespace Inno.Core.Input;

/// <summary>
/// Identifies the supported mouse cursor values for this contract.
/// </summary>
public enum MouseCursor
{
    /// <summary>
    /// No concrete cursor shape; the consuming host defines visibility behavior.
    /// </summary>
    None = -1,
        
    /// <summary>
    /// The standard arrow pointer.
    /// </summary>
    Arrow = 0,
    /// <summary>
    /// The text insertion cursor.
    /// </summary>
    TextInput = 1,
    /// <summary>
    /// An all-direction move or resize cursor.
    /// </summary>
    ResizeAll = 2,
    /// <summary>
    /// A north-south resize cursor.
    /// </summary>
    ResizeNS = 3,
    /// <summary>
    /// An east-west resize cursor.
    /// </summary>
    ResizeEW = 4,
    /// <summary>
    /// A northeast-southwest diagonal resize cursor.
    /// </summary>
    ResizeNESW = 5,
    /// <summary>
    /// A northwest-southeast diagonal resize cursor.
    /// </summary>
    ResizeNWSE = 6,
    /// <summary>
    /// A hand pointer for an interactive target.
    /// </summary>
    Hand = 7,
    /// <summary>
    /// A cursor indicating that an operation is unavailable.
    /// </summary>
    NotAllowed = 8,
}
