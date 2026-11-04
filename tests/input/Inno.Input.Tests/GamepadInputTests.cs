using System;
using System.Collections.Generic;
using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Input;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Input.Tests;

public sealed class GamepadInputTests
{
    private readonly ITestOutputHelper m_output;

    public GamepadInputTests(ITestOutputHelper output) => m_output = output;

    [Fact]
    public void UnchangedControllerSnapshotsReuseStateWithoutPerDeviceAllocation()
    {
        using var empty = new EventInputBackend(7);
        using var connected = new EventInputBackend(7);
        for (ulong id = 1; id <= 16; id++)
        {
            connected.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(id, true)));
            connected.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(id, GamepadButton.South, true)));
        }
        connected.Capture(0);
        Measure(empty);
        Measure(connected);
        for (int sample = 0; sample < 5; sample++)
        {
            long baseline = Measure(empty);
            long observed = Measure(connected);
            m_output.WriteLine($"sample={sample}; frames=1000; emptyBytes={baseline}; sixteenDeviceBytes={observed}");
            Assert.Equal(baseline, observed);
        }

        static long Measure(EventInputBackend backend)
        {
            long before = GC.GetAllocatedBytesForCurrentThread();
            InputSnapshot? last = null;
            for (int frame = 0; frame < 1000; frame++)
                last = backend.Capture(frame);
            long bytes = GC.GetAllocatedBytesForCurrentThread() - before;
            GC.KeepAlive(last);
            return bytes;
        }
    }

    [Fact]
    public void SuspensionPreservesConnectionFactsButClearsAndBlocksControls()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, true)));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.South, true)));
        backend.ProcessEvent(new ApplicationSuspensionChangedEvent(true));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.East, true)));
        InputSnapshot suspended = backend.Capture(1);
        Assert.Empty(suspended.gamepads[11].buttonsDown);
        Assert.Collection(suspended.sequence.changes,
            change => Assert.True(Assert.IsType<GamepadConnectionChange>(change).connected),
            change => Assert.False(Assert.IsType<GamepadButtonChange>(change).pressed));
        backend.ProcessEvent(new ApplicationSuspensionChangedEvent(false));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.East, true)));
        Assert.Contains(GamepadButton.East, backend.Capture(2).gamepads[11].buttonsDown);
    }

    [Fact]
    public void MultiwindowSourceCanAcquireAnotherWindowAfterTheFirstCloses()
    {
        using var source = new EventInputSource(0);
        source.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, true)));
        using var backend = source.CreateBackend();
        source.ProcessEvent(new WindowVisibilityChangedEvent(7, false));
        source.ProcessEvent(new WindowCloseEvent(7));
        source.ProcessEvent(new WindowFocusChangedEvent(8, true));
        source.ProcessEvent(new GamepadEvent(8, new GamepadButtonChange(11, GamepadButton.South, true)));
        Assert.Contains(GamepadButton.South, backend.Capture(1).gamepads[11].buttonsDown);
        using var next = source.CreateBackend();
        Assert.Contains(GamepadButton.South, next.Capture(1).gamepads[11].buttonsDown);
    }

    [Fact]
    public void OrderedFactsAndFrozenHeldStateSurviveLaterFrames()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, true)));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.South, true)));
        backend.ProcessEvent(new TextInputEvent(7, "text"));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadAxisChange(11, GamepadAxis.LeftX, -0.75f)));
        InputSnapshot first = backend.Capture(1);
        Assert.Collection(first.sequence.changes,
            change => Assert.True(Assert.IsType<GamepadConnectionChange>(change).connected),
            change => Assert.True(Assert.IsType<GamepadButtonChange>(change).pressed),
            change => Assert.Equal("text", Assert.IsType<TextInputChange>(change).text),
            change => Assert.Equal(-0.75f, Assert.IsType<GamepadAxisChange>(change).value));
        InputSnapshot idle = backend.Capture(2);
        Assert.Same(first.gamepads, idle.gamepads);
        Assert.Empty(idle.sequence.changes);
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.South, false)));
        Assert.Empty(backend.Capture(3).gamepads[11].buttonsDown);
        Assert.Contains(GamepadButton.South, first.gamepads[11].buttonsDown);
    }

    [Fact]
    public void FocusLossReleasesControlsAndRejectsLateEventsUntilFocusReturns()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, true)));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.South, true)));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadAxisChange(11, GamepadAxis.LeftTrigger, 1f)));
        backend.Capture(1);
        backend.ProcessEvent(new WindowFocusChangedEvent(7, false));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.East, true)));
        InputSnapshot lost = backend.Capture(2);
        Assert.Collection(lost.sequence.changes,
            change => Assert.False(Assert.IsType<GamepadButtonChange>(change).pressed),
            change => Assert.Equal(0f, Assert.IsType<GamepadAxisChange>(change).value));
        Assert.Empty(lost.gamepads[11].buttonsDown);
        Assert.Equal(0f, lost.gamepads[11].GetAxis(GamepadAxis.LeftTrigger));
        backend.ProcessEvent(new WindowFocusChangedEvent(7, true));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.East, true)));
        Assert.Contains(GamepadButton.East, backend.Capture(3).gamepads[11].buttonsDown);
    }

    [Fact]
    public void SourceSeedsNewSessionsWithoutManufacturingEdgesOrSharingMutableState()
    {
        using var source = new EventInputSource(7);
        source.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, true)));
        source.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.South, true)));
        using var first = source.CreateBackend();
        using var second = source.CreateBackend();
        InputSnapshot captured = first.Capture(1);
        Assert.Empty(captured.sequence.changes);
        Assert.Contains(GamepadButton.South, captured.gamepads[11].buttonsDown);
        first.Dispose();
        source.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, false)));
        Assert.Empty(second.Capture(2).gamepads);
        Assert.Contains(GamepadButton.South, captured.gamepads[11].buttonsDown);
        using var third = source.CreateBackend();
        Assert.Empty(third.Capture(1).gamepads);
    }

    [Fact]
    public void UnknownDisconnectedAndOtherWindowControlsCannotEnterTheSession()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new GamepadEvent(7, new GamepadButtonChange(11, GamepadButton.South, true)));
        Assert.Empty(backend.Capture(1).gamepads);
        backend.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, true)));
        backend.ProcessEvent(new GamepadEvent(8, new GamepadButtonChange(11, GamepadButton.South, true)));
        Assert.Empty(backend.Capture(2).gamepads[11].buttonsDown);
        backend.ProcessEvent(new GamepadEvent(0, new GamepadConnectionChange(11, false)));
        backend.ProcessEvent(new GamepadEvent(7, new GamepadAxisChange(11, GamepadAxis.LeftX, 1f)));
        InputSnapshot removed = backend.Capture(3);
        Assert.Empty(removed.gamepads);
        Assert.False(Assert.IsType<GamepadConnectionChange>(Assert.Single(removed.sequence.changes)).connected);
    }

    [Fact]
    public void AxisAndIdentityValidationRejectsInvalidSamplesAndCopiesCallerState()
    {
        var axes = new Dictionary<GamepadAxis, float> { [GamepadAxis.LeftX] = -0.6f };
        var state = new GamepadState(11, axes: axes);
        axes.Clear();
        Assert.Equal(-0.5f, state.GetAxis(GamepadAxis.LeftX, 0.2f), 5);
        Assert.Throws<ArgumentOutOfRangeException>(() => new GamepadState(0));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GamepadAxisChange(11, GamepadAxis.LeftTrigger, -0.1f));
        Assert.Throws<ArgumentOutOfRangeException>(() => new GamepadAxisChange(11, GamepadAxis.LeftX, float.NaN));
        Assert.Throws<ArgumentOutOfRangeException>(() => state.GetAxis(GamepadAxis.LeftX, 1f));
        Assert.Throws<ArgumentException>(() => new GamepadEvent(0, new GamepadButtonChange(11, GamepadButton.South, true)));
        Assert.Throws<ArgumentException>(() => new InputSnapshot(0, InputSequence.empty,
            gamepads: new Dictionary<ulong, GamepadState> { [12] = state }));
    }
}
