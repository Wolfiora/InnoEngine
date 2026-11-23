using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/Material")]
internal sealed class MaterialIconSetting() : IconSetting(ImGuiIcon.C_PALETTE)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Material assets and their Inspector headers.";
}
