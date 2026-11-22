using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/Layers")]
internal sealed class LayersIconSetting() : IconSetting(ImGuiIcon.C_LAYER_GROUP)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a project layer configuration.";
}
