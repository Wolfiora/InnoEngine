using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Composition;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;

namespace Inno.Build.MacOSArm64;

/// <summary>
/// Prepares the MacOS Player's explicit layout through shared publication mechanisms.
/// </summary>
public sealed class MacOSArm64PlayerSupportPackSource : IPlayerSupportPackSource
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
    public MacOSArm64PlayerSupportPackSource(
        NativeProductContribution nativePlan,
        BuildHostDescriptor host,
        string dotnetHost,
        INativeBindingGenerator bindingGenerator,
        IPlayerSupportPackValidator supportPackValidator
    ) {
        m_preparation = new FilePlayerSupportPackPreparation(
            Inno.Build.MacOSArm64.MacOSArm64BuildModule.target.id, "osx-arm64", Inno.Build.MacOSArm64.MacOSArm64BuildModule.target.id.value, ".dylib",
            supportPackValidator,
            "platforms/MacOSArm64/player/Inno.Player.MacOSArm64/Inno.Player.MacOSArm64.csproj",
            "platforms/MacOSArm64/build/Inno.Build.MacOSArm64/Templates/MacOSArm64Player.project.xml",
            nativePlan.plan, nativePlan.executor, new MacOSArm64NativeToolchainProvider(dotnetHost), host, bindingGenerator,
            ["platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64PlayerSupportPackSource.cs",
                "platforms/MacOSArm64/build/Inno.Build.MacOSArm64/SupportPacks/MacOSArm64SupportPackValidator.cs"]);
    }

    /// <inheritdoc />
    public BuildTargetId target => Inno.Build.MacOSArm64.MacOSArm64BuildModule.target.id;

    /// <inheritdoc />
    public ValueTask<PlayerSupportPackPlan> CreatePlanAsync(
        PlayerSupportPackPlanningContext context,
        CancellationToken cancellationToken
    ) => m_preparation.CreatePlanAsync(context, cancellationToken);

    /// <inheritdoc />
    public void Validate(string directory) => m_preparation.Validate(directory);
}
