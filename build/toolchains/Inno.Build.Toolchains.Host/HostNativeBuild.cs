using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    /// The exact graphics, input, audio, text and product UI products published by this operation.
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
    public static async Task<IReadOnlyList<NativeBuildProduct>> BuildRuntimeAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        cancellationToken.ThrowIfCancellationRequested();
        string sources = Path.Combine(context.engineRoot, "extern");
        if (!Directory.Exists(sources))
            throw new DirectoryNotFoundException($"Native source checkout is unavailable at '{sources}'.");
        context = await HostNativeToolchain.ResolveAsync(context, cancellationToken).ConfigureAwait(false);
        var products = new List<NativeBuildProduct>
        {
            await BgfxNativeBuild.BuildAsync(context, cancellationToken).ConfigureAwait(false),
            await Sdl3Toolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false),
            await MiniAudioToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false),
            await TextToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false),
            await UiToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false)
        };
        return products.AsReadOnly();
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
    /// The exact runtime and authoring products published by this operation, including native link dependencies.
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
    public static async Task<IReadOnlyList<NativeBuildProduct>> BuildEditorAsync(
        NativeBuildContext context,
        CancellationToken cancellationToken = default
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        context = await HostNativeToolchain.ResolveAsync(context, cancellationToken).ConfigureAwait(false);
        var products = (await BuildRuntimeAsync(context, cancellationToken).ConfigureAwait(false)).ToList();
        products.Add(await BgfxToolsBuild.BuildAsync(context, cancellationToken).ConfigureAwait(false));
        NativeBuildProduct imGui = await ImGuiToolchain.BuildAsync(context, cancellationToken).ConfigureAwait(false);
        products.Add(imGui);
        products.Add(await ImGuizmoToolchain.BuildAsync(context, imGui, cancellationToken).ConfigureAwait(false));
        return products.AsReadOnly();
    }
}
