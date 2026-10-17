using Microsoft.CodeAnalysis;

namespace Inno.Runtime.Generators;

internal static class RuntimeRegistrationDiagnostics
{
    private static readonly DiagnosticDescriptor UnavailableConstruction = new(
        "INNORUN002", "Static converter construction is unavailable",
        "Serialization converter '{0}' cannot be linked: {1}", "Architecture", DiagnosticSeverity.Error, true);

    internal static void ReportUnavailable(
        SourceProductionContext output,
        INamedTypeSymbol type,
        string reason
    ) => output.ReportDiagnostic(Diagnostic.Create(UnavailableConstruction,
        type.Locations.Length == 0 ? Location.None : type.Locations[0], type.ToDisplayString(), reason));
}
