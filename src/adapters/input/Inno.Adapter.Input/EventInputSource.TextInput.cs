using System;

using Inno.Core.Events;
using Inno.Core.Mathematics;
using Inno.Input;
using Inno.Platform;

namespace Inno.Adapter.Input;

/// <summary>
/// Routes backend-neutral platform events through Core Events to isolated runtime-session input backends.
/// </summary>
public sealed partial class EventInputSource
{
    private readonly int m_textOwnerThread = Environment.CurrentManagedThreadId;
    private IPlatformWindow? m_textWindow;
    private Vector2 m_textOrigin;
    private TextSession? m_textSession;

    /// <inheritdoc />
    public void SetTextInputTarget(
        IPlatformWindow? window,
        Vector2 origin
    ) {
        RequireTextThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!float.IsFinite(origin.x) || !float.IsFinite(origin.y))
            throw new ArgumentOutOfRangeException(nameof(origin));
        if (window is not null && m_windowId != 0 && window.windowId != m_windowId)
            throw new ArgumentException("The text target must belong to this input source's window.", nameof(window));
        if (!ReferenceEquals(m_textWindow, window))
            RetireTextInput();
        m_textWindow = window;
        m_textOrigin = origin;
    }

    private IInputTextSession? BeginTextInput()
    {
        RequireTextThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_textSession is { isActive: true })
            return null;
        RetireTextInput();
        if (!m_ownership.acceptsInput || m_textWindow is not IPlatformTextInputWindow window ||
            window.isClosed || !window.isFocused)
            return null;
        IPlatformTextInputSession? native = window.TryBeginTextInput();
        if (native is null)
            return null;
        return m_textSession = new TextSession(this, native);
    }

    private void RetireTextInputFor(Event evnt)
    {
        if (m_textWindow is null)
            return;
        if (evnt is ApplicationSuspensionChangedEvent { isSuspended: true } ||
            evnt is WindowEvent window && window.windowId == m_textWindow.windowId &&
            evnt is WindowCloseEvent or WindowFocusChangedEvent { isFocused: false } or
                WindowVisibilityChangedEvent { isVisible: false })
            RetireTextInput();
    }

    private void RetireTextInput()
    {
        RequireTextThread();
        m_textSession?.Dispose();
    }

    private void RequireTextThread()
    {
        if (Environment.CurrentManagedThreadId != m_textOwnerThread)
            throw new InvalidOperationException("Text input routing requires the input source owner thread.");
    }

    private sealed class TextSession(
        EventInputSource owner,
        IPlatformTextInputSession native
    ) : IInputTextSession
    {
        private bool m_disposed;

        /// <inheritdoc />
        public bool isActive
        {
            get
            {
                owner.RequireTextThread();
                return !m_disposed && ReferenceEquals(owner.m_textSession, this) &&
                    owner.m_textWindow is { isClosed: false, isFocused: true } && native.isActive;
            }
        }

        /// <inheritdoc />
        public void SetArea(
            Rect area,
            float cursorOffset
        ) {
            owner.RequireTextThread();
            if (!isActive)
                throw new InvalidOperationException("The runtime text-input session is no longer active.");
            if (!float.IsFinite(area.x) || !float.IsFinite(area.y) || !float.IsFinite(area.width) ||
                !float.IsFinite(area.height) || area.width <= 0 || area.height <= 0 ||
                !float.IsFinite(cursorOffset) || cursorOffset < 0 || cursorOffset > area.width)
                throw new ArgumentOutOfRangeException(nameof(area));
            float x = area.x + owner.m_textOrigin.x;
            float y = area.y + owner.m_textOrigin.y;
            if (!float.IsFinite(x) || !float.IsFinite(y))
                throw new ArgumentOutOfRangeException(nameof(area));
            native.SetArea(new Rect(x, y, area.width, area.height), cursorOffset);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            owner.RequireTextThread();
            if (m_disposed)
                return;
            native.Dispose();
            m_disposed = true;
            if (ReferenceEquals(owner.m_textSession, this))
                owner.m_textSession = null;
        }
    }
}
