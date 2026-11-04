using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Translates window-owned input-method transitions without exposing native text buffers.
/// </summary>
public sealed unsafe partial class Sdl3PlatformApplication
{
    private readonly Dictionary<uint, TextCompositionState> m_compositions = [];
    private readonly HashSet<uint> m_pendingCompositionEnds = [];
    private readonly HashSet<Event> m_retiredTextEvents = new(ReferenceEqualityComparer.Instance);
    private ulong m_nextCompositionId;
    private bool m_textInputSuspended;

    private bool TryTranslateComposition(
        ref SDLTextEditingEvent native,
        out Event? evnt
    ) {
        evnt = null;
        uint windowId = native.WindowId;
        if (!AcceptsTextInput(windowId, native.Timestamp))
            return false;
        string text = native.Text == null ? string.Empty : Marshal.PtrToStringUTF8((nint)native.Text) ?? string.Empty;
        bool active = m_compositions.TryGetValue(windowId, out TextCompositionState? previous);
        if (text.Length == 0)
        {
            if (!active)
                return false;
            // SDL emits an empty editing event before committed text on supported desktop backends.
            // Preserve its ordered update; only classify it as cancellation after this pump batch ends.
            m_pendingCompositionEnds.Add(windowId);
        }
        else
            m_pendingCompositionEnds.Remove(windowId);
        ulong id = active ? previous!.compositionId : checked(++m_nextCompositionId);
        int start = native.Start < 0 || native.Length < 0 ? -1 : native.Start;
        int length = start < 0 ? 0 : native.Length;
        var state = new TextCompositionState(id, text, start, length);
        m_compositions[windowId] = state;
        evnt = new TextCompositionEvent(windowId, new(
            active ? TextCompositionPhase.Updated : TextCompositionPhase.Started, state));
        return true;
    }

    private bool TryTranslateText(
        ref SDLTextInputEvent native,
        out Event? evnt
    ) {
        evnt = null;
        uint windowId = native.WindowID;
        if (!AcceptsTextInput(windowId, native.Timestamp))
            return false;
        string? text = native.Text == null ? null : Marshal.PtrToStringUTF8((nint)native.Text);
        if (string.IsNullOrEmpty(text))
            return false;
        m_pendingCompositionEnds.Remove(windowId);
        evnt = m_compositions.Remove(windowId, out TextCompositionState? previous)
            ? new TextCompositionEvent(windowId, new(TextCompositionPhase.Committed, new(previous.compositionId, text)))
            : new TextInputEvent(windowId, text);
        return true;
    }

    private bool AcceptsTextInput(
        uint windowId,
        ulong timestamp
    ) => m_windows.TryGetValue(windowId, out Sdl3PlatformWindow? window) && window.AcceptsTextInput(timestamp);

    private void StopWindowTextInput(Sdl3PlatformWindow window)
    {
        List<TextCompositionState> cancelled = [];
        for (int index = m_pendingEvents.Count - 1; index >= m_pendingEventReadIndex; index--)
        {
            if (m_pendingEvents[index] is TextInputEvent text && text.windowId == window.windowId)
                m_retiredTextEvents.Add(m_pendingEvents[index]);
            else if (m_pendingEvents[index] is TextCompositionEvent composition && composition.windowId == window.windowId
                && m_retiredTextEvents.Add(composition)
                && composition.change.phase is TextCompositionPhase.Committed or TextCompositionPhase.Cancelled)
                cancelled.Add(composition.change.state);
        }
        if (m_compositions.Remove(window.windowId, out TextCompositionState? state))
            cancelled.Add(state);
        // A translated terminal event may not yet have reached the input owner. Retire that owner's
        // composition as well, without delivering the old commit into the replacement session.
        foreach (TextCompositionState pending in cancelled)
            m_pendingEvents.Add(new TextCompositionEvent(window.windowId, new(TextCompositionPhase.Cancelled, pending)));
        m_pendingCompositionEnds.Remove(window.windowId);
    }

    private void FinishCompositionBatch()
    {
        foreach (uint windowId in m_pendingCompositionEnds)
            if (m_compositions.Remove(windowId, out TextCompositionState? state))
                m_pendingEvents.Add(new TextCompositionEvent(windowId, new(TextCompositionPhase.Cancelled, state)));
        m_pendingCompositionEnds.Clear();
    }

    private void RetireComposition(uint windowId)
    {
        m_compositions.Remove(windowId);
        m_pendingCompositionEnds.Remove(windowId);
    }
}
