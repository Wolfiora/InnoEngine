using System;
using Inno.Build.Composition;
using Inno.Build.Managed;
using Inno.Build.Toolchains;

namespace Inno.Build.BrowserWasm32;

/// <summary>
/// Owns the implemented Browser target facts and complete publication contribution.
/// </summary>
public static class BrowserWasm32BuildModule
{
    /// <summary>
    /// Gets the explicit implemented product target, independent of the tool execution host.
    /// </summary>
    public static PlatformTargetDescriptor target { get; } = new(
        new BuildTargetId("browser-wasm"), "Browser", "wasm32", "emscripten-wasm32", "browser-wasm", false);

    /// <summary>
    /// Contributes the platform's target, product inputs and SDK resolver as one registration.
    /// </summary>
    /// <param name="context">
    /// The borrowed SDK and execution-host selection.
    /// </param>
    /// <param name="nativePlan">
    /// The selected shared Player component closure.
    /// </param>
    /// <param name="supportPackValidator">
    /// The borrowed platform and selected static-closure validator.
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
        IPlayerSupportPackValidator supportPackValidator,
        INativeBindingGenerator bindingGenerator
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nativePlan);
        return new(target,
            () => new BrowserWasm32GameBuildTarget(supportPackValidator),
            new BrowserWasm32PlayerSupportPackSource(context.host, context.toolsTarget, nativePlan, bindingGenerator, supportPackValidator),
            new EmscriptenNativeToolchainProvider(context.dotnetHost),
            nativeProducts: [nativePlan], bindingGenerator: bindingGenerator,
            managedDeployments: [ManagedDeploymentId.monoWasm, ManagedDeploymentId.monoWasmAot]);
    }
}
