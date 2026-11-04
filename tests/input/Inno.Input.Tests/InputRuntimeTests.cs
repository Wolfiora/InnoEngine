using Inno.Core.Logging;
using Inno.Adapter.Serialization.DotNet;
using Inno.Adapter.Modules.DotNet;
using Inno.Runtime.Contracts;
using System;
using System.Collections.Generic;
using System.IO;

using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Input.Runtime;
using Inno.Adapter.Input;
using Inno.Runtime;
using Xunit;

namespace Inno.Input.Tests;

public sealed class InputRuntimeTests
{
    [Fact]
    public void ButtonsAndWheelUseTheirOwnCoordinatesWithoutPrecedingMotion()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new MouseMovedEvent(7, 1, 2));
        backend.ProcessEvent(new MouseButtonPressedEvent(7, 20, 30, MouseButton.Left));
        backend.ProcessEvent(new MouseScrolledEvent(7, 40, 50, 0, 1));
        backend.ProcessEvent(new MouseButtonReleasedEvent(7, 60, 70, MouseButton.Left));
        InputSnapshot sample = backend.Capture(1);
        Assert.Collection(sample.sequence.changes,
            change => Assert.Equal(1, Assert.IsType<PointerMoveInputChange>(change).position.x),
            change => Assert.Equal(20, Assert.IsType<PointerButtonInputChange>(change).position.x),
            change => Assert.Equal(40, Assert.IsType<PointerScrollInputChange>(change).position.x),
            change => Assert.Equal(60, Assert.IsType<PointerButtonInputChange>(change).position.x));
        Assert.Equal(new Inno.Core.Mathematics.Vector2(60, 70), sample.mousePosition);
        Assert.False(sample.IsMouseButtonDown(MouseButton.Left));
    }

    [Fact]
    public void FirstObservedRepeatEstablishesHeldStateWithoutSynthesizingAPress()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new KeyPressedEvent(7, KeyCode.RightArrow, KeyModifier.None, true));
        InputSnapshot repeated = backend.Capture(1);
        Assert.True(repeated.IsKeyDown(KeyCode.RightArrow));
        Assert.False(repeated.WasKeyPressed(KeyCode.RightArrow));
        Assert.True(Assert.IsType<KeyInputChange>(Assert.Single(repeated.sequence.changes)).isRepeat);
        backend.ProcessEvent(new KeyReleasedEvent(7, KeyCode.RightArrow));
        InputSnapshot released = backend.Capture(2);
        Assert.True(released.WasKeyReleased(KeyCode.RightArrow));
        Assert.False(released.IsKeyDown(KeyCode.RightArrow));
    }

    [Fact]
    public void SnapshotRetainsTextKeyAndPointerOrderWithEventLocalModifiers()
    {
        using var backend = new EventInputBackend(7);
        backend.ProcessEvent(new TextInputEvent(7, "A"));
        backend.ProcessEvent(new KeyPressedEvent(7, KeyCode.LeftArrow, KeyModifier.Control));
        backend.ProcessEvent(new MouseMovedEvent(7, 12, 24));
        backend.ProcessEvent(new MouseButtonPressedEvent(7, 12f, 24f, MouseButton.Left));
        backend.ProcessEvent(new KeyReleasedEvent(7, KeyCode.LeftArrow, KeyModifier.None));
        backend.ProcessEvent(new TextInputEvent(7, "B"));
        InputSnapshot sample = backend.Capture(1);
        Assert.Collection(sample.sequence.changes,
            change => Assert.Equal("A", Assert.IsType<TextInputChange>(change).text),
            change => Assert.Equal(KeyModifier.Control, Assert.IsType<KeyInputChange>(change).modifiers),
            change => Assert.Equal(new Inno.Core.Mathematics.Vector2(12, 24), Assert.IsType<PointerMoveInputChange>(change).position),
            change => Assert.Equal(new Inno.Core.Mathematics.Vector2(12, 24), Assert.IsType<PointerButtonInputChange>(change).position),
            change => Assert.False(Assert.IsType<KeyInputChange>(change).isPressed),
            change => Assert.Equal("B", Assert.IsType<TextInputChange>(change).text));
        Assert.Same(InputSequence.empty, backend.Capture(2).sequence);
        Assert.Equal(6, sample.sequence.changes.Count);
    }

    [Fact]
    public void SnapshotOwnsItsCollectionsAndCannotBeMutatedThroughExposedViews()
    {
        var keys = new HashSet<KeyCode> { KeyCode.A };
        var buttons = new HashSet<MouseButton> { MouseButton.Left };
        InputChange[] changes = [
            new KeyInputChange(KeyCode.A, true, false, KeyModifier.None),
            new PointerButtonInputChange(default, MouseButton.Left, true, KeyModifier.None),
            new TextInputChange("initial", KeyModifier.None)
        ];
        var sample = new InputSnapshot(1, new InputSequence(changes), keysDown: keys, mouseButtonsDown: buttons);
        keys.Clear();
        buttons.Clear();
        changes[2] = new TextInputChange("changed", KeyModifier.None);
        Assert.True(sample.IsKeyDown(KeyCode.A));
        Assert.True(sample.WasKeyPressed(KeyCode.A));
        Assert.True(sample.IsMouseButtonDown(MouseButton.Left));
        Assert.Equal("initial", Assert.Single(sample.textInput));
        Assert.Throws<NotSupportedException>(() => ((ICollection<KeyCode>)sample.keysPressed).Clear());
        Assert.Throws<NotSupportedException>(() => ((ICollection<MouseButton>)sample.mouseButtonsPressed).Clear());
        Assert.Throws<NotSupportedException>(() => ((IList<string>)sample.textInput)[0] = "mutated");
    }

    [Fact]
    public void PointerMappingPreservesOrderingWheelUnitsAndSourceAfterFailure()
    {
        var key = new KeyInputChange(KeyCode.LeftArrow, true, true, KeyModifier.Control);
        var text = new TextInputChange("文", KeyModifier.None);
        var source = new InputSequence([
            key,
            new PointerMoveInputChange(new(20, 30), new(10, 15), KeyModifier.Shift),
            text,
            new PointerScrollInputChange(new(20, 30), new(1, -2), KeyModifier.Alt)
        ]);
        InputSequence mapped = source.MapPointer(static point => new(point.x * 2, point.y * 3));
        Assert.Same(key, mapped.changes[0]);
        Assert.Same(text, mapped.changes[2]);
        var movement = Assert.IsType<PointerMoveInputChange>(mapped.changes[1]);
        Assert.Equal(new Inno.Core.Mathematics.Vector2(40, 90), movement.position);
        Assert.Equal(new Inno.Core.Mathematics.Vector2(20, 45), movement.previousPosition);
        Assert.Equal(KeyModifier.Shift, movement.modifiers);
        Assert.Equal(new Inno.Core.Mathematics.Vector2(1, -2), mapped.scrollDelta);
        Assert.Empty(mapped.keysPressed);
        Assert.Throws<ArgumentOutOfRangeException>(() => source.MapPointer(static _ => new(float.NaN, 0)));
        Assert.Equal(new Inno.Core.Mathematics.Vector2(20, 30), Assert.IsType<PointerMoveInputChange>(source.changes[1]).position);
        var keyboardOnly = new InputSequence([key, text]);
        Assert.Same(keyboardOnly, keyboardOnly.MapPointer(static _ => throw new InvalidOperationException()));
    }

    [Fact]
    public void DirectEventBackendRejectsGloballyConsumedInput()
    {
        using var backend = new EventInputBackend(7);
        var consumed = new KeyPressedEvent(7, KeyCode.Space);
        consumed.HandleInGlobal();
        backend.ProcessEvent(consumed);
        Assert.False(backend.Capture(1).IsKeyDown(KeyCode.Space));
    }

    [Fact]
    public void WindowSourcesRemainIsolatedAcrossTextWheelAndRelease()
    {
        using var first = new EventInputSource(7);
        using var second = new EventInputSource(8);
        using var firstBackend = first.CreateBackend();
        using var secondBackend = second.CreateBackend();
        Event[] events = [
            new KeyPressedEvent(7, KeyCode.Space),
            new TextInputEvent(7, "typed"),
            new MouseScrolledEvent(7, 25f, 35f, 2f, 3f)
        ];
        foreach (Event evnt in events)
        {
            first.ProcessEvent(evnt);
            second.ProcessEvent(evnt);
        }
        InputSnapshot accepted = firstBackend.Capture(1);
        InputSnapshot isolated = secondBackend.Capture(1);
        Assert.True(accepted.IsKeyDown(KeyCode.Space));
        Assert.Equal("typed", Assert.Single(accepted.textInput));
        Assert.Equal(3f, accepted.scrollDelta.y);
        Assert.False(isolated.IsKeyDown(KeyCode.Space));
        Assert.Empty(isolated.textInput);
        Assert.Equal(0f, isolated.scrollDelta.y);
        first.ProcessEvent(new WindowFocusChangedEvent(7, false));
        Assert.True(firstBackend.Capture(2).WasKeyReleased(KeyCode.Space));
        Assert.False(secondBackend.Capture(2).WasKeyReleased(KeyCode.Space));
    }

    [Fact]
    public void WindowCloseReleasesHeldInputWithoutDeliveringQueuedEditsToOtherWindows()
    {
        using var first = new EventInputBackend(7);
        using var second = new EventInputBackend(8);
        first.ProcessEvent(new KeyPressedEvent(7, KeyCode.Space, KeyModifier.Control));
        first.ProcessEvent(new MouseButtonPressedEvent(7, 12f, 24f, MouseButton.Left));
        first.ProcessEvent(new TextInputEvent(7, "uncommitted"));
        second.ProcessEvent(new KeyPressedEvent(8, KeyCode.Enter));
        first.ProcessEvent(new WindowCloseEvent(7));
        second.ProcessEvent(new WindowCloseEvent(7));
        InputSnapshot closed = first.Capture(1);
        Assert.False(closed.IsKeyDown(KeyCode.Space));
        Assert.False(closed.IsMouseButtonDown(MouseButton.Left));
        Assert.Empty(closed.keysPressed);
        Assert.Empty(closed.textInput);
        Assert.Equal(KeyModifier.None, closed.modifiers);
        Assert.Collection(closed.sequence.changes,
            change => Assert.False(Assert.IsType<KeyInputChange>(change).isPressed),
            change => Assert.False(Assert.IsType<PointerButtonInputChange>(change).isPressed));
        Assert.True(second.Capture(1).IsKeyDown(KeyCode.Enter));
        Assert.Same(InputSequence.empty, first.Capture(2).sequence);
    }

    [Fact]
    public void ApplicationSuspensionClearsTransientInputAndRejectsBackgroundPresses()
    {
        using var source = new EventInputSource(windowId: 7);
        using var backend = source.CreateBackend();
        source.ProcessEvent(new KeyPressedEvent(7, KeyCode.Space));
        source.ProcessEvent(new MouseButtonPressedEvent(7, 12f, 24f, MouseButton.Left));
        source.ProcessEvent(new TextInputEvent(7, "x"));
        source.ProcessEvent(new ApplicationSuspensionChangedEvent(true));
        source.ProcessEvent(new KeyPressedEvent(7, KeyCode.Enter));
        InputSnapshot suspended = backend.Capture(1);
        Assert.False(suspended.IsKeyDown(KeyCode.Space));
        Assert.False(suspended.WasKeyPressed(KeyCode.Space));
        Assert.True(suspended.WasKeyReleased(KeyCode.Space));
        Assert.False(suspended.IsMouseButtonDown(MouseButton.Left));
        Assert.False(suspended.IsKeyDown(KeyCode.Enter));
        Assert.Empty(suspended.textInput);
        source.ProcessEvent(new ApplicationSuspensionChangedEvent(false));
        source.ProcessEvent(new KeyPressedEvent(7, KeyCode.Enter));
        Assert.True(backend.Capture(2).WasKeyPressed(KeyCode.Enter));
    }

    [Fact]
    public void GloballyConsumedPlatformEventsNeverReachSessionInput()
    {
        using var source = new EventInputSource(windowId: 0);
        using var backend = source.CreateBackend();
        var consumed = new KeyPressedEvent(42, KeyCode.Space);
        consumed.HandleInGlobal();
        source.ProcessEvent(consumed);
        Assert.False(backend.Capture(1).IsKeyDown(KeyCode.Space));
        source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Space));
        Assert.True(backend.Capture(2).IsKeyDown(KeyCode.Space));
    }

    [Fact]
    public void DisposingOneSessionDoesNotDisconnectOtherSessionSubscriptions()
    {
        using var source = new EventInputSource(windowId: 0);
        var retired = source.CreateBackend();
        using var current = source.CreateBackend();
        retired.Dispose();
        source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Space));
        Assert.True(current.Capture(1).IsKeyDown(KeyCode.Space));
        source.Dispose();
        Assert.Throws<ObjectDisposedException>(() => source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Enter)));
    }

    [Fact]
    public void EventBackendCapturesTransitionsAndClearsTransientState()
    {
        using var backend = new EventInputBackend(windowId: 7);
        backend.ProcessEvent(new KeyPressedEvent(7, KeyCode.Space));
        backend.ProcessEvent(new MouseMovedEvent(7, 10f, 20f));
        backend.ProcessEvent(new MouseScrolledEvent(7, 10f, 20f, 0f, 2f));

        InputSnapshot first = backend.Capture(3);
        InputSnapshot second = backend.Capture(4);

        Assert.True(first.IsKeyDown(KeyCode.Space));
        Assert.True(first.WasKeyPressed(KeyCode.Space));
        Assert.Equal(10f, first.mousePosition.x);
        Assert.Equal(2f, first.scrollDelta.y);
        Assert.True(second.IsKeyDown(KeyCode.Space));
        Assert.False(second.WasKeyPressed(KeyCode.Space));
        Assert.Equal(0f, second.mouseDelta.x);
        Assert.Equal(0f, second.scrollDelta.y);
    }

    [Fact]
    public void AllWindowSourceAcceptsDetachedWindowAndClearsItOnFocusLoss()
    {
        using var source = new EventInputSource(windowId: 0);
        using var backend = source.CreateBackend();

        source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Space));
        source.ProcessEvent(new MouseButtonPressedEvent(42, 25f, 35f, MouseButton.Left));
        InputSnapshot pressed = backend.Capture(1);
        Assert.True(pressed.IsKeyDown(KeyCode.Space));
        Assert.True(pressed.IsMouseButtonDown(MouseButton.Left));

        source.ProcessEvent(new WindowFocusChangedEvent(42, false));
        InputSnapshot released = backend.Capture(2);
        Assert.False(released.IsKeyDown(KeyCode.Space));
        Assert.True(released.WasKeyReleased(KeyCode.Space));
        Assert.False(released.IsMouseButtonDown(MouseButton.Left));
    }

    [Fact]
    public void RuntimeSubsystemBindsSnapshotAcrossSimulationPhases()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoInputRuntimeTests", Guid.NewGuid().ToString("N"));
        using var source = new EventInputSource(windowId: 1);
        var observations = new List<bool>();
        using EngineHost host = new EngineHostBuilder()
                .UseMetadataSources(new DotNetAssemblyCatalogSource(typeof(InputRuntimeTests).Assembly),
                    new ReflectionTypeCatalogSource(), new ReflectionSerializationMetadataSource())
            .Build();
        var options = new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Play,
            applicationId = "tests.input",
            createLogSink = _ => new FileLogSink(Path.Combine(Path.Combine(root, "tests.input"), "Logs")),
            jobExecutionMode = RuntimeJobExecutionMode.SingleThread,
            createSubsystems = owner =>
            [
                new InputRuntimeFactory(_ => source.CreateBackend()),
                new InputProbeFactory(observations)
            ]
        };

        using (RuntimeSession session = host.CreateSession(options))
        {
            source.ProcessEvent(new KeyPressedEvent(1, KeyCode.Enter));
            session.Tick(0.01f);
        }

        Assert.Equal([true, true], observations);
        Assert.Throws<InvalidOperationException>(() => _ = Input.snapshot);
        if (Directory.Exists(root))
            Directory.Delete(root, recursive: true);
    }

    private sealed class InputProbeFactory(List<bool> observations) : IRuntimeSubsystemFactory
    {
        public RuntimeSubsystemDescriptor descriptor { get; } = new(
            new RuntimeSubsystemId("tests.input.probe"),
            dependencies: [new RuntimeSubsystemId("inno.runtime.input")]);

        public IRuntimeSubsystem Create(RuntimeSubsystemContext context) => new InputProbe(observations);
    }

    private sealed class InputProbe(List<bool> observations) : RuntimeSubsystem
    {
        protected override void OnUpdate(RuntimeFrame frame)
            => observations.Add(Input.IsKeyDown(KeyCode.Enter));

        protected override void OnLateUpdate(RuntimeFrame frame)
            => observations.Add(Input.WasKeyPressed(KeyCode.Enter));
    }
}
