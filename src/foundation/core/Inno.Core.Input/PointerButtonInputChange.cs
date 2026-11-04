using Inno.Core.Mathematics;

namespace Inno.Core.Input;

/// <summary>
/// Captures a pointer-button transition together with its hit-test position.
/// </summary>
public sealed record PointerButtonInputChange : PointerInputChange
{
    /// <summary>
    /// Creates a positioned pointer-button transition.
    /// </summary>
    /// <param name="position">
    /// Pointer coordinates at the transition.
    /// </param>
    /// <param name="button">
    /// The physical pointer button.
    /// </param>
    /// <param name="isPressed">
    /// True for a press; false for a release.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the transition.
    /// </param>
    /// <exception cref="System.ArgumentOutOfRangeException">
    /// A position component is not finite.
    /// </exception>
    public PointerButtonInputChange(
        Vector2 position,
        MouseButton button,
        bool isPressed,
        KeyModifier modifiers
    ) : base(position, modifiers) {
        this.button = button;
        this.isPressed = isPressed;
    }

    /// <summary>
    /// Gets the physical pointer button.
    /// </summary>
    public MouseButton button { get; }
    /// <summary>
    /// Gets whether the button is pressed by this transition.
    /// </summary>
    public bool isPressed { get; }
}
