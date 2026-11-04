using System;

namespace Inno.Core.Input;

/// <summary>
/// Places an input-method composition transition among the ordered physical input facts.
/// </summary>
public sealed record TextCompositionChange : InputChange
{
    /// <summary>
    /// Creates an immutable composition transition.
    /// </summary>
    /// <param name="phase">
    /// Whether composition starts, changes, commits or cancels.
    /// </param>
    /// <param name="state">
    /// Operation identity and complete text at this transition; cancellation text is never committed.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers observed at the transition.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The state is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The phase is not defined.
    /// </exception>
    public TextCompositionChange(
        TextCompositionPhase phase,
        TextCompositionState state,
        KeyModifier modifiers = KeyModifier.None
    ) : base(modifiers) {
        ArgumentNullException.ThrowIfNull(state);
        if (!Enum.IsDefined(phase))
            throw new ArgumentOutOfRangeException(nameof(phase));
        this.phase = phase;
        this.state = state;
    }

    /// <summary>
    /// Gets the transition, including the distinction between commit and cancellation.
    /// </summary>
    public TextCompositionPhase phase { get; }
    /// <summary>
    /// Gets the immutable identity, text and scalar selection.
    /// </summary>
    public TextCompositionState state { get; }
}
