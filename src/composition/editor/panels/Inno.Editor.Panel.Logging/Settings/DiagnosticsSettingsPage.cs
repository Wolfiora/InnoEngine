using Inno.Editor.Settings;

namespace Inno.Editor.Panel.Logging;

[EditorSettingPath("Editor/Diagnostics")]
internal sealed class DiagnosticsSettingsPage : EditorSetting
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Configure diagnostic presentation and retention behavior for this editor.";
}
