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

namespace Inno.Build.Cli;

internal static class EngineBuildWorkflow
{
    internal static async Task RunAsync(
        string command,
        CliOptions options,
        CancellationToken cancellationToken
    ) {
        string selectedRoot = options.Read("engine-root", string.Empty);
        string root = Path.GetFullPath(string.IsNullOrWhiteSpace(selectedRoot) ? ToolchainEnvironment.FindRepoRoot() : selectedRoot);
        if (!File.Exists(Path.Combine(root, "InnoEngine.sln")))
            throw new DirectoryNotFoundException($"Engine checkout is unavailable at '{root}'.");
        string dotnet = options.Read("dotnet", "dotnet");
        string configuration = options.Read("configuration", "Release");
        ToolchainEnvironment.ValidateConfiguration(configuration.ToLowerInvariant());
        configuration = configuration.Equals("debug", StringComparison.OrdinalIgnoreCase) ? "Debug" : "Release";
        if (command == "clean")
        {
            Clean(root);
            return;
        }
        BuildTargetId target = new(options.Require("target"));
        BuildTargetId toolsTarget = new(options.Require("tools-target"));
        BuildCompositionContext captured = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, toolsTarget);
        var context = new BuildCompositionContext(ToolchainEnvironment.ResolveExecutable(dotnet),
            captured.applicationDirectory, captured.host, toolsTarget,
            new NativeBindingGenerator(ToolchainEnvironment.ResolveExecutable(dotnet)));
        BuildDistribution distribution = StandardBuildDistribution.Create(context).build;
        if (command == "shader")
        {
            BgfxShaderTargetProfile platform = StandardBuildDistribution.Create(context).ResolveShaderTarget(target);
            if (!GraphicsApi.TryParse(options.Require("renderer"), out GraphicsApi renderer))
                throw new ArgumentException("The shader platform or rendering API is invalid.");
            NativeBuildContext native = new NativeBuildContext(root, configuration.ToLowerInvariant())
                .WithBindingGenerator(context.bindingGenerator);
            native = native.WithToolchain(await distribution.ResolveNativeToolchain(toolsTarget.value)
                .ResolveAsync(native, context.host, toolsTarget.value, cancellationToken).ConfigureAwait(false));
            NativeBuildProduct product = (await distribution.ResolveNativeProduct(toolsTarget.value, "shader-tools")
                .BuildAsync(native, cancellationToken).ConfigureAwait(false)).Single();
            string extension = native.RequireToolchain().host.system == "Windows" ? ".exe" : string.Empty;
            var executables = Enum.GetValues<BgfxTool>().ToDictionary(static tool => tool,
                tool => product.files.Single(file => Path.GetFileName(file) == tool.ToString().ToLowerInvariant()
                    + "-" + native.configuration + extension));
            ShaderArtifactBuilder.Compile(options.Require("assets"), options.Require("shader"),
                platform, renderer, options.Require("output"), new ToolRunner(executables), cancellationToken);
            return;
        }
        if (command == "bindings")
        {
            string productId = options.Read("product", "editor");
            ProductNativeBuildPlan product = distribution.ResolveNativeProduct(target.value, productId);
            NativeBuildContext bindings = new(root, configuration.ToLowerInvariant());
            bindings = bindings.WithBindingGenerator(context.bindingGenerator);
            bindings = bindings.WithToolchain(await distribution.ResolveNativeToolchain(target.value)
                .ResolveAsync(bindings, context.host, target.value, cancellationToken).ConfigureAwait(false));
            var request = new NativeBindingGenerationRequest(product.steps.Select(static step => step.component)
                .Where(static owner => owner.bindingDefinition is not null).DistinctBy(static owner => owner.nativeProject),
                target.value, NativeBindingOutputMode.TargetArtifacts);
            await context.bindingGenerator.GenerateAsync(bindings, request, cancellationToken).ConfigureAwait(false);
        }
        if (command == "bindings")
            return;
        if (command == "engine")
        {
            BuildPlatformContribution platform = distribution.platforms.Single(contribution => contribution.descriptor.id == target);
            string project = platform.editorProject
                ?? throw new NotSupportedException($"Target '{target}' does not provide an Editor product.");
            await ToolchainEnvironment.RunAsync(context.dotnetHost,
                ["build", Path.GetFullPath(project, root), "--configuration", configuration,
                    "-p:InnoNativeTarget=" + target.value, "-p:InnoToolTarget=" + toolsTarget.value,
                    "--disable-build-servers", "-m:1", "-nodeReuse:false"], root, cancellationToken,
                DotNetSdkEnvironment.Create(context.dotnetHost)).ConfigureAwait(false);
        }
        string output = options.Read("output", Path.Combine(root, "artifacts", "support-packs"));
        Console.WriteLine(await distribution.supportPacks.PublishAsync(
            root, output, target, context.dotnetHost, cancellationToken).ConfigureAwait(false));
    }

    private static void Clean(string root)
    {
        foreach (string sourceRoot in new[] { "src", "backends", "platforms", "build", "tools" })
        {
            string directory = Path.Combine(root, sourceRoot);
            foreach (string project in Directory.EnumerateFiles(directory, "*.csproj", SearchOption.AllDirectories)
                .Where(static path => !path.Split(Path.DirectorySeparatorChar).Any(static part => part is "bin" or "obj")).ToArray())
            {
                string owner = Path.GetDirectoryName(project)!;
                foreach (string name in new[] { "bin", "obj" })
                {
                    string output = Path.GetFullPath(Path.Combine(owner, name));
                    if (!output.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        throw new InvalidOperationException("Refusing to clean an output outside the engine checkout.");
                    if (AppContext.BaseDirectory.StartsWith(output + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                        continue;
                    ToolchainEnvironment.DeleteDirectory(output);
                }
            }
        }
        foreach (string category in new[] { "native", "managed", "support-packs", "builds" })
            ToolchainEnvironment.DeleteDirectory(Path.Combine(root, "artifacts", category));
    }
}
