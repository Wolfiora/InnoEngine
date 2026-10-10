using System.Threading.Tasks;
using Inno.Core.Execution;
using Inno.Adapter.Platform;
using System;
using System.Collections.Generic;
using Inno.Native.Bgfx;
using Inno.Platform;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

/// <summary>
/// Implements the backend-neutral render device contract using generation-scoped BGFX resources.
/// </summary>
public sealed unsafe partial class BgfxDevice
{
    private readonly Dictionary<ulong, BgfxWindowSurfaceResource> m_windowSurfaces = [];

    /// <summary>
    /// Creates a detached-window presentation surface at a frame safety point.
    /// </summary>
    /// <param name="nativeHandles">
    /// Platform window handles supplied by the active window backend.
    /// </param>
    /// <param name="width">
    /// Drawable width in pixels.
    /// </param>
    /// <param name="height">
    /// Drawable height in pixels.
    /// </param>
    /// <param name="name">
    /// Debug and diagnostic name.
    /// </param>
    /// <returns>
    /// An opaque surface handle consumable by <see cref="RasterPassBuilder.UseSurface"/>.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The selected integration or renderer cannot provide the requested surface.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when BGFX cannot create the window framebuffer.
    /// </exception>
    public RenderSurfaceHandle CreateWindowSurface(
        PlatformNativeHandles nativeHandles,
        int width,
        int height,
        string name
    ) {
        EnsureSurfaceSafetyPoint();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        IBgfxSurfaceIntegration integration = m_surfaceIntegration
            ?? throw new NotSupportedException("Additional BGFX windows require an explicit surface integration.");
        if (!integration.supportsAdditionalSurfaces)
            throw new NotSupportedException("The selected host does not support additional window surfaces.");
        BgfxSurfaceDescriptor descriptor = BgfxSurfaceBinding.Validate(
            integration.Resolve(nativeHandles, BgfxSurfaceRole.Additional));
        if (!capabilities.Supports(GraphicsCapability.SwapChain))
        {
            throw new NotSupportedException(
                "The active graphics backend does not support additional presentation surfaces.");
        }

        bgfx.FrameBufferHandle frameBuffer = CreateNativeWindowSurface(descriptor, width, height);
        if (!frameBuffer.Valid)
        {
            throw new InvalidOperationException($"BGFX could not create detached window surface '{name}'.");
        }

        ulong id = m_nextPersistentId++;
        m_windowSurfaces.Add(id, new BgfxWindowSurfaceResource(frameBuffer, width, height, descriptor, name));
        return CreateRenderSurfaceHandle(id, generation);
    }

    /// <summary>
    /// Reports whether the native window surface encodes linear render-target writes as sRGB.
    /// </summary>
    /// <param name="surface">
    /// Active detached-window surface owned by this device generation.
    /// </param>
    /// <returns>
    /// True for an sRGB presentation target; false when a final output transfer is required.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The surface is stale or not active on this device.
    /// </exception>
    public bool WindowSurfaceIsSrgb(RenderSurfaceHandle surface)
    {
        _ = ResolveSurface(surface);
        return backbufferIsSrgb
            && capabilities.backend != GraphicsApi.Direct3D11
            && capabilities.backend != GraphicsApi.Direct3D12;
    }

    /// <summary>
    /// Recreates a detached-window presentation surface for a new drawable extent.
    /// </summary>
    /// <param name="surface">
    /// Surface owned by this device generation.
    /// </param>
    /// <param name="width">
    /// New drawable width in pixels.
    /// </param>
    /// <param name="height">
    /// New drawable height in pixels.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the surface is stale or no longer active.
    /// </exception>
    public void ResizeWindowSurface(
        RenderSurfaceHandle surface,
        int width,
        int height
    ) {
        EnsureSurfaceSafetyPoint();
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        BgfxWindowSurfaceResource resource = ResolveSurface(surface);
        if (resource.width == width && resource.height == height)
        {
            return;
        }

        bgfx.FrameBufferHandle replacement = CreateNativeWindowSurface(resource.descriptor, width, height);
        if (!replacement.Valid)
        {
            throw new InvalidOperationException($"BGFX could not resize detached window surface '{resource.name}'.");
        }

        bgfx.FrameBufferHandle previous = resource.frameBuffer;
        resource.frameBuffer = replacement;
        resource.width = width;
        resource.height = height;
        resource.retirement.pending.Add(previous);
        if (!m_surfaceRetirements.Contains(resource.retirement))
            m_surfaceRetirements.Add(resource.retirement);
    }

