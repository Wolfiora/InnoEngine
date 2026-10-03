using System;
using System.Threading;
using System.Threading.Tasks;

using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.ImGui;
using Inno.Build.Toolchains.ImGuizmo;
using Inno.Build.Toolchains.MiniAudio;
using Inno.Build.Toolchains.Sdl3;
using Inno.Build.Toolchains.Text;
using Inno.Build.Toolchains.UI;

namespace Inno.Build.Toolchains.Host;

/// <summary>
/// Composes the built-in native toolchains using one explicit checkout and cancellation lifetime.
/// </summary>
public static class HostNativeBuild
{
    /// <summary>
    /// Prepares every native component required by the desktop Player.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose runtime closure must be prepared.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels active process trees and subsequent component stages.
    /// </param>
    /// <returns>
    /// Completion after graphics, input, audio, text and product UI outputs are available.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A component process fails or a required output is missing.
    /// </exception>
    public static async Task BuildRuntimeAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        await BgfxNativeBuild.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        await Sdl3Toolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        await MiniAudioToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        await TextToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        await UiToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Prepares the runtime closure, offline graphics tools and Editor presentation components.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration selected by the Editor build.
    /// </param>
    /// <param name="cancellationToken">
    /// Cancels active process trees and subsequent component stages.
    /// </param>
    /// <returns>
    /// Completion after all native inputs for the Editor and its authoring tools are available.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A component process fails or a required output is missing.
    /// </exception>
    public static async Task BuildEditorAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        await BuildRuntimeAsync(context, cancellationToken).ConfigureAwait(false);
        await BgfxToolsBuild.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        await ImGuiToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        await ImGuizmoToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
    }
}
