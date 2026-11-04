using System.Collections.Frozen;
using System;
using System.Linq;
using System.Collections.Generic;

using Inno.Core.Input;
using Inno.Core.Mathematics;

namespace Inno.UI;

/// <summary>
/// Defines one independent UI layout and interaction surface.
/// </summary>
public readonly record struct UiContextOptions
{
    /// <summary>
    /// Creates validated context options.
    /// </summary>
    /// <param name="name">
    /// A non-empty diagnostic context name.
    /// </param>
    /// <param name="width">
    /// The positive initial width in pixels.
    /// </param>
    /// <param name="height">
    /// The positive initial height in pixels.
    /// </param>
    /// <param name="density">
    /// The positive density-independent pixel ratio.
    /// </param>
    public UiContextOptions(
        string name,
        int width,
        int height,
        float density = 1f
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (!float.IsFinite(density) || density <= 0f)
            throw new ArgumentOutOfRangeException(nameof(density));
        this.name = name.Trim();
        this.width = width;
        this.height = height;
        this.density = density;
    }

    /// <summary>
    /// Gets the diagnostic context name.
    /// </summary>
    public string name { get; }
    /// <summary>
    /// Gets the initial width in pixels.
    /// </summary>
    public int width { get; }
    /// <summary>
    /// Gets the initial height in pixels.
    /// </summary>
    public int height { get; }
    /// <summary>
    /// Gets the density-independent pixel ratio.
    /// </summary>
    public float density { get; }
}

/// <summary>
/// Contains immutable text in one explicitly selected UI document language.
/// </summary>
public sealed class UiDocumentSource
{
    /// <summary>
    /// Creates a validated in-memory document source.
    /// </summary>
    /// <param name="language">
    /// Explicit source language understood by a registered backend.
    /// </param>
    /// <param name="text">
    /// Complete immutable document text.
    /// </param>
    /// <param name="sourceUri">
    /// Virtual source address used only for diagnostics and relative dependency identity.
    /// </param>
    /// <param name="resourceBindings">
    /// Optional frontend resource names mapped to registered runtime resource names; captured defensively.
    /// </param>
    public UiDocumentSource(
        UiDocumentLanguageId language,
        string text,
        string sourceUri = "memory://ui-document",
        IReadOnlyDictionary<string, string>? resourceBindings = null
    ) {
        if (!language.isValid)
            throw new ArgumentException("A document source requires an assigned language.", nameof(language));
        ArgumentException.ThrowIfNullOrWhiteSpace(text);
        ArgumentException.ThrowIfNullOrWhiteSpace(sourceUri);
        this.language = language;
        this.text = text;
        this.sourceUri = sourceUri;
        var bindings = new Dictionary<string, string>(StringComparer.Ordinal);
        if (resourceBindings is not null)
            foreach ((string name, string source) in resourceBindings)
            {
                ArgumentException.ThrowIfNullOrWhiteSpace(name);
                ArgumentException.ThrowIfNullOrWhiteSpace(source);
                bindings.Add(name, source);
            }
        this.resourceBindings = new System.Collections.ObjectModel.ReadOnlyDictionary<string, string>(bindings);
    }

    /// <summary>
    /// Gets the explicitly selected source language.
    /// </summary>
    public UiDocumentLanguageId language { get; }
    /// <summary>
    /// Gets the complete immutable source text.
    /// </summary>
    public string text { get; }
    /// <summary>
    /// Gets the virtual source address.
    /// </summary>
    public string sourceUri { get; }

    /// <summary>
    /// Gets immutable runtime-only source bindings interpreted by the selected language backend.
    /// </summary>
    public IReadOnlyDictionary<string, string> resourceBindings { get; }
}

