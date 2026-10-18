using System.Threading;
using BGCS.Configuration;
using BGCS.Cpp2C.Configuration;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class HostBindingGeneration
{
    internal static void Generate(
        NativeBuildContext context,
        BindingGenerationConfiguration definition,
        (CsCodeGeneratorConfig managed, Cpp2CGeneratorConfig? bridge, string? hostBridgeRoot) loaded,
        BindingGenerationPublication publication,
        string fingerprint,
        CancellationToken cancellation
    ) {
        string managed = publication.GetHostCandidate(publication.managedDestination);
        string? native = publication.nativeDestination is null ? null : publication.GetHostCandidate(publication.nativeDestination);
        BindingGenerationDiagnostics.Generate(context, definition, loaded, native, managed, cancellation);
        if (definition.request.checkOnly)
        {
            BindingGenerationDiagnostics.RequireEqual(managed, publication.managedDestination, true);
            if (native is not null)
                BindingGenerationDiagnostics.RequireEqual(native, publication.nativeDestination!, false);
        }
    }
}
