using UiDrawing = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using Inno.Core.Settings;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;
using Inno.Scene.Layers;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;
[ProjectSettingPath("Project/Scene/Layers")]
internal sealed class GameLayersSetting : ProjectSettingEditor<GameLayerCatalog>
{
    private const int C_LAYER_NAME_BUFFER_SIZE = 128;
    private readonly string[] m_nameBuffers = new string[GameLayer.C_MAX_COUNT];
    private string m_error = string.Empty;
    private string? []? m_observedNames;
    /// <summary>
    /// Gets the stable project-setting identity used for discovery and persistence.
    /// </summary>
    /// <summary>
    /// Gets the presentation section that groups this setting.
    /// </summary>
    public override string section => "Definitions";
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Define layer names and slots. Stable IDs are generated as projectId.name and are never authored manually.";

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value currently being presented.
    /// </param>
    protected override void OnDraw(GameLayerCatalog setting)
    {
        string? [] names = CaptureNames(setting);
        SynchronizeBuffers(names);
        DrawLayerRows(setting, names);
        if (!string.IsNullOrEmpty(m_error))
            EditorUI.current.widgets.ColoredText(EditorPalette.error, m_error);
    }

    private void DrawLayerRows(
        GameLayerCatalog setting,
        string? [] names
    ) {
        int definedCount = names.Count(static name => !string.IsNullOrWhiteSpace(name));
        DrawLayerToolbar(setting, names, definedCount);
        Ui.Spacing();
        ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.NoSavedSettings;
        Ui.PushStyleVar(ImGuiStyleVar.CellPadding, EditorUI.current.widgets.style.cellPadding);
        if (!Ui.BeginTable("##game_layer_definitions", 3, flags, default(System.Numerics.Vector2), 0f))
        {
            Ui.PopStyleVar();
            return;
        }

        try
        {
            Ui.TableSetupColumn("Slot", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), 48f * EditorUI.current.widgets.style.zoom);
            Ui.TableSetupColumn("Name", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize));
            Ui.TableSetupColumn("Action", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), 72f * EditorUI.current.widgets.style.zoom);
            DrawLayerTableHeader();
            for (int index = 0; index < GameLayer.C_MAX_COUNT; index++)
            {
                if (!string.IsNullOrWhiteSpace(names[index]))
                    DrawLayerRow(setting, names, index);
            }
        }
        finally
        {
            Ui.EndTable();
            Ui.PopStyleVar();
        }
    }

    private void DrawLayerRow(
        GameLayerCatalog setting,
        IReadOnlyList<string?> names,
        int index
    ) {
        var layer = new GameLayer(index);
        Ui.TableNextRow();
        _ = Ui.TableSetColumnIndex(0);
        Ui.AlignTextToFramePadding();
        InsetPlainCell();
        Ui.Text(index.ToString("00", CultureInfo.InvariantCulture));
        _ = Ui.TableSetColumnIndex(1);
        if (layer == GameLayer.defaultLayer)
        {
            InsetPlainCell();
            Ui.Text(names[index] ?? "Default");
            _ = Ui.TableSetColumnIndex(2);
            InsetPlainCell();
            EditorUI.current.widgets.ColoredText(EditorPalette.textDisabled, "Fixed");
            return;
        }

        Ui.PushId(index);
        try
        {
            string name = m_nameBuffers[index];
            bool committed = DrawTextField("##layer_name", "Layer name", ref name);
            m_nameBuffers[index] = name;
            if (committed)
                CommitDefinition(setting, layer, name);
            _ = Ui.TableSetColumnIndex(2);
            InsetPlainCell();
            Vector2 removeSize = new(Ui.MeasureText("Remove").X, Ui.GetFrameHeight());
            if (EditorUI.current.widgets.ClickableText($"remove_layer_{index}", "Remove", removeSize, "Remove this definition while preserving Scene slot assignments."))
            {
                _ = setting.Remove(layer);
                SynchronizeSlot(setting, layer);
                m_error = string.Empty;
            }
        }
        finally
        {
            Ui.PopId();
        }
    }

    private static bool DrawTextField(
        string id,
        string hint,
        ref string value
    ) {
        Ui.SetNextItemWidth(-1f);
        Ui.PushStyleColor(ImGuiCol.FrameBg, EditorPalette.transparent);
        Ui.PushStyleColor(ImGuiCol.FrameBgHovered, EditorPalette.transparent);
        Ui.PushStyleColor(ImGuiCol.FrameBgActive, EditorPalette.transparent);
        try
        {
            bool submitted = EditorImGui.InputTextWithHint(id, hint, ref value, C_LAYER_NAME_BUFFER_SIZE, ImGuiInputTextFlags.EnterReturnsTrue);
            return submitted || Ui.IsItemDeactivatedAfterEdit();
        }
        finally
        {
            Ui.PopStyleColor(3);
        }
    }

    private void DrawLayerToolbar(
        GameLayerCatalog setting,
        string? [] names,
        int definedCount
    ) {
        float spacing = UiDrawing.GetLayout().itemSpacing.X;
        EditorUI.current.widgets.LabelChip("Defined", EditorPalette.collectionRowAlternate);
        Ui.SameLine(0f, 0f);
        DrawReadOnlyCount($"{definedCount} / {GameLayer.C_MAX_COUNT}", 78f * EditorUI.current.widgets.style.zoom);
        Ui.SameLine(0f, spacing);
        DrawAddLayer(setting, names, definedCount);
    }

    private static void DrawReadOnlyCount(
        string value,
        float width
    ) {
        EditorUiLayout style = UiDrawing.GetLayout();
        Vector2 minimum = Ui.GetCursorScreenPosition();
        Vector2 size = new(MathF.Max(1f, width), Ui.GetFrameHeight());
        Ui.Dummy(size);
        Vector2 maximum = minimum + size;
        EditorImGui.DrawFilledRectangle(minimum, maximum, Ui.GetStyleColor(ImGuiCol.FrameBg), style.frameRounding);
        EditorImGui.DrawRectangle(minimum, maximum, Ui.GetStyleColor(ImGuiCol.Border), style.frameRounding, EditorUI.current.widgets.style.borderSize);
        Vector2 textSize = Ui.MeasureText(value);
        EditorImGui.DrawText(minimum + (size - textSize) * 0.5f, Ui.GetStyleColor(ImGuiCol.Text), value);
    }

    private static void DrawLayerTableHeader()
    {
        uint background = Ui.ToPackedColor(EditorPalette.collectionHeader);
        Ui.TableNextRow();
        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, background);
        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, background);
        DrawHeaderCell(0, "Slot");
        DrawHeaderCell(1, "Name");
        DrawHeaderCell(2, "Action");
    }

    private static void DrawHeaderCell(
        int column,
        string label
    ) {
        _ = Ui.TableSetColumnIndex(column);
        InsetPlainCell();
        Ui.Text(label);
    }

    private static void InsetPlainCell() => Ui.SetCursorPosX(Ui.GetCursorPosX() + UiDrawing.GetLayout().framePadding.X);
    private void DrawAddLayer(
        GameLayerCatalog setting,
        string? [] names,
        int definedCount
    ) {
        Ui.BeginDisabled(definedCount >= GameLayer.C_MAX_COUNT);
        try
        {
            float selectorWidth = MathF.Max(1f, Ui.GetContentRegionAvailable().X);
            if (!EditorUI.current.widgets.BeginMenuSelector("add_game_layer", "Add layer...", selectorWidth, selectorWidth))
            {
                return;
            }

            try
            {
                for (int index = 1; index < GameLayer.C_MAX_COUNT; index++)
                {
                    if (!string.IsNullOrWhiteSpace(names[index]))
                        continue;
                    string slot = index.ToString("00", CultureInfo.InvariantCulture);
                    if (Ui.Selectable($"{slot}  Layer {index}"))
                        CommitDefinition(setting, new GameLayer(index), $"Layer {index}");
                }
            }
            finally
            {
                EditorUI.current.widgets.EndMenuSelector();
            }
        }
        finally
        {
            Ui.EndDisabled();
        }
    }

    private void CommitDefinition(
        GameLayerCatalog setting,
        GameLayer layer,
        string name
    ) {
        try
        {
            setting.Define(layer, name);
            SynchronizeSlot(setting, layer);
            m_error = string.Empty;
        }
        catch (ArgumentException exception)
        {
            m_error = exception.Message;
            SynchronizeSlot(setting, layer);
        }
    }

    private void SynchronizeBuffers(IReadOnlyList<string?> names)
    {
        if (m_observedNames is not null && m_observedNames.SequenceEqual(names, StringComparer.Ordinal))
        {
            return;
        }

        for (int index = 0; index < GameLayer.C_MAX_COUNT; index++)
            m_nameBuffers[index] = names[index] ?? string.Empty;
        m_observedNames = names.ToArray();
    }

    private void SynchronizeSlot(
        GameLayerCatalog setting,
        GameLayer layer
    ) {
        m_nameBuffers[layer.index] = setting.GetName(layer) ?? string.Empty;
        m_observedNames = CaptureNames(setting);
    }

    private static string? [] CaptureNames(GameLayerCatalog stack)
    {
        var result = new string? [GameLayer.C_MAX_COUNT];
        for (int index = 0; index < GameLayer.C_MAX_COUNT; index++)
            result[index] = stack.GetName(new GameLayer(index));
        return result;
    }
}
