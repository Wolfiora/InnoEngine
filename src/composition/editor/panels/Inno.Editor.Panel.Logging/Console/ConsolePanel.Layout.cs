using System;
using System.Collections.Generic;
using System.Numerics;
using Ui = Inno.Editor.ImGui.ImGui;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;

namespace Inno.Editor.Panel.Logging;

internal sealed partial class ConsolePanel
{
    private sealed class EntryMeasurement(
        ConsoleView.Row row,
        bool open,
        float height
    ) {
        internal readonly ConsoleView.Row row = row;
        internal readonly bool open = open;
        internal float height = height;
        internal int remainingMeasurements = open ? 2 : 0;
    }

    private Dictionary<string, EntryMeasurement> m_measurements = new(StringComparer.Ordinal);
    private EntryMeasurement[] m_layout = [];
    private readonly List<int> m_pendingMeasurements = [];
    private IReadOnlyList<ConsoleView.Row>? m_layoutRows;
    private float[] m_offsets = [0f];
    private (float width, float frameHeight, float padding, float spacing, float zoom)? m_layoutShape;
    private int m_openRevision;
    private int m_layoutOpenRevision = -1;
    private bool m_scrollFocusedRow;
    private readonly Dictionary<string, CardWindowSlot> m_cardSlots = new(StringComparer.Ordinal);
    private readonly List<string> m_cardWindowNames = [];
    private readonly Stack<int> m_freeCardWindows = [];
    private readonly List<string> m_retiredCardKeys = [];
    private ulong m_cardFrame;

    private sealed class CardWindowSlot(int index)
    {
        internal readonly int index = index;
        internal ulong lastFrame;
    }

    private string AcquireCardWindow(string key)
    {
        if (!m_cardSlots.TryGetValue(key, out CardWindowSlot? slot))
        {
            if (!m_freeCardWindows.TryPop(out int index))
            {
                index = m_cardWindowNames.Count;
                m_cardWindowNames.Add("##ConsoleEntryCard_" + index);
            }
            slot = new CardWindowSlot(index);
            m_cardSlots.Add(key, slot);
        }
        slot.lastFrame = m_cardFrame;
        return m_cardWindowNames[slot.index];
    }

    private void RetireCardWindows()
    {
        m_retiredCardKeys.Clear();
        foreach (var entry in m_cardSlots)
            if (entry.Value.lastFrame != m_cardFrame)
                m_retiredCardKeys.Add(entry.Key);
        foreach (string key in m_retiredCardKeys)
        {
            m_freeCardWindows.Push(m_cardSlots[key].index);
            m_cardSlots.Remove(key);
        }
        m_retiredCardKeys.Clear();
    }

    private void ToggleEntry(string key)
    {
        if (!m_openEntries.Remove(key))
            m_openEntries.Add(key);
        m_openRevision++;
    }

    private void ClearLayout()
    {
        m_measurements.Clear();
        m_layoutRows = null;
        m_layout = [];
        m_pendingMeasurements.Clear();
        m_offsets = [0f];
        m_layoutShape = null;
        m_scrollFocusedRow = false;
        m_cardSlots.Clear();
        m_freeCardWindows.Clear();
        for (int index = m_cardWindowNames.Count - 1; index >= 0; index--)
            m_freeCardWindows.Push(index);
    }

    private void RefreshLayout()
    {
        var layout = Ui.GetLayout();
        var shape = (width: Ui.GetContentRegionAvailable().X, frameHeight: Ui.GetFrameHeight(), padding: layout.framePadding.Y,
            spacing: layout.itemSpacing.Y, zoom: EditorUI.current.widgets.style.zoom);
        if (ReferenceEquals(m_layoutRows, m_view.rows) && m_layoutShape == shape && m_layoutOpenRevision == m_openRevision)
            return;
        bool sameShape = m_layoutShape == shape;
        float collapsedHeight = MathF.Truncate(shape.frameHeight + shape.padding * 2f);
        var measurements = new Dictionary<string, EntryMeasurement>(m_view.rows.Count, StringComparer.Ordinal);
        var entries = new EntryMeasurement[m_view.rows.Count];
        for (int index = 0; index < entries.Length; index++)
        {
            ConsoleView.Row row = m_view.rows[index];
            bool open = m_openEntries.Contains(row.key);
            EntryMeasurement measurement = sameShape && m_measurements.TryGetValue(row.key, out EntryMeasurement? previous)
                && ReferenceEquals(previous.row, row) && previous.open == open
                ? previous : new(row, open, collapsedHeight);
            entries[index] = measurement;
            measurements.Add(row.key, measurement);
        }
        m_measurements = measurements;
        m_layout = entries;
        m_pendingMeasurements.Clear();
        for (int index = 0; index < entries.Length; index++)
            if (entries[index].remainingMeasurements > 0)
                m_pendingMeasurements.Add(index);
        m_offsets = new float[entries.Length + 1];
        m_layoutRows = m_view.rows;
        m_layoutShape = shape;
        m_layoutOpenRevision = m_openRevision;
        RefreshOffsets(0);
    }

