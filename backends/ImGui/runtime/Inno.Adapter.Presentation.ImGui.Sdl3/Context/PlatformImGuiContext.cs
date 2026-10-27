using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;

using Inno.Adapter.Platform.Sdl3;
using Inno.Native.Sdl3;
using Inno.Native.ImGui;
using ImGuiNative = Inno.Native.ImGui.ImGui;

namespace Inno.Adapter.Presentation.ImGui;

/// <summary>
/// Provides operation-scoped platform im gui context services and state without transferring their ownership.
/// </summary>
public sealed unsafe partial class PlatformImGuiContext
{
    private const float DEFAULT_KEY_REPEAT_DELAY_SECONDS = 0.275f;
    private const float DEFAULT_KEY_REPEAT_RATE_SECONDS = 0.050f;
    private const float DEFAULT_FONT_SIZE_PIXELS = 16.0f;
    private const float DEFAULT_ICON_SIZE_PIXELS = 14.0f;
    private const float DEFAULT_MOUSE_SCROLLING_UNITS = 0.3f;
    private const string DEFAULT_FONTS_DIRECTORY_RELATIVE_PATH = "BuiltIn/Fonts";
    private const string DEFAULT_ICONS_DIRECTORY_RELATIVE_PATH = "BuiltIn/Icons";
    private const string DEFAULT_FONT_FILE_NAME = "JetBrainsMono-Regular.ttf";
    private const double LIVE_RESIZE_HOVER_LOCK_TIMEOUT_SECONDS = 0.25;

    private readonly ImGuiInteractionOptions m_interaction;
    private readonly Sdl3PlatformWindow m_window;
    private readonly ImGuiNativeContext* m_context;
    private readonly PlatformImGuiViewportBackend? m_viewports;
    private readonly IPlatformImGuiRenderer m_renderer;
    private readonly Dictionary<ImGuiMouseCursor, nint> m_cursors = [];
    private readonly HashSet<uint> m_pendingLiveResizeWindowIds = [];
    private readonly Stopwatch m_frameTimer = Stopwatch.StartNew();

    private ImGuiMouseCursor m_currentCursor = ImGuiMouseCursor.None;
    private SDLWindow* m_textInputWindow = null;
    private TimeSpan m_lastFrameTime;
    private TimeSpan m_lastLiveResizeLockTime;
    private IntPtr m_iniFilename;
    private uint m_liveResizeLockedWindowId;
    private uint m_mousePendingLeaveWindowId;
    private uint m_mouseWindowId;
    private int m_mouseButtonsDown;
    private int m_mousePendingLeaveFrame;
    private readonly bool m_enableSmoothResize;
    private bool m_hasStartedFrame;
    private bool m_isFrameActive;
    private bool m_isFrameOperationActive;
    private bool m_textInputActive;
    private bool m_disposed;

