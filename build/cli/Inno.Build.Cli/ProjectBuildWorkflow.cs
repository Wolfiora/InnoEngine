using System;
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
        await BuildComposition.PrepareNativeAsync(cancellationToken).ConfigureAwait(false);
        using BuildWorkspace workspace = BuildWorkspace.Open(command.projectDirectory, command.supportPackRoot);
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
            GameBuildRequest request = command.CreateGameRequest(workspace.LoadGameProfile(command.profilePath));
            // The command-line host has no UI scheduler. Keep the authoring owner on this thread
            // while preparing external tools, then capture its build snapshot before the first await.
            _ = workspace.pipeline.EnsurePlayerSupportPackAsync(
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
