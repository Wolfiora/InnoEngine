using System.Numerics;

using Inno.Editor.Core;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;

namespace Inno.Editor.Panel.FileBrowser;

[EditorModal("asset-sample-import.progress", "Importing Sample", order: 110)]
internal sealed class AssetSampleImportModal(AssetSampleImportModule imports) : EditorModal
{
    /// <summary>
    /// Shows progress while the import owns pending background work.
    /// </summary>
    public override bool isVisible => imports.isBusy;

    /// <summary>
    /// Blocks unrelated interaction until import publication or cancellation completes.
    /// </summary>
    public override bool blocksInteraction => true;

    /// <summary>
    /// Draws progress and cancellation through the shared Editor modal widgets.
    /// </summary>
    /// <param name="context">
    /// The current Editor frame and interaction services.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        EditorWidget.WrappedText(imports.status);
        EditorWidget.CenteredProgressBar(imports.progress, new Vector2(-1f, 0f), $"{imports.progress:P0}");
        if (EditorWidget.CenteredButton("Cancel", EditorWidget.style.itemSpacing.Y))
            imports.Cancel();
    }
}
