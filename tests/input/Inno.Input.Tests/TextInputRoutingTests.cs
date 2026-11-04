using System;
using System.Threading;

using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Mathematics;
using Inno.Input.Runtime;
using Inno.Platform;
using Xunit;

namespace Inno.Input.Tests;

public sealed class TextInputRoutingTests
{
    [Fact]
    public void RuntimeTextAreaUsesTheHostRouteWithoutRestartingComposition()
    {
        using var window = new Window(7);
        using var source = new EventInputSource(0);
        source.SetTextInputTarget(window, new(30, 50));
        using IInputBackend backend = source.CreateBackend();
        using var runtime = new InputRuntime(backend);
        Assert.Throws<InvalidOperationException>(() => runtime.TryBeginTextInput());
        runtime.Attach();
        using IInputTextSession session = Assert.IsAssignableFrom<IInputTextSession>(runtime.TryBeginTextInput());
        Assert.Null(runtime.TryBeginTextInput());
        session.SetArea(new(2, 3, 80, 20), 12);
        Assert.Equal(new Rect(32, 53, 80, 20), window.area);
        Assert.Equal(12, window.cursor);
        source.SetTextInputTarget(window, new(100, 200));
        session.SetArea(new(2, 3, 80, 20), 12);
        Assert.Equal(new Rect(102, 203, 80, 20), window.area);
        Assert.Equal(1, window.acquisitions);
        Assert.Throws<ArgumentOutOfRangeException>(() => session.SetArea(new(float.NaN, 0, 80, 20), 0));
        Assert.True(session.isActive);
    }

    [Fact]
    public void RuntimeRetirementReleasesThePlatformLeaseAndStopsAdmission()
    {
        using var window = new Window(7);
        using var source = new EventInputSource(7);
        source.SetTextInputTarget(window, default);
        using var runtime = new InputRuntime(source.CreateBackend());
        runtime.Attach();
        using IInputTextSession session = Assert.IsAssignableFrom<IInputTextSession>(runtime.TryBeginTextInput());
        runtime.Dispose();
        Assert.False(session.isActive);
        Assert.Equal(1, window.releases);
        Assert.Throws<ObjectDisposedException>(() => runtime.TryBeginTextInput());
        using IInputBackend replacement = source.CreateBackend();
        using IInputTextSession next = Assert.IsAssignableFrom<IInputTextSession>(
            Assert.IsAssignableFrom<IInputTextBackend>(replacement).TryBeginTextInput());
        Assert.True(next.isActive);
    }

    [Fact]
    public void RuntimeSessionsCannotStealOrReleaseEachOthersTextInput()
    {
        using var window = new Window(7);
        using var source = new EventInputSource(7);
        source.SetTextInputTarget(window, default);
        using IInputBackend first = source.CreateBackend();
        using IInputBackend second = source.CreateBackend();
        var firstText = Assert.IsAssignableFrom<IInputTextBackend>(first);
        var secondText = Assert.IsAssignableFrom<IInputTextBackend>(second);
        IInputTextSession old = Assert.IsAssignableFrom<IInputTextSession>(firstText.TryBeginTextInput());
        Assert.Null(secondText.TryBeginTextInput());
        first.Dispose();
        Assert.False(old.isActive);
        using IInputTextSession current = Assert.IsAssignableFrom<IInputTextSession>(secondText.TryBeginTextInput());
        old.Dispose();
        Assert.True(current.isActive);
        Assert.Throws<ObjectDisposedException>(() => firstText.TryBeginTextInput());
    }

    [Fact]
    public void WindowReplacementAndFocusLossInvalidatePriorSessions()
    {
        using var window = new Window(7);
        using var replacement = new Window(8);
        using var source = new EventInputSource(0);
        using IInputBackend backend = source.CreateBackend();
        var text = Assert.IsAssignableFrom<IInputTextBackend>(backend);
        Assert.Null(text.TryBeginTextInput());
        source.SetTextInputTarget(window, default);
        using IInputTextSession first = Assert.IsAssignableFrom<IInputTextSession>(text.TryBeginTextInput());
        source.SetTextInputTarget(replacement, default);
        Assert.False(first.isActive);
        Assert.Throws<InvalidOperationException>(() => first.SetArea(new(0, 0, 10, 10), 0));
        using IInputTextSession second = Assert.IsAssignableFrom<IInputTextSession>(text.TryBeginTextInput());
        source.ProcessEvent(new WindowFocusChangedEvent(8, false));
        Assert.False(second.isActive);
        Assert.Null(text.TryBeginTextInput());
        source.ProcessEvent(new WindowFocusChangedEvent(8, true));
        using IInputTextSession third = Assert.IsAssignableFrom<IInputTextSession>(text.TryBeginTextInput());
        source.SetTextInputTarget(null, default);
        Assert.False(third.isActive);
        Assert.Null(text.TryBeginTextInput());
    }

