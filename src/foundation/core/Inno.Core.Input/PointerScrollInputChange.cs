using System;
using Inno.Core.Mathematics;

namespace Inno.Core.Input;

/// <summary>
/// Captures wheel movement at the pointer position that receives it.
/// </summary>
public sealed record PointerScrollInputChange : PointerInputChange
{
    /// <summary>
    /// Creates a positioned wheel change.
    /// </summary>
    /// <param name="position">
    /// The pointer position at the wheel event.
    /// </param>
    /// <param name="delta">
    /// Wheel movement, positive right horizontally and up vertically.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the wheel event.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A position or wheel component is not finite.
    /// </exception>
    public PointerScrollInputChange(
        Vector2 position,
        Vector2 delta,
        KeyModifier modifiers
    ) : base(position, modifiers) {
        if (!float.IsFinite(delta.x) || !float.IsFinite(delta.y))
            throw new ArgumentOutOfRangeException(nameof(delta));
        this.delta = delta;
    }

    /// <summary>
    /// Gets wheel movement independently of pointer-coordinate scaling.
    /// </summary>
    public Vector2 delta { get; }
}
