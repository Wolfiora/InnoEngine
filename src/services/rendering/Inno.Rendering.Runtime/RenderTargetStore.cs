using System;
using System.Collections.Generic;
using Inno.Core.Execution;
using Inno.Rendering;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Owns persistent offscreen targets without exposing backend-native handles.
/// </summary>
public sealed class RenderTargetStore : IDisposable
{
    private readonly IRenderDevice m_device;
    private readonly int m_capacity;
    private int m_frameAllocations;
    private long m_rejected;
    private RenderRetirementQueue? m_frameRetirement;
    private readonly Dictionary<RenderTexture, TargetEntry> m_targets = [];
    private readonly HashSet<RenderTexture> m_pendingReleases = [];
    private readonly List<PersistentTextureHandle> m_retireNextFrame = [];
    private readonly List<PersistentTextureHandle> m_retireFollowingFrame = [];
    private bool m_disposed;
    private RenderRetirementQueue? m_retirement;

    /// <summary>
    /// Creates a target registry for one device generation.
    /// </summary>
    /// <param name="device">
    /// Device that owns persistent target resources.
    /// </param>
    /// <param name="capacity">
    /// Positive target and per-frame allocation capacity.
    /// </param>
    public RenderTargetStore(IRenderDevice device, int capacity = 1024)
    {
        m_device = device ?? throw new ArgumentNullException(nameof(device));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(capacity);
        m_capacity = capacity;
    }

    /// <summary>
    /// Gets the number of retained offscreen targets.
    /// </summary>
    public int count => m_targets.Count;

    /// <summary>
    /// Gets target admissions rejected by finite capacity.
    /// </summary>
    public long rejectedCount => m_rejected;

    /// <summary>
    /// Imports or creates one target in the current frame graph.
    /// </summary>
    /// <param name="graph">
    /// Current frame graph builder.
    /// </param>
    /// <param name="target">
    /// Persistent target description.
    /// </param>
    /// <returns>
    /// A graph-scoped handle for the current target resource.
    /// </returns>
    public RenderTextureHandle Import(RenderGraphBuilder graph, RenderTexture target)
    {
        ObjectDisposedException.ThrowIf(m_disposed || m_retirement is not null, this);
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(target);
        m_pendingReleases.Remove(target);

        if (!m_targets.TryGetValue(target, out TargetEntry? entry)
            || entry.revision != target.contentRevision
            || !entry.descriptor.Equals(target.descriptor))
        {
            if ((entry is null && m_targets.Count >= m_capacity) || m_frameAllocations >= m_capacity)
            {
                m_rejected++;
                throw new InvalidOperationException("Render target allocation capacity has been reached.");
            }
            PersistentTextureHandle next = m_device.CreateTexture(target.descriptor, target.name);
            m_frameAllocations++;
            if (entry is not null)
                Retire(entry.handle);
            entry = new TargetEntry(target.contentRevision, target.descriptor, next);
            m_targets[target] = entry;
        }

        return graph.ImportTexture(target.name, entry.handle, entry.descriptor);
    }

    /// <summary>
    /// Tries to get the current opaque device texture for UI presentation.
    /// </summary>
    /// <param name="target">
    /// Persistent target description.
    /// </param>
    /// <param name="texture">
    /// Receives a backend-neutral persistent handle.
    /// </param>
    /// <returns>
    /// <see langword="true"/> after a graph write to the current target revision has been recorded successfully.
    /// </returns>
    public bool TryGetTexture(RenderTexture target, out PersistentTextureHandle texture)
    {
        ObjectDisposedException.ThrowIf(m_disposed || m_retirement is not null, this);
        ArgumentNullException.ThrowIfNull(target);
        if (m_targets.TryGetValue(target, out TargetEntry? entry) && entry.hasWritten)
        {
            texture = entry.handle;
            return true;
        }

        texture = default;
        return false;
    }

    /// <summary>
    /// Publishes target availability only after the backend recorded a graph write to its active revision.
    /// </summary>
    /// <param name="graph">
    /// The graph whose backend command recording completed without an exception.
    /// </param>
    internal void MarkWrittenOutputs(CompiledRenderGraph graph)
    {
        ObjectDisposedException.ThrowIf(m_disposed || m_retirement is not null, this);
        ArgumentNullException.ThrowIfNull(graph);
        foreach (TargetEntry entry in m_targets.Values)
            if (!entry.hasWritten && WritesTarget(graph, entry.handle))
                entry.hasWritten = true;
    }

