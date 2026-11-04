using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed unsafe class TextInputSessionTests
{
    [Fact]
    public void ExclusiveOwnershipRetiresOnFocusVisibilityAndSuspension()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(TextInputSessionTests) + "." + nameof(ExclusiveOwnershipRetiresOnFocusVisibilityAndSuspension)))
            return;
        using var app = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Text input ownership", width = 40, height = 40, visible = false });
        Drain(app);
        var input = Assert.IsAssignableFrom<IPlatformTextInputWindow>(window);
        Assert.Null(input.TryBeginTextInput());
        PushWindow(window.windowId, SDLEventType.WindowShown);
        PushWindow(window.windowId, SDLEventType.WindowFocusGained);
        Drain(app);
        using IPlatformTextInputSession first = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing input lease.");
        Assert.True(first.isActive);
        Assert.Null(input.TryBeginTextInput());
        first.SetArea(new Rect(1.25f, 2.5f, 10.5f, 14.25f), 3.5f);
        SDLRect area = default;
        int cursor = 0;
        Assert.NotEqual(0, SDL.Raw.GetTextInputArea(SDL.Raw.GetWindowFromId(window.windowId), &area, &cursor));
        Assert.Equal((1, 2, 11, 15, 4), (area.X, area.Y, area.W, area.H, cursor));
        Assert.Throws<ArgumentOutOfRangeException>(() => first.SetArea(new Rect(0, 0, float.NaN, 1), 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => first.SetArea(new Rect(0, 0, 1, 1), 2));
        Assert.Throws<InvalidOperationException>(() => Task.Run(() => first.SetArea(new Rect(0, 0, 1, 1), 0)).GetAwaiter().GetResult());
        PushWindow(window.windowId, SDLEventType.WindowFocusLost);
        Drain(app);
        Assert.False(first.isActive);
        Assert.Throws<InvalidOperationException>(() => first.SetArea(new Rect(0, 0, 1, 1), 0));
        Assert.Null(input.TryBeginTextInput());
        PushWindow(window.windowId, SDLEventType.WindowFocusGained);
        Drain(app);
        using IPlatformTextInputSession second = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing replacement lease.");
        first.Dispose();
        Assert.True(second.isActive);
        PushWindow(window.windowId, SDLEventType.WindowHidden);
        Drain(app);
        Assert.False(second.isActive);
        Assert.Null(input.TryBeginTextInput());
        PushWindow(window.windowId, SDLEventType.WindowShown);
        Drain(app);
        using IPlatformTextInputSession third = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing visible lease.");
        var background = new SDLEvent { Type = (uint)SDLEventType.WillEnterBackground };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&background));
        Drain(app);
        Assert.False(third.isActive);
        Assert.Null(input.TryBeginTextInput());
        var foreground = new SDLEvent { Type = (uint)SDLEventType.DidEnterForeground };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&foreground));
        Drain(app);
        using IPlatformTextInputSession fourth = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing foreground lease.");
        window.RequestClose();
        Assert.False(fourth.isActive);
        Assert.Null(input.TryBeginTextInput());
    }

    [Fact]
    public void ReplacementRejectsPreviouslyQueuedNativeAndTranslatedText()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(TextInputSessionTests) + "." + nameof(ReplacementRejectsPreviouslyQueuedNativeAndTranslatedText)))
            return;
        using var app = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Text input generations", width = 40, height = 40, visible = false });
        Drain(app);
        var input = Assert.IsAssignableFrom<IPlatformTextInputWindow>(window);
        PushWindow(window.windowId, SDLEventType.WindowShown);
        PushWindow(window.windowId, SDLEventType.WindowFocusGained);
        Drain(app);
        using IPlatformTextInputSession first = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing lease.");
        nint buffer = Marshal.StringToCoTaskMemUTF8("old");
        try
        {
            PushText(window.windowId, buffer);
            first.Dispose();
            using IPlatformTextInputSession second = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing replacement.");
            Drain(app, rejectText: true);
            // Poll translates the complete batch before returning its first event.
            var key = new SDLEvent { Key = new SDLKeyboardEvent { Type = SDLEventType.KeyDown, WindowID = window.windowId } };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&key));
            PushText(window.windowId, buffer);
            Assert.True(app.PollEvent(out Event? firstEvent));
            Assert.IsNotType<TextInputEvent>(firstEvent);
            second.Dispose();
            using IPlatformTextInputSession third = input.TryBeginTextInput() ?? throw new InvalidOperationException("Missing final lease.");
            Drain(app, rejectText: true);
            PushText(window.windowId, buffer);
            bool received = false;
            while (app.PollEvent(out Event? evnt))
                received |= evnt is TextInputEvent;
            Assert.True(received);
            var editing = new SDLEvent { Edit = new SDLTextEditingEvent
            { Type = SDLEventType.TextEditing, WindowId = window.windowId, Text = (sbyte*)buffer, Start = 0, Length = 3 } };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&editing));
            TextCompositionEvent? started = null;
            while (app.PollEvent(out Event? evnt))
                if (evnt is TextCompositionEvent composition)
                    started = composition;
            Assert.NotNull(started);
            Assert.Equal(TextCompositionPhase.Started, started.change.phase);
            Assert.NotEqual(0, SDL.Raw.PushEvent(&key));
            PushText(window.windowId, buffer);
            Assert.True(app.PollEvent(out Event? preceding));
            Assert.IsNotType<TextCompositionEvent>(preceding);
            third.Dispose();
            TextCompositionEvent? cancelled = null;
            while (app.PollEvent(out Event? evnt))
            {
                Assert.IsNotType<TextInputEvent>(evnt);
                if (evnt is TextCompositionEvent composition)
                {
                    Assert.Equal(TextCompositionPhase.Cancelled, composition.change.phase);
                    cancelled = composition;
                }
            }
            Assert.NotNull(cancelled);
            Assert.Equal(started.change.state.compositionId, cancelled.change.state.compositionId);
        }
        finally
        {
            try { Drain(app); }
            finally { Marshal.FreeCoTaskMem(buffer); }
        }
    }

    private static void PushText(
        uint windowId,
        nint text
    ) {
        var evnt = new SDLEvent { Text = new SDLTextInputEvent
        { Type = SDLEventType.TextInput, WindowID = windowId, Text = (sbyte*)text } };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
    }

    private static void PushWindow(
        uint windowId,
        SDLEventType type
    ) {
        var evnt = new SDLEvent { Window = new SDLWindowEvent { Type = type, WindowID = windowId } };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
    }

    private static void Drain(
        Sdl3PlatformApplication app,
        bool rejectText = false
    ) {
        while (app.PollEvent(out Event? evnt))
            if (rejectText)
                Assert.False(evnt is TextInputEvent or TextCompositionEvent);
    }
}
