using System;
using Inno.Build.Composition;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOS;

/// <summary>
/// Owns the implemented MacOS target facts and complete publication contribution.
/// </summary>
public static class MacOSBuildModule
{
    /// <summary>
    /// Gets the explicit implemented product target, independent of the tool execution host.
    /// </summary>
    public static PlatformTargetDescriptor target { get; } = new(
        BuildTargetId.macOSArm64, "MacOS", "arm64", "darwin-arm64", "osx-arm64", true);

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
    /// <returns>
    /// A complete immutable contribution without starting tools or creating files.
    /// </returns>
    public static BuildPlatformContribution CreateContribution(
        BuildCompositionContext context,
        ProductNativeBuildPlan nativePlan,
        ProductNativeBuildPlan editorNativePlan,
        ProductNativeBuildPlan shaderToolsPlan
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentNullException.ThrowIfNull(editorNativePlan);
        return new(target,
            () => new MacOSArm64GameBuildTarget(),
            new MacOSPlayerSupportPackSource(nativePlan, context.host, context.dotnetHost, context.bindingGenerator), new MacOSNativeToolchainProvider(context.dotnetHost),
            "platforms/MacOS/editor/Inno.Editor.MacOS/Inno.Editor.MacOS.csproj",
            [nativePlan, editorNativePlan, shaderToolsPlan]);
    }
}
