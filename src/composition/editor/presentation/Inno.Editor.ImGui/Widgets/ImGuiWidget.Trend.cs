using System;
using System.Numerics;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Provides session-owned numeric trend presentation over borrowed sample buffers.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Draws a bounded line plot from borrowed numeric samples without copying their buffer.
    /// </summary>
    /// <param name="label">
    /// The visible plot title and stable item identifier in its current owner.
    /// </param>
    /// <param name="values">
    /// Finite samples borrowed only for this call; an empty span draws an empty plot.
    /// </param>
    /// <param name="offset">
    /// The first chronological sample in a circular buffer, or zero for empty input.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The label is empty or a sample is not finite.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The offset is outside the supplied sample span.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// No matching owner-thread drawing session is active.
    /// </exception>
    public void Trend(
        string label,
        ReadOnlySpan<float> values,
        int offset = 0
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(label);
        if (offset < 0 || (values.Length == 0 ? offset != 0 : offset >= values.Length))
            throw new ArgumentOutOfRangeException(nameof(offset));
        float maximum = 0f;
        float minimum = 0f;
        foreach (float value in values)
        {
            if (!float.IsFinite(value))
                throw new ArgumentException("Trend samples must be finite.", nameof(values));
            maximum = MathF.Max(maximum, value);
            minimum = MathF.Min(minimum, value);
        }
        NativeImGui.TextUnformatted(label);
        Vector2 origin = NativeImGui.GetCursorScreenPos();
        Vector2 size = new(MathF.Max(1f, NativeImGui.GetContentRegionAvail().X), style.trendHeight);
        _ = NativeImGui.InvisibleButton(label, size);
        EditorDrawList draw = ImGui.GetDrawList();
        draw.FilledRectangle(origin, origin + size, NativeImGui.ColorConvertFloat4ToU32(EditorPalette.collectionRow));
        double range = maximum > minimum ? (double)maximum - minimum : 1d;
        uint color = NativeImGui.GetColorU32((int)ImGuiCol.PlotLines);
        Vector2 previous = default;
        for (int index = 0; index < values.Length; index++)
        {
            float value = values[(offset + index) % values.Length];
            float x = values.Length <= 1 ? 0f : (float)index / (values.Length - 1);
            float normalized = (float)(((double)value - minimum) / range);
            Vector2 point = origin + new Vector2(x * size.X, (1f - normalized) * size.Y);
            if (index > 0)
                draw.Line(previous, point, color);
            previous = point;
        }
    }
}
