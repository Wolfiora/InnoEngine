using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Inno.Build.Composition;
using Inno.Build.Managed.DotNet;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;
using Inno.Build.MacOSArm64;
using Inno.Build.BrowserWasm32;
using Inno.Integration.WindowsX64.Bgfx;
using Inno.Integration.MacOSArm64.Bgfx;
using Inno.Integration.BrowserWasm32.Bgfx;

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
    /// <param name="tools">
    /// Borrowed frozen content-compilation tools, or null to consume this application's explicit deployment.
    /// </param>
    /// <returns>
    /// A frozen standard distribution with backend configuration bound to the same platform identities.
    /// </returns>
    public static StandardBuildDistribution Create(
        BuildCompositionContext context,
        ToolRunner? tools = null
    ) {
        ArgumentNullException.ThrowIfNull(context);
        tools ??= new ToolRunner();
        ProductNativeBuildPlan windowsPlayer = StandardNativeBuildPlans.CreatePlayer(WindowsX64BgfxIntegration.nativeProfile);
        ProductNativeBuildPlan macPlayer = StandardNativeBuildPlans.CreatePlayer(MacOSArm64BgfxIntegration.nativeProfile);
        ProductNativeBuildPlan browserPlayer = StandardNativeBuildPlans.CreateStaticPlayer(BrowserWasm32BgfxIntegration.nativeOptions);
        var windowsValidator = new PlayerSupportPackClosureValidator(
            StandardSupportPackRequirements.CreateDynamic(windowsPlayer, BuildTargetId.windowsX64, ".dll"),
            new WindowsX64SupportPackValidator(), StandardNativeDeployment.CreateValidator(BuildTargetId.windowsX64, ".dll"));
        var macValidator = new PlayerSupportPackClosureValidator(
            StandardSupportPackRequirements.CreateDynamic(macPlayer, BuildTargetId.macOSArm64, ".dylib"),
            new MacOSArm64SupportPackValidator(), StandardNativeDeployment.CreateValidator(BuildTargetId.macOSArm64, ".dylib"));
        var browserValidator = new PlayerSupportPackClosureValidator(
            StandardSupportPackRequirements.CreateStatic(browserPlayer, BuildTargetId.browserWasm),
            new BrowserWasm32SupportPackValidator(), null);
        (BuildPlatformContribution platform, BgfxShaderTargetProfile shaders, GameContentCompilerFactory compiler)[] bindings =
        [
            (WindowsX64BuildModule.CreateContribution(context,
                windowsPlayer,
                StandardNativeBuildPlans.CreateEditor(WindowsX64BgfxIntegration.nativeProfile),
                StandardNativeBuildPlans.CreateShaderTools(WindowsX64BgfxIntegration.nativeProfile), windowsValidator), WindowsX64BgfxIntegration.shaderProfile, WindowsX64BgfxIntegration.CreateContentCompilerFactory(tools)),
            (MacOSArm64BuildModule.CreateContribution(context,
                macPlayer,
                StandardNativeBuildPlans.CreateEditor(MacOSArm64BgfxIntegration.nativeProfile),
                StandardNativeBuildPlans.CreateShaderTools(MacOSArm64BgfxIntegration.nativeProfile), macValidator), MacOSArm64BgfxIntegration.shaderProfile, MacOSArm64BgfxIntegration.CreateContentCompilerFactory(tools)),
            (BrowserWasm32BuildModule.CreateContribution(context, browserPlayer, browserValidator), BrowserWasm32BgfxIntegration.shaderProfile, BrowserWasm32BgfxIntegration.CreateContentCompilerFactory(tools))
        ];
        foreach ((BuildPlatformContribution platform, BgfxShaderTargetProfile shaders, GameContentCompilerFactory _) in bindings)
            if (platform.descriptor.id.value != shaders.id)
                throw new InvalidOperationException("Platform publication and shader target identities disagree.");
        var build = new BuildDistribution(bindings.Select(static binding => new GameBuildContribution(binding.platform, binding.compiler)).ToArray(),
            [new CoreClrDeploymentCompiler(context.dotnetHost),
                new MonoWasmDeploymentCompiler(context.dotnetHost, aheadOfTime: false),
                new MonoWasmDeploymentCompiler(context.dotnetHost, aheadOfTime: true),
                new NativeAotDeploymentCompiler(context.dotnetHost)]);
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
