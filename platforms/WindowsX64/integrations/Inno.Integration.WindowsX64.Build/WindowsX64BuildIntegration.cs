using System;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Products.Standard;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Build.WindowsX64;
using Inno.Integration.WindowsX64.Bgfx;

namespace Inno.Integration.WindowsX64.Build;

/// <summary>
/// Composes this platform's binding, component and deployment policies.
/// </summary>
public static class WindowsX64BuildIntegration
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
        var generator = new NativeBindingGenerator(WindowsX64BindingProfile.Create());
        ProductNativeBuildPlan player = StandardNativeComponents.CreatePlayer(WindowsX64BgfxIntegration.nativeProfile, WindowsX64NativeProducts.Options, WindowsX64NativeLayout.Map);
        var validator = new PlayerSupportPackClosureValidator(
            StandardSupportPackRequirements.CreateDynamic(player, WindowsX64BuildModule.target.id, ".dll"),
            new WindowsX64SupportPackValidator(), StandardNativeDeployment.CreateValidator(WindowsX64BuildModule.target.id, ".dll"));
        var executor = new ComponentNativeProductExecutor();
        return WindowsX64BuildModule.CreateContribution(context, new NativeProductContribution(player, executor),
            new NativeProductContribution(StandardNativeComponents.CreateEditor(WindowsX64BgfxIntegration.nativeProfile, WindowsX64NativeProducts.Options, WindowsX64NativeLayout.Map), executor),
            new NativeProductContribution(StandardNativeComponents.CreateShaderTools(WindowsX64BgfxIntegration.nativeProfile, WindowsX64NativeProducts.Options, WindowsX64NativeLayout.Map), executor), validator, generator);
    }
}
