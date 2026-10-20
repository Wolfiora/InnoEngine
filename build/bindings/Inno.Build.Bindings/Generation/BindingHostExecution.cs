using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BGCS;
using BGCS.Contracts.Artifacts;

namespace Inno.Build.Bindings;

internal static class BindingHostExecution
{
    internal static void Generate(
        GenerationRequest request,
        string engineRoot,
        string nativeOutput,
        string managedOutput,
        CancellationToken cancellation
    ) => GenerateAsync(request, engineRoot, nativeOutput, managedOutput, cancellation).GetAwaiter().GetResult();

    private static async Task GenerateAsync(
        GenerationRequest request,
        string engineRoot,
        string nativeOutput,
        string managedOutput,
        CancellationToken cancellation
    ) {
        await using BindingHost host = BindingHostFactory.CreateBuilder(cancellation).Build();
        GenerationResult result = await host.GenerateAsync(request, cancellation).ConfigureAwait(false);
        foreach (var diagnostic in result.diagnostics)
        {
            Console.WriteLine($"BGCS {diagnostic.code}: {diagnostic.message}");
            if (diagnostic.details is not null)
                Console.WriteLine(diagnostic.details);
        }
        if (!result.success)
            throw new InvalidDataException("BGCS generation failed: " + result.GetDiagnosticSummary());
        foreach (var stage in result.cacheStages)
            Console.WriteLine($"BGCS cache {stage.targetId}/{stage.stage}: {stage.status}; {stage.reason}");
        var artifacts = BindingArtifactManifest.Read(result.outputRoot, request.configuration.library.id, cancellation);
        ArtifactDescriptor[] managed = artifacts.Where(static artifact => artifact.relativePath.EndsWith("/managed/Bindings.cs", StringComparison.Ordinal)).ToArray();
        if (managed.Length != 1)
            throw new InvalidDataException("BGCS must publish exactly one selected target's managed Bindings.cs.");
        NativeSourceManifest[] manifests = BindingArtifactManifest.ReadNativeSources(
            result.outputRoot, request.configuration.library.id, cancellation).ToArray();
        if (manifests.Length != 1)
            throw new InvalidDataException("A component generation requires exactly one native source manifest.");
        NativeSourceManifest manifest = manifests[0];
        var generatedRoots = manifest.inputRoots.Where(static root => root.kind == "generated")
            .Select(static root => root.id).ToHashSet(StringComparer.Ordinal);
        var nativePaths = manifest.sources.Concat(manifest.headers).Select(static artifact => artifact.relativePath)
            .Concat(manifest.inputs.Where(input => generatedRoots.Contains(input.rootId)).Select(static input => input.relativePath))
            .ToHashSet(StringComparer.Ordinal);
        var selectedArtifacts = artifacts.Where(artifact => nativePaths.Contains(artifact.relativePath)).ToArray();
        if (selectedArtifacts.Length != nativePaths.Count)
            throw new InvalidDataException("Every generated native input must belong to the SDK's verified publication.");
        Directory.CreateDirectory(managedOutput);
        Directory.CreateDirectory(nativeOutput);
        foreach (ArtifactDescriptor artifact in selectedArtifacts.Append(managed[0]))
        {
            cancellation.ThrowIfCancellationRequested();
            string destination = artifact == managed[0] ? Path.Combine(managedOutput, "Bindings.cs")
                : Path.Combine(nativeOutput, artifact.relativePath);
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(Path.Combine(result.outputRoot, artifact.relativePath), destination, overwrite: false);
        }
        BindingNativeCompilation.Write(manifest, result, request, engineRoot, nativeOutput, cancellation);
    }
}
