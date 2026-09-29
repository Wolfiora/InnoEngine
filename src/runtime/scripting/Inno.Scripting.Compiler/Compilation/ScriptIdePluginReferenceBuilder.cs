using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

using Inno.Extensibility.Modules;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Scripting.Compiler;

internal static class ScriptIdePluginReferenceBuilder
{
    internal static IReadOnlyDictionary<string, string> Build(
        ScriptCompilerOptions options,
        ScriptSourceSet sources,
        ScriptApiReferenceSet runtimeApi,
        ScriptApiReferenceSet editorApi
    ) {
        var results = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        string root = Path.Combine(options.ideDirectory, "PluginReferences");
        foreach (ScriptAssemblyInput assembly in sources.assemblies)
        {
            if (assembly.domain != AssemblyDomain.InnoPlugin)
                continue;

            ScriptApiReferenceSet api = assembly.scope == ScriptAssemblyScope.Editor
                ? editorApi
                : runtimeApi;
            string[] dependencies = assembly.references
                .Select(name => results.TryGetValue(name, out string? path)
                    ? path
                    : throw new InvalidDataException(
                        $"IDE Plugin dependency '{name}' for '{assembly.name}' is unavailable."))
                .ToArray();
            string fingerprint = CreateFingerprint(assembly, api, dependencies);
            string directory = Path.Combine(root, fingerprint);
            string outputPath = Path.Combine(directory, assembly.name + ".dll");
            if (!File.Exists(outputPath))
            {
                Directory.CreateDirectory(directory);
                Emit(assembly, api, dependencies, outputPath);
            }
            results.Add(assembly.name, outputPath);
        }
        return results;
    }

    private static string CreateFingerprint(
        ScriptAssemblyInput assembly,
        ScriptApiReferenceSet api,
        IReadOnlyList<string> dependencies
    ) {
        string input = string.Join('\n', new[]
        {
            assembly.name,
            assembly.definitionHash,
            api.contractFingerprint,
            assembly.scope.ToString(),
            assembly.nullable.ToString(),
            assembly.allowUnsafe.ToString()
        }.Concat(assembly.defines)
            .Concat(assembly.sources.Select(static source => source.contentHash))
            .Concat(dependencies));
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(input)));
    }

    private static void Emit(
        ScriptAssemblyInput assembly,
        ScriptApiReferenceSet api,
        IReadOnlyList<string> dependencies,
        string outputPath
    ) {
        string[] symbols = assembly.defines
            .Concat(assembly.scope == ScriptAssemblyScope.Editor ? ["INNO_EDITOR"] : [])
            .Concat(["DEBUG", "TRACE"])
            .Distinct(StringComparer.Ordinal)
            .ToArray();
        var parseOptions = new CSharpParseOptions(
            LanguageVersion.Latest,
            DocumentationMode.Parse,
            SourceCodeKind.Regular,
            preprocessorSymbols: symbols);
        SyntaxTree[] trees = assembly.sources.Select(source =>
            CSharpSyntaxTree.ParseText(
                SourceText.From(File.ReadAllText(source.snapshotPath), Encoding.UTF8),
                parseOptions,
                source.sourcePath)).ToArray();
        IEnumerable<MetadataReference> references = FrameworkReferenceResolver
            .CreateReferencePackReferences()
            .Concat(api.ideReferencePaths.Select(static path => MetadataReference.CreateFromFile(path)))
            .Concat(dependencies.Select(static path => MetadataReference.CreateFromFile(path)));
        CSharpCompilation compilation = CSharpCompilation.Create(
            assembly.name,
            trees,
            references,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Debug,
                allowUnsafe: assembly.allowUnsafe,
                deterministic: true,
                nullableContextOptions: assembly.nullable
                    ? NullableContextOptions.Enable
                    : NullableContextOptions.Disable));
        string temporaryPath = outputPath + ".tmp-" + Guid.NewGuid().ToString("N");
        try
        {
            using (FileStream stream = File.Create(temporaryPath))
            {
                EmitResult result = compilation.Emit(
                    stream,
                    options: new EmitOptions(metadataOnly: true, includePrivateMembers: false));
                if (!result.Success)
                {
                    string errors = string.Join(Environment.NewLine,
                        result.Diagnostics
                            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                            .Select(static diagnostic => diagnostic.ToString()));
                    throw new InvalidDataException(
                        $"Failed to generate IDE Plugin reference '{assembly.name}':{Environment.NewLine}{errors}");
                }
            }
            try
            {
                File.Move(temporaryPath, outputPath);
            }
            catch (IOException) when (File.Exists(outputPath))
            {
                // Another projection already published the same immutable reference.
            }
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }
}
