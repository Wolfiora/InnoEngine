using System;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Submits fixed-height content through the native clipper while retaining explicitly active rows.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Draws visible row ranges and preserves native scrolling, navigation and table cursor bookkeeping.
    /// </summary>
    /// <typeparam name="TState">
    /// Call-scoped drawing data, passed directly so static callbacks need no captured closure.
    /// </typeparam>
    /// <param name="itemCount">
    /// The nonnegative count in the caller's current filtered and sorted view.
    /// </param>
    /// <param name="itemHeight">
    /// The complete fixed row advance in screen pixels, or -1 to measure the first row through the native clipper.
    /// </param>
    /// <param name="state">
    /// Data borrowed only for this call; it is never stored by the widget.
    /// </param>
    /// <param name="drawRange">
    /// A synchronous drawing callback receiving inclusive start and exclusive end indices.
    /// </param>
    /// <param name="preservedRows">
    /// Optional rows that must still submit an active edit, drag source or requested focus.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The drawing callback is null.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A count, row height or preserved index is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// No native frame is active in this UI session.
    /// </exception>
    public void ClippedRows<TState>(
        int itemCount,
        float itemHeight,
        TState state,
        Action<TState, int, int> drawRange,
        ReadOnlySpan<int> preservedRows = default
    ) {
        m_session.EnsureDrawing();
        ArgumentNullException.ThrowIfNull(drawRange);
        if (itemCount < 0)
            throw new ArgumentOutOfRangeException(nameof(itemCount));
        if (!float.IsFinite(itemHeight) || (itemHeight <= 0f && itemHeight != -1f))
            throw new ArgumentOutOfRangeException(nameof(itemHeight));
        foreach (int row in preservedRows)
            if (row < 0 || row >= itemCount)
                throw new ArgumentOutOfRangeException(nameof(preservedRows));
        if (NativeImGui.Raw.GetCurrentContext()->WithinFrameScope == 0)
            throw new InvalidOperationException("Row clipping requires an active native frame.");
        if (itemCount == 0)
            return;
        ImGuiListClipper* clipper = NativeImGui.Raw.ImGuiListClipper();
        if (clipper == null)
            throw new OutOfMemoryException("The native row clipper could not be allocated.");
        try
        {
            NativeImGui.Raw.Begin(clipper, itemCount, itemHeight);
            foreach (int row in preservedRows)
                NativeImGui.Raw.IncludeItemByIndex(clipper, row);
            while (NativeImGui.Raw.Step(clipper) != 0)
                drawRange(state, clipper->DisplayStart, clipper->DisplayEnd);
        }
        finally
        {
            // The official destructor ends an unfinished clipper, including callback failures.
            NativeImGui.Raw.Destroy(clipper);
        }
    }
}
