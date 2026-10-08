using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains;

/// <summary>
/// Resolves one platform's SDK for an explicit target on a declared tool execution host.
/// </summary>
public interface INativeToolchainProvider
{
    /// <summary>
    /// Freezes compiler tools, SDK inputs and child environment before a native operation starts.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration owned by the requesting operation.
    /// </param>
    /// <param name="host">
    /// The explicitly declared execution host, independent of the target.
    /// </param>
    /// <param name="targetId">
    /// The exact requested target; unsupported combinations must fail.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels SDK discovery before any product staging.
    /// </param>
    /// <returns>
    /// A complete immutable tool selection for the requested host and target.
    /// </returns>
    ValueTask<NativeToolchainSelection> ResolveAsync(
        NativeBuildContext context,
        BuildHostDescriptor host,
        string targetId,
        CancellationToken cancellationToken
    );
}
