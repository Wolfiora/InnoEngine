using Inno.Editor.Settings;

namespace Inno.Editor.Panel.Logging;

[EditorSettingPath("Editor/Diagnostics/Console")]
internal sealed class ConsoleSettingsPage : EditorSetting
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Configure how the Console retains ordinary logs across editor workflows.";
}
