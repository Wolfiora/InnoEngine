using System.Threading;
using System.Threading.Tasks;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOS;

/// <summary>
/// Prepares the MacOS Player's explicit layout through shared publication mechanisms.
/// </summary>
public sealed class MacOSPlayerSupportPackSource : IPlayerSupportPackSource
{
    private readonly FilePlayerSupportPackPreparation m_preparation;

    /// <summary>
    /// Captures the selected product closure and tool execution host.
    /// </summary>
    /// <param name="nativePlan">
    /// The explicit Player component closure.
    /// </param>
    /// <param name="host">
    /// The declared machine executing platform tools.
    /// </param>
    /// <param name="bindingGenerator">
    /// The borrowed shared binding implementation selected by distribution.
    /// </param>
    /// <param name="dotnetHost">
    /// The explicit SDK executable for binding generation.
    /// </param>
    public MacOSPlayerSupportPackSource(
        ProductNativeBuildPlan nativePlan,
        BuildHostDescriptor host,
        string dotnetHost,
        INativeBindingGenerator bindingGenerator
    ) {
        m_preparation = new FilePlayerSupportPackPreparation(
            BuildTargetId.macOSArm64, "osx-arm64", BuildTargetId.macOSArm64.value, ".dylib",
            new MacOSSupportPackValidator(),
            "platforms/MacOS/player/Inno.Player.MacOS/Inno.Player.MacOS.csproj",
            "platforms/MacOS/build/Inno.Build.MacOS/Templates/MacOSPlayer.project.xml",
            nativePlan, new MacOSNativeToolchainProvider(dotnetHost), host, bindingGenerator);
    }

    /// <inheritdoc />
    public BuildTargetId target => BuildTargetId.macOSArm64;

    /// <inheritdoc />
    public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    ) => m_preparation.CreatePlanAsync(context, cancellationToken);

    /// <inheritdoc />
    public void Validate(string directory) => m_preparation.Validate(directory);
}
