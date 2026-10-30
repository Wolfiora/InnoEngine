using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
namespace Inno.Editor.Panel.ShaderEditor;

[EditorModal("shader.check", "Checking Shader", order: 220)]
internal sealed class ShaderCheckModal(ShaderEditorDocuments documents) : EditorModal
{
    private DialogPresentation m_content;
    private DialogLayout m_layout;
    /// <inheritdoc />
    public override bool isVisible => documents.TryGetCheckDraft(out _);
    /// <inheritdoc />
    public override bool allowScrolling => false;
    /// <inheritdoc />
    protected override Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        m_content = new(documents.checkStatus);
        m_layout = EditorUiSession.current.widgets.MeasureDialog(m_content, availableContentSize);
        return m_layout.size;
    }
    /// <inheritdoc />
    protected override void OnDraw(EditorContext context) => EditorUiSession.current.widgets.DrawDialog(m_content, m_layout);
}
