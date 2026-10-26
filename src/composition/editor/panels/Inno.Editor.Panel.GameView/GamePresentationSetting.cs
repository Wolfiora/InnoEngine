using System;
using Inno.Core.Settings;
using Inno.Editor.Settings;
using Inno.Runtime;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.GameView;
[ProjectSettingPath("Project/Player/Presentation")]
internal sealed class GamePresentationSetting : ProjectSettingEditor<GamePresentationSettings>
{
    /// <summary>
    /// Gets the stable project-setting identity edited by this presentation.
    /// </summary>
    /// <summary>
    /// Gets the section that groups Player presentation controls.
    /// </summary>
    public override string section => "Display";
    /// <summary>
    /// Gets the explanation shared by Game View and deployed Player presentation.
    /// </summary>
    public override string description => "Controls the reference frame and aspect fitting used by both Game View and exported Players.";

    /// <summary>
    /// Draws project-wide game presentation controls.
    /// </summary>
    /// <param name="setting">
    /// Isolated mutable presentation settings staged by the Settings window.
    /// </param>
    protected override void OnDraw(GamePresentationSettings setting)
    {
        bool preserve = setting.preserveAspectRatio;
        if (Ui.Checkbox("Preserve Aspect Ratio", ref preserve))
            setting.preserveAspectRatio = preserve;
        int width = Math.Max(1, setting.referenceWidth);
        int height = Math.Max(1, setting.referenceHeight);
        Ui.BeginDisabled(!preserve);
        try
        {
            if (Ui.InputInt("Reference Width", ref width, 1, 100, 0))
                setting.referenceWidth = Math.Max(1, width);
            if (Ui.InputInt("Reference Height", ref height, 1, 100, 0))
                setting.referenceHeight = Math.Max(1, height);
        }
        finally
        {
            Ui.EndDisabled();
        }
    }
}
