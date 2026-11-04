using System.Numerics;

using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Editor.ImGui;
using Inno.Input;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class EditorGameInputCaptureTests
{
    [Theory]
    [InlineData(TextCompositionPhase.Committed)]
    [InlineData(TextCompositionPhase.Cancelled)]
    public void ConsumedCompositionTerminalCancelsOnlyTheCapturedWindow(TextCompositionPhase phase)
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        using var backend = new EventInputBackend(0);
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        capture.CompleteFrame(out uint gained);
        backend.ProcessEvent(new WindowFocusChangedEvent(gained, true));
        var start = new TextCompositionEvent(42, new(TextCompositionPhase.Started, new(1, "组合")));
        backend.ProcessEvent(Assert.IsType<TextCompositionEvent>(capture.Route(start)));
        Assert.NotNull(backend.Capture(1).textComposition);
        var foreign = new TextCompositionEvent(43, new(phase, new(1, "other")));
        foreign.HandleInGlobal();
        Assert.Null(capture.Route(foreign));
        Assert.NotNull(backend.Capture(2).textComposition);
        var terminal = new TextCompositionEvent(42, new(phase, new(1, "组合")));
        terminal.HandleInGlobal();
        backend.ProcessEvent(Assert.IsType<WindowFocusChangedEvent>(capture.Route(terminal)));
        InputSnapshot released = backend.Capture(3);
        Assert.Null(released.textComposition);
        Assert.Empty(released.textInput);
        Assert.Equal(TextCompositionPhase.Cancelled,
            Assert.IsType<TextCompositionChange>(Assert.Single(released.sequence.changes)).phase);
        Assert.Null(capture.Route(start));
    }

    [Fact]
    public void SuspensionReachesGameInputWithoutReplayingControlsBeforeFocusRecovery()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        using var backend = new EventInputBackend(0);
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(new GamepadEvent(0, new GamepadConnectionChange(11, true)))));
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        capture.CompleteFrame(out uint gained);
        backend.ProcessEvent(new WindowFocusChangedEvent(gained, true));
        var press = new GamepadEvent(42, new GamepadButtonChange(11, GamepadButton.South, true));
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(press)));
        backend.Capture(1);
        backend.ProcessEvent(Assert.IsType<ApplicationSuspensionChangedEvent>(capture.Route(new ApplicationSuspensionChangedEvent(true))));
        Assert.Empty(backend.Capture(2).gamepads[11].buttonsDown);
        Assert.Null(capture.Route(press));
        backend.ProcessEvent(Assert.IsType<ApplicationSuspensionChangedEvent>(capture.Route(new ApplicationSuspensionChangedEvent(false))));
        Assert.Null(capture.Route(press));
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        capture.CompleteFrame(out gained);
        Assert.Equal(42u, gained);
        backend.ProcessEvent(new WindowFocusChangedEvent(gained, true));
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(press)));
        Assert.Contains(GamepadButton.South, backend.Capture(3).gamepads[11].buttonsDown);
    }

    [Fact]
    public void ControllersFollowGameViewFocusWithoutLosingConnectionFacts()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        using var backend = new EventInputBackend(0);
        var connected = new GamepadEvent(0, new GamepadConnectionChange(11, true));
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(connected)));
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        Assert.Equal(0u, capture.CompleteFrame(out uint gained));
        Assert.Equal(42u, gained);
        backend.ProcessEvent(new WindowFocusChangedEvent(gained, true));
        var press = new GamepadEvent(42, new GamepadButtonChange(11, GamepadButton.South, true));
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(press)));
        Assert.Contains(GamepadButton.South, backend.Capture(1).gamepads[11].buttonsDown);

        capture.BeginFrame();
        uint lost = capture.CompleteFrame(out gained);
        Assert.Equal(42u, lost);
        Assert.Equal(0u, gained);
        backend.ProcessEvent(new WindowFocusChangedEvent(lost, false));
        Assert.Empty(backend.Capture(2).gamepads[11].buttonsDown);
        Assert.Null(capture.Route(press));

        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        capture.CompleteFrame(out gained);
        Assert.Equal(42u, gained);
        backend.ProcessEvent(new WindowFocusChangedEvent(gained, true));
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(press)));
        Assert.Contains(GamepadButton.South, backend.Capture(3).gamepads[11].buttonsDown);
        capture.CompleteFrame(out gained);
        Assert.Equal(0u, gained);

        var release = new GamepadEvent(42, new GamepadButtonChange(11, GamepadButton.South, false));
        release.HandleInGlobal();
        backend.ProcessEvent(Assert.IsType<WindowFocusChangedEvent>(capture.Route(release)));
        Assert.Empty(backend.Capture(4).gamepads[11].buttonsDown);
        var removed = new GamepadEvent(0, new GamepadConnectionChange(11, false));
        backend.ProcessEvent(Assert.IsType<GamepadEvent>(capture.Route(removed)));
        Assert.Empty(backend.Capture(5).gamepads);
    }

    [Fact]
    public void ButtonAndWheelProjectionPreservePositionsAndSharedConsumption()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.Report(9, new Vector2(20, 30), new Vector2(100), focused: true, hovered: true);
        var press = new MouseButtonPressedEvent(42, 25, 36, MouseButton.Left);
        MouseButtonPressedEvent routedPress = Assert.IsType<MouseButtonPressedEvent>(capture.Route(press));
        Assert.Equal(5, routedPress.x);
        Assert.Equal(6, routedPress.y);
        routedPress.HandleInGlobal();
        Assert.True(press.isGlobalHandled);

        var wheel = new MouseScrolledEvent(42, 30, 40, 2, -1);
        MouseScrolledEvent routedWheel = Assert.IsType<MouseScrolledEvent>(capture.Route(wheel));
        Assert.Equal(10, routedWheel.x);
        Assert.Equal(10, routedWheel.y);
        Assert.Equal(-1, routedWheel.offsetY);
        wheel.HandleInGlobal();
        Assert.True(routedWheel.isGlobalHandled);

        var release = new MouseButtonReleasedEvent(42, 200, 210, MouseButton.Left);
        MouseButtonReleasedEvent routedRelease = Assert.IsType<MouseButtonReleasedEvent>(capture.Route(release));
        Assert.Equal(180, routedRelease.x);
        Assert.Equal(180, routedRelease.y);
        routedRelease.HandleInGlobal();
        Assert.True(release.isGlobalHandled);
        Assert.Null(capture.Route(new MouseButtonPressedEvent(42, 200, 210, MouseButton.Left)));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void ConsumedMouseMovementStillUpdatesPointerBounds(bool pressed)
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.Report(9, new Vector2(20f, 30f), new Vector2(100f), focused: true, hovered: true);
        var moved = new MouseMovedEvent(42, 150f, 150f);
        moved.HandleInGlobal();

        Assert.Null(capture.Route(moved));
        Event pointer = pressed
            ? new MouseButtonPressedEvent(42, 150f, 150f, MouseButton.Left)
            : new MouseScrolledEvent(42, 150f, 150f, 0f, 1f);
        Assert.Null(capture.Route(pointer));
        Assert.NotNull(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }

    [Fact]
    public void ConsumedMouseMovementDoesNotReleaseCapturedInputOrChangeOtherWindows()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        var unrelated = new MouseMovedEvent(1, 150f, 150f);
        unrelated.HandleInGlobal();
        Assert.Null(capture.Route(unrelated));
        Assert.NotNull(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));

        var moved = new MouseMovedEvent(42, 150f, 150f);
        moved.HandleInGlobal();
        Assert.Null(capture.Route(moved));
        Assert.NotNull(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));
        Assert.Equal(0u, capture.CompleteFrame(out _));
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public void WindowExitDisarmsCaptureBeforeItsFirstDeliveredInput(
        bool close,
        bool consumed
    ) {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Event exit = close ? new WindowCloseEvent(42) : new WindowFocusChangedEvent(42, false);
        if (consumed)
            exit.HandleInGlobal();

        Assert.Null(capture.Route(exit));
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.Null(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));
        Assert.Equal(0u, capture.CompleteFrame(out _));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void DispatcherRoutesInputOnlyAfterEditorConsumption(bool globalConsumption)
    {
        var dispatcher = new EventDispatcher();
        using EventHub ui = dispatcher.CreateHub(100);
        using System.IDisposable subscription = ui.Listen<KeyPressedEvent>(evnt =>
        {
            if (globalConsumption)
                evnt.HandleInGlobal();
            else
                evnt.HandleInHub();
        });
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Event? routed = null;
        dispatcher.dispatched += evnt => routed = capture.Route(evnt);
        var pressed = new KeyPressedEvent(42, KeyCode.A);
        dispatcher.Enqueue(pressed);

        Assert.Null(routed);
        dispatcher.Flush();

        if (globalConsumption)
            Assert.Null(routed);
        else
            Assert.Same(pressed, routed);
    }

    [Fact]
    public void ConsumedReleaseResetsInputDuringDispatchBeforeSimulation()
    {
        var dispatcher = new EventDispatcher();
        using EventHub ui = dispatcher.CreateHub(100);
        using System.IDisposable subscription = ui.Listen<KeyReleasedEvent>(evnt => evnt.HandleInGlobal());
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        using var source = new EventInputSource(0);
        using IInputBackend backend = source.CreateBackend();
        dispatcher.dispatched += evnt =>
        {
            Event? routed = capture.Route(evnt);
            if (routed is not null)
                source.ProcessEvent(routed);
        };
        dispatcher.Enqueue(new KeyPressedEvent(42, KeyCode.A));
        dispatcher.Flush();
        Assert.True(backend.Capture(1).IsKeyDown(KeyCode.A));
        dispatcher.Enqueue(new KeyReleasedEvent(42, KeyCode.A));

        dispatcher.Flush();

        Assert.False(backend.Capture(2).IsKeyDown(KeyCode.A));
        Assert.Equal(0u, capture.CompleteFrame(out _));
    }

    [Fact]
    public void ConsumedEventsCannotStartCapture()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        var pressed = new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left);
        var moved = new MouseMovedEvent(42, 10f, 10f);
        pressed.HandleInGlobal();
        moved.HandleInGlobal();

        Assert.Null(capture.Route(pressed));
        Assert.Null(capture.Route(moved));
        Assert.Null(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));
    }

    [Theory]
    [InlineData("key")]
    [InlineData("button")]
    [InlineData("focus")]
    [InlineData("close")]
    public void ConsumedReleasesResetHeldInputWithoutReplayingTheEvent(string releaseKind)
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.NotNull(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.NotNull(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));
        Event release = releaseKind switch
        {
            "key" => new KeyReleasedEvent(42, KeyCode.A),
            "button" => new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left),
            "focus" => new WindowFocusChangedEvent(42, false),
            "close" => new WindowCloseEvent(42),
            _ => throw new System.ArgumentException("Unknown release kind.", nameof(releaseKind))
        };
        release.HandleInGlobal();

        WindowFocusChangedEvent reset = Assert.IsType<WindowFocusChangedEvent>(capture.Route(release));
        Assert.False(reset.isFocused);
        Assert.Equal(42u, reset.windowId);
        Assert.False(reset.isGlobalHandled);
        Assert.Null(capture.Route(release));
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.B)));
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.Equal(0u, capture.CompleteFrame(out _));
        Assert.Equal(0u, capture.CompleteFrame(out _));

        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.Null(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));
        Assert.NotNull(capture.Route(new KeyPressedEvent(42, KeyCode.B)));
    }

    [Fact]
    public void ConsumedReleaseFromAnotherWindowDoesNotResetGameInput()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.NotNull(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));
        var unrelated = new MouseButtonReleasedEvent(1, 25f, 35f, MouseButton.Left);
        unrelated.HandleInGlobal();

        Assert.Null(capture.Route(unrelated));
        Assert.Equal(0u, capture.CompleteFrame(out _));
        Assert.NotNull(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.NotNull(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));
    }

    [Fact]
    public void CoordinateRoutingKeepsConsumptionSharedInBothDirections()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, new Vector2(20f, 30f), new Vector2(100f), focused: true, hovered: true);
        var original = new MouseMovedEvent(42, 25f, 35f);
        MouseMovedEvent routed = Assert.IsType<MouseMovedEvent>(capture.Route(original));
        routed.HandleInGlobal();

        Assert.True(original.isGlobalHandled);
        Assert.True(routed.isGlobalHandled);

        var second = new MouseMovedEvent(42, 25f, 35f);
        MouseMovedEvent secondRoute = Assert.IsType<MouseMovedEvent>(capture.Route(second));
        second.HandleInGlobal();
        Assert.True(secondRoute.isGlobalHandled);
    }

    [Fact]
    public void DetachedFocusedGameViewReceivesOnlyItsWindowInput()
    {
        var capture = new EditorGameInputCapture(id => id == 9 ? 42u : null);
        capture.BeginFrame();
        capture.Report(9, new Vector2(20f, 30f), new Vector2(100f, 80f), focused: true, hovered: true);
        Assert.Equal(0u, capture.CompleteFrame(out _));

        Assert.Null(capture.Route(new KeyPressedEvent(1, KeyCode.A)));
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        MouseMovedEvent moved = Assert.IsType<MouseMovedEvent>(capture.Route(new MouseMovedEvent(42, 25f, 35f)));
        Assert.Equal(5f, moved.x);
        Assert.Equal(5f, moved.y);
        Assert.IsType<MouseButtonPressedEvent>(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));
        Assert.Null(capture.Route(new MouseButtonPressedEvent(1, 25f, 35f, MouseButton.Right)));
    }

    [Fact]
    public void OccludedOrUnfocusedGameViewCannotStartPointerInput()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        capture.CompleteFrame(out _);

        Assert.Null(capture.Route(new MouseMovedEvent(42, 30f, 30f)));
        Assert.Null(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));

        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: false, hovered: true);
        capture.CompleteFrame(out _);
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }

    [Fact]
    public void FocusLossAndHiddenPanelReleaseHeldInput()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame(out _);
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.IsType<MouseButtonPressedEvent>(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));

        capture.BeginFrame();
        Assert.Equal(42u, capture.CompleteFrame(out _));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));

        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame(out _);
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.IsType<WindowFocusChangedEvent>(capture.Route(new WindowFocusChangedEvent(42, false)));
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.Equal(0u, capture.CompleteFrame(out _));
    }

    [Fact]
    public void MissingOrUnfocusedPlatformWindowCannotActivateGameView()
    {
        var capture = new EditorGameInputCapture(_ => null);
        capture.BeginFrame();
        Assert.False(capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true));
        Assert.Equal(0u, capture.CompleteFrame(out _));
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }

    [Fact]
    public void CapturedPointerCanReleaseOutsideImage()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame(out _);

        Assert.IsType<MouseButtonPressedEvent>(capture.Route(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left)));
        Assert.IsType<MouseMovedEvent>(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.IsType<MouseButtonReleasedEvent>(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, 25f, 35f, MouseButton.Left)));
    }

    [Fact]
    public void MovingGameViewToAnotherPlatformWindowResetsOldInput()
    {
        uint currentWindowId = 1;
        var capture = new EditorGameInputCapture(_ => currentWindowId);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame(out _);
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(1, KeyCode.A)));

        currentWindowId = 42;
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.Equal(1u, capture.CompleteFrame(out _));
        Assert.Null(capture.Route(new KeyPressedEvent(1, KeyCode.A)));
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }
}
