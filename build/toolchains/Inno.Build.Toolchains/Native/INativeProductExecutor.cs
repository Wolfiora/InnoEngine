using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Executes an explicitly selected native product graph without target discovery.
/// </summary>
public interface INativeProductExecutor
{
    /// <summary>
    /// Builds or reuses the complete product with the operation's frozen tools.
    /// </summary>
    /// <param name="context">
    /// The frozen operation.
    /// </param>
    /// <param name="plan">
    /// The complete dependency-ordered component plan.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels waits and unpublished work.
    /// </param>
    /// <returns>
    /// The owned complete output and binding closure.
    /// </returns>
    Task<ProductNativeBuildResult> BuildAsync(
        NativeBuildContext context,
        ProductNativeBuildPlan plan,
        CancellationToken cancellationToken
    );
}
