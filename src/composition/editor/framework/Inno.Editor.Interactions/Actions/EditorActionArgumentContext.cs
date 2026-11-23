namespace Inno.Editor.Interactions;

/// <summary>
/// Provides a strongly typed action argument to a targetless editor action.
/// </summary>
/// <typeparam name="TArgument">
/// The action argument type required by the action.
/// </typeparam>
public sealed class EditorActionArgumentContext<TArgument> : EditorActionContext
{
    internal EditorActionArgumentContext(
        EditorActionContext context,
        TArgument argument
    ) : base(context.editor, context.interactions, context.area, target: null, argument: argument)
    {
        this.argument = argument;
    }

    /// <summary>
    /// Gets the strongly typed action argument.
    /// </summary>
    public new TArgument argument { get; }
}
