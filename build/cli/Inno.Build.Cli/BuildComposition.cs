using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Build.Distribution.Standard;
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

    internal static async Task PrepareNativeAsync(
        BuildCompositionContext context,
        CancellationToken cancellationToken
    ) {
        string root = ToolchainEnvironment.FindRepoRoot();
        var nativeContext = new NativeBuildContext(root, C_HOST_CONFIGURATION).WithBindingGenerator(context.bindingGenerator);
        var provider = StandardBuildDistribution.Create(context).build.ResolveNativeToolchain(context.toolsTarget.value);
        nativeContext = nativeContext.WithToolchain(await provider.ResolveAsync(
            nativeContext, context.host, context.toolsTarget.value, cancellationToken).ConfigureAwait(false));
        ProductNativeBuildPlan plan = StandardBuildDistribution.Create(context).build.ResolveNativeProduct(context.toolsTarget.value, "shader-tools");
        var products = await plan.BuildAsync(nativeContext, cancellationToken).ConfigureAwait(false);
        await ProductNativeDeployment.InstallAsync(products, plan, AppContext.BaseDirectory, cancellationToken).ConfigureAwait(false);
    }

    internal static BuildPipeline CreatePipeline(
        BuildCompositionContext context,
        EngineHost engine,
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        ScriptCompiler compiler,
        string supportPackRoot
    ) {
        return BuildPipelineFactory.Create(
            context, StandardBuildDistribution.Create(context).build, engine, assets, plugins, settings, compiler, supportPackRoot);
    }
}
