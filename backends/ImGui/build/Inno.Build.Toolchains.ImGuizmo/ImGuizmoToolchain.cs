using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;

using Inno.Build.Toolchains;

namespace Inno.Build.Toolchains.ImGuizmo;

/// <summary>
/// Builds and installs this component through the shared native process lifecycle.
/// </summary>
public static class ImGuizmoToolchain
{
    /// <summary>
    /// Gets the unique native and recipe owners used for sources and target-scoped intermediates.
    /// </summary>
    public static NativeComponentDescriptor componentDescriptor { get; } = new(
        "cimguizmo",
        "backends/ImGui/native/Inno.Native.ImGuizmo/Inno.Native.ImGuizmo.csproj",
        "backends/ImGui/build/Inno.Build.Toolchains.ImGuizmo/Inno.Build.Toolchains.ImGuizmo.csproj",
        bindingDefinition: "backends/ImGui/native/Inno.Native.ImGuizmo/Bindings/bindings.props");

    /// <summary>
    /// Builds and installs the component for the explicitly selected native target.
    /// </summary>
    /// <param name="context">
    /// The checkout and configuration whose sources and outputs belong to this operation.
    /// </param>
    /// <param name="imGui">
    /// The matching published ImGui product providing the native link dependency.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The configuration is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A native build process fails.
    /// </exception>
    /// <param name="cancellationToken">
    /// Cancels child processes and prevents artifact installation after cancellation.
    /// </param>
    /// <returns>
    /// The validated gizmo library linked against the explicitly supplied ImGui product.
    /// </returns>
    /// <exception cref="OperationCanceledException">
    /// The operation was canceled.
    /// </exception>
    public static async Task<NativeBuildProduct> BuildAsync(
        NativeBuildContext context,
        NativeBuildProduct imGui,
        CancellationToken cancellationToken = default
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(imGui);
        NativeToolchainSelection selection = context.RequireToolchain();
        NativeBuildRecipe sourceRecipe = CreateRecipe(context);
        if (imGui.component != "cimgui" || imGui.targetId != selection.targetId)
            throw new ArgumentException("The supplied ImGui product does not match this gizmo target.", nameof(imGui));
        string extension = selection.sharedLibraryExtension == ".dll" ? ".lib" : selection.sharedLibraryExtension;
        string[] libraries = imGui.files.Where(file => Path.GetExtension(file) == extension).ToArray();
        if (libraries.Length != 1)
            throw new InvalidOperationException("The ImGui product must provide exactly one link library.");
        string cimguiDir = Path.Combine(context.engineRoot, "extern", "cimgui");
        NativeBuildRecipe recipe = BindImGui(context, sourceRecipe, imGui);
        return await NativeArtifactPublisher.PublishAsync(
            context,
            recipe,
            async (
                scoped,
                output,
                token
            ) => {
                string build = await NativeCMakeExecutor.BuildAsync(scoped, componentDescriptor,
                    componentDescriptor.GetToolchainRoot(scoped.engineRoot), "cimguizmo",
                    ["-DINNO_CIMGUI_SOURCE_DIR=" + cimguiDir,
                        "-DINNO_CIMGUI_LIBRARY=" + libraries[0]], token).ConfigureAwait(false);
                string library = NativeCMakeExecutor.FindOutput(scoped, build, "*cimguizmo" + selection.sharedLibraryExtension);
                string name = "libcimguizmo-" + scoped.configuration + selection.sharedLibraryExtension;
                File.Copy(library, Path.Combine(output, name));
                token.ThrowIfCancellationRequested();

            }, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Declares gizmo source inputs before preparing bindings or its ImGui link dependency.
    /// </summary>
    /// <param name="context">
    /// The operation with a frozen target SDK and explicit shared component options.
    /// </param>
    /// <returns>
    /// The source recipe; execution adds the validated ImGui library and its artifact identity.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The context is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Required upstream source, SDK or component options are unavailable.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// The requested linkage is not supported by this recipe.
    /// </exception>
    public static NativeBuildRecipe CreateRecipe(NativeBuildContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        NativeToolchainSelection selection = context.RequireToolchain();
        if (context.RequireComponentOptions(componentDescriptor).libraryKind != NativeLibraryKind.Shared)
            throw new NotSupportedException("This presentation recipe supports shared linkage only.");
        string source = componentDescriptor.GetToolchainRoot(context.engineRoot);
        string upstream = Path.Combine(context.engineRoot, "extern", "cimguizmo");
        string imGui = Path.Combine(context.engineRoot, "extern", "cimgui");
        CImguizmoBuildUtils.ValidateSource(upstream, imGui);
        return NativeBuildRecipe.CreateForComponent(context, componentDescriptor,
            "cimguizmo", selection.targetId, [upstream, imGui, Path.Combine(source, "CMakeLists.txt")],
            ["dependency=cimgui"], [Path.Combine(source, "CImguizmoBuildConstants.cs"),
                Path.Combine(source, "CImguizmoBuildUtils.cs"), Path.Combine(source, "ImGuizmoToolchain.cs")]);
    }

    /// <summary>
    /// Freezes the exact ImGui link input for both compilation and complete product cache lookup.
    /// </summary>
    /// <param name="context">
    /// The operation owning the explicit target SDK.
    /// </param>
    /// <param name="sourceRecipe">
    /// The component's previously declared source recipe.
    /// </param>
    /// <param name="imGui">
    /// The borrowed, integrity-checked dependency matching this target.
    /// </param>
    /// <returns>
    /// A recipe including the selected link library and dependency fingerprint.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// The dependency belongs to a different component or target.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The dependency does not expose exactly one link library.
    /// </exception>
    public static NativeBuildRecipe BindImGui(
        NativeBuildContext context,
        NativeBuildRecipe sourceRecipe,
        NativeBuildProduct imGui
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(sourceRecipe);
        ArgumentNullException.ThrowIfNull(imGui);
        NativeToolchainSelection selection = context.RequireToolchain();
        if (imGui.component != "cimgui" || imGui.targetId != selection.targetId)
            throw new ArgumentException("The supplied ImGui product does not match this gizmo target.", nameof(imGui));
        string extension = selection.sharedLibraryExtension == ".dll" ? ".lib" : selection.sharedLibraryExtension;
        string[] libraries = imGui.files.Where(file => Path.GetExtension(file) == extension).ToArray();
        if (libraries.Length != 1)
            throw new InvalidOperationException("The ImGui product must provide exactly one link library.");
        return sourceRecipe.WithDependency(NativeBuildInput.FromPath(context.engineRoot, libraries[0]), imGui.fingerprint);
    }
}
