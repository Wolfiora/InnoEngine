namespace Inno.Rendering;

/// <summary>
/// Describes stencil behavior for one triangle face orientation.
/// </summary>
public readonly record struct RenderStencilFaceState
{
    /// <summary>
    /// Creates one face stencil state.
    /// </summary>
    /// <param name="compare">
    /// Stencil comparison.
    /// </param>
    /// <param name="fail">
    /// Operation after stencil comparison failure.
    /// </param>
    /// <param name="depthFail">
    /// Operation after stencil success and depth failure.
    /// </param>
    /// <param name="pass">
    /// Operation after stencil and depth success.
    /// </param>
    public RenderStencilFaceState(
        RenderStencilCompare compare,
        RenderStencilOperation fail,
        RenderStencilOperation depthFail,
        RenderStencilOperation pass
    ) {
        this.compare = compare;
        this.fail = fail;
        this.depthFail = depthFail;
        this.pass = pass;
    }

    /// <summary>
    /// Gets stencil comparison.
    /// </summary>
    public RenderStencilCompare compare { get; }

    /// <summary>
    /// Gets the stencil-failure operation.
    /// </summary>
    public RenderStencilOperation fail { get; }

    /// <summary>
    /// Gets the depth-failure operation.
    /// </summary>
    public RenderStencilOperation depthFail { get; }

    /// <summary>
    /// Gets the complete-pass operation.
    /// </summary>
    public RenderStencilOperation pass { get; }
}
