using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Bindings;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;

namespace Inno.Build.Cli;

internal sealed class BuildCommandContext
{
    internal BuildCommandContext(BuildCommandLine options)
    {
        string selectedRoot = options.Read("engine-root", string.Empty);
        root = Path.GetFullPath(string.IsNullOrWhiteSpace(selectedRoot) ? ToolchainEnvironment.FindRepoRoot() : selectedRoot);
        if (!File.Exists(Path.Combine(root, "InnoEngine.sln")))
            throw new DirectoryNotFoundException($"Engine checkout is unavailable at '{root}'.");
        configuration = options.Read("configuration", "Release");
        ToolchainEnvironment.ValidateConfiguration(configuration.ToLowerInvariant());
        configuration = configuration.Equals("debug", StringComparison.OrdinalIgnoreCase) ? "Debug" : "Release";
        target = new BuildTargetId(options.Require("target"));
        toolsTarget = new BuildTargetId(options.Require("tools-target"));
        string dotnet = ToolchainEnvironment.ResolveExecutable(options.Read("dotnet", "dotnet"));
        BuildCompositionContext captured = StandardBuildEnvironment.Capture(AppContext.BaseDirectory, toolsTarget);
        composition = new BuildCompositionContext(dotnet, captured.applicationDirectory, captured.host, toolsTarget,
            new NativeBindingGenerator(dotnet));
        distribution = StandardBuildDistribution.Create(composition).build;
    }

    internal string root { get; }
    internal string configuration { get; }
    internal BuildTargetId target { get; }
    internal BuildTargetId toolsTarget { get; }
    internal BuildCompositionContext composition { get; }
    internal BuildDistribution distribution { get; }

    internal async Task<string> PreflightEditorAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        BuildPlatformContribution? platform = distribution.platforms.SingleOrDefault(value => value.descriptor.id == target);
        string project = platform?.editorProject
            ?? throw new NotSupportedException($"Target '{target}' does not provide an Editor product.");
        var native = new NativeBuildContext(root, configuration.ToLowerInvariant());
        await distribution.ResolveNativeToolchain(target.value).ResolveAsync(native, composition.host,
            target.value, cancellationToken).ConfigureAwait(false);
        return Path.GetFullPath(project, root);
    }

    internal string[] ProductArguments(string project) =>
    [
        project, "--configuration", configuration, "-p:InnoNativeTarget=" + target.value,
        "-p:InnoToolTarget=" + toolsTarget.value, "--disable-build-servers", "-m:1", "-nodeReuse:false"
    ];
}