    /// <summary>
    /// Queues one target for frame-safe retirement.
    /// </summary>
    /// <param name="target">
    /// Target no longer used by request producers.
    /// </param>
    public void Release(RenderTexture target)
    {
        ObjectDisposedException.ThrowIf(m_disposed || m_retirement is not null, this);
        ArgumentNullException.ThrowIfNull(target);
        if (m_targets.ContainsKey(target)) m_pendingReleases.Add(target);
    }

    /// <summary>
    /// Advances queued target releases at a frame safety point.
    /// </summary>
    /// <remarks>
    /// Retired textures remain active across one complete submitted frame so a
    /// prepared UI or presentation packet cannot observe an invalidated handle.
    /// </remarks>
    public void PrepareFrame()
    {
        ObjectDisposedException.ThrowIf(m_disposed || m_retirement is not null, this);
        if (m_frameRetirement is null)
        {
            m_frameRetirement = new RenderRetirementQueue();
            foreach (PersistentTextureHandle texture in m_retireFollowingFrame)
                m_frameRetirement.Add(() => m_device.DestroyTexture(texture));
            m_retireFollowingFrame.Clear();
        }
        try { m_frameRetirement.Dispose(); }
        catch (Exception pending) when (RetirementPendingException.Find(pending) is not null) { throw; }
        catch { m_frameRetirement = null; throw; }
        m_frameRetirement = null;
        m_frameAllocations = 0;
        m_retireFollowingFrame.AddRange(m_retireNextFrame);
        m_retireNextFrame.Clear();

        foreach (RenderTexture target in m_pendingReleases)
        {
            if (m_targets.Remove(target, out TargetEntry? entry))
                Retire(entry.handle);
        }
        m_pendingReleases.Clear();
    }

    /// <summary>
    /// Queues all owned resources for device-safe destruction.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// A backend resource is still active. Remaining resources stay owned until disposal is retried.
    /// </exception>
    /// <exception cref="AggregateException">
    /// Quiescent resources were all attempted, but one or more backend releases failed.
    /// </exception>
    public void Dispose()
    {
        if (m_disposed)
            return;
        if (m_retirement is null)
        {
            m_retirement = new RenderRetirementQueue();
            if (m_frameRetirement is not null) m_retirement.Add(m_frameRetirement.Dispose);
            foreach (TargetEntry entry in m_targets.Values)
                m_retirement.Add(() => m_device.DestroyTexture(entry.handle));
            foreach (PersistentTextureHandle texture in m_retireNextFrame)
                m_retirement.Add(() => m_device.DestroyTexture(texture));
            foreach (PersistentTextureHandle texture in m_retireFollowingFrame)
                m_retirement.Add(() => m_device.DestroyTexture(texture));
        }
        try { m_retirement.Dispose(); }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null) { throw; }
        catch
        {
            CompleteRetirement();
            throw;
        }
        CompleteRetirement();
    }

    private static bool WritesTarget(CompiledRenderGraph graph, PersistentTextureHandle handle)
    {
        foreach (CompiledRenderTexture texture in graph.textures)
        {
            if (!texture.imported || texture.persistentHandle != handle)
                continue;
            foreach (CompiledRenderPass pass in graph.passes)
                foreach (CompiledRenderAttachment attachment in pass.attachments)
                    if (attachment.texture == texture.handle)
                        return true;
        }
        return false;
    }

    private void CompleteRetirement()
    {
        m_targets.Clear();
        m_pendingReleases.Clear();
        m_retireNextFrame.Clear();
        m_retireFollowingFrame.Clear();
        m_disposed = true;
    }

    private void Retire(PersistentTextureHandle texture)
    {
        if (texture.isValid)
            m_retireNextFrame.Add(texture);
    }

    private sealed record TargetEntry(
        long revision,
        RenderTextureDescriptor descriptor,
        PersistentTextureHandle handle)
    {
        internal bool hasWritten { get; set; }
    }
}
