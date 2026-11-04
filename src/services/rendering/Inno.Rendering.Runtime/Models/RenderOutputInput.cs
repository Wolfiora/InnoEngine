using System.Collections.Frozen;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Rendering;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Frame-local input located in the output viewport's physical pixels.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("0fe2e29e-4826-5e41-9ebd-a3110e673b5e")]
public sealed class RenderOutputInput
{
    /// <summary>
    /// Creates a viewport-local view of an immutable input sequence.
    /// </summary>
    /// <param name="pointerPosition">
    /// Final pointer position in viewport pixels.
    /// </param>
    /// <param name="pointerInside">
    /// Whether the final pointer is inside this output.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the frame boundary.
    /// </param>
    /// <param name="sequence">
    /// Ordered changes mapped to the output coordinate space.
    /// </param>
    /// <param name="interactionEnabled">
    /// Whether gameplay interaction is allowed.
    /// </param>
    /// <param name="gamepads">
    /// Immutable controller states admitted by the owning input route; null means no admitted devices.
    /// </param>
    /// <param name="textComposition">
    /// Active composition owned by this viewport, or null when none remains active.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The sequence is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A controller state is null or does not match its dictionary key.
    /// </exception>
    public RenderOutputInput(
        Vector2 pointerPosition,
        bool pointerInside,
        KeyModifier modifiers,
        InputSequence sequence,
        bool interactionEnabled = true,
        IReadOnlyDictionary<ulong, GamepadState>? gamepads = null,
        TextCompositionState? textComposition = null
    ) {
        ArgumentNullException.ThrowIfNull(sequence);
        this.pointerPosition = pointerPosition;
        this.pointerInside = pointerInside;
        this.modifiers = modifiers;
        this.sequence = sequence;
        this.textComposition = textComposition;
        var capturedGamepads = gamepads?.ToFrozenDictionary() ?? FrozenDictionary<ulong, GamepadState>.Empty;
        this.gamepads = capturedGamepads;
        foreach ((ulong id, GamepadState state) in capturedGamepads)
            if (state is null || id != state.deviceId)
                throw new ArgumentException("Controller state must match its connection identity.", nameof(gamepads));
        this.interactionEnabled = interactionEnabled;
    }

    /// <summary>
    /// Gets the immutable input changes in acquisition order.
    /// </summary>
    public InputSequence sequence { get; }

    /// <summary>
    /// Gets the active viewport-owned composition without retaining an operating-system context.
    /// </summary>
    public TextCompositionState? textComposition { get; }

    /// <summary>
    /// Gets whether composition owns this sample; suppress gameplay activation even on the commit frame.
    /// </summary>
    public bool hasTextCompositionInput => textComposition is not null || sequence.hasTextCompositionActivity;

    /// <summary>
    /// Gets controller held state for this input owner without retaining backend or native objects.
    /// </summary>
    public IReadOnlyDictionary<ulong, GamepadState> gamepads { get; }

    /// <summary>
    /// Gets a snapshot with no active input.
    /// </summary>
    public static RenderOutputInput empty { get; } = new(default, false, KeyModifier.None, InputSequence.empty);

    /// <summary>
    /// Gets a snapshot that clears gameplay interaction while retaining the rendered output.
    /// </summary>
    public static RenderOutputInput suspended { get; } = new(default, false, KeyModifier.None, InputSequence.empty, interactionEnabled: false);

    /// <summary>
    /// Gets viewport-local pointer coordinates.
    /// </summary>
    public Vector2 pointerPosition { get; }
    /// <summary>
    /// Gets whether the pointer is inside this output.
    /// </summary>
    public bool pointerInside { get; }
    /// <summary>
    /// Gets wheel movement.
    /// </summary>
    public Vector2 scrollDelta => sequence.scrollDelta;
    /// <summary>
    /// Gets active keyboard modifiers.
    /// </summary>
    public KeyModifier modifiers { get; }
    /// <summary>
    /// Gets keys pressed this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysPressed => sequence.keysPressed;
    /// <summary>
    /// Gets keys released this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysReleased => sequence.keysReleased;
    /// <summary>
    /// Gets buttons pressed this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> buttonsPressed => sequence.buttonsPressed;
    /// <summary>
    /// Gets buttons released this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> buttonsReleased => sequence.buttonsReleased;
    /// <summary>
    /// Gets ordered text commits.
    /// </summary>
    public IReadOnlyList<string> textInput => sequence.textInput;

    /// <summary>
    /// Gets whether game pointer and keyboard interaction is enabled.
    /// </summary>
    public bool interactionEnabled { get; }
}

