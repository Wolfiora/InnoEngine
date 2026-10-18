using System;

namespace Inno.Core.Logging;

/// <summary>
/// Writes log entries to the process console, using level-based colors when a terminal is available.
/// </summary>
public class ConsoleLogSink : ILogSink
{
    private readonly bool m_useColors;

    /// <summary>
    /// Creates a console sink with explicitly selected terminal capabilities.
    /// </summary>
    /// <param name="useColors">
    /// Whether the host supports console colors; redirected output always uses plain text.
    /// </param>
    public ConsoleLogSink(bool useColors = false) => m_useColors = useColors;

    /// <summary>
    /// Writes the specified entry to standard output.
    /// </summary>
    /// <param name="entry">
    /// The log entry to print.
    /// </param>
    public void Receive(LogEntry entry)
    {
        if (!m_useColors || Console.IsOutputRedirected)
        {
            WriteEntry(entry);
            return;
        }

        ConsoleColor originalColor = Console.ForegroundColor;
        try
        {
            Console.ForegroundColor = entry.level switch
            {
                LogLevel.Debug => ConsoleColor.DarkGray,
                LogLevel.Info => ConsoleColor.Green,
                LogLevel.Warn => ConsoleColor.Yellow,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Fatal => ConsoleColor.Magenta,
                _ => ConsoleColor.White
            };
            WriteEntry(entry);
        }
        finally
        {
            Console.ForegroundColor = originalColor;
        }
    }

    private static void WriteEntry(LogEntry entry)
        => Console.WriteLine($"[{entry.time:HH:mm:ss}] <{entry.domain}/{entry.scope}> [{entry.category}]: {entry.message}");
}
