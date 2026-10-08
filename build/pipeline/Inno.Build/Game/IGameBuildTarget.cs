using Inno.Build.Managed;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build;

/// <summary>
/// Defines the replaceable platform packaging boundary for one game build target.
/// </summary>
public interface IGameBuildTarget : IPlayerSupportPackValidator
{
    /// <summary>
    /// Gets the stable target identity implemented by this packager.
    /// </summary>
    BuildTargetId id { get; }

    /// <summary>
    /// Gets the managed publisher selected when the profile does not specify an override.
    /// </summary>
    ManagedDeploymentId defaultManagedDeployment { get; }

    /// <summary>
    /// Gets the managed toolchain target identifier required by this platform composition.
    /// </summary>
    string runtimeIdentifier { get; }

    /// <summary>
    /// Gets the user-facing target name presented by authoring hosts.
    /// </summary>
    string displayName { get; }

    /// <summary>
    /// Composes one platform output from a verified Support Pack and source-free content directory.
    /// </summary>
    /// <param name="context">
    /// The isolated package context owned by the current build.
    /// </param>
    /// <param name="cancellationToken">
    /// The token that cancels work before the build is committed.
    /// </param>
    /// <returns>
    /// The single platform output path created beneath <see cref="GameBuildPackageContext.outputDirectory"/>.
    /// </returns>
    /// <exception cref="System.OperationCanceledException">
    /// Thrown when packaging is canceled.
    /// </exception>
    ValueTask<string> PackageAsync(
        GameBuildPackageContext context,
        CancellationToken cancellationToken = default
    );
}
