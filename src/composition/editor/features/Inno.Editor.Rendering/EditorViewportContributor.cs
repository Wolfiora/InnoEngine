namespace Inno.Editor.Rendering;

/// <summary>
/// Contributes one rendering model while the host owns viewport composition, targets, and presentation.
/// </summary>
public abstract class EditorViewportContributor
{
    /// <summary>
    /// Creates a parameterless reloadable viewport contributor.
    /// </summary>
    protected EditorViewportContributor() {}

    /// <summary>
    /// Determines whether this rendering model participates in the supplied viewport content.
    /// </summary>
    /// <param name="context">
    /// Frame-only Editor, navigation, content, and viewport context.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when this contributor can build a valid model contribution.
    /// </returns>
    public abstract bool CanContribute(EditorViewportContext context);

    /// <summary>
    /// Configures neutral host navigation before input is processed and a request is built.
    /// </summary>
    /// <param name="context">
    /// Frame-only Editor, navigation, content, and viewport context.
    /// </param>
    /// <returns>
    /// The controller's current navigation capabilities and optional selection focus bound.
    /// </returns>
    public virtual EditorViewportNavigationProfile ConfigureNavigation(EditorViewportContext context)
        => EditorViewportNavigationProfile.disabled;

    /// <summary>
    /// Builds one model-neutral render contribution for the current frame.
    /// </summary>
    /// <param name="context">
    /// Frame-only Editor and viewport context.
    /// </param>
    /// <returns>
    /// Contributor-selected pipeline and frame data.
    /// </returns>
    public abstract EditorViewportContribution Build(EditorViewportContext context);

    /// <summary>
    /// Draws optional controller-specific toolbar controls when this contributor owns viewport interaction.
    /// </summary>
    /// <param name="context">
    /// Frame-only Editor and viewport context.
    /// </param>
    public virtual void DrawToolbar(EditorViewportContext context) {}

    /// <summary>
    /// Handles one pointer click when this contributor owns viewport interaction.
    /// </summary>
    /// <param name="context">
    /// Normalized frame-only pointer context.
    /// </param>
    public virtual void HandlePointer(EditorViewportPointerContext context) {}
}

/// <summary>
/// Selects whether transform handles use all axes or a contributor-declared planar orientation.
/// </summary>
public enum EditorViewportManipulationPlane
{
    /// <summary>
    /// Keeps the full three-dimensional translation, rotation, and scale controls.
    /// </summary>
    Spatial,
    /// <summary>
    /// Uses the X/Y translation and scale axes with the Z rotation handle.
    /// </summary>
    XY,
    /// <summary>
    /// Uses the X/Z translation and scale axes with the Y rotation handle.
    /// </summary>
    XZ,
    /// <summary>
    /// Uses the Y/Z translation and scale axes with the X rotation handle.
    /// </summary>
    YZ
}
