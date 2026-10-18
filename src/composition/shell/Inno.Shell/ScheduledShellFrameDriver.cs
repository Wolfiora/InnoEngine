using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Shell;

/// <summary>
/// Runs frames when an external host grants a nonblocking presentation opportunity.
/// </summary>
public sealed class ScheduledShellFrameDriver : IShellFrameDriver
{
    private readonly Func<CancellationToken, ValueTask> m_nextFrame;

    /// <summary>
    /// Creates a driver whose scheduling callback resumes on the frame owner's thread.
    /// </summary>
    /// <param name="nextFrame">
    /// The callback that waits for a presentation opportunity and observes cancellation.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// The callback is null.
    /// </exception>
    public ScheduledShellFrameDriver(Func<CancellationToken, ValueTask> nextFrame)
    {
        m_nextFrame = nextFrame ?? throw new ArgumentNullException(nameof(nextFrame));
    }

    /// <summary>
    /// Gets whether this driver permits blocking frame pacing (false).
    /// </summary>
    public bool allowsBlockingPacing => false;

    /// <summary>
    /// Advances frames until the callback stops the loop, cancellation is requested or a callback fails.
    /// </summary>
    /// <param name="advanceFrame">
    /// The owner-thread frame callback; false terminates the loop.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels before the next frame can execute.
    /// </param>
    /// <returns>
    /// Completion when the callback stops; cancellation and callback failures propagate to the owner.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The frame callback is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Cancellation was requested before a frame.
    /// </exception>
    public async ValueTask RunAsync(
        Func<bool> advanceFrame,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(advanceFrame);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await m_nextFrame(cancellationToken);
            cancellationToken.ThrowIfCancellationRequested();
            if (!advanceFrame())
                return;
        }
    }
}
