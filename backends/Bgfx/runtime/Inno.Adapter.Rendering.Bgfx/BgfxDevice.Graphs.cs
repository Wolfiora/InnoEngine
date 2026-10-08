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
    /// Begins recording commands for one validated compiled render graph.
    /// </summary>
    /// <param name="graph">
    /// The graph consumed by begin graph; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void BeginGraph(CompiledRenderGraph graph)
    {
        EnsureFrameSafetyPoint();
        ArgumentNullException.ThrowIfNull(graph);
        if (m_activeGraph is not null)
        {
            throw new InvalidOperationException("A render graph is already executing.");
        }

        int requestedViews = graph.passes.Count;
        if (requestedViews > capabilities.limits.maxViews - m_nextViewId)
        {
            throw new InvalidOperationException(
                $"Frame requires at least {m_nextViewId + requestedViews} views, "
                + $"but the device supports {capabilities.limits.maxViews}. "
                + "Reduce active cameras, viewports, or render passes.");
        }

        m_activeGraph = graph;
        m_activeGraphViewBase = m_nextViewId;
        m_graphTextures.Clear();
        m_transientTextureSlots.Clear();
        m_transientTextureSlotDescriptors.Clear();
        m_graphFrameBuffers.Clear();
        try
        {
            PrepareGraphBuffers(graph);

            foreach (CompiledRenderTexture texture in graph.textures)
            {
                bgfx.TextureHandle nativeTexture;
                if (texture.imported)
                {
                    ValidatePersistentHandle(texture.persistentHandle);
                    if (!m_persistentTextures.TryGetValue(GetHandleIdentity(texture.persistentHandle).value, out nativeTexture))
                    {
                        throw new InvalidOperationException($"Imported texture '{texture.name}' is no longer active.");
                    }
                }
                else
                {
                    if (texture.physicalSlot < 0)
                    {
                        continue;
                    }

                    if (!m_transientTextureSlots.TryGetValue(texture.physicalSlot, out nativeTexture))
                    {
                        nativeTexture = AcquireTransientTexture(
                            texture.descriptor,
                            texture.name,
                            texture.physicalSlot);
                        m_transientTextureSlots.Add(texture.physicalSlot, nativeTexture);
                        m_transientTextureSlotDescriptors.Add(texture.physicalSlot, texture.descriptor);
                    }
                    else if (!m_transientTextureSlotDescriptors[texture.physicalSlot].Equals(texture.descriptor))
                    {
                        throw new InvalidOperationException(
                            $"Render-graph physical texture slot {texture.physicalSlot} aliases incompatible descriptors.");
                    }
                }

                m_graphTextures.Add(GetHandleIdentity(texture.handle).index, nativeTexture);
            }

            if (graph.passes.Count != 0)
            {
                ushort* order = stackalloc ushort[graph.passes.Count];
                for (int index = 0; index < graph.passes.Count; index++)
                {
                    order[index] = checked((ushort)(m_activeGraphViewBase + graph.passes[index].viewIndex));
                }

                bgfx.set_view_order(
                    checked((ushort)m_activeGraphViewBase),
                    checked((ushort)graph.passes.Count),
                    order);
            }

            m_nextViewId += requestedViews;
        }
        catch
        {
            ReleasePreparedGraphResources();
            m_activeGraphViewBase = 0;
            throw;
        }
    }

    /// <summary>
    /// Begins recording commands for one compiled render pass.
    /// </summary>
    /// <param name="pass">
    /// The pass consumed by begin pass; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <returns>
    /// The validated render command encoder that represents the completed operation.
    /// </returns>
    public RenderCommandEncoder BeginPass(CompiledRenderPass pass)
    {
        ArgumentNullException.ThrowIfNull(pass);
        if (m_activeGraph is null || !m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException("Pass execution is outside a valid BGFX graph scope.");
        }

        ushort viewId = checked((ushort)(m_activeGraphViewBase + pass.viewIndex));
        bgfx.set_view_name(viewId, pass.name, Utf8Length(pass.name));
        bgfx.set_view_mode(viewId, bgfx.ViewMode.Sequential);
        ApplyViewTransform(viewId, pass.viewTransform);
        ConfigureViewTarget(viewId, pass);
        m_activeEncoder = bgfx.encoder_begin(false);
        if (m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException($"BGFX could not acquire an encoder for pass '{pass.name}'.");
        }

        bgfx.encoder_touch(m_activeEncoder, viewId);
        return new BgfxCommandEncoder(this, m_activeEncoder, viewId);
    }

    /// <summary>
    /// Ends the active render pass and seals its recorded commands.
    /// </summary>
    /// <param name="pass">
    /// The pass consumed by end pass; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void EndPass(CompiledRenderPass pass)
    {
        ArgumentNullException.ThrowIfNull(pass);
        if (m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException("No BGFX encoder is active.");
        }

        bgfx.encoder_end(m_activeEncoder);
        m_activeEncoder = bgfx.Encoder.Null;
    }

    /// <summary>
    /// Finishes graph recording and submits its completed command stream.
    /// </summary>
    /// <param name="graph">
    /// The graph consumed by end graph; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void EndGraph(CompiledRenderGraph graph)
    {
        ArgumentNullException.ThrowIfNull(graph);
        if (!ReferenceEquals(m_activeGraph, graph) || !m_activeEncoder.IsNull)
        {
            throw new InvalidOperationException("BGFX graph cleanup does not match the active graph state.");
        }

        ReturnTransientGraphResources();
        m_graphFrameBuffers.Clear();
        m_graphTextures.Clear();
        m_graphBuffers.Clear();
        m_activeGraph = null;
        m_activeGraphViewBase = 0;
    }

    private static void ApplyViewTransform(
        ushort viewId,
        RenderViewTransform? transform
    ) {
        if (transform is null)
        {
            bgfx.set_view_transform(viewId, null, null);
            return;
        }

        ReadOnlySpan<float> view = transform.viewMatrix.Span;
        ReadOnlySpan<float> projection = transform.projectionMatrix.Span;
        fixed (float* viewPointer = view)
        fixed (float* projectionPointer = projection)
        {
            bgfx.set_view_transform(viewId, viewPointer, projectionPointer);
        }
    }

    private void ReleasePreparedGraphResources()
    {
        ReturnTransientGraphResources();
        m_graphFrameBuffers.Clear();
        m_graphTextures.Clear();
        m_graphBuffers.Clear();
        m_activeGraph = null;
        m_activeGraphViewBase = 0;
    }

}
