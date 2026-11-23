namespace Inno.Rendering.Runtime.Reload.TestModule;

/// <summary>
/// Supplies a collectible Plugin-owned pipeline for rendering generation removal tests.
/// </summary>
[RenderPipelineExtension(C_EXTENSION_ID)]
internal sealed class ReloadableRenderPipeline : RenderPipeline
{
    /// <summary>
    /// Identifies the pipeline independently of its collectible CLR generation.
    /// </summary>
    internal const string C_EXTENSION_ID = "tests.runtime.reloadable-plugin";

    /// <summary>
    /// Adds observable render work to the frame graph.
    /// </summary>
    /// <param name="context">
    /// The active pipeline context that owns the frame graph mutation.
    /// </param>
    public override void Build(RenderPipelineContext context)
        => context.graph
            .AddRasterPass(
                "Reloadable Plugin Pass",
                new RenderPhaseID("tests.runtime.reloadable-plugin.visible"),
                0,
                static (
                    _,
                    _
                ) => {}
            )
            .HasSideEffect();
}
