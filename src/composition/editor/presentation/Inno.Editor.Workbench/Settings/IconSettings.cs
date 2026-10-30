using Inno.Editor.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;
internal abstract class IconSetting : EditorSetting
{
    private static readonly IReadOnlyList<(string Name, string Glyph)> C_ICONS = EditorIconCatalog.standard;
    private readonly string m_glyph;
    /// <summary>
    /// Creates a validated icon setting instance.
    /// </summary>
    /// <param name="glyph">
    /// The glyph text validated by the icon setting operation.
    /// </param>
    protected IconSetting(string glyph)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(glyph);
        m_glyph = glyph;
    }

    /// <summary>
    /// Gets a new value initialized to this setting's canonical default state.
    /// </summary>
    public override EditorSettingObject defaultValue => CreateDefault(m_glyph);
    /// <summary>
    /// Gets the presentation section that groups this setting.
    /// </summary>
    public override string section => "Editor Icons";

    /// <summary>
    /// Renders this feature using the current editor presentation context.
    /// </summary>
    /// <param name="setting">
    /// The mutable editor setting value currently being presented.
    /// </param>
    protected override void OnDraw(EditorSettingObject setting)
    {
        string value = setting.GetAsString("value", m_glyph) ?? m_glyph;
        float iconSlotWidth = GetIconSlotWidth();
        Ui.SetNextItemWidth(-1f);
        bool isOpen = EditorUI.current.widgets.BeginBoundedCombo("##icon", value + "  " + GetIconName(value));
        if (isOpen)
        {
            try
            {
                for (int i = 0; i < C_ICONS.Count; i++)
                {
                    (string name, string candidate) = C_ICONS[i];
                    Ui.PushId(name);
                    try
                    {
                        bool selected = string.Equals(candidate, value, StringComparison.Ordinal);
                        if (Ui.Selectable("##icon_option", selected, ImGuiSelectableFlags.None, new Vector2(0f, Ui.GetFrameHeight())))
                        {
                            value = candidate;
                            setting.SetAsString("value", candidate);
                        }

                        Vector2 minimum = Ui.GetItemMinimum();
                        Vector2 maximum = Ui.GetItemMaximum();
                        EditorUiLayout style = Ui.GetLayout();
                        float lineHeight = Ui.GetTextLineHeight();
                        DrawIconTextAt(
                            Ui.GetDrawList(),
                            new Vector2(minimum.X + style.framePadding.X, minimum.Y + MathF.Max(0f, (maximum.Y - minimum.Y - lineHeight) * 0.5f)),
                            candidate,
                            name,
                            iconSlotWidth
                        );
                        if (selected)
                            Ui.SetItemDefaultFocus();
                    }
                    finally
                    {
                        Ui.PopId();
                    }
                }
            }
            finally
            {
                EditorUI.current.widgets.EndBoundedCombo();
            }
        }
    }

    private static EditorSettingObject CreateDefault(string glyph)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(glyph);
        var result = new EditorSettingObject();
        result.SetAsString("value", glyph);
        return result;
    }

    private static float GetIconSlotWidth() => EditorUI.current.widgets.GetIconCatalogWidth();
    private static string GetIconName(string glyph)
    {
        for (int i = 0; i < C_ICONS.Count; i++)
        {
            if (string.Equals(C_ICONS[i].Glyph, glyph, StringComparison.Ordinal))
                return C_ICONS[i].Name;
        }

        return glyph;
    }

    private static void DrawIconTextAt(
        EditorDrawList drawList,
        Vector2 origin,
        string glyph,
        string text,
        float iconSlotWidth
    ) {
        EditorUiLayout style = Ui.GetLayout();
        float fontSize = Ui.GetFontSize();
        float lineHeight = Ui.GetTextLineHeight();
        uint color = Ui.GetStyleColor(ImGuiCol.Text);
        EditorUI.current.widgets.AddGlyphCentered(fontSize, glyph, new Vector2(origin.X + iconSlotWidth * 0.5f, origin.Y + lineHeight * 0.5f), color);
        Inno.Editor.ImGui.ImGui.DrawText(new Vector2(origin.X + iconSlotWidth + style.itemInnerSpacing.X, origin.Y), color, text);
    }
}

[EditorSettingPath("Editor/Appearance/Icons/Scene")]
internal sealed class SceneIconSetting() : IconSetting(ImGuiIcon.Cubes)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a scene document or loaded scene.";
}

[EditorSettingPath("Editor/Appearance/Icons/GameObject")]
internal sealed class GameObjectIconSetting() : IconSetting(ImGuiIcon.Cube)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a scene GameObject.";
}

[EditorSettingPath("Editor/Appearance/Icons/Prefab")]
internal sealed class PrefabIconSetting() : IconSetting(ImGuiIcon.DiceD6)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a reusable prefab object.";
}

[EditorSettingPath("Editor/Appearance/Icons/Layers")]
internal sealed class LayersIconSetting() : IconSetting(ImGuiIcon.LayerGroup)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a project layer configuration.";
}

[EditorSettingPath("Editor/Appearance/Icons/Folder")]
internal sealed class FolderIconSetting() : IconSetting(ImGuiIcon.Folder)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a source directory.";
}

[EditorSettingPath("Editor/Appearance/Icons/File")]
internal sealed class FileIconSetting() : IconSetting(ImGuiIcon.File)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a generic source file.";
}

[EditorSettingPath("Editor/Appearance/Icons/Shader")]
internal sealed class ShaderIconSetting() : IconSetting(ImGuiIcon.Shader)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Shader graph assets and their Inspector headers.";
}

[EditorSettingPath("Editor/Appearance/Icons/ShaderSource")]
internal sealed class ShaderSourceIconSetting() : IconSetting(ImGuiIcon.RupeeSign)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Shader source-function libraries. The default function symbol distinguishes reusable code from complete Shader graphs.";
}

[EditorSettingPath("Editor/Appearance/Icons/Material")]
internal sealed class MaterialIconSetting() : IconSetting(ImGuiIcon.Palette)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Material assets and their Inspector headers.";
}

[EditorSettingPath("Editor/Appearance/Icons/RenderPipeline")]
internal sealed class RenderPipelineIconSetting() : IconSetting(ImGuiIcon.RupiahSign)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used consistently for Render Pipeline assets and their Inspector headers.";
}
