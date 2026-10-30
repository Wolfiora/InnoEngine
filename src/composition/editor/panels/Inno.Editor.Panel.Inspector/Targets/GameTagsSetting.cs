using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Core.Settings;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;
using Inno.Scene;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
[ProjectSettingPath("Project/Scene/Tags")]
internal sealed class GameTagsSetting : ProjectSettingEditor<GameTagCatalog>
{
    private const int C_TAG_BUFFER_SIZE = 128;
    private string m_error = string.Empty;
    private string m_newTag = string.Empty;
    /// <summary>
    /// Gets the presentation section that groups this setting.
    /// </summary>
    public override string section => "Definitions";
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Define the project-wide runtime tags that scenes and Plugins may assign to GameObjects.";

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value currently being presented.
    /// </param>
    protected override void OnDraw(GameTagCatalog setting)
    {
        DrawAddRow(setting);
        Ui.Spacing();
        DrawTags(setting);
        if (!string.IsNullOrEmpty(m_error))
            EditorUI.current.widgets.ColoredText(EditorPalette.error, m_error);
    }

    private void DrawAddRow(GameTagCatalog setting)
    {
        float spacing = Ui.GetLayout().itemSpacing.X;
        float actionWidth = EditorUI.current.widgets.GetCompactIconSize().X;
        Ui.SetNextItemWidth(MathF.Max(1f, Ui.GetContentRegionAvailable().X - actionWidth - spacing));
        bool submit = Ui.InputTextWithHint("##new_project_tag", "New tag", ref m_newTag, C_TAG_BUFFER_SIZE, ImGuiInputTextFlags.EnterReturnsTrue);
        Ui.SameLine(0f, spacing);
        submit |= EditorUI.current.widgets.ClickableIcon("add_project_tag", ImGuiIcon.Plus, "Add project tag");
        if (!submit || string.IsNullOrWhiteSpace(m_newTag))
            return;
        try
        {
            if (!setting.Add(m_newTag))
                m_error = $"Tag '{m_newTag.Trim()}' is already defined.";
            else
                m_error = string.Empty;
            m_newTag = string.Empty;
        }
        catch (ArgumentException exception)
        {
            m_error = exception.Message;
        }
    }

    private static void DrawTags(GameTagCatalog setting)
    {
        IReadOnlyList<string> tags = setting.GetTags();
        ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.NoSavedSettings;
        Ui.PushStyleVar((ImGuiStyleVar.CellPadding), EditorUI.current.widgets.style.cellPadding);
        if (!Ui.BeginTable("##project_tag_definitions", 2, flags, default(System.Numerics.Vector2), 0f))
        {
            Ui.PopStyleVar();
            return;
        }

        try
        {
            Ui.TableSetupColumn("Tag", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize));
            Ui.TableSetupColumn("Action", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), 72f * EditorUI.current.widgets.style.zoom);
            DrawTagTableHeader();
            for (int i = 0; i < tags.Count; i++)
            {
                string tag = tags[i];
                bool isDefault = string.Equals(tag, GameObject.defaultTag, StringComparison.Ordinal);
                Ui.TableNextRow();
                _ = Ui.TableSetColumnIndex(0);
                Ui.AlignTextToFramePadding();
                InsetPlainCell();
                Ui.Text(tag);
                _ = Ui.TableSetColumnIndex(1);
                InsetPlainCell();
                if (isDefault)
                {
                    EditorUI.current.widgets.ColoredText(EditorPalette.textDisabled, "Fixed");
                    continue;
                }

                if (EditorUI.current.widgets.ClickableText(
                    $"remove_project_tag_{tag}",
                    "Remove",
                    new Vector2(Ui.MeasureText("Remove").X, Ui.GetFrameHeight()),
                    "Remove this definition. Existing scene assignments are preserved and become undefined."
                ))
                {
                    _ = setting.Remove(tag);
                }
            }
        }
        finally
        {
            Ui.EndTable();
            Ui.PopStyleVar();
        }
    }

    private static void DrawTagTableHeader()
    {
        uint background = Ui.ToPackedColor(EditorPalette.collectionHeader);
        Ui.TableNextRow();
        Ui.TableSetBgColor((ImGuiTableBgTarget.RowBg0), background);
        Ui.TableSetBgColor((ImGuiTableBgTarget.RowBg1), background);
        DrawHeaderCell(0, "Tag");
        DrawHeaderCell(1, "Action");
    }

    private static void DrawHeaderCell(
        int column,
        string label
    ) {
        _ = Ui.TableSetColumnIndex(column);
        InsetPlainCell();
        Ui.Text(label);
    }

    private static void InsetPlainCell() => Ui.SetCursorPosX(Ui.GetCursorPosX() + Ui.GetLayout().framePadding.X);
}