    internal PlatformImGuiContext(
        Sdl3PlatformApplication application,
        Sdl3PlatformWindow window,
        ImGuiContextFlags contextFlags,
        ImGuiInteractionOptions interaction,
        IPlatformImGuiRenderer? renderer
    ) {
        var enableViewports = (contextFlags & ImGuiContextFlags.EnableViewports) != 0;
        var enableDocking = (contextFlags & ImGuiContextFlags.EnableDocking) != 0;
        m_enableSmoothResize = (contextFlags & ImGuiContextFlags.EnableSmoothResize) != 0;

        m_interaction = interaction;
        m_window = window;
        m_context = ImGuiNative.Raw.CreateContext(null);
        ImGuiNative.Raw.SetCurrentContext(m_context);

        try
        {
            var io = ImGuiNative.Raw.GetIO();
            // SDL cannot reliably identify the viewport beneath a transient NoInputs viewport while
            // docking, and the official SDL backend disables this capability on macOS. Let Dear ImGui
            // use its viewport heuristic instead of publishing incorrect hovered viewport identities.
            io->BackendFlags |= (int)ImGuiBackendFlags.HasMouseCursors
                | (int)ImGuiBackendFlags.HasSetMousePos
                | (int)ImGuiBackendFlags.RendererHasTextures;
            io->ConfigFlags |= (int)ImGuiConfigFlags.NavEnableKeyboard;

            if (enableDocking)
            {
                io->ConfigFlags |= (int)ImGuiConfigFlags.DockingEnable;
            }

            if (enableViewports)
            {
                io->ConfigFlags |= (int)ImGuiConfigFlags.ViewportsEnable;
                io->ConfigDpiScaleViewports = 1;
                io->BackendFlags |= (int)ImGuiBackendFlags.PlatformHasViewports | (int)ImGuiBackendFlags.RendererHasViewports;
            }

            io->ConfigDpiScaleFonts = 1;
            io->ConfigMacOsxBehaviors = interaction.commandKeyBehavior ? (byte)1 : (byte)0;
            io->MouseDrawCursor = 0;
            io->Fonts->RendererHasTextures = 1;
            ConfigureKeyRepeat(io);
            ConfigureFonts(m_context, io);
            ImGuiPackedColor.EnsureInitialized();

            UpdateDisplayMetrics(io);
            m_renderer = renderer ?? new PlatformImGuiSdlRenderer(window);
            if (m_renderer.supportsVertexOffsets)
                io->BackendFlags |= (int)ImGuiBackendFlags.RendererHasVtxOffset;
            m_viewports = enableViewports ? new PlatformImGuiViewportBackend(application, window, m_renderer) : null;
            RegisterClipboardCallbacks();
        }
        catch
        {
            m_viewports?.Dispose();
            ImGuiFont.UnregisterContext(m_context);
            ImGuiNative.Raw.DestroyContext(m_context);
            if (renderer is null)
                m_renderer?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Resolves the SDL window currently owned by an ImGui viewport.
    /// </summary>
    /// <param name="viewportId">
    /// The ImGui viewport identity.
    /// </param>
    /// <param name="windowId">
    /// The SDL window identity when available.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the viewport has a live SDL window.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after the presentation context has been disposed.
    /// </exception>
    public partial bool TryGetWindowId(
        uint viewportId,
        out uint windowId
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ImGuiNative.Raw.SetCurrentContext(m_context);
        if (viewportId == ImGuiNative.Raw.GetMainViewport()->ID)
        {
            windowId = m_window.windowId;
            return true;
        }
        if (m_viewports is not null && m_viewports.TryGetWindowId(viewportId, out windowId))
            return true;
        windowId = 0;
        return false;
    }

    /// <summary>
    /// Updates the ini file state and applies the resulting invariants.
    /// </summary>
    /// <param name="filePath">
    /// The file path text validated by the set ini file operation.
    /// </param>
    public partial void SetIniFile(string? filePath)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_hasStartedFrame)
            throw new InvalidOperationException("The ImGui layout file must be configured before the first frame.");

        ImGuiNative.Raw.SetCurrentContext(m_context);
        var io = ImGuiNative.Raw.GetIO();
        if (m_iniFilename != IntPtr.Zero)
        {
            Marshal.FreeCoTaskMem(m_iniFilename);
            m_iniFilename = IntPtr.Zero;
        }
        if (string.IsNullOrWhiteSpace(filePath))
        {
            SetIniFilename(io, IntPtr.Zero);
            return;
        }

        string fullPath = Path.GetFullPath(filePath);
        string? directory = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrWhiteSpace(directory))
            Directory.CreateDirectory(directory);
        m_iniFilename = Marshal.StringToCoTaskMemUTF8(fullPath);
        SetIniFilename(io, m_iniFilename);
    }

    /// <summary>
    /// Loads the current editor layout text into Dear ImGui without retaining the source string.
    /// </summary>
    /// <param name="settings">
    /// The validated configuration that controls this operation.
    /// </param>
    public partial void LoadIniSettings(string? settings)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_hasStartedFrame)
            throw new InvalidOperationException("ImGui layout settings must be loaded before the first frame.");
        if (string.IsNullOrEmpty(settings))
            return;

        ImGuiNative.Raw.SetCurrentContext(m_context);
        byte[] encoded = Encoding.UTF8.GetBytes(settings);
        fixed (byte* data = encoded)
            ImGuiNative.Raw.LoadIniSettingsFromMemory((sbyte*)data, (uint)encoded.Length);
    }

    /// <summary>
    /// Renders the image presentation for the current editor frame.
    /// </summary>
    /// <param name="texture">
    /// The texture consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="size">
    /// The size consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="uv0">
    /// The uv0 consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="uv1">
    /// The uv1 consumed by draw image; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public unsafe partial void DrawImage(
        ImGuiTextureHandle texture,
        Vector2 size,
        Vector2 uv0,
        Vector2 uv1
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!texture.isValid)
        {
            throw new ArgumentException("ImGui texture token is invalid.", nameof(texture));
        }

        if (uv1 == default)
        {
            uv1 = Vector2.One;
        }

        ImGuiNative.Image(
            new ImTextureRef { TexID = texture.value },
            size,
            uv0,
            uv1);
    }

    /// <summary>
    /// Attempts to capture ini settings without changing state when the operation cannot complete.
    /// </summary>
    /// <param name="settings">
    /// The validated configuration that controls this operation.
    /// </param>
    /// <param name="force">
    /// Whether force behavior is enabled while try capture ini settings executes.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the requested condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    public partial bool TryCaptureIniSettings(
        out string settings,
        bool force
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        ImGuiNative.Raw.SetCurrentContext(m_context);
        ImGuiIO* io = ImGuiNative.Raw.GetIO();
        if (!force && io->WantSaveIniSettings == 0)
        {
            settings = string.Empty;
            return false;
        }

        settings = Marshal.PtrToStringUTF8((nint)ImGuiNative.Raw.SaveIniSettingsToMemory(null)) ?? string.Empty;
        io->WantSaveIniSettings = 0;
        return true;
    }

    private static void ConfigureKeyRepeat(ImGuiIO* io)
    {
        io->KeyRepeatDelay = DEFAULT_KEY_REPEAT_DELAY_SECONDS;
        io->KeyRepeatRate = DEFAULT_KEY_REPEAT_RATE_SECONDS;
    }

    /// <summary>
    /// Registers a named font style and its glyph ranges in the active ImGui font atlas.
    /// </summary>
    /// <param name="style">
    /// The style consumed by register font style; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="filePath">
    /// The file path text validated by the register font style operation.
    /// </param>
    /// <param name="fontSizePixels">
    /// The font size pixels consumed by register font style; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public partial void RegisterFontStyle(
        ImGuiFontStyle style,
        string filePath,
        float fontSizePixels
    ) {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_hasStartedFrame)
            throw new InvalidOperationException("Font styles must be registered before the first ImGui frame.");
        ImGuiFont.ValidateStyle(style);
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        if (!float.IsFinite(fontSizePixels) || fontSizePixels <= 0f)
            throw new ArgumentOutOfRangeException(nameof(fontSizePixels));

        string fullPath = Path.GetFullPath(filePath);
        if (!File.Exists(fullPath))
            throw new FileNotFoundException("The font file does not exist.", fullPath);

        ImGuiNative.Raw.SetCurrentContext(m_context);
        ImGuiIO* io = ImGuiNative.Raw.GetIO();
        ImFont* loadedFont = LoadFont(io->Fonts, fullPath, fontSizePixels);
        if (loadedFont == null)
            throw new InvalidOperationException($"Could not load the font '{fullPath}'.");

        ResolveFontPaths(out _, out List<string> iconFontPaths);
        MergeIconFontsIntoCurrentBaseFont(io->Fonts, iconFontPaths);
        ImGuiFont.RegisterStyle(m_context, style, (nint)loadedFont);
        if (style == ImGuiFontStyle.Regular)
            io->FontDefault = loadedFont;
    }

    private static void ConfigureFonts(
        ImGuiNativeContext* context,
        ImGuiIO* io
    ) {
        var fonts = io->Fonts;
        ImGuiNative.Raw.Clear(fonts);

        ResolveFontPaths(out var fontPaths, out var iconFontPaths);
        var styleFonts = new Dictionary<ImGuiFontStyle, nint>();
        ImFont* defaultFont = null;
        var preferredIndex = FindPreferredFontIndex(fontPaths);
        for (var i = 0; i < fontPaths.Count; i++)
        {
            ImFont* loadedFont = LoadFont(fonts, fontPaths[i], DEFAULT_FONT_SIZE_PIXELS);
            if (loadedFont == null)
            {
                continue;
            }

            MergeIconFontsIntoCurrentBaseFont(fonts, iconFontPaths);
            styleFonts[GetFontStyle(fontPaths[i])] = (nint)loadedFont;
            if (i == preferredIndex)
            {
                defaultFont = loadedFont;
            }
            else if (defaultFont == null)
            {
                defaultFont = loadedFont;
            }
        }

        if (defaultFont == null)
        {
            defaultFont = LoadDefaultFont(fonts);
            if (defaultFont != null)
            {
                MergeIconFontsIntoCurrentBaseFont(fonts, iconFontPaths);
            }
        }

        io->FontDefault = defaultFont;
        if (defaultFont != null && !styleFonts.ContainsKey(ImGuiFontStyle.Regular))
            styleFonts[ImGuiFontStyle.Regular] = (nint)defaultFont;
        ImGuiFont.RegisterContext(context, styleFonts);
    }

    private static ImGuiFontStyle GetFontStyle(string fontPath)
    {
        string name = Path.GetFileNameWithoutExtension(fontPath);
        bool bold = name.Contains("Bold", StringComparison.OrdinalIgnoreCase);
        bool italic = name.Contains("Italic", StringComparison.OrdinalIgnoreCase);
        return (bold ? ImGuiFontStyle.Bold : ImGuiFontStyle.Regular) |
               (italic ? ImGuiFontStyle.Italic : ImGuiFontStyle.Regular);
    }

    private static int FindPreferredFontIndex(List<string> fontPaths)
    {
        if (fontPaths.Count == 0 || string.IsNullOrWhiteSpace(DEFAULT_FONT_FILE_NAME))
        {
            return -1;
        }

        for (var i = 0; i < fontPaths.Count; i++)
        {
            var fileName = Path.GetFileName(fontPaths[i]);
            if (fileName.Equals(DEFAULT_FONT_FILE_NAME, StringComparison.OrdinalIgnoreCase))
            {
                return i;
            }
        }

        return -1;
    }

    private static void ResolveFontPaths(
        out List<string> baseFonts,
        out List<string> iconFonts
    ) {
        baseFonts = new List<string>();
        iconFonts = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var fontDirectory in ResolveAssetDirectories(DEFAULT_FONTS_DIRECTORY_RELATIVE_PATH))
        {
            CollectFontFiles(fontDirectory, baseFonts, seen, includeSubdirectories: false);
        }

        foreach (var iconDirectory in ResolveAssetDirectories(DEFAULT_ICONS_DIRECTORY_RELATIVE_PATH))
        {
            CollectFontFiles(iconDirectory, iconFonts, seen, includeSubdirectories: false);
        }

        baseFonts.Sort(StringComparer.OrdinalIgnoreCase);
        iconFonts.Sort(StringComparer.OrdinalIgnoreCase);
    }

    private static List<string> ResolveAssetDirectories(string relativePath)
    {
        var directories = new List<string>();
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var candidates = new[]
        {
            Path.Combine(AppContext.BaseDirectory, relativePath),
            Path.Combine(Path.GetDirectoryName(typeof(PlatformImGuiContext).Assembly.Location) ?? string.Empty, relativePath)
        };

        foreach (var candidate in candidates)
        {
            if (string.IsNullOrWhiteSpace(candidate))
            {
                continue;
            }

            var fullPath = Path.GetFullPath(candidate);
            if (Directory.Exists(fullPath) && seen.Add(fullPath))
            {
                directories.Add(fullPath);
            }
        }

        return directories;
    }

    private static void CollectFontFiles(
        string directory,
        List<string> output,
        HashSet<string> seen,
        bool includeSubdirectories
    ) {
        var searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
        var fontFiles = Directory.GetFiles(directory, "*.*", searchOption);
        foreach (var fontFile in fontFiles)
        {
            var extension = Path.GetExtension(fontFile);
            if (!extension.Equals(".ttf", StringComparison.OrdinalIgnoreCase)
                && !extension.Equals(".otf", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }

            var fullPath = Path.GetFullPath(fontFile);
            if (seen.Add(fullPath))
            {
                output.Add(fullPath);
            }
        }
    }

    private static unsafe void MergeIconFontsIntoCurrentBaseFont(
        ImFontAtlas* fonts,
        List<string> iconFontPaths
    ) {
        if (iconFontPaths.Count == 0)
        {
            return;
        }

        for (var index = 0; index < iconFontPaths.Count; index++)
        {
            ImFontConfig* mergeConfig = ImGuiNative.Raw.ImFontConfig();
            mergeConfig->MergeMode = 1;
            mergeConfig->PixelSnapH = 1;
            try
            {
                _ = LoadFont(fonts, iconFontPaths[index], DEFAULT_ICON_SIZE_PIXELS, mergeConfig);
            }
            finally
            {
                ImGuiNative.Raw.Destroy(mergeConfig);
            }
        }
    }

    internal void ProcessEvent(ref SDLEvent sdlEvent)
    {
        if (m_disposed)
        {
            return;
        }

        ImGuiNative.Raw.SetCurrentContext(m_context);
        var io = ImGuiNative.Raw.GetIO();
        var eventType = (SDLEventType)sdlEvent.Type;

        if (!TryGetWindowId(ref sdlEvent, out var eventWindowId))
        {
            m_viewports?.ProcessEvent(ref sdlEvent, 0);
            return;
        }

        if (eventWindowId != m_window.windowId && (m_viewports == null || !m_viewports.OwnsWindow(eventWindowId)))
        {
            return;
        }

        if (m_enableSmoothResize)
            RefreshLiveResizeHoverLock(eventType, eventWindowId, ref sdlEvent);

        m_viewports?.ProcessEvent(ref sdlEvent, eventWindowId);

        switch (eventType)
        {
            case SDLEventType.KeyDown:
            case SDLEventType.KeyUp:
            {
                var down = eventType == SDLEventType.KeyDown;
                if (down && sdlEvent.Key.Repeat != 0)
                {
                    // Keep key-repeat timing driven by ImGui (io->KeyRepeatDelay/KeyRepeatRate).
                    break;
                }

                UpdateKeyModifiers(io, (SDLKeymod)sdlEvent.Key.Mod);

                var key = TranslateKey(sdlEvent.Key.Scancode);
                if (key != ImGuiKey.None)
                {
                    ImGuiNative.Raw.AddKeyEvent(io, key, down ? (byte)1 : (byte)0);
                    ImGuiNative.Raw.SetKeyEventNativeData(io, key, (int)sdlEvent.Key.Key, (int)sdlEvent.Key.Scancode, (int)sdlEvent.Key.Scancode);
                }
                break;
            }

            case SDLEventType.TextInput:
            {
                if (sdlEvent.Text.Text != null)
                    ImGuiNative.Raw.AddInputCharactersUTF8(io, sdlEvent.Text.Text);
                break;
            }

            case SDLEventType.MouseMotion:
            {
                m_mouseWindowId = eventWindowId;
                m_mousePendingLeaveFrame = 0;
                m_mousePendingLeaveWindowId = 0;
                Vector2 mousePosition = GetEventMousePosition(
                    io,
                    eventWindowId,
                    sdlEvent.Motion.X,
                    sdlEvent.Motion.Y);
                ImGuiNative.Raw.AddMousePosEvent(io, mousePosition.X, mousePosition.Y);
                break;
            }

            case SDLEventType.MouseButtonDown:
            case SDLEventType.MouseButtonUp:
            {
                bool down = eventType == SDLEventType.MouseButtonDown;
                if (TryTranslateMouseButton(sdlEvent.Button.Button, out int button))
                {
                    int mask = 1 << button;
                    if (down)
                        m_mouseButtonsDown |= mask;
                    else
                        m_mouseButtonsDown &= ~mask;
                    ImGuiNative.Raw.AddMouseButtonEvent(io, button, down ? (byte)1 : (byte)0);
                    // Docking can destroy the source viewport before SDL emits MouseUp. Capturing
                    // the mouse keeps the complete press/release sequence inside this application.
                    _ = SDL.CaptureMouse(m_mouseButtonsDown != 0);
                }
                break;
            }

            case SDLEventType.MouseWheel:
            {
                var wheelX = sdlEvent.Wheel.X;
                var wheelY = sdlEvent.Wheel.Y;

                wheelX *= m_interaction.horizontalWheelDirection;

                ImGuiNative.Raw.AddMouseWheelEvent(io,
                    wheelX * DEFAULT_MOUSE_SCROLLING_UNITS,
                    wheelY * DEFAULT_MOUSE_SCROLLING_UNITS);
                break;
            }

            case SDLEventType.WindowFocusGained:
                ImGuiNative.Raw.AddFocusEvent(io, 1);
                break;

            case SDLEventType.WindowFocusLost:
                ImGuiNative.Raw.AddFocusEvent(io, 0);
                break;

            case SDLEventType.WindowMouseEnter:
                m_mouseWindowId = eventWindowId;
                m_mousePendingLeaveFrame = 0;
                m_mousePendingLeaveWindowId = 0;
                break;

            case SDLEventType.WindowMouseLeave:
                // SDL may deliver Leave before Enter while crossing viewport windows. Defer the
                // reset so a matching Enter or Motion event can cancel it without breaking drag.
                if (m_mouseWindowId == eventWindowId)
                {
                    m_mousePendingLeaveFrame = ImGuiNative.GetFrameCount() + 1;
                    m_mousePendingLeaveWindowId = eventWindowId;
                }
                break;

            case SDLEventType.WindowDisplayChanged:
            case SDLEventType.WindowDisplayScaleChanged:
            case SDLEventType.WindowResized:
            case SDLEventType.WindowPixelSizeChanged:
                m_viewports?.SynchronizeWindow(eventWindowId);
                UpdateDisplayMetrics(io);
                break;

            case SDLEventType.WindowExposed:
                UpdateDisplayMetrics(io);
                break;
        }
    }

    internal void PrepareLiveResizeWindow(uint windowId)
    {
        if (m_disposed || !m_enableSmoothResize)
        {
            return;
        }

        var ownsMainWindow = windowId == m_window.windowId;
        var ownsViewportWindow = m_viewports?.OwnsWindow(windowId) == true;
        if (!ownsMainWindow && !ownsViewportWindow)
        {
            return;
        }

        m_pendingLiveResizeWindowIds.Add(windowId);
        var now = m_frameTimer.Elapsed;
        m_liveResizeLockedWindowId = windowId;
        m_lastLiveResizeLockTime = now;
    }

    /// <summary>
    /// Records frame rendering for the current frame.
    /// </summary>
    /// <param name="drawFrame">
    /// The callback invoked by render frame within the operation's owned lifetime.
    /// </param>
    /// <param name="prepareFrame">
    /// Optional callback invoked with the owned context before native frame creation.
    /// </param>
    /// <returns>
    /// Borrowed native draw data for the completed frame, valid until the next frame or retirement.
    /// </returns>
    public partial IntPtr RenderFrame(
        Action drawFrame,
        Action? prepareFrame
    ) {
        ArgumentNullException.ThrowIfNull(drawFrame);
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_isFrameOperationActive || m_isFrameActive)
            throw new InvalidOperationException("An ImGui frame cannot be reentered.");

        var now = m_frameTimer.Elapsed;
        var deltaSeconds = (float)(now - m_lastFrameTime).TotalSeconds;
        m_lastFrameTime = now;

        m_isFrameOperationActive = true;
        try
        {
            SynchronizePendingLiveResizeWindows();
            BeginFrame(deltaSeconds, prepareFrame);
            drawFrame();
            return EndFrame();
        }
        catch (Exception failure)
        {
            try
            {
                AbortNativeFrame();
            }
            catch (Exception cleanupFailure)
            {
                throw new AggregateException("Presentation failed and its native frame could not retire.", failure, cleanupFailure);
            }
            throw;
        }
        finally
        {
            m_isFrameActive = false;
            m_isFrameOperationActive = false;
        }
    }

    private void AbortNativeFrame()
    {
        if (m_disposed || m_context->WithinFrameScope == 0)
            return;
        ImGuiNativeContext* previous = ImGuiNative.Raw.GetCurrentContext();
        ImGuiNative.Raw.SetCurrentContext(m_context);
        try
        {
            ImGuiNative.Raw.EndFrame();
        }
        finally
        {
            if (previous != m_context)
                ImGuiNative.Raw.SetCurrentContext(previous);
        }
    }

    private void BeginFrame(
        float deltaTimeSeconds,
        Action? prepareFrame
    ) {
        if (m_disposed)
        {
            throw new ObjectDisposedException(nameof(PlatformImGuiContext));
        }

        ImGuiNative.Raw.SetCurrentContext(m_context);
        m_viewports?.ProcessRetirements();
        m_hasStartedFrame = true;

        var io = ImGuiNative.Raw.GetIO();
        io->MouseDrawCursor = 0;
        UpdateDisplayMetrics(io);
        io->DeltaTime = deltaTimeSeconds > 0f ? deltaTimeSeconds : (1f / 60f);
        ReconcileMouseButtons(io);
        ReleaseCompletedLiveResizeHoverLock();
        FlushPendingMouseLeave(io);
        UpdateMouseData(io, m_window.GetSdlWindow());
        UpdateTextInputState(io);

        m_viewports?.SynchronizeMetrics();
        prepareFrame?.Invoke();
        m_isFrameActive = true;
        ImGuiNative.NewFrame();

        // During live resize, prevent the main viewport from hosting other windows.
        var mainViewport = ImGuiNative.Raw.GetMainViewport();
        if (m_liveResizeLockedWindowId != 0
            && (io->ConfigFlags & (int)ImGuiConfigFlags.ViewportsEnable) != 0
            && mainViewport != null)
        {
            mainViewport->Flags &= ~(int)ImGuiViewportFlags.CanHostOtherWindows;
        }
    }

    private IntPtr EndFrame()
    {
        if (m_disposed)
        {
            throw new ObjectDisposedException(nameof(PlatformImGuiContext));
        }

        var io = ImGuiNative.Raw.GetIO();

        try
        {
            ImGuiNative.Raw.SetCurrentContext(m_context);
            UpdateMouseCursor(io);
            ImGuiNative.SetMouseCursor((int)ImGuiMouseCursor.None);
            ImGuiNative.Render();

            io->MouseDrawCursor = 0;
            var drawData = ImGuiNative.Raw.GetDrawData();
            m_renderer.RenderMain(GetDrawDataAddress(drawData));
            if ((io->ConfigFlags & (int)ImGuiConfigFlags.ViewportsEnable) != 0)
            {
                ImGuiNative.UpdatePlatformWindows();
                ImGuiNative.Raw.RenderPlatformWindowsDefault(null, null);
                ImGuiNative.Raw.SetCurrentContext(m_context);
            }

            return GetDrawDataAddress(drawData);
        }
        finally
        {
            m_isFrameActive = false;
        }
    }

    private void RefreshLiveResizeHoverLock(
        SDLEventType eventType,
        uint eventWindowId,
        ref SDLEvent sdlEvent
    ) {
        if (m_liveResizeLockedWindowId == 0)
        {
            return;
        }

        float mouseX = 0f;
        float mouseY = 0f;
        var mouseButtons = ((SDLMouseButtonFlags)SDL.Raw.GetMouseState(&mouseX, &mouseY));
        if ((mouseButtons & SDLMouseButtonFlags.Left) == 0)
        {
            m_liveResizeLockedWindowId = 0;
            return;
        }

        var now = m_frameTimer.Elapsed;
        if (eventType == SDLEventType.WindowExposed && eventWindowId == m_liveResizeLockedWindowId)
        {
            m_lastLiveResizeLockTime = now;
        }

        if ((now - m_lastLiveResizeLockTime).TotalSeconds > LIVE_RESIZE_HOVER_LOCK_TIMEOUT_SECONDS)
        {
            m_liveResizeLockedWindowId = 0;
            return;
        }

        if (eventType == SDLEventType.WindowExposed
            && eventWindowId == m_liveResizeLockedWindowId
            && sdlEvent.Window.Data1 == 0)
        {
            m_liveResizeLockedWindowId = 0;
            return;
        }

        if (eventType == SDLEventType.MouseButtonUp && sdlEvent.Button.Button == SDL.Constants.SDL_BUTTON_LEFT)
        {
            m_liveResizeLockedWindowId = 0;
        }
    }

    /// <summary>
    /// Retires the context and its owned resources after the current frame operation has completed.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// A frame operation still owns the context; the caller must retry after that operation completes.
    /// </exception>
    public partial void Dispose()
    {
        if (m_disposed)
        {
            return;
        }

        if (m_isFrameOperationActive)
            throw new InvalidOperationException("A presentation context cannot retire during its frame operation.");

        ImGuiNative.Raw.SetCurrentContext(m_context);
        var io = ImGuiNative.Raw.GetIO();
        UnregisterClipboardCallbacks();
        if (m_textInputActive)
        {
            var textInputWindow = m_textInputWindow == null ? m_window.GetSdlWindow() : m_textInputWindow;
            _ = SDL.Raw.StopTextInput(textInputWindow);
            m_textInputWindow = null;
            m_textInputActive = false;
        }

        _ = SDL.CaptureMouse(false);
        m_mouseButtonsDown = 0;
        m_mouseWindowId = 0;
        m_mousePendingLeaveFrame = 0;
        m_pendingLiveResizeWindowIds.Clear();

        if ((io->ConfigFlags & (int)ImGuiConfigFlags.ViewportsEnable) != 0)
        {
            ImGuiNative.DestroyPlatformWindows();
        }

        m_viewports?.Dispose();
        m_renderer.DrainViewportRetirements();

        foreach (var cursor in m_cursors.Values)
        {
            if (cursor != 0)
            {
                SDL.Raw.DestroyCursor((SDLCursor*)cursor);
            }
        }

        m_cursors.Clear();
        m_renderer.Dispose();
        ImGuiFont.UnregisterContext(m_context);
        ImGuiNative.Raw.DestroyContext(m_context);
        if (m_iniFilename != IntPtr.Zero)
        {
            Marshal.FreeCoTaskMem(m_iniFilename);
            m_iniFilename = IntPtr.Zero;
        }
        m_disposed = true;
    }

    private void UpdateDisplayMetrics(ImGuiIO* io)
    {
        var windowWidth = 0;
        var windowHeight = 0;
        SDLWindow* sdlWindow = m_window.GetSdlWindow();
        SDL.Raw.GetWindowSize(sdlWindow, &windowWidth, &windowHeight);

        var pixelWidth = 0;
        var pixelHeight = 0;
        SDL.Raw.GetWindowSizeInPixels(sdlWindow, &pixelWidth, &pixelHeight);

        io->DisplaySize = new ImVec2C { X = windowWidth, Y = windowHeight };

        if (windowWidth > 0 && windowHeight > 0)
        {
            io->DisplayFramebufferScale = new ImVec2C
            {
                X = pixelWidth / (float)windowWidth,
                Y = pixelHeight / (float)windowHeight
            };
        }
        else
        {
            io->DisplayFramebufferScale = new ImVec2C { X = 1f, Y = 1f };
        }
    }

    private void UpdateTextInputState(ImGuiIO* io)
    {
        var targetWindow = ResolveTextInputWindow();
        if (io->WantTextInput != 0)
        {
            if (!m_textInputActive || m_textInputWindow != targetWindow)
            {
                if (m_textInputActive && m_textInputWindow != null)
                {
                    _ = SDL.Raw.StopTextInput(m_textInputWindow);
                }

                _ = SDL.Raw.StartTextInputWithProperties(targetWindow, 0);
                m_textInputWindow = targetWindow;
                m_textInputActive = true;
            }
        }
        else if (m_textInputActive)
        {
            var textInputWindow = m_textInputWindow == null ? targetWindow : m_textInputWindow;
            _ = SDL.Raw.StopTextInput(textInputWindow);
            m_textInputWindow = null;
            m_textInputActive = false;
        }
    }

    private static unsafe void SetIniFilename(
        ImGuiIO* io,
        IntPtr fileName
    ) {
        io->IniFilename = (sbyte*)fileName;
    }

    private static unsafe ImFont* LoadFont(
        ImFontAtlas* fonts,
        string filePath,
        float fontSizePixels,
        ImFontConfig* config = null
    ) {
        int length = Encoding.UTF8.GetByteCount(filePath);
        Span<byte> encoded = length < 1024 ? stackalloc byte[1024] : new byte[checked(length + 1)];
        Encoding.UTF8.GetBytes(filePath, encoded);
        encoded[length] = 0;
        fixed (byte* filename = encoded)
            return ImGuiNative.Raw.AddFontFromFileTTF(fonts, (sbyte*)filename, fontSizePixels, config, null);
    }

    private static unsafe ImFont* LoadDefaultFont(ImFontAtlas* fonts) => ImGuiNative.Raw.AddFontDefault(fonts, null);

    private static unsafe IntPtr GetDrawDataAddress(ImDrawData* drawData) => (nint)drawData;

    private SDLWindow* ResolveTextInputWindow()
    {
        var keyboardFocus = SDL.Raw.GetKeyboardFocus();
        if (keyboardFocus == null)
        {
            return m_window.GetSdlWindow();
        }

        var focusedWindowId = SDL.Raw.GetWindowId(keyboardFocus);
        if (focusedWindowId == m_window.windowId || (m_viewports != null && m_viewports.OwnsWindow(focusedWindowId)))
        {
            return keyboardFocus;
        }

        return m_window.GetSdlWindow();
    }

    private static void UpdateMouseData(
        ImGuiIO* io,
        SDLWindow* window
    ) {
        // Mouse position is fed from SDL mouse events (per-window coordinates).
        // Polling here would overwrite secondary viewport coordinates with the wrong window space.
        if (io->WantSetMousePos != 0)
        {
            SDL.Raw.WarpMouseInWindow(window, io->MousePos.X, io->MousePos.Y);
        }
    }

    private void SynchronizePendingLiveResizeWindows()
    {
        if (m_pendingLiveResizeWindowIds.Count == 0)
        {
            return;
        }

        uint[] windowIds = [.. m_pendingLiveResizeWindowIds];
        m_pendingLiveResizeWindowIds.Clear();
        foreach (uint windowId in windowIds)
        {
            if (windowId == m_window.windowId)
            {
                int pixelWidth = 0;
                int pixelHeight = 0;
                SDL.Raw.GetWindowSizeInPixels(m_window.GetSdlWindow(), &pixelWidth, &pixelHeight);
                m_renderer.SynchronizeMainOutput(pixelWidth, pixelHeight);
            }
            else
                m_viewports?.SynchronizeWindow(windowId);
        }
    }

    private void ReconcileMouseButtons(ImGuiIO* io)
    {
        if (m_mouseButtonsDown == 0)
            return;

        float mouseX = 0f;
        float mouseY = 0f;
        SDLMouseButtonFlags currentButtons = ((SDLMouseButtonFlags)SDL.Raw.GetMouseState(&mouseX, &mouseY));
        ReadOnlySpan<byte> sdlButtons =
        [
            SDL.Constants.SDL_BUTTON_LEFT,
            SDL.Constants.SDL_BUTTON_RIGHT,
            SDL.Constants.SDL_BUTTON_MIDDLE,
            SDL.Constants.SDL_BUTTON_X1,
            SDL.Constants.SDL_BUTTON_X2
        ];
        for (int button = 0; button < sdlButtons.Length; button++)
        {
            int trackedMask = 1 << button;
            if ((m_mouseButtonsDown & trackedMask) == 0)
                continue;

            SDLMouseButtonFlags sdlMask = (SDLMouseButtonFlags)(1u << (sdlButtons[button] - 1));
            if ((currentButtons & sdlMask) != 0)
                continue;

            // Recover when a destroyed viewport prevented the corresponding MouseUp event from
            // reaching the event queue. Without this release ImGui keeps its active drag forever.
            m_mouseButtonsDown &= ~trackedMask;
            ImGuiNative.Raw.AddMouseButtonEvent(io, button, 0);
        }

        if (m_mouseButtonsDown == 0)
            _ = SDL.CaptureMouse(false);
    }

    private static Vector2 GetEventMousePosition(
        ImGuiIO* io,
        uint windowId,
        float localX,
        float localY
    ) {
        Vector2 position = new(localX, localY);
        if ((io->ConfigFlags & (int)ImGuiConfigFlags.ViewportsEnable) == 0)
        {
            return position;
        }

        SDLWindow* window = SDL.Raw.GetWindowFromId(windowId);
        if (window == null)
        {
            return position;
        }

        var windowX = 0;
        var windowY = 0;
        _ = SDL.Raw.GetWindowPosition(window, &windowX, &windowY);
        return position + new Vector2(windowX, windowY);
    }

    private void FlushPendingMouseLeave(ImGuiIO* io)
    {
        if (!ShouldFlushPendingMouseLeave(
                m_mousePendingLeaveFrame,
                ImGuiNative.GetFrameCount(),
                m_mouseButtonsDown,
                m_mousePendingLeaveWindowId,
                m_liveResizeLockedWindowId))
        {
            return;
        }

        m_mouseWindowId = 0;
        m_mousePendingLeaveFrame = 0;
        m_mousePendingLeaveWindowId = 0;
        ImGuiNative.Raw.AddMousePosEvent(io, -float.MaxValue, -float.MaxValue);
    }

    private void ReleaseCompletedLiveResizeHoverLock()
    {
        if (m_liveResizeLockedWindowId == 0 ||
            (m_frameTimer.Elapsed - m_lastLiveResizeLockTime).TotalSeconds <=
            LIVE_RESIZE_HOVER_LOCK_TIMEOUT_SECONDS)
        {
            return;
        }

        float mouseX = 0f;
        float mouseY = 0f;
        SDLMouseButtonFlags mouseButtons = ((SDLMouseButtonFlags)SDL.Raw.GetMouseState(&mouseX, &mouseY));
        if ((mouseButtons & SDLMouseButtonFlags.Left) == 0)
            m_liveResizeLockedWindowId = 0;
    }

    private static bool ShouldFlushPendingMouseLeave(
        int pendingFrame,
        int currentFrame,
        int mouseButtonsDown,
        uint pendingWindowId,
        uint liveResizeLockedWindowId
    ) {
        if (pendingFrame == 0 || mouseButtonsDown != 0 || currentFrame < pendingFrame)
            return false;

        return liveResizeLockedWindowId == 0 || pendingWindowId != liveResizeLockedWindowId;
    }

    private void UpdateMouseCursor(ImGuiIO* io)
    {
        if ((io->ConfigFlags & (int)ImGuiConfigFlags.NoMouseCursorChange) != 0)
        {
            return;
        }

        var imguiCursor = (ImGuiMouseCursor)ImGuiNative.GetMouseCursor();
        if (imguiCursor == ImGuiMouseCursor.None || io->MouseDrawCursor != 0)
        {
            _ = SDL.HideCursor();
            m_currentCursor = ImGuiMouseCursor.None;
            return;
        }

        var sdlCursor = GetOrCreateCursor(imguiCursor);
        if (sdlCursor != null && m_currentCursor != imguiCursor)
        {
            _ = SDL.Raw.SetCursor(sdlCursor);
            m_currentCursor = imguiCursor;
        }

        _ = SDL.ShowCursor();
    }

    private SDLCursor* GetOrCreateCursor(ImGuiMouseCursor cursor)
    {
        if (m_cursors.TryGetValue(cursor, out var cachedCursor))
        {
            return (SDLCursor*)cachedCursor;
        }

        var sdlCursorType = cursor switch
        {
            ImGuiMouseCursor.Arrow => SDLSystemCursor.Default,
            ImGuiMouseCursor.TextInput => SDLSystemCursor.Text,
            ImGuiMouseCursor.ResizeAll => SDLSystemCursor.Move,
            ImGuiMouseCursor.ResizeNs => SDLSystemCursor.NsResize,
            ImGuiMouseCursor.ResizeEw => SDLSystemCursor.EwResize,
            ImGuiMouseCursor.ResizeNesw => SDLSystemCursor.NeswResize,
            ImGuiMouseCursor.ResizeNwse => SDLSystemCursor.NwseResize,
            ImGuiMouseCursor.Hand => SDLSystemCursor.Pointer,
            ImGuiMouseCursor.Wait => SDLSystemCursor.Wait,
            ImGuiMouseCursor.Progress => SDLSystemCursor.Progress,
            ImGuiMouseCursor.NotAllowed => SDLSystemCursor.NotAllowed,
            _ => SDLSystemCursor.Default
        };

        var createdCursor = SDL.Raw.CreateSystemCursor(sdlCursorType);
        m_cursors[cursor] = (nint)createdCursor;
        return createdCursor;
    }

    private static bool TryTranslateMouseButton(
        byte sdlButton,
        out int imguiButton
    ) {
        switch (sdlButton)
        {
            case SDL.Constants.SDL_BUTTON_LEFT:
                imguiButton = 0;
                return true;
            case SDL.Constants.SDL_BUTTON_RIGHT:
                imguiButton = 1;
                return true;
            case SDL.Constants.SDL_BUTTON_MIDDLE:
                imguiButton = 2;
                return true;
            case SDL.Constants.SDL_BUTTON_X1:
                imguiButton = 3;
                return true;
            case SDL.Constants.SDL_BUTTON_X2:
                imguiButton = 4;
                return true;
            default:
                imguiButton = 0;
                return false;
        }
    }

    private static void UpdateKeyModifiers(
        ImGuiIO* io,
        SDLKeymod modifiers
    ) {
        ImGuiNative.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModCtrl, (modifiers & SDLKeymod.Ctrl) != 0 ? (byte)1 : (byte)0);
        ImGuiNative.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModShift, (modifiers & SDLKeymod.Shift) != 0 ? (byte)1 : (byte)0);
        ImGuiNative.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModAlt, (modifiers & SDLKeymod.Alt) != 0 ? (byte)1 : (byte)0);
        ImGuiNative.Raw.AddKeyEvent(io, ImGuiKey.ImGuiModSuper, (modifiers & SDLKeymod.Gui) != 0 ? (byte)1 : (byte)0);
    }

    private static ImGuiKey TranslateKey(SDLScancode scancode)
    {
        return scancode switch
        {
            SDLScancode.Tab => ImGuiKey.Tab,
            SDLScancode.Left => ImGuiKey.LeftArrow,
            SDLScancode.Right => ImGuiKey.RightArrow,
            SDLScancode.Up => ImGuiKey.UpArrow,
            SDLScancode.Down => ImGuiKey.DownArrow,
            SDLScancode.Pageup => ImGuiKey.PageUp,
            SDLScancode.Pagedown => ImGuiKey.PageDown,
            SDLScancode.Home => ImGuiKey.Home,
            SDLScancode.End => ImGuiKey.End,
            SDLScancode.Insert => ImGuiKey.Insert,
            SDLScancode.Delete => ImGuiKey.Delete,
            SDLScancode.Backspace => ImGuiKey.Backspace,
            SDLScancode.Space => ImGuiKey.Space,
            SDLScancode.Return => ImGuiKey.Enter,
            SDLScancode.Escape => ImGuiKey.Escape,
            SDLScancode.Apostrophe => ImGuiKey.Apostrophe,
            SDLScancode.Comma => ImGuiKey.Comma,
            SDLScancode.Minus => ImGuiKey.Minus,
            SDLScancode.Period => ImGuiKey.Period,
            SDLScancode.Slash => ImGuiKey.Slash,
            SDLScancode.Semicolon => ImGuiKey.Semicolon,
            SDLScancode.Equals => ImGuiKey.Equal,
            SDLScancode.Leftbracket => ImGuiKey.LeftBracket,
            SDLScancode.Backslash => ImGuiKey.Backslash,
            SDLScancode.Rightbracket => ImGuiKey.RightBracket,
            SDLScancode.Grave => ImGuiKey.GraveAccent,
            SDLScancode.Capslock => ImGuiKey.CapsLock,
            SDLScancode.Scrolllock => ImGuiKey.ScrollLock,
            SDLScancode.Numlockclear => ImGuiKey.NumLock,
            SDLScancode.Printscreen => ImGuiKey.PrintScreen,
            SDLScancode.Pause => ImGuiKey.Pause,
            SDLScancode.Kp0 => ImGuiKey.Keypad0,
            SDLScancode.Kp1 => ImGuiKey.Keypad1,
            SDLScancode.Kp2 => ImGuiKey.Keypad2,
            SDLScancode.Kp3 => ImGuiKey.Keypad3,
            SDLScancode.Kp4 => ImGuiKey.Keypad4,
            SDLScancode.Kp5 => ImGuiKey.Keypad5,
            SDLScancode.Kp6 => ImGuiKey.Keypad6,
            SDLScancode.Kp7 => ImGuiKey.Keypad7,
            SDLScancode.Kp8 => ImGuiKey.Keypad8,
            SDLScancode.Kp9 => ImGuiKey.Keypad9,
            SDLScancode.KpPeriod => ImGuiKey.KeypadDecimal,
            SDLScancode.KpDivide => ImGuiKey.KeypadDivide,
            SDLScancode.KpMultiply => ImGuiKey.KeypadMultiply,
            SDLScancode.KpMinus => ImGuiKey.KeypadSubtract,
            SDLScancode.KpPlus => ImGuiKey.KeypadAdd,
            SDLScancode.KpEnter => ImGuiKey.KeypadEnter,
            SDLScancode.KpEquals => ImGuiKey.KeypadEqual,
            SDLScancode.Lctrl => ImGuiKey.LeftCtrl,
            SDLScancode.Lshift => ImGuiKey.LeftShift,
            SDLScancode.Lalt => ImGuiKey.LeftAlt,
            SDLScancode.Lgui => ImGuiKey.LeftSuper,
            SDLScancode.Rctrl => ImGuiKey.RightCtrl,
            SDLScancode.Rshift => ImGuiKey.RightShift,
            SDLScancode.Ralt => ImGuiKey.RightAlt,
            SDLScancode.Rgui => ImGuiKey.RightSuper,
            SDLScancode.Menu => ImGuiKey.Menu,
            SDLScancode._0 => ImGuiKey._0,
            SDLScancode._1 => ImGuiKey._1,
            SDLScancode._2 => ImGuiKey._2,
            SDLScancode._3 => ImGuiKey._3,
            SDLScancode._4 => ImGuiKey._4,
            SDLScancode._5 => ImGuiKey._5,
            SDLScancode._6 => ImGuiKey._6,
            SDLScancode._7 => ImGuiKey._7,
            SDLScancode._8 => ImGuiKey._8,
            SDLScancode._9 => ImGuiKey._9,
            SDLScancode.A => ImGuiKey.A,
            SDLScancode.B => ImGuiKey.B,
            SDLScancode.C => ImGuiKey.C,
            SDLScancode.D => ImGuiKey.D,
            SDLScancode.E => ImGuiKey.E,
            SDLScancode.F => ImGuiKey.F,
            SDLScancode.G => ImGuiKey.G,
            SDLScancode.H => ImGuiKey.H,
            SDLScancode.I => ImGuiKey.I,
            SDLScancode.J => ImGuiKey.J,
            SDLScancode.K => ImGuiKey.K,
            SDLScancode.L => ImGuiKey.L,
            SDLScancode.M => ImGuiKey.M,
            SDLScancode.N => ImGuiKey.N,
            SDLScancode.O => ImGuiKey.O,
            SDLScancode.P => ImGuiKey.P,
            SDLScancode.Q => ImGuiKey.Q,
            SDLScancode.R => ImGuiKey.R,
            SDLScancode.S => ImGuiKey.S,
            SDLScancode.T => ImGuiKey.T,
            SDLScancode.U => ImGuiKey.U,
            SDLScancode.V => ImGuiKey.V,
            SDLScancode.W => ImGuiKey.W,
            SDLScancode.X => ImGuiKey.X,
            SDLScancode.Y => ImGuiKey.Y,
            SDLScancode.Z => ImGuiKey.Z,
            SDLScancode.F1 => ImGuiKey.F1,
            SDLScancode.F2 => ImGuiKey.F2,
            SDLScancode.F3 => ImGuiKey.F3,
            SDLScancode.F4 => ImGuiKey.F4,
            SDLScancode.F5 => ImGuiKey.F5,
            SDLScancode.F6 => ImGuiKey.F6,
            SDLScancode.F7 => ImGuiKey.F7,
            SDLScancode.F8 => ImGuiKey.F8,
            SDLScancode.F9 => ImGuiKey.F9,
            SDLScancode.F10 => ImGuiKey.F10,
            SDLScancode.F11 => ImGuiKey.F11,
            SDLScancode.F12 => ImGuiKey.F12,
            _ => ImGuiKey.None
        };
    }

    private static bool TryGetWindowId(
        ref SDLEvent sdlEvent,
        out uint windowId
    ) {
        var eventType = (SDLEventType)sdlEvent.Type;
        switch (eventType)
        {
            case SDLEventType.KeyDown:
            case SDLEventType.KeyUp:
                windowId = sdlEvent.Key.WindowID;
                return true;

            case SDLEventType.TextInput:
                windowId = sdlEvent.Text.WindowID;
                return true;

            case SDLEventType.MouseMotion:
                windowId = sdlEvent.Motion.WindowID;
                return true;

            case SDLEventType.MouseButtonDown:
            case SDLEventType.MouseButtonUp:
                windowId = sdlEvent.Button.WindowID;
                return true;

            case SDLEventType.MouseWheel:
                windowId = sdlEvent.Wheel.WindowID;
                return true;

            case SDLEventType.WindowFocusGained:
            case SDLEventType.WindowFocusLost:
            case SDLEventType.WindowMouseEnter:
            case SDLEventType.WindowMouseLeave:
            case SDLEventType.WindowDisplayChanged:
            case SDLEventType.WindowDisplayScaleChanged:
            case SDLEventType.WindowExposed:
            case SDLEventType.WindowMoved:
            case SDLEventType.WindowResized:
            case SDLEventType.WindowPixelSizeChanged:
            case SDLEventType.WindowCloseRequested:
                windowId = sdlEvent.Window.WindowID;
                return true;

            default:
                windowId = 0;
                return false;
        }
    }

}