    /// <summary>
    /// Stops accepting work for a surface and queues all of its framebuffer generations for retirement.
    /// </summary>
    /// <param name="surface">
    /// The active surface owned by this device generation.
    /// </param>
    /// <returns>
    /// A task completed after the render thread has processed every associated destruction command.
    /// This confirms release of the borrowed window, not a general GPU fence.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The surface is stale or no longer active.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The caller is not the API thread or a graph or encoder is active.
    /// </exception>
    public Task RetireWindowSurface(RenderSurfaceHandle surface)
    {
        EnsureSurfaceSafetyPoint();
        BgfxWindowSurfaceResource resource = ResolveSurface(surface);
        m_windowSurfaces.Remove(GetHandleIdentity(surface).value);
        resource.retirement.pending.Add(resource.frameBuffer);
        resource.retirement.closing = true;
        if (!m_surfaceRetirements.Contains(resource.retirement))
            m_surfaceRetirements.Add(resource.retirement);
        return resource.retirement.completion.Task;
    }

    /// <summary>
    /// Advances nonpresenting frames until all requested window retirements have been acknowledged.
    /// </summary>
    /// <remarks>
    /// Call only on the API thread with no open frame, graph, encoder or native UI callback.
    /// A failed drain retains the device and the borrowed native windows.
    /// </remarks>
    /// <exception cref="InvalidOperationException">
    /// The caller is not at a closed-frame API-thread safety point.
    /// </exception>
    /// <exception cref="RetirementTimeoutException">
    /// The bounded retirement deadline expired; dependent owners must remain alive.
    /// </exception>
    public void DrainWindowSurfaceRetirements()
    {
        EnsureSurfaceSafetyPoint();
        if (m_frameOpen)
            throw new InvalidOperationException("Window retirement draining requires a closed render frame.");
        if (!HasClosingSurface())
            return;
        m_surfaceRetirementBarrier ??= new RetirementBarrier("BGFX window surfaces");
        m_surfaceRetirementBarrier.Wait(() =>
        {
            SubmitNativeFrame(bgfx.FrameFlags.Flush);
            foreach (BgfxSurfaceRetirement retirement in m_surfaceRetirements)
                if (retirement.closing)
                    throw new RetirementPendingException("BGFX has not acknowledged window destruction.", retirement.failure);
        });
        m_surfaceRetirementBarrier = null;
    }

    private bool HasClosingSurface()
    {
        foreach (BgfxSurfaceRetirement retirement in m_surfaceRetirements)
            if (retirement.closing)
                return true;
        return false;
    }

