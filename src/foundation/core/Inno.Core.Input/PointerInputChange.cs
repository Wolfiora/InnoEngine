using System;
using Inno.Core.Mathematics;

namespace Inno.Core.Input;

/// <summary>
/// Captures a pointer fact in the coordinate space declared by its containing snapshot.
/// </summary>
public abstract record PointerInputChange : InputChange
{
    /// <summary>
    /// Initializes an assembly-owned pointer fact after validating its position.
    /// </summary>
    /// <param name="position">
    /// Finite coordinates in the containing snapshot's declared space.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers when the fact was acquired.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A position component is not finite.
    /// </exception>
    private protected PointerInputChange(
        Vector2 position,
        KeyModifier modifiers
    ) : base(modifiers) {
        if (!float.IsFinite(position.x) || !float.IsFinite(position.y))
            throw new ArgumentOutOfRangeException(nameof(position));
        this.position = position;
    }

    /// <summary>
    /// Gets the pointer position at this change, not the final frame position.
    /// </summary>
    public Vector2 position { get; }
}
