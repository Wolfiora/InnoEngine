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
            ? new MouseButtonPressedEvent(42, MouseButton.Left)
            : new MouseScrolledEvent(42, 0f, 1f);
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
        Assert.NotNull(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));

        var moved = new MouseMovedEvent(42, 150f, 150f);
        moved.HandleInGlobal();
        Assert.Null(capture.Route(moved));
        Assert.NotNull(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));
        Assert.Equal(0u, capture.CompleteFrame());
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
        Assert.Null(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));
        Assert.Equal(0u, capture.CompleteFrame());
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
        Assert.Equal(0u, capture.CompleteFrame());
    }

    [Fact]
    public void ConsumedEventsCannotStartCapture()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        var pressed = new MouseButtonPressedEvent(42, MouseButton.Left);
        var moved = new MouseMovedEvent(42, 10f, 10f);
        pressed.HandleInGlobal();
        moved.HandleInGlobal();

        Assert.Null(capture.Route(pressed));
        Assert.Null(capture.Route(moved));
        Assert.Null(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));
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
        Assert.NotNull(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));
        Event release = releaseKind switch
        {
            "key" => new KeyReleasedEvent(42, KeyCode.A),
            "button" => new MouseButtonReleasedEvent(42, MouseButton.Left),
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
        Assert.Equal(0u, capture.CompleteFrame());
        Assert.Equal(0u, capture.CompleteFrame());

        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.Null(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));
        Assert.NotNull(capture.Route(new KeyPressedEvent(42, KeyCode.B)));
    }

    [Fact]
    public void ConsumedReleaseFromAnotherWindowDoesNotResetGameInput()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.NotNull(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));
        var unrelated = new MouseButtonReleasedEvent(1, MouseButton.Left);
        unrelated.HandleInGlobal();

        Assert.Null(capture.Route(unrelated));
        Assert.Equal(0u, capture.CompleteFrame());
        Assert.NotNull(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.NotNull(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));
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
        Assert.Equal(0u, capture.CompleteFrame());

        Assert.Null(capture.Route(new KeyPressedEvent(1, KeyCode.A)));
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        MouseMovedEvent moved = Assert.IsType<MouseMovedEvent>(capture.Route(new MouseMovedEvent(42, 25f, 35f)));
        Assert.Equal(5f, moved.x);
        Assert.Equal(5f, moved.y);
        Assert.IsType<MouseButtonPressedEvent>(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));
        Assert.Null(capture.Route(new MouseButtonPressedEvent(1, MouseButton.Right)));
    }

    [Fact]
    public void OccludedOrUnfocusedGameViewCannotStartPointerInput()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: false);
        capture.CompleteFrame();

        Assert.Null(capture.Route(new MouseMovedEvent(42, 30f, 30f)));
        Assert.Null(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));

        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: false, hovered: true);
        capture.CompleteFrame();
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }

    [Fact]
    public void FocusLossAndHiddenPanelReleaseHeldInput()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame();
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.IsType<MouseButtonPressedEvent>(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));

        capture.BeginFrame();
        Assert.Equal(42u, capture.CompleteFrame());
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));

        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame();
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.IsType<WindowFocusChangedEvent>(capture.Route(new WindowFocusChangedEvent(42, false)));
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
        Assert.Equal(0u, capture.CompleteFrame());
    }

    [Fact]
    public void MissingOrUnfocusedPlatformWindowCannotActivateGameView()
    {
        var capture = new EditorGameInputCapture(_ => null);
        capture.BeginFrame();
        Assert.False(capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true));
        Assert.Equal(0u, capture.CompleteFrame());
        Assert.Null(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }

    [Fact]
    public void CapturedPointerCanReleaseOutsideImage()
    {
        var capture = new EditorGameInputCapture(_ => 42u);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame();

        Assert.IsType<MouseButtonPressedEvent>(capture.Route(new MouseButtonPressedEvent(42, MouseButton.Left)));
        Assert.IsType<MouseMovedEvent>(capture.Route(new MouseMovedEvent(42, 150f, 150f)));
        Assert.IsType<MouseButtonReleasedEvent>(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));
        Assert.Null(capture.Route(new MouseButtonReleasedEvent(42, MouseButton.Left)));
    }

    [Fact]
    public void MovingGameViewToAnotherPlatformWindowResetsOldInput()
    {
        uint currentWindowId = 1;
        var capture = new EditorGameInputCapture(_ => currentWindowId);
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        capture.CompleteFrame();
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(1, KeyCode.A)));

        currentWindowId = 42;
        capture.BeginFrame();
        capture.Report(9, Vector2.Zero, new Vector2(100f), focused: true, hovered: true);
        Assert.Equal(1u, capture.CompleteFrame());
        Assert.Null(capture.Route(new KeyPressedEvent(1, KeyCode.A)));
        Assert.IsType<KeyPressedEvent>(capture.Route(new KeyPressedEvent(42, KeyCode.A)));
    }
}
