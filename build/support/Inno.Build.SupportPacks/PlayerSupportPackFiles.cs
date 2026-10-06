using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;
using Inno.Core.IO;

namespace Inno.Build.SupportPacks;

internal static class PlayerSupportPackFiles
{
    internal static async Task CopyPlayerSourcesAsync(
        PlayerSupportPackBuildContext context,
        string project,
        string destination,
        CancellationToken cancellationToken,
        IReadOnlyDictionary<string, string>? environment = null
    ) {
        string evaluated = await ToolchainEnvironment.CaptureOutputAsync(
            context.dotnetHost,
            ["msbuild", project, "-getItem:Compile", "-p:Configuration=Release",
                "-p:DesignTimeBuild=true", "-nodeReuse:false", "-nologo"],
            context.engineRoot, cancellationToken, environment).ConfigureAwait(false);
        using JsonDocument document = JsonDocument.Parse(evaluated);
        JsonElement sources = document.RootElement.GetProperty("Items").GetProperty("Compile");
        string projectDirectory = Path.GetDirectoryName(project)!;
        var outputs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (JsonElement source in sources.EnumerateArray()
            .OrderBy(static source => source.GetProperty("Identity").GetString(), StringComparer.Ordinal))
        {
            cancellationToken.ThrowIfCancellationRequested();
            string input = PathBoundary.RequireUnlinkedPath(
                context.engineRoot, source.GetProperty("FullPath").GetString()!);
            string relative = source.TryGetProperty("Link", out JsonElement link)
                && !string.IsNullOrWhiteSpace(link.GetString())
                ? link.GetString()! : Path.GetRelativePath(projectDirectory, input);
            string output = PathBoundary.Resolve(destination, relative);
            if (!outputs.Add(relative.Replace('\\', '/')))
                throw new InvalidDataException($"Player sources repeat publication path '{relative}'.");
            Directory.CreateDirectory(Path.GetDirectoryName(output)!);
            File.Copy(input, output);
        }
        if (outputs.Count == 0)
            throw new InvalidDataException("The Player project declares no publication sources.");
    }

    internal static void CopyCompositionInputs(
        string engineRoot,
        string playerDirectory
    ) {
        string analyzerDirectory = Path.Combine(playerDirectory, "Analyzers");
        Directory.CreateDirectory(analyzerDirectory);
        string analyzer = Path.Combine(engineRoot, "src", "runtime", "generators",
            "Inno.Runtime.Generators", "bin", "Release", "netstandard2.0", "Inno.Runtime.Generators.dll");
        if (!File.Exists(analyzer))
            throw new FileNotFoundException("The runtime registration generator is absent from the prepared build.", analyzer);
        File.Copy(analyzer, Path.Combine(analyzerDirectory, Path.GetFileName(analyzer)));
        string serializationAnalyzer = Path.Combine(engineRoot, "src", "foundation", "core",
            "Inno.Core.Serialization.Generators", "bin", "Release", "netstandard2.0", "Inno.Core.Serialization.Generators.dll");
        if (!File.Exists(serializationAnalyzer))
            throw new FileNotFoundException("The serialization metadata generator is absent from the prepared build.", serializationAnalyzer);
        File.Copy(serializationAnalyzer, Path.Combine(analyzerDirectory, Path.GetFileName(serializationAnalyzer)));
        string sdkSelection = Path.Combine(engineRoot, "global.json");
        File.Copy(sdkSelection, Path.Combine(playerDirectory, "global.json"));
    }

    internal static void CopyReferences(
        string buildOutput,
        string destination
    ) {
        string[] files = Directory.Exists(buildOutput)
            ? Directory.EnumerateFiles(buildOutput, "Inno.*.dll")
                .Where(static file => !Path.GetFileName(file).StartsWith("Inno.Plugin.", StringComparison.Ordinal)
                    && Path.GetFileName(file) is not "Inno.GameScripts.dll" and not "Inno.EditorScripts.dll" and not "Inno.Player.Browser.dll" and not "Inno.Player.dll")
                .Order(StringComparer.Ordinal).ToArray()
            : [];
        if (files.Length == 0)
            throw new InvalidDataException("Player build produced no target compilation references.");
        Directory.CreateDirectory(destination);
        foreach (string file in files)
            File.Copy(file, Path.Combine(destination, Path.GetFileName(file)));
    }
}
