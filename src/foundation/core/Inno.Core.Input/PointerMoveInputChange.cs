using System;
using Inno.Core.Mathematics;

namespace Inno.Core.Input;

/// <summary>
/// Captures pointer movement and its prior position in one declared coordinate space.
/// </summary>
public sealed record PointerMoveInputChange : PointerInputChange
{
    /// <summary>
    /// Creates a pointer movement without accumulating away its ordering.
    /// </summary>
    /// <param name="position">
    /// The new pointer position.
    /// </param>
    /// <param name="previousPosition">
    /// The preceding pointer position.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the movement.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A position is not finite.
    /// </exception>
    public PointerMoveInputChange(
        Vector2 position,
        Vector2 previousPosition,
        KeyModifier modifiers
    ) : base(position, modifiers) {
        if (!float.IsFinite(previousPosition.x) || !float.IsFinite(previousPosition.y))
            throw new ArgumentOutOfRangeException(nameof(previousPosition));
        this.previousPosition = previousPosition;
    }

    /// <summary>
    /// Gets the pointer position immediately preceding this movement.
    /// </summary>
    public Vector2 previousPosition { get; }
}
