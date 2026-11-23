namespace Inno.Rendering;

/// <summary>
/// Bridges compiled graph execution to a concrete graphics backend.
/// </summary>
public interface IRenderGraphBackend
{
    /// <summary>
    /// Begins execution and prepares transient allocations.
    /// </summary>
    /// <param name="graph">
    /// Compiled graph to execute.
    /// </param>
    void BeginGraph(CompiledRenderGraph graph);

    /// <summary>
    /// Begins one scheduled pass and acquires its command encoder.
    /// </summary>
    /// <param name="pass">
    /// Scheduled pass metadata.
    /// </param>
    /// <returns>
    /// A command encoder scoped to this pass.
    /// </returns>
    RenderCommandEncoder BeginPass(CompiledRenderPass pass);

    /// <summary>
    /// Ends one scheduled pass and releases its encoder.
    /// </summary>
    /// <param name="pass">
    /// Scheduled pass metadata.
    /// </param>
    void EndPass(CompiledRenderPass pass);

    /// <summary>
    /// Ends graph execution without presenting an additional frame.
    /// </summary>
    /// <param name="graph">
    /// Compiled graph that finished or failed.
    /// </param>
    void EndGraph(CompiledRenderGraph graph);
}
