using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.Cli;

internal sealed record ProductCleanPlan(
    string root,
    string receipt,
    string[] directories
) {
    internal static async Task<ProductCleanPlan> CreateAsync(
        BuildCommandContext context,
        string project,
        CancellationToken cancellationToken
    ) {
        string result = await ToolchainEnvironment.CaptureOutputAsync(context.composition.dotnetHost,
            ["msbuild", project, "-nologo", "-getProperty:TargetDir,IntermediateOutputPath",
                "-p:Configuration=" + context.configuration, "-p:InnoNativeTarget=" + context.target.value],
            context.root, cancellationToken, DotNetSdkEnvironment.Create(context.composition.dotnetHost)).ConfigureAwait(false);
        using JsonDocument document = JsonDocument.Parse(result);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        string owner = Path.GetDirectoryName(project)!;
        string[] directories =
        [
            Resolve(owner, "bin", properties.GetProperty("TargetDir").GetString()!),
            Resolve(owner, "obj", properties.GetProperty("IntermediateOutputPath").GetString()!)
        ];
        string receipt = Path.Combine(context.root, "artifacts", "products", "editor", context.target.value,
            context.configuration, "ready.json");
        PathBoundary.RequireUnlinkedPath(context.root, receipt);
        return new ProductCleanPlan(context.root, receipt, directories);
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
