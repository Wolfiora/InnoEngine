using System;
using System.Linq;

using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Xunit;

namespace Inno.Input.Tests;

public sealed class TextCompositionTests
{
    [Fact]
    public void CompositionSelectionUsesUnicodeScalarsAndRejectsMalformedText()
    {
        var state = new TextCompositionState(1, "文😀字", 1, 1);
        Assert.Equal(1, state.selectionStart);
        Assert.Equal(1, state.selectionLength);
        Assert.Throws<ArgumentOutOfRangeException>(() => new TextCompositionState(0, ""));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TextCompositionState(1, "😀", 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => new TextCompositionState(1, "a", -1, 1));
        Assert.Throws<ArgumentException>(() => new TextCompositionState(1, "\ud800"));
        Assert.Throws<ArgumentException>(() => new TextCompositionState(1, "\udc00"));
    }

    [Fact]
    public void CommitRetainsCompositionOwnershipForTheWholeSampleWithoutDuplicatingText()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Started, "ni"));
        InputSnapshot first = backend.Capture(1);
        Assert.Equal("ni", first.textComposition?.text);
        Assert.True(first.hasTextCompositionInput);
        Assert.Empty(first.textInput);
        backend.ProcessEvent(new KeyPressedEvent(7, KeyCode.Enter));
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Updated, "你好"));
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Committed, "你好"));
        backend.ProcessEvent(new KeyReleasedEvent(7, KeyCode.Enter));
        InputSnapshot committed = backend.Capture(2);
        Assert.Null(committed.textComposition);
        Assert.True(committed.hasTextCompositionInput);
        Assert.Equal("你好", Assert.Single(committed.textInput));
        Assert.Collection(committed.sequence.changes,
            item => Assert.IsType<KeyInputChange>(item),
            item => Assert.Equal(TextCompositionPhase.Updated, Assert.IsType<TextCompositionChange>(item).phase),
            item => Assert.Equal(TextCompositionPhase.Committed, Assert.IsType<TextCompositionChange>(item).phase),
            item => Assert.IsType<KeyInputChange>(item));
        Assert.Same(committed.sequence, committed.sequence.MapPointer(static position => position + Vector2.ONE));
        Assert.False(backend.Capture(3).hasTextCompositionInput);
        Assert.Equal("ni", first.textComposition?.text);
    }

    [Fact]
    public void LateCompositionCallbacksCannotOverwriteANewerOperation()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Started, "old"));
        backend.Capture(1);
        backend.ProcessEvent(Composition(7, 2, TextCompositionPhase.Started, "new"));
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Updated, "late"));
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Committed, "late"));
        InputSnapshot snapshot = backend.Capture(2);
        Assert.Equal("new", snapshot.textComposition?.text);
        Assert.Empty(snapshot.textInput);
        Assert.Collection(snapshot.sequence.changes,
            item => Assert.Equal(TextCompositionPhase.Cancelled, Assert.IsType<TextCompositionChange>(item).phase),
            item => Assert.Equal(TextCompositionPhase.Started, Assert.IsType<TextCompositionChange>(item).phase));
        backend.ProcessEvent(Composition(7, 2, TextCompositionPhase.Cancelled, "new"));
        backend.Capture(3);
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Started, "old start"));
        Assert.Same(InputSequence.empty, backend.Capture(4).sequence);
    }

    [Fact]
    public void FocusLossCancelsCompositionAndRejectsLateInputUntilReacquired()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Started, "draft"));
        backend.Capture(1);
        backend.ProcessEvent(new WindowFocusChangedEvent(7, false));
        backend.ProcessEvent(Composition(7, 1, TextCompositionPhase.Committed, "late"));
        backend.ProcessEvent(new TextInputEvent(7, "late plain text"));
        backend.ProcessEvent(new KeyPressedEvent(7, KeyCode.Enter));
        InputSnapshot cancelled = backend.Capture(2);
        Assert.Null(cancelled.textComposition);
        Assert.Empty(cancelled.textInput);
        Assert.Empty(cancelled.keysPressed);
        Assert.Equal(TextCompositionPhase.Cancelled, Assert.IsType<TextCompositionChange>(Assert.Single(cancelled.sequence.changes)).phase);
        backend.ProcessEvent(new WindowFocusChangedEvent(7, true));
        backend.ProcessEvent(Composition(7, 2, TextCompositionPhase.Started, "fresh"));
        Assert.Equal("fresh", backend.Capture(3).textComposition?.text);
    }

    [Fact]
    public void SessionsDoNotInheritAnOldCompositionAndSourceRetirementCancelsActiveText()
    {
        var source = new EventInputSource(7);
        using var first = source.CreateBackend();
        source.ProcessEvent(Composition(7, 1, TextCompositionPhase.Started, "first"));
        Assert.NotNull(first.Capture(1).textComposition);
        using var second = source.CreateBackend();
        source.ProcessEvent(Composition(7, 1, TextCompositionPhase.Updated, "ongoing"));
        Assert.Equal("ongoing", first.Capture(2).textComposition?.text);
        Assert.Null(second.Capture(2).textComposition);
        source.Dispose();
        Assert.Equal(TextCompositionPhase.Cancelled,
            Assert.Single(first.Capture(3).sequence.changes.OfType<TextCompositionChange>()).phase);
        Assert.False(first.Capture(4).hasTextCompositionInput);
    }

    [Fact]
    public void HiddenAndInitiallyUnfocusedRoutesRejectTextWithoutCrossWindowLeakage()
    {
        using var source = new EventInputSource(7);
        source.ProcessEvent(new WindowFocusChangedEvent(7, false));
        using var backend = source.CreateBackend();
        source.ProcessEvent(Composition(7, 1, TextCompositionPhase.Started, "unfocused"));
        Assert.Null(backend.Capture(1).textComposition);
        source.ProcessEvent(new WindowFocusChangedEvent(7, true));
        source.ProcessEvent(Composition(8, 1, TextCompositionPhase.Started, "foreign"));
        Assert.Null(backend.Capture(2).textComposition);
        source.ProcessEvent(Composition(7, 2, TextCompositionPhase.Started, "owned"));
        Assert.Equal("owned", backend.Capture(3).textComposition?.text);
        source.ProcessEvent(new WindowVisibilityChangedEvent(7, false));
        source.ProcessEvent(Composition(7, 2, TextCompositionPhase.Committed, "hidden"));
        InputSnapshot hidden = backend.Capture(4);
        Assert.Null(hidden.textComposition);
        Assert.Empty(hidden.textInput);
    }

    private static TextCompositionEvent Composition(
        uint windowId,
        ulong compositionId,
        TextCompositionPhase phase,
        string text
    ) => new(windowId, new(phase, new(compositionId, text)));
}
