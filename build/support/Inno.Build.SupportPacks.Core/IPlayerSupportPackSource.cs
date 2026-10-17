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
    /// Validates discovery inputs and freezes a plan before publication output exists.
    /// </summary>
    /// <param name="context">
    /// The source checkout and host used for read-only SDK discovery.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels preparation before installation.
    /// </param>
    /// <returns>
    /// A frozen plan with no running work; missing tools and unsupported hosts fail here.
    /// </returns>
    ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    );
}
