using System.Threading;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Expands a domain's surface contract into ordinary graph stages before source dependency capture.
/// </summary>
public abstract class ShaderTarget
{
    /// <summary>
    /// Builds explicit stage interfaces, resource declarations, techniques and pass states.
    /// </summary>
    /// <param name="context">
    /// Invocation-scoped detached document and complete owner serialization services.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancellation between expansion operations.
    /// </param>
    /// <returns>
    /// A detached graph for the common typed compiler; all required capabilities belong in its pass contracts.
    /// </returns>
    public abstract GraphDocument Expand(
        ShaderTargetContext context,
        CancellationToken cancellationToken
    );
}