/// <summary>
/// Contains a language-tagged fragment for a backend-specific document mutation.
/// </summary>
public readonly record struct UiDocumentFragment
{
    /// <summary>
    /// Creates a validated source fragment.
    /// </summary>
    /// <param name="language">
    /// Explicit fragment language.
    /// </param>
    /// <param name="text">
    /// Complete fragment text.
    /// </param>
    public UiDocumentFragment(
        UiDocumentLanguageId language,
        string text
    ) {
        if (!language.isValid)
            throw new ArgumentException("A document fragment requires an assigned language.", nameof(language));
        ArgumentNullException.ThrowIfNull(text);
        this.language = language;
        this.text = text;
    }

    /// <summary>
    /// Gets the explicitly selected fragment language.
    /// </summary>
    public UiDocumentLanguageId language { get; }
    /// <summary>
    /// Gets the complete fragment text.
    /// </summary>
    public string text { get; }
}

/// <summary>
/// Contains immutable RGBA8 texture pixels addressable by a document source name.
/// </summary>
public sealed class UiTextureData
{
    private readonly byte[] m_pixels;

    /// <summary>
    /// Creates a validated immutable texture.
    /// </summary>
    /// <param name="width">
    /// The positive texture width.
    /// </param>
    /// <param name="height">
    /// The positive texture height.
    /// </param>
    /// <param name="pixels">
    /// Tightly packed RGBA8 pixels.
    /// </param>
    public UiTextureData(
        int width,
        int height,
        ReadOnlySpan<byte> pixels
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        if (pixels.Length != checked(width * height * 4))
            throw new ArgumentException("UI texture pixels must be tightly packed RGBA8.", nameof(pixels));
        this.width = width;
        this.height = height;
        m_pixels = pixels.ToArray();
    }

    /// <summary>
    /// Gets the texture width.
    /// </summary>
    public int width { get; }
    /// <summary>
    /// Gets the texture height.
    /// </summary>
    public int height { get; }
    /// <summary>
    /// Gets the immutable RGBA8 pixels.
    /// </summary>
    public ReadOnlyMemory<byte> pixels => m_pixels;
}

/// <summary>
/// Identifies one document event made visible to game code.
/// </summary>
public enum UiEventType
{
    /// <summary>
    /// The target was clicked.
    /// </summary>
    Click,
    /// <summary>
    /// The target value changed.
    /// </summary>
    Change,
    /// <summary>
    /// A form was submitted.
    /// </summary>
    Submit,
    /// <summary>
    /// The target gained focus.
    /// </summary>
    Focus,
    /// <summary>
    /// The target lost focus.
    /// </summary>
    Blur,
    /// <summary>
    /// A pointer entered the target.
    /// </summary>
    MouseEnter,
    /// <summary>
    /// A pointer left the target.
    /// </summary>
    MouseLeave
}

/// <summary>
/// Describes one queued document event after a context update.
/// </summary>
public sealed class UiEvent : Inno.Core.Events.Event
{
    /// <summary>
    /// Creates a document event with its source document and target element.
    /// </summary>
    /// <param name="type">
    /// The kind of document interaction.
    /// </param>
    /// <param name="document">
    /// The document that produced the event.
    /// </param>
    /// <param name="targetId">
    /// The target element identifier, which can be empty.
    /// </param>
    /// <param name="value">
    /// The control value captured when this event occurred.
    /// </param>
    /// <param name="phase">
    /// The editing or commit phase of this event.
    /// </param>
    /// <param name="origin">
    /// The operation that caused the event.
    /// </param>
    public UiEvent(
        UiEventType type,
        UiDocumentHandle document,
        string targetId,
        UiControlValue value,
        UiEventPhase phase,
        UiEventOrigin origin
    ) {
        this.value = value;
        this.phase = phase;
        this.origin = origin;
        this.type = type;
        this.document = document;
        this.targetId = targetId ?? throw new ArgumentNullException(nameof(targetId));
    }

    /// <summary>
    /// Gets the kind of document interaction.
    /// </summary>
    public UiEventType type { get; }

    /// <summary>
    /// Gets the immutable value at event time, not the current control value.
    /// </summary>
    public UiControlValue value { get; }

