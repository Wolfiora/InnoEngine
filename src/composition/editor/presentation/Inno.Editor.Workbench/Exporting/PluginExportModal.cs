using Inno.Editor.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Numerics;
using Inno.Editor.Core;
using Inno.Editor.ImGui.ImGuiWidget;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;
[EditorModal("export.plugin", "Export as Plugin", order: 300)]
internal sealed class PluginExportModal(ExportWindowModule window) : EditorModal
{
    private const int C_TEXT_CAPACITY = 4096;
    /// <summary>
    /// Gets whether this implementation is visible.
    /// </summary>
    public override bool isVisible => window.isPluginVisible;
    /// <summary>
    /// Gets whether this implementation can move.
    /// </summary>
    public override bool canMove => true;
    /// <summary>
    /// Gets the preferred initial window size in logical editor units.
    /// </summary>
    public override Vector2 initialSize => new(680f, 0f);

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnDraw(EditorContext context)
    {
        EditorUI.current.widgets.WrappedText("Packages the current project's complete authored source. Plugin.inno, dependency declarations, " + "settings contributions, and deterministic metadata are generated automatically.");
        Ui.Spacing();
        Ui.BeginDisabled(window.isPluginBusy);
        try
        {
            Ui.Text("Plugin ID");
            EditorUI.current.widgets.ColoredText(Inno.Editor.ImGui.EditorPalette.textDisabled, $"{window.pluginId} (from Project ID)");
            DrawText("Display Name", "plugin_name", window.pluginDisplayName, value => window.pluginDisplayName = value);
            DrawText("Destination IPlugin", "plugin_output", window.pluginOutputPath, value => window.pluginOutputPath = value);
            bool includeDependencies = window.includePluginDependencies;
            if (Ui.Checkbox("Include dependency Plugin packages", ref includeDependencies))
                window.includePluginDependencies = includeDependencies;
        }
        finally
        {
            Ui.EndDisabled();
        }

        EditorUI.current.widgets.ColoredText(
            Inno.Editor.ImGui.EditorPalette.textDisabled,
            "Identity comes from Settings > Project > Identity. Other defaults come from Build > Plugin."
        );
        DrawOutcome();
        DrawButtons();
    }

    private void DrawOutcome()
    {
        if (!string.IsNullOrEmpty(window.error))
        {
            Ui.Spacing();
            EditorUI.current.widgets.WrappedText(window.error);
        }
        else if (!string.IsNullOrEmpty(window.status))
        {
            Ui.Spacing();
            EditorUI.current.widgets.WrappedText(window.status);
        }
    }

    private void DrawButtons()
    {
        Ui.Spacing();
        EditorUiLayout style = Ui.GetLayout();
        float closeWidth = Ui.MeasureText("Close").X + style.framePadding.X * 2f;
        string primaryLabel = window.isPluginBusy ? "Cancel" : "Export";
        float exportWidth = Ui.MeasureText(primaryLabel).X + style.framePadding.X * 2f;
        Ui.SetCursorPosX(Ui.GetCursorPosX() + MathF.Max(0f, Ui.GetContentRegionAvailable().X - closeWidth - style.itemSpacing.X - exportWidth));
        Ui.BeginDisabled(window.isPluginBusy);
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
        if (Ui.Button(primaryLabel))
        {
            if (window.isPluginBusy)
                window.CancelPluginExport();
            else
                window.BeginPluginExport();
        }
    }

    private static void DrawText(
        string label,
        string id,
        string value,
        Action<string> apply
    ) {
        Ui.Text(label);
        Ui.SetNextItemWidth(-1f);
        _ = Ui.InputText($"##{id}", ref value, C_TEXT_CAPACITY);
        apply(value);
    }
}
