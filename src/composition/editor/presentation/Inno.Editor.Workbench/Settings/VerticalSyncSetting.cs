using Inno.Editor.Settings;

using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Rendering/Vertical Sync", order: 10)]
internal sealed class VerticalSyncSetting : EditorSetting
{
    /// <summary>
    /// Gets a new value initialized to this setting's canonical default state.
    /// </summary>
    public override EditorSettingObject defaultValue
    {
        get
        {
            var result = new EditorSettingObject();
            result.SetAsBoolean("enabled", false);
            return result;
        }
    }

    /// <summary>
    /// Gets the presentation section that groups this setting.
    /// </summary>
    public override string section => "Frame Rate";

    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Synchronize presentation to the display refresh rate. Disable to allow higher frame rates.";

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value currently being presented.
    /// </param>
    protected override void OnDraw(EditorSettingObject setting)
    {
        bool enabled = setting.GetAsBoolean("enabled", false);
        if (Gui.Checkbox("##vsync", ref enabled))
        {
            setting.SetAsBoolean("enabled", enabled);
        }
    }
}
