using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Inno.Adapter.Platform;
using Inno.Adapter.Platform.Sdl3;
using Inno.Core.Execution;
using Inno.Native.ImGui;
using Inno.Native.Sdl3;
using Inno.Platform;

using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

internal sealed unsafe partial class PlatformImGuiViewportBackend : IDisposable
{
    private static readonly ImGuiPlatformIoNative.PlatformGetWindowPosCallback s_platformGetWindowPos = PlatformGetWindowPosOutCallback;
    private static readonly ImGuiPlatformIoNative.PlatformGetWindowSizeCallback s_platformGetWindowSize = PlatformGetWindowSizeOutCallback;

    private static readonly object S_CONTEXT_SYNC = new();
    private static readonly Dictionary<nuint, PlatformImGuiViewportBackend> S_BACKENDS_BY_CONTEXT = [];

    private readonly Sdl3PlatformApplication m_application;
    private readonly SDLWindow* m_mainWindow;
    private readonly IPlatformImGuiRenderer m_renderer;
    private readonly nuint m_contextKey;
    private readonly Dictionary<uint, ViewportWindowData> m_viewportsById = [];
    private readonly Dictionary<uint, uint> m_windowToViewport = [];
    private readonly List<ViewportWindowData> m_retiringViewports = [];
    private void* m_windowCallbacks;
    private Exception? m_callbackFailure;
    private bool m_monitorsDirty;
    private bool m_disposed;

