using System;
using Inno.Editor.ImGui;
using Inno.Editor.Settings;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Rendering/Maximum Frame Rate", order: 20)]
internal sealed class MaximumFrameRateSetting : EditorSetting
{
    /// <summary>
    /// Gets a new value initialized to this setting's canonical default state.
    /// </summary>
    public override EditorSettingObject defaultValue
    {
        get
        {
            var result = new EditorSettingObject();
            result.SetAsInt32("value", 0);
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
    public override string description => "Software frame-rate limit. Zero means Unlimited. Fixed-step simulation is unaffected.";

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value currently being presented.
    /// </param>
    protected override void OnDraw(EditorSettingObject setting)
    {
        int rate = setting.GetAsInt32("value", 0);
        if (Gui.InputInt("##maximum_fps", ref rate, 1, 100, 0))
        {
            setting.SetAsInt32("value", Math.Clamp(rate, 0, 1000));
        }
        if (rate == 0)
        {
            EditorUI.current.widgets.ColoredText(EditorPalette.textDisabled, "Unlimited");
        }
    }
}
