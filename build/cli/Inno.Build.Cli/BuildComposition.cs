using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Composition;
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
    ) {
        var context = new BuildCompositionContext(
            Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? "dotnet", AppContext.BaseDirectory);
        return BuildPipelineFactory.Create(
            context, BuiltInBuildDistribution.Create(context), engine, assets, plugins, settings, compiler, supportPackRoot);
    }
}
