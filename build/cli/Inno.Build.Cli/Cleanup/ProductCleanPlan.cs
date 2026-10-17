using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Cli;

internal sealed record ProductCleanPlan(
    string root,
    string[] directories
) {
    internal static async Task<ProductCleanPlan> CreateAsync(
        BuildCommandContext context,
        string project,
        CancellationToken cancellationToken
    ) {
        var projects = new List<string> { project };
        foreach (var platform in context.distribution.platforms)
            if (platform.descriptor.id == context.target && platform.editorProject is not null)
                projects.Add(Path.GetFullPath(platform.editorProject, context.root));
        var directories = new List<string>();
        foreach (string selected in projects)
        {
            string result = await ToolchainEnvironment.CaptureOutputAsync(
                context.composition.dotnetHost,
                ["msbuild", selected, "-nologo", "-getProperty:TargetDir,IntermediateOutputPath",
                    "-p:Configuration=" + context.configuration],
                context.root,
                cancellationToken,
                DotNetSdkEnvironment.Create(context.composition.dotnetHost)
            ).ConfigureAwait(false);
            using JsonDocument document = JsonDocument.Parse(result);
            JsonElement properties = document.RootElement.GetProperty("Properties");
            string owner = Path.GetDirectoryName(selected)!;
            directories.Add(Resolve(owner, "bin", properties.GetProperty("TargetDir").GetString()!));
            directories.Add(Resolve(owner, "obj", properties.GetProperty("IntermediateOutputPath").GetString()!));
        }
        return new ProductCleanPlan(context.root, directories.ToArray());
    }

    private static string Resolve(
        string owner,
        string category,
        string output
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(output);
        string root = Path.Combine(owner, category);
        string path = Path.GetFullPath(output, owner).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
        string relative = Path.GetRelativePath(root, path);
        if (Path.IsPathRooted(relative) || relative == "." || relative == ".."
            || relative.StartsWith(".." + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new InvalidOperationException($"The selected product output is outside its '{category}' owner: '{path}'.");
        return PathBoundary.RequireUnlinkedPath(root, path);
    }
}
