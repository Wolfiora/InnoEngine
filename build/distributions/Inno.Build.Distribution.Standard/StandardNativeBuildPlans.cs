using System;
using System.Collections.Generic;
using System.IO;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx;
using Inno.Build.Toolchains.ImGui;
using Inno.Build.Toolchains.ImGuizmo;
using Inno.Build.Toolchains.MiniAudio;
using Inno.Build.Toolchains.Sdl3;
using Inno.Build.Toolchains.Text;
using Inno.Build.Toolchains.UI;

namespace Inno.Build.Distribution.Standard;

/// <summary>
/// Declares the standard product closures while keeping backend selection out of shared build mechanisms.
/// </summary>
public static class StandardNativeBuildPlans
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
    public static ProductNativeBuildPlan CreatePlayer(BgfxNativeBuildProfile profile) => new("player", CreatePlayerSteps(RequireProfile(profile)));

    /// <summary>
    /// Selects only the offline graphics compiler component needed for shader publication.
    /// </summary>
    /// <returns>
    /// An immutable tool product without runtime or Editor presentation dependencies.
    /// </returns>
    /// <param name="profile">
    /// The selected integration-owned BGFX SDK invocation for standalone component builds.
    /// </param>
    public static ProductNativeBuildPlan CreateShaderTools(BgfxNativeBuildProfile profile) => new("shader-tools", [CreateShaderToolsStep(RequireProfile(profile))]);

    /// <summary>
    /// Composes the shared runtime closure and the Editor's presentation and authoring tools.
    /// </summary>
    /// <returns>
    /// A fresh immutable Editor plan with the ImGuizmo dependency declared explicitly.
    /// </returns>
    /// <param name="profile">
    /// The selected integration-owned BGFX SDK invocation for standalone component builds.
    /// </param>
    public static ProductNativeBuildPlan CreateEditor(BgfxNativeBuildProfile profile)
    {
        List<ProductNativeBuildStep> steps = CreatePlayerSteps(RequireProfile(profile));
        steps.Add(CreateShaderToolsStep(profile));
        steps.Add(new("cimgui", ImGuiToolchain.componentDescriptor, new(NativeLibraryKind.Shared), [],
            static (
                context,
                dependencies,
                token
            ) => ImGuiToolchain.BuildAsync(context, token),
            static (
                relative,
                target
            ) => RuntimePath("cimgui", relative, target)));
        steps.Add(new("cimguizmo", ImGuizmoToolchain.componentDescriptor, new(NativeLibraryKind.Shared), ["cimgui"],
            static (
                context,
                dependencies,
                token
            ) => ImGuizmoToolchain.BuildAsync(context, dependencies["cimgui"], token),
            static (
                relative,
                target
            ) => RuntimePath("cimguizmo", relative, target)));
        return new("editor", steps);
    }

    /// <summary>
    /// Declares the real static component closure consumed by an explicit aggregate executor.
    /// </summary>
    /// <returns>
    /// A Player closure with static recipes and no unused standalone producers.
    /// </returns>
    /// <param name="graphicsOptions">
    /// The selected platform and graphics integration's explicit static component configuration.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Graphics configuration is null.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Graphics configuration requests shared rather than static linkage.
    /// </exception>
    public static ProductNativeBuildPlan CreateStaticPlayer(NativeComponentBuildOptions graphicsOptions)
    {
        ArgumentNullException.ThrowIfNull(graphicsOptions);
        if (graphicsOptions.libraryKind != NativeLibraryKind.Static)
            throw new ArgumentException("Static Player aggregation requires static graphics linkage.", nameof(graphicsOptions));
        return new("player", CreatePlayerSteps(null, graphicsOptions));
    }

    private static BgfxNativeBuildProfile RequireProfile(BgfxNativeBuildProfile profile)
    {
        ArgumentNullException.ThrowIfNull(profile);
        return profile;
    }

    private static ProductNativeBuildStep CreateShaderToolsStep(BgfxNativeBuildProfile profile) => new("bgfx-tools", BgfxNativeBuild.componentDescriptor, new(NativeLibraryKind.Shared), [],
            (
                context,
                dependencies,
                token
            ) => BgfxToolsBuild.BuildAsync(context, profile, token),
            static (
                relative,
                target
            ) => relative.StartsWith("includes" + Path.DirectorySeparatorChar, System.StringComparison.Ordinal)
                ? Path.Combine("bgfx", relative) : Path.Combine("bgfx", target, "tools", relative));

    private static List<ProductNativeBuildStep> CreatePlayerSteps(
        BgfxNativeBuildProfile? profile,
        NativeComponentBuildOptions? graphicsOptions = null
    ) =>
    [
        new("bgfx", BgfxNativeBuild.componentDescriptor, graphicsOptions ?? new(NativeLibraryKind.Shared), [],
            profile is null ? null : (
                context,
                dependencies,
                token
            ) => BgfxNativeBuild.BuildAsync(context, profile, token),
            static (
                relative,
                target
            ) => RuntimePath("bgfx", relative, target)),
        new("sdl3", Sdl3Toolchain.componentDescriptor, new(profile is null ? NativeLibraryKind.Static : NativeLibraryKind.Shared), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => Sdl3Toolchain.BuildAsync(context, token),
            static (
                relative,
                target
            ) => RuntimePath("sdl3", relative, target)),
        new("miniaudio", MiniAudioToolchain.componentDescriptor, new(profile is null ? NativeLibraryKind.Static : NativeLibraryKind.Shared), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => MiniAudioToolchain.BuildAsync(context, token),
            static (
                relative,
                target
            ) => RuntimePath("miniaudio", relative, target)),
        new("text", TextToolchain.componentDescriptor, new(profile is null ? NativeLibraryKind.Static : NativeLibraryKind.Shared), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => TextToolchain.BuildAsync(context, token),
            static (
                relative,
                target
            ) => RuntimePath("text", relative, target)),
        new("ui", UiToolchain.componentDescriptor, new(profile is null ? NativeLibraryKind.Static : NativeLibraryKind.Shared), [],
            profile is null ? null : static (
                context,
                dependencies,
                token
            ) => UiToolchain.BuildAsync(context, token),
            static (
                relative,
                target
            ) => RuntimePath("ui", relative, target))
    ];

    private static string? RuntimePath(
        string component,
        string relative,
        string target
    ) => relative.Split(Path.DirectorySeparatorChar)[0] == "Link"
        ? null : Path.Combine(component, target, relative);
}
