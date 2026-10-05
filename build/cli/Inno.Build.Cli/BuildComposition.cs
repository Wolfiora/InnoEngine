using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Managed;
using Inno.Build.Managed.DotNet;
using Inno.Build.Platform.Browser;
using Inno.Build.Platform.MacOS;
using Inno.Build.Platform.Windows;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;
using Inno.Core.Settings;
using Inno.Plugins.Authoring;
using Inno.Runtime;
using Inno.Scripting.Compiler;

namespace Inno.Build.Cli;

internal static class BuildComposition
{
#if DEBUG
    private const string C_HOST_CONFIGURATION = "debug";
#else
    private const string C_HOST_CONFIGURATION = "release";
#endif

    internal static async Task PrepareNativeAsync(CancellationToken cancellationToken)
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        var products = await HostNativeBuild.BuildEditorAsync(
            new NativeBuildContext(root, C_HOST_CONFIGURATION), cancellationToken).ConfigureAwait(false);
        await HostNativeDeployment.InstallAsync(products, AppContext.BaseDirectory, cancellationToken).ConfigureAwait(false);
    }

    internal static BuildPipeline CreatePipeline(
        EngineHost engine,
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        ScriptCompiler compiler,
        string supportPackRoot
    ) => new(assets, plugins, settings, engine.serialization, engine.generations, compiler, supportPackRoot,
        [new MacOSArm64GameBuildTarget(assets, engine.serialization, engine.types),
            new WindowsX64GameBuildTarget(assets, engine.serialization, engine.types),
            new BrowserWasmGameBuildTarget(assets, engine.serialization, engine.types)],
        CreateManagedDeployments(), SourcePlayerSupportPackProvisioner.TryCreateForHost(
            AppContext.BaseDirectory, BuiltInPlayerSupportPacks.CreatePublisher()));

    private static ManagedDeploymentCatalog CreateManagedDeployments()
    {
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet";
        return new ManagedDeploymentCatalog([
            new CoreClrDeploymentCompiler(host),
            new MonoWasmDeploymentCompiler(host, aheadOfTime: false),
            new MonoWasmDeploymentCompiler(host, aheadOfTime: true),
            new NativeAotDeploymentCompiler(host)
        ]);
    }
}
