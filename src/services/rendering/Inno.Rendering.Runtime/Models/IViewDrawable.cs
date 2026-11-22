using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Prepares one model-independent drawable for a specific render pass.
/// </summary>
[StableTypeID("796c6186-c301-5cb2-a634-d03f8156a039")]
public interface IViewDrawable
{
    /// <summary>
    /// Resolves reusable resources before render graph execution.
    /// </summary>
    /// <param name="context">
    /// Active pipeline build context.
    /// </param>
    /// <param name="view">
    /// View that will draw this item.
    /// </param>
    /// <param name="prepared">
    /// Prepared frame-local draw when resources are available.
    /// </param>
    /// <returns>
    /// Whether the drawable is ready for this pass.
    /// </returns>
    bool TryPrepare(
        RenderPipelineContext context,
        RenderView view,
        out IPreparedViewDrawable? prepared
    );
}
