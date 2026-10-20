using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;
using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal sealed unsafe partial class PlatformImGuiViewportBackend
{
    private static void RefreshMonitors(SDLWindow* fallbackWindow)
    {
        var monitors = new List<ImGuiPlatformMonitor>();
        int displayCount = 0;
        uint* displays = SDL.Raw.GetDisplays(&displayCount);
        try
        {
            uint primary = SDL.GetPrimaryDisplay();
            if (displays != null && displayCount > 0)
            {
                if (primary != 0)
                    AddDisplayMonitor(primary, monitors);
                for (int index = 0; index < displayCount; index++)
                {
                    uint display = displays[index];
                    if (display != 0 && display != primary)
                        AddDisplayMonitor(display, monitors);
                }
            }
            if (monitors.Count == 0 && fallbackWindow != null)
            {
                uint fallback = SDL.Raw.GetDisplayForWindow(fallbackWindow);
                if (fallback != 0)
                    AddDisplayMonitor(fallback, monitors);
            }
        }
        finally
        {
            SDL.Raw.Free(displays);
        }

        ImGuiPlatformIO* io = ImGuiNative.Raw.GetPlatformIO();
        ref ImVectorImGuiPlatformMonitor storage = ref io->Monitors;
        if (monitors.Count > storage.Capacity)
        {
            uint bytes = checked((uint)monitors.Count * (uint)sizeof(ImGuiPlatformMonitor));
            var replacement = (ImGuiPlatformMonitor*)ImGuiNative.Raw.MemAlloc(bytes);
            if (replacement == null)
                throw new OutOfMemoryException("Cannot allocate the ImGui monitor snapshot.");
            CollectionsMarshal.AsSpan(monitors).CopyTo(new Span<ImGuiPlatformMonitor>(replacement, monitors.Count));
            ImGuiPlatformMonitor* previous = storage.Data;
            storage.Data = replacement;
            storage.Capacity = monitors.Count;
            storage.Size = monitors.Count;
            // The context's native vector destructor uses the same ImGui allocator for this transferred storage.
            ImGuiNative.Raw.MemFree(previous);
        }
        else
        {
            CollectionsMarshal.AsSpan(monitors).CopyTo(new Span<ImGuiPlatformMonitor>(storage.Data, monitors.Count));
            storage.Size = monitors.Count;
        }
    }

    private static void AddDisplayMonitor(
        uint displayId,
        List<ImGuiPlatformMonitor> monitors
    ) {
        SDLRect main = default;
        if (SDL.Raw.GetDisplayBounds(displayId, &main) == 0)
            return;
        SDLRect work = main;
        _ = SDL.Raw.GetDisplayUsableBounds(displayId, &work);
        float scale = SDL.GetDisplayContentScale(displayId);
        monitors.Add(new ImGuiPlatformMonitor
        {
            MainPos = new ImVec2C { X = main.X, Y = main.Y },
            MainSize = new ImVec2C { X = main.W, Y = main.H },
            WorkPos = new ImVec2C { X = work.X, Y = work.Y },
            WorkSize = new ImVec2C { X = work.W, Y = work.H },
            DpiScale = scale <= 0f ? 1f : scale,
            PlatformHandle = (void*)(nuint)displayId
        });
    }
}
