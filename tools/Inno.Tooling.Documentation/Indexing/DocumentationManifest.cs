using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationManifest(
    string identity,
    string schema,
    string parserSchema,
    DocumentationBinding binding,
    IReadOnlyDictionary<string, DocumentationPageReference> pages,
    IReadOnlyList<DocumentationProject> projects,
    IReadOnlyList<DocumentationSearchEntry> search,
    IReadOnlyList<DocumentationSymbolReference> symbols,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<DocumentationReferenceInfo>? references = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyDictionary<string, string>? sourceFiles = null,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<DocumentationApiOrigin>? apiOrigins = null
);
