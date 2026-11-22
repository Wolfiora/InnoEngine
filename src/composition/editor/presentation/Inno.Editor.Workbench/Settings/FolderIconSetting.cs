using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/Folder")]
internal sealed class FolderIconSetting() : IconSetting(ImGuiIcon.C_FOLDER)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a source directory.";
}
