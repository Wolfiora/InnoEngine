using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using Microsoft.CodeAnalysis.Text;

namespace Inno.Scripting.Compiler;

internal sealed record ScriptApiReferenceSet(
    IReadOnlyList<string> runtimeReferencePaths,
    IReadOnlyList<string> ideReferencePaths,
    string contractFingerprint,
    string cacheDirectory
);

internal static class ScriptApiReferenceBuilder
{
    private static readonly SemaphoreSlim S_BUILD_GATE = new(1, 1);

    internal static ScriptApiReferenceSet Build(
        ScriptCompilerOptions options,
        ScriptApiProfile profile,
        ScriptApiProfile? baseProfile = null,
        ScriptApiReferenceSet? baseReferences = null,
        CancellationToken cancellationToken = default
    ) {
        S_BUILD_GATE.Wait(cancellationToken);
        try
        {
            return BuildLocked(options, profile, baseProfile, baseReferences, cancellationToken);
        }
        finally
        {
            S_BUILD_GATE.Release();
        }
    }

    private static ScriptApiReferenceSet BuildLocked(
        ScriptCompilerOptions options,
        ScriptApiProfile profile,
        ScriptApiProfile? baseProfile,
        ScriptApiReferenceSet? baseReferences,
        CancellationToken cancellationToken
    ) {
        cancellationToken.ThrowIfCancellationRequested();
        string fingerprint = CreateFingerprint(profile);
        string directory = Path.Combine(options.scriptApiDirectory, profile.name, fingerprint);
        Directory.CreateDirectory(directory);
        string[] implementationPaths = GetImplementationPaths(profile);
        ScriptApiTypeExport[] allExports = profile.exports
            .SelectMany(static export => export.exports)
            .ToArray();
        HashSet<Type> exportedTypes = allExports
            .Select(static export => export.type)
            .ToHashSet();
        var runtimeReferencePaths = new List<string>(profile.exports.Count);
        string runtimeDirectory = Path.Combine(directory, "Runtime");
        Directory.CreateDirectory(runtimeDirectory);
        foreach (ScriptApiAssembly export in profile.exports)
        {
            cancellationToken.ThrowIfCancellationRequested();
            string assemblyName = export.assembly.GetName().Name
                ?? throw new InvalidOperationException("A script API assembly has no simple name.");
            string referencePath = Path.Combine(runtimeDirectory, assemblyName + ".dll");
            if (!File.Exists(referencePath))
                EmitImplementationReferenceAssembly(
                    export, referencePath, implementationPaths, exportedTypes, cancellationToken);
            runtimeReferencePaths.Add(referencePath);
        }

        HashSet<Type> baseTypes = baseProfile?.exports
            .SelectMany(static export => export.exports)
            .Select(static export => export.type)
            .ToHashSet() ?? [];
        ScriptApiTypeExport[] logicalExports = allExports
            .Where(export => !baseTypes.Contains(export.type))
            .OrderBy(static export => export.type.FullName, StringComparer.Ordinal)
            .ToArray();
        var ideReferencePaths = new List<string>();
        if (baseReferences is not null)
            ideReferencePaths.AddRange(baseReferences.ideReferencePaths);
        if (logicalExports.Length > 0)
        {
            string logicalDirectory = Path.Combine(directory, "IDE");
            Directory.CreateDirectory(logicalDirectory);
            string assemblyName = "Inno.ScriptApi." + profile.name;
            string referencePath = Path.Combine(logicalDirectory, assemblyName + ".dll");
            if (!File.Exists(referencePath))
            {
                EmitLogicalReferenceAssembly(
                    assemblyName,
                    referencePath,
                    logicalExports,
                    exportedTypes,
                    profile.namespaceMappings,
                    baseReferences?.ideReferencePaths ?? [],
                    cancellationToken);
            }
            string documentationPath = Path.ChangeExtension(referencePath, ".xml");
            if (!File.Exists(documentationPath))
            {
                WriteLogicalDocumentation(
                    referencePath,
                    assemblyName,
                    logicalExports,
                    profile.namespaceMappings,
                    profile.typeMappings);
            }
            ideReferencePaths.Add(referencePath);
        }
        ScriptApiArtifactCache.Collect(
            options.scriptApiDirectory,
            new[] { directory, baseReferences?.cacheDirectory });
        return new ScriptApiReferenceSet(
            runtimeReferencePaths,
            ideReferencePaths,
            fingerprint,
            directory);
    }

