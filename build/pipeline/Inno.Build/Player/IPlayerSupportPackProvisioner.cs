using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build;

/// <summary>
/// Prepares current Player Support Pack inputs without coupling game builds to an SDK or platform toolchain.
/// </summary>
public interface IPlayerSupportPackProvisioner
{
    /// <summary>
    /// Checks current source inputs and selects a complete immutable pack before the build resumes.
    /// </summary>
    /// <param name="target">
    /// The platform and architecture target whose current inputs are required.
    /// </param>
    /// <param name="supportPackRoot">
    /// The catalog directory where the verified pack must be installed.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation before the installation commits.
    /// </param>
    /// <returns>
    /// An operation that completes after the pack has been installed.
    /// </returns>
    ValueTask ProvisionAsync(
        BuildTargetId target,
        string supportPackRoot,
        CancellationToken cancellationToken = default
    );
}
