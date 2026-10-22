using System;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.MacOSArm64;
using Inno.Build.Products.Standard;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Integration.MacOSArm64.Bgfx;

namespace Inno.Integration.MacOSArm64.Build;

/// <summary>
/// Composes this platform's binding, component and deployment policies.
/// </summary>
public static class MacOSArm64BuildIntegration
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
        var generator = new NativeBindingGenerator(MacOSArm64BindingProfile.Create());
        ProductNativeBuildPlan player = StandardNativeComponents.CreatePlayer(MacOSArm64BgfxIntegration.nativeProfile, MacOSArm64NativeProducts.Options, MacOSArm64NativeLayout.Map);
        var validator = new PlayerSupportPackClosureValidator(
            StandardSupportPackRequirements.CreateDynamic(player, MacOSArm64BuildModule.target.id, ".dylib"),
            new MacOSArm64SupportPackValidator(), StandardNativeDeployment.CreateValidator(MacOSArm64BuildModule.target.id, ".dylib"));
        var executor = new ComponentNativeProductExecutor();
        return MacOSArm64BuildModule.CreateContribution(context, new NativeProductContribution(player, executor),
            new NativeProductContribution(StandardNativeComponents.CreateEditor(MacOSArm64BgfxIntegration.nativeProfile, MacOSArm64NativeProducts.Options, MacOSArm64NativeLayout.Map), executor),
            new NativeProductContribution(StandardNativeComponents.CreateShaderTools(MacOSArm64BgfxIntegration.nativeProfile, MacOSArm64NativeProducts.Options, MacOSArm64NativeLayout.Map), executor), validator, generator);
    }
}
