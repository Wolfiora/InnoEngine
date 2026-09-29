using System.Numerics;

using Inno.Editor.Core;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;

namespace Inno.Editor.Exporting;

[EditorModal("export.game.progress", "Exporting Game", order: 311)]
internal sealed class GameExportProgressModal(ExportWindowModule window) : EditorModal
{
    /// <summary>
    /// Gets whether the export progress overlay should be displayed.
    /// </summary>
    public override bool isVisible => window.isGameProgressVisible;

    /// <summary>
    /// Gets whether export progress prevents interaction with the Editor beneath it.
    /// </summary>
    public override bool blocksInteraction => true;

    /// <summary>
    /// Draws the current export status and its cancellation or dismissal action.
    /// </summary>
    /// <param name="context">
    /// The active Editor context for the current frame.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        string message = string.IsNullOrEmpty(window.error) ? window.status : window.error;
        EditorWidget.WrappedText(message);
        EditorWidget.CenteredProgressBar(
            window.gameProgress,
            new Vector2(-1f, 0f),
            $"{window.gameProgress:P0}");
        string button = window.isGameBusy ? "Cancel" : "Close";
        if (!EditorWidget.CenteredButton(button, EditorWidget.style.itemSpacing.Y))
            return;
        if (window.isGameBusy)
            window.CancelGameExport();
        else
            window.CloseGameProgress();
    }
}
