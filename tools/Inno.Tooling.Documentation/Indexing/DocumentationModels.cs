using System.Collections.Generic;

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
    string toolingRevision
);

internal sealed record DocumentationManifest(
    string identity,
    string schema,
    string parserSchema,
    DocumentationBinding binding,
    IReadOnlyDictionary<string, DocumentationPageReference> pages,
    IReadOnlyList<DocumentationProject> projects,
    IReadOnlyList<DocumentationSearchEntry> search,
    IReadOnlyList<DocumentationSymbolReference> symbols
);

internal sealed record DocumentationSymbolReference(
    string project,
    string surface,
    string id,
    string pageId,
    string anchor
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
