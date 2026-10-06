using Inno.Adapter.Platform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Inno.Native.Bgfx;
using Inno.Platform;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Implements the sole BGFX device generation, API-thread frame boundary and graph backend.
/// </summary>
public sealed unsafe partial class BgfxDevice : RenderDevice, IRenderDevice, IRenderGraphBackend
{
    private const uint C_TRANSIENT_CACHE_RETENTION_FRAMES = 8;

    private static int s_nextGeneration;

    private readonly BgfxProcessDeviceLease m_processLease;
    private readonly int m_apiThreadId;
    private readonly int m_deferredDestroyFrames;
    private readonly bool m_hasPrimarySurface;
    private readonly Dictionary<ulong, bgfx.TextureHandle> m_persistentTextures = [];
    private readonly Dictionary<ulong, RenderTextureDescriptor> m_persistentTextureDescriptors = [];
    private readonly List<DeferredResource> m_deferredResources = [];
    private readonly Dictionary<ulong, PendingTextureReadback> m_textureReadbacks = [];
    private readonly Dictionary<int, bgfx.TextureHandle> m_graphTextures = [];
    private readonly Dictionary<int, bgfx.TextureHandle> m_transientTextureSlots = [];
    private readonly Dictionary<int, RenderTextureDescriptor> m_transientTextureSlotDescriptors = [];
    private readonly List<PooledTransientTexture> m_transientTexturePool = [];
    private readonly List<bgfx.FrameBufferHandle> m_graphFrameBuffers = [];
    private readonly List<CachedGraphFrameBuffer> m_graphFrameBufferCache = [];
    private uint m_resetFlags;
    private bool m_resetPending;

    private CompiledRenderGraph? m_activeGraph;
    private bgfx.Encoder m_activeEncoder;
    private ulong m_nextPersistentId = 1;
    private ulong m_nextReadbackId = 1;
    private uint m_backendFrame;
    private int m_activeGraphViewBase;
    private int m_backbufferWidth;
    private int m_backbufferHeight;
    private int m_nextViewId;
    private RenderPresentationSize? m_pendingPresentationSize;
    private bool m_presentationUpdatePending;
    private int m_drawCount;
    private int m_dispatchCount;
    private ulong m_transientTextureAllocationCount;
    private ulong m_transientFrameBufferAllocationCount;
    private bool m_frameOpen;
    private bool m_disposed;

