using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using System;
using System.IO;
using System.Linq;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx.Tools;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;

namespace Inno.Build.Cli;

internal static class ProjectBuildWorkflow
{
    internal static async Task<int> RunAsync(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        BuildCommand command = BuildCommand.Parse(arguments);
        string application = Path.GetDirectoryName(typeof(BuildApplication).Assembly.Location)!;
        BuildCompositionContext context = StandardBuildEnvironment.Capture(application, command.toolsTarget, command.dotnetHost);
        using ProductNativeBuildResult products = await BuildComposition.PrepareNativeAsync(context, cancellationToken).ConfigureAwait(false);
        NativeBuildProduct product = products.Single();
        string extension = context.host.system == "Windows" ? ".exe" : string.Empty;
        var executables = Enum.GetValues<BgfxTool>().ToDictionary(static tool => tool,
            tool => product.files.Single(file => Path.GetFileName(file) == tool.ToString().ToLowerInvariant() + extension));
        string includes = Path.GetDirectoryName(product.files.Single(file => Path.GetFileName(file) == "bgfx_shader.sh"))!;
        var tools = new ToolRunner(executables, includes);
        using BuildWorkspace workspace = BuildWorkspace.Open(command.projectDirectory, command.supportPackRoot, context, tools);
        if (command.kind == BuildCommandKind.ImportSample)
        {
            Console.WriteLine(workspace.ImportSample(command.sampleSource, cancellationToken));
            return 0;
        }
        if (command.kind == BuildCommandKind.Scripts)
        {
            Console.WriteLine(workspace.ExportProjectScripts(command.outputDirectory));
            return 0;
        }
        var progress = new ConsoleBuildProgress();
        BuildResult result;
        if (command.kind == BuildCommandKind.Game)
        {
            GameBuildRequest request = command.CreateGameRequest(workspace.LoadGameProfile(command.profilePath, command.gameTarget));
            // The command-line host has no UI scheduler. Keep the authoring owner on this thread
            // while preparing external tools, then capture its build snapshot before the first await.
            using PlayerSupportPackLease prepared = workspace.pipeline.EnsurePlayerSupportPackAsync(
                    request.profile.target, request.profile.managedDeployment, cancellationToken)
                .GetAwaiter().GetResult();
            result = await workspace.pipeline.BuildGameAsync(request, progress, cancellationToken);
        }
        else if (command.kind == BuildCommandKind.Plugin)
        {
            result = await workspace.pipeline.BuildPluginAsync(
                command.CreatePluginRequest(workspace.projectId), progress, cancellationToken);
        }
        else
        {
            throw new InvalidOperationException("Unknown project build kind.");
        }
        foreach (BuildDiagnostic diagnostic in result.diagnostics)
            Console.Error.WriteLine($"{diagnostic.severity} {diagnostic.code}: {diagnostic.message}");
        if (!result.succeeded)
            return 1;
        Console.WriteLine(result.outputPath);
        return 0;
    }

    private sealed class ConsoleBuildProgress : IProgress<BuildProgress>
    {
        /// <summary>
        /// Writes a build stage update to the command-line host output.
        /// </summary>
        /// <param name="value">
        /// The current stage and normalized completion value.
        /// </param>
        public void Report(BuildProgress value) => Console.Error.WriteLine($"[{value.fraction:P0}] {value.message}");
    }
}
