using EditorImGui = Inno.Editor.ImGui.ImGui;
using System.Runtime.CompilerServices;
using NumericsVector2 = System.Numerics.Vector2;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using NumericsVector4 = System.Numerics.Vector4;
using NativeVector4 = Inno.Native.ImGui.ImVec4C;
using System;
using System.Collections.Generic;
using System.Linq;

using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Panel.Settings;

internal sealed unsafe class SettingsPageView(SettingsEditSession session)
{
    internal void Draw(
        SettingsPage page,
        Action<SettingsPage> navigate
    ) {
        DrawPageHeader(page);
        NativeImGui.Spacing();
        PushDisabledText();
        EditorWidget.WrappedText(page.description);
        NativeImGui.PopStyleColor();
        NativeImGui.Dummy(new System.Numerics.Vector2(
            0f,
            16f * EditorWidget.style.zoom));

        if (page.children.Count != 0)
        {
            DrawOverview(page.children, navigate);
            NativeImGui.Dummy(new System.Numerics.Vector2(
                0f,
                8f * EditorWidget.style.zoom));
        }

        if (!page.hasSettings)
        {
            if (page.children.Count == 0)
                DrawOverview(page.children, navigate);
            return;
        }

        int fieldIndex = 0;
        foreach (IGrouping<string, SettingsField> group in page.settings.GroupBy(
                     static setting => setting.section ?? string.Empty,
                     StringComparer.OrdinalIgnoreCase))
        {
            if (!string.IsNullOrEmpty(group.Key))
                NativeImGui.SeparatorText(group.Key);
            DrawSection(group, ref fieldIndex);
            NativeImGui.Spacing();
        }
    }

    private void DrawPageHeader(SettingsPage page)
    {
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp |
                                ImGuiTableFlags.NoSavedSettings |
                                ImGuiTableFlags.NoPadOuterX;
        if (!NativeImGui.BeginTable("##settings_page_header", 2, (int)flags, default(System.Numerics.Vector2), 0f))
            return;
        try
        {
            NativeImGui.TableSetupColumn(
                "##settings_page_title",
                (int)ImGuiTableColumnFlags.WidthStretch,
                1f);
            NativeImGui.TableSetupColumn(
                "##settings_page_reset",
                (int)ImGuiTableColumnFlags.WidthFixed,
                GetResetColumnWidth());
            NativeImGui.TableNextRow();
            _ = NativeImGui.TableSetColumnIndex(0);
            NativeImGui.AlignTextToFramePadding();
            NativeImGui.TextUnformatted(page.label);
            _ = NativeImGui.TableSetColumnIndex(1);
            NativeImGui.PushID(page.path);
            try
            {
                bool canReset = session.CanReset(page);
                if (DrawResetButton(canReset))
                    session.Reset(page);
            }
            finally
            {
                NativeImGui.PopID();
            }
        }
        finally
        {
            NativeImGui.EndTable();
        }
    }

    private static void DrawOverview(
        IReadOnlyList<SettingsPage> children,
        Action<SettingsPage> navigate
    ) {
        if (children.Count == 0)
        {
            PushDisabledText();
            NativeImGui.TextUnformatted("No settings are currently registered on this page.");
            NativeImGui.PopStyleColor();
            return;
        }

        for (int i = 0; i < children.Count; i++)
        {
            SettingsPage child = children[i];
            bool clicked = EditorWidget.HoverText(
                $"settings_overview_{child.path}",
                child.label);
            if (clicked)
                navigate(child);
            EditorWidget.DrawItemTooltip(child.description);
            NativeImGui.Spacing();
        }
    }

    private void DrawSection(
        IEnumerable<SettingsField> settings,
        ref int fieldIndex
    ) {
        ImGuiTableFlags flags = ImGuiTableFlags.SizingStretchProp |
                                ImGuiTableFlags.NoSavedSettings |
                                ImGuiTableFlags.NoPadOuterX;
        float tableWidth = MathF.Max(1f, NativeImGui.GetContentRegionAvail().X);
        NativeImGui.PushStyleVar(
            (int)ImGuiStyleVar.CellPadding,
            EditorWidget.style.settingsFieldPadding);
        bool tableStarted = false;
        try
        {
            tableStarted = NativeImGui.BeginTable(
                "##settings_section",
                3,
                (int)flags,
                new System.Numerics.Vector2(tableWidth, 0f));
            if (!tableStarted)
                return;

            EditorWidget.SetupPropertyColumns();
            NativeImGui.TableSetupColumn(
                "##settings_reset",
                (int)ImGuiTableColumnFlags.WidthFixed,
                GetResetColumnWidth());
            foreach (SettingsField setting in settings)
                DrawFieldRow(setting, fieldIndex++);
        }
        finally
        {
            if (tableStarted)
                NativeImGui.EndTable();
            NativeImGui.PopStyleVar();
        }
    }

