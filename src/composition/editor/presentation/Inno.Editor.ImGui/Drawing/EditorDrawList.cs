using System;
using System.Numerics;
using BGCS.Runtime;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// A pointer-free drawing borrow valid only in its original window, UI session and frame.
/// </summary>
public readonly unsafe struct EditorDrawList
{
    private readonly EditorUiSession m_session;
    private readonly int m_frame;
    private readonly uint m_window;
    internal EditorDrawList(EditorUiSession session)
    {
        session.EnsureDrawing();
        if (NativeImGui.Raw.GetCurrentContext()->WithinFrameScope == 0)
            throw new InvalidOperationException("Drawing requires an active native frame.");
        m_session = session;
        m_frame = NativeImGui.GetFrameCount();
        m_window = NativeImGui.Raw.GetCurrentWindow()->ID;
    }

    /// <summary>
    /// Adds a straight screen-space stroke.
    /// </summary>
    /// <param name="start">
    /// The first endpoint.
    /// </param>
    /// <param name="end">
    /// The final endpoint.
    /// </param>
    /// <param name="color">
    /// Packed RGBA color.
    /// </param>
    /// <param name="thickness">
    /// Stroke width in UI pixels.
    /// </param>
    public void Line(
        Vector2 start,
        Vector2 end,
        uint color,
        float thickness = 1f
    ) => ImGui.DrawNativeLine(Resolve(), start, end, color, thickness);
    /// <summary>
    /// Adds a filled rectangular surface.
    /// </summary>
    /// <param name="minimum">
    /// The upper-left corner.
    /// </param>
    /// <param name="maximum">
    /// The lower-right corner.
    /// </param>
    /// <param name="color">
    /// Packed RGBA color.
    /// </param>
    /// <param name="rounding">
    /// Corner radius.
    /// </param>
    /// <param name="topCornersOnly">
    /// Whether only the two top corners should be rounded.
    /// </param>
    public void FilledRectangle(
        Vector2 minimum,
        Vector2 maximum,
        uint color,
        float rounding = 0f,
        bool topCornersOnly = false
    ) => ImGui.DrawNativeFilledRectangle(Resolve(), minimum, maximum, color, rounding, topCornersOnly ? (int)ImDrawFlags.RoundCornersTop : 0);
    /// <summary>
    /// Adds a rectangular outline.
    /// </summary>
    /// <param name="minimum">
    /// The upper-left corner.
    /// </param>
    /// <param name="maximum">
    /// The lower-right corner.
    /// </param>
    /// <param name="color">
    /// Packed RGBA color.
    /// </param>
    /// <param name="rounding">
    /// Corner radius.
    /// </param>
    /// <param name="thickness">
    /// Stroke width in UI pixels.
    /// </param>
    public void Rectangle(
        Vector2 minimum,
        Vector2 maximum,
        uint color,
        float rounding = 0f,
        float thickness = 1f
    ) => ImGui.DrawNativeRectangle(Resolve(), minimum, maximum, color, rounding, 0, thickness);
    /// <summary>
    /// Adds a filled circle with backend-selected tessellation.
    /// </summary>
    /// <param name="center">
    /// The screen-space center.
    /// </param>
    /// <param name="radius">
    /// The nonnegative radius.
    /// </param>
    /// <param name="color">
    /// Packed RGBA color.
    /// </param>
    public void FilledCircle(
        Vector2 center,
        float radius,
        uint color
    ) {
        ImVec2C native = ImGui.ToNative(center);
        NativeImGui.Raw.AddCircleFilled(Resolve(), &native, radius, color, 0);
    }

    /// <summary>
    /// Adds literal text using the current font and an explicit scale.
    /// </summary>
    /// <param name="position">
    /// Screen-space text origin.
    /// </param>
    /// <param name="color">
    /// Packed RGBA color.
    /// </param>
    /// <param name="text">
    /// Literal managed text; encoded only for this synchronous call.
    /// </param>
    /// <param name="scale">
    /// Positive scale relative to the current font size.
    /// </param>
    public void Text(
        Vector2 position,
        uint color,
        string text,
        float scale = 1f
    ) {
        ImDrawList* draw = Resolve();
        ArgumentNullException.ThrowIfNull(text);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(scale);
        ImVec2C native = ImGui.ToNative(position);
        using NativeStringLease encoded = Utf8Codec.EncodeNullTerminated(text, allowsNull: false);
        NativeImGui.Raw.AddText(draw, NativeImGui.Raw.GetFont(), NativeImGui.GetFontSize() * scale, &native, color, (sbyte*)encoded.pointer, null, 0f, null);
    }

    /// <summary>
    /// Restricts subsequent drawing to one screen rectangle.
    /// </summary>
    /// <param name="minimum">
    /// The clip minimum.
    /// </param>
    /// <param name="maximum">
    /// The clip maximum.
    /// </param>
    /// <param name="intersect">
    /// Whether to intersect the enclosing clip.
    /// </param>
    public void PushClip(
        Vector2 minimum,
        Vector2 maximum,
        bool intersect = true
    ) {
        ImVec2C first = ImGui.ToNative(minimum), last = ImGui.ToNative(maximum);
        NativeImGui.Raw.PushClipRect(Resolve(), &first, &last, intersect ? (byte)1 : (byte)0);
    }

    /// <summary>
    /// Restores the enclosing clip after a matching PushClip.
    /// </summary>
    public void PopClip() => NativeImGui.Raw.PopClipRect(Resolve());
    /// <summary>
    /// Begins ordered draw channels within this window.
    /// </summary>
    /// <param name="count">
    /// The positive number of layers.
    /// </param>
    public void SplitChannels(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(count);
        NativeImGui.Raw.ChannelsSplit(Resolve(), count);
    }

    /// <summary>
    /// Selects a channel from the current split.
    /// </summary>
    /// <param name="index">
    /// The zero-based channel.
    /// </param>
    public void SetChannel(int index) => NativeImGui.Raw.ChannelsSetCurrent(Resolve(), index);
    /// <summary>
    /// Merges the current split back into ordered draw commands.
    /// </summary>
    public void MergeChannels() => NativeImGui.Raw.ChannelsMerge(Resolve());
    private ImDrawList* Resolve()
    {
        if (m_session is null)
            throw new InvalidOperationException("A default drawing borrow has no owner.");
        m_session.EnsureDrawing();
        if (NativeImGui.Raw.GetCurrentContext()->WithinFrameScope == 0 || NativeImGui.GetFrameCount() != m_frame || NativeImGui.Raw.GetCurrentWindow()->ID != m_window)
            throw new InvalidOperationException("The drawing borrow belongs to a different window or frame.");
        return NativeImGui.Raw.GetWindowDrawList();
    }
}
