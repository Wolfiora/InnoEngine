using System;
using System.IO;
using System.Linq;
using System.Threading;
using BGCS.Configuration;
using BGCS.Cpp2C.Configuration;
using BGCS.Cpp2C.Facade;
using BGCS.Facade;
using BGCS.Intermediate;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingGenerationDiagnostics
{
    internal static void Generate(
        NativeBuildContext context,
        BindingGenerationConfiguration definition,
        (CsCodeGeneratorConfig managed, Cpp2CGeneratorConfig? bridge, string? hostBridgeRoot) loaded,
        string? nativeOutput,
        string managedOutput,
        CancellationToken cancellation
    ) {
        if (loaded.bridge is not null)
        {
            loaded.bridge.enableIncrementalCache = false;
            context.RecordBindingGeneration();
            var generator = new Cpp2CCodeGenerator(loaded.bridge);
            generator.GenerateConfigured(nativeOutput);
            RequireSuccess(generator.lastResult);
            RewriteInputs(loaded.managed, NativeBindingGenerationIdentity.Resolve(loaded.bridge.outputPath,
                loaded.bridge.configDirectory!), loaded.hostBridgeRoot, nativeOutput!, Path.GetDirectoryName(definition.configPath)!);
        }
        cancellation.ThrowIfCancellationRequested();
        loaded.managed.enableIncrementalCache = false;
        context.RecordBindingGeneration();
        var managed = new CsCodeGenerator(loaded.managed);
        managed.GenerateConfigured(managedOutput);
        RequireSuccess(managed.lastResult);
        cancellation.ThrowIfCancellationRequested();
        if (!File.Exists(Path.Combine(managedOutput, "Bindings.cs"))
            || Directory.EnumerateFiles(managedOutput, "*.cs", SearchOption.AllDirectories).Count() != 1)
            throw new InvalidDataException("The component must produce exactly one managed Bindings.cs source.");
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

    private static void RequireSuccess<T>(BindingGenerationResult<T>? result) where T : class
    {
        if (result is null || !result.success)
            throw new InvalidDataException("Binding generation failed: " + string.Join(Environment.NewLine,
                result?.diagnostics.Select(static diagnostic => diagnostic.code + ": " + diagnostic.message) ?? []));
        foreach (BindingDiagnostic diagnostic in result.diagnostics)
            if (diagnostic.severity == BindingDiagnosticSeverity.Warning)
                Console.WriteLine($"BGCS {diagnostic.code}: {diagnostic.message}");
    }

    private static void RewriteInputs(
        CsCodeGeneratorConfig managed,
        string declaredRoot,
        string? hostRoot,
        string candidate,
        string configDirectory
    ) {
        managed.entryFiles = managed.entryFiles.Select(Rewrite).ToList();
        managed.allowedHeaders = managed.allowedHeaders.Select(Rewrite).ToList();
        managed.includeFolders = managed.includeFolders.Select(Rewrite).ToList();
        string Rewrite(string path)
        {
            string full = NativeBindingGenerationIdentity.Resolve(path, configDirectory);
            string? root = NativeBindingGenerationIdentity.IsWithin(full, declaredRoot) ? declaredRoot
                : hostRoot is not null && NativeBindingGenerationIdentity.IsWithin(full, hostRoot) ? hostRoot : null;
            return root is null ? path : Path.Combine(candidate, Path.GetRelativePath(root, full));
        }
    }
}
