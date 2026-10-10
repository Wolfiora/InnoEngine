using System;
using Inno.Rendering;

namespace Inno.Rendering.Runtime.Reload.TestModule;

/// <summary>
/// Contributes collectible frame callbacks while its host removes registration during preparation.
/// </summary>
public sealed class ReloadableContributor : IRenderFrameGraphContributor
{
    private readonly Action<IRenderFrameGraphContributor> m_unregister;
    private readonly bool m_rejectGraph;

    /// <summary>
    /// Creates a frame contributor whose callback exercises the public registration lifetime.
    /// </summary>
    /// <param name="unregister">
    /// Host-owned callback removing this instance from the next frame snapshot.
    /// </param>
    /// <param name="rejectGraph">
    /// Whether graph construction must fail after adding its collectible draw callback.
    /// </param>
    public ReloadableContributor(
        Action<IRenderFrameGraphContributor> unregister,
        bool rejectGraph
    ) {
        m_unregister = unregister;
        m_rejectGraph = rejectGraph;
    }

    /// <inheritdoc />
    public void PrepareFrame(ulong frameIndex) => m_unregister(this);

    /// <inheritdoc />
    public void AddRenderPasses(
        RenderGraphBuilder graph,
        ulong frameIndex
    ) {
        graph.AddRasterPass("Collectible Callback", new RenderPhaseId("tests.collectible.contributor"),
            0, Encode).HasSideEffect();
        if (m_rejectGraph)
            throw new InvalidOperationException("Reject the collectible frame mutation.");
    }

    private void Encode(
        int payload,
        RenderPassContext context
    ) {
        GC.KeepAlive(this);
    }
}
