using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Settings;

internal sealed class SettingsPageView(SettingsEditSession session)
{
    internal void Draw(
        SettingsPage page,
        Action<SettingsPage> navigate
    ) {
        DrawPageHeader(page);
        Gui.Spacing();
        PushDisabledText();
        EditorUI.current.widgets.WrappedText(page.description);
        Gui.PopStyleColor();
        Gui.Dummy(new Vector2(0f, 16f * EditorUI.current.widgets.style.zoom));
        if (page.children.Count != 0)
        {
            DrawOverview(page.children, navigate);
            Gui.Dummy(new Vector2(0f, 8f * EditorUI.current.widgets.style.zoom));
        }

        if (!page.hasSettings)
        {
            if (page.children.Count == 0)
            {
                DrawOverview(page.children, navigate);
            }
            return;
        }

        int fieldIndex = 0;
        foreach (IGrouping<string, SettingsField> group in page.settings.GroupBy(
            static setting => setting.section ?? string.Empty,
            StringComparer.OrdinalIgnoreCase
        )) {
            if (!string.IsNullOrEmpty(group.Key))
            {
                Gui.SeparatorText(group.Key);
            }
            DrawSection(group, ref fieldIndex);
            Gui.Spacing();
        }
    }

    private static SettingsStyle settingsStyle => new(EditorUI.current.widgets.style);

    private void DrawPageHeader(SettingsPage page)
    {
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.NoPadOuterX;
        if (!Gui.BeginTable("##settings_page_header", 2, flags, default, 0f))
        {
            return;
        }
        try
        {
            Gui.TableSetupColumn("##settings_page_title", ImGuiTableColumnFlags.WidthStretch, 1f);
            Gui.TableSetupColumn("##settings_page_reset", ImGuiTableColumnFlags.WidthFixed, GetResetColumnWidth());
            Gui.TableNextRow();
            _ = Gui.TableSetColumnIndex(0);
            Gui.AlignTextToFramePadding();
            Gui.Text(page.label);
            _ = Gui.TableSetColumnIndex(1);
            Gui.PushID(page.path);
            try
            {
                bool canReset = session.CanReset(page);
                if (DrawResetButton(canReset))
                {
                    session.Reset(page);
                }
            }
            finally
            {
                Gui.PopID();
            }
        }
        finally
        {
            Gui.EndTable();
        }
    }

    private void DrawSection(
        IEnumerable<SettingsField> settings,
        ref int fieldIndex
    ) {
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.NoPadOuterX;
        float tableWidth = MathF.Max(1f, Gui.GetContentRegionAvailable().X);
        Gui.PushStyleVar(ImGuiStyleVar.CellPadding, settingsStyle.fieldPadding);
        bool tableStarted = false;
        try
        {
            tableStarted = Gui.BeginTable("##settings_section", 3, flags, new Vector2(tableWidth, 0f));
            if (!tableStarted)
            {
                return;
            }
            EditorUI.current.widgets.SetupPropertyColumns();
            Gui.TableSetupColumn("##settings_reset", ImGuiTableColumnFlags.WidthFixed, GetResetColumnWidth());
            foreach (SettingsField setting in settings)
            {
                DrawFieldRow(setting, fieldIndex++);
            }
        }
        finally
        {
            if (tableStarted)
            {
                Gui.EndTable();
            }
            Gui.PopStyleVar();
        }
    }

    private void DrawFieldRow(
        SettingsField setting,
        int fieldIndex
    ) {
        Gui.TableNextRow();
        uint background = Gui.ToPackedColor(fieldIndex % 2 == 0 ? SettingsStyle.field : SettingsStyle.alternateField);
        Gui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, background);
        Gui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, background);
        Gui.PushID(setting.path);
        try
        {
            _ = Gui.TableSetColumnIndex(0);
            Gui.SetCursorPosX(Gui.GetCursorPosX() + Gui.GetLayout().windowPadding.X);
            EditorUI.current.widgets.PropertyLabel(setting.label, setting.description);
            _ = Gui.TableSetColumnIndex(1);
            bool groupStarted = false;
            try
            {
                Gui.BeginGroup();
                groupStarted = true;
                session.UpdateDirty(setting, session.Draw(setting));
                Gui.EndGroup();
                groupStarted = false;
                _ = Gui.TableSetColumnIndex(2);
                bool canReset = session.CanReset(setting);
                if (DrawResetButton(canReset))
                {
                    session.Reset(setting);
                }
            }
            finally
            {
                if (groupStarted)
                {
                    Gui.EndGroup();
                }
            }
        }
        finally
        {
            Gui.PopID();
        }

        DrawFieldBackgroundGutters(background);
    }

    private static void DrawOverview(
        IReadOnlyList<SettingsPage> children,
        Action<SettingsPage> navigate
    ) {
        if (children.Count == 0)
        {
            PushDisabledText();
            Gui.Text("No settings are currently registered on this page.");
            Gui.PopStyleColor();
            return;
        }

        for (int i = 0; i < children.Count; i++)
        {
            SettingsPage child = children[i];
            bool clicked = EditorUI.current.widgets.HoverText($"settings_overview_{child.path}", child.label);
            if (clicked)
            {
                navigate(child);
            }
            EditorUI.current.widgets.DrawItemTooltip(child.description);
            Gui.Spacing();
        }
    }

    private static void DrawFieldBackgroundGutters(uint background)
    {
        (float minimumX, float maximumX, float rowMinimum, float rowMaximum) table = Gui.GetTableBounds();
        (float minimumX, float maximumX, Vector2 workMaximum, Vector2 padding) window = Gui.GetWindowBounds();
        float tableMinimumX = table.minimumX;
        float tableMaximumX = table.maximumX;
        float rowMinimumY = table.rowMinimum;
        float rowMaximumY = table.rowMaximum;
        float windowMinimumX = window.minimumX;
        float windowMaximumX = window.maximumX;
        if (rowMaximumY <= rowMinimumY)
        {
            return;
        }
        EditorDrawList drawList = Gui.GetDrawList();
        drawList.PushClip(new Vector2(windowMinimumX, rowMinimumY), new Vector2(windowMaximumX, rowMaximumY), false);
        try
        {
            if (tableMinimumX > windowMinimumX)
            {
                Gui.DrawFilledRectangle(
                    new Vector2(windowMinimumX, rowMinimumY),
                    new Vector2(tableMinimumX, rowMaximumY),
                    background,
                    0
                );
            }

            if (tableMaximumX < windowMaximumX)
            {
                Gui.DrawFilledRectangle(
                    new Vector2(tableMaximumX, rowMinimumY),
                    new Vector2(windowMaximumX, rowMaximumY),
                    background,
                    0
                );
            }
        }
        finally
        {
            drawList.PopClip();
        }
    }

    private static float GetResetColumnWidth()
    {
        EditorUILayout style = Gui.GetLayout();
        return style.itemSpacing.X + Gui.MeasureText("Reset").X + style.framePadding.X * 2f + style.windowPadding.X;
    }

    private static bool DrawResetButton(bool enabled)
    {
        EditorUILayout style = Gui.GetLayout();
        Gui.SetCursorPosX(Gui.GetCursorPosX() + style.itemSpacing.X);
        Gui.BeginDisabled(!enabled);
        try
        {
            return Gui.SmallButton("Reset");
        }
        finally
        {
            Gui.EndDisabled();
        }
    }

    private static void PushDisabledText() => Gui.PushStyleColor(ImGuiCol.Text, Gui.GetStyleColorValue(ImGuiCol.TextDisabled));
}
