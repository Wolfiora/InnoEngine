using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Managed;

/// <summary>
/// Publishes an explicit code closure independently of platform layout, content packaging and signing.
/// </summary>
public interface IManagedDeploymentCompiler
{
    /// <summary>
    /// Gets the open stable implementation identity.
    /// </summary>
    ManagedDeploymentId id { get; }

    /// <summary>
    /// Gets the provider's actual target and execution capabilities.
    /// </summary>
    ManagedDeploymentCapabilities capabilities { get; }

    /// <summary>
    /// Compiles or publishes the prepared code closure into isolated managed staging.
    /// </summary>
    /// <param name="request">
    /// The immutable prepared project, target, frozen inputs and staging locations.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels the publisher and waits for its process tree and output readers to retire.
    /// </param>
    /// <returns>
    /// Verified output evidence; failed or canceled publication never returns a successful result.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// The target is unsupported, required toolchain inputs are absent, or publication fails.
    /// </exception>
    /// <exception cref="System.OperationCanceledException">
    /// Publication was canceled and all owned work has stopped.
    /// </exception>
    ValueTask<ManagedDeploymentResult> CompileAsync(
        ManagedDeploymentRequest request,
        CancellationToken cancellationToken = default
    );
}
