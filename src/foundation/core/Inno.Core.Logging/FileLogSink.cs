using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Inno.Core.Logging;

/// <summary>
/// Writes complete log entries to uniquely named rotating files under the router's delivery policy.
/// </summary>
public class FileLogSink : ILogSink, IDisposable
{
    /// <summary>
    /// Prefix used for generated log file names.
    /// </summary>
    public const string C_LOG_FILE_PREFIX = "log_";

    private const string C_LOG_FILE_EXTENSION = ".log";
    private static readonly UTF8Encoding S_ENCODING = new(encoderShouldEmitUTF8Identifier: false);
    private static readonly int S_NEWLINE_BYTES = S_ENCODING.GetByteCount(Environment.NewLine);

    private readonly string m_logDirectory;
    private readonly long m_maxFileSize;
    private readonly int m_maxFiles;
    private readonly object m_lifecycleSync = new();
    private string m_currentFile = string.Empty;
    private long m_currentSize;
    private StreamWriter? m_writer;
    private bool m_disposed;

    /// <summary>
    /// Opens an isolated log file without creating another queue or delivery worker.
    /// </summary>
    /// <param name="logDirectory">
    /// Directory in which complete log files are retained.
    /// </param>
    /// <param name="maxFileSizeBytes">
    /// Positive rotation threshold. An entry larger than this threshold occupies its own file.
    /// </param>
    /// <param name="maxFiles">
    /// Positive number of retained files, including the current file.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The directory is blank or invalid.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A rotation or retention limit is not positive.
    /// </exception>
    /// <exception cref="IOException">
    /// The directory or initial file cannot be opened.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The caller cannot create or access the log directory.
    /// </exception>
    public FileLogSink(
        string logDirectory,
        long maxFileSizeBytes = 10 * 1024 * 1024,
        int maxFiles = 10
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(logDirectory);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxFileSizeBytes);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(maxFiles);
        m_logDirectory = Path.GetFullPath(logDirectory);
        m_maxFileSize = maxFileSizeBytes;
        m_maxFiles = maxFiles;
        Directory.CreateDirectory(m_logDirectory);
        try
        {
            OpenWriter();
            CleanupOldFiles();
        }
        catch
        {
            CloseWriter();
            throw;
        }
    }

    /// <summary>
    /// Writes and flushes one complete entry before returning, serializing concurrent callers.
    /// </summary>
    /// <param name="entry">
    /// The immutable entry to persist.
    /// </param>
    /// <remarks>
    /// The router owns scheduling and backpressure. Write failures propagate to its sink quarantine.
    /// Retention cannot remove files locked by another active writer and retries them on later rotation.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// The sink has been disposed.
    /// </exception>
    /// <exception cref="IOException">
    /// An entry cannot be written, flushed or rotated.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The caller cannot create the next log file or access the directory.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A previous failed rotation left no writable file.
    /// </exception>
    public void Receive(LogEntry entry)
    {
        lock (m_lifecycleSync)
        {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            string line = FormatEntry(entry);
            int bytes = S_ENCODING.GetByteCount(line) + S_NEWLINE_BYTES;
            if (m_currentSize > 0 && bytes > m_maxFileSize - m_currentSize)
                RotateFile();
            StreamWriter writer = m_writer
                ?? throw new InvalidOperationException("The file log sink has no writable file after a failed rotation.");
            writer.WriteLine(line);
            writer.Flush();
            m_currentSize += bytes;
        }
    }

    /// <summary>
    /// Waits for an active write and closes the owned file, reporting any final flush failure.
    /// </summary>
    /// <exception cref="IOException">
    /// The owned writer cannot finish flushing or closing its file.
    /// </exception>
    public void Dispose()
    {
        lock (m_lifecycleSync)
        {
            if (m_disposed)
                return;
            m_disposed = true;
            CloseWriter();
        }
    }

    private void RotateFile()
    {
        CloseWriter();
        OpenWriter();
        CleanupOldFiles();
    }

    private void CleanupOldFiles()
    {
        string currentName = Path.GetFileName(m_currentFile);
        FileInfo[] expired = new DirectoryInfo(m_logDirectory)
            .GetFiles(C_LOG_FILE_PREFIX + "*" + C_LOG_FILE_EXTENSION)
            .Where(file => !string.Equals(file.Name, currentName, StringComparison.Ordinal))
            .OrderByDescending(static file => file.LastWriteTimeUtc)
            .ThenByDescending(static file => file.Name, StringComparer.Ordinal)
            .Skip(m_maxFiles - 1)
            .ToArray();
        foreach (FileInfo file in expired)
        {
            try
            {
                // Hold an exclusive open through deletion, including on platforms with advisory sharing.
                using var retired = new FileStream(
                    file.FullName,
                    FileMode.Open,
                    FileAccess.Read,
                    FileShare.None,
                    bufferSize: 1,
                    FileOptions.DeleteOnClose);
            }
            catch (IOException)
            {
                // Another active sink or reader may still own the file.
            }
            catch (UnauthorizedAccessException)
            {
                // Retention does not grant permission to alter another file's access policy.
            }
        }
    }

    private static string FormatEntry(LogEntry entry)
        => $"[{entry.time:yyyy-MM-dd HH:mm:ss.fff}] [{entry.domain}/{entry.scope}] [{entry.level}]: {entry.message} ({entry.file}:{entry.line})";

    private void OpenWriter()
    {
        string timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmssfffffff");
        m_currentFile = Path.Combine(m_logDirectory, $"{C_LOG_FILE_PREFIX}{timestamp}_{Guid.NewGuid():N}{C_LOG_FILE_EXTENSION}");
        var stream = new FileStream(
            m_currentFile,
            FileMode.CreateNew,
            FileAccess.Write,
            FileShare.Read,
            bufferSize: 16 * 1024,
            FileOptions.SequentialScan);
        try
        {
            m_writer = new StreamWriter(stream, S_ENCODING);
            m_currentSize = 0;
        }
        catch
        {
            stream.Dispose();
            throw;
        }
    }

    private void CloseWriter()
    {
        StreamWriter? writer = m_writer;
        m_writer = null;
        writer?.Dispose();
    }
}
