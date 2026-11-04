using Inno.Core.Events;

namespace Inno.Adapter.Input;

internal sealed class WindowInputOwnership
{
    private bool m_focused = true;
    private bool m_visible = true;
    private bool m_suspended;
    private bool m_closed;

    internal bool acceptsInput => m_focused && m_visible && !m_suspended && !m_closed;

    internal void Initialize(WindowInputOwnership source)
    {
        m_focused = source.m_focused;
        m_visible = source.m_visible;
        m_suspended = source.m_suspended;
        m_closed = source.m_closed;
    }

    internal void Process(
        Event evnt,
        uint windowId
    ) {
        if (evnt is WindowEvent window && windowId != 0 && window.windowId != windowId)
            return;
        switch (evnt)
        {
            case WindowFocusChangedEvent focus:
                m_focused = focus.isFocused;
                if (windowId == 0 && focus.isFocused)
                    m_visible = true;
                break;
            case WindowVisibilityChangedEvent visibility: m_visible = visibility.isVisible; break;
            case ApplicationSuspensionChangedEvent suspension: m_suspended = suspension.isSuspended; break;
            case WindowCloseEvent:
                if (windowId == 0)
                    m_focused = false;
                else
                    m_closed = true;
                break;
        }
    }
}
