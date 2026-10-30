using System;
using System.Numerics;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;
/// <summary>
/// Provides reusable editor controls and rendering helpers built on the native ImGui API.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Gets the layout size of a compact colored label chip using the centralized editor style.
    /// </summary>
    /// <param name="label">
    /// Visible label text.
    /// </param>
    /// <returns>
    /// The size reserved by <see cref="LabelChip"/> for the supplied text.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="label"/> is <see langword="null"/>.
    /// </exception>
    public Vector2 GetLabelChipSize(string label)
    {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(label);
        Vector2 textSize = NativeImGui.CalcTextSize(label);
        return new Vector2(textSize.X + style.labelChipPadding.X * 2f, MathF.Max(NativeImGui.GetFrameHeight(), textSize.Y + style.labelChipPadding.Y * 2f));
    }

    /// <summary>
    /// Draws non-interactive text centered on a compact, softly rounded colored background.
    /// </summary>
    /// <param name="label">
    /// Visible label text.
    /// </param>
    /// <param name="background">
    /// Background color drawn behind the label.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="label"/> is <see langword="null"/>.
    /// </exception>
    public void LabelChip(
        string label,
        Vector4 background
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(label);
        Vector2 size = GetLabelChipSize(label);
        Vector2 minimum = NativeImGui.GetCursorScreenPos();
        Vector2 textSize = NativeImGui.CalcTextSize(label);
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        ImGui.DrawNativeFilledRectangle(drawList, minimum, minimum + size, NativeImGui.ColorConvertFloat4ToU32(background), style.labelChipRounding, 0);
        ImGui.DrawNativeText(drawList, minimum + (size - textSize) * 0.5f, NativeImGui.GetColorU32((int)(ImGuiCol.Text)), label);
        NativeImGui.Dummy(size);
    }

    /// <summary>
    /// Gets the layout size of a compact outlined type badge.
    /// </summary>
    /// <param name="label">
    /// Visible type name.
    /// </param>
    /// <returns>
    /// The size reserved by <see cref="TypeBadge"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="label"/> is <see langword="null"/>.
    /// </exception>
    public Vector2 GetTypeBadgeSize(string label)
    {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(label);
        Vector2 textSize = NativeImGui.CalcTextSize(label);
        return textSize + style.typeBadgePadding * 2f;
    }

    /// <summary>
    /// Draws a compact, non-interactive type badge with a subdued fill and semantic outline.
    /// </summary>
    /// <param name="label">
    /// Visible type name.
    /// </param>
    /// <param name="accent">
    /// Semantic type-family accent.
    /// </param>
    /// <param name="tooltip">
    /// Optional explanation shown while the badge is hovered.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="label"/> is <see langword="null"/>.
    /// </exception>
    public void TypeBadge(
        string label,
        Vector4 accent,
        string? tooltip = null
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(label);
        Vector2 size = GetTypeBadgeSize(label);
        Vector2 minimum = NativeImGui.GetCursorScreenPos();
        Vector2 textSize = NativeImGui.CalcTextSize(label);
        ImDrawList* draw = NativeImGui.Raw.GetWindowDrawList();
        Vector4 background = Vector4.Lerp(EditorPalette.windowBackground, accent, 0.14f);
        background.W = EditorPalette.opacityOpaque;
        Vector4 outline = accent;
        outline.W = EditorPalette.opacityStrong;
        ImGui.DrawNativeFilledRectangle(draw, minimum, minimum + size, NativeImGui.ColorConvertFloat4ToU32(background), style.typeBadgeRounding, 0);
        ImGui.DrawNativeRectangle(draw, minimum, minimum + size, NativeImGui.ColorConvertFloat4ToU32(outline), style.typeBadgeRounding, (int)(ImDrawFlags.None), style.borderSize);
        ImGui.DrawNativeText(draw, minimum + (size - textSize) * 0.5f, NativeImGui.ColorConvertFloat4ToU32(accent), label);
        NativeImGui.Dummy(size);
        DrawItemTooltip(tooltip);
    }

    /// <summary>
    /// Draws a compact checkbox whose checked fill uses the current text color.
    /// </summary>
    /// <param name="id">
    /// Stable control identifier.
    /// </param>
    /// <param name="value">
    /// Mutable checked state.
    /// </param>
    /// <param name="size">
    /// Visual square size in pixels.
    /// </param>
    /// <param name="tooltip">
    /// Optional explanation displayed while the checkbox is hovered.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changed.
    /// </returns>
    public bool CompactCheckbox(
        string id,
        ref bool value,
        float size = -1f,
        string? tooltip = null
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        if (size < 0f)
            size = style.compactCheckboxSize;
        if (size <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Checkbox size must be positive.");
        }

        Vector2 initialCursor = NativeImGui.GetCursorScreenPos();
        float frameHeight = NativeImGui.GetFrameHeight();
        float verticalOffset = MathF.Max(0f, (frameHeight - size) * 0.5f);
        Vector2 min = initialCursor + new Vector2(0f, verticalOffset);
        bool changed = NativeImGui.InvisibleButton($"##compact_checkbox_{id}", new Vector2(size, frameHeight));
        if (changed)
        {
            value = !value;
        }

        bool hovered = NativeImGui.IsItemHovered();
        Vector2 max = min + new Vector2(size);
        uint textColor = NativeImGui.GetColorU32((int)(ImGuiCol.Text));
        ImDrawList* drawList = NativeImGui.Raw.GetWindowDrawList();
        if (value)
        {
            ImGui.DrawNativeFilledRectangle(drawList, min, max, textColor, 1f, 0);
            uint checkColor = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.cardHeader);
            ImGui.DrawNativeLine(drawList, min + new Vector2(size * 0.22f, size * 0.52f), min + new Vector2(size * 0.43f, size * 0.72f), checkColor, 1.5f);
            ImGui.DrawNativeLine(drawList, min + new Vector2(size * 0.43f, size * 0.72f), min + new Vector2(size * 0.80f, size * 0.28f), checkColor, 1.5f);
        }
        else
        {
            ImGui.DrawNativeRectangle(drawList, min, max, textColor, 1f, 0, 1f);
        }

        if (hovered)
        {
            ImGui.DrawNativeRectangle(drawList, min - Vector2.One, max + Vector2.One, NativeImGui.ColorConvertFloat4ToU32(EditorPalette.compactControlHovered), 1f, 0, 1.5f);
        }

        DrawItemTooltip(tooltip);
        return changed;
    }

    /// <summary>
    /// Draws a standard checkbox with the shared editor tooltip behavior.
    /// </summary>
    /// <param name="label">
    /// Visible label and stable ImGui identifier.
    /// </param>
    /// <param name="value">
    /// Mutable checked state.
    /// </param>
    /// <param name="tooltip">
    /// Optional explanation displayed while the checkbox is hovered.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the value changed.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="label"/> is empty or whitespace.
    /// </exception>
    public bool Checkbox(
        string label,
        ref bool value,
        string? tooltip = null
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        bool changed = NativeImGui.Checkbox(label, ref value);
        DrawItemTooltip(tooltip);
        return changed;
    }

    /// <summary>
    /// Draws a horizontally centered button with optional space above it.
    /// </summary>
    /// <param name="label">
    /// Visible label and optional ImGui identifier suffix.
    /// </param>
    /// <param name="topPadding">
    /// Additional vertical space above the button.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the button is pressed.
    /// </returns>
    public bool CenteredButton(
        string label,
        float topPadding = 0f
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        if (topPadding < 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(topPadding), topPadding, "Top padding cannot be negative.");
        }

        float width = ImGuiUtf8Buffer.MeasureText(label, -1f).X + NativeImGui.Raw.GetStyle()->FramePadding.X * 2f;
        return DrawCenteredButton(label, NativeImGui.GetCursorScreenPos() + new Vector2(0f, topPadding), NativeImGui.GetContentRegionAvail().X, width);
    }

    /// <summary>
    /// Draws a progress bar whose overlay remains centered over the complete bar.
    /// </summary>
    /// <param name="fraction">
    /// The completed fraction rendered by the progress bar.
    /// </param>
    /// <param name="size">
    /// The requested ImGui progress-bar size.
    /// </param>
    /// <param name="overlay">
    /// The text drawn at the geometric center of the complete progress bar.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="overlay"/> is <see langword="null"/>.
    /// </exception>
    public void CenteredProgressBar(
        float fraction,
        Vector2 size,
        string overlay
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(overlay);
        ImVec2C nativeSize = ImGui.ToNative(size);
        ReadOnlySpan<byte> emptyOverlay = "\0"u8;
        fixed (byte* empty = emptyOverlay)
            NativeImGui.Raw.ProgressBar(fraction, &nativeSize, (sbyte*)empty);
        Vector2 minimum = NativeImGui.GetItemRectMin();
        Vector2 maximum = NativeImGui.GetItemRectMax();
        Vector2 textSize = NativeImGui.CalcTextSize(overlay);
        Vector2 textPosition = minimum + (maximum - minimum - textSize) * 0.5f;
        ImGui.DrawNativeText(NativeImGui.Raw.GetWindowDrawList(), textPosition, NativeImGui.GetColorU32((int)(ImGuiCol.Text)), overlay);
    }

    private static bool DrawCenteredButton(
        string label,
        Vector2 origin,
        float availableWidth,
        float width
    ) {
        NativeImGui.SetCursorScreenPos(origin + new Vector2(MathF.Max(0f, (availableWidth - width) * 0.5f), 0f));
        return NativeImGui.Button(label);
    }
}
