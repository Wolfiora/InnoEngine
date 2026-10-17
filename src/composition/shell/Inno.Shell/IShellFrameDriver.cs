using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Shell;

/// <summary>
/// Gives a host ownership of frame scheduling while the shell owns frame execution and retirement.
/// </summary>
public interface IShellFrameDriver
{
    /// <summary>
    /// Gets whether the shell may block the calling thread to enforce its frame rate.
    /// </summary>
    bool allowsBlockingPacing { get; }

    /// <summary>
    /// Schedules frames on the owning thread until a frame callback requests termination.
    /// </summary>
    /// <param name="advanceFrame">
    /// Executes one frame and returns true when the host should schedule another frame.
    /// </param>
    /// <param name="cancellationToken">
    /// Requests termination before the next frame.
    /// </param>
    /// <returns>
    /// Completion after scheduling ends; callback failures must propagate to the shell.
    /// </returns>
    ValueTask RunAsync(
        Func<bool> advanceFrame,
        CancellationToken cancellationToken
    );
}
