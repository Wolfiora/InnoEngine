using System;
using Inno.Build.Composition;
using Inno.Build.Toolchains;

namespace Inno.Build.WindowsX64;

/// <summary>
/// Owns the implemented Windows target facts and complete publication contribution.
/// </summary>
public static class WindowsX64BuildModule
{
    /// <summary>
    /// Gets the explicit implemented product target, independent of the tool execution host.
    /// </summary>
    public static PlatformTargetDescriptor target { get; } = new(
        BuildTargetId.windowsX64, "Windows", "x64", "msvc-x64", "win-x64", true);

    /// <summary>
    /// Contributes the platform's target, product inputs and SDK resolver as one registration.
    /// </summary>
    /// <param name="context">
    /// The borrowed SDK and execution-host selection.
    /// </param>
    /// <param name="nativePlan">
    /// The selected shared Player component closure.
    /// </param>
    /// <param name="editorNativePlan">
    /// The explicitly selected Editor component closure for ordinary product builds.
    /// </param>
    /// <param name="shaderToolsPlan">
    /// The host-executed offline compiler product for this explicit target.
    /// </param>
    /// <param name="supportPackValidator">
    /// The complete borrowed platform and selected component validator.
    /// </param>
    /// <returns>
    /// A complete immutable contribution without starting tools or creating files.
    /// </returns>
    public static BuildPlatformContribution CreateContribution(
        BuildCompositionContext context,
        ProductNativeBuildPlan nativePlan,
        ProductNativeBuildPlan editorNativePlan,
        ProductNativeBuildPlan shaderToolsPlan,
        IPlayerSupportPackValidator supportPackValidator
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentNullException.ThrowIfNull(editorNativePlan);
        return new(target,
            () => new WindowsX64GameBuildTarget(supportPackValidator),
            new WindowsX64PlayerSupportPackSource(nativePlan, context.host, context.dotnetHost, context.bindingGenerator, supportPackValidator), new WindowsX64NativeToolchainProvider(context.dotnetHost),
            "platforms/WindowsX64/editor/Inno.Editor.WindowsX64/Inno.Editor.WindowsX64.csproj",
            [nativePlan, editorNativePlan, shaderToolsPlan]);
    }
}
