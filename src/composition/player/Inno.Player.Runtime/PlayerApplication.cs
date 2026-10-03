using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Player.Runtime;

/// <summary>
/// Owns the common Player startup, frame execution and verified resource retirement.
/// </summary>
public static class PlayerApplication
{
    /// <summary>
    /// Runs a frozen game deployment using explicitly supplied host services.
    /// </summary>
    /// <param name="options">
    /// The prepared content and platform service composition.
    /// </param>
    /// <param name="cancellationToken">
    /// Requests termination of frame scheduling.
    /// </param>
    /// <returns>
    /// Zero after orderly shutdown; startup, execution and retirement failures propagate.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Options or a required host service is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Frame scheduling was canceled.
    /// </exception>
    public static async Task<int> RunAsync(
        PlayerLaunchOptions options,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.adapters);
        ArgumentNullException.ThrowIfNull(options.moduleActivator);
        ArgumentNullException.ThrowIfNull(options.frameDriver);
        using GamePlayerHost host = GamePlayerHost.Create(options);
        return await host.RunGameAsync(options.frameDriver, options.smokeFrameLimit, cancellationToken);
    }
}
