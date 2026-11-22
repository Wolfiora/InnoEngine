namespace Inno.Editor.Interactions;

internal sealed class EditorMenuPlacement
{
    internal EditorMenuPlacement(
        string path,
        string actionID,
        int order,
        bool separatorBefore,
        object? argument
    ) {
        this.path = path;
        this.actionID = actionID;
        this.order = order;
        this.separatorBefore = separatorBefore;
        this.argument = argument;
    }

    internal string path { get; }

    /// <summary>
    /// Gets the action identifier.
    /// </summary>
    internal string actionID { get; }

    /// <summary>
    /// Gets the stable menu order.
    /// </summary>
    internal int order { get; }

    /// <summary>
    /// Gets whether a separator precedes this placement.
    /// </summary>
    internal bool separatorBefore { get; }

    /// <summary>
    /// Gets the optional action argument.
    /// </summary>
    internal object? argument { get; }
}
