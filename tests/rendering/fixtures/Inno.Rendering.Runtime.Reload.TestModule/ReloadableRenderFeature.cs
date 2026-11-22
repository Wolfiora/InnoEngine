namespace Inno.Rendering.Runtime.Reload.TestModule;

[RenderFeatureExtension(C_EXTENSION_ID)]
internal sealed class ReloadableRenderFeature : RenderPipelineFeature
{
    internal const string C_EXTENSION_ID = "tests.runtime.reloadable-plugin-feature";

    /// <summary>
    /// Adds observable feature work to the active pipeline graph.
    /// </summary>
    /// <param name="context">
    /// The active feature context that owns the frame graph mutation.
    /// </param>
    public override void AddRenderPasses(RenderFeatureContext context)
        => context.graph
            .AddRasterPass(
                "Reloadable Plugin Feature Pass",
                new RenderPhaseID("tests.runtime.reloadable-plugin-feature.visible"),
                1,
                static (
                    _,
                    _
                ) => {}
            )
            .HasSideEffect();
}