    private uint SubmitNativeFrame(bgfx.FrameFlags flags)
    {
        ulong submission = checked(m_submissionSequence + 1);
        foreach (BgfxSurfaceRetirement retirement in m_surfaceRetirements)
        {
            if (retirement.failure is not null)
                throw new RetirementPendingException("A window framebuffer could not retire.", retirement.failure);
            try
            {
                foreach (bgfx.FrameBufferHandle frameBuffer in retirement.pending)
                    bgfx.destroy_frame_buffer(frameBuffer);
                if (retirement.pending.Count > 0)
                    retirement.lastSubmission = submission;
                retirement.pending.Clear();
            }
            catch (Exception failure)
            {
                retirement.failure = failure;
                retirement.completion.TrySetException(failure);
                throw new RetirementPendingException("Window framebuffer destruction failed; its window remains owned.", failure);
            }
        }

        // frame() waits for the preceding submission's render commands before handing over this one.
        // The same conservative acknowledgment is used when the renderer executes inline.
        uint frame;
        try
        {
            frame = bgfx.frame((byte)flags);
        }
        catch (Exception failure)
        {
            foreach (BgfxSurfaceRetirement retirement in m_surfaceRetirements)
            {
                retirement.failure = failure;
                retirement.completion.TrySetException(failure);
            }
            throw new RetirementPendingException("The render submission failed; borrowed windows remain owned.", failure);
        }
        m_backendFrame = frame;
        ulong acknowledged = m_submissionSequence;
        m_submissionSequence = submission;
        for (int index = m_surfaceRetirements.Count - 1; index >= 0; index--)
        {
            BgfxSurfaceRetirement retirement = m_surfaceRetirements[index];
            if (retirement.lastSubmission > acknowledged)
                continue;
            if (retirement.closing)
                retirement.completion.TrySetResult();
            m_surfaceRetirements.RemoveAt(index);
        }
        return frame;
    }

    private static bgfx.FrameBufferHandle CreateNativeWindowSurface(
        BgfxSurfaceDescriptor descriptor,
        int width,
        int height
    )
        => bgfx.create_frame_buffer_from_nwh(
            descriptor.windowHandle.ToPointer(),
            checked((ushort)width),
            checked((ushort)height),
            bgfx.TextureFormat.BGRA8,
            bgfx.TextureFormat.D24S8);

    private BgfxWindowSurfaceResource ResolveSurface(RenderSurfaceHandle surface)
    {
        ValidatePersistentHandle(surface);
        if (!m_windowSurfaces.TryGetValue(GetHandleIdentity(surface).value, out BgfxWindowSurfaceResource? resource))
        {
            throw new ArgumentException("Presentation surface is not active on this device.", nameof(surface));
        }

        return resource;
    }

    private void EnsureSurfaceSafetyPoint()
    {
        EnsureApiThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_activeGraph is not null || !m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException(
                "Window surface changes require an API-thread point outside graph execution.");
        }
    }

    private sealed class BgfxWindowSurfaceResource
    {
        /// <summary>
        /// Creates a validated bgfx window surface resource instance.
        /// </summary>
        /// <param name="frameBuffer">
        /// The frame buffer consumed by bgfx window surface resource; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <param name="width">
        /// The width in logical units or pixels required by this operation.
        /// </param>
        /// <param name="height">
        /// The height in logical units or pixels required by this operation.
        /// </param>
        /// <param name="descriptor">
        /// The descriptor consumed by bgfx window surface resource; ownership remains with the caller unless explicitly stated otherwise.
        /// </param>
        /// <param name="name">
        /// The human-readable name used for presentation and diagnostics.
        /// </param>
        public BgfxWindowSurfaceResource(
            bgfx.FrameBufferHandle frameBuffer,
            int width,
            int height,
            BgfxSurfaceDescriptor descriptor,
            string name
        ) {
            this.frameBuffer = frameBuffer;
            this.width = width;
            this.height = height;
            this.descriptor = descriptor;
            this.name = name;
        }

        /// <summary>
        /// Gets the BGFX framebuffer owned by this presentation surface.
        /// </summary>
        public bgfx.FrameBufferHandle frameBuffer { get; set; }
        internal BgfxSurfaceRetirement retirement { get; } = new();
        /// <summary>
        /// Gets the scalar measurement or identity associated with the current state.
        /// </summary>
        public int width { get; set; }
        /// <summary>
        /// Gets the scalar measurement or identity associated with the current state.
        /// </summary>
        public int height { get; set; }
        /// <summary>
        /// Gets the platform-native window and display handles used to create the surface.
        /// </summary>
        public BgfxSurfaceDescriptor descriptor { get; }
        /// <summary>
        /// Gets the human-readable name used for presentation and diagnostics.
        /// </summary>
        public string name { get; }
    }
}
