using System;
using System.Runtime.InteropServices;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.PlayMode.Tests;

internal static unsafe class NativePanelInput
{
    internal static void Focus(
        NativePanelHost host,
        EditorPanelExtension panel,
        string windowFragment
    ) {
        host.DrawPanel(panel, () =>
        {
            ImGuiNativeContext* context = NativeImGui.Raw.GetCurrentContext();
            for (int index = 0; index < context->Windows.Size; index++)
            {
                ImGuiWindow* window = context->Windows.Data[index];
                string? name = Marshal.PtrToStringUTF8((nint)window->Name);
                if (window->Active != 0 && name?.Contains(windowFragment, StringComparison.Ordinal) == true)
                {
                    NativeImGui.Raw.SetWindowFocus(window->Name);
                    return;
                }
            }
            throw new InvalidOperationException("The actual input owner window is absent: " + windowFragment);
        });
    }

    internal static void Key(
        NativePanelHost host,
        EditorPanelExtension panel,
        ImGuiKey key,
        bool shift = false,
        bool primary = false
    ) {
        ImGuiIO* io = NativeImGui.Raw.GetIO();
        if (shift)
            NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModShift, 1);
        if (primary)
            NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 1);
        NativeImGui.Raw.AddKeyEvent(io, key, 1);
        host.DrawPanel(panel);
        NativeImGui.Raw.AddKeyEvent(io, key, 0);
        if (shift)
            NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModShift, 0);
        if (primary)
            NativeImGui.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, 0);
        host.DrawPanel(panel);
    }
}
