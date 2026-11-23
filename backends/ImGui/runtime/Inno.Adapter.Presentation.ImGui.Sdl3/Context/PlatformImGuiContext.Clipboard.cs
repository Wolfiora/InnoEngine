using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;

using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Routes clipboard operations through the SDL owner of this ImGui context.
/// </summary>
public sealed unsafe partial class PlatformImGuiContext
{
    private static readonly object CLIPBOARD_CONTEXT_SYNC = new();
    private static readonly Dictionary<nuint, PlatformImGuiContext> CLIPBOARD_CONTEXTS = [];

    private nint m_clipboardText;

    private sbyte* GetClipboardText()
    {
        ReleaseClipboardText();
        if (SDL.Raw.HasClipboardText() == 0)
        {
            return null;
        }
        // SDL transfers this allocation to the caller; retain it until the next request or context shutdown.
        m_clipboardText = (nint)SDL.Raw.GetClipboardText();
        return (sbyte*)m_clipboardText;
    }

    private void RegisterClipboardCallbacks()
    {
        lock (CLIPBOARD_CONTEXT_SYNC)
        {
            CLIPBOARD_CONTEXTS.Add((nuint)m_context, this);
        }
        ImGuiPlatformIO* platformIO = ImGuiNative.Raw.GetPlatformIO();
        platformIO->PlatformGetClipboardTextFn = &PlatformGetClipboardTextCallback;
        platformIO->PlatformSetClipboardTextFn = &PlatformSetClipboardTextCallback;
    }

    private void UnregisterClipboardCallbacks()
    {
        ImGuiPlatformIO* platformIO = ImGuiNative.Raw.GetPlatformIO();
        platformIO->PlatformGetClipboardTextFn = null;
        platformIO->PlatformSetClipboardTextFn = null;
        lock (CLIPBOARD_CONTEXT_SYNC)
        {
            nuint contextKey = (nuint)m_context;
            if (CLIPBOARD_CONTEXTS.GetValueOrDefault(contextKey) == this)
            {
                CLIPBOARD_CONTEXTS.Remove(contextKey);
            }
        }
        ReleaseClipboardText();
    }

    private void ReleaseClipboardText()
    {
        if (m_clipboardText == 0)
        {
            return;
        }
        SDL.Raw.Free((void*)m_clipboardText);
        m_clipboardText = 0;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static sbyte* PlatformGetClipboardTextCallback(ImGuiNativeContext* context)
    {
        try
        {
            PlatformImGuiContext? owner = FindClipboardContext(context);
            return owner is null ? null : owner.GetClipboardText();
        }
        catch (Exception)
        {
            // An exception must not unwind into native code. Null reports that no text is available.
            return null;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetClipboardTextCallback(
        ImGuiNativeContext* context,
        sbyte* text
    ) {
        try
        {
            if (text is not null && FindClipboardContext(context) is not null)
            {
                _ = SDL.Raw.SetClipboardText(text);
            }
        }
        catch (Exception)
        {
            // Exceptions must not cross Dear ImGui's unmanaged callback boundary.
        }
    }

    private static PlatformImGuiContext? FindClipboardContext(ImGuiNativeContext* context)
    {
        if (context is null)
        {
            return null;
        }
        lock (CLIPBOARD_CONTEXT_SYNC)
        {
            return CLIPBOARD_CONTEXTS.GetValueOrDefault((nuint)context);
        }
    }
}
