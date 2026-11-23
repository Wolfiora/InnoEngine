using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/Prefab")]
internal sealed class PrefabIconSetting() : IconSetting(ImGuiIcon.C_DICE_D_6)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a reusable prefab object.";
}
