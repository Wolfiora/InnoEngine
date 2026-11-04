using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Native.Sdl3;
using Inno.Platform;
using Xunit;

namespace Inno.Adapter.Platform.Sdl3.Tests;

public sealed unsafe class TextCompositionTests
{
    [Fact]
    public void NativeEditingCommitAndCancellationRemainOrderedAndWindowOwned()
    {
        if (NativeWindowTestHost.RunIfRequired(nameof(TextCompositionTests) + "." + nameof(NativeEditingCommitAndCancellationRemainOrderedAndWindowOwned)))
            return;
        using var app = new Sdl3PlatformApplication(NativeHostFixture.Create());
        using var window = app.CreateWindow(new PlatformWindowOptions
        { title = "Composition contract", width = 40, height = 40, visible = false });
        Drain(app);
        var buffers = new List<nint>();
        try
        {
            var shown = new SDLEvent { Window = new SDLWindowEvent { Type = SDLEventType.WindowShown, WindowID = window.windowId } };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&shown));
            PushFocus(window.windowId, true);
            Drain(app);
            using IPlatformTextInputSession session = Assert.IsAssignableFrom<IPlatformTextInputWindow>(window).TryBeginTextInput()
                ?? throw new InvalidOperationException("The focused window did not acquire text input.");
            PushEditing(window.windowId, "ni", 0, 2);
            PushEditing(window.windowId, "", 0, 0);
            PushText(window.windowId, "你");
            List<Event> committed = Drain(app);
            TextCompositionChange[] changes = committed.OfType<TextCompositionEvent>().Select(item => item.change).ToArray();
            Assert.Equal(new[] { TextCompositionPhase.Started, TextCompositionPhase.Updated, TextCompositionPhase.Committed },
                changes.Select(item => item.phase));
            Assert.Single(changes.Select(item => item.state.compositionId).Distinct());
            Assert.Equal("你", changes[2].state.text);
            Assert.DoesNotContain(committed, item => item is TextInputEvent);

            PushEditing(window.windowId, "文😀字", 1, 1);
            TextCompositionChange next = Assert.Single(Drain(app).OfType<TextCompositionEvent>()).change;
            Assert.Equal(TextCompositionPhase.Started, next.phase);
            Assert.True(next.state.compositionId > changes[0].state.compositionId);
            Assert.Equal(1, next.state.selectionLength);
            PushEditing(window.windowId, "", 0, 0);
            Assert.Equal(new[] { TextCompositionPhase.Updated, TextCompositionPhase.Cancelled },
                Drain(app).OfType<TextCompositionEvent>().Select(item => item.change.phase));
            PushText(window.windowId, "plain");
            Assert.Equal("plain", Assert.Single(Drain(app).OfType<TextInputEvent>()).text);

            PushEditing(window.windowId, "uncommitted", -1, -1);
            Assert.Equal(-1, Assert.Single(Drain(app).OfType<TextCompositionEvent>()).change.state.selectionStart);
            PushFocus(window.windowId, false);
            PushText(window.windowId, "late");
            PushEditing(window.windowId, "late", 0, 0);
            Assert.DoesNotContain(Drain(app), item => item is TextInputEvent or TextCompositionEvent);
            PushFocus(window.windowId, true);
            PushEditing(uint.MaxValue, "foreign", 0, 0);
            Assert.DoesNotContain(Drain(app), item => item is TextCompositionEvent);
        }
        finally
        {
            // Keep all pushed native buffers alive until the owned event batch has been consumed.
            try { Drain(app); }
            finally
            {
                foreach (nint buffer in buffers)
                    Marshal.FreeCoTaskMem(buffer);
            }
        }

        void PushEditing(
            uint windowId,
            string text,
            int start,
            int length
        ) {
            nint buffer = Marshal.StringToCoTaskMemUTF8(text);
            buffers.Add(buffer);
            var evnt = new SDLEvent
            {
                Edit = new SDLTextEditingEvent
                {
                    Type = SDLEventType.TextEditing, WindowId = windowId,
                    Text = (sbyte*)buffer, Start = start, Length = length
                }
            };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
        }

        void PushText(
            uint windowId,
            string text
        ) {
            nint buffer = Marshal.StringToCoTaskMemUTF8(text);
            buffers.Add(buffer);
            var evnt = new SDLEvent
            {
                Text = new SDLTextInputEvent
                { Type = SDLEventType.TextInput, WindowID = windowId, Text = (sbyte*)buffer }
            };
            Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
        }
    }

    private static List<Event> Drain(Sdl3PlatformApplication app)
    {
        List<Event> events = [];
        while (app.PollEvent(out Event? evnt))
            if (evnt is not null)
                events.Add(evnt);
        return events;
    }

    private static void PushFocus(
        uint windowId,
        bool focused
    ) {
        var evnt = new SDLEvent
        {
            Window = new SDLWindowEvent
            { Type = focused ? SDLEventType.WindowFocusGained : SDLEventType.WindowFocusLost, WindowID = windowId }
        };
        Assert.NotEqual(0, SDL.Raw.PushEvent(&evnt));
    }
}
