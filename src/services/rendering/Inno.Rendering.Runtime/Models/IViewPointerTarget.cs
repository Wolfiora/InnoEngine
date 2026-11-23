using Inno.Core.Mathematics;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Receives pointer input after the rendering model resolves visible draw order.
/// </summary>
[StableTypeID("8d770d54-025c-591a-b49e-9c12c4bde162")]
public interface IViewPointerTarget
{
    /// <summary>
    /// Gets whether this target retains pointer ownership from an earlier press.
    /// Rendering models route captured input before ordinary hit testing.
    /// </summary>
    bool hasPointerCapture { get; }

    /// <summary>
    /// Gets whether keyboard and text input should continue reaching this target.
    /// </summary>
    bool hasKeyboardFocus { get; }

    /// <summary>
    /// Changes keyboard focus after the rendering model resolves a pointer press.
    /// </summary>
    /// <param name="focused">
    /// Whether this target owns subsequent keyboard input.
    /// </param>
    void SetKeyboardFocus(bool focused);

    /// <summary>
    /// Maps one output pointer onto this item's local surface.
    /// </summary>
    /// <param name="view">
    /// Exact rendered view.
    /// </param>
    /// <param name="input">
    /// Viewport-local input.
    /// </param>
    /// <param name="localPosition">
    /// Local target coordinates when hit.
    /// </param>
    /// <returns>
    /// Whether this target is eligible to receive pointer input.
    /// </returns>
    bool TryHit(
        RenderView view,
        RenderOutputInput input,
        out Vector2 localPosition
    );

    /// <summary>
    /// Advances this target once for the frame with either routed input or an empty snapshot.
    /// </summary>
    /// <param name="input">
    /// Routed viewport-local input, or an empty snapshot.
    /// </param>
    /// <param name="localPosition">
    /// Target-local pointer coordinates.
    /// </param>
    /// <param name="frameIndex">
    /// Shared output frame index.
    /// </param>
    void Advance(
        RenderOutputInput input,
        Vector2 localPosition,
        ulong frameIndex
    );
}