    /// <summary>
    /// Initializes BGFX and captures immutable device capabilities.
    /// </summary>
    /// <param name="options">
    /// Backend-neutral initialization options.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// Thrown when another BGFX device is active or initialization fails.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The selected window does not expose the native surface SPI required by BGFX.
    /// </exception>
    public BgfxDevice(BgfxDeviceOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        PlatformNativeHandles nativeHandles = options.window is null ? default
            : options.window is INativeWindowSurface surface ? surface.nativeHandles
            : throw new NotSupportedException("BGFX requires a window that implements INativeWindowSurface.");
        m_processLease = BgfxProcessDeviceLease.Acquire(options.forceSingleThreaded);

        m_apiThreadId = Environment.CurrentManagedThreadId;
        m_deferredDestroyFrames = options.deferredDestroyFrames;
        m_hasPrimarySurface = options.window is not null;
        int pixelWidth = options.backbufferWidth;
        int pixelHeight = options.backbufferHeight;
        primaryPresentationSize = m_hasPrimarySurface && pixelWidth > 0 && pixelHeight > 0
            ? new RenderPresentationSize(pixelWidth, pixelHeight) : null;
        m_backbufferWidth = Math.Max(1, pixelWidth);
        m_backbufferHeight = Math.Max(1, pixelHeight);
        m_resetFlags = (uint)(
            (options.verticalSync ? bgfx.ResetFlags.Vsync : bgfx.ResetFlags.None)
            | (options.sRgbBackbuffer && nativeHandles.handleKind != PlatformNativeHandleId.browserCanvas
                ? bgfx.ResetFlags.SrgbBackbuffer
                : bgfx.ResetFlags.None));

        bool nativeInitialized = false;
        try
        {
            if (options.forceSingleThreaded)
            {
                bgfx.render_frame(0);
            }

            bgfx.Init init;
            bgfx.init_ctor(&init);
            if (options.preferredBackend.HasValue)
            {
                init.type = BgfxCapabilityMapper.ToNativeRenderer(options.preferredBackend.Value);
            }

            if (options.window is not null)
                ApplyPlatformData(ref init, nativeHandles);
            init.resolution.width = checked((uint)m_backbufferWidth);
            init.resolution.height = checked((uint)m_backbufferHeight);
            init.resolution.reset = m_resetFlags;
            if (!bgfx.init(&init))
            {
                throw new InvalidOperationException("BGFX device initialization failed.");
            }
            nativeInitialized = true;
            m_processLease.MarkInitialized();

            generation = unchecked((uint)Interlocked.Increment(ref s_nextGeneration));
            if (generation == 0)
            {
                generation = unchecked((uint)Interlocked.Increment(ref s_nextGeneration));
            }

            capabilities = BgfxCapabilityMapper.FromNative(bgfx.get_caps());
        }
        catch
        {
            if (nativeInitialized)
                bgfx.shutdown();
            m_processLease.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Gets the immutable feature and limit set reported by the active graphics backend.
    /// </summary>
    public GraphicsCapabilities capabilities { get; }

    /// <summary>
    /// Gets whether the primary presentation surface encodes linear color as sRGB.
    /// </summary>
    public bool backbufferIsSrgb => (m_resetFlags & (uint)bgfx.ResetFlags.SrgbBackbuffer) != 0;

    /// <summary>
    /// Gets whether the primary BGFX surface automatically encodes linear RGB to sRGB.
    /// </summary>
    public bool primaryPresentationEncodesSrgb => backbufferIsSrgb;

    /// <summary>
    /// Gets the generation identity that owns this value.
    /// </summary>
    public uint generation { get; private set; }

    /// <summary>
    /// Gets the current drawable pixel size of the main presentation surface.
    /// </summary>
    public RenderPresentationSize? primaryPresentationSize { get; private set; }

    /// <summary>
    /// Gets the submitted and completed frame counters used for deferred retirement.
    /// </summary>
    public RenderDeviceFrameCounters frameCounters => new(Volatile.Read(ref m_drawCount), Volatile.Read(ref m_dispatchCount));

    /// <summary>
    /// Gets API-thread allocation diagnostics for the current device generation's native transient pools.
    /// </summary>
    public RenderDeviceAllocationCounters? allocationCounters
    {
        get
        {
            EnsureApiThread();
            ObjectDisposedException.ThrowIf(m_disposed, this);
            return new RenderDeviceAllocationCounters(generation, m_transientTextureAllocationCount,
                m_transientBufferAllocationCount, m_transientFrameBufferAllocationCount);
        }
    }

    /// <summary>
    /// Gets the last frame number returned by BGFX submission.
    /// </summary>
    public uint backendFrame => m_backendFrame;

    internal int allocatedViewCount => m_nextViewId;

    internal void RecordDraw(int count = 1) => Interlocked.Add(ref m_drawCount, count);

    internal void RecordDispatch(int count = 1) => Interlocked.Add(ref m_dispatchCount, count);

    private void ValidatePersistentHandle(PersistentTextureHandle texture)
    {
        if (!texture.isValid || GetHandleIdentity(texture).generation != generation)
        {
            throw new ArgumentException("Texture handle belongs to another device generation.", nameof(texture));
        }
    }

    private void ValidatePersistentHandle(RenderSurfaceHandle surface)
    {
        if (!surface.isValid || GetHandleIdentity(surface).generation != generation)
        {
            throw new ArgumentException("Presentation surface handle belongs to another device generation.", nameof(surface));
        }
    }

    private void EnsureFrameSafetyPoint()
    {
        EnsureApiThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!m_frameOpen || m_activeGraph is not null || !m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException("Operation requires an open frame before graph execution.");
        }
    }

    private void EnsureApiThread()
    {
        if (Environment.CurrentManagedThreadId != m_apiThreadId)
        {
            throw new InvalidOperationException("BGFX API operations must run on the device API thread.");
        }
    }

    private static void ApplyPlatformData(
        ref bgfx.Init init,
        PlatformNativeHandles handles
    ) {
        if (handles.handleKind == PlatformNativeHandleId.browserCanvas)
        {
            init.platformData.nwh = handles.windowHandle.ToPointer();
            return;
        }

        if (handles.handleKind != PlatformNativeHandleId.win32 && handles.handleKind != PlatformNativeHandleId.cocoa)
        {
            throw new PlatformNotSupportedException(
                $"BGFX window surfaces do not support native handle kind '{handles.handleKind}'.");
        }

        init.platformData.nwh = handles.windowHandle.ToPointer();
        init.platformData.ndt = handles.displayHandle.ToPointer();
    }

    private static uint PackColor(RenderClearColor color)
    {
        byte r = (byte)(Math.Clamp(color.r, 0f, 1f) * 255f);
        byte g = (byte)(Math.Clamp(color.g, 0f, 1f) * 255f);
        byte b = (byte)(Math.Clamp(color.b, 0f, 1f) * 255f);
        byte a = (byte)(Math.Clamp(color.a, 0f, 1f) * 255f);
        return ((uint)r << 24) | ((uint)g << 16) | ((uint)b << 8) | a;
    }

    private static int Utf8Length(string value) => Encoding.UTF8.GetByteCount(value);
}
