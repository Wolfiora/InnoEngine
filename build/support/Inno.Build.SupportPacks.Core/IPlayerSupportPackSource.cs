using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.SupportPacks;

/// <summary>
/// Prepares one platform's runtime and compiler inputs without owning installation or replacement.
/// </summary>
public interface IPlayerSupportPackSource : IPlayerSupportPackValidator
{
    /// <summary>
    /// Gets the stable target identity implemented by this source.
    /// </summary>
    BuildTargetId target { get; }

    /// <summary>
    /// Builds the complete pack in the supplied isolated directory.
    /// </summary>
    /// <param name="context">
    /// The source checkout, staging directory and selected SDK executable.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation before installation.
    /// </param>
    /// <returns>
    /// Completion after all required runtime and compiler inputs are prepared.
    /// </returns>
    ValueTask PrepareAsync(
        PlayerSupportPackBuildContext context,
        CancellationToken cancellationToken
    );
}
