namespace Inno.Core.Input;

/// <summary>
/// Names standard controller positions independently of printed labels or platform button codes.
/// </summary>
public enum GamepadButton
{
    /// <summary>
    /// Bottom face button.
    /// </summary>
    South,
    /// <summary>
    /// Right face button.
    /// </summary>
    East,
    /// <summary>
    /// Left face button.
    /// </summary>
    West,
    /// <summary>
    /// Top face button.
    /// </summary>
    North,
    /// <summary>
    /// Secondary menu button.
    /// </summary>
    Back,
    /// <summary>
    /// System guide button when delivered by the host.
    /// </summary>
    Guide,
    /// <summary>
    /// Primary menu button.
    /// </summary>
    Start,
    /// <summary>
    /// Left stick press.
    /// </summary>
    LeftStick,
    /// <summary>
    /// Right stick press.
    /// </summary>
    RightStick,
    /// <summary>
    /// Left shoulder button.
    /// </summary>
    LeftShoulder,
    /// <summary>
    /// Right shoulder button.
    /// </summary>
    RightShoulder,
    /// <summary>
    /// Upward directional button.
    /// </summary>
    DpadUp,
    /// <summary>
    /// Downward directional button.
    /// </summary>
    DpadDown,
    /// <summary>
    /// Leftward directional button.
    /// </summary>
    DpadLeft,
    /// <summary>
    /// Rightward directional button.
    /// </summary>
    DpadRight
}
