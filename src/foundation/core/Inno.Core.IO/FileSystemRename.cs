using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace Inno.Core.IO;

internal static class FileSystemRename
{
    private const int C_RETRY_WINDOW_MILLISECONDS = 2000;
    private const int C_RETRY_INTERVAL_MILLISECONDS = 25;

    internal static void MoveDirectory(
        string source,
        string destination,
        CancellationToken cancellationToken = default
    ) => Execute(() => Directory.Move(source, destination), cancellationToken);

    internal static void MoveFile(
        string source,
        string destination,
        bool overwrite
    ) => Execute(() => File.Move(source, destination, overwrite), default);

    private static void Execute(
        Action rename,
        CancellationToken cancellationToken
    ) {
        long started = Stopwatch.GetTimestamp();
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                rename();
                return;
            }
            catch (Exception exception) when (IsTemporarilyBlocked(exception)
                && Stopwatch.GetElapsedTime(started).TotalMilliseconds < C_RETRY_WINDOW_MILLISECONDS)
            {
                // Windows readers and indexers may briefly open files without delete sharing.
                // Retry only the atomic rename; permanent access failures retain their original exception.
                if (cancellationToken.CanBeCanceled)
                {
                    if (cancellationToken.WaitHandle.WaitOne(C_RETRY_INTERVAL_MILLISECONDS))
                        cancellationToken.ThrowIfCancellationRequested();
                }
                else
                {
                    Thread.Sleep(C_RETRY_INTERVAL_MILLISECONDS);
                }
            }
        }
    }

    private static bool IsTemporarilyBlocked(Exception exception)
        => OperatingSystem.IsWindows()
           && exception is IOException or UnauthorizedAccessException
           && exception.HResult is unchecked((int)0x80070005)
               or unchecked((int)0x80070020) or unchecked((int)0x80070021);
}
