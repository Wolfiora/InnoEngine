using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/RenderPipeline")]
internal sealed class RenderPipelineIconSetting() : IconSetting(ImGuiIcon.C_RUPIAH_SIGN)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Render Pipeline assets and their Inspector headers.";
}
