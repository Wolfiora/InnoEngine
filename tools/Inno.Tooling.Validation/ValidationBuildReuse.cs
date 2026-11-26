using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Tooling.Validation;

internal static class ValidationBuildReuse
{
    internal static async Task CheckAsync(
        ValidationPlan plan,
        ValidationContext context,
        string directory,
        CancellationToken cancellationToken
    ) {
        string entry = Path.GetFullPath("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", plan.root);
        JsonElement editor = context.sdkGraph
            .EnumerateArray()
            .Single(item => Path.GetFullPath(item.GetProperty("Path").GetString()!) == entry);
        if (!context.properties.ContainsKey("InnoNativeTarget")
            || !File.Exists(editor.GetProperty("Properties").GetProperty("TargetPath").GetString())
        ) {
            throw new InvalidDataException("Hot validation requires the previously built Editor target.");
        }
        Directory.CreateDirectory(directory);
        SortedDictionary<string, (DateTime timestamp, string digest)> before = Capture(plan.root);
        string log = Path.Combine(directory, "hot-build.log");
        await ValidationProcess.RunAsync(
            plan.dotnet,
            [
                "build",
                entry,
                ..context.properties.Select(pair => "-p:" + pair.Key + "=" + pair.Value),
                "--disable-build-servers",
                "-m:1",
                "-nr:false",
                "-bl:" + Path.Combine(directory, "hot-build.binlog")
            ],
            plan.root,
            log,
            cancellationToken: cancellationToken
        ).ConfigureAwait(false);
        string output = await File.ReadAllTextAsync(log, cancellationToken).ConfigureAwait(false);
        bool consume = !string.IsNullOrWhiteSpace(context.properties.GetValueOrDefault("InnoNativeDependencyManifest"));
        if (consume && output.Contains("INNO-NATIVE-PREPARE", StringComparison.Ordinal))
        {
            throw new InvalidDataException("Consume-only build invoked a native producer.");
        }
        string operation = consume ? "CONSUME" : "PREPARE";
        MatchCollection counters = Regex.Matches(
            output,
            $@"INNO-NATIVE-{operation}[^\r\n]*processes=(\d+)[^\r\n]*bindingGenerations=(\d+)"
        );
        if (counters.Count == 0 || counters.Any(match => match.Groups[1].Value != "0" || match.Groups[2].Value != "0"))
        {
            throw new InvalidDataException("Hot build has missing counters or repeated native compilation or binding generation.");
        }
        MatchCollection requests = Regex.Matches(output, @"INNO-NATIVE-PREPARE request=([a-f0-9]{64})[^\r\n]*reusedPreparation=(\d+)");
        if ((!consume && requests.Count != counters.Count) || requests.GroupBy(match => match.Groups[1].Value)
            .Any(group => group.Count(match => match.Groups[2].Value == "0") > 1)
        ) {
            throw new InvalidDataException("Missing or repeated frozen native request.");
        }
        if (!before.SequenceEqual(Capture(plan.root)))
        {
            throw new InvalidDataException("Hot build changed a stable native selection.");
        }
    }

    private static SortedDictionary<string, (DateTime timestamp, string digest)> Capture(string root)
    {
        var selections = new SortedDictionary<string, (DateTime, string)>(StringComparer.Ordinal);
        foreach (string relative in new[] { "artifacts/native", "artifacts/bindings" })
        {
            string directory = Path.Combine(root, relative);
            if (!Directory.Exists(directory))
            {
                continue;
            }
            foreach (string file in Directory.EnumerateFiles(directory, "*.current", SearchOption.AllDirectories))
            {
                using FileStream input = File.OpenRead(file);
                selections.Add(file, (File.GetLastWriteTimeUtc(file), Convert.ToHexStringLower(SHA256.HashData(input))));
            }
        }
        return selections;
    }
}
