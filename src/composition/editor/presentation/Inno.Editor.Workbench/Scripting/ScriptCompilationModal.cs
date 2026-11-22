using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Scripting;

namespace Inno.Editor.Workbench;

[EditorModal("scripting.compilation", "Compiling Scripts", order: 100)]
internal sealed class ScriptCompilationModal(IEditorScriptCompilation scripting) : EditorModal
{
    private DialogPresentation m_content;
    private DialogLayout m_layout;
    private long m_operation;

    /// <inheritdoc/>
    public override bool isVisible => scripting.requiresExclusiveProgress;

    /// <inheritdoc/>
    public override bool allowScrolling => false;

    /// <inheritdoc/>
    protected override Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        EditorScriptCompilationProgress snapshot = scripting.progress;
        m_operation = snapshot.operationID;
        m_content = new(snapshot.status, snapshot.fraction, actionLabel: "Cancel", actionEnabled: snapshot.canCancel, cancelOnEscape: true);
        m_layout = EditorUISession.current.widgets.MeasureDialog(m_content, availableContentSize);
        return m_layout.size;
    }

    /// <inheritdoc/>
    protected override void OnDraw(EditorContext context)
    {
        if (EditorUISession.current.widgets.DrawDialog(m_content, m_layout))
        {
            scripting.CancelCompilation(m_operation);
        }
    }
}
