using System;
using Inno.Build.Bindings;
using Inno.Build.BrowserWasm32;
using Inno.Build.Composition;
using Inno.Build.Products.Standard;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Integration.BrowserWasm32.Bgfx;

namespace Inno.Integration.BrowserWasm32.Build;

/// <summary>
/// Composes this platform's binding, component and deployment policies.
/// </summary>
public static class BrowserWasm32BuildIntegration
{
    /// <summary>
    /// Creates the complete platform registration without discovering tools or writing outputs.
    /// </summary>
    /// <param name="context">
    /// The explicit operation host.
    /// </param>
    /// <returns>
    /// A platform contribution containing its selected native and binding capabilities.
    /// </returns>
    public static BuildPlatformContribution Create(BuildCompositionContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        var generator = new NativeBindingGenerator(BrowserWasm32BindingProfile.Create());
        ProductNativeBuildPlan player = StandardNativeComponents.CreateStaticPlayer(BrowserWasm32NativeProducts.Options, BrowserWasm32NativeLayout.Map);
        var validator = new PlayerSupportPackClosureValidator(
            StandardSupportPackRequirements.CreateStatic(player, BrowserWasm32BuildModule.target.id, BrowserWasm32LinkerSettings.Create()),
            new BrowserWasm32SupportPackValidator(), null);
        return BrowserWasm32BuildModule.CreateContribution(context,
            new NativeProductContribution(player, new BrowserWasm32NativeProductExecutor()), validator, generator);
    }
}
