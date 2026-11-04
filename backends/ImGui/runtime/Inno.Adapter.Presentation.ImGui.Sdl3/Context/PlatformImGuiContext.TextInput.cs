using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;

using Inno.Adapter.Platform.Sdl3;
using Inno.Core.Mathematics;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Converts public ImGui input-method requests into the shared platform text-input lease.
/// </summary>
public sealed unsafe partial class PlatformImGuiContext
{
    private uint m_imeWindowId;
    private Rect? m_imeArea;
    private Rect? m_appliedImeArea;
    private ExceptionDispatchInfo? m_textInputFailure;
    private ulong m_textInputStartedAt;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetImeDataCallback(
        ImGuiNativeContext* context,
        ImGuiViewport* viewport,
        ImGuiPlatformImeData* data
    ) {
        PlatformImGuiContext? owner = FindPlatformContext(context);
        if (owner is null)
            return;
        try
        {
            owner.m_imeWindowId = 0;
            owner.m_imeArea = null;
            if (viewport != null && data != null && (data->WantVisible != 0 || data->WantTextInput != 0)
                && owner.TryGetWindowId(viewport->ID, out uint windowId))
            {
                owner.m_imeWindowId = windowId;
                if (data->WantVisible != 0)
                    owner.m_imeArea = new Rect(data->InputPos.X - viewport->Pos.X,
                        data->InputPos.Y - viewport->Pos.Y, 1, data->InputLineHeight);
            }
            owner.ApplyTextInputRequest();
        }
        catch (Exception failure)
        {
            // Preserve diagnostics for the managed frame boundary; never unwind through native code.
            owner.m_textInputFailure ??= ExceptionDispatchInfo.Capture(failure);
        }
    }

    private void UpdateTextInputState()
    {
        m_textInputFailure?.Throw();
        ApplyTextInputRequest();
    }

    private void ApplyTextInputRequest()
    {
        Sdl3PlatformWindow? targetWindow = m_imeWindowId == m_window.windowId
            ? m_window : m_viewports?.FindOwnedWindow(m_imeWindowId);
        if (targetWindow is not { isFocused: true, isClosed: false })
            targetWindow = null;
        if (m_textInputSession is not null && (m_textInputWindow != targetWindow || !m_textInputSession.isActive))
        {
            m_textInputSession.Dispose();
            m_textInputSession = null;
            m_textInputWindow = null;
            m_appliedImeArea = null;
        }
        if (targetWindow is not null && m_textInputSession is null)
        {
            m_textInputSession = targetWindow.TryBeginTextInput();
            if (m_textInputSession is not null)
            {
                m_textInputWindow = targetWindow;
                m_textInputStartedAt = SDL.Raw.GetTicksNs();
            }
        }
        if (m_textInputSession is not null && m_imeArea is Rect area && m_appliedImeArea != area)
        {
            m_textInputSession.SetArea(area, 0);
            m_appliedImeArea = area;
        }
    }
}
