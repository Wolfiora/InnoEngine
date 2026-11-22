using System;
using System.Collections.Generic;
using System.Linq;

namespace Inno.Editor.Core;

/// <summary>
/// Exchanges frame-scoped, reload-safe statistics between independent editor features and viewers.
/// </summary>
/// <remarks>
/// Contributions contain only stable identifiers and strings. A contribution is retained for the
/// current frame and one completed-frame handoff so panel draw order cannot lose data.
/// </remarks>
public sealed class EditorStatistics
{
    private readonly object m_sync = new();
    private Dictionary<EditorStatisticID, EditorStatistic> m_current = [];
    private Dictionary<EditorStatisticID, EditorStatistic> m_completed = [];
    private IReadOnlyList<EditorStatistic>? m_snapshot;
    private bool m_snapshotDirty = true;

    /// <summary>
    /// Publishes or replaces one statistic for the current editor frame.
    /// </summary>
    /// <param name="statistic">
    /// Immutable contribution containing no runtime provider references.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="statistic"/> is <see langword="null"/>.
    /// </exception>
    public void Publish(EditorStatistic statistic)
    {
        ArgumentNullException.ThrowIfNull(statistic);
        lock (m_sync)
        {
            EditorStatistic? previous = m_current.GetValueOrDefault(statistic.id) ?? m_completed.GetValueOrDefault(statistic.id);
            if (previous is null || !SameValue(previous, statistic))
            {
                m_snapshotDirty = true;
            }
            m_current[statistic.id] = statistic;
        }
    }

    /// <summary>
    /// Publishes or replaces several statistics for the current editor frame.
    /// </summary>
    /// <param name="statistics">
    /// Contributions to publish in enumeration order.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="statistics"/> or one of its values is null.
    /// </exception>
    public void Publish(IEnumerable<EditorStatistic> statistics)
    {
        ArgumentNullException.ThrowIfNull(statistics);
        foreach (EditorStatistic statistic in statistics)
        {
            Publish(statistic);
        }
    }

    /// <summary>
    /// Gets a deterministic snapshot combining current contributions with the previous completed
    /// frame for providers that draw after their consumer.
    /// </summary>
    /// <returns>
    /// An immutable, sorted snapshot containing at most one value for each stable statistic ID.
    /// Unchanged content reuses the same snapshot; previously returned snapshots remain valid.
    /// </returns>
    public IReadOnlyList<EditorStatistic> GetSnapshot()
    {
        lock (m_sync)
        {
            if (!m_snapshotDirty && m_snapshot is not null)
            {
                return m_snapshot;
            }
            var merged = new Dictionary<EditorStatisticID, EditorStatistic>(m_completed);
            foreach ((EditorStatisticID id, EditorStatistic statistic) in m_current)
            {
                merged[id] = statistic;
            }
            EditorStatistic[] values = merged.Values
                .OrderBy(static value => value.groupOrder)
                .ThenBy(static value => value.groupName, StringComparer.OrdinalIgnoreCase)
                .ThenBy(static value => value.order)
                .ThenBy(static value => value.label, StringComparer.OrdinalIgnoreCase)
                .ThenBy(static value => value.id.value, StringComparer.Ordinal)
                .ToArray();
            m_snapshot = Array.AsReadOnly(values);
            m_snapshotDirty = false;
            return m_snapshot;
        }
    }

    internal void AdvanceFrame()
    {
        lock (m_sync)
        {
            foreach (EditorStatisticID id in m_completed.Keys)
            {
                if (m_current.ContainsKey(id))
                {
                    continue;
                }
                m_snapshotDirty = true;
                break;
            }
            (m_completed, m_current) = (m_current, m_completed);
            m_current.Clear();
        }
    }

    private static bool SameValue(
        EditorStatistic first,
        EditorStatistic second
    ) => ReferenceEquals(first, second)
         || first.groupID == second.groupID
            && first.groupOrder == second.groupOrder
            && first.order == second.order
            && string.Equals(first.groupName, second.groupName, StringComparison.Ordinal)
            && string.Equals(first.label, second.label, StringComparison.Ordinal)
            && string.Equals(first.value, second.value, StringComparison.Ordinal);
}
