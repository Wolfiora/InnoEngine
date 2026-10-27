using System.Numerics;
using UiDrawing = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Settings;
internal sealed class SettingsPageView(SettingsEditSession session)
{
    private static SettingsStyle settingsStyle => new(EditorUI.current.widgets.style);

    internal void Draw(
        SettingsPage page,
        Action<SettingsPage> navigate
    ) {
        DrawPageHeader(page);
        Ui.Spacing();
        PushDisabledText();
        EditorUI.current.widgets.WrappedText(page.description);
        Ui.PopStyleColor();
        Ui.Dummy(new System.Numerics.Vector2(0f, 16f * EditorUI.current.widgets.style.zoom));
        if (page.children.Count != 0)
        {
            DrawOverview(page.children, navigate);
            Ui.Dummy(new System.Numerics.Vector2(0f, 8f * EditorUI.current.widgets.style.zoom));
        }

        if (!page.hasSettings)
        {
            if (page.children.Count == 0)
                DrawOverview(page.children, navigate);
            return;
        }

        int fieldIndex = 0;
        foreach (IGrouping<string, SettingsField> group in page.settings.GroupBy(static setting => setting.section ?? string.Empty, StringComparer.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(group.Key))
                Ui.SeparatorText(group.Key);
            DrawSection(group, ref fieldIndex);
            Ui.Spacing();
        }
    }

    private void DrawPageHeader(SettingsPage page)
    {
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.NoPadOuterX;
        if (!Ui.BeginTable("##settings_page_header", 2, flags, default(System.Numerics.Vector2), 0f))
            return;
        try
        {
            Ui.TableSetupColumn("##settings_page_title", ImGuiTableColumnFlags.WidthStretch, 1f);
            Ui.TableSetupColumn("##settings_page_reset", ImGuiTableColumnFlags.WidthFixed, GetResetColumnWidth());
            Ui.TableNextRow();
            _ = Ui.TableSetColumnIndex(0);
            Ui.AlignTextToFramePadding();
            Ui.Text(page.label);
            _ = Ui.TableSetColumnIndex(1);
            Ui.PushId(page.path);
            try
            {
                bool canReset = session.CanReset(page);
                if (DrawResetButton(canReset))
                    session.Reset(page);
            }
            finally
            {
                Ui.PopId();
            }
        }
        finally
        {
            Ui.EndTable();
        }
    }

    private static void DrawOverview(
        IReadOnlyList<SettingsPage> children,
        Action<SettingsPage> navigate
    ) {
        if (children.Count == 0)
        {
            PushDisabledText();
            Ui.Text("No settings are currently registered on this page.");
            Ui.PopStyleColor();
            return;
        }

        for (int i = 0; i < children.Count; i++)
        {
            SettingsPage child = children[i];
            bool clicked = EditorUI.current.widgets.HoverText($"settings_overview_{child.path}", child.label);
            if (clicked)
                navigate(child);
            EditorUI.current.widgets.DrawItemTooltip(child.description);
            Ui.Spacing();
        }
    }

    private void DrawSection(
        IEnumerable<SettingsField> settings,
        ref int fieldIndex
    ) {
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.NoPadOuterX;
        float tableWidth = MathF.Max(1f, Ui.GetContentRegionAvailable().X);
        Ui.PushStyleVar(ImGuiStyleVar.CellPadding, settingsStyle.fieldPadding);
        bool tableStarted = false;
        try
        {
            tableStarted = Ui.BeginTable("##settings_section", 3, flags, new System.Numerics.Vector2(tableWidth, 0f));
            if (!tableStarted)
                return;
            EditorUI.current.widgets.SetupPropertyColumns();
            Ui.TableSetupColumn("##settings_reset", ImGuiTableColumnFlags.WidthFixed, GetResetColumnWidth());
            foreach (SettingsField setting in settings)
                DrawFieldRow(setting, fieldIndex++);
        }
        finally
        {
            if (tableStarted)
                Ui.EndTable();
            Ui.PopStyleVar();
        }
    }

    private void DrawFieldRow(
        SettingsField setting,
        int fieldIndex
    ) {
        Ui.TableNextRow();
        uint background = Ui.ToPackedColor(fieldIndex % 2 == 0 ? SettingsStyle.field : SettingsStyle.alternateField);
        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, background);
        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, background);
        Ui.PushId(setting.path);
        try
        {
            _ = Ui.TableSetColumnIndex(0);
            Ui.SetCursorPosX(Ui.GetCursorPosX() + UiDrawing.GetLayout().windowPadding.X);
            EditorUI.current.widgets.PropertyLabel(setting.label, setting.description);
            _ = Ui.TableSetColumnIndex(1);
            bool groupStarted = false;
            try
            {
                Ui.BeginGroup();
                groupStarted = true;
                session.UpdateDirty(setting, session.Draw(setting));
                Ui.EndGroup();
                groupStarted = false;
                _ = Ui.TableSetColumnIndex(2);
                bool canReset = session.CanReset(setting);
                if (DrawResetButton(canReset))
                    session.Reset(setting);
            }
            finally
            {
                if (groupStarted)
                    Ui.EndGroup();
            }
        }
        finally
        {
            Ui.PopId();
        }

        DrawFieldBackgroundGutters(background);
    }

    private static void DrawFieldBackgroundGutters(uint background)
    {
        var table = UiDrawing.GetTableBounds();
        var window = UiDrawing.GetWindowBounds();
        float tableMinimumX = table.minimumX;
        float tableMaximumX = table.maximumX;
        float rowMinimumY = table.rowMinimum;
        float rowMaximumY = table.rowMaximum;
        float windowMinimumX = window.minimumX;
        float windowMaximumX = window.maximumX;
        if (rowMaximumY <= rowMinimumY)
            return;
        EditorDrawList drawList = EditorImGui.GetDrawList();
        drawList.PushClip(new Vector2(windowMinimumX, rowMinimumY), new Vector2(windowMaximumX, rowMaximumY), false);
        try
        {
            if (tableMinimumX > windowMinimumX)
            {
                EditorImGui.DrawFilledRectangle(new System.Numerics.Vector2(windowMinimumX, rowMinimumY), new System.Numerics.Vector2(tableMinimumX, rowMaximumY), background, 0);
            }

            if (tableMaximumX < windowMaximumX)
            {
                EditorImGui.DrawFilledRectangle(new System.Numerics.Vector2(tableMaximumX, rowMinimumY), new System.Numerics.Vector2(windowMaximumX, rowMaximumY), background, 0);
            }
        }
        finally
        {
            drawList.PopClip();
        }
    }

    private static float GetResetColumnWidth()
    {
        EditorUiLayout style = UiDrawing.GetLayout();
        return style.itemSpacing.X + Ui.MeasureText("Reset").X + style.framePadding.X * 2f + style.windowPadding.X;
    }

    private static bool DrawResetButton(bool enabled)
    {
        EditorUiLayout style = UiDrawing.GetLayout();
        Ui.SetCursorPosX(Ui.GetCursorPosX() + style.itemSpacing.X);
        Ui.BeginDisabled(!enabled);
        try
        {
            return Ui.SmallButton("Reset");
        }
        finally
        {
            Ui.EndDisabled();
        }
    }

    private static void PushDisabledText() => Ui.PushStyleColor(ImGuiCol.Text, UiDrawing.GetStyleColorValue(ImGuiCol.TextDisabled));
}
