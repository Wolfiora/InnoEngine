using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance")]
internal sealed class AppearanceSettingsPage : EditorSetting
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Customize editor appearance, scaling, and semantic presentation.";
}
