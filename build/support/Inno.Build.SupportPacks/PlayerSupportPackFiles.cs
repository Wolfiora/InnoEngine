using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build;
using Inno.Build.Toolchains;

namespace Inno.Build.SupportPacks;

internal static class PlayerSupportPackFiles
{
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
