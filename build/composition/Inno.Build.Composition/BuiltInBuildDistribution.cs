using System;
using Inno.Build.Managed.DotNet;
using Inno.Build.Platform.Browser;
using Inno.Build.Platform.MacOS;
using Inno.Build.Platform.Windows;
using Inno.Build.SupportPacks;

namespace Inno.Build.Composition;

/// <summary>
/// Defines the single built-in distribution consumed by Editor, CLI, and MSBuild hosts.
/// </summary>
public static class BuiltInBuildDistribution
{
    /// <summary>
    /// Composes the built-in platform, managed deployment, and Support Pack implementations.
    /// </summary>
    /// <param name="context">
    /// Host-owned SDK and source provisioning dependencies.
    /// </param>
    /// <returns>
    /// A frozen, independently composed distribution without running tools or reading project content.
    /// </returns>
    public static BuildDistribution Create(BuildCompositionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return new BuildDistribution(
            [
                static (
                    assets,
                    serialization,
                    types
                ) => new WindowsX64GameBuildTarget(assets, serialization, types),
                static (
                    assets,
                    serialization,
                    types
                ) => new MacOSArm64GameBuildTarget(assets, serialization, types),
                static (
                    assets,
                    serialization,
                    types
                ) => new BrowserWasmGameBuildTarget(assets, serialization, types)
            ],
            [
                new CoreClrDeploymentCompiler(context.dotnetHost),
                new MonoWasmDeploymentCompiler(context.dotnetHost, aheadOfTime: false),
                new MonoWasmDeploymentCompiler(context.dotnetHost, aheadOfTime: true),
                new NativeAotDeploymentCompiler(context.dotnetHost)
            ],
            [
                new DesktopPlayerSupportPackSource(
                    BuildTargetId.windowsX64, "win-x64", "windows-x64", ".dll", new WindowsSupportPackValidator()),
                new DesktopPlayerSupportPackSource(
                    BuildTargetId.macOSArm64, "osx-arm64", "osx-arm64", ".dylib", new MacOSSupportPackValidator()),
                new BrowserPlayerSupportPackSource()
            ]);
    }
}
