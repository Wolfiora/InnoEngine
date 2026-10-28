using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Numerics;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;
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
        EditorUI.current.widgets.WrappedText("Build a standalone Player from the current project. Only imported runtime content " + "is deployed; source files stay in the project.");
        Ui.SeparatorText("Application");
        if (BeginFields("##application_fields"))
        {
            DrawReadOnly("Application ID", window.gameApplicationId);
            DrawText("Product Name", "game_name", window.gameProductName, value => window.gameProductName = value);
            DrawText("Persistent Data Folder", "game_data", window.gamePersistentDataPath, value => window.gamePersistentDataPath = value);
            DrawText("Startup Scene", "game_scene", window.gameStartupScene, value => window.gameStartupScene = value);
            Ui.EndTable();
        }

        Ui.SeparatorText("Window and Platform");
        if (BeginFields("##platform_fields"))
        {
            DrawWindowSize();
            DrawTarget();
            DrawManagedDeployment();
            Ui.EndTable();
        }

        Ui.SeparatorText("Output");
        if (BeginFields("##output_fields"))
        {
            DrawText("Output Directory", "game_output", window.gameOutputDirectory, value => window.gameOutputDirectory = value);
            Ui.EndTable();
        }

        Ui.Spacing();
        EditorUI.current.widgets.WrappedText("Defaults: Settings > Build > Game. Application ID: Settings > Project > Identity.");
        if (!string.IsNullOrEmpty(window.error))
        {
            Ui.Spacing();
            EditorUI.current.widgets.WrappedText(window.error);
        }

        Ui.Separator();
        DrawButtons();
    }

    private static bool BeginFields(string id)
    {
        if (!Ui.BeginTable(id, 2, (ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.NoPadOuterX), default(System.Numerics.Vector2), 0f))
        {
            return false;
        }

        EditorUI.current.widgets.SetupPropertyColumns();
        return true;
    }

    private static void BeginField(string label)
    {
        Ui.TableNextRow();
        _ = Ui.TableSetColumnIndex(0);
        EditorUI.current.widgets.PropertyLabel(label);
        _ = Ui.TableSetColumnIndex(1);
    }

    private static void DrawReadOnly(
        string label,
        string value
    ) {
        BeginField(label);
        Ui.PushStyleColor((ImGuiCol.Text), EditorPalette.textDisabled);
        try
        {
            EditorUI.current.widgets.WrappedText(value);
        }
        finally
        {
            Ui.PopStyleColor();
        }
    }

    private void DrawWindowSize()
    {
        int width = window.gameWindowWidth;
        int height = window.gameWindowHeight;
        BeginField("Initial Width");
        Ui.SetNextItemWidth(-1f);
        if (Ui.InputInt("##game_window_width", ref width, 1, 100, 0))
            window.gameWindowWidth = Math.Max(1, width);
        BeginField("Initial Height");
        Ui.SetNextItemWidth(-1f);
        if (Ui.InputInt("##game_window_height", ref height, 1, 100, 0))
            window.gameWindowHeight = Math.Max(1, height);
    }

    private void DrawManagedDeployment()
    {
        BeginField("Managed Deployment");
        Ui.SetNextItemWidth(-1f);
        if (!EditorUI.current.widgets.BeginBoundedCombo("##game_deployment", window.gameManagedDeployment?.value ?? "Platform default"))
            return;
        try
        {
            if (Ui.Selectable("Platform default", window.gameManagedDeployment is null))
                window.gameManagedDeployment = null;
            foreach (ManagedDeploymentId deployment in window.availableManagedDeployments)
                if (Ui.Selectable(deployment.value, window.gameManagedDeployment == deployment))
                    window.gameManagedDeployment = deployment;
        }
        finally
        {
            EditorUI.current.widgets.EndBoundedCombo();
        }
    }

    private void DrawTarget()
    {
        BeginField("Target");
        Ui.SetNextItemWidth(-1f);
        string preview = window.GetGameTargetDisplayName(window.gameTarget);
        if (!EditorUI.current.widgets.BeginBoundedCombo("##game_target", preview))
            return;
        try
        {
            foreach (BuildTargetId target in window.availableGameTargets)
                DrawTargetChoice(target);
        }
        finally
        {
            EditorUI.current.widgets.EndBoundedCombo();
        }
    }

    private void DrawTargetChoice(BuildTargetId target)
    {
        bool selected = window.gameTarget == target;
        if (Ui.Selectable(window.GetGameTargetDisplayName(target), selected))
            window.gameTarget = target;
        if (selected)
            Ui.SetItemDefaultFocus();
    }

    private void DrawButtons()
    {
        Ui.Spacing();
        string primaryLabel = "Export";
        EditorUiLayout style = Ui.GetLayout();
        float closeWidth = Ui.MeasureText("Close").X + style.framePadding.X * 2f;
        float primaryWidth = Ui.MeasureText(primaryLabel).X + style.framePadding.X * 2f;
        Ui.SetCursorPosX(Ui.GetCursorPosX() + MathF.Max(0f, Ui.GetContentRegionAvailable().X - closeWidth - style.itemSpacing.X - primaryWidth));
        if (Ui.Button("Close"))
            window.CloseGame();
        Ui.SameLine(0f, -1f);
        if (Ui.Button(primaryLabel))
            window.BeginGameExport();
    }

    private static void DrawText(
        string label,
        string id,
        string value,
        Action<string> apply
    ) {
        BeginField(label);
        Ui.SetNextItemWidth(-1f);
        _ = Ui.InputText($"##{id}", ref value, C_TEXT_CAPACITY);
        apply(value);
    }
}
