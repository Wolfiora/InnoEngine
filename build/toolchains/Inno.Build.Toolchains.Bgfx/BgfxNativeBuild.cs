using System;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Build.Toolchains.Bgfx;

/// <summary>
/// Publishes pinned graphics runtime libraries from an isolated source snapshot.
/// </summary>
public static class BgfxNativeBuild
{
    /// <summary>
    /// Builds and installs the native graphics artifacts for the current host.
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
        CancellationToken cancellationToken = default
    ) => BgfxBuildSession.PublishAsync(context, false, cancellationToken);
}
