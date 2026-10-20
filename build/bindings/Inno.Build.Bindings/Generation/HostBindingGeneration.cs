using System.Threading;
using BGCS;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class HostBindingGeneration
{
    internal static void Generate(
        NativeBuildContext context,
        BindingGenerationConfiguration definition,
        GenerationRequest loaded,
        BindingGenerationPublication publication,
        string fingerprint,
        CancellationToken cancellation
    ) {
        string managed = publication.GetHostCandidate(publication.managedDestination);
        string native = publication.GetHostCandidate(publication.nativeDestination!);
        BindingGenerationDiagnostics.Generate(context, definition, loaded, publication.bgcsCandidate, native, managed, cancellation);
        if (definition.request.checkOnly)
        {
            BindingGenerationDiagnostics.RequireEqual(managed, publication.managedDestination, true);
            BindingGenerationDiagnostics.RequireEqual(native, publication.nativeDestination!, false);
        }
    }
}
