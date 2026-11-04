using System;

using Inno.Core.Mathematics;
using Inno.Native.Sdl3;
using Inno.Platform;

namespace Inno.Adapter.Platform.Sdl3;

/// <summary>
/// Keeps system text input under an explicit window-owned lease shared by presentation clients.
/// </summary>
public sealed unsafe partial class Sdl3PlatformWindow
{
    private TextInputSession? m_textInputSession;
    private ulong m_textInputStartedAt;
    private bool m_textInputSuspended;

    /// <inheritdoc />
    public IPlatformTextInputSession? TryBeginTextInput()
    {
        ValidateTextInputThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!m_isFocused || !isVisible || m_isClosed || m_textInputSuspended || m_textInputSession is not null)
            return null;
        if (SDL.Raw.StartTextInputWithProperties(m_window, 0) == 0)
            throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "System text input could not be started.");
        m_textInputStartedAt = SDL.Raw.GetTicksNs();
        return m_textInputSession = new TextInputSession(this);
    }

    internal bool AcceptsTextInput(ulong timestamp)
        => m_textInputSession is { isActive: true } && timestamp >= m_textInputStartedAt;

    internal void SuspendTextInput(bool suspended)
    {
        if (suspended)
            EndTextInput();
        m_textInputSuspended = suspended;
    }

    private void EndTextInput()
    {
        if (m_textInputSession is null)
            return;
        if (SDL.Raw.StopTextInput(m_window) == 0)
            throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "System text input could not be stopped.");
        m_textInputSession.Retire();
        m_textInputSession = null;
        m_onTextInputStopped(this);
    }

    private void SetTextArea(
        Rect area,
        float cursorOffset
    ) {
        ValidateTextArea(area, cursorOffset);
        int x = checked((int)Math.Floor(area.x));
        int y = checked((int)Math.Floor(area.y));
        int right = checked((int)Math.Ceiling((double)area.x + area.width));
        int bottom = checked((int)Math.Ceiling((double)area.y + area.height));
        var rectangle = new SDLRect { X = x, Y = y, W = checked(right - x), H = checked(bottom - y) };
        int cursor = checked((int)Math.Round((double)cursorOffset + area.x - x));
        if (SDL.Raw.SetTextInputArea(m_window, &rectangle, cursor) == 0)
            throw new InvalidOperationException(Sdl3NativeText.ReadError() ?? "System text input placement failed.");
    }

    private void ValidateTextInputThread()
    {
        if (Environment.CurrentManagedThreadId != m_ownerThreadId)
            throw new InvalidOperationException("Text input operations must run on the window owner thread.");
    }

    private static void ValidateTextArea(
        Rect area,
        float cursorOffset
    ) {
        if (!float.IsFinite(area.x) || !float.IsFinite(area.y) || !float.IsFinite(area.width)
            || !float.IsFinite(area.height) || area.width <= 0 || area.height <= 0
            || area.x < int.MinValue || area.y < int.MinValue
            || (double)area.x + area.width >= int.MaxValue || (double)area.y + area.height >= int.MaxValue
            || Math.Ceiling((double)area.x + area.width) - Math.Floor(area.x) > int.MaxValue
            || Math.Ceiling((double)area.y + area.height) - Math.Floor(area.y) > int.MaxValue)
            throw new ArgumentOutOfRangeException(nameof(area));
        if (!float.IsFinite(cursorOffset) || cursorOffset < 0 || cursorOffset > area.width)
            throw new ArgumentOutOfRangeException(nameof(cursorOffset));
    }

    private sealed class TextInputSession(Sdl3PlatformWindow owner) : IPlatformTextInputSession
    {
        private Sdl3PlatformWindow? m_owner = owner;

        /// <inheritdoc />
        public bool isActive => m_owner is { m_disposed: false, m_isClosed: false, m_isFocused: true,
            isVisible: true, m_textInputSuspended: false };

        /// <inheritdoc />
        public void SetArea(
            Rect area,
            float cursorOffset
        ) {
            Sdl3PlatformWindow owner = m_owner ?? throw new InvalidOperationException("The text input session has retired.");
            owner.ValidateTextInputThread();
            if (!isActive)
                throw new InvalidOperationException("The text input session is not active.");
            owner.SetTextArea(area, cursorOffset);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (m_owner is not Sdl3PlatformWindow owner)
                return;
            owner.ValidateTextInputThread();
            owner.EndTextInput();
        }

        internal void Retire() => m_owner = null;
    }
}
