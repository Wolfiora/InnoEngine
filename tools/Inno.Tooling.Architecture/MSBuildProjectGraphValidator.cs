using System;
using Inno.Build.Toolchains;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Tooling.Architecture;

internal static class MSBuildProjectGraphValidator
{
    private static readonly HashSet<string> ExecutableProducts = new(StringComparer.Ordinal)
    {
        "InnoEngine.Editor", "InnoEngine.Build"
    };

    internal static async Task<IReadOnlyDictionary<string, string>> ValidateAsync(
        string root,
        string configuration,
        string dotnetHost,
        string? artifactsPath,
        string? outputPath,
        ICollection<string> failures,
        CancellationToken cancellationToken
    ) {
        if (!Path.IsPathFullyQualified(dotnetHost) || !File.Exists(dotnetHost))
            throw new ArgumentException("Architecture SDK evaluation requires an existing absolute dotnet executable.", nameof(dotnetHost));
        string[] projects = new[] { "entry", "src", "backends", "platforms", "build", "tools", "tests" }
            .SelectMany(owner => RepositorySourceInventory.Files(Path.Combine(root, owner), "*.csproj"))
            .Order(StringComparer.Ordinal).ToArray();
        var evaluated = new List<EvaluatedProject>();
        foreach (string project in projects)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string relative = Relative(root, project);
            try
            {
                EvaluatedProject snapshot = await EvaluateAsync(root, project, configuration,
                    dotnetHost, artifactsPath, cancellationToken).ConfigureAwait(false);
                evaluated.Add(snapshot);
                ValidateProject(root, snapshot, failures);
            }
            catch (Exception failure) when (failure is InvalidDataException or JsonException)
            {
                failures.Add($"{relative}: effective MSBuild evaluation failed: {failure.Message}");
            }
        }
        ValidateGraph(evaluated, failures);
        if (outputPath is not null)
        {
            string destination = Path.GetFullPath(outputPath);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            await File.WriteAllTextAsync(destination, JsonSerializer.Serialize(evaluated,
                new JsonSerializerOptions { WriteIndented = true }), cancellationToken).ConfigureAwait(false);
        }
        Console.WriteLine($"Evaluated MSBuild ownership for {evaluated.Count}/{projects.Length} projects without executing build targets.");
        return evaluated.ToDictionary(project => Path.GetFullPath(project.path, root),
            static project => project.targetPath, StringComparer.OrdinalIgnoreCase);
    }

    private static async Task<EvaluatedProject> EvaluateAsync(
        string root,
        string project,
        string configuration,
        string dotnetHost,
        string? artifactsPath,
        CancellationToken cancellationToken
    ) {
        var start = new ProcessStartInfo(dotnetHost)
        {
            WorkingDirectory = root,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        foreach ((string name, string? value) in DotNetSdkEnvironment.Create(dotnetHost))
        {
            if (value is null)
                start.Environment.Remove(name);
            else
                start.Environment[name] = value;
        }
        foreach (string argument in new[]
        {
            "msbuild", project, "-nologo", "-getItem:ProjectReference,Compile,Using",
            "-getProperty:AssemblyName,OutputType,InnoProductId,InnoProductTarget,InnoNativeTarget,ImplicitUsings,TargetPath",
            "-p:DesignTimeBuild=true", "-p:BuildProjectReferences=false", "-p:InnoProductRestore=false",
            "-p:Configuration=" + configuration
        })
            start.ArgumentList.Add(argument);
        if (artifactsPath is not null)
            start.ArgumentList.Add("-p:ArtifactsPath=" + artifactsPath);
        using var process = new Process { StartInfo = start };
        if (!process.Start())
            throw new InvalidDataException("The selected SDK did not start.");
        using var bounded = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        bounded.CancelAfter(TimeSpan.FromSeconds(60));
        Task<string> standardOutput = process.StandardOutput.ReadToEndAsync();
        Task<string> standardError = process.StandardError.ReadToEndAsync();
        try
        {
            await process.WaitForExitAsync(bounded.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync(CancellationToken.None).ConfigureAwait(false);
            await Task.WhenAll(standardOutput, standardError).ConfigureAwait(false);
            cancellationToken.ThrowIfCancellationRequested();
            throw new InvalidDataException("SDK project evaluation exceeded its one-minute process budget.");
        }
        string output = await standardOutput.ConfigureAwait(false);
        string errors = await standardError.ConfigureAwait(false);
        if (process.ExitCode != 0)
            throw new InvalidDataException($"SDK exit code {process.ExitCode}: {errors}{output}");
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement properties = document.RootElement.GetProperty("Properties");
        JsonElement items = document.RootElement.GetProperty("Items");
        EvaluatedReference[] references = items.GetProperty("ProjectReference").EnumerateArray()
            .Select(item => new EvaluatedReference(Relative(root, item.GetProperty("FullPath").GetString()!),
                !string.Equals(Value(item, "ReferenceOutputAssembly"), "false", StringComparison.OrdinalIgnoreCase),
                Value(item, "OutputItemType"))).ToArray();
        EvaluatedSource[] sources = items.GetProperty("Compile").EnumerateArray()
            .Select(item => new EvaluatedSource(Relative(root, item.GetProperty("FullPath").GetString()!),
                Value(item, "Link"))).ToArray();
        return new(Relative(root, project), Value(properties, "AssemblyName"), Value(properties, "OutputType"),
            Value(properties, "InnoProductId"), Value(properties, "InnoProductTarget"),
            Value(properties, "InnoNativeTarget"), Value(properties, "ImplicitUsings"),
            items.GetProperty("Using").GetArrayLength(), references, sources, Value(properties, "TargetPath"));
    }

    private static void ValidateProject(
        string root,
        EvaluatedProject project,
        ICollection<string> failures
    ) {
        bool test = project.path.StartsWith("tests/", StringComparison.Ordinal)
            || project.path.Contains("/tests/", StringComparison.Ordinal);
        if (project.implicitUsings.Equals("enable", StringComparison.OrdinalIgnoreCase)
            || project.implicitUsings.Equals("true", StringComparison.OrdinalIgnoreCase)
            || project.usingItems != 0)
            failures.Add($"{project.path}: effective compilation-wide using imports are forbidden.");
        if (!test && project.outputType == "Exe" && !ExecutableProducts.Contains(project.assemblyName))
            failures.Add($"{project.path}: effective production executable is not an approved product.");
        if (project.path.StartsWith("platforms/", StringComparison.Ordinal) && project.outputType == "Exe"
            && (project.productId.Length == 0 || project.target.Length == 0 || project.nativeTarget != project.target))
            failures.Add($"{project.path}: effective product target and native ABI must be explicitly assigned and equal.");
        foreach (EvaluatedReference reference in project.references)
        {
            if (!File.Exists(Path.GetFullPath(reference.path, root)))
                failures.Add($"{project.path}: effective project dependency is missing: {reference.path}.");
            if (!reference.runtime || reference.outputItemType == "Analyzer")
                continue;
            PlatformOwnershipValidator.ValidateDependency(project.path, reference.path, failures);
            if (project.assemblyName == "Inno.Rendering"
                && !reference.path.StartsWith("src/foundation/", StringComparison.Ordinal))
                failures.Add($"{project.path}: effective Rendering Core dependencies must belong to Foundation: {reference.path}.");
            if (project.path.StartsWith("src/foundation/", StringComparison.Ordinal)
                && !reference.path.StartsWith("src/foundation/", StringComparison.Ordinal))
                failures.Add($"{project.path}: effective Foundation dependency belongs to an upper layer: {reference.path}.");
        }
        foreach (EvaluatedSource source in project.sources)
        {
            bool generated = source.path.Contains("/Generated/", StringComparison.Ordinal)
                || source.path.Contains("/obj/", StringComparison.Ordinal);
            string owner = Path.GetDirectoryName(project.path)!.Replace('\\', '/') + "/";
            if (!generated && (source.link.Length != 0 || !source.path.StartsWith(owner, StringComparison.Ordinal)))
                failures.Add($"{project.path}: effective hand-authored Compile source has a second owner: {source.path}.");
        }
    }

    private static void ValidateGraph(
        IReadOnlyList<EvaluatedProject> projects,
        ICollection<string> failures
    ) {
        var graph = projects.ToDictionary(static project => project.path, StringComparer.OrdinalIgnoreCase);
        // Reload fixtures intentionally compile distinct generations with the same assembly identity.
        foreach (IGrouping<string, EvaluatedProject> identity in projects
            .Where(static project => !project.path.StartsWith("tests/", StringComparison.Ordinal)
                && !project.path.Contains("/tests/", StringComparison.Ordinal))
            .GroupBy(static project => project.assemblyName, StringComparer.Ordinal))
            if (identity.Count() != 1)
                failures.Add($"Effective assembly identity '{identity.Key}' has multiple project owners.");
        var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        var active = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        void Visit(EvaluatedProject project)
        {
            if (active.Contains(project.path))
            {
                failures.Add($"{project.path}: effective runtime project references contain a cycle.");
                return;
            }
            if (!visited.Add(project.path))
                return;
            active.Add(project.path);
            foreach (EvaluatedReference reference in project.references.Where(static reference => reference.runtime && reference.outputItemType != "Analyzer"))
                if (graph.TryGetValue(reference.path, out EvaluatedProject? dependency))
                    Visit(dependency);
            active.Remove(project.path);
        }
        foreach (EvaluatedProject project in projects)
            Visit(project);
        foreach (EvaluatedProject player in projects.Where(static project => project.assemblyName.StartsWith("Inno.Player.", StringComparison.Ordinal)))
        {
            var pending = new Stack<EvaluatedProject>();
            visited.Clear();
            pending.Push(player);
            while (pending.TryPop(out EvaluatedProject? current))
            {
                if (!visited.Add(current.path))
                    continue;
                if (ArchitectureRules.IsForbiddenPlayerDependency(current.assemblyName))
                    failures.Add($"{player.path}: effective Player closure includes forbidden authoring/build dependency {current.path}.");
                foreach (EvaluatedReference reference in current.references.Where(static reference => reference.runtime && reference.outputItemType != "Analyzer"))
                    if (graph.TryGetValue(reference.path, out EvaluatedProject? dependency))
                        pending.Push(dependency);
            }
        }
    }

    private static string Value(
        JsonElement element,
        string property
    ) => element.TryGetProperty(property, out JsonElement value) ? value.GetString() ?? string.Empty : string.Empty;

    private static string Relative(
        string root,
        string path
    ) => Path.GetRelativePath(root, path).Replace('\\', '/');

    private sealed record EvaluatedProject(
        string path,
        string assemblyName,
        string outputType,
        string productId,
        string target,
        string nativeTarget,
        string implicitUsings,
        int usingItems,
        IReadOnlyList<EvaluatedReference> references,
        IReadOnlyList<EvaluatedSource> sources,
        string targetPath
    );

    private sealed record EvaluatedReference(
        string path,
        bool runtime,
        string outputItemType
    );

    private sealed record EvaluatedSource(
        string path,
        string link
    );
}
