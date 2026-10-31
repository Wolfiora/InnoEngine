using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;

[EditorModal("export.plugin", "Export as Plugin", order: 300)]
internal sealed class PluginExportModal(ExportWindowModule window) : EditorModal
{
    private const int C_TEXT_CAPACITY = 4096;
    private const string C_DESCRIPTION = "Packages the current project's complete authored source. Plugin.inno, dependency declarations, settings contributions, and deterministic metadata are generated automatically.";
    private const string C_HINT = "Identity comes from Settings > Project > Identity. Other defaults come from Build > Plugin.";
    private readonly List<float> m_rows = new(16);
    private string m_identity = string.Empty;
    private string m_name = string.Empty;
    private string m_destination = string.Empty;
    private string m_outcome = string.Empty;
    private bool m_busy;
    private bool m_dependencies;
    private long m_operation;
    private float m_wrapWidth;
    /// <inheritdoc />
    public override bool isVisible => window.isPluginVisible;
    /// <inheritdoc />
    public override bool canMove => true;
    /// <inheritdoc />
    public override Vector2 initialSize => new(680f, 0f);
    /// <inheritdoc />
    protected override Vector2? OnPrepare(
        EditorContext context,
        Vector2 availableContentSize
    ) {
        var progress = window.progress;
        m_operation = progress.operationId;
        m_identity = $"{window.pluginId} (from Project ID)";
        m_name = window.pluginDisplayName;
        m_destination = window.pluginOutputPath;
        m_dependencies = window.includePluginDependencies;
        m_busy = window.isPluginBusy;
        m_outcome = string.IsNullOrEmpty(progress.error) ? progress.status : progress.error;
        float width = availableContentSize.X;
        float scrollbar = Ui.GetLayout().scrollbarSize;
        float checkboxWidth = Ui.GetFrameHeight() + Ui.GetLayout().itemInnerSpacing.X +
            Ui.MeasureText("Include dependency Plugin packages").X;
        float height = Present(false, width);
        float availableHeight = availableContentSize.Y - (checkboxWidth > width ? scrollbar : 0f);
        if (height > availableHeight)
        {
            width = MathF.Max(1f, width - scrollbar);
            height = Present(false, width);
        }
        m_wrapWidth = width;
        return new Vector2(MathF.Max(width, checkboxWidth), height);
    }
    /// <inheritdoc />
    protected override void OnDraw(EditorContext context) => Present(true, m_wrapWidth);

    private float Present(
        bool draw,
        float width
    ) {
        float height = 0f;
        int row = 0;
        Vector2 origin = draw ? Ui.GetCursorPosition() : Vector2.Zero;
        if (!draw)
            m_rows.Clear();
        var layout = Ui.GetLayout();
        float gap = layout.itemSpacing.Y;
        float frame = Ui.MeasureText("M").Y + layout.framePadding.Y * 2f;
        Text(C_DESCRIPTION, true);
        Text("Plugin ID");
        Text(m_identity, true);
        Field("Display Name", "##plugin_name", ref m_name);
        Field("Destination IPlugin", "##plugin_output", ref m_destination);
        Row(frame);
        if (draw)
        {
            Ui.BeginDisabled(m_busy);
            try
            {
                if (Ui.Checkbox("Include dependency Plugin packages", ref m_dependencies))
                    window.includePluginDependencies = m_dependencies;
            }
            finally
            {
                Ui.EndDisabled();
            }
        }
        Text(C_HINT, true);
        if (m_outcome.Length != 0)
            Text(m_outcome, true);
        Row(frame);
        if (draw)
        {
            float closeWidth = Ui.MeasureText("Close").X + layout.framePadding.X * 2f;
            float actionWidth = Ui.MeasureText(m_busy ? "Cancel" : "Export").X + layout.framePadding.X * 2f;
            Ui.SetCursorPosX(Ui.GetCursorPosX() + MathF.Max(0f, width - closeWidth - actionWidth - layout.itemSpacing.X));
            Ui.BeginDisabled(m_busy);
            try
            {
                if (Ui.Button("Close"))
                    window.ClosePlugin();
            }
            finally
            {
                Ui.EndDisabled();
            }
            Ui.SameLine(0f, -1f);
            if (Ui.Button(m_busy ? "Cancel" : "Export"))
            {
                if (m_busy)
                    window.CancelPluginExport(m_operation);
                else
                    window.BeginPluginExport();
            }
        }
        return MathF.Ceiling(height);

        void Row(float extent)
        {
            if (draw)
                Ui.SetCursorPosition(origin + new Vector2(0f, m_rows[row++]));
            else
            {
                if (m_rows.Count != 0)
                    height += gap;
                m_rows.Add(height);
                height += extent;
            }
        }
        void Text(
            string text,
            bool wrapped = true
        ) {
            Row(draw ? 0f : (wrapped ? Ui.MeasureWrappedText(text, width) : Ui.MeasureText(text)).Y);
            if (!draw)
                return;
            if (wrapped)
                EditorUiSession.current.widgets.WrappedText(text, width);
            else
                Ui.Text(text);
        }
        void Field(
            string label,
            string id,
            ref string value
        ) {
            Text(label);
            Row(frame);
            if (!draw)
                return;
            Ui.BeginDisabled(m_busy);
            try
            {
                Ui.SetNextItemWidth(width);
                if (Ui.InputText(id, ref value, C_TEXT_CAPACITY))
                {
                    if (id == "##plugin_name")
                        window.pluginDisplayName = value;
                    else
                        window.pluginOutputPath = value;
                }
            }
            finally
            {
                Ui.EndDisabled();
            }
        }
    }
}
