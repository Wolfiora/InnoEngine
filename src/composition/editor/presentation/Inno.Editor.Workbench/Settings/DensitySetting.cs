using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;
[EditorSettingPath("Editor/Appearance/Density", order: 50)]
internal sealed class DensitySetting : EditorSetting
{
    /// <summary>
    /// Gets the comfortable default density.
    /// </summary>
    public override EditorSettingObject defaultValue => CreateDefault();
    /// <summary>
    /// Gets the appearance section containing this setting.
    /// </summary>
    public override string section => "Layout";
    /// <summary>
    /// Gets the user-facing density description.
    /// </summary>
    public override string description => "Use comfortable spacing by default, or fit more controls with compact density.";

    /// <summary>
    /// Draws the density selector.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value.
    /// </param>
    protected override void OnDraw(EditorSettingObject setting)
    {
        bool compact = setting.GetAsBoolean("compact", false);
        string preview = compact ? "Compact" : "Comfortable";
        Ui.SetNextItemWidth(-1f);
        if (!EditorUI.current.widgets.BeginBoundedCombo("##editor_density", preview))
            return;
        try
        {
            if (Ui.Selectable("Comfortable", !compact))
                setting.SetAsBoolean("compact", false);
            if (Ui.Selectable("Compact", compact))
                setting.SetAsBoolean("compact", true);
        }
        finally
        {
            EditorUI.current.widgets.EndBoundedCombo();
        }
    }

    private static EditorSettingObject CreateDefault()
    {
        var result = new EditorSettingObject();
        result.SetAsBoolean("compact", false);
        return result;
    }
}
