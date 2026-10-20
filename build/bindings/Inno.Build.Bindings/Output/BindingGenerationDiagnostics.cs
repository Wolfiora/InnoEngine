using System;
using System.IO;
using System.Linq;
using System.Threading;
using BGCS;
using BGCS.Contracts.Artifacts;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingGenerationDiagnostics
{
    internal static void Generate(
        NativeBuildContext context,
        BindingGenerationConfiguration definition,
        GenerationRequest request,
        string candidate,
        string nativeOutput,
        string managedOutput,
        CancellationToken cancellation
    ) {
        GenerationRequest selected = request.WithConfiguration(request.configuration with
        {
            output = request.configuration.output with { root = candidate }
        });
        context.RecordBindingGeneration();
        BindingHostExecution.Generate(selected, context.engineRoot, nativeOutput, managedOutput, cancellation);
    }

    internal static void RequireEqual(
        string candidate,
        string current,
        bool normalizeNewlines
    ) {
        string[] files = Directory.EnumerateFiles(candidate, "*", SearchOption.AllDirectories).ToArray();
        if (!Directory.Exists(current) || Directory.EnumerateFiles(current, "*", SearchOption.AllDirectories).Count() != files.Length)
            throw new InvalidDataException($"Generated output is missing or has a different file inventory: {current}");
        foreach (string file in files)
        {
            string other = Path.Combine(current, Path.GetRelativePath(candidate, file));
            bool equal = File.Exists(other) && (normalizeNewlines
                ? File.ReadAllText(file).Replace("\r\n", "\n", StringComparison.Ordinal)
                    == File.ReadAllText(other).Replace("\r\n", "\n", StringComparison.Ordinal)
                : File.ReadAllBytes(file).AsSpan().SequenceEqual(File.ReadAllBytes(other)));
            if (!equal)
                throw new InvalidDataException($"Generated output is out of date: {other}");
        }
    }
}
