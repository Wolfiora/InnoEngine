using System.Numerics;
using System.Runtime.InteropServices;

using Inno.Native.ImGui;
using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal static unsafe class ImGuiPlatformIoNative
{
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

    internal static void SetPlatformGetWindowPos(
        ImGuiPlatformIO* platformIo,
        PlatformGetWindowPosCallback callback
    ) {
        var callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        var setterArg = (delegate* unmanaged[Cdecl]<ImGuiViewport*, ImVec2C*, void>)callbackPtr;
        ImGuiNative.Raw.PlatformIOSetPlatformGetWindowPos(platformIo, setterArg);
    }

    internal static void SetPlatformGetWindowSize(
        ImGuiPlatformIO* platformIo,
        PlatformGetWindowSizeCallback callback
    ) {
        var callbackPtr = Marshal.GetFunctionPointerForDelegate(callback);
        var setterArg = (delegate* unmanaged[Cdecl]<ImGuiViewport*, ImVec2C*, void>)callbackPtr;
        ImGuiNative.Raw.PlatformIOSetPlatformGetWindowSize(platformIo, setterArg);
    }
}