    internal PlatformImGuiViewportBackend(
        Sdl3PlatformApplication application,
        Sdl3PlatformWindow mainWindow,
        IPlatformImGuiRenderer renderer
    ) {
        ArgumentNullException.ThrowIfNull(renderer);
        ImGuiNativeContext* context = ImGuiNative.Raw.GetCurrentContext();
        if (context is null)
        {
            throw new InvalidOperationException("An active ImGui context is required for viewport routing.");
        }
        m_contextKey = (nuint)context;
        lock (S_CONTEXT_SYNC)
        {
            if (S_BACKENDS_BY_CONTEXT.ContainsKey(m_contextKey))
            {
                throw new InvalidOperationException("The current ImGui context already owns a viewport backend.");
            }
        }
        m_application = application;
        m_renderer = renderer;
        _ = SDL.SetHint(SDL.Constants.SDL_HINT_WINDOW_ACTIVATE_WHEN_RAISED, "1");
        m_mainWindow = mainWindow.GetSdlWindow();

        ImGuiPlatformIO* platformIo = ImGuiNative.Raw.GetPlatformIO();
        if (ImGuiNative.Raw.GetIO()->BackendPlatformUserData is not null)
        {
            throw new InvalidOperationException("The current ImGui context already has platform backend state.");
        }
        platformIo->PlatformCreateWindow = &PlatformCreateWindowCallback;
        platformIo->PlatformDestroyWindow = &PlatformDestroyWindowCallback;
        platformIo->PlatformShowWindow = &PlatformShowWindowCallback;
        platformIo->PlatformGetWindowFramebufferScale = default;
        platformIo->PlatformSetWindowFocus = &PlatformSetWindowFocusCallback;
        platformIo->PlatformGetWindowFocus = &PlatformGetWindowFocusCallback;
        platformIo->PlatformGetWindowMinimized = &PlatformGetWindowMinimizedCallback;
        platformIo->PlatformSetWindowTitle = &PlatformSetWindowTitleCallback;
        platformIo->PlatformSetWindowAlpha = &PlatformSetWindowAlphaCallback;
        platformIo->RendererRenderWindow = &RendererRenderWindowCallback;
        platformIo->RendererSwapBuffers = &RendererSwapBuffersCallback;
        ImGuiPlatformIoNative.SetPlatformGetWindowPos(platformIo, s_platformGetWindowPos);
        ImGuiPlatformIoNative.SetPlatformGetWindowSize(platformIo, s_platformGetWindowSize);

        ImGuiViewport* mainViewport = ImGuiNative.Raw.GetMainViewport();
        SDLWindow* mainSdlWindow = mainWindow.GetSdlWindow();
        mainViewport->PlatformHandle = (void*)mainSdlWindow;
        mainViewport->PlatformHandleRaw = (void*)mainWindow.nativeHandles.windowHandle;

        try
        {
            RefreshMonitors(mainSdlWindow);
            m_windowCallbacks = ImGuiPlatformIoNative.CreateWindowCallbacks(
                context,
                &PlatformSetWindowPosCallback,
                &PlatformSetWindowSizeCallback
            );
            lock (S_CONTEXT_SYNC)
            {
                S_BACKENDS_BY_CONTEXT.Add(m_contextKey, this);
            }
        }
        catch
        {
            ImGuiPlatformIoNative.DestroyWindowCallbacks(m_windowCallbacks);
            m_windowCallbacks = null;
            ImGuiNative.Raw.ClearPlatformHandlers(platformIo);
            ImGuiNative.Raw.ClearRendererHandlers(platformIo);
            mainViewport->PlatformHandle = null;
            mainViewport->PlatformHandleRaw = null;
            throw;
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

        foreach (KeyValuePair<uint, ViewportWindowData> kv in m_viewportsById)
        {
            DestroyViewportWindow(kv.Value);
        }

        m_viewportsById.Clear();
        m_windowToViewport.Clear();
        if (m_retiringViewports.Count != 0)
        {
            m_renderer.DrainViewportRetirements();
        }
        ProcessRetirements();
        if (m_retiringViewports.Count != 0)
        {
            throw new RetirementPendingException("Detached ImGui windows are still borrowed by their renderer.");
        }

        ImGuiPlatformIO* platformIo = ImGuiNative.Raw.GetPlatformIO();
        ImGuiPlatformIoNative.DestroyWindowCallbacks(m_windowCallbacks);
        m_windowCallbacks = null;
        ImGuiNative.Raw.ClearPlatformHandlers(platformIo);
        ImGuiNative.Raw.ClearRendererHandlers(platformIo);

        ImGuiViewport* mainViewport = ImGuiNative.Raw.GetMainViewport();
        if (mainViewport is not null)
        {
            mainViewport->PlatformHandle = null;
            mainViewport->PlatformHandleRaw = null;
        }

        lock (S_CONTEXT_SYNC)
        {
            if (S_BACKENDS_BY_CONTEXT.TryGetValue(m_contextKey, out PlatformImGuiViewportBackend? registered)
                && ReferenceEquals(registered, this)
            ) {
                S_BACKENDS_BY_CONTEXT.Remove(m_contextKey);
            }
        }
        m_disposed = true;
    }

    internal bool OwnsWindow(uint windowId) => m_windowToViewport.ContainsKey(windowId);

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
        SDLEventType eventType = (SDLEventType)sdlEvent.Type;
        if (eventType >= SDLEventType.DisplayFirst && eventType <= SDLEventType.DisplayLast
            || eventType == SDLEventType.WindowDisplayChanged
            || eventType == SDLEventType.WindowDisplayScaleChanged
        ) {
            m_monitorsDirty = true;
        }
        if (!m_windowToViewport.TryGetValue(windowId, out uint viewportId)
            || !m_viewportsById.TryGetValue(viewportId, out ViewportWindowData? data)
        ) {
            return;
        }

        ImGuiViewport* viewport = data.viewport;
        if (eventType == SDLEventType.WindowCloseRequested)
        {
            viewport->PlatformRequestClose = 1;
        }
        if (eventType == SDLEventType.WindowMoved)
        {
            viewport->PlatformRequestMove = 1;
        }
        if (eventType == SDLEventType.WindowResized || eventType == SDLEventType.WindowPixelSizeChanged)
        {
            viewport->PlatformRequestResize = 1;
        }
    }

    internal void SynchronizeMetrics()
    {
        if (m_monitorsDirty)
        {
            RefreshMonitors(m_mainWindow);
            m_monitorsDirty = false;
        }
        ImGuiViewport* main = ImGuiNative.Raw.GetMainViewport();
        Vector2 scale = GetWindowFramebufferScale(m_mainWindow);
        if (scale.X > 0 && scale.Y > 0)
        {
            main->FramebufferScale = new ImVec2C { X = scale.X, Y = scale.Y };
            if (main->DpiScale == 0f)
            {
                main->DpiScale = GetWindowDpiScale(m_mainWindow);
            }
        }
        foreach (ViewportWindowData data in m_viewportsById.Values)
        {
            SynchronizeWindow(data.windowId);
        }
    }

