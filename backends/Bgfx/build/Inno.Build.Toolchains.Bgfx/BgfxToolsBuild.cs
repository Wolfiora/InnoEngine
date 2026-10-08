using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Publishes pinned graphics tools from an isolated source snapshot.
/// </summary>
public static class BgfxToolsBuild
{
    /// <summary>
    /// Builds and installs graphics artifacts using explicit platform-owned SDK configuration.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose sources and outputs belong to this operation.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is not debug or release.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A native build process fails.
    /// </exception>
    /// <param name="profile">
    /// The platform-owned project generation, SDK invocation and output selection.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels child processes and prevents artifact installation after cancellation.
    /// </param>
    /// <returns>
    /// The validated product with an exact graphics library or offline-tool output closure.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        BgfxNativeBuildProfile profile,
        CancellationToken cancellationToken = default
    ) => BgfxBuildSession.PublishAsync(context, profile, true, cancellationToken);
}
