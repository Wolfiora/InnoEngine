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

sealed unsafe partial class BgfxDevice
{
    /// <summary>
    /// Begins a frame-scoped operation and makes queued work visible.
    /// </summary>
    public void BeginFrame()
    {
        EnsureApiThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_frameOpen)
        {
            throw new InvalidOperationException("A BGFX frame is already open.");
        }

        ResetPreviousViews();
        Volatile.Write(ref m_drawCount, 0);
        Volatile.Write(ref m_dispatchCount, 0);
        ProcessDeferredResources(force: false);
        ProcessCanceledReadbacks();
        TrimTransientResourceCaches();
        if (m_presentationUpdatePending)
        {
            if (m_pendingPresentationSize is RenderPresentationSize size)
            {
                m_resetPending |= size.width != m_backbufferWidth || size.height != m_backbufferHeight;
                m_backbufferWidth = size.width;
                m_backbufferHeight = size.height;
            }
            primaryPresentationSize = m_pendingPresentationSize;
            m_pendingPresentationSize = null;
            m_presentationUpdatePending = false;
        }
        if (m_resetPending)
        {
            m_resetPending = false;
            if (capabilities.backend != GraphicsApi.Noop)
            {
                bgfx.reset(
                    checked((uint)m_backbufferWidth),
                    checked((uint)m_backbufferHeight),
                    m_resetFlags,
                    bgfx.TextureFormat.Count);
            }
        }

        m_frameOpen = true;
    }

    /// <summary>
    /// Executes the prepared operation and publishes only a completed result.
    /// </summary>
    /// <param name="graph">
    /// The graph consumed by execute; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="frameIndex">
    /// The monotonic frame identity associated with this operation.
    /// </param>
    public void Execute(
        CompiledRenderGraph graph,
        ulong frameIndex
    ) {
        EnsureFrameSafetyPoint();
        ArgumentNullException.ThrowIfNull(graph);
        graph.Execute(this, frameIndex);
    }

    /// <summary>
    /// Commits the current frame-scoped operation and returns its completion identity.
    /// </summary>
    /// <returns>
    /// The validated uint that represents the completed operation.
    /// </returns>
    public uint EndFrame()
    {
        EnsureApiThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (!m_frameOpen)
        {
            throw new InvalidOperationException("No BGFX frame is open.");
        }

        if (m_activeGraph is not null || !m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException("All render graphs and encoders must end before BGFX frame submission.");
        }

        if (m_nextViewId == 0)
        {
            bgfx.touch(0);
        }

        m_backendFrame = bgfx.frame((byte)bgfx.FrameFlags.None);
        m_frameOpen = false;
        return m_backendFrame;
    }

    /// <summary>
    /// Publishes drawable availability at the next BeginFrame without inventing a windowless output.
    /// </summary>
    /// <param name="size">
    /// The real primary pixel extent, or null while the existing surface is unavailable.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The assigned extent is invalid.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// A windowless device is asked to advertise a primary output.
    /// </exception>
    public void SetPrimaryPresentationSize(RenderPresentationSize? size)
    {
        EnsureApiThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (size is RenderPresentationSize assigned && !assigned.isValid)
            throw new ArgumentException("A primary presentation extent must have positive pixel dimensions.", nameof(size));
        if (size.HasValue && !m_hasPrimarySurface)
            throw new NotSupportedException("A windowless BGFX device has no primary presentation surface.");
        m_pendingPresentationSize = size;
        m_presentationUpdatePending = true;
    }

    /// <summary>
    /// Queues an idempotent presentation policy update for the next BeginFrame reset.
    /// Noop devices retain the policy without issuing a native presentation reset.
    /// </summary>
    /// <param name="enabled">
    /// Whether presentation should wait for display refresh.
    /// </param>
    public void SetVerticalSync(bool enabled)
    {
        EnsureApiThread();
        ObjectDisposedException.ThrowIf(m_disposed, this);
        uint next = enabled
            ? m_resetFlags | (uint)bgfx.ResetFlags.Vsync
            : m_resetFlags & ~(uint)bgfx.ResetFlags.Vsync;
        if (next == m_resetFlags)
            return;
        m_resetFlags = next;
        m_resetPending = true;
    }

    private void ResetPreviousViews()
    {
        for (int viewIndex = 0; viewIndex < m_nextViewId; viewIndex++)
        {
            bgfx.reset_view(checked((ushort)viewIndex));
        }

        m_nextViewId = 0;
    }

}
