using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/Scene")]
internal sealed class SceneIconSetting() : IconSetting(ImGuiIcon.C_CUBES)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a scene document or loaded scene.";
}
