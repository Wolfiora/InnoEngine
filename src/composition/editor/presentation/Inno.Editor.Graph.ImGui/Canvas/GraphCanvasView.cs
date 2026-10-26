using System;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using CanvasDrawing = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Graph.ImGui;
/// <summary>
/// Projects and presents an arbitrary graph viewport without owning document mutations or domain semantics.
/// </summary>
public readonly struct GraphCanvasView
{
    private readonly GraphCanvasState m_state;
    private readonly Vector2 m_origin;
    private readonly Vector2 m_size;
    /// <summary>
    /// Creates a frame-local graph presenter over navigation owned by its document.
    /// </summary>
    /// <param name="state">
    /// The existing neutral navigation and selection state.
    /// </param>
    /// <param name="origin">
    /// The screen-space canvas origin.
    /// </param>
    /// <param name="size">
    /// The finite nonnegative canvas extent.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The canvas geometry is invalid.
    /// </exception>
    public GraphCanvasView(
        GraphCanvasState state,
        Vector2 origin,
        Vector2 size
    ) {
        ArgumentNullException.ThrowIfNull(state);
        if (!float.IsFinite(origin.X) || !float.IsFinite(origin.Y))
            throw new ArgumentOutOfRangeException(nameof(origin));
        if (!float.IsFinite(size.X) || !float.IsFinite(size.Y) || size.X < 0f || size.Y < 0f)
            throw new ArgumentOutOfRangeException(nameof(size));
        m_state = state;
        m_origin = origin;
        m_size = size;
    }

    /// <summary>
    /// Projects a graph-space location into the current screen rectangle.
    /// </summary>
    /// <param name="point">
    /// The domain-independent graph coordinate.
    /// </param>
    /// <returns>
    /// The corresponding screen coordinate.
    /// </returns>
    public Vector2 ToScreen(GraphPosition point) => m_origin + new Vector2(m_state.pan.x, m_state.pan.y) + new Vector2(point.x, point.y) * m_state.zoom;
    /// <summary>
    /// Converts a screen location back into graph coordinates.
    /// </summary>
    /// <param name="point">
    /// The screen coordinate to project.
    /// </param>
    /// <returns>
    /// The graph coordinate under the pointer.
    /// </returns>
    public GraphPosition ToGraph(Vector2 point) => new((point.X - m_origin.X - m_state.pan.x) / m_state.zoom, (point.Y - m_origin.Y - m_state.pan.y) / m_state.zoom);
    /// <summary>
    /// Draws a bounded graph-space grid through the current UI drawing context.
    /// </summary>
    /// <param name="color">
    /// The packed presentation color.
    /// </param>
    /// <param name="spacing">
    /// Positive graph-space grid spacing.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Spacing is nonpositive or nonfinite.
    /// </exception>
    public void DrawGrid(
        uint color,
        float spacing = 32f
    ) {
        if (!float.IsFinite(spacing) || spacing <= 0f)
            throw new ArgumentOutOfRangeException(nameof(spacing));
        float step = MathF.Max(4f, spacing * m_state.zoom);
        for (float x = ((m_state.pan.x % step) + step) % step; x < m_size.X; x += step)
            CanvasDrawing.DrawLine(m_origin + new Vector2(x, 0f), m_origin + new Vector2(x, m_size.Y), color);
        for (float y = ((m_state.pan.y % step) + step) % step; y < m_size.Y; y += step)
            CanvasDrawing.DrawLine(m_origin + new Vector2(0f, y), m_origin + new Vector2(m_size.X, y), color);
    }

    /// <summary>
    /// Draws a horizontal-tangent connection using the same curve used for hit testing.
    /// </summary>
    /// <param name="output">
    /// The output endpoint in screen space.
    /// </param>
    /// <param name="input">
    /// The input endpoint in screen space.
    /// </param>
    /// <param name="color">
    /// The packed connection color.
    /// </param>
    /// <param name="thickness">
    /// The positive screen-space stroke width.
    /// </param>
    public void DrawConnection(
        Vector2 output,
        Vector2 input,
        uint color,
        float thickness = 2f
    ) {
        float tangent = Tangent(output, input);
        CanvasDrawing.DrawCubicCurve(output, output + new Vector2(tangent, 0f), input - new Vector2(tangent, 0f), input, color, thickness);
    }

    /// <summary>
    /// Checks pointer distance against the displayed cubic connection without any UI or document access.
    /// </summary>
    /// <param name="point">
    /// The screen-space pointer location.
    /// </param>
    /// <param name="output">
    /// The displayed output endpoint.
    /// </param>
    /// <param name="input">
    /// The displayed input endpoint.
    /// </param>
    /// <param name="radius">
    /// Nonnegative screen-space acceptance radius.
    /// </param>
    /// <returns>
    /// True when the pointer is within the curve's hit area, including its endpoints.
    /// </returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Radius is negative or nonfinite.
    /// </exception>
    public static bool HitConnection(
        Vector2 point,
        Vector2 output,
        Vector2 input,
        float radius = 6f
    ) {
        if (!float.IsFinite(radius) || radius < 0f)
            throw new ArgumentOutOfRangeException(nameof(radius));
        float tangent = Tangent(output, input);
        Vector2 first = output + new Vector2(tangent, 0f);
        Vector2 second = input - new Vector2(tangent, 0f);
        Vector2 previous = output;
        for (int segment = 1; segment <= 32; segment++)
        {
            float t = segment / 32f;
            float s = 1f - t;
            Vector2 next = s * s * s * output + 3f * s * s * t * first + 3f * s * t * t * second + t * t * t * input;
            Vector2 line = next - previous;
            float projection = Math.Clamp(Vector2.Dot(point - previous, line) / MathF.Max(0.001f, line.LengthSquared()), 0f, 1f);
            if (Vector2.DistanceSquared(point, previous + projection * line) <= radius * radius)
                return true;
            previous = next;
        }

        return false;
    }

    private static float Tangent(
        Vector2 output,
        Vector2 input
    ) => MathF.Max(36f, MathF.Abs(input.X - output.X) * 0.45f);
}
