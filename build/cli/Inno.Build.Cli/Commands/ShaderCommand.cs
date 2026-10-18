using Inno.Build.Bindings;
using System.Collections.Generic;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains.Bgfx;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx.Shaders;
using Inno.Build.Toolchains.Bgfx.Tools;
using Inno.Rendering;
using Inno.Core.IO;

namespace Inno.Build.Cli;

internal static class ShaderCommand
{
    internal static async Task RunAsync(
        BuildCommandContext request,
        BuildCommandLine options,
        CancellationToken cancellationToken
    ) {
        string root = request.root;
        string configuration = request.configuration;
        BuildTargetId target = request.target;
        BuildTargetId toolsTarget = request.toolsTarget;
        BuildCompositionContext context = request.composition;
        BuildDistribution distribution = request.distribution;

        BgfxShaderTargetProfile platform = StandardBuildDistribution.Create(context).ResolveShaderTarget(target);
        if (!GraphicsApi.TryParse(options.Require("renderer"), out GraphicsApi renderer))
            throw new ArgumentException("The shader platform or rendering API is invalid.");
        NativeBuildContext native = new NativeBuildContext(root, configuration.ToLowerInvariant())
            .WithBindingGenerator(context.bindingGenerator);
        native = native.WithToolchain(await distribution.ResolveNativeToolchain(toolsTarget.value)
            .ResolveAsync(native, context.host, toolsTarget.value, cancellationToken).ConfigureAwait(false));
        using ProductNativeBuildResult products = await distribution.ResolveNativeProduct(toolsTarget.value, "shader-tools")
            .BuildAsync(native, cancellationToken).ConfigureAwait(false);
        NativeBuildProduct product = products.Single();
        string extension = native.RequireToolchain().host.system == "Windows" ? ".exe" : string.Empty;
        var executables = Enum.GetValues<BgfxTool>().ToDictionary(static tool => tool,
            tool => product.files.Single(file => Path.GetFileName(file) == tool.ToString().ToLowerInvariant()
                + extension));
        string includes = Path.GetDirectoryName(product.files.Single(file => Path.GetFileName(file) == "bgfx_shader.sh"))!;
        ShaderArtifactBuilder.Compile(options.Require("assets"), options.Require("shader"),
            platform, renderer, options.Require("output"), new ToolRunner(executables, includes), cancellationToken);
        return;
        
    }
}
