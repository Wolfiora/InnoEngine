namespace Inno.Text;

/// <summary>
/// Selects the logical direction used by the shaping engine.
/// </summary>
public enum TextDirection
{
    /// <summary>
    /// Lets the shaping engine infer direction from the text.
    /// </summary>
    Automatic,
    /// <summary>
    /// Shapes text from left to right.
    /// </summary>
    LeftToRight,
    /// <summary>
    /// Shapes text from right to left.
    /// </summary>
    RightToLeft,
    /// <summary>
    /// Shapes text from top to bottom.
    /// </summary>
    TopToBottom,
    /// <summary>
    /// Shapes text from bottom to top.
    /// </summary>
    BottomToTop
}
