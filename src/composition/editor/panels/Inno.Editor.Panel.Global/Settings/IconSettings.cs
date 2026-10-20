using System.Runtime.CompilerServices;
using NumericsVector2 = System.Numerics.Vector2;
using NativeVector2 = Inno.Native.ImGui.ImVec2C;
using BGCS.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;

using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Settings;
using Inno.Native.ImGui;
using Inno.Adapter.Presentation.ImGui;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Panel.Global;

internal abstract unsafe class IconSetting : EditorSetting
{
    private static readonly IReadOnlyList<(string Name, string Glyph)> C_ICONS =
        typeof(ImGuiIcon)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(static field => field.IsLiteral && field.FieldType == typeof(string))
            .Select(static field => (field.Name, (string)field.GetRawConstantValue()!))
            .OrderBy(static value => value.Name, StringComparer.Ordinal)
            .ToArray();

    private static int s_iconSlotFrame = -1;
    private static float s_iconSlotWidth;

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
        ImDrawList* comboDrawList = NativeImGui.Raw.GetWindowDrawList();
        NativeImGui.SetNextItemWidth(-1f);
        bool isOpen = EditorWidget.BeginBoundedCombo("##icon", string.Empty);
        Vector2 comboMinimum = NativeImGui.GetItemRectMin();
        Vector2 comboMaximum = NativeImGui.GetItemRectMax();
        ImGuiStyle* comboStyle = NativeImGui.Raw.GetStyle();
        float comboLineHeight = NativeImGui.GetTextLineHeight();
        DrawIconTextAt(
            comboDrawList,
            new Vector2(
                comboMinimum.X + comboStyle->FramePadding.X,
                comboMinimum.Y + MathF.Max(
                    0f,
                    (comboMaximum.Y - comboMinimum.Y - comboLineHeight) * 0.5f)),
            value,
            GetIconName(value),
            iconSlotWidth);
        if (isOpen)
        {
            try
            {
                for (int i = 0; i < C_ICONS.Count; i++)
                {
                    (string name, string candidate) = C_ICONS[i];
                    NativeImGui.PushID(name);
                    try
                    {
                        bool selected = string.Equals(candidate, value, StringComparison.Ordinal);
                        if (NativeImGui.Selectable(
                                "##icon_option",
                                selected,
                                (int)ImGuiSelectableFlags.None,
                                new Vector2(0f, NativeImGui.GetFrameHeight())))
                        {
                            value = candidate;
                            setting.SetAsString("value", candidate);
                        }
                        Vector2 minimum = NativeImGui.GetItemRectMin();
                        Vector2 maximum = NativeImGui.GetItemRectMax();
                        ImGuiStyle* style = NativeImGui.Raw.GetStyle();
                        float lineHeight = NativeImGui.GetTextLineHeight();
                        DrawIconTextAt(
                            NativeImGui.Raw.GetWindowDrawList(),
                            new Vector2(
                                minimum.X + style->FramePadding.X,
                                minimum.Y + MathF.Max(0f, (maximum.Y - minimum.Y - lineHeight) * 0.5f)),
                            candidate,
                            name,
                            iconSlotWidth);
                        if (selected)
                            NativeImGui.SetItemDefaultFocus();
                    }
                    finally
                    {
                        NativeImGui.PopID();
                    }
                }
            }
            finally
            {
                EditorWidget.EndBoundedCombo();
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

    private static float GetIconSlotWidth()
    {
        int frame = NativeImGui.GetFrameCount();
        if (s_iconSlotFrame == frame)
            return s_iconSlotWidth;

        ImFont* font = NativeImGui.Raw.GetFont();
        float fontSize = NativeImGui.GetFontSize();
        float result = NativeImGui.GetTextLineHeight();
        for (int i = 0; i < C_ICONS.Count; i++)
        {
            Vector4 bounds = ImGuiWidget.GetGlyphVisualBounds(font, fontSize, C_ICONS[i].Glyph);
            result = MathF.Max(result, bounds.Z - bounds.X);
        }
        s_iconSlotFrame = frame;
        s_iconSlotWidth = result;
        return s_iconSlotWidth;
    }

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
        ImDrawList* drawList,
        Vector2 origin,
        string glyph,
        string text,
        float iconSlotWidth
    ) {
        ImGuiStyle* style = NativeImGui.Raw.GetStyle();
        ImFont* font = NativeImGui.Raw.GetFont();
        float fontSize = NativeImGui.GetFontSize();
        float lineHeight = NativeImGui.GetTextLineHeight();
        uint color = NativeImGui.GetColorU32((int)ImGuiCol.Text);
        ImGuiWidget.AddGlyphCentered(
            drawList,
            font,
            fontSize,
            glyph,
            new Vector2(origin.X + iconSlotWidth * 0.5f, origin.Y + lineHeight * 0.5f),
            color);
        using (NativeStringLease nativeText = Utf8Codec.EncodeNullTerminated(text, allowsNull: false))
        {
            NativeImGui.Raw.AddText(
                drawList,
                Unsafe.BitCast<NumericsVector2, NativeVector2>(new Vector2(origin.X + iconSlotWidth + style->ItemInnerSpacing.X, origin.Y)),
                color,
                (sbyte*)nativeText.pointer,
                null
            );
        }
    }
}

[EditorSettingPath("Editor/Appearance/Icons/Scene")]
internal sealed unsafe class SceneIconSetting() : IconSetting(ImGuiIcon.Cubes)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a scene document or loaded scene.";
}

