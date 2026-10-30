using System;
using System.Text;
using Inno.Editor.Diagnostics;

namespace Inno.Editor.Panel.Logging;
internal readonly struct ConsoleEntryCopyTarget
{
    private readonly EditorConsoleOccurrence m_entry;
    private readonly int m_repeatCount;
    internal ConsoleEntryCopyTarget(
        EditorConsoleOccurrence entry,
        int repeatCount
    ) {
        m_entry = entry;
        m_repeatCount = repeatCount;
    }

    internal string message => m_entry.displayMessage;
    internal string fullText => FormatFullText(m_entry, m_repeatCount);

    private static string FormatFullText(
        EditorConsoleOccurrence entry,
        int repeatCount
    ) {
        var builder = new StringBuilder();
        builder.Append('[').Append(entry.time.ToString("yyyy-MM-dd HH:mm:ss.fff")).Append("] [").Append(entry.kind).Append("] [").Append(entry.level).Append("] [").Append(entry.category).Append(']');
        builder.Append(' ').Append(entry.displayMessage);
        if (repeatCount > 1)
            builder.Append(" (x").Append(repeatCount).Append(')');
        if (!string.IsNullOrWhiteSpace(entry.file))
        {
            builder.AppendLine();
            builder.Append(entry.file);
            if (entry.line > 0)
            {
                builder.Append('(').Append(entry.line);
                if (entry.column > 0)
                    builder.Append(',').Append(entry.column);
                builder.Append(')');
            }
        }

        return builder.ToString();
    }
}
