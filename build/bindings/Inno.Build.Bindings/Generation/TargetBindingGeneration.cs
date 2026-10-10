using System.IO;
using System.Threading;
using BGCS.Configuration;
using BGCS.Cpp2C.Configuration;
using Inno.Build.Toolchains;

namespace Inno.Build.Bindings;

internal static class TargetBindingGeneration
{
    internal static void Generate(
        NativeBuildContext context,
        BindingGenerationConfiguration definition,
        (CsCodeGeneratorConfig managed, Cpp2CGeneratorConfig? bridge, string? hostBridgeRoot) loaded,
        BindingGenerationPublication publication,
        string fingerprint,
        CancellationToken cancellation
    ) {
        string native = Path.Combine(publication.targetCandidate, "Native");
        string managed = Path.Combine(publication.targetCandidate, "Generated");
        BindingGenerationDiagnostics.Generate(context, definition, loaded, native, managed, cancellation);
        if (definition.request.checkOnly)
        {
            BindingGenerationDiagnostics.RequireEqual(managed, Path.Combine(publication.managedDestination, "Generated"), true);
            if (loaded.bridge is not null)
                BindingGenerationDiagnostics.RequireEqual(native, Path.Combine(publication.managedDestination, "Native"), false);
        }
        else
            BuildArtifactManifest.Write(publication.targetCandidate, fingerprint, ["Native", "Generated"], context);
    }

    internal static NativeBindingGenerationDescriptor Describe(
        string destination,
        string fingerprint,
        bool hasBridge
    ) => new()
    {
        fingerprint = fingerprint,
        bindingsPath = Path.Combine(destination, "Generated", "Bindings.cs"),
        bridgeDirectory = hasBridge ? Path.Combine(destination, "Native") : string.Empty
    };
}
