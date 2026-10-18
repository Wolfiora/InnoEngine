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
    /// <remarks>
    /// Hosts must preserve their owner thread across asynchronous startup and frame callbacks using
    /// their synchronization context. A blocking host can use OwnerThreadExecution.Run.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// Options or a required host service is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// Frame scheduling was canceled.
    /// </exception>
    /// <exception cref="System.IO.InvalidDataException">
    /// Deployment metadata, linked code, or the prepared content identity is inconsistent.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A host factory does not provide its required product, or runtime startup or retirement fails.
    /// </exception>
    public static async Task<int> RunAsync(
        PlayerLaunchOptions options,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.modules);
        ArgumentNullException.ThrowIfNull(options.types);
        ArgumentNullException.ThrowIfNull(options.serializationMetadata);
        ArgumentNullException.ThrowIfNull(options.adapters);
        ArgumentNullException.ThrowIfNull(options.moduleActivator);
        ArgumentNullException.ThrowIfNull(options.frameDriver);
        ArgumentNullException.ThrowIfNull(options.contentSource);
        ArgumentNullException.ThrowIfNull(options.createStorage);
        cancellationToken.ThrowIfCancellationRequested();
        using GamePlayerHost host = await GamePlayerHost.CreateAsync(options, cancellationToken);
        return await host.RunGameAsync(options.frameDriver, options.smokeFrameLimit, cancellationToken);
    }
}
