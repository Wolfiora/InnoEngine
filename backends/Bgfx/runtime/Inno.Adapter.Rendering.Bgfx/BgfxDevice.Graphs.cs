using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Inno.Adapter.Platform;
using Inno.Native.Bgfx;
using Inno.Platform;
using Inno.Rendering;

namespace Inno.Adapter.Rendering.Bgfx;

public sealed unsafe partial class BgfxDevice
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
        if (requestedViews > capabilities.limits.maxViews - m_nextViewID)
        {
            throw new InvalidOperationException(
                $"Frame requires at least {m_nextViewID + requestedViews} views, "
                + $"but the device supports {capabilities.limits.maxViews}. "
                + "Reduce active cameras, viewports, or render passes."
            );
        }

        m_activeGraph = graph;
        m_activeGraphViewBase = m_nextViewID;
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
                        nativeTexture = AcquireTransientTexture(texture.descriptor, texture.name, texture.physicalSlot);
                        m_transientTextureSlots.Add(texture.physicalSlot, nativeTexture);
                        m_transientTextureSlotDescriptors.Add(texture.physicalSlot, texture.descriptor);
                    }
                    else if (!m_transientTextureSlotDescriptors[texture.physicalSlot].Equals(texture.descriptor))
                    {
                        throw new InvalidOperationException(
                            $"Render-graph physical texture slot {texture.physicalSlot} aliases incompatible descriptors."
                        );
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

                bgfx.Raw.set_view_order(
                    checked((ushort)m_activeGraphViewBase),
                    checked((ushort)graph.passes.Count),
                    order
                );
            }

            m_nextViewID += requestedViews;
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
        if (m_activeGraph is null || m_activeEncoder is not null)
        {
            throw new InvalidOperationException("Pass execution is outside a valid BGFX graph scope.");
        }

        ushort viewID = checked((ushort)(m_activeGraphViewBase + pass.viewIndex));
        int viewNameLength = Encoding.UTF8.GetByteCount(pass.name);
        Span<byte> viewNameBytes = viewNameLength < 256
                                   ? stackalloc byte[256]
                                   : new byte[checked(viewNameLength + 1)];
        Encoding.UTF8.GetBytes(pass.name, viewNameBytes);
        viewNameBytes[viewNameLength] = 0;
        fixed (byte* viewNamePointer = viewNameBytes)
        {
            bgfx.Raw.set_view_name(viewID, (sbyte*)viewNamePointer, viewNameLength);
        }
        bgfx.set_view_mode(viewID, bgfx.ViewMode.Sequential);
        ApplyViewTransform(viewID, pass.viewTransform);
        ConfigureViewTarget(viewID, pass);
        m_activeEncoder = bgfx.Raw.encoder_begin(0);
        if (m_activeEncoder is null)
        {
            throw new InvalidOperationException($"BGFX could not acquire an encoder for pass '{pass.name}'.");
        }

        bgfx.Raw.encoder_touch(m_activeEncoder, viewID);
        return new BgfxCommandEncoder(this, m_activeEncoder, viewID);
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
        if (m_activeEncoder is null)
        {
            throw new InvalidOperationException("No BGFX encoder is active.");
        }

        bgfx.Raw.encoder_end(m_activeEncoder);
        m_activeEncoder = null;
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
        if (!ReferenceEquals(m_activeGraph, graph) || m_activeEncoder is not null)
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

    private void ReleasePreparedGraphResources()
    {
        ReturnTransientGraphResources();
        m_graphFrameBuffers.Clear();
        m_graphTextures.Clear();
        m_graphBuffers.Clear();
        m_activeGraph = null;
        m_activeGraphViewBase = 0;
    }

    private static void ApplyViewTransform(
        ushort viewID,
        RenderViewTransform? transform
    ) {
        if (transform is null)
        {
            bgfx.Raw.set_view_transform(viewID, null, null);
            return;
        }

        ReadOnlySpan<float> view = transform.viewMatrix.Span;
        ReadOnlySpan<float> projection = transform.projectionMatrix.Span;
        fixed (float* viewPointer = view)
        fixed (float* projectionPointer = projection)
        {
            bgfx.Raw.set_view_transform(viewID, viewPointer, projectionPointer);
        }
    }
}