    private static void EmitImplementationReferenceAssembly(
        ScriptApiAssembly export,
        string referencePath,
        IReadOnlyList<string> implementationPaths,
        IReadOnlySet<Type> exportedTypes,
        CancellationToken cancellationToken
    ) {
        string assemblyName = export.assembly.GetName().Name!;
        string source = ScriptApiStubSourceBuilder.BuildImplementation(export, exportedTypes);
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
            SourceText.From(source, Encoding.UTF8),
            new CSharpParseOptions(LanguageVersion.Latest),
            $"<{assemblyName}.ScriptApi.g.cs>",
            cancellationToken: cancellationToken);
        var references = new Dictionary<string, MetadataReference>(StringComparer.OrdinalIgnoreCase);
        foreach (MetadataReference reference in FrameworkReferenceResolver.CreateReferencePackReferences())
        {
            if (!string.IsNullOrWhiteSpace(reference.Display))
                references[reference.Display!] = reference;
        }
        foreach (string implementationPath in implementationPaths)
        {
            if (string.Equals(
                    Path.GetFileNameWithoutExtension(implementationPath),
                    assemblyName,
                    StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            references[implementationPath] = MetadataReference.CreateFromFile(implementationPath);
        }

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            [syntaxTree],
            references.Values,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release,
                allowUnsafe: true,
                deterministic: true,
                concurrentBuild: false,
                nullableContextOptions: NullableContextOptions.Enable,
                metadataImportOptions: MetadataImportOptions.Public));
        EmitReferenceAssembly(
            compilation, referencePath, source,
            $"Failed to build script API reference assembly '{assemblyName}'.", cancellationToken);
    }

    private static void EmitLogicalReferenceAssembly(
        string assemblyName,
        string referencePath,
        IReadOnlyList<ScriptApiTypeExport> exports,
        IReadOnlySet<Type> exportedTypes,
        IReadOnlyList<ScriptApiNamespaceMapping> namespaceMappings,
        IReadOnlyList<string> baseReferencePaths,
        CancellationToken cancellationToken
    ) {
        Dictionary<string, string> mappings = namespaceMappings
            .GroupBy(static mapping => mapping.implementationNamespace, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.First().apiNamespace,
                StringComparer.Ordinal);
        string source = ScriptApiStubSourceBuilder.BuildLogical(exports, exportedTypes, mappings);
        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(
            SourceText.From(source, Encoding.UTF8),
            new CSharpParseOptions(LanguageVersion.Latest),
            $"<{assemblyName}.g.cs>",
            cancellationToken: cancellationToken);
        var references = new Dictionary<string, MetadataReference>(StringComparer.OrdinalIgnoreCase);
        foreach (MetadataReference reference in FrameworkReferenceResolver.CreateReferencePackReferences())
        {
            if (!string.IsNullOrWhiteSpace(reference.Display))
                references[reference.Display!] = reference;
        }
        foreach (string baseReferencePath in baseReferencePaths)
            references[baseReferencePath] = MetadataReference.CreateFromFile(baseReferencePath);

        CSharpCompilation compilation = CSharpCompilation.Create(
            assemblyName,
            [syntaxTree],
            references.Values,
            new CSharpCompilationOptions(
                OutputKind.DynamicallyLinkedLibrary,
                optimizationLevel: OptimizationLevel.Release,
                allowUnsafe: true,
                deterministic: true,
                concurrentBuild: false,
                nullableContextOptions: NullableContextOptions.Enable,
                metadataImportOptions: MetadataImportOptions.Public));
        EmitReferenceAssembly(
            compilation, referencePath, source,
            $"Failed to build logical script API assembly '{assemblyName}'.", cancellationToken);
    }

    private static void EmitReferenceAssembly(
        CSharpCompilation compilation,
        string referencePath,
        string source,
        string failureMessage,
        CancellationToken cancellationToken
    ) {
        string temporaryPath = referencePath + ".tmp-" + Guid.NewGuid().ToString("N");
        try
        {
            using (FileStream stream = File.Create(temporaryPath))
            {
                EmitResult result = compilation.Emit(
                    peStream: stream,
                    options: new EmitOptions(metadataOnly: true, includePrivateMembers: false),
                    cancellationToken: cancellationToken);
                if (!result.Success)
                {
                    string errors = string.Join(
                        Environment.NewLine,
                        result.Diagnostics
                            .Where(static diagnostic => diagnostic.Severity == DiagnosticSeverity.Error)
                            .Select(static diagnostic => diagnostic.ToString()));
                    throw new InvalidOperationException(
                        failureMessage + $"{Environment.NewLine}{errors}{Environment.NewLine}{source}");
                }
            }
            cancellationToken.ThrowIfCancellationRequested();
            InstallReference(temporaryPath, referencePath);
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private static void InstallReference(
        string temporaryPath,
        string referencePath
    ) {
        try
        {
            File.Move(temporaryPath, referencePath);
        }
        catch (IOException) when (File.Exists(referencePath))
        {
            // Another compiler already published the same immutable reference.
        }
        finally
        {
            if (File.Exists(temporaryPath))
                File.Delete(temporaryPath);
        }
    }

    private static void WriteLogicalDocumentation(
        string referencePath,
        string assemblyName,
        IReadOnlyList<ScriptApiTypeExport> exports,
        IReadOnlyList<ScriptApiNamespaceMapping> namespaceMappings,
        IReadOnlyList<ScriptApiTypeMapping> typeMappings
    ) {
        Dictionary<string, string> mappings = namespaceMappings
            .GroupBy(static mapping => mapping.implementationNamespace, StringComparer.Ordinal)
            .ToDictionary(
                static group => group.Key,
                static group => group.First().apiNamespace,
                StringComparer.Ordinal);
        ScriptApiDocumentationBuilder.Write(
            Path.ChangeExtension(referencePath, ".xml"),
            assemblyName,
            exports,
            mappings,
            typeMappings);
    }

    private static string CreateFingerprint(ScriptApiProfile profile)
    {
        ScriptApiTypeExport[] allExports = profile.exports
            .SelectMany(static export => export.exports)
            .ToArray();
        HashSet<Type> exportedTypes = allExports
            .Select(static export => export.type)
            .ToHashSet();
        var builder = new StringBuilder("Inno.ScriptApi.PublicContract")
            .Append('|')
            .Append(profile.name);
        foreach (ScriptApiAssembly export in profile.exports
                     .OrderBy(static value => value.assembly.GetName().Name, StringComparer.Ordinal))
        {
            builder.Append("|implementation:")
                .Append(export.assembly.GetName().Name)
                .Append('|')
                .Append(ScriptApiStubSourceBuilder.BuildImplementation(export, exportedTypes));
        }
        foreach (string apiNamespace in profile.apiNamespaces.OrderBy(static value => value, StringComparer.Ordinal))
            builder.Append('|').Append(apiNamespace);
        foreach (ScriptApiNamespaceMapping mapping in profile.namespaceMappings
                     .OrderBy(static value => value.apiNamespace, StringComparer.Ordinal)
                     .ThenBy(static value => value.implementationNamespace, StringComparer.Ordinal))
        {
            builder.Append('|')
                .Append(mapping.apiNamespace)
                .Append('>')
                .Append(mapping.implementationNamespace);
        }
        foreach (ScriptApiTypeMapping mapping in profile.typeMappings
                     .OrderBy(static value => value.apiNamespace, StringComparer.Ordinal)
                     .ThenBy(static value => value.apiName, StringComparer.Ordinal)
                     .ThenBy(static value => value.implementationNamespace, StringComparer.Ordinal)
                     .ThenBy(static value => value.implementationName, StringComparer.Ordinal))
        {
            builder.Append('|')
                .Append(mapping.apiNamespace)
                .Append('.')
                .Append(mapping.apiName)
                .Append('>')
                .Append(mapping.implementationNamespace)
                .Append('.')
                .Append(mapping.implementationName)
                .Append('`')
                .Append(mapping.arity);
        }
        foreach (ScriptApiAttachableType attachableType in profile.attachableTypes
                     .OrderBy(static value => value.implementationName, StringComparer.Ordinal))
        {
            builder.Append('|')
                .Append(attachableType.implementationName)
                .Append('>')
                .Append(attachableType.kind);
        }
        byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(builder.ToString()));
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static string[] GetImplementationPaths(ScriptApiProfile profile)
    {
        IReadOnlySet<string> frameworkAssemblyNames = FrameworkReferenceResolver.GetFrameworkAssemblyNames();
        return profile.implementationAssemblies
            .Where(assembly => !frameworkAssemblyNames.Contains(assembly.GetName().Name ?? string.Empty))
            .Select(static assembly => assembly.Location)
            .Where(static path => !string.IsNullOrWhiteSpace(path))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(static path => path, StringComparer.Ordinal)
            .ToArray();
    }

}
