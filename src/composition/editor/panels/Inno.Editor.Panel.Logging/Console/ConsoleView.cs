using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Core.Logging;
using Inno.Editor.Diagnostics;

namespace Inno.Editor.Panel.Logging;

internal sealed class ConsoleView
{
    private long m_revision = -1;
    private string m_query = string.Empty;
    private ulong m_levels;
    private bool m_collapsed;
    private readonly Dictionary<string, int> m_indices = new(StringComparer.Ordinal);

    internal IReadOnlyList<Row> rows { get; private set; } = Array.Empty<Row>();

    internal bool Update(
        EditorConsoleSnapshot snapshot,
        string query,
        IReadOnlySet<LogLevel> levels,
        bool collapsed
    ) {
        ulong mask = 0;
        foreach (LogLevel level in levels)
            mask |= 1UL << (int)level;
        if (m_revision == snapshot.revision && m_levels == mask && m_collapsed == collapsed
            && string.Equals(m_query, query, StringComparison.Ordinal))
            return false;
        var filtered = new List<Row>();
        if (collapsed)
        {
            foreach (EditorConsoleGroup group in snapshot.groups)
            {
                var matches = new List<EditorConsoleOccurrence>();
                foreach (EditorConsoleOccurrence occurrence in group.occurrences)
                {
                    if (Matches(occurrence, query, levels))
                        matches.Add(occurrence);
                }
                if (matches.Count > 0)
                    filtered.Add(new Row("group/" + group.identity, matches[^1], matches.AsReadOnly()));
            }
        }
        else
        {
            foreach (EditorConsoleOccurrence occurrence in snapshot.occurrences)
            {
                if (Matches(occurrence, query, levels))
                    filtered.Add(new Row("occurrence/" + occurrence.sequence, occurrence, Array.AsReadOnly(new[] { occurrence })));
            }
        }
        if (collapsed)
        {
            filtered.Sort(static (
                first,
                second
            ) => first.latest.sequence.CompareTo(second.latest.sequence));
        }
        for (int index = 0; index < filtered.Count; index++)
        {
            Row candidate = filtered[index];
            if (m_indices.TryGetValue(candidate.key, out int previousIndex))
            {
                Row previous = rows[previousIndex];
                if (ReferenceEquals(previous.latest, candidate.latest) && previous.occurrences.SequenceEqual(candidate.occurrences))
                    filtered[index] = previous;
            }
        }
        m_indices.Clear();
        for (int index = 0; index < filtered.Count; index++)
            m_indices.Add(filtered[index].key, index);
        rows = filtered.AsReadOnly();
        m_revision = snapshot.revision;
        m_query = query;
        m_levels = mask;
        m_collapsed = collapsed;
        return true;
    }

    internal int FindIndex(string? key) => key is not null && m_indices.TryGetValue(key, out int index) ? index : -1;

    internal void Clear()
    {
        rows = Array.Empty<Row>();
        m_indices.Clear();
        m_revision = -1;
        m_query = string.Empty;
        m_levels = 0;
        m_collapsed = false;
    }

    private static bool Matches(
        EditorConsoleOccurrence occurrence,
        string query,
        IReadOnlySet<LogLevel> levels
    ) => levels.Contains(occurrence.level)
        && (query.Length == 0
            || occurrence.message.Contains(query, StringComparison.OrdinalIgnoreCase)
            || occurrence.source.Contains(query, StringComparison.OrdinalIgnoreCase)
            || occurrence.file.Contains(query, StringComparison.OrdinalIgnoreCase)
            || occurrence.code.Contains(query, StringComparison.OrdinalIgnoreCase)
            || occurrence.category.Contains(query, StringComparison.OrdinalIgnoreCase)
            || occurrence.stackTrace.Contains(query, StringComparison.OrdinalIgnoreCase));

    internal sealed class Row
    {
        internal Row(
            string key,
            EditorConsoleOccurrence latest,
            IReadOnlyList<EditorConsoleOccurrence> occurrences
        ) {
            this.key = key;
            this.latest = latest;
            this.occurrences = occurrences;
            copyTarget = new ConsoleEntryCopyTarget(latest, occurrences.Count);
        }

        internal string key { get; }
        internal EditorConsoleOccurrence latest { get; }
        internal IReadOnlyList<EditorConsoleOccurrence> occurrences { get; }
        internal object copyTarget { get; }
    }
}
