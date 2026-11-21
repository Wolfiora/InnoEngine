using System;
using System.Runtime.InteropServices;
using Inno.Core.Execution;
using Inno.Native.ImGui;

using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal static unsafe class ImGuiPlatformIoNative
{
    internal static void* CreateWindowCallbacks(
        ImGuiNativeContext* context,
        delegate* unmanaged[Cdecl]<ImGuiViewport*, float, float, void> setPosition,
        delegate* unmanaged[Cdecl]<ImGuiViewport*, float, float, void> setSize
    ) {
        void* registration = ImGuiNative.Raw.InnoCreatePlatformWindowCallbacks(context, setPosition, setSize);
        if (registration is null)
        {
            throw new InvalidOperationException("The current ImGui context cannot own platform window callbacks.");
        }
        return registration;
    }

    internal static void DestroyWindowCallbacks(void* registration)
    {
        if (registration is null)
        {
            return;
        }
        if (ImGuiNative.Raw.InnoCanDestroyPlatformWindowCallbacks(registration) == 0)
        {
            throw new RetirementPendingException("Platform window callbacks still require their current context's idle owner thread.");
        }
        ImGuiNative.Raw.InnoDestroyPlatformWindowCallbacks(registration);
    }

    internal static void SetPlatformGetWindowPos(
        ImGuiPlatformIO* platformIo,
        PlatformGetWindowPosCallback callback
    ) {
        nint callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        delegate* unmanaged[Cdecl]<ImGuiViewport*, ImVec2C*, void> setterArg =
            (delegate* unmanaged[Cdecl]<ImGuiViewport*, ImVec2C*, void>)callbackPtr;
        ImGuiNative.Raw.PlatformIOSetPlatformGetWindowPos(platformIo, setterArg);
    }

    internal static void SetPlatformGetWindowSize(
        ImGuiPlatformIO* platformIo,
        PlatformGetWindowSizeCallback callback
    ) {
        nint callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        delegate* unmanaged[Cdecl]<ImGuiViewport*, ImVec2C*, void> setterArg =
            (delegate* unmanaged[Cdecl]<ImGuiViewport*, ImVec2C*, void>)callbackPtr;
        ImGuiNative.Raw.PlatformIOSetPlatformGetWindowSize(platformIo, setterArg);
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal unsafe delegate void PlatformGetWindowPosCallback(
        ImGuiViewport* viewport,
        ImVec2C* outPos
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal unsafe delegate void PlatformGetWindowSizeCallback(
        ImGuiViewport* viewport,
        ImVec2C* outSize
    );
}
