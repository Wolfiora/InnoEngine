using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
namespace Inno.Editor.Workbench;
[EditorModal("export.game.progress", "Exporting Game", order: 311)]
internal sealed class GameExportProgressModal(ExportWindowModule window) : EditorModal
{
    private DialogPresentation m_content;
    private DialogLayout m_layout;
    private long m_operation;
    /// <inheritdoc />
    public override bool isVisible => window.isGameProgressVisible;
    /// <inheritdoc />
    public override bool allowScrolling => false;
    /// <inheritdoc />
    protected override Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        var snapshot = window.progress;
        m_operation = snapshot.operationId;
        m_content = new(
            string.IsNullOrEmpty(snapshot.error) ? snapshot.status : snapshot.error,
            snapshot.fraction,
            indeterminate: !snapshot.fraction.HasValue,
            actionLabel: snapshot.isBusy ? "Cancel" : null,
            actionEnabled: !snapshot.cancellationRequested,
            cancelOnEscape: true
        );
        m_layout = EditorUiSession.current.widgets.MeasureDialog(m_content, availableContentSize);
        return m_layout.size;
    }
    /// <inheritdoc />
    protected override void OnDraw(EditorContext context)
    {
        if (EditorUiSession.current.widgets.DrawDialog(m_content, m_layout))
            window.CancelGameExport(m_operation);
    }
}
