namespace Inno.Editor.Interactions;

/// <summary>
/// Defines a target action with a typed presentation-only argument.
/// </summary>
/// <typeparam name="TTarget">
/// The target type accepted by the action.
/// </typeparam>
/// <typeparam name="TPresentation">
/// The presentation data type.
/// </typeparam>
public abstract class EditorPresentationAction<TTarget, TPresentation> : EditorAction<TTarget>
{
    /// <summary>
    /// Presents this action through the current editor interaction surface.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the operation succeeds or its condition is satisfied; otherwise, <see langword="false"/>.
    /// </returns>
    protected sealed override bool Present(EditorActionContext<TTarget> context)
        => context.argument is TPresentation presentation
           && Present(new EditorActionContext<TTarget, TPresentation>(context, context.target, presentation));

    /// <summary>
    /// Presents active content using strongly typed presentation data.
    /// </summary>
    /// <param name="context">
    /// The typed presentation request.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when content was presented.
    /// </returns>
    protected abstract bool Present(EditorActionContext<TTarget, TPresentation> context);
}
