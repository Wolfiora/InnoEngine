using System.Threading;
using System.Threading.Tasks;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.WindowsX64;

/// <summary>
/// Prepares the Windows Player's explicit layout through shared publication mechanisms.
/// </summary>
public sealed class WindowsX64PlayerSupportPackSource : IPlayerSupportPackSource
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
    /// <param name="supportPackValidator">
    /// The borrowed validator selected by the distribution.
    /// </param>
    public WindowsX64PlayerSupportPackSource(
        ProductNativeBuildPlan nativePlan,
        BuildHostDescriptor host,
        string dotnetHost,
        INativeBindingGenerator bindingGenerator,
        IPlayerSupportPackValidator supportPackValidator
    ) {
        m_preparation = new FilePlayerSupportPackPreparation(
            BuildTargetId.windowsX64, "win-x64", BuildTargetId.windowsX64.value, ".dll",
            supportPackValidator,
            "platforms/WindowsX64/player/Inno.Player.WindowsX64/Inno.Player.WindowsX64.csproj",
            "platforms/WindowsX64/build/Inno.Build.WindowsX64/Templates/WindowsX64Player.project.xml",
            nativePlan, new WindowsX64NativeToolchainProvider(dotnetHost), host, bindingGenerator,
            ["platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64PlayerSupportPackSource.cs",
                "platforms/WindowsX64/build/Inno.Build.WindowsX64/SupportPacks/WindowsX64SupportPackValidator.cs"]);
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
