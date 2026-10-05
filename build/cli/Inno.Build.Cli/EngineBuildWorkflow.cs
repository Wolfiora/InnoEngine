using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.SupportPacks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;
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
        if (command == "shader")
        {
            if (!Enum.TryParse(options.Require("platform"), out BgfxShaderTargetPlatform platform)
                || !GraphicsApi.TryParse(options.Require("renderer"), out GraphicsApi renderer))
                throw new ArgumentException("The shader platform or rendering API is invalid.");
            ShaderArtifactBuilder.Compile(options.Require("assets"), options.Require("shader"),
                platform, renderer, options.Require("output"));
            return;
        }
        if (command == "clean")
        {
            Clean(root);
            return;
        }
        if (command == "bindings")
        {
            await GenerateBindingsAsync(root, dotnet, configuration, cancellationToken);
            return;
        }
        BuildTargetId target = new(options.Require("target"));
        string output = options.Read("output", Path.Combine(root, "artifacts", "support-packs"));
        if (command == "engine")
        {
            await GenerateBindingsAsync(root, dotnet, configuration, cancellationToken);
            var products = await HostNativeBuild.BuildEditorAsync(
                new NativeBuildContext(root, configuration.ToLowerInvariant()), cancellationToken);
            await ToolchainEnvironment.RunAsync(dotnet,
                ["build", Path.Combine(root, "src/composition/editor/host/Inno.Editor.Application/Inno.Editor.Application.csproj"),
                    "--configuration", configuration, "--disable-build-servers", "-m:1", "-nodeReuse:false"],
                root, cancellationToken);
            string editor = Path.Combine(root, "src", "composition", "editor", "host", "Inno.Editor.Application",
                "bin", configuration, "net9.0");
            await HostNativeDeployment.InstallAsync(products, editor, cancellationToken).ConfigureAwait(false);
        }
        Console.WriteLine(await BuiltInPlayerSupportPacks.CreatePublisher().PublishAsync(root, output, target, dotnet, cancellationToken));
    }

    private static async Task GenerateBindingsAsync(
        string root,
        string dotnet,
        string configuration,
        CancellationToken cancellationToken
    ) {
        foreach (string project in Directory.EnumerateFiles(Path.Combine(root, "native"), "*.csproj", SearchOption.AllDirectories)
            .Where(static path => !path.Split(Path.DirectorySeparatorChar).Any(static part => part is "bin" or "obj")))
        {
            if (!File.Exists(Path.Combine(Path.GetDirectoryName(project)!, "Bindings", "bindgen.json")))
                continue;
            await ToolchainEnvironment.RunAsync(dotnet,
                ["build", project, "-t:GenerateBindings", "-m:1", "-nodeReuse:false", "--configuration", configuration],
                root, cancellationToken);
        }
    }

    private static void Clean(string root)
    {
        foreach (string sourceRoot in new[] { "src", "native", "build", "tools" })
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
