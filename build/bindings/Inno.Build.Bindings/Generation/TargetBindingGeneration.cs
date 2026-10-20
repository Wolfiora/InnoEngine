using System.IO;
using System.Threading;
using BGCS;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class TargetBindingGeneration
{
    internal static void Generate(
        NativeBuildContext context,
        BindingGenerationConfiguration definition,
        GenerationRequest loaded,
        BindingGenerationPublication publication,
        string fingerprint,
        CancellationToken cancellation
    ) {
        string native = Path.Combine(publication.targetCandidate, "Native");
        string managed = Path.Combine(publication.targetCandidate, "Generated");
        BindingGenerationDiagnostics.Generate(context, definition, loaded, publication.bgcsCandidate, native, managed, cancellation);
        if (definition.request.checkOnly)
        {
            BindingGenerationDiagnostics.RequireEqual(managed, Path.Combine(publication.managedDestination, "Generated"), true);
            BindingGenerationDiagnostics.RequireEqual(native, Path.Combine(publication.managedDestination, "Native"), false);
        }
        else
            BuildArtifactManifest.Write(publication.targetCandidate, fingerprint, ["Native", "Generated"], context);
    }

    internal static NativeBindingGenerationDescriptor Describe(
        string destination,
        string fingerprint
    ) => new()
    {
        fingerprint = fingerprint,
        artifactDirectory = destination,
        bindingsPath = Path.Combine(destination, "Generated", "Bindings.cs"),
        nativeDirectory = Path.Combine(destination, "Native")
    };
}
