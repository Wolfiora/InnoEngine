using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Shell;

/// <summary>
/// Runs an application-owned frame loop synchronously on its calling thread.
/// </summary>
public sealed class PollingShellFrameDriver : IShellFrameDriver
{
    /// <summary>
    /// Gets whether this driver permits blocking frame pacing (true).
    /// </summary>
    public bool allowsBlockingPacing => true;

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
    public ValueTask RunAsync(
        Func<bool> advanceFrame,
        CancellationToken cancellationToken
    ) {
        ArgumentNullException.ThrowIfNull(advanceFrame);
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!advanceFrame())
                return ValueTask.CompletedTask;
        }
    }
}
