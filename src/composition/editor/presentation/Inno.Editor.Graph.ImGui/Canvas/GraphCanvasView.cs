using System;
using System.Numerics;
using Inno.Core.Graphs;
using Inno.Editor.Graph;
using Inno.Editor.ImGui;
using CanvasDrawing = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Graph.ImGui;
/// <summary>
/// Projects and presents an arbitrary graph viewport without owning document mutations or domain semantics.
/// </summary>
public readonly partial struct GraphCanvasView
{
    private const int C_MAX_GRID_LINES_PER_AXIS = 2048;
    private readonly GraphPosition m_pan;
    private readonly float m_zoom;
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
    /// <exception cref="ArgumentNullException">
    /// The document-owned navigation state is null.
    /// </exception>
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
        m_pan = state.pan;
        m_zoom = state.zoom;
        m_origin = origin;
        m_size = size;
    }

    /// <summary>
    /// Gets the graph-to-screen zoom captured for this frame, independently of later navigation changes.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// The view was not initialized through its constructor.
    /// </exception>
    public float zoom => m_zoom > 0f ? m_zoom : throw new InvalidOperationException("A graph canvas view requires an initialized projection.");

    /// <summary>
    /// Projects a graph-space location into the current screen rectangle.
    /// </summary>
    /// <param name="point">
    /// The domain-independent graph coordinate.
    /// </param>
    /// <returns>
    /// The corresponding screen coordinate.
    /// </returns>
    public Vector2 ToScreen(GraphPosition point) => m_origin + new Vector2(m_pan.x, m_pan.y) + new Vector2(point.x, point.y) * zoom;
    /// <summary>
    /// Converts a screen location back into graph coordinates.
    /// </summary>
    /// <param name="point">
    /// The screen coordinate to project.
    /// </param>
    /// <returns>
    /// The graph coordinate under the pointer.
    /// </returns>
    public GraphPosition ToGraph(Vector2 point) => new((point.X - m_origin.X - m_pan.x) / zoom, (point.Y - m_origin.Y - m_pan.y) / zoom);
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
        float step = MathF.Max(MathF.Max(4f, spacing * zoom), MathF.Max(m_size.X, m_size.Y) / C_MAX_GRID_LINES_PER_AXIS);
        for (float x = ((m_pan.x % step) + step) % step; x < m_size.X; x += step)
            CanvasDrawing.DrawLine(m_origin + new Vector2(x, 0f), m_origin + new Vector2(x, m_size.Y), color);
        for (float y = ((m_pan.y % step) + step) % step; y < m_size.Y; y += step)
            CanvasDrawing.DrawLine(m_origin + new Vector2(0f, y), m_origin + new Vector2(m_size.X, y), color);
    }

    /// <summary>
    /// Draws a horizontal-tangent connection using the same curve used for hit testing.
    /// </summary>
    /// <param name="draw">
    /// The current window's frame-local drawing borrow; it is neither retained nor released by this call.
    /// </param>
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
    /// <exception cref="ArgumentOutOfRangeException">
    /// An endpoint or derived control point is nonfinite, or the stroke width is not positive and finite.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The canvas is uninitialized or a visible connection uses an expired or mismatched drawing borrow.
    /// </exception>
    public void DrawConnection(
        EditorDrawList draw,
        Vector2 output,
        Vector2 input,
        uint color,
        float thickness = 2f
    ) {
        _ = zoom;
        if (!float.IsFinite(thickness) || thickness <= 0f)
            throw new ArgumentOutOfRangeException(nameof(thickness));
        if (!float.IsFinite(output.X) || !float.IsFinite(output.Y) || !float.IsFinite(input.X) || !float.IsFinite(input.Y))
            throw new ArgumentOutOfRangeException(nameof(output), "Connection endpoints must be finite screen coordinates.");
        if (m_size.X == 0f || m_size.Y == 0f)
            return;
        float tangent = Tangent(output, input);
        Vector2 first = output + new Vector2(tangent, 0f);
        Vector2 second = input - new Vector2(tangent, 0f);
        if (!float.IsFinite(first.X) || !float.IsFinite(second.X))
            throw new ArgumentOutOfRangeException(nameof(input), "Connection controls exceed finite screen coordinates.");
        var clip = CanvasDrawing.GetClipBounds();
        Vector2 minimum = Vector2.Min(Vector2.Min(output, input), Vector2.Min(first, second)) - new Vector2(thickness);
        Vector2 maximum = Vector2.Max(Vector2.Max(output, input), Vector2.Max(first, second)) + new Vector2(thickness);
        if (maximum.X < MathF.Max(m_origin.X, clip.minimum.X) || minimum.X > MathF.Min(m_origin.X + m_size.X, clip.maximum.X)
            || maximum.Y < MathF.Max(m_origin.Y, clip.minimum.Y) || minimum.Y > MathF.Min(m_origin.Y + m_size.Y, clip.maximum.Y))
            return;
        draw.PushClip(m_origin, m_origin + m_size, true);
        try
        {
            CanvasDrawing.DrawCubicCurve(output, first, second, input, color, thickness);
        }
        finally
        {
            draw.PopClip();
        }
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
