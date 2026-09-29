using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build;

/// <summary>
/// Supplies a missing Player Support Pack without making the game build depend on a particular SDK or platform toolchain.
/// </summary>
public interface IPlayerSupportPackProvisioner
{
    /// <summary>
    /// Installs one target pack into the catalog root before the build resumes.
    /// </summary>
    /// <param name="target">
    /// The missing platform and architecture target.
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
