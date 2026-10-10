using System;
using Inno.Build.Composition;
using Inno.Build.Toolchains;

namespace Inno.Build.Browser;

/// <summary>
/// Owns the implemented Browser target facts and complete publication contribution.
/// </summary>
public static class BrowserBuildModule
{
    /// <summary>
    /// Gets the explicit implemented product target, independent of the tool execution host.
    /// </summary>
    public static PlatformTargetDescriptor target { get; } = new(
        BuildTargetId.browserWasm, "Browser", "wasm32", "emscripten-wasm32", "browser-wasm", false);

    /// <summary>
    /// Contributes the platform's target, product inputs and SDK resolver as one registration.
    /// </summary>
    /// <param name="context">
    /// The borrowed SDK and execution-host selection.
    /// </param>
    /// <param name="nativePlan">
    /// The selected shared Player component closure.
    /// </param>
    /// <returns>
    /// A complete immutable contribution without starting tools or creating files.
    /// </returns>
    public static BuildPlatformContribution CreateContribution(
        BuildCompositionContext context,
        ProductNativeBuildPlan nativePlan
    ) {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(nativePlan);
        return new(target,
            () => new BrowserWasm32GameBuildTarget(
                new BrowserSupportPackValidator(nativePlan)),
            new BrowserPlayerSupportPackSource(context.host, context.toolsTarget, nativePlan, context.bindingGenerator),
            new EmscriptenNativeToolchainProvider(context.dotnetHost));
    }
}