    private void RefreshOffsets(int start)
    {
        float spacing = Ui.GetLayout().itemSpacing.Y;
        for (int index = start; index < m_layout.Length; index++)
            m_offsets[index + 1] = m_offsets[index] + m_layout[index].height + spacing;
    }

    private void DrawVisibleEntries()
    {
        var clip = Ui.GetClipBounds();
        if (clip.maximum.Y <= clip.minimum.Y || clip.maximum.X <= clip.minimum.X)
            return;
        RefreshLayout();
        m_cardFrame++;
        Vector2 origin = Ui.GetCursorScreenPosition();
        int first = FindLayoutRow(clip.minimum.Y - origin.Y);
        int last = FindLayoutRow(clip.maximum.Y - origin.Y);
        int focused = m_view.FindIndex(m_focusedKey);
        int changedFrom = m_layout.Length;
        float adjustment = 0f;
        int visibleIndex = first;
        int pendingIndex = 0;
        bool focusPending = focused >= 0;
        while (visibleIndex <= last || pendingIndex < m_pendingMeasurements.Count || focusPending)
        {
            int index = Math.Min(visibleIndex <= last ? visibleIndex : int.MaxValue,
                pendingIndex < m_pendingMeasurements.Count ? m_pendingMeasurements[pendingIndex] : int.MaxValue);
            if (focusPending)
                index = Math.Min(index, focused);
            if (index == visibleIndex)
                visibleIndex++;
            if (pendingIndex < m_pendingMeasurements.Count && index == m_pendingMeasurements[pendingIndex])
                pendingIndex++;
            if (index == focused)
                focusPending = false;
            EntryMeasurement entry = m_layout[index];
            bool measure = entry.remainingMeasurements > 0;
            Ui.SetCursorScreenPosition(origin + new Vector2(0f, m_offsets[index] + adjustment));
            float height = DrawConsoleEntry(entry.row, measure);
            if (!float.IsFinite(height) || height <= 0f)
                throw new InvalidOperationException("Console row measurement did not produce a finite positive height.");
            if (measure)
                entry.remainingMeasurements--;
            if (height != entry.height)
            {
                adjustment += height - entry.height;
                entry.height = height;
                changedFrom = Math.Min(changedFrom, index);
            }
            if (index == focused && m_scrollFocusedRow)
            {
                float top = origin.Y + m_offsets[index] + adjustment;
                if (top < clip.minimum.Y)
                    Ui.SetScrollHereY(0f);
                else if (top + height > clip.maximum.Y)
                    Ui.SetScrollHereY(1f);
            }
        }
        for (int index = m_pendingMeasurements.Count - 1; index >= 0; index--)
            if (m_layout[m_pendingMeasurements[index]].remainingMeasurements == 0)
                m_pendingMeasurements.RemoveAt(index);
        if (changedFrom < m_layout.Length)
            RefreshOffsets(changedFrom);
        m_scrollFocusedRow = false;
        // A slot is reusable only after all rows have finished submitting this frame.
        RetireCardWindows();
        Ui.SetCursorScreenPosition(origin);
        Ui.Dummy(new(1f, MathF.Max(1f, m_offsets[^1] - Ui.GetLayout().itemSpacing.Y)));
    }

    private int FindLayoutRow(float position)
    {
        int index = Array.BinarySearch(m_offsets, position);
        return Math.Clamp(index < 0 ? ~index - 1 : index, 0, m_layout.Length - 1);
    }
}
