using System;
using System.Collections.Generic;
using System.Numerics;
using Inno.Editor.ImGui;
using Inno.Editor.Settings;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Workbench;

internal abstract class IconSetting : EditorSetting
{
    private static readonly IReadOnlyList<(string name, string glyph)> ICONS = EditorIconCatalog.standard;
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
        Gui.SetNextItemWidth(-1f);
        bool isOpen = EditorUI.current.widgets.BeginBoundedCombo("##icon", value + "  " + GetIconName(value));
        if (isOpen)
        {
            try
            {
                for (int i = 0; i < ICONS.Count; i++)
                {
                    (string name, string candidate) = ICONS[i];
                    Gui.PushID(name);
                    try
                    {
                        bool selected = string.Equals(candidate, value, StringComparison.Ordinal);
                        if (Gui.Selectable("##icon_option", selected, ImGuiSelectableFlags.None, new Vector2(0f, Gui.GetFrameHeight())))
                        {
                            value = candidate;
                            setting.SetAsString("value", candidate);
                        }

                        Vector2 minimum = Gui.GetItemMinimum();
                        Vector2 maximum = Gui.GetItemMaximum();
                        EditorUILayout style = Gui.GetLayout();
                        float lineHeight = Gui.GetTextLineHeight();
                        DrawIconTextAt(
                            Gui.GetDrawList(),
                            new Vector2(
                                minimum.X + style.framePadding.X,
                                minimum.Y + MathF.Max(0f, (maximum.Y - minimum.Y - lineHeight) * 0.5f)
                            ),
                            candidate,
                            name,
                            iconSlotWidth
                        );
                        if (selected)
                        {
                            Gui.SetItemDefaultFocus();
                        }
                    }
                    finally
                    {
                        Gui.PopID();
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
        for (int i = 0; i < ICONS.Count; i++)
        {
            if (string.Equals(ICONS[i].glyph, glyph, StringComparison.Ordinal))
            {
                return ICONS[i].name;
            }
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
        EditorUILayout style = Gui.GetLayout();
        float fontSize = Gui.GetFontSize();
        float lineHeight = Gui.GetTextLineHeight();
        uint color = Gui.GetStyleColor(ImGuiCol.Text);
        EditorUI.current.widgets.AddGlyphCentered(
            fontSize,
            glyph,
            new Vector2(origin.X + iconSlotWidth * 0.5f, origin.Y + lineHeight * 0.5f),
            color
        );
        Gui.DrawText(new Vector2(origin.X + iconSlotWidth + style.itemInnerSpacing.X, origin.Y), color, text);
    }
}
