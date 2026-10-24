using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using BGCS;
using BGCS.Core.Artifacts;
using BGCS.Core.Inputs;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class BindingNativeCompilation
{
    internal static void Write(
        NativeSourceManifest manifest,
        GenerationResult result,
        GenerationRequest request,
        string engineRoot,
        string nativeOutput,
        CancellationToken cancellation
    ) {
        var bindings = result.nativeInputRoots.Where(binding => binding.targetId == manifest.targetId)
            .ToDictionary(static binding => binding.rootId, StringComparer.Ordinal);
        NativeBindingSourceRoot[] roots = manifest.inputRoots.Select(root => new NativeBindingSourceRoot(
            root.id,
            root.kind == "generated" ? string.Empty : Path.GetRelativePath(engineRoot, bindings[root.id].directory).Replace('\\', '/'),
            root.kind == "generated",
            root.kind is "generated" or "project" or "user")).ToArray();
        var sources = manifest.sources.Select(static source => source.relativePath).ToHashSet(StringComparer.Ordinal);
        var files = manifest.inputs.Select(input => new NativeBindingSourceFile(input.rootId, input.relativePath,
            input.length, input.digest.value, input.role == "ShimSource"
                || input.rootId == "generated" && sources.Contains(input.relativePath))).ToList();
        foreach (ArtifactDescriptor artifact in manifest.sources.Concat(manifest.headers))
        {
            if (!files.Any(file => file.rootId == "generated" && file.relativePath == artifact.relativePath))
                files.Add(new("generated", artifact.relativePath, new FileInfo(Path.Combine(nativeOutput, artifact.relativePath)).Length,
                    artifact.digest.value, sources.Contains(artifact.relativePath)));
        }
        var table = manifest.staticExportTable;
        var target = result.targetModels.Single(model => model.targetId == manifest.targetId).resolvedTarget
            ?? throw new InvalidDataException("The SDK must return the resolved target used for generation.");
        var parserDefines = target.toolchain.defines.ToHashSet(StringComparer.Ordinal);
        var explicitDefines = request.configuration.source.defines.Concat(request.configuration.targets.Single().settings.defines)
            .ToHashSet(StringComparer.Ordinal);
        var compilation = new NativeBindingCompilation
        {
            roots = roots,
            files = files.OrderBy(static file => file.rootId, StringComparer.Ordinal)
                .ThenBy(static file => file.relativePath, StringComparer.Ordinal).ToArray(),
            includeDirectories = manifest.includeDirectories.Select(static directory => new NativeBindingIncludeDirectory(
                directory.rootId, directory.relativePath, directory.isSystem, directory.isFramework)).ToArray(),
            defines = manifest.defines.Where(define => !parserDefines.Contains(define) || explicitDefines.Contains(define)).ToArray(),
            compilerDefines = manifest.defines.Where(define => parserDefines.Contains(define) && !explicitDefines.Contains(define)).ToArray(),
            cxxStandard = Math.Max(manifest.language == SourceLanguage.Cpp ? ParseStandard(manifest.languageStandard, "c++") : 0,
                manifest.languageRequirements.Select(static requirement => requirement.minimumStandard).DefaultIfEmpty().Max()),
            cStandard = manifest.language == SourceLanguage.C ? ParseStandard(manifest.languageStandard, "c") : 0,
            requiredExports = manifest.exportBindings.Select(static binding => binding.exportName).Distinct(StringComparer.Ordinal).ToArray(),
            staticExportSymbols = table is null ? [] : [table.getExportSymbol, table.getRevisionSymbol, table.getCountSymbol,
                table.getAbiIdentitySymbol, table.getPointerSizeSymbol],
            staticExportAbi = table?.abiIdentity ?? string.Empty,
            staticExportNames = table?.exportNames.ToArray() ?? []
        };
        File.WriteAllBytes(Path.Combine(nativeOutput, "compilation.json"), JsonSerializer.SerializeToUtf8Bytes(compilation));
        NativeBindingCompilation verified = NativeBindingCompilation.Load(nativeOutput);
        verified.VerifyInputs(id => verified.ResolveRoot(engineRoot, nativeOutput, id), cancellation);
    }

    private static int ParseStandard(
        string standard,
        string prefix
    ) {
        string normalized = standard.StartsWith("gnu", StringComparison.Ordinal) ? "c" + standard[3..] : standard;
        if (!normalized.StartsWith(prefix, StringComparison.Ordinal)
            || !int.TryParse(normalized.AsSpan(prefix.Length), NumberStyles.None, CultureInfo.InvariantCulture, out int value))
            throw new InvalidDataException("The selected native language standard cannot be represented by CMake: " + standard);
        return value;
    }
}