[EditorSettingPath("Editor/Appearance/Icons/GameObject")]
internal sealed unsafe class GameObjectIconSetting() : IconSetting(ImGuiIcon.Cube)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a scene GameObject.";
}

[EditorSettingPath("Editor/Appearance/Icons/Prefab")]
internal sealed unsafe class PrefabIconSetting() : IconSetting(ImGuiIcon.DiceD6)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a reusable prefab object.";
}

[EditorSettingPath("Editor/Appearance/Icons/Layers")]
internal sealed unsafe class LayersIconSetting() : IconSetting(ImGuiIcon.LayerGroup)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a project layer configuration.";
}

[EditorSettingPath("Editor/Appearance/Icons/Folder")]
internal sealed unsafe class FolderIconSetting() : IconSetting(ImGuiIcon.Folder)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a source directory.";
}

[EditorSettingPath("Editor/Appearance/Icons/File")]
internal sealed unsafe class FileIconSetting() : IconSetting(ImGuiIcon.File)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
    public override string description => "Selects the icon used wherever the editor presents a generic source file.";
}

[EditorSettingPath("Editor/Appearance/Icons/Shader")]
internal sealed unsafe class ShaderIconSetting() : IconSetting(ImGuiIcon.Shader)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
public override string description => "Selects the icon used consistently for Shader graph assets and their Inspector headers.";
}

[EditorSettingPath("Editor/Appearance/Icons/ShaderSource")]
internal sealed unsafe class ShaderSourceIconSetting() : IconSetting(ImGuiIcon.RupeeSign)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
public override string description
        => "Selects the icon used consistently for Shader source-function libraries. The default function symbol distinguishes reusable code from complete Shader graphs.";
}

[EditorSettingPath("Editor/Appearance/Icons/Material")]
internal sealed unsafe class MaterialIconSetting() : IconSetting(ImGuiIcon.Palette)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
public override string description => "Selects the icon used consistently for Material assets and their Inspector headers.";
}

[EditorSettingPath("Editor/Appearance/Icons/RenderPipeline")]
internal sealed unsafe class RenderPipelineIconSetting() : IconSetting(ImGuiIcon.RupiahSign)
{
    /// <summary>
    /// Gets the user-facing explanation of this feature or setting.
    /// </summary>
public override string description => "Selects the icon used consistently for Render Pipeline assets and their Inspector headers.";
}
