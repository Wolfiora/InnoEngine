using System;
using System.Runtime.InteropServices;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Testing.Editor.Native;

/// <summary>
/// Delivers real native focus and keyboard events through a panel fixture's frame boundary.
/// </summary>
public static unsafe class NativePanelInput
{
    /// <summary>
    /// Focuses the actual active native window matching the requested fragment.
    /// </summary>
    /// <param name="host">The current thread's live native host.</param>
    /// <param name="panel">The panel drawn to discover its window.</param>
    /// <param name="windowFragment">The required fragment of the actual native window name.</param>
    /// <exception cref="InvalidOperationException">No active native window matches the fragment.</exception>
    public static void Focus(
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

    /// <summary>
    /// Delivers a complete key press and release through actual native frames.
    /// </summary>
    /// <param name="host">The current thread's live native host.</param>
    /// <param name="panel">The panel receiving input.</param>
    /// <param name="key">The native key to press.</param>
    /// <param name="shift">Whether to hold Shift during the key press.</param>
    /// <param name="primary">Whether to hold the fixture's Control convention during the key press.</param>
    public static void Key(
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
