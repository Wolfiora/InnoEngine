using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/Shader")]
internal sealed class ShaderIconSetting() : IconSetting(ImGuiIcon.C_SHADER)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Shader graph assets and their Inspector headers.";
}
