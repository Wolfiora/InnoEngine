using System.Threading;
using System.Threading.Tasks;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.Windows;

/// <summary>
/// Prepares the Windows Player's explicit layout through shared publication mechanisms.
/// </summary>
public sealed class WindowsPlayerSupportPackSource : IPlayerSupportPackSource
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
    /// <param name="dotnetHost">
    /// The explicit SDK executable for binding generation.
    /// </param>
    public WindowsPlayerSupportPackSource(
        ProductNativeBuildPlan nativePlan,
        BuildHostDescriptor host,
        string dotnetHost
    ) {
        m_preparation = new FilePlayerSupportPackPreparation(
            BuildTargetId.windowsX64, "win-x64", BuildTargetId.windowsX64.value, ".dll",
            new WindowsSupportPackValidator(),
            "platforms/Windows/player/Inno.Player.Windows/Inno.Player.Windows.csproj",
            "platforms/Windows/build/Inno.Build.Windows/Templates/WindowsPlayer.project.xml",
            nativePlan, new WindowsNativeToolchainProvider(dotnetHost), host);
    }

    /// <inheritdoc />
    public BuildTargetId target => BuildTargetId.windowsX64;

    /// <inheritdoc />
    public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    ) => m_preparation.CreatePlanAsync(context, cancellationToken);

    /// <inheritdoc />
    public void Validate(string directory) => m_preparation.Validate(directory);
}
