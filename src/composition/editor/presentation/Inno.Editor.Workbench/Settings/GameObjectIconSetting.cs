using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/GameObject")]
internal sealed class GameObjectIconSetting() : IconSetting(ImGuiIcon.C_CUBE)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a scene GameObject.";
}
