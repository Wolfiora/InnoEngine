using Inno.Adapter.Platform;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;

using Inno.Adapter.Platform.Sdl3;
using Inno.Platform;
using Inno.Native.Sdl3;
using Inno.Native.ImGui;
using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal sealed unsafe class PlatformImGuiViewportBackend : IDisposable
{
    private sealed class ViewportWindowData
    {
        internal required PlatformImGuiViewportBackend owner;
        internal required Sdl3PlatformWindow registeredWindow;
        internal ImGuiViewportPtr viewport;
        internal bool drawable;
        internal SDLWindow window;
        internal SDLRenderer renderer;
        internal IPlatformImGuiRenderer? externalRenderer;
        internal PlatformImGuiViewportTarget? externalTarget;
        internal SDLTexturePtr fontTexture;
        internal int fontTextureWidth;
        internal int fontTextureHeight;
        internal nint fontPixelsPtr;
        internal int fontTexturePitch;
        internal int fontTextureUniqueId = -1;
        internal ImTextureRect fontUsedRect;
        internal ImTextureRect fontUpdateRect;
        internal uint windowId;
        internal GCHandle gcHandle;
        internal SDLVertex[] vertexScratch = [];
        internal int[] indexScratch = [];
    }

    private static readonly PlatformCreateWindow s_platformCreateWindow = PlatformCreateWindowCallback;
    private static readonly PlatformDestroyWindow s_platformDestroyWindow = PlatformDestroyWindowCallback;
    private static readonly PlatformShowWindow s_platformShowWindow = PlatformShowWindowCallback;
    private static readonly PlatformSetWindowPos s_platformSetWindowPos = PlatformSetWindowPosCallback;
    private static readonly PlatformSetWindowSize s_platformSetWindowSize = PlatformSetWindowSizeCallback;
    private static readonly PlatformSetWindowFocus s_platformSetWindowFocus = PlatformSetWindowFocusCallback;
    private static readonly PlatformGetWindowFocus s_platformGetWindowFocus = PlatformGetWindowFocusCallback;
    private static readonly PlatformGetWindowMinimized s_platformGetWindowMinimized = PlatformGetWindowMinimizedCallback;
    private static readonly PlatformSetWindowTitle s_platformSetWindowTitle = PlatformSetWindowTitleCallback;
    private static readonly PlatformSetWindowAlpha s_platformSetWindowAlpha = PlatformSetWindowAlphaCallback;
    private static readonly RendererRenderWindow s_rendererRenderWindow = RendererRenderWindowCallback;
    private static readonly RendererSwapBuffers s_rendererSwapBuffers = RendererSwapBuffersCallback;
    private static readonly ImGuiPlatformIoNative.PlatformGetWindowPosCallback s_platformGetWindowPos = PlatformGetWindowPosOutCallback;
    private static readonly ImGuiPlatformIoNative.PlatformGetWindowSizeCallback s_platformGetWindowSize = PlatformGetWindowSizeOutCallback;

    private static readonly object S_CONTEXT_SYNC = new();
    private static readonly Dictionary<nuint, PlatformImGuiViewportBackend> S_BACKENDS_BY_CONTEXT = [];

    private readonly Sdl3PlatformApplication m_application;
    private readonly SDLWindow m_mainWindow;
    private readonly IPlatformImGuiRenderer m_renderer;
    private readonly nuint m_contextKey;
    private readonly Dictionary<uint, ViewportWindowData> m_viewportsById = [];
    private readonly Dictionary<uint, uint> m_windowToViewport = [];
    private bool m_monitorsDirty;
    private bool m_disposed;

    internal PlatformImGuiViewportBackend(
        Sdl3PlatformApplication application,
        Sdl3PlatformWindow mainWindow,
        IPlatformImGuiRenderer renderer
    ) {
        ArgumentNullException.ThrowIfNull(renderer);
        ImGuiContextPtr context = ImGuiNative.GetCurrentContext();
        if (context.IsNull)
            throw new InvalidOperationException("An active ImGui context is required for viewport routing.");
        m_contextKey = (nuint)context.Handle;
        lock (S_CONTEXT_SYNC)
        {
            if (S_BACKENDS_BY_CONTEXT.ContainsKey(m_contextKey))
                throw new InvalidOperationException("The current ImGui context already owns a viewport backend.");
        }
        m_application = application;
        m_renderer = renderer;
        _ = SDL.SetHint(SDL.SDL_HINT_WINDOW_ACTIVATE_WHEN_RAISED, "1");
        m_mainWindow = mainWindow.GetSdlWindow();

        var platformIo = ImGuiNative.GetPlatformIO();
        platformIo.PlatformCreateWindow = (delegate* unmanaged[Cdecl]<ImGuiViewport*, void>)FunctionPtr(s_platformCreateWindow);
        platformIo.PlatformDestroyWindow = (delegate* unmanaged[Cdecl]<ImGuiViewport*, void>)FunctionPtr(s_platformDestroyWindow);
        platformIo.PlatformShowWindow = (delegate* unmanaged[Cdecl]<ImGuiViewport*, void>)FunctionPtr(s_platformShowWindow);
        platformIo.PlatformSetWindowPos = (delegate* unmanaged[Cdecl]<ImGuiViewport*, Vector2, void>)FunctionPtr(s_platformSetWindowPos);
        platformIo.PlatformSetWindowSize = (delegate* unmanaged[Cdecl]<ImGuiViewport*, Vector2, void>)FunctionPtr(s_platformSetWindowSize);
        platformIo.PlatformGetWindowFramebufferScale = null;
        platformIo.PlatformSetWindowFocus = (delegate* unmanaged[Cdecl]<ImGuiViewport*, void>)FunctionPtr(s_platformSetWindowFocus);
        platformIo.PlatformGetWindowFocus = (delegate* unmanaged[Cdecl]<ImGuiViewport*, byte>)FunctionPtr(s_platformGetWindowFocus);
        platformIo.PlatformGetWindowMinimized = (delegate* unmanaged[Cdecl]<ImGuiViewport*, byte>)FunctionPtr(s_platformGetWindowMinimized);
        platformIo.PlatformSetWindowTitle = (delegate* unmanaged[Cdecl]<ImGuiViewport*, byte*, void>)FunctionPtr(s_platformSetWindowTitle);
        platformIo.PlatformSetWindowAlpha = (delegate* unmanaged[Cdecl]<ImGuiViewport*, float, void>)FunctionPtr(s_platformSetWindowAlpha);
        platformIo.RendererRenderWindow = (delegate* unmanaged[Cdecl]<ImGuiViewport*, void*, void>)FunctionPtr(s_rendererRenderWindow);
        platformIo.RendererSwapBuffers = (delegate* unmanaged[Cdecl]<ImGuiViewport*, void*, void>)FunctionPtr(s_rendererSwapBuffers);
        ImGuiPlatformIoNative.SetPlatformGetWindowPos(platformIo, s_platformGetWindowPos);
        ImGuiPlatformIoNative.SetPlatformGetWindowSize(platformIo, s_platformGetWindowSize);

        var mainViewport = ImGuiNative.GetMainViewport();
        SDLWindow mainSdlWindow = mainWindow.GetSdlWindow();
        mainViewport.PlatformHandle = (void*)mainSdlWindow.Handle;
        mainViewport.PlatformHandleRaw = (void*)mainWindow.nativeHandles.windowHandle;

        RefreshMonitors(mainSdlWindow);
        lock (S_CONTEXT_SYNC)
            S_BACKENDS_BY_CONTEXT.Add(m_contextKey, this);
    }

    internal bool OwnsWindow(uint windowId)
    {
        return m_windowToViewport.ContainsKey(windowId);
    }

    internal bool TryGetWindowId(
        uint viewportId,
        out uint windowId
    ) {
        if (m_viewportsById.TryGetValue(viewportId, out ViewportWindowData? viewport))
        {
            windowId = viewport.windowId;
            return true;
        }
        windowId = 0;
        return false;
    }

    internal void ProcessEvent(
        ref SDLEvent sdlEvent,
        uint windowId
    ) {
        var eventType = (SDLEventType)sdlEvent.Type;
        if (eventType >= SDLEventType.DisplayFirst && eventType <= SDLEventType.DisplayLast
            || eventType == SDLEventType.WindowDisplayChanged
            || eventType == SDLEventType.WindowDisplayScaleChanged)
        {
            m_monitorsDirty = true;
        }
        if (!m_windowToViewport.TryGetValue(windowId, out uint viewportId)
            || !m_viewportsById.TryGetValue(viewportId, out ViewportWindowData? data))
        {
            return;
        }

        ImGuiViewportPtr viewport = data.viewport;
        if (eventType == SDLEventType.WindowCloseRequested)
            viewport.PlatformRequestClose = true;
        if (eventType == SDLEventType.WindowMoved)
            viewport.PlatformRequestMove = true;
        if (eventType == SDLEventType.WindowResized || eventType == SDLEventType.WindowPixelSizeChanged)
            viewport.PlatformRequestResize = true;
    }

    internal void SynchronizeMetrics()
    {
        if (m_monitorsDirty)
        {
            RefreshMonitors(m_mainWindow);
            m_monitorsDirty = false;
        }
        var main = ImGuiNative.GetMainViewport();
        Vector2 scale = GetWindowFramebufferScale(m_mainWindow);
        if (scale.X > 0 && scale.Y > 0)
        {
            main.FramebufferScale = scale;
            if (main.DpiScale == 0f)
                main.DpiScale = GetWindowDpiScale(m_mainWindow);
        }
        foreach (ViewportWindowData data in m_viewportsById.Values)
            SynchronizeWindow(data.windowId);
    }

    /// <summary>
    /// Synchronizes a secondary viewport's geometry, framebuffer scale, and renderer output.
    /// </summary>
    /// <param name="windowId">
    /// The SDL window identifier owned by the viewport.
    /// </param>
    internal void SynchronizeWindow(uint windowId)
    {
        if (!m_windowToViewport.TryGetValue(windowId, out var viewportId))
        {
            return;
        }

        if (!m_viewportsById.TryGetValue(viewportId, out ViewportWindowData? data))
        {
            return;
        }
        ImGuiViewportPtr viewport = data.viewport;

        var width = 0;
        var height = 0;
        SDL.GetWindowSize(data.window, ref width, ref height);
        if (width <= 0 || height <= 0)
        {
            data.drawable = false;
            return;
        }

        var x = 0;
        var y = 0;
        _ = SDL.GetWindowPosition(data.window, ref x, ref y);

        bool geometryChanged = viewport.Handle->Pos != new Vector2(x, y) || viewport.Handle->Size != new Vector2(width, height);
        viewport.Handle->Pos = new Vector2(x, y);
        viewport.Handle->WorkPos = new Vector2(x, y);
        viewport.Handle->Size = new Vector2(width, height);
        viewport.Handle->WorkSize = new Vector2(width, height);
        Vector2 framebufferScale = GetWindowFramebufferScale(data.window);
        if (framebufferScale.X > 0 && framebufferScale.Y > 0)
        {
            if (viewport.Handle->DpiScale == 0f)
                viewport.Handle->DpiScale = GetWindowDpiScale(data.window);
            viewport.Handle->FramebufferScale = framebufferScale;
        }
        if (geometryChanged)
        {
            viewport.Handle->PlatformRequestMove = 1;
            viewport.Handle->PlatformRequestResize = 1;
        }


        int pixelWidth = 0;
        int pixelHeight = 0;
        SDL.GetWindowSizeInPixels(data.window, ref pixelWidth, ref pixelHeight);
        data.drawable = pixelWidth > 0 && pixelHeight > 0
            && (SDL.GetWindowFlags(data.window) & (SDLWindowFlags.Minimized | SDLWindowFlags.Hidden)) == 0;
        if (data.externalRenderer is not null && data.externalTarget is not null)
        {
            if (data.drawable && (data.externalTarget.width != pixelWidth || data.externalTarget.height != pixelHeight))
            {
                data.externalTarget.width = pixelWidth;
                data.externalTarget.height = pixelHeight;
                data.externalRenderer.ResizeViewport(data.externalTarget);
            }
        }
        else
        {
            SynchronizeRendererOutput(data.renderer);
        }
    }

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }

        foreach (var kv in m_viewportsById)
        {
            DestroyViewportWindow(kv.Value);
        }

        m_viewportsById.Clear();
        m_windowToViewport.Clear();

        var platformIo = ImGuiNative.GetPlatformIO();
        platformIo.ClearPlatformHandlers();
        platformIo.ClearRendererHandlers();

        var mainViewport = ImGuiNative.GetMainViewport();
        if (!mainViewport.IsNull)
        {
            mainViewport.PlatformHandle = null;
            mainViewport.PlatformHandleRaw = null;
        }

        lock (S_CONTEXT_SYNC)
        {
            if (S_BACKENDS_BY_CONTEXT.TryGetValue(m_contextKey, out PlatformImGuiViewportBackend? registered)
                && ReferenceEquals(registered, this))
            {
                S_BACKENDS_BY_CONTEXT.Remove(m_contextKey);
            }
        }
        m_disposed = true;
    }

    private static void PlatformCreateWindowCallback(ImGuiViewport* viewport)
    {
        if (viewport == null || viewport->PlatformUserData != null || (viewport->Flags & ImGuiViewportFlags.OwnedByApp) != 0)
        {
            return;
        }

        var flags = SDLWindowFlags.Hidden | SDLWindowFlags.Resizable | SDLWindowFlags.HighPixelDensity;
        if ((viewport->Flags & ImGuiViewportFlags.NoDecoration) != 0)
        {
            flags |= SDLWindowFlags.Borderless;
        }

        if ((viewport->Flags & ImGuiViewportFlags.NoTaskBarIcon) != 0)
        {
            flags |= SDLWindowFlags.Utility;
        }

        if ((viewport->Flags & ImGuiViewportFlags.TopMost) != 0)
        {
            flags |= SDLWindowFlags.AlwaysOnTop;
        }

        var width = Math.Max(1, (int)viewport->Size.X);
        var height = Math.Max(1, (int)viewport->Size.Y);
        var window = SDL.CreateWindow("ImGui", width, height, flags);
        if (window.IsNull)
        {
            return;
        }

        _ = SDL.SetWindowPosition(window, (int)viewport->Pos.X, (int)viewport->Pos.Y);

        PlatformImGuiViewportBackend backend = GetCurrentBackend();
        IPlatformImGuiRenderer rendererBackend = backend.m_renderer;
        SDLRenderer renderer = SDLRenderer.Null;
        if (!rendererBackend.supportsViewports)
        {
            renderer = SDL.CreateRenderer(window, (byte*)0);
            if (renderer.IsNull)
            {
                SDL.DestroyWindow(window);
                return;
            }

            _ = SDL.SetRenderDrawBlendMode(renderer, SDL.SDL_BLENDMODE_BLEND);
        }

        Sdl3PlatformWindow registeredWindow;
        try
        {
            registeredWindow = backend.m_application.AdoptWindow((nint)window.Handle);
        }
        catch
        {
            if (!renderer.IsNull)
                SDL.DestroyRenderer(renderer);
            SDL.DestroyWindow(window);
            throw;
        }
        var data = new ViewportWindowData
        {
            owner = backend,
            registeredWindow = registeredWindow,
            viewport = new ImGuiViewportPtr(viewport),
            drawable = true,
            window = window,
            renderer = renderer,
            windowId = SDL.GetWindowID(window)
        };
        try
        {
            if (rendererBackend.supportsViewports)
            {
                int pixelWidth = 0;
                int pixelHeight = 0;
                SDL.GetWindowSizeInPixels(window, ref pixelWidth, ref pixelHeight);
                data.externalRenderer = rendererBackend;
                data.externalTarget = new PlatformImGuiViewportTarget(
                    viewport->ID,
                    data.windowId,
                    registeredWindow.nativeHandles,
                    pixelWidth,
                    pixelHeight);
                rendererBackend.CreateViewport(data.externalTarget);
            }
            data.gcHandle = GCHandle.Alloc(data, GCHandleType.Normal);
            backend.m_viewportsById.Add(viewport->ID, data);
            backend.m_windowToViewport.Add(data.windowId, viewport->ID);

            var handlePtr = GCHandle.ToIntPtr(data.gcHandle);
            viewport->PlatformUserData = (void*)handlePtr;
            viewport->RendererUserData = (void*)handlePtr;
            viewport->PlatformHandle = (void*)window.Handle;
            viewport->PlatformHandleRaw = (void*)registeredWindow.nativeHandles.windowHandle;
        }
        catch
        {
            backend.m_viewportsById.Remove(viewport->ID);
            backend.m_windowToViewport.Remove(data.windowId);
            DestroyViewportWindow(data);
            throw;
        }
    }

    private static void PlatformDestroyWindowCallback(ImGuiViewport* viewport)
    {
        if (viewport == null || (viewport->Flags & ImGuiViewportFlags.OwnedByApp) != 0)
        {
            return;
        }

        if (TryGetViewportData(viewport, out var data))
        {
            data.owner.m_viewportsById.Remove(viewport->ID);
            data.owner.m_windowToViewport.Remove(data.windowId);
            DestroyViewportWindow(data);
        }

        viewport->PlatformUserData = null;
        viewport->RendererUserData = null;
        viewport->PlatformHandle = null;
        viewport->PlatformHandleRaw = null;
    }

    private static void PlatformShowWindowCallback(ImGuiViewport* viewport)
    {
        if (!TryGetWindow(viewport, out var window))
        {
            return;
        }

        _ = SDL.ShowWindow(window);
        GetCurrentBackend().SynchronizeWindow(SDL.GetWindowID(window));
    }

    private static void PlatformSetWindowPosCallback(
        ImGuiViewport* viewport,
        Vector2 pos
    ) {
        if (!TryGetWindow(viewport, out var window))
        {
            return;
        }

        _ = SDL.SetWindowPosition(window, (int)pos.X, (int)pos.Y);
    }

    private static void PlatformGetWindowPosOutCallback(
        ImGuiViewport* viewport,
        Vector2* outPos
    ) {
        if (outPos == null)
        {
            return;
        }

        if (!TryGetWindow(viewport, out var window))
        {
            *outPos = Vector2.Zero;
            return;
        }

        var x = 0;
        var y = 0;
        _ = SDL.GetWindowPosition(window, ref x, ref y);
        *outPos = new Vector2(x, y);
    }

    private static void PlatformSetWindowSizeCallback(
        ImGuiViewport* viewport,
        Vector2 size
    ) {
        if (!TryGetWindow(viewport, out var window))
        {
            return;
        }

        _ = SDL.SetWindowSize(window, Math.Max(1, (int)size.X), Math.Max(1, (int)size.Y));
    }

    private static void PlatformGetWindowSizeOutCallback(
        ImGuiViewport* viewport,
        Vector2* outSize
    ) {
        if (outSize == null)
        {
            return;
        }

        if (!TryGetWindow(viewport, out var window))
        {
            *outSize = Vector2.Zero;
            return;
        }

        var width = 0;
        var height = 0;
        SDL.GetWindowSize(window, ref width, ref height);
        *outSize = new Vector2(width, height);
    }

    private static void PlatformSetWindowFocusCallback(ImGuiViewport* viewport)
    {
        if (!TryGetWindow(viewport, out var window))
        {
            return;
        }

        FocusWindow(window);
    }

    private static byte PlatformGetWindowFocusCallback(ImGuiViewport* viewport)
    {
        if (!TryGetWindow(viewport, out var window))
        {
            return 0;
        }

        var flags = (SDLWindowFlags)SDL.GetWindowFlags(window);
        return (flags & SDLWindowFlags.InputFocus) != 0 ? (byte)1 : (byte)0;
    }

    private static byte PlatformGetWindowMinimizedCallback(ImGuiViewport* viewport)
    {
        if (!TryGetWindow(viewport, out var window))
        {
            return 0;
        }

        var flags = (SDLWindowFlags)SDL.GetWindowFlags(window);
        return (flags & SDLWindowFlags.Minimized) != 0 ? (byte)1 : (byte)0;
    }

    private static void PlatformSetWindowTitleCallback(
        ImGuiViewport* viewport,
        byte* title
    ) {
        if (!TryGetWindow(viewport, out var window))
        {
            return;
        }

        _ = SDL.SetWindowTitle(window, title);
    }

    private static void PlatformSetWindowAlphaCallback(
        ImGuiViewport* viewport,
        float alpha
    ) {
        if (!TryGetWindow(viewport, out var window))
        {
            return;
        }

        _ = SDL.SetWindowOpacity(window, alpha);
    }

    private static void RendererRenderWindowCallback(
        ImGuiViewport* viewport,
        void* renderArg
    ) {
        _ = renderArg;
        if (!TryGetViewportData(viewport, out var data) || viewport == null || viewport->DrawData == null)
        {
            return;
        }

        data.owner.SynchronizeWindow(data.windowId);
        if (!data.drawable)
            return;

        if (data.externalRenderer is not null && data.externalTarget is not null)
        {
            data.externalRenderer.RenderViewport(data.externalTarget, (IntPtr)viewport->DrawData);
        }
        else
        {
            EnsureViewportFontTexture(data);
            RenderDrawData(data, data.fontTexture, viewport->DrawData);
        }
    }

    private static void RendererSwapBuffersCallback(
        ImGuiViewport* viewport,
        void* renderArg
    ) {
        _ = renderArg;
        if (!TryGetViewportData(viewport, out var data))
        {
            return;
        }

        data.owner.SynchronizeWindow(data.windowId);
        if (!data.drawable)
            return;

        if (data.externalRenderer is not null && data.externalTarget is not null)
        {
            data.externalRenderer.PresentViewport(data.externalTarget);
        }
        else
        {
            _ = SDL.RenderPresent(data.renderer);
        }
    }

    private static bool TryGetViewportData(
        ImGuiViewport* viewport,
        out ViewportWindowData data
    ) {
        if (viewport == null || viewport->PlatformUserData == null)
        {
            data = null!;
            return false;
        }

        var handle = GCHandle.FromIntPtr((IntPtr)viewport->PlatformUserData);
        if (!handle.IsAllocated || handle.Target is not ViewportWindowData windowData)
        {
            data = null!;
            return false;
        }

        data = windowData;
        return true;
    }

    private static bool TryGetWindow(
        ImGuiViewport* viewport,
        out SDLWindow window
    ) {
        if (viewport != null && viewport->PlatformHandle != null)
        {
            window = new SDLWindow((nint)viewport->PlatformHandle);
            return true;
        }

        if (TryGetViewportData(viewport, out var data))
        {
            window = data.window;
            return true;
        }

        window = SDLWindow.Null;
        return false;
    }

    private static void RefreshMonitors(SDLWindow fallbackWindow)
    {
        var platformIo = ImGuiNative.GetPlatformIO();
        platformIo.Monitors.Clear();

        var displayCount = 0;
        var displays = SDL.GetDisplays(&displayCount);
        try
        {
            var primaryDisplay = SDL.GetPrimaryDisplay();
            if (displays != null && displayCount > 0)
            {
                if (primaryDisplay != 0)
                {
                    AddDisplayMonitor(primaryDisplay, ref platformIo);
                }

                for (var i = 0; i < displayCount; i++)
                {
                    var displayId = displays[i];
                    if (displayId == 0 || displayId == primaryDisplay)
                    {
                        continue;
                    }

                    AddDisplayMonitor(displayId, ref platformIo);
                }
            }

            if (platformIo.Monitors.Size == 0 && !fallbackWindow.IsNull)
            {
                var fallbackDisplay = SDL.GetDisplayForWindow(fallbackWindow);
                if (fallbackDisplay != 0)
                {
                    AddDisplayMonitor(fallbackDisplay, ref platformIo);
                }
            }

        }
        finally
        {
            if (displays != null)
            {
                SDL.Free(displays);
            }
        }
    }

    private static void AddDisplayMonitor(
        uint displayId,
        ref ImGuiPlatformIOPtr platformIo
    ) {
        SDLRect mainBounds = default;
        if (!SDL.GetDisplayBounds(displayId, ref mainBounds))
        {
            return;
        }

        var workBounds = mainBounds;
        _ = SDL.GetDisplayUsableBounds(displayId, ref workBounds);
        var dpiScale = SDL.GetDisplayContentScale(displayId);
        if (dpiScale <= 0f)
        {
            dpiScale = 1f;
        }

        platformIo.Monitors.PushBack(new ImGuiPlatformMonitor(
            mainPos: new Vector2(mainBounds.X, mainBounds.Y),
            mainSize: new Vector2(mainBounds.W, mainBounds.H),
            workPos: new Vector2(workBounds.X, workBounds.Y),
            workSize: new Vector2(workBounds.W, workBounds.H),
            dpiScale: dpiScale,
            platformHandle: (void*)(nuint)displayId));
    }

    private static void DestroyViewportWindow(ViewportWindowData data)
    {
        if (data.externalRenderer is not null && data.externalTarget is not null)
        {
            data.externalRenderer.DestroyViewport(data.externalTarget);
            data.externalRenderer = null;
            data.externalTarget = null;
        }

        if (!data.fontTexture.IsNull)
        {
            SDL.DestroyTexture(data.fontTexture);
            data.fontTexture = SDLTexturePtr.Null;
        }

        if (!data.renderer.IsNull)
        {
            SDL.DestroyRenderer(data.renderer);
            data.renderer = SDLRenderer.Null;
        }

        if (!data.window.IsNull)
        {
            data.owner.m_application.ReleaseWindow(data.registeredWindow);
            SDL.DestroyWindow(data.window);
            data.window = SDLWindow.Null;
        }

        if (data.gcHandle.IsAllocated)
        {
            data.gcHandle.Free();
        }
    }

    private static void EnsureViewportFontTexture(ViewportWindowData data)
    {
        var io = ImGuiNative.GetIO();
        if (io.Fonts.IsNull)
        {
            return;
        }

        io.Fonts.RendererHasTextures = true;
        var texData = io.Fonts.TexData;
        if (texData.IsNull)
        {
            return;
        }

        if (texData.Status == ImTextureStatus.WantDestroy)
        {
            if (!data.fontTexture.IsNull)
            {
                SDL.DestroyTexture(data.fontTexture);
                data.fontTexture = SDLTexturePtr.Null;
                data.fontTextureWidth = 0;
                data.fontTextureHeight = 0;
                data.fontPixelsPtr = 0;
                data.fontTexturePitch = 0;
                data.fontTextureUniqueId = -1;
                data.fontUsedRect = default;
                data.fontUpdateRect = default;
            }

            return;
        }

        if (texData.Pixels == null || texData.Width <= 0 || texData.Height <= 0)
        {
            return;
        }

        var needsRecreate = data.fontTexture.IsNull
            || data.fontTextureWidth != texData.Width
            || data.fontTextureHeight != texData.Height;
        if (needsRecreate)
        {
            if (!data.fontTexture.IsNull)
            {
                SDL.DestroyTexture(data.fontTexture);
                data.fontTexture = SDLTexturePtr.Null;
            }

            data.fontTexture = CreateTexture(data.renderer, texData.Width, texData.Height);
            if (data.fontTexture.IsNull)
            {
                data.fontTextureWidth = 0;
                data.fontTextureHeight = 0;
                return;
            }

            data.fontTextureWidth = texData.Width;
            data.fontTextureHeight = texData.Height;
        }

        var pitch = texData.GetPitch();
        var pixelsPtr = (nint)texData.Pixels;
        var uniqueId = texData.UniqueID;
        var usedRect = texData.UsedRect;
        var updateRect = texData.UpdateRect;
        var needsUpload = needsRecreate
            || data.fontPixelsPtr != pixelsPtr
            || data.fontTexturePitch != pitch
            || data.fontTextureUniqueId != uniqueId
            || !TextureRectEquals(data.fontUsedRect, usedRect)
            || !TextureRectEquals(data.fontUpdateRect, updateRect)
            || texData.Status == ImTextureStatus.WantCreate
            || texData.Status == ImTextureStatus.WantUpdates;
        if (!needsUpload)
        {
            return;
        }

        _ = SDL.UpdateTexture(data.fontTexture, SDLRectPtr.Null, texData.Pixels, pitch);
        data.fontPixelsPtr = pixelsPtr;
        data.fontTexturePitch = pitch;
        data.fontTextureUniqueId = uniqueId;
        data.fontUsedRect = usedRect;
        data.fontUpdateRect = updateRect;
    }

    private static void RenderDrawData(
        ViewportWindowData data,
        SDLTexturePtr fontTexture,
        ImDrawData* drawDataNative
    ) {
        var renderer = data.renderer;
        if (renderer.IsNull || drawDataNative == null || drawDataNative->Valid == 0)
        {
            return;
        }

        var drawData = new ImDrawDataPtr(drawDataNative);
        _ = SDL.SetRenderViewport(renderer, SDLRectPtr.Null);
        _ = SDL.SetRenderClipRect(renderer, SDLRectPtr.Null);
        _ = SDL.SetRenderDrawColor(renderer, 0, 0, 0, 0);
        _ = SDL.RenderClear(renderer);

        var clipOff = drawData.DisplayPos;
        var clipScale = drawData.FramebufferScale;
        var fontTexId = ImGuiNative.GetIO().Fonts.TexRef.GetTexID();

        for (var listIndex = 0; listIndex < drawData.CmdListsCount; listIndex++)
        {
            var drawList = drawData.CmdLists[listIndex];
            if (drawList.IsNull || drawList.VtxBuffer.Size <= 0)
            {
                continue;
            }

            var vertexCount = drawList.VtxBuffer.Size;
            EnsureVertexCapacity(data, vertexCount);
            var srcVertices = drawList.VtxBuffer.Data;
            for (var i = 0; i < vertexCount; i++)
            {
                data.vertexScratch[i] = ToSdlVertex(srcVertices[i], clipOff, clipScale);
            }

            var cmdCount = drawList.CmdBuffer.Size;
            for (var cmdIndex = 0; cmdIndex < cmdCount; cmdIndex++)
            {
                var drawCmd = drawList.CmdBuffer[cmdIndex];
                if (drawCmd.UserCallback != null || drawCmd.ElemCount == 0)
                {
                    continue;
                }

                var clipRectX = (drawCmd.ClipRect.X - clipOff.X) * clipScale.X;
                var clipRectY = (drawCmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                var clipRectZ = (drawCmd.ClipRect.Z - clipOff.X) * clipScale.X;
                var clipRectW = (drawCmd.ClipRect.W - clipOff.Y) * clipScale.Y;
                if (clipRectZ <= clipRectX || clipRectW <= clipRectY)
                {
                    continue;
                }

                var clipRect = new SDLRect((int)clipRectX, (int)clipRectY, (int)(clipRectZ - clipRectX), (int)(clipRectW - clipRectY));
                _ = SDL.SetRenderClipRect(renderer, ref clipRect);

                var elemCount = (int)drawCmd.ElemCount;
                EnsureIndexCapacity(data, elemCount);
                var srcIndices = drawList.IdxBuffer.Data;
                var idxOffset = (int)drawCmd.IdxOffset;
                var vtxOffset = (int)drawCmd.VtxOffset;
                for (var i = 0; i < elemCount; i++)
                {
                    data.indexScratch[i] = srcIndices[idxOffset + i] + vtxOffset;
                }

                var texture = drawCmd.GetTexID() == fontTexId ? fontTexture : TextureFromImGui(drawCmd.GetTexID());
                if (texture.IsNull)
                {
                    texture = fontTexture;
                }

                fixed (SDLVertex* pVertices = data.vertexScratch)
                fixed (int* pIndices = data.indexScratch)
                {
                    _ = SDL.RenderGeometry(renderer, texture, pVertices, vertexCount, pIndices, elemCount);
                }
            }
        }

        _ = SDL.SetRenderClipRect(renderer, SDLRectPtr.Null);
    }

    private static void FocusWindow(SDLWindow window)
    {
        if (!window.IsNull)
        {
            _ = SDL.RaiseWindow(window);
        }
    }

    private static PlatformImGuiViewportBackend GetCurrentBackend()
    {
        ImGuiContextPtr context = ImGuiNative.GetCurrentContext();
        if (context.IsNull)
            throw new InvalidOperationException("No ImGui context is active for a viewport callback.");
        lock (S_CONTEXT_SYNC)
        {
            if (S_BACKENDS_BY_CONTEXT.TryGetValue(
                    (nuint)context.Handle,
                    out PlatformImGuiViewportBackend? backend))
            {
                return backend;
            }
        }
        throw new InvalidOperationException("The active ImGui context has no viewport backend.");
    }

    private static void SynchronizeRendererOutput(SDLRenderer renderer)
    {
        if (renderer.IsNull)
        {
            return;
        }

        _ = SDL.SetRenderLogicalPresentation(
            renderer,
            0,
            0,
            SDLRendererLogicalPresentation.Disabled);
        _ = SDL.SetRenderViewport(renderer, SDLRectPtr.Null);
        _ = SDL.SetRenderClipRect(renderer, SDLRectPtr.Null);
    }

    private static void EnsureVertexCapacity(
        ViewportWindowData data,
        int required
    ) {
        if (data.vertexScratch.Length >= required)
        {
            return;
        }

        Array.Resize(ref data.vertexScratch, required);
    }

    private static void EnsureIndexCapacity(
        ViewportWindowData data,
        int required
    ) {
        if (data.indexScratch.Length >= required)
        {
            return;
        }

        Array.Resize(ref data.indexScratch, required);
    }

    private static SDLTexturePtr CreateTexture(
        SDLRenderer renderer,
        int width,
        int height
    ) {
        var props = SDL.CreateProperties();
        try
        {
            _ = SDL.SetNumberProperty(props, SDL.SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER, (long)SDLPixelFormat.PixelformatRgba32);
            _ = SDL.SetNumberProperty(props, SDL.SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER, (long)SDLTextureAccess.TextureaccessStatic);
            _ = SDL.SetNumberProperty(props, SDL.SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER, width);
            _ = SDL.SetNumberProperty(props, SDL.SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER, height);
            var texture = SDL.CreateTextureWithProperties(renderer, props);
            if (!texture.IsNull)
            {
                _ = SDL.SetTextureBlendMode(texture, SDL.SDL_BLENDMODE_BLEND);
                _ = SDL.SetTextureScaleMode(texture, SDLScaleMode.ScalemodeLinear);
            }

            return texture;
        }
        finally
        {
            SDL.DestroyProperties(props);
        }
    }

    private static Vector2 GetWindowFramebufferScale(SDLWindow window)
    {
        var windowWidth = 0;
        var windowHeight = 0;
        SDL.GetWindowSize(window, ref windowWidth, ref windowHeight);
        if (windowWidth <= 0 || windowHeight <= 0)
        {
            return Vector2.Zero;
        }

        var pixelWidth = 0;
        var pixelHeight = 0;
        SDL.GetWindowSizeInPixels(window, ref pixelWidth, ref pixelHeight);
        return new Vector2(pixelWidth / (float)windowWidth, pixelHeight / (float)windowHeight);
    }

    private static float GetWindowDpiScale(SDLWindow window)
    {
        // ImGui's DPI value is in logical coordinates; pixel density belongs to FramebufferScale.
        float scale = SDL.GetWindowDisplayScale(window) / SDL.GetWindowPixelDensity(window);
        if (!float.IsFinite(scale) || scale <= 0f)
            throw new InvalidOperationException("SDL could not resolve the live window's display scale: " + SDL.GetError());
        return scale;
    }

    private static SDLVertex ToSdlVertex(
        ImDrawVert vtx,
        Vector2 displayPos,
        Vector2 framebufferScale
    ) {
        var color = ImGuiPackedColor.ToSdlFColor(vtx.Col);
        var x = (vtx.Pos.X - displayPos.X) * framebufferScale.X;
        var y = (vtx.Pos.Y - displayPos.Y) * framebufferScale.Y;

        return new SDLVertex(
            new SDLFPoint(x, y),
            color,
            new SDLFPoint(vtx.Uv.X, vtx.Uv.Y));
    }

    private static SDLTexturePtr TextureFromImGui(ImTextureID textureId)
    {
        return (SDLTexturePtr)(SDLTexture*)(void*)textureId;
    }

    private static bool TextureRectEquals(
        ImTextureRect a,
        ImTextureRect b
    ) {
        return a.X == b.X
            && a.Y == b.Y
            && a.W == b.W
            && a.H == b.H;
    }

    private static void* FunctionPtr(Delegate del)
    {
        return (void*)Marshal.GetFunctionPointerForDelegate(del);
    }
}