    [Fact]
    public void FailedRetirementPreservesTheOldRouteForRetry()
    {
        using var window = new Window(7);
        using var replacement = new Window(8);
        using var source = new EventInputSource(0);
        using IInputBackend backend = source.CreateBackend();
        source.SetTextInputTarget(window, new(3, 4));
        using IInputTextSession session = Assert.IsAssignableFrom<IInputTextSession>(
            Assert.IsAssignableFrom<IInputTextBackend>(backend).TryBeginTextInput());
        window.rejectRetirement = true;
        Assert.Throws<InvalidOperationException>(() => source.SetTextInputTarget(replacement, new(50, 60)));
        Assert.True(session.isActive);
        session.SetArea(new(1, 2, 10, 10), 0);
        Assert.Equal(new Rect(4, 6, 10, 10), window.area);
        Assert.Throws<InvalidOperationException>(backend.Dispose);
        Assert.True(session.isActive);
        window.rejectRetirement = false;
        backend.Dispose();
        Assert.False(session.isActive);
    }

    [Fact]
    public void SourceRetirementAndRepeatedLeasesReleaseOnlyTheirOwnWindow()
    {
        using var window = new Window(7);
        using var source = new EventInputSource(7);
        source.SetTextInputTarget(window, default);
        using IInputBackend backend = source.CreateBackend();
        var text = Assert.IsAssignableFrom<IInputTextBackend>(backend);
        for (int cycle = 0; cycle < 100; cycle++)
        {
            using IInputTextSession session = Assert.IsAssignableFrom<IInputTextSession>(text.TryBeginTextInput());
            session.SetArea(new(1, 2, 30, 20), 3);
        }
        Assert.Equal(100, window.releases);
        using IInputTextSession final = Assert.IsAssignableFrom<IInputTextSession>(text.TryBeginTextInput());
        source.Dispose();
        Assert.False(final.isActive);
        Assert.Equal(101, window.releases);
        Assert.Null(text.TryBeginTextInput());
        Assert.False(window.isClosed);
    }

    [Fact]
    public void ProviderUsesThePrimaryWindowOnlyForASingleWindowRoute()
    {
        using var window = new Window(7);
        var provider = new EventInputBackendProvider();
        using IInputEventSource player = provider.CreateEventSource(window, false);
        using IInputBackend playerBackend = player.CreateBackend();
        using IInputTextSession session = Assert.IsAssignableFrom<IInputTextSession>(
            Assert.IsAssignableFrom<IInputTextBackend>(playerBackend).TryBeginTextInput());
        using IInputEventSource editor = provider.CreateEventSource(window, true);
        using IInputBackend editorBackend = editor.CreateBackend();
        Assert.Null(Assert.IsAssignableFrom<IInputTextBackend>(editorBackend).TryBeginTextInput());
        using var other = new Window(8);
        Assert.Throws<ArgumentException>(() => player.SetTextInputTarget(other, default));
        Assert.True(session.isActive);
    }

    [Fact]
    public void NativeTextOwnershipRejectsTheWrongThread()
    {
        using var window = new Window(7);
        using var source = new EventInputSource(7);
        source.SetTextInputTarget(window, default);
        using IInputBackend backend = source.CreateBackend();
        var text = Assert.IsAssignableFrom<IInputTextBackend>(backend);
        Exception? failure = null;
        var thread = new Thread(() => failure = Record.Exception(() => text.TryBeginTextInput()));
        thread.Start();
        Assert.True(thread.Join(TimeSpan.FromSeconds(5)));
        Assert.IsType<InvalidOperationException>(failure);
        Assert.Equal(0, window.acquisitions);
        using IInputTextSession session = Assert.IsAssignableFrom<IInputTextSession>(text.TryBeginTextInput());
        Assert.True(session.isActive);
    }

    private sealed class Window(uint id) : IPlatformTextInputWindow
    {
        private Session? m_current;
        internal Rect area;
        internal float cursor;
        internal int acquisitions;
        internal int releases;
        internal bool rejectRetirement;
        public uint windowId => id;
        public string title => "Text routing";
        public int width => 640;
        public int height => 480;
        public int pixelWidth => 1280;
        public int pixelHeight => 960;
        public bool isClosed { get; private set; }
        public bool isFocused => !isClosed;
        public void RequestClose() => isClosed = true;
        public void Dispose() => isClosed = true;

        public IPlatformTextInputSession? TryBeginTextInput()
        {
            if (m_current is { isActive: true } || isClosed)
                return null;
            acquisitions++;
            return m_current = new Session(this);
        }

        private sealed class Session(Window owner) : IPlatformTextInputSession
        {
            private bool m_disposed;
            public bool isActive => !m_disposed && ReferenceEquals(owner.m_current, this) && !owner.isClosed;
            public void SetArea(
                Rect area,
                float cursorOffset
            ) {
                Assert.True(isActive);
                owner.area = area;
                owner.cursor = cursorOffset;
            }

            public void Dispose()
            {
                if (m_disposed)
                    return;
                if (owner.rejectRetirement)
                    throw new InvalidOperationException("Injected platform retirement failure.");
                m_disposed = true;
                owner.releases++;
                owner.m_current = null;
            }
        }
    }
}
