using System;
using System.Collections.Generic;
using System.Collections.Frozen;
using System.Linq;
using Inno.Core.Mathematics;

namespace Inno.Core.Input;

/// <summary>
/// Owns immutable ordered input facts and derives convenient frame summaries from that same sequence.
/// </summary>
/// <remarks>
/// This value does not dispatch events, own focus or retain platform handles. It can be shared across
/// frame consumers; coordinate mapping creates a new value and never changes the source sequence.
/// </remarks>
public sealed class InputSequence
{
    /// <summary>
    /// Creates a frozen sequence in acquisition order.
    /// </summary>
    /// <param name="changes">
    /// Non-null immutable input facts in their original order.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The sequence or a contained change is null.
    /// </exception>
    public InputSequence(IEnumerable<InputChange> changes) : this(Capture(changes))
    {
    }

    private InputSequence(InputChange[] captured)
    {
        HashSet<KeyCode>? pressedKeys = null;
        HashSet<KeyCode>? releasedKeys = null;
        HashSet<MouseButton>? pressedButtons = null;
        HashSet<MouseButton>? releasedButtons = null;
        List<string>? text = null;
        Vector2 movement = default;
        Vector2 scroll = default;
        foreach (InputChange change in captured)
        {
            ArgumentNullException.ThrowIfNull(change);
            switch (change)
            {
                case KeyInputChange { isPressed: true, isRepeat: false } key:
                    (pressedKeys ??= []).Add(key.key);
                    break;
                case KeyInputChange { isPressed: false } key:
                    (releasedKeys ??= []).Add(key.key);
                    break;
                case PointerButtonInputChange button:
                    (button.isPressed ? pressedButtons ??= [] : releasedButtons ??= []).Add(button.button);
                    break;
                case TextInputChange commit:
                    (text ??= []).Add(commit.text);
                    break;
                case TextCompositionChange composition:
                    hasTextCompositionActivity = true;
                    if (composition.phase == TextCompositionPhase.Committed && composition.state.text.Length != 0)
                        (text ??= []).Add(composition.state.text);
                    break;
                case PointerMoveInputChange move:
                    movement += move.position - move.previousPosition;
                    break;
                case PointerScrollInputChange wheel:
                    scroll += wheel.delta;
                    break;
            }
        }
        this.changes = Array.AsReadOnly(captured);
        keysPressed = pressedKeys?.ToFrozenSet() ?? FrozenSet<KeyCode>.Empty;
        keysReleased = releasedKeys?.ToFrozenSet() ?? FrozenSet<KeyCode>.Empty;
        buttonsPressed = pressedButtons?.ToFrozenSet() ?? FrozenSet<MouseButton>.Empty;
        buttonsReleased = releasedButtons?.ToFrozenSet() ?? FrozenSet<MouseButton>.Empty;
        textInput = text is null ? Array.Empty<string>() : text.AsReadOnly();
        mouseDelta = movement;
        scrollDelta = scroll;
    }

    /// <summary>
    /// Gets the reusable sequence containing no input changes.
    /// </summary>
    public static InputSequence empty { get; } = new([]);
    /// <summary>
    /// Gets all changes in their original order, including repeated keys.
    /// </summary>
    public IReadOnlyList<InputChange> changes { get; }
    /// <summary>
    /// Gets non-repeated physical presses present in this sequence.
    /// </summary>
    public IReadOnlySet<KeyCode> keysPressed { get; }
    /// <summary>
    /// Gets physical key releases present in this sequence.
    /// </summary>
    public IReadOnlySet<KeyCode> keysReleased { get; }
    /// <summary>
    /// Gets physical pointer presses present in this sequence.
    /// </summary>
    public IReadOnlySet<MouseButton> buttonsPressed { get; }
    /// <summary>
    /// Gets physical pointer releases present in this sequence.
    /// </summary>
    public IReadOnlySet<MouseButton> buttonsReleased { get; }
    /// <summary>
    /// Gets committed text in acquisition order.
    /// </summary>
    public IReadOnlyList<string> textInput { get; }
    /// <summary>
    /// Gets whether this sample contains composition activity, including a final commit or cancellation.
    /// </summary>
    /// <remarks>
    /// Gameplay activation must not reuse candidate-confirmation input merely because composition ended in this sample.
    /// </remarks>
    public bool hasTextCompositionActivity { get; }
    /// <summary>
    /// Gets the sum of pointer movements in the sequence's coordinate space.
    /// </summary>
    public Vector2 mouseDelta { get; }
    /// <summary>
    /// Gets accumulated wheel movement, independent of coordinate density.
    /// </summary>
    public Vector2 scrollDelta { get; }

    /// <summary>
    /// Maps pointer positions without reordering keys, text or pointer transitions.
    /// </summary>
    /// <param name="map">
    /// A synchronous mapping in the caller's owned viewport; it is not retained.
    /// </param>
    /// <returns>
    /// A new coordinate-mapped sequence, or this sequence when it contains no pointer facts.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The mapping is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The mapping returns a non-finite position.
    /// </exception>
    public InputSequence MapPointer(Func<Vector2, Vector2> map)
    {
        ArgumentNullException.ThrowIfNull(map);
        InputChange[]? mapped = null;
        for (int index = 0; index < changes.Count; index++)
        {
            InputChange change = changes[index];
            if (change is not PointerInputChange)
                continue;
            mapped ??= changes.ToArray();
            mapped[index] = change switch
            {
                PointerMoveInputChange move => new PointerMoveInputChange(map(move.position), map(move.previousPosition), move.modifiers),
                PointerButtonInputChange button => new PointerButtonInputChange(map(button.position), button.button, button.isPressed, button.modifiers),
                PointerScrollInputChange scroll => new PointerScrollInputChange(map(scroll.position), scroll.delta, scroll.modifiers),
                _ => change
            };
        }
        return mapped is null ? this : new InputSequence(mapped);
    }

    private static InputChange[] Capture(IEnumerable<InputChange> changes)
    {
        ArgumentNullException.ThrowIfNull(changes);
        return changes.ToArray();
    }
}