    private void DrawFieldRow(
        SettingsField setting,
        int fieldIndex
    ) {
        NativeImGui.TableNextRow();
        uint background = NativeImGui.ColorConvertFloat4ToU32(
            fieldIndex % 2 == 0
                ? EditorPalette.settingsField
                : EditorPalette.settingsFieldAlternate);
        NativeImGui.TableSetBgColor((int)ImGuiTableBgTarget.RowBg0, background);
        NativeImGui.TableSetBgColor((int)ImGuiTableBgTarget.RowBg1, background);

        NativeImGui.PushID(setting.path);
        try
        {
            _ = NativeImGui.TableSetColumnIndex(0);
            NativeImGui.SetCursorPosX(
                NativeImGui.GetCursorPosX() + NativeImGui.Raw.GetStyle()->WindowPadding.X);
            EditorWidget.PropertyLabel(setting.label, setting.description);

            _ = NativeImGui.TableSetColumnIndex(1);
            bool groupStarted = false;
            try
            {
                NativeImGui.BeginGroup();
                groupStarted = true;
                session.UpdateDirty(setting, session.Draw(setting));
                NativeImGui.EndGroup();
                groupStarted = false;

                _ = NativeImGui.TableSetColumnIndex(2);
                bool canReset = session.CanReset(setting);
                if (DrawResetButton(canReset))
                    session.Reset(setting);
            }
            finally
            {
                if (groupStarted)
                    NativeImGui.EndGroup();
            }
        }
        finally
        {
            NativeImGui.PopID();
        }
        DrawFieldBackgroundGutters(background);
    }

    private static void DrawFieldBackgroundGutters(uint background)
    {
        ImGuiTable* table = NativeImGui.Raw.GetCurrentTable();
        ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
        float tableMinimumX = table->OuterRect.Min.X;
        float tableMaximumX = table->OuterRect.Max.X;
        float rowMinimumY = table->RowPosY1;
        float rowMaximumY = table->RowPosY2;
        float windowMinimumX = window->InnerRect.Min.X;
        float windowMaximumX = window->InnerRect.Max.X;
        if (rowMaximumY <= rowMinimumY)
            return;

        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        NativeVector2 clipMinimum = Unsafe.BitCast<NumericsVector2, NativeVector2>(new System.Numerics.Vector2(windowMinimumX, rowMinimumY));
        NativeVector2 clipMaximum = Unsafe.BitCast<NumericsVector2, NativeVector2>(new System.Numerics.Vector2(windowMaximumX, rowMaximumY));
        NativeImGui.Raw.PushClipRect(
            drawList,
            &clipMinimum,
            &clipMaximum,
            0
        );
        try
        {
            if (tableMinimumX > windowMinimumX)
            {
                EditorImGui.DrawFilledRectangle(
                    new System.Numerics.Vector2(windowMinimumX, rowMinimumY),
                    new System.Numerics.Vector2(tableMinimumX, rowMaximumY),
                    background,
                    0
                );
            }
            if (tableMaximumX < windowMaximumX)
            {
                EditorImGui.DrawFilledRectangle(
                    new System.Numerics.Vector2(tableMaximumX, rowMinimumY),
                    new System.Numerics.Vector2(windowMaximumX, rowMaximumY),
                    background,
                    0
                );
            }
        }
        finally
        {
            NativeImGui.Raw.PopClipRect(drawList);
        }
    }

    private static float GetResetColumnWidth()
    {
        ImGuiStyle* style = NativeImGui.Raw.GetStyle();
        return style->ItemSpacing.X +
               NativeImGui.CalcTextSize("Reset").X +
               style->FramePadding.X * 2f +
               style->WindowPadding.X;
    }

    private static bool DrawResetButton(bool enabled)
    {
        ImGuiStyle* style = NativeImGui.Raw.GetStyle();
        NativeImGui.SetCursorPosX(NativeImGui.GetCursorPosX() + style->ItemSpacing.X);
        NativeImGui.BeginDisabled(!enabled);
        try
        {
            return NativeImGui.SmallButton("Reset");
        }
        finally
        {
            NativeImGui.EndDisabled();
        }
    }

    private static void PushDisabledText()
        => NativeImGui.PushStyleColor(
            (int)ImGuiCol.Text,
            Unsafe.BitCast<NativeVector4, NumericsVector4>(NativeImGui.Raw.GetStyle()->Colors[(int)ImGuiCol.TextDisabled]));

}