    /// <summary>
    /// Gets the edit phase of this event.
    /// </summary>
    public UiEventPhase phase { get; }

    /// <summary>
    /// Gets the cause of this event.
    /// </summary>
    public UiEventOrigin origin { get; }

    /// <summary>
    /// Gets the source document.
    /// </summary>
    public UiDocumentHandle document { get; }

    /// <summary>
    /// Gets the target element identifier.
    /// </summary>
    public string targetId { get; }
}

/// <summary>
/// Carries one immutable input snapshot across the UI backend boundary.
/// </summary>
public sealed class UiInputSnapshot
{
    /// <summary>
    /// Creates a context-local view of one immutable ordered input sequence.
    /// </summary>
    /// <param name="mousePosition">
    /// Final pointer position in context pixels.
    /// </param>
    /// <param name="modifiers">
    /// Physical modifiers at the frame boundary.
    /// </param>
    /// <param name="sequence">
    /// Input changes already mapped into context pixels, in acquisition order.
    /// </param>
    /// <param name="gamepads">
    /// Immutable controller states admitted by the owning input route; null means no admitted devices.
    /// </param>
    /// <param name="textComposition">
    /// Active composition admitted by the same input route, or null when none remains active.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The sequence is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// A controller state is null or does not match its dictionary key.
    /// </exception>
    public UiInputSnapshot(
        Vector2 mousePosition,
        KeyModifier modifiers,
        InputSequence sequence,
        IReadOnlyDictionary<ulong, GamepadState>? gamepads = null,
        TextCompositionState? textComposition = null
    ) {
        ArgumentNullException.ThrowIfNull(sequence);
        this.mousePosition = mousePosition;
        this.modifiers = modifiers;
        this.sequence = sequence;
        this.textComposition = textComposition;
        var capturedGamepads = gamepads?.ToFrozenDictionary() ?? FrozenDictionary<ulong, GamepadState>.Empty;
        this.gamepads = capturedGamepads;
        foreach ((ulong id, GamepadState state) in capturedGamepads)
            if (state is null || id != state.deviceId)
                throw new ArgumentException("Controller state must match its connection identity.", nameof(gamepads));
    }

    /// <summary>
    /// Gets the ordered immutable changes without retaining a platform event or callback.
    /// </summary>
    public InputSequence sequence { get; }

    /// <summary>
    /// Gets active provisional text belonging to this context's input route.
    /// </summary>
    public TextCompositionState? textComposition { get; }

    /// <summary>
    /// Gets whether composition owns this sample, including a commit or cancellation.
    /// </summary>
    public bool hasTextCompositionInput => textComposition is not null || sequence.hasTextCompositionActivity;

    /// <summary>
    /// Gets controller held state for this input owner without retaining backend or native objects.
    /// </summary>
    public IReadOnlyDictionary<ulong, GamepadState> gamepads { get; }

    /// <summary>
    /// Gets the pointer position.
    /// </summary>
    public Vector2 mousePosition { get; }
    /// <summary>
    /// Gets pointer wheel movement.
    /// </summary>
    public Vector2 scrollDelta => sequence.scrollDelta;
    /// <summary>
    /// Gets active keyboard modifiers.
    /// </summary>
    public KeyModifier modifiers { get; }
    /// <summary>
    /// Gets physical keys pressed this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysPressed => sequence.keysPressed;
    /// <summary>
    /// Gets physical keys released this frame.
    /// </summary>
    public IReadOnlyCollection<KeyCode> keysReleased => sequence.keysReleased;
    /// <summary>
    /// Gets pointer buttons pressed this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> buttonsPressed => sequence.buttonsPressed;
    /// <summary>
    /// Gets pointer buttons released this frame.
    /// </summary>
    public IReadOnlyCollection<MouseButton> buttonsReleased => sequence.buttonsReleased;
    /// <summary>
    /// Gets ordered Unicode text commits.
    /// </summary>
    public IReadOnlyList<string> textInput => sequence.textInput;
}
