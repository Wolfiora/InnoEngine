using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Build.Composition;
using Inno.Build.Managed.DotNet;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Build.Windows;
using Inno.Build.MacOS;
using Inno.Build.Browser;
using Inno.Build.Linux;
using Inno.Integration.Windows.Bgfx;
using Inno.Integration.MacOS.Bgfx;
using Inno.Integration.Browser.Bgfx;
using Inno.Integration.Linux.Bgfx;

namespace Inno.Build.Distribution.Standard;

/// <summary>
/// Binds platform publication and backend configuration once at the standard distribution boundary.
/// </summary>
public sealed class StandardBuildDistribution
{
    private readonly IReadOnlyDictionary<BuildTargetId, BgfxShaderTargetProfile> m_shaders;

    private StandardBuildDistribution(
        BuildDistribution build,
        IReadOnlyDictionary<BuildTargetId, BgfxShaderTargetProfile> shaders
    ) {
        this.build = build;
        m_shaders = shaders;
    }

    /// <summary>
    /// Gets the neutral distribution borrowed by product hosts and shared pipeline mechanisms.
    /// </summary>
    public BuildDistribution build { get; }

    /// <summary>
    /// Composes one explicit binding per implemented platform without resolving tools.
    /// </summary>
    /// <param name="context">
    /// The explicitly selected SDK and execution host.
    /// </param>
    /// <returns>
    /// A frozen standard distribution with backend configuration bound to the same platform identities.
    /// </returns>
    public static StandardBuildDistribution Create(BuildCompositionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        (BuildPlatformContribution platform, BgfxShaderTargetProfile shaders, GameContentCompilerFactory compiler)[] bindings =
        [
            (WindowsBuildModule.CreateContribution(context,
                StandardNativeBuildPlans.CreatePlayer(WindowsBgfxIntegration.nativeProfile),
                StandardNativeBuildPlans.CreateEditor(WindowsBgfxIntegration.nativeProfile),
                StandardNativeBuildPlans.CreateShaderTools(WindowsBgfxIntegration.nativeProfile)), WindowsBgfxIntegration.shaderProfile, WindowsBgfxIntegration.contentCompilerFactory),
            (MacOSBuildModule.CreateContribution(context,
                StandardNativeBuildPlans.CreatePlayer(MacOSBgfxIntegration.nativeProfile),
                StandardNativeBuildPlans.CreateEditor(MacOSBgfxIntegration.nativeProfile),
                StandardNativeBuildPlans.CreateShaderTools(MacOSBgfxIntegration.nativeProfile)), MacOSBgfxIntegration.shaderProfile, MacOSBgfxIntegration.contentCompilerFactory),
            (BrowserBuildModule.CreateContribution(context, StandardNativeBuildPlans.CreateStaticPlayer(BrowserBgfxIntegration.nativeOptions)), BrowserBgfxIntegration.shaderProfile, BrowserBgfxIntegration.contentCompilerFactory)
        ];
        foreach ((BuildPlatformContribution platform, BgfxShaderTargetProfile shaders, GameContentCompilerFactory _) in bindings)
            if (platform.descriptor.id.value != shaders.id)
                throw new InvalidOperationException("Platform publication and shader target identities disagree.");
        var build = new BuildDistribution(bindings.Select(static binding => new GameBuildContribution(binding.platform, binding.compiler)).ToArray(),
            [new CoreClrDeploymentCompiler(context.dotnetHost),
                new MonoWasmDeploymentCompiler(context.dotnetHost, aheadOfTime: false),
                new MonoWasmDeploymentCompiler(context.dotnetHost, aheadOfTime: true),
                new NativeAotDeploymentCompiler(context.dotnetHost)],
            LinuxBuildModule.CreateNativeContributions(context).Select(static contribution => new NativeToolchainContribution(
                contribution.descriptor, contribution.provider,
                [StandardNativeBuildPlans.CreateShaderTools(LinuxBgfxIntegration.CreateNativeProfile(contribution.descriptor.id.value))])).ToArray());
        return new(build, new ReadOnlyDictionary<BuildTargetId, BgfxShaderTargetProfile>(
            bindings.ToDictionary(static binding => binding.platform.descriptor.id, static binding => binding.shaders)));
    }

    /// <summary>
    /// Resolves explicitly registered shader configuration independently of tool execution target.
    /// </summary>
    /// <param name="target">
    /// The artifact's actual publication target.
    /// </param>
    /// <returns>
    /// The borrowed immutable configuration; unavailable targets fail before native preparation.
    /// </returns>
    /// <exception cref="NotSupportedException">
    /// The distribution has no shader configuration for this target.
    /// </exception>
    public BgfxShaderTargetProfile ResolveShaderTarget(BuildTargetId target)
        => m_shaders.TryGetValue(target, out BgfxShaderTargetProfile? profile) ? profile
            : throw new NotSupportedException($"No shader configuration is registered for '{target}'.");
}
