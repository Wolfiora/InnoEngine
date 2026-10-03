using System;
using System.Numerics;
using Inno.Build;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Native.ImGui;
using EditorImGui = Inno.Editor.ImGui.ImGui;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Exporting;

[EditorModal("export.game", "Export as Game", order: 310)]
internal sealed class GameExportModal(ExportWindowModule window) : EditorModal
{
    private const int C_TEXT_CAPACITY = 4096;

    /// <summary>
    /// Gets whether this implementation is visible.
    /// </summary>
    public override bool isVisible => window.isGameVisible;

    /// <summary>
    /// Gets whether this implementation can move.
    /// </summary>
    public override bool canMove => true;

    /// <summary>
    /// Gets whether this implementation can resize.
    /// </summary>
    public override bool canResize => true;

    /// <summary>
    /// Gets the preferred initial window size in logical editor units.
    /// </summary>
    public override Vector2 initialSize => new(760f, 540f);

    /// <summary>
    /// Gets the smallest size that keeps the export form usable.
    /// </summary>
    public override Vector2 minimumSize => new(620f, 460f);

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        EditorWidget.WrappedText(
            "Build a standalone Player from the current project. Only imported runtime content " +
            "is deployed; source files stay in the project.");
        NativeImGui.SeparatorText("Application");
        if (BeginFields("##application_fields"))
        {
            DrawReadOnly("Application ID", window.gameApplicationId);
            DrawText("Product Name", "game_name", window.gameProductName, value => window.gameProductName = value);
            DrawText("Persistent Data Folder", "game_data", window.gamePersistentDataPath,
                value => window.gamePersistentDataPath = value);
            DrawText("Startup Scene", "game_scene", window.gameStartupScene, value => window.gameStartupScene = value);
            NativeImGui.EndTable();
        }
        NativeImGui.SeparatorText("Window and Platform");
        if (BeginFields("##platform_fields"))
        {
            DrawWindowSize();
            DrawTarget();
            NativeImGui.EndTable();
        }
        NativeImGui.SeparatorText("Output");
        if (BeginFields("##output_fields"))
        {
            DrawText("Output Directory", "game_output", window.gameOutputDirectory, value => window.gameOutputDirectory = value);
            NativeImGui.EndTable();
        }
        NativeImGui.Spacing();
        EditorWidget.WrappedText("Defaults: Settings > Build > Game. Application ID: Settings > Project > Identity.");
        if (!string.IsNullOrEmpty(window.error))
        {
            NativeImGui.Spacing();
            EditorWidget.WrappedText(window.error);
        }
        NativeImGui.Separator();
        DrawButtons();
    }

    private static bool BeginFields(string id)
    {
        if (!NativeImGui.BeginTable(
                id,
                2,
                ImGuiTableFlags.SizingStretchProp |
                ImGuiTableFlags.NoSavedSettings |
                ImGuiTableFlags.NoPadOuterX))
        {
            return false;
        }
        EditorWidget.SetupPropertyColumns();
        return true;
    }

    private static void BeginField(string label)
    {
        NativeImGui.TableNextRow();
        _ = NativeImGui.TableSetColumnIndex(0);
        EditorWidget.PropertyLabel(label);
        _ = NativeImGui.TableSetColumnIndex(1);
    }

    private static void DrawReadOnly(
        string label,
        string value
    ) {
        BeginField(label);
        NativeImGui.PushStyleColor(ImGuiCol.Text, EditorPalette.textDisabled);
        try
        {
            EditorWidget.WrappedText(value);
        }
        finally
        {
            NativeImGui.PopStyleColor();
        }
    }

    private void DrawWindowSize()
    {
        int width = window.gameWindowWidth;
        int height = window.gameWindowHeight;
        BeginField("Initial Width");
        NativeImGui.SetNextItemWidth(-1f);
        if (NativeImGui.InputInt("##game_window_width", ref width))
            window.gameWindowWidth = Math.Max(1, width);
        BeginField("Initial Height");
        NativeImGui.SetNextItemWidth(-1f);
        if (NativeImGui.InputInt("##game_window_height", ref height))
            window.gameWindowHeight = Math.Max(1, height);
    }

    private void DrawTarget()
    {
        BeginField("Target");
        NativeImGui.SetNextItemWidth(-1f);
        string preview = window.GetGameTargetDisplayName(window.gameTarget);
        if (!EditorWidget.BeginBoundedCombo("##game_target", preview))
            return;
        try
        {
            foreach (BuildTargetId target in window.availableGameTargets)
                DrawTargetChoice(target);
        }
        finally
        {
            EditorWidget.EndBoundedCombo();
        }
    }

    private void DrawTargetChoice(BuildTargetId target)
    {
        bool selected = window.gameTarget == target;
        if (NativeImGui.Selectable(window.GetGameTargetDisplayName(target), selected))
            window.gameTarget = target;
        if (selected)
            NativeImGui.SetItemDefaultFocus();
    }

    private void DrawButtons()
    {
        NativeImGui.Spacing();
        string primaryLabel = "Export";
        ImGuiStylePtr style = NativeImGui.GetStyle();
        float closeWidth = NativeImGui.CalcTextSize("Close").X + style.FramePadding.X * 2f;
        float primaryWidth = NativeImGui.CalcTextSize(primaryLabel).X + style.FramePadding.X * 2f;
        NativeImGui.SetCursorPosX(
            NativeImGui.GetCursorPosX() +
            MathF.Max(0f, NativeImGui.GetContentRegionAvail().X - closeWidth - style.ItemSpacing.X - primaryWidth));
        if (NativeImGui.Button("Close"))
            window.CloseGame();
        NativeImGui.SameLine();
        if (NativeImGui.Button(primaryLabel))
            window.BeginGameExport();
    }

    private static void DrawText(
        string label,
        string id,
        string value,
        Action<string> apply
    ) {
        BeginField(label);
        NativeImGui.SetNextItemWidth(-1f);
        _ = EditorImGui.InputText($"##{id}", ref value, C_TEXT_CAPACITY);
        apply(value);
    }
}
