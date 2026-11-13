using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationProject(
    string name,
    string path,
    string? entry,
    string[] surfaces
);

internal sealed record DocumentationBinding(
    string repository,
    string revision,
    string sourceTree,
    string projectManifest,
    string projectManifestIdentity,
    string toolingRevision,
    [property: JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)] IReadOnlyList<DocumentationApiCapture>? apiManifests = null
);

internal sealed record DocumentationApiCapture(
    string path,
    string sha256,
    string model,
    string configuration,
    string? target,
    string sourceRevision,
    string sourceTree
);

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

internal sealed record DocumentationSymbolReference(
    string project,
    string surface,
    string id,
    string pageId,
    string anchor,
    string[]? surfaces = null
);

internal sealed record DocumentationPageReference(
    string pageId,
    string key,
    string payloadIdentity,
    DocumentationMetadata metadata
);

internal sealed record DocumentationSearchEntry(
    string pageId,
    string section,
    string title,
    string body,
    string[] tokens
);
