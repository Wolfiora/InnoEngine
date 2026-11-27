using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Inno.Adapter.Input;
using Inno.Adapter.Modules.DotNet;
using Inno.Adapter.Serialization.DotNet;
using Inno.Core.Events;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Logging;
using Inno.Input.Runtime;
using Inno.Runtime;
using Inno.Runtime.Contracts;

namespace Inno.Input.Tests;

public sealed class InputRuntimeTests
{
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
        using IInputBackend firstBackend = first.CreateBackend();
        using IInputBackend secondBackend = second.CreateBackend();
        Event[] events =
        [
            new KeyPressedEvent(7, KeyCode.Space),
            new TextInputEvent(7, "typed"),
            new MouseScrolledEvent(7, 2f, 3f)
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
    public void ApplicationSuspensionClearsTransientInputAndRejectsBackgroundPresses()
    {
        using var source = new EventInputSource(windowID: 7);
        using IInputBackend backend = source.CreateBackend();
        source.ProcessEvent(new KeyPressedEvent(7, KeyCode.Space));
        source.ProcessEvent(new MouseButtonPressedEvent(7, MouseButton.Left));
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
        using var source = new EventInputSource(windowID: 0);
        using IInputBackend backend = source.CreateBackend();
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
        using var source = new EventInputSource(windowID: 0);
        IInputBackend retired = source.CreateBackend();
        using IInputBackend current = source.CreateBackend();
        retired.Dispose();
        source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Space));
        Assert.True(current.Capture(1).IsKeyDown(KeyCode.Space));
        source.Dispose();
        Assert.Throws<ObjectDisposedException>(() => source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Enter)));
    }

    [Fact]
    public void EventBackendCapturesTransitionsAndClearsTransientState()
    {
        using var backend = new EventInputBackend(windowID: 7);
        backend.ProcessEvent(new KeyPressedEvent(7, KeyCode.Space));
        backend.ProcessEvent(new MouseMovedEvent(7, 10f, 20f));
        backend.ProcessEvent(new MouseScrolledEvent(7, 0f, 2f));

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
        using var source = new EventInputSource(windowID: 0);
        using IInputBackend backend = source.CreateBackend();

        source.ProcessEvent(new KeyPressedEvent(42, KeyCode.Space));
        source.ProcessEvent(new MouseButtonPressedEvent(42, MouseButton.Left));
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
        using var source = new EventInputSource(windowID: 1);
        List<bool> observations = [];
        using EngineHost host = new EngineHostBuilder()
            .UseMetadataSources(
                new DotNetAssemblyCatalogSource(typeof(InputRuntimeTests).Assembly),
                new ReflectionTypeCatalogSource(),
                new ReflectionSerializationMetadataSource()
            )
            .Build();
        var options = new RuntimeSessionOptions
        {
            kind = RuntimeSessionKind.Play,
            applicationID = "tests.input",
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
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public void DeviceFramesPreserveConnectionsAndOrderedChangesAfterRetirement()
    {
        var events = new EventDispatcher();
        using EventHub hub = events.CreateHub();
        List<InputDeviceChange> observed = [];
        using IDisposable subscription = hub.Listen<InputDeviceEvent>(evnt => observed.Add(evnt.change));
        using var runtime = new InputRuntime(new EventInputBackend(0), new IdentityAllocator(), events);
        runtime.Attach();

        using InputDeviceConnection first = runtime.ConnectDevice("fixture.dial");
        using InputDeviceConnection second = runtime.ConnectDevice("fixture.dial");
        first.SetValue("pressed", 1);
        first.SetValue("pressed", 0);
        second.SetValue("position", 0.5);
        runtime.BeginFrame(default);
        InputSnapshot previous = Input.snapshot;
        runtime.EndFrame(default);
        first.Dispose();

        runtime.BeginFrame(default);
        InputSnapshot next = Input.snapshot;
        runtime.EndFrame(default);
        Assert.Equal(2, previous.devices.Count);
        Assert.Equal(5, previous.deviceChanges.Count);
        Assert.Equal(1, previous.deviceChanges[2].value);
        Assert.Equal(0, previous.deviceChanges[3].value);
        Assert.Equal(0.5, Assert.Single(next.devices).controls["position"]);
        Assert.Equal(InputDeviceChangeKind.Disconnected, Assert.Single(next.deviceChanges).kind);
        Assert.Equal(next.deviceChanges[0], observed[^1]);
        Assert.Throws<ObjectDisposedException>(() => first.SetValue("pressed", 1));
    }

    private sealed class InputProbeFactory(List<bool> observations) : IRuntimeSubsystemFactory
    {
        public RuntimeSubsystemDescriptor descriptor { get; } = new(
            new RuntimeSubsystemID("tests.input.probe"),
            dependencies: [new RuntimeSubsystemID("inno.runtime.input")]
        );

        public IRuntimeSubsystem Create(RuntimeSubsystemContext context) => new InputProbe(observations);
    }

    private sealed class InputProbe(List<bool> observations) : RuntimeSubsystem
    {
        protected override void OnUpdate(RuntimeFrame frame) => observations.Add(Input.IsKeyDown(KeyCode.Enter));

        protected override void OnLateUpdate(RuntimeFrame frame)
            => observations.Add(Input.WasKeyPressed(KeyCode.Enter));
    }
}
