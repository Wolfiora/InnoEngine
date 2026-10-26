using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Bgfx.Tools;

namespace Inno.Build.Cli;

internal static class ProjectBuildWorkflow
{
    internal static async Task<int> RunAsync(
        string[] arguments,
        CancellationToken cancellationToken
    ) {
        BuildCommand command = BuildCommand.Parse(arguments);
        string engineRoot = BuildCommandContext.ResolveEngineRoot(command.engineRoot);
        int exitCode = await ExecuteAsync(command, engineRoot, cancellationToken).ConfigureAwait(false);
        if (exitCode == 0)
        {
            try
            {
                await BuildMaintenanceApplication.CleanAsync(engineRoot, new BuildCachePolicy(),
                    false, false, null, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception failure)
            {
                Console.Error.WriteLine($"Project operation succeeded; automatic cache maintenance was deferred: {failure.Message}");
            }
        }
        return exitCode;
    }

    private static async Task<int> ExecuteAsync(
        BuildCommand command,
        string engineRoot,
        CancellationToken cancellationToken
    ) {
        string application = Path.GetDirectoryName(typeof(BuildApplication).Assembly.Location)!;
        BuildCompositionContext context = StandardBuildEnvironment.Capture(application, command.toolsTarget, command.dotnetHost);
        using ProductNativeBuildResult products = await BuildComposition.PrepareNativeAsync(context, engineRoot, cancellationToken).ConfigureAwait(false);
        NativeBuildProduct product = products.Single();
        string extension = context.host.services.executableExtension;
        var executables = Enum.GetValues<BgfxTool>().ToDictionary(static tool => tool,
            tool => product.files.Single(file => Path.GetFileName(file) == tool.ToString().ToLowerInvariant() + extension));
        string includes = Path.GetDirectoryName(product.files.Single(file => Path.GetFileName(file) == "bgfx_shader.sh"))!;
        var tools = new ToolRunner(executables, includes, context.host.services.workspaces);
        using BuildWorkspace workspace = BuildWorkspace.Open(command.projectDirectory, command.supportPackRoot, context, tools);
        if (command.kind == BuildCommandKind.ImportSample)
        {
            Console.WriteLine(workspace.ImportSample(command.sampleSource, cancellationToken));
            return 0;
        }
        if (command.kind == BuildCommandKind.Scripts)
        {
            Console.WriteLine(workspace.ExportProjectScripts(command.outputDirectory, command.generateIdeProjects));
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
