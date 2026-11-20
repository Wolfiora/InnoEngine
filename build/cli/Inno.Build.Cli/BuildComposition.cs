using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Assets.Pipeline;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx.Tools;
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

    internal static async Task<ProductNativeBuildResult> PrepareNativeAsync(
        BuildCompositionContext context,
        string engineRoot,
        CancellationToken cancellationToken,
        string? nativeDependencies = null
    ) {
        var distribution = StandardBuildDistribution.Create(context).build;
        NativeProductContribution contribution = distribution.ResolveNativeProduct(context.toolsTarget.value, "shader-tools");
        if (nativeDependencies is not null)
            return await NativeDependencyManifest.AcquireAsync(engineRoot, nativeDependencies, context.toolsTarget.value,
                C_HOST_CONFIGURATION, contribution.plan, cancellationToken).ConfigureAwait(false);
        var nativeContext = new NativeBuildContext(engineRoot, C_HOST_CONFIGURATION).WithBindingGenerator(StandardBuildDistribution.Create(context).build.ResolveBindingGenerator(context.toolsTarget.value));
        var provider = StandardBuildDistribution.Create(context).build.ResolveNativeToolchain(context.toolsTarget.value);
        nativeContext = nativeContext.WithToolchain(await provider.ResolveAsync(
            nativeContext, context.host, context.toolsTarget.value, cancellationToken).ConfigureAwait(false));
        return await contribution.BuildAsync(nativeContext, cancellationToken).ConfigureAwait(false);
    }

    internal static BuildPipeline CreatePipeline(
        BuildCompositionContext context,
        EngineHost engine,
        AssetPipeline assets,
        PluginEnvironment plugins,
        ProjectSettingsStore settings,
        ScriptCompiler compiler,
        string supportPackRoot,
        ToolRunner tools
    ) {
        return BuildPipelineFactory.Create(
            context, StandardBuildDistribution.Create(context, tools).build, engine, assets, plugins, settings, compiler, supportPackRoot);
    }
}
