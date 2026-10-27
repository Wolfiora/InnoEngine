using System;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

internal sealed unsafe class NativeClipboard : IDisposable
{
    private GCHandle m_handle;
    private nint m_text;
    private ExceptionDispatchInfo? m_failure;

    internal NativeClipboard(ImGuiNativeContext* context)
    {
        m_handle = GCHandle.Alloc(this);
        ImGuiPlatformIO* platform = NativeImGui.Raw.GetPlatformIO(context);
        platform->PlatformClipboardUserData = (void*)GCHandle.ToIntPtr(m_handle);
        platform->PlatformGetClipboardTextFn = &Read;
        platform->PlatformSetClipboardTextFn = &Write;
        m_text = Marshal.StringToCoTaskMemUTF8(string.Empty);
    }

    internal void ThrowIfFaulted() => m_failure?.Throw();

    public void Dispose()
    {
        if (!m_handle.IsAllocated)
            return;
        Marshal.FreeCoTaskMem(m_text);
        m_text = 0;
        m_handle.Free();
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static sbyte* Read(ImGuiNativeContext* context)
    {
        NativeClipboard owner = GetOwner(context);
        return (sbyte*)owner.m_text;
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void Write(
        ImGuiNativeContext* context,
        sbyte* text
    ) {
        NativeClipboard owner = GetOwner(context);
        try
        {
            nint replacement = Marshal.StringToCoTaskMemUTF8(Marshal.PtrToStringUTF8((nint)text) ?? string.Empty);
            Marshal.FreeCoTaskMem(owner.m_text);
            owner.m_text = replacement;
        }
        catch (Exception failure)
        {
            owner.m_failure ??= ExceptionDispatchInfo.Capture(failure);
        }
    }

    private static NativeClipboard GetOwner(ImGuiNativeContext* context)
        => (NativeClipboard)GCHandle.FromIntPtr((nint)NativeImGui.Raw.GetPlatformIO(context)->PlatformClipboardUserData).Target!;
}
