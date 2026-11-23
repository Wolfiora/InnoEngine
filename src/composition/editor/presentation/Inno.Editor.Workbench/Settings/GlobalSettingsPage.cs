using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor")]
internal sealed class GlobalSettingsPage : EditorSetting
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Configure editor-wide appearance, behavior, and user-facing tools.";
}
