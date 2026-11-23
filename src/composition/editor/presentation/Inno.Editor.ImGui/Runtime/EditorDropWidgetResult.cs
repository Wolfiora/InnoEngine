using Inno.Editor.Interactions;

namespace Inno.Editor.ImGui;

/// <summary>
/// Reports the preview and delivery state of an ImGui editor drop target.
/// </summary>
public readonly record struct EditorDropWidgetResult
{
    /// <summary>
    /// Creates a combined native-preview and managed-drop result.
    /// </summary>
    /// <param name="isPreviewing">
    /// Whether a compatible native payload is currently hovering the target.
    /// </param>
    /// <param name="status">
    /// The compatibility and visual state returned by the managed drop router.
    /// </param>
    /// <param name="result">
    /// The result produced when the payload was delivered.
    /// </param>
    public EditorDropWidgetResult(
        bool isPreviewing,
        EditorDropStatus status,
        EditorDropResult result
    ) {
        this.isPreviewing = isPreviewing;
        this.status = status;
        this.result = result;
    }

    /// <summary>
    /// Gets whether a compatible native payload is hovering the target.
    /// </summary>
    public bool isPreviewing { get; }

    /// <summary>
    /// Gets the managed drop compatibility status.
    /// </summary>
    public EditorDropStatus status { get; }

    /// <summary>
    /// Gets the delivered drop result.
    /// </summary>
    public EditorDropResult result { get; }

    /// <summary>
    /// Gets an inactive drop target result.
    /// </summary>
    public static EditorDropWidgetResult none => new(false, EditorDropStatus.rejected, EditorDropResult.rejected);
}
