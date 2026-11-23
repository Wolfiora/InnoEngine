using Inno.Editor.ImGui;
using Inno.Editor.Settings;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Icons/ShaderSource")]
internal sealed class ShaderSourceIconSetting() : IconSetting(ImGuiIcon.C_RUPEE_SIGN)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description
        => "Selects the icon used consistently for Shader source-function libraries. The default function symbol distinguishes reusable code from complete Shader graphs.";
}
