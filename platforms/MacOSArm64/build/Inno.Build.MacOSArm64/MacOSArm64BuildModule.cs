using System;
using Inno.Build.Composition;
using Inno.Build.Managed;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOSArm64;

/// <summary>
/// Owns the implemented MacOS target facts and complete publication contribution.
/// </summary>
public static class MacOSArm64BuildModule
{
    /// <summary>
    /// Gets the explicit implemented product target, independent of the tool execution host.
    /// </summary>
    public static PlatformTargetDescriptor target { get; } = new(
        new BuildTargetId("macos-arm64"), "MacOS", "arm64", "darwin-arm64", "osx-arm64", true);

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
    /// <param name="bindingGenerator">
    /// The generator configured by this platform integration.
    /// </param>
    /// <returns>
    /// A complete immutable contribution without starting tools or creating files.
    /// </returns>
    public static BuildPlatformContribution CreateContribution(
        BuildCompositionContext context,
        NativeProductContribution nativePlan,
        NativeProductContribution editorNativePlan,
        NativeProductContribution shaderToolsPlan,
        IPlayerSupportPackValidator supportPackValidator,
        INativeBindingGenerator bindingGenerator
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nativePlan);
        ArgumentNullException.ThrowIfNull(editorNativePlan);
        return new(target,
            () => new MacOSArm64GameBuildTarget(supportPackValidator),
            new MacOSArm64PlayerSupportPackSource(nativePlan, context.host, context.dotnetHost, bindingGenerator, supportPackValidator), new MacOSArm64NativeToolchainProvider(context.dotnetHost),
            "platforms/MacOSArm64/editor/Inno.Editor.MacOSArm64/Inno.Editor.MacOSArm64.csproj",
            [nativePlan, editorNativePlan, shaderToolsPlan], bindingGenerator,
            [ManagedDeploymentId.coreClr, ManagedDeploymentId.nativeAot]);
    }
}