    /// <summary>
    /// Synchronizes a secondary viewport's drawable scale and renderer output.
    /// </summary>
    /// <param name="windowId">
    /// The SDL window identifier owned by the viewport->
    /// </param>
    internal void SynchronizeWindow(uint windowId)
    {
        if (!m_windowToViewport.TryGetValue(windowId, out uint viewportId))
        {
            return;
        }

        if (!m_viewportsById.TryGetValue(viewportId, out ViewportWindowData? data))
        {
            return;
        }
        ImGuiViewport* viewport = data.viewport;

        int width = 0;
        int height = 0;
        SDL.Raw.GetWindowSize(data.window, &width, &height);
        if (width <= 0 || height <= 0)
        {
            data.drawable = false;
            return;
        }

        // ImGui owns requested geometry. Native move/resize events request reconciliation
        // through the platform getters; metric refresh must not overwrite an active drag.
        Vector2 framebufferScale = GetWindowFramebufferScale(data.window);
        if (framebufferScale.X > 0 && framebufferScale.Y > 0)
        {
            if (viewport->DpiScale == 0f)
            {
                viewport->DpiScale = GetWindowDpiScale(data.window);
            }
            viewport->FramebufferScale = new ImVec2C { X = framebufferScale.X, Y = framebufferScale.Y };
        }
        int pixelWidth = 0;
        int pixelHeight = 0;
        SDL.Raw.GetWindowSizeInPixels(data.window, &pixelWidth, &pixelHeight);
        data.drawable = pixelWidth > 0 && pixelHeight > 0
            && (((SDLWindowFlags)SDL.Raw.GetWindowFlags(data.window)) & (SDLWindowFlags.Minimized | SDLWindowFlags.Hidden)) == 0;
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

    private void RecordCallbackFailure(Exception failure)
        => m_callbackFailure = m_callbackFailure is null
        ? failure
        : new AggregateException("Multiple native viewport callbacks failed in the same frame.", m_callbackFailure, failure);

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformCreateWindowCallback(ImGuiViewport* viewport)
    {
        try
        {
        if (viewport is null || viewport->PlatformUserData is not null || (viewport->Flags & (int)ImGuiViewportFlags.OwnedByApp) != 0)
        {
            return;
        }

        SDLWindowFlags flags = SDLWindowFlags.Hidden | SDLWindowFlags.Resizable | SDLWindowFlags.HighPixelDensity;
        if ((viewport->Flags & (int)ImGuiViewportFlags.NoDecoration) != 0)
        {
            flags |= SDLWindowFlags.Borderless;
        }

        if ((viewport->Flags & (int)ImGuiViewportFlags.NoTaskBarIcon) != 0)
        {
            flags |= SDLWindowFlags.Utility;
        }

        if ((viewport->Flags & (int)ImGuiViewportFlags.TopMost) != 0)
        {
            flags |= SDLWindowFlags.AlwaysOnTop;
        }

        int width = Math.Max(1, (int)viewport->Size.X);
        int height = Math.Max(1, (int)viewport->Size.Y);
        SDLWindow* window;
        fixed (byte* title = "ImGui\0"u8)
        {
            window = SDL.Raw.CreateWindow((sbyte*)title, width, height, (ulong)flags);
        }
        if (window is null)
        {
            return;
        }

        _ = SDL.Raw.SetWindowPosition(window, (int)viewport->Pos.X, (int)viewport->Pos.Y);

        PlatformImGuiViewportBackend backend = GetCurrentBackend();
        IPlatformImGuiRenderer rendererBackend = backend.m_renderer;
        SDLRenderer* renderer = null;
        if (!rendererBackend.supportsViewports)
        {
            renderer = SDL.Raw.CreateRenderer(window, null);
            if (renderer is null)
            {
                SDL.Raw.DestroyWindow(window);
                return;
            }

            _ = SDL.Raw.SetRenderDrawBlendMode(renderer, SDL.Constants.SDL_BLENDMODE_BLEND);
        }

        Sdl3PlatformWindow registeredWindow;
        try
        {
            registeredWindow = backend.m_application.AdoptWindow((nint)window);
        }
        catch
        {
            if (renderer is not null)
            {
                SDL.Raw.DestroyRenderer(renderer);
            }
            SDL.Raw.DestroyWindow(window);
            throw;
        }
            var data = new ViewportWindowData
        {
            owner = backend,
            registeredWindow = registeredWindow,
            viewport = viewport,
            drawable = true,
            window = window,
            renderer = renderer,
            windowId = SDL.Raw.GetWindowId(window)
        };
        try
        {
            if (rendererBackend.supportsViewports)
            {
                int pixelWidth = 0;
                int pixelHeight = 0;
                SDL.Raw.GetWindowSizeInPixels(window, &pixelWidth, &pixelHeight);
                data.externalRenderer = rendererBackend;
                data.externalTarget = new PlatformImGuiViewportTarget(
                    viewport->ID,
                    data.windowId,
                    registeredWindow.nativeHandles,
                    pixelWidth,
                    pixelHeight
                );
                rendererBackend.CreateViewport(data.externalTarget);
            }
            data.gcHandle = GCHandle.Alloc(data, GCHandleType.Normal);
            backend.m_viewportsById.Add(viewport->ID, data);
            backend.m_windowToViewport.Add(data.windowId, viewport->ID);

            nint handlePtr = GCHandle.ToIntPtr(data.gcHandle);
            viewport->PlatformUserData = (void*)handlePtr;
            viewport->RendererUserData = (void*)handlePtr;
            viewport->PlatformHandle = (void*)window;
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
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformDestroyWindowCallback(ImGuiViewport* viewport)
    {
        try
        {
            if (viewport is null || (viewport->Flags & (int)ImGuiViewportFlags.OwnedByApp) != 0)
            {
                return;
            }

            if (TryGetViewportData(viewport, out ViewportWindowData data))
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
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformShowWindowCallback(ImGuiViewport* viewport)
    {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return;
            }

            _ = SDL.Raw.ShowWindow(window);
            GetCurrentBackend().SynchronizeWindow(SDL.Raw.GetWindowId(window));
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetWindowPosCallback(
        ImGuiViewport* viewport,
        float x,
        float y
    ) {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return;
            }

            _ = SDL.Raw.SetWindowPosition(window, (int)x, (int)y);
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    private static void PlatformGetWindowPosOutCallback(
        ImGuiViewport* viewport,
        ImVec2C* outPos
    ) {
        try
        {
            if (outPos is null)
            {
                return;
            }

            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                *outPos = default;
                return;
            }

            int x = 0;
            int y = 0;
            _ = SDL.Raw.GetWindowPosition(window, &x, &y);
            *outPos = new ImVec2C { X = x, Y = y };
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetWindowSizeCallback(
        ImGuiViewport* viewport,
        float width,
        float height
    ) {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return;
            }

            _ = SDL.Raw.SetWindowSize(window, Math.Max(1, (int)width), Math.Max(1, (int)height));
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    private static void PlatformGetWindowSizeOutCallback(
        ImGuiViewport* viewport,
        ImVec2C* outSize
    ) {
        try
        {
            if (outSize is null)
            {
                return;
            }

            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                *outSize = default;
                return;
            }

            int width = 0;
            int height = 0;
            SDL.Raw.GetWindowSize(window, &width, &height);
            *outSize = new ImVec2C { X = width, Y = height };
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetWindowFocusCallback(ImGuiViewport* viewport)
    {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return;
            }

            FocusWindow(window);
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static byte PlatformGetWindowFocusCallback(ImGuiViewport* viewport)
    {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return 0;
            }

            SDLWindowFlags flags = (SDLWindowFlags)((SDLWindowFlags)SDL.Raw.GetWindowFlags(window));
            return (flags & SDLWindowFlags.InputFocus) != 0 ? (byte)1 : (byte)0;
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
            return 0;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static byte PlatformGetWindowMinimizedCallback(ImGuiViewport* viewport)
    {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return 0;
            }

            SDLWindowFlags flags = (SDLWindowFlags)((SDLWindowFlags)SDL.Raw.GetWindowFlags(window));
            return (flags & SDLWindowFlags.Minimized) != 0 ? (byte)1 : (byte)0;
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
            return 0;
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetWindowTitleCallback(
        ImGuiViewport* viewport,
        sbyte* title
    ) {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return;
            }

            _ = SDL.Raw.SetWindowTitle(window, title);
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void PlatformSetWindowAlphaCallback(
        ImGuiViewport* viewport,
        float alpha
    ) {
        try
        {
            if (!TryGetWindow(viewport, out SDLWindow* window))
            {
                return;
            }

            _ = SDL.Raw.SetWindowOpacity(window, alpha);
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void RendererRenderWindowCallback(
        ImGuiViewport* viewport,
        void* renderArg
    ) {
        try
        {
            _ = renderArg;
            if (!TryGetViewportData(viewport, out ViewportWindowData data) || viewport is null || viewport->DrawData is null)
            {
                return;
            }

            data.owner.SynchronizeWindow(data.windowId);
            if (!data.drawable)
            {
                return;
            }

            if (data.externalRenderer is not null && data.externalTarget is not null)
            {
                data.externalRenderer.RenderViewport(data.externalTarget, (nint)viewport->DrawData);
            }
            else
            {
                EnsureViewportFontTexture(data);
                RenderDrawData(data, data.fontTexture, viewport->DrawData);
            }
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    [UnmanagedCallersOnly(CallConvs = [typeof(CallConvCdecl)])]
    private static void RendererSwapBuffersCallback(
        ImGuiViewport* viewport,
        void* renderArg
    ) {
        try
        {
            _ = renderArg;
            if (!TryGetViewportData(viewport, out ViewportWindowData data))
            {
                return;
            }

            data.owner.SynchronizeWindow(data.windowId);
            if (!data.drawable)
            {
                return;
            }

            if (data.externalRenderer is not null && data.externalTarget is not null)
            {
                data.externalRenderer.PresentViewport(data.externalTarget);
            }
            else
            {
                _ = SDL.Raw.RenderPresent(data.renderer);
            }
        }
        catch (Exception failure)
        {
            GetCurrentBackend().RecordCallbackFailure(failure);
        }
    }

    private static bool TryGetViewportData(
        ImGuiViewport* viewport,
        out ViewportWindowData data
    ) {
        if (viewport is null || viewport->PlatformUserData is null)
        {
            data = null!;
            return false;
        }

        GCHandle handle = GCHandle.FromIntPtr((nint)viewport->PlatformUserData);
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
        out SDLWindow* window
    ) {
        if (viewport is not null && viewport->PlatformHandle is not null)
        {
            window = (SDLWindow*)viewport->PlatformHandle;
            return true;
        }

        if (TryGetViewportData(viewport, out ViewportWindowData data))
        {
            window = data.window;
            return true;
        }

        window = null;
        return false;
    }

    private static void EnsureViewportFontTexture(ViewportWindowData data)
    {
        ImGuiIO* io = ImGuiNative.Raw.GetIO();
        if (io->Fonts is null)
        {
            return;
        }

        io->Fonts->RendererHasTextures = 1;
        ImTextureData* texData = io->Fonts->TexData;
        if (texData is null)
        {
            return;
        }

        if (texData->Status == ImTextureStatus.WantDestroy)
        {
            if (data.fontTexture is not null)
            {
                SDL.Raw.DestroyTexture(data.fontTexture);
                data.fontTexture = null;
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

        if (texData->Pixels is null || texData->Width <= 0 || texData->Height <= 0)
        {
            return;
        }

        bool needsRecreate = data.fontTexture is null
                             || data.fontTextureWidth != texData->Width
                             || data.fontTextureHeight != texData->Height;
        if (needsRecreate)
        {
            if (data.fontTexture is not null)
            {
                SDL.Raw.DestroyTexture(data.fontTexture);
                data.fontTexture = null;
            }

            data.fontTexture = CreateTexture(data.renderer, texData->Width, texData->Height);
            if (data.fontTexture is null)
            {
                data.fontTextureWidth = 0;
                data.fontTextureHeight = 0;
                return;
            }

            data.fontTextureWidth = texData->Width;
            data.fontTextureHeight = texData->Height;
        }

        int pitch = ImGuiNative.Raw.GetPitch(texData);
        nint pixelsPtr = (nint)texData->Pixels;
        int uniqueId = texData->UniqueId;
        ImTextureRect usedRect = texData->UsedRect;
        ImTextureRect updateRect = texData->UpdateRect;
        bool needsUpload = needsRecreate
                           || data.fontPixelsPtr != pixelsPtr
                           || data.fontTexturePitch != pitch
                           || data.fontTextureUniqueId != uniqueId
                           || !TextureRectEquals(data.fontUsedRect, usedRect)
                           || !TextureRectEquals(data.fontUpdateRect, updateRect)
                           || texData->Status == ImTextureStatus.WantCreate
                           || texData->Status == ImTextureStatus.WantUpdates;
        if (!needsUpload)
        {
            return;
        }

        _ = SDL.Raw.UpdateTexture(data.fontTexture, null, texData->Pixels, pitch);
        data.fontPixelsPtr = pixelsPtr;
        data.fontTexturePitch = pitch;
        data.fontTextureUniqueId = uniqueId;
        data.fontUsedRect = usedRect;
        data.fontUpdateRect = updateRect;
    }

    private static void RenderDrawData(
        ViewportWindowData data,
        SDLTexture* fontTexture,
        ImDrawData* drawDataNative
    ) {
        SDLRenderer* renderer = data.renderer;
        if (renderer is null || drawDataNative is null || drawDataNative->Valid == 0)
        {
            return;
        }

        ImDrawData* drawData = drawDataNative;
        _ = SDL.Raw.SetRenderViewport(renderer, null);
        _ = SDL.Raw.SetRenderClipRect(renderer, null);
        _ = SDL.Raw.SetRenderDrawColor(renderer, 0, 0, 0, 0);
        _ = SDL.Raw.RenderClear(renderer);

        ImVec2C clipOff = drawData->DisplayPos;
        ImVec2C clipScale = drawData->FramebufferScale;
        ulong fontTexId = ImGuiNative.Raw.GetTexID(&ImGuiNative.Raw.GetIO()->Fonts->TexRef);

        for (int listIndex = 0; listIndex < drawData->CmdListsCount; listIndex++)
        {
            ImDrawList* drawList = drawData->CmdLists.Data[listIndex];
            if (drawList is null || drawList->VtxBuffer.Size <= 0)
            {
                continue;
            }

            int vertexCount = drawList->VtxBuffer.Size;
            EnsureVertexCapacity(data, vertexCount);
            ImDrawVert* srcVertices = drawList->VtxBuffer.Data;
            for (int i = 0; i < vertexCount; i++)
            {
                data.vertexScratch[i] = ToSdlVertex(srcVertices[i], clipOff, clipScale);
            }

            int cmdCount = drawList->CmdBuffer.Size;
            for (int cmdIndex = 0; cmdIndex < cmdCount; cmdIndex++)
            {
                ImDrawCmd drawCmd = drawList->CmdBuffer.Data[cmdIndex];
                if (drawCmd.UserCallback is not null || drawCmd.ElemCount == 0)
                {
                    continue;
                }

                float clipRectX = (drawCmd.ClipRect.X - clipOff.X) * clipScale.X;
                float clipRectY = (drawCmd.ClipRect.Y - clipOff.Y) * clipScale.Y;
                float clipRectZ = (drawCmd.ClipRect.Z - clipOff.X) * clipScale.X;
                float clipRectW = (drawCmd.ClipRect.W - clipOff.Y) * clipScale.Y;
                if (clipRectZ <= clipRectX || clipRectW <= clipRectY)
                {
                    continue;
                }

                var clipRect = new SDLRect
                {
                    X = (int)clipRectX,
                    Y = (int)clipRectY,
                    W = (int)(clipRectZ - clipRectX),
                    H = (int)(clipRectW - clipRectY)
                };
                _ = SDL.Raw.SetRenderClipRect(renderer, &clipRect);

                int elemCount = (int)drawCmd.ElemCount;
                EnsureIndexCapacity(data, elemCount);
                ushort* srcIndices = drawList->IdxBuffer.Data;
                int idxOffset = (int)drawCmd.IdxOffset;
                int vtxOffset = (int)drawCmd.VtxOffset;
                for (int i = 0; i < elemCount; i++)
                {
                    data.indexScratch[i] = srcIndices[idxOffset + i] + vtxOffset;
                }

                SDLTexture* texture = ImGuiNative.Raw.GetTexID(&drawCmd) == fontTexId
                                      ? fontTexture
                                      : TextureFromImGui(ImGuiNative.Raw.GetTexID(&drawCmd));
                if (texture is null)
                {
                    texture = fontTexture;
                }

                fixed (SDLVertex* pVertices = data.vertexScratch)
                fixed (int* pIndices = data.indexScratch)
                {
                    _ = SDL.Raw.RenderGeometry(renderer, texture, pVertices, vertexCount, pIndices, elemCount);
                }
            }
        }

        _ = SDL.Raw.SetRenderClipRect(renderer, null);
    }

    private static void FocusWindow(SDLWindow* window)
    {
        if (window is not null)
        {
            _ = SDL.Raw.RaiseWindow(window);
        }
    }

    private static PlatformImGuiViewportBackend GetCurrentBackend()
    {
        ImGuiNativeContext* context = ImGuiNative.Raw.GetCurrentContext();
        if (context is null)
        {
            throw new InvalidOperationException("No ImGui context is active for a viewport callback.");
        }
        lock (S_CONTEXT_SYNC)
        {
            if (S_BACKENDS_BY_CONTEXT.TryGetValue((nuint)context, out PlatformImGuiViewportBackend? backend))
            {
                return backend;
            }
        }
        throw new InvalidOperationException("The active ImGui context has no viewport backend.");
    }

    private static void SynchronizeRendererOutput(SDLRenderer* renderer)
    {
        if (renderer is null)
        {
            return;
        }

        _ = SDL.Raw.SetRenderLogicalPresentation(renderer, 0, 0, SDLRendererLogicalPresentation.Disabled);
        _ = SDL.Raw.SetRenderViewport(renderer, null);
        _ = SDL.Raw.SetRenderClipRect(renderer, null);
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

    private static SDLTexture* CreateTexture(
        SDLRenderer* renderer,
        int width,
        int height
    ) {
        uint props = SDL.CreateProperties();
        try
        {
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_FORMAT_NUMBER, (long)SDLPixelFormat.Rgba32);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_ACCESS_NUMBER, (long)SDLTextureAccess.Static);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_WIDTH_NUMBER, width);
            _ = SDL.SetNumberProperty(props, SDL.Constants.SDL_PROP_TEXTURE_CREATE_HEIGHT_NUMBER, height);
            SDLTexture* texture = SDL.Raw.CreateTextureWithProperties(renderer, props);
            if (texture is not null)
            {
                _ = SDL.Raw.SetTextureBlendMode(texture, SDL.Constants.SDL_BLENDMODE_BLEND);
                _ = SDL.Raw.SetTextureScaleMode(texture, SDLScaleMode.Linear);
            }

            return texture;
        }
        finally
        {
            SDL.DestroyProperties(props);
        }
    }

    private static Vector2 GetWindowFramebufferScale(SDLWindow* window)
    {
        int windowWidth = 0;
        int windowHeight = 0;
        SDL.Raw.GetWindowSize(window, &windowWidth, &windowHeight);
        if (windowWidth <= 0 || windowHeight <= 0)
        {
            return Vector2.Zero;
        }

        int pixelWidth = 0;
        int pixelHeight = 0;
        SDL.Raw.GetWindowSizeInPixels(window, &pixelWidth, &pixelHeight);
        return new Vector2(pixelWidth / (float)windowWidth, pixelHeight / (float)windowHeight);
    }

    private static float GetWindowDpiScale(SDLWindow* window)
    {
        // ImGui's DPI value is in logical coordinates; pixel density belongs to FramebufferScale.
        float scale = SDL.Raw.GetWindowDisplayScale(window) / SDL.Raw.GetWindowPixelDensity(window);
        if (!float.IsFinite(scale) || scale <= 0f)
        {
            throw new InvalidOperationException(
                "SDL could not resolve the live window's display scale: "
                + Marshal.PtrToStringUTF8((nint)SDL.Raw.GetError())
            );
        }
        return scale;
    }

    private static SDLVertex ToSdlVertex(
        ImDrawVert vtx,
        ImVec2C displayPos,
        ImVec2C framebufferScale
    ) {
        SDLFColor color = ImGuiPackedColor.ToSdlFColor(vtx.Col);
        float x = (vtx.Pos.X - displayPos.X) * framebufferScale.X;
        float y = (vtx.Pos.Y - displayPos.Y) * framebufferScale.Y;

        return new SDLVertex
        {
            Position = new SDLFPoint { X = x, Y = y },
            Color = color,
            TexCoord = new SDLFPoint { X = vtx.Uv.X, Y = vtx.Uv.Y }
        };
    }

    private static SDLTexture* TextureFromImGui(ulong textureId) => (SDLTexture*)(nuint)textureId;

    private static bool TextureRectEquals(
        ImTextureRect a,
        ImTextureRect b
    ) => a.X == b.X
         && a.Y == b.Y
         && a.W == b.W
         && a.H == b.H;

    private sealed class ViewportWindowData
    {
        internal required PlatformImGuiViewportBackend owner;
        internal required Sdl3PlatformWindow registeredWindow;
        internal ImGuiViewport* viewport;
        internal bool drawable;
        internal Task? retirement;
        internal bool detached;
        internal SDLWindow* window;
        internal SDLRenderer* renderer;
        internal IPlatformImGuiRenderer? externalRenderer;
        internal PlatformImGuiViewportTarget? externalTarget;
        internal SDLTexture* fontTexture;
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
}
