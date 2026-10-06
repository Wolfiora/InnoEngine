using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Core.IO;

/// <summary>
/// Owns shared reading or exclusive writing rights coordinated by independent processes.
/// </summary>
public sealed class FileLease : IDisposable
{
    private readonly FileStream m_stream;

    private FileLease(FileStream stream) => m_stream = stream;

    /// <summary>
    /// Waits for exclusive ownership without blocking an owner thread or changing shared data.
    /// </summary>
    /// <param name="path">
    /// The absolute lease file path; all participants must use the same persistent path.
    /// </param>
    /// <param name="timeout">
    /// The maximum wait, or <see cref="Timeout.InfiniteTimeSpan"/> for no time limit.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels waiting before ownership is returned.
    /// </param>
    /// <returns>
    /// The exclusive lease whose disposal permits the next participant to acquire ownership.
    /// </returns>
    /// <remarks>
    /// Lease files remain after disposal. Removing them would let Unix processes lock different
    /// inodes for the same path while an earlier participant still owns the original inode.
    /// </remarks>
    /// <exception cref="ArgumentException">
    /// The path is empty or relative, or the timeout is negative and not infinite.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// Another participant retains ownership beyond the permitted wait.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Waiting was canceled; no lease is returned.
    /// </exception>
    /// <exception cref="IOException">
    /// The lease cannot be opened for a reason other than competing ownership.
    /// </exception>
    /// <exception cref="UnauthorizedAccessException">
    /// The operating system denies access to the lease path.
    /// </exception>
    public static async ValueTask<FileLease> AcquireAsync(
        string path,
        TimeSpan timeout,
        CancellationToken cancellationToken = default
    ) => await AcquireCoreAsync(path, timeout, FileMode.OpenOrCreate, FileAccess.ReadWrite,
        FileShare.None, cancellationToken).ConfigureAwait(false);

    /// <summary>
    /// Pins an existing generation against exclusive retirement while permitting other readers.
    /// </summary>
    /// <param name="path">
    /// The absolute persistent lease path previously initialized by an exclusive owner.
    /// </param>
    /// <param name="timeout">
    /// The maximum wait, or <see cref="Timeout.InfiniteTimeSpan"/> for no time limit.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels waiting before ownership is returned.
    /// </param>
    /// <returns>
    /// A shared lease; an exclusive lease cannot be acquired until every reader releases ownership.
    /// </returns>
    /// <exception cref="FileNotFoundException">
    /// The owner has not initialized the persistent lease file.
    /// </exception>
    /// <exception cref="TimeoutException">
    /// An exclusive owner retains the file beyond the permitted wait.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Waiting was canceled; no lease is returned.
    /// </exception>
    public static async ValueTask<FileLease> AcquireSharedAsync(
        string path,
        TimeSpan timeout,
        CancellationToken cancellationToken = default
    ) => await AcquireCoreAsync(path, timeout, FileMode.Open, FileAccess.Read,
        FileShare.Read, cancellationToken).ConfigureAwait(false);

    private static async ValueTask<FileLease> AcquireCoreAsync(
        string path,
        TimeSpan timeout,
        FileMode mode,
        FileAccess access,
        FileShare sharing,
        CancellationToken cancellationToken
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(path);
        if (!Path.IsPathFullyQualified(path))
            throw new ArgumentException("A file lease requires an absolute path.", nameof(path));
        if (timeout < TimeSpan.Zero && timeout != Timeout.InfiniteTimeSpan)
            throw new ArgumentOutOfRangeException(nameof(timeout));
        cancellationToken.ThrowIfCancellationRequested();
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        Stopwatch elapsed = Stopwatch.StartNew();
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                FileStream stream = new(path, mode, access, sharing);
                if (cancellationToken.IsCancellationRequested)
                {
                    stream.Dispose();
                    cancellationToken.ThrowIfCancellationRequested();
                }
                return new FileLease(stream);
            }
            catch (IOException failure) when ((failure.HResult & 0xffff) is 11 or 32 or 33)
            {
                if (timeout != Timeout.InfiniteTimeSpan && elapsed.Elapsed >= timeout)
                    throw new TimeoutException($"File ownership timed out at '{path}'.", failure);
                TimeSpan delay = TimeSpan.FromMilliseconds(20);
                if (timeout != Timeout.InfiniteTimeSpan && timeout - elapsed.Elapsed < delay)
                    delay = timeout - elapsed.Elapsed;
                if (delay > TimeSpan.Zero)
                    await Task.Delay(delay, cancellationToken).ConfigureAwait(false);
            }
        }
    }

    /// <summary>
    /// Releases ownership; repeated disposal has no effect and the lease file is retained.
    /// </summary>
    public void Dispose() => m_stream.Dispose();
}
