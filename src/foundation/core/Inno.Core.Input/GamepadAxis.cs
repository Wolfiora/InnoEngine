namespace Inno.Core.Input;

/// <summary>
/// Names normalized controller axes; stick down/right is positive and triggers range from zero to one.
/// </summary>
public enum GamepadAxis
{
    /// <summary>
    /// Left stick horizontal displacement.
    /// </summary>
    LeftX,
    /// <summary>
    /// Left stick vertical displacement.
    /// </summary>
    LeftY,
    /// <summary>
    /// Right stick horizontal displacement.
    /// </summary>
    RightX,
    /// <summary>
    /// Right stick vertical displacement.
    /// </summary>
    RightY,
    /// <summary>
    /// Left trigger travel.
    /// </summary>
    LeftTrigger,
    /// <summary>
    /// Right trigger travel.
    /// </summary>
    RightTrigger
}
