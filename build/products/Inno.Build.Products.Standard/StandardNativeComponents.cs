using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.ImGui;
using Inno.Build.Toolchains.ImGuizmo;
using Inno.Build.Toolchains.MiniAudio;
using Inno.Build.Toolchains.Sdl3;
using Inno.Build.Toolchains.Text;
using Inno.Build.Toolchains.UI;

namespace Inno.Build.Products.Standard;

/// <summary>
/// Declares the standard product closures while keeping backend selection out of shared build mechanisms.
/// </summary>
public static class StandardNativeComponents
{
    /// <summary>
    /// Composes the standard Player's reusable native components with explicit deployment policies.
    /// </summary>
    /// <returns>
    /// A fresh immutable Player plan without resolving tools or touching source files.
    /// </returns>
    /// <param name="profile">
    /// The selected integration-owned BGFX SDK invocation for standalone component builds.
    /// </param>
    /// <param name="options">
    /// The platform-owned configuration for each named component.
    /// </param>
    /// <param name="layout">
    /// The platform-owned destination of each component output, or null to exclude it.
    /// </param>
    public static ProductNativeBuildPlan CreatePlayer(
        BgfxNativeBuildProfile profile,
        Func<string, NativeComponentBuildOptions> options,
        Func<string, string, string, string?> layout
    ) => new("player", CreatePlayerSteps(RequireProfile(profile), options, layout), StandardNativeDeployment.CreateMetadata);

    /// <summary>
    /// Selects only the offline graphics compiler component needed for shader publication.
    /// </summary>
    /// <returns>
    /// An immutable tool product without runtime or Editor presentation dependencies.
    /// </returns>
    /// <param name="profile">
    /// The selected integration-owned BGFX SDK invocation for standalone component builds.
    /// </param>
    /// <param name="options">
    /// The platform-owned configuration for each named component.
    /// </param>
    /// <param name="layout">
    /// The platform-owned destination of each component output, or null to exclude it.
    /// </param>
    public static ProductNativeBuildPlan CreateShaderTools(
        BgfxNativeBuildProfile profile,
        Func<string, NativeComponentBuildOptions> options,
        Func<string, string, string, string?> layout
    ) => new("shader-tools", [CreateShaderToolsStep(RequireProfile(profile), options, layout)], StandardNativeDeployment.CreateMetadata);

    /// <summary>
    /// Composes the shared runtime closure and the Editor's presentation and authoring tools.
    /// </summary>
    /// <returns>
    /// A fresh immutable Editor plan with the ImGuizmo dependency declared explicitly.
    /// </returns>
    /// <param name="profile">
    /// The selected integration-owned BGFX SDK invocation for standalone component builds.
    /// </param>
    /// <param name="options">
    /// The platform-owned configuration for each named component.
    /// </param>
    /// <param name="layout">
    /// The platform-owned destination of each component output, or null to exclude it.
    /// </param>
    public static ProductNativeBuildPlan CreateEditor(
        BgfxNativeBuildProfile profile,
        Func<string, NativeComponentBuildOptions> options,
        Func<string, string, string, string?> layout
    ) {
        List<ProductNativeBuildStep> steps = CreatePlayerSteps(RequireProfile(profile), options, layout);
        steps.Add(CreateShaderToolsStep(profile, options, layout));
        steps.Add(new("cimgui", ImGuiToolchain.componentDescriptor, options("cimgui"), [],
            static (
                context,
                dependencies,
                token
            ) => ImGuiToolchain.BuildAsync(context, token),
            ImGuiToolchain.CreateRecipe,
            (
                relative,
                target
            ) => layout("cimgui", relative, target)));
        steps.Add(new("cimguizmo", ImGuizmoToolchain.componentDescriptor, options("cimguizmo"), ["cimgui"],
            static (
                context,
                dependencies,
                token
            ) => ImGuizmoToolchain.BuildAsync(context, dependencies["cimgui"], token),
            ImGuizmoToolchain.CreateRecipe,
            (
                relative,
                target
            ) => layout("cimguizmo", relative, target),
            bindDependencies: static (
                context,
                recipe,
                dependencies
            ) => ImGuizmoToolchain.BindImGui(context, recipe, dependencies["cimgui"])));
        return new("editor", steps, StandardNativeDeployment.CreateMetadata);
    }

    /// <summary>
    /// Declares the real static component closure consumed by an explicit aggregate executor.
    /// </summary>
    /// <returns>
    /// A Player closure with static recipes and no unused standalone producers.
    /// </returns>
    /// <param name="options">
    /// The platform-owned configuration for each named component.
    /// </param>
    /// <param name="layout">
    /// The platform-owned destination of each component output, or null to exclude it.
    /// </param>
    public static ProductNativeBuildPlan CreateStaticPlayer(
        Func<string, NativeComponentBuildOptions> options,
        Func<string, string, string, string?> layout
    ) {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(layout);
        List<ProductNativeBuildStep> steps = CreatePlayerSteps(null, options, layout);
        if (steps.Any(static step => step.options.libraryKind != NativeLibraryKind.Static))
            throw new ArgumentException("Static aggregation requires explicit static component linkage.", nameof(options));
        return new("player", steps);
    }

    private static BgfxNativeBuildProfile RequireProfile(BgfxNativeBuildProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return profile;
    }

    private static ProductNativeBuildStep CreateShaderToolsStep(
        BgfxNativeBuildProfile profile,
        Func<string, NativeComponentBuildOptions> options,
        Func<string, string, string, string?> layout
    ) => new("bgfx-tools", BgfxToolsBuild.componentDescriptor, options("bgfx-tools"), [],
            (
                context,
                dependencies,
                token
            ) => BgfxToolsBuild.BuildAsync(context, profile, token),
            context => BgfxToolsBuild.CreateRecipe(context, profile),
            (
                relative,
                target
            ) => layout("bgfx-tools", relative, target),
            configuration: "release");

    private static List<ProductNativeBuildStep> CreatePlayerSteps(
        BgfxNativeBuildProfile? profile,
        Func<string, NativeComponentBuildOptions> options,
        Func<string, string, string, string?> layout
    ) =>
    [
        new("bgfx", BgfxNativeBuild.componentDescriptor, options("bgfx"), [],
            profile is null ? null : (
                context,
                dependencies,
                token
            ) => BgfxNativeBuild.BuildAsync(context, profile, token),
            profile is null ? null : context => BgfxNativeBuild.CreateRecipe(context, profile),
            (
                relative,
                target
            ) => layout("bgfx", relative, target)),
        new("sdl3", Sdl3Toolchain.componentDescriptor, options("sdl3"), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => Sdl3Toolchain.BuildAsync(context, token),
            profile is null ? null : Sdl3Toolchain.CreateRecipe,
            (
                relative,
                target
            ) => layout("sdl3", relative, target)),
        new("miniaudio", MiniAudioToolchain.componentDescriptor, options("miniaudio"), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => MiniAudioToolchain.BuildAsync(context, token),
            profile is null ? null : MiniAudioToolchain.CreateRecipe,
            (
                relative,
                target
            ) => layout("miniaudio", relative, target)),
        new("text", TextToolchain.componentDescriptor, options("text"), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => TextToolchain.BuildAsync(context, token),
            profile is null ? null : TextToolchain.CreateRecipe,
            (
                relative,
                target
            ) => layout("text", relative, target)),
        new("ui", UiToolchain.componentDescriptor, options("ui"), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => UiToolchain.BuildAsync(context, token),
            profile is null ? null : UiToolchain.CreateRecipe,
            (
                relative,
                target
            ) => layout("ui", relative, target))
    ];

}
