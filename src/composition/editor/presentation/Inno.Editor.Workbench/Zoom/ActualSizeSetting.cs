using System;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;

[EditorSettingPath("Editor/Appearance/Accessibility/Actual Size")]
internal sealed class ActualSizeSetting : EditorSetting
{
    private static readonly (float value, string label)[] CHOICES =
    [
        (0.75f, "75%"),
        (0.9f, "90%"),
        (1f, "100%"),
        (1.1f, "110%"),
        (1.25f, "125%"),
        (1.5f, "150%")
    ];

    /// <summary>
    /// Gets a new value initialized to this setting's canonical default state.
    /// </summary>
    public override EditorSettingObject defaultValue => CreateDefault();

    /// <summary>
    /// Gets the presentation section that groups this setting.
    /// </summary>
    public override string section => "Font";

    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Set the editor's actual font, spacing, control, and window size.";

    /// <summary>
    /// Draws this feature using the current editor presentation context.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value currently being presented.
    /// </param>
    protected override void OnDraw(EditorSettingObject setting)
    {
        float value = setting.GetAsSingle("value", 1f);
        string preview = GetLabel(value);
        Gui.SetNextItemWidth(-1f);
        if (!EditorUI.current.widgets.BeginBoundedCombo("##actual_size", preview))
        {
            return;
        }
        try
        {
            for (int i = 0; i < CHOICES.Length; i++)
            {
                (float candidate, string label) = CHOICES[i];
                if (Gui.Selectable(label, MathF.Abs(candidate - value) < 0.0001f))
                {
                    setting.SetAsSingle("value", candidate);
                }
            }
        }
        finally
        {
            EditorUI.current.widgets.EndBoundedCombo();
        }
    }

    private static EditorSettingObject CreateDefault()
    {
        var result = new EditorSettingObject();
        result.SetAsSingle("value", 1f);
        return result;
    }

    private static string GetLabel(float value)
    {
        for (int i = 0; i < CHOICES.Length; i++)
        {
            if (MathF.Abs(CHOICES[i].value - value) < 0.0001f)
            {
                return CHOICES[i].label;
            }
        }

        return $"{value * 100f:0}%";
    }
}
