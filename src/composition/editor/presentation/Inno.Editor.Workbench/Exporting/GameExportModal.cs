using System;
using System.Numerics;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Editor.Core;
using Inno.Editor.ImGui;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

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
        EditorUI.current.widgets.WrappedText(
            "Build a standalone Player from the current project. Only imported runtime content " + "is deployed; source files stay in the project."
        );
        Gui.SeparatorText("Application");
        if (BeginFields("##application_fields"))
        {
            try
            {
                EditorUI.current.widgets.SetupPropertyColumns();
                DrawReadOnly("Application ID", window.gameApplicationID);
                DrawText("Product Name", "game_name", window.gameProductName, value => window.gameProductName = value);
                DrawText(
                    "Persistent Data Folder",
                    "game_data",
                    window.gamePersistentDataPath,
                    value => window.gamePersistentDataPath = value
                );
                DrawText("Startup Scene", "game_scene", window.gameStartupScene, value => window.gameStartupScene = value);
            }
            finally
            {
                Gui.EndTable();
            }
        }

        Gui.SeparatorText("Window and Platform");
        if (BeginFields("##platform_fields"))
        {
            try
            {
                EditorUI.current.widgets.SetupPropertyColumns();
                DrawWindowSize();
                DrawTarget();
                DrawManagedDeployment();
            }
            finally
            {
                Gui.EndTable();
            }
        }

        Gui.SeparatorText("Output");
        if (BeginFields("##output_fields"))
        {
            try
            {
                EditorUI.current.widgets.SetupPropertyColumns();
                DrawText("Output Directory", "game_output", window.gameOutputDirectory, value => window.gameOutputDirectory = value);
            }
            finally
            {
                Gui.EndTable();
            }
        }

        Gui.Spacing();
        EditorUI.current.widgets.WrappedText("Defaults: Settings > Build > Game. Application ID: Settings > Project > Identity.");
        if (!string.IsNullOrEmpty(window.error))
        {
            Gui.Spacing();
            EditorUI.current.widgets.WrappedText(window.error);
        }

        Gui.Separator();
        DrawButtons();
    }

    private void DrawWindowSize()
    {
        int width = window.gameWindowWidth;
        int height = window.gameWindowHeight;
        BeginField("Initial Width");
        Gui.SetNextItemWidth(-1f);
        if (Gui.InputInt("##game_window_width", ref width, 1, 100, 0))
        {
            window.gameWindowWidth = Math.Max(1, width);
        }
        BeginField("Initial Height");
        Gui.SetNextItemWidth(-1f);
        if (Gui.InputInt("##game_window_height", ref height, 1, 100, 0))
        {
            window.gameWindowHeight = Math.Max(1, height);
        }
    }

    private void DrawManagedDeployment()
    {
        BeginField("Managed Deployment");
        Gui.SetNextItemWidth(-1f);
        if (!EditorUI.current.widgets.BeginBoundedCombo("##game_deployment", window.gameManagedDeployment?.value ?? "Platform default"))
        {
            return;
        }
        try
        {
            if (Gui.Selectable("Platform default", window.gameManagedDeployment is null))
            {
                window.gameManagedDeployment = null;
            }
            foreach (ManagedDeploymentID deployment in window.availableManagedDeployments)
            {
                if (Gui.Selectable(deployment.value, window.gameManagedDeployment == deployment))
                {
                    window.gameManagedDeployment = deployment;
                }
            }
        }
        finally
        {
            EditorUI.current.widgets.EndBoundedCombo();
        }
    }

    private void DrawTarget()
    {
        BeginField("Target");
        Gui.SetNextItemWidth(-1f);
        string preview = window.GetGameTargetDisplayName(window.gameTarget);
        if (!EditorUI.current.widgets.BeginBoundedCombo("##game_target", preview))
        {
            return;
        }
        try
        {
            foreach (BuildTargetID target in window.availableGameTargets)
            {
                DrawTargetChoice(target);
            }
        }
        finally
        {
            EditorUI.current.widgets.EndBoundedCombo();
        }
    }

    private void DrawTargetChoice(BuildTargetID target)
    {
        bool selected = window.gameTarget == target;
        if (Gui.Selectable(window.GetGameTargetDisplayName(target), selected))
        {
            window.gameTarget = target;
        }
        if (selected)
        {
            Gui.SetItemDefaultFocus();
        }
    }

    private void DrawButtons()
    {
        Gui.Spacing();
        string primaryLabel = "Export";
        EditorUILayout style = Gui.GetLayout();
        float closeWidth = Gui.MeasureText("Close").X + style.framePadding.X * 2f;
        float primaryWidth = Gui.MeasureText(primaryLabel).X + style.framePadding.X * 2f;
        Gui.SetCursorPosX(
            Gui.GetCursorPosX() + MathF.Max(0f, Gui.GetContentRegionAvailable().X - closeWidth - style.itemSpacing.X - primaryWidth)
        );
        if (Gui.Button("Close"))
        {
            window.CloseGame();
        }
        Gui.SameLine(0f, -1f);
        if (Gui.Button(primaryLabel))
        {
            window.BeginGameExport();
        }
    }

    private static bool BeginFields(string id)
        => Gui.BeginTable(
            id,
            2,
            ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings | ImGuiTableFlags.NoPadOuterX,
            default,
            0f
        );

    private static void BeginField(string label)
    {
        Gui.TableNextRow();
        _ = Gui.TableSetColumnIndex(0);
        EditorUI.current.widgets.PropertyLabel(label);
        _ = Gui.TableSetColumnIndex(1);
    }

    private static void DrawReadOnly(
        string label,
        string value
    ) {
        BeginField(label);
        Gui.PushStyleColor((ImGuiCol.Text), EditorPalette.textDisabled);
        try
        {
            EditorUI.current.widgets.WrappedText(value);
        }
        finally
        {
            Gui.PopStyleColor();
        }
    }

    private static void DrawText(
        string label,
        string id,
        string value,
        Action<string> apply
    ) {
        BeginField(label);
        Gui.SetNextItemWidth(-1f);
        _ = Gui.InputText($"##{id}", ref value, C_TEXT_CAPACITY);
        apply(value);
    }
}
