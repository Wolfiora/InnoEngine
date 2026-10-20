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
    private static readonly object S_CLIPBOARD_CONTEXT_SYNC = new();
    private static readonly Dictionary<nuint, PlatformImGuiContext> S_CLIPBOARD_CONTEXTS = [];

    private nint m_clipboardText;

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static sbyte* PlatformGetClipboardTextCallback(ImGuiNativeContext* context)
    {
        try
        {
            PlatformImGuiContext? owner = FindClipboardContext(context);
            return owner is null ? null : owner.GetClipboardText();
        }
        catch
        {
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
            if (text != null && FindClipboardContext(context) is not null)
                _ = SDL.Raw.SetClipboardText(text);
        }
        catch
        {
            // Exceptions must not cross Dear ImGui's unmanaged callback boundary.
        }
    }

    private static PlatformImGuiContext? FindClipboardContext(ImGuiNativeContext* context)
    {
        if (context == null)
            return null;
        lock (S_CLIPBOARD_CONTEXT_SYNC)
            return S_CLIPBOARD_CONTEXTS.GetValueOrDefault((nuint)context);
    }

    private sbyte* GetClipboardText()
    {
        ReleaseClipboardText();
        if (SDL.Raw.HasClipboardText() == 0)
            return null;
        // SDL transfers this allocation to the caller; retain it until the next request or context shutdown.
        m_clipboardText = (nint)SDL.Raw.GetClipboardText();
        return (sbyte*)m_clipboardText;
    }

    private void RegisterClipboardCallbacks()
    {
        lock (S_CLIPBOARD_CONTEXT_SYNC)
            S_CLIPBOARD_CONTEXTS.Add((nuint)m_context, this);
        ImGuiPlatformIO* platformIo = ImGuiNative.Raw.GetPlatformIO();
        platformIo->PlatformGetClipboardTextFn = &PlatformGetClipboardTextCallback;
        platformIo->PlatformSetClipboardTextFn = &PlatformSetClipboardTextCallback;
    }

    private void UnregisterClipboardCallbacks()
    {
        ImGuiPlatformIO* platformIo = ImGuiNative.Raw.GetPlatformIO();
        platformIo->PlatformGetClipboardTextFn = null;
        platformIo->PlatformSetClipboardTextFn = null;
        lock (S_CLIPBOARD_CONTEXT_SYNC)
        {
            nuint contextKey = (nuint)m_context;
            if (S_CLIPBOARD_CONTEXTS.GetValueOrDefault(contextKey) == this)
                S_CLIPBOARD_CONTEXTS.Remove(contextKey);
        }
        ReleaseClipboardText();
    }

    private void ReleaseClipboardText()
    {
        if (m_clipboardText == 0)
            return;
        SDL.Raw.Free((void*)m_clipboardText);
        m_clipboardText = 0;
    }
}
