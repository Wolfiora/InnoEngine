using System;

namespace Inno.Editor.Interactions;

/// <summary>
/// Defines a targetless editor action that requires one strongly typed command argument.
/// </summary>
/// <typeparam name="TArgument">
/// The command argument type accepted by the action.
/// </typeparam>
public abstract class EditorArgumentAction<TArgument> : EditorAction
{
    internal sealed override Type argumentType => typeof(TArgument);

    /// <summary>
    /// Evaluates the operation's current availability and presentation state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// The validated editor action state that represents the completed operation.
    /// </returns>
    protected sealed override EditorActionState Query(EditorActionContext context)
        => context.target is null
           && context.argument is TArgument argument
           ? Query(new EditorActionArgumentContext<TArgument>(context, argument))
           : EditorActionState.hidden;

    /// <summary>
    /// Evaluates the action for a typed argument.
    /// </summary>
    /// <param name="context">
    /// The typed contextual request.
    /// </param>
    /// <returns>
    /// The current presentation and availability state.
    /// </returns>
    protected virtual EditorActionState Query(EditorActionArgumentContext<TArgument> context) => EditorActionState.enabled;

    /// <summary>
    /// Applies the editor action to the supplied interaction context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected sealed override void Execute(EditorActionContext context)
    {
        if (context.target is not null || context.argument is not TArgument argument)
        {
            throw new InvalidOperationException($"Editor action '{GetType().FullName}' requires argument '{typeof(TArgument).FullName}'.");
        }

        Execute(new EditorActionArgumentContext<TArgument>(context, argument));
    }

    /// <summary>
    /// Executes the action for a typed argument.
    /// </summary>
    /// <param name="context">
    /// The typed contextual request.
    /// </param>
    protected abstract void Execute(EditorActionArgumentContext<TArgument> context);
}
