using System.Collections.Generic;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationMetadata(
    string uid,
    string kind,
    string[] owners,
    string[] aliases,
    string[] contracts,
    string? apiSurface
);

internal sealed record DocumentationPage(
    string path,
    string identity,
    string title,
    string text,
    DocumentationMetadata metadata,
    IReadOnlyList<DocumentationSection> sections,
    IReadOnlyList<DocumentationSymbol> symbols,
    IReadOnlyList<string> includes,
    IReadOnlyList<string> links
);

internal sealed record DocumentationSection(
    string anchor,
    string title,
    int level,
    int start,
    int end,
    int line
);

internal sealed record DocumentationSymbol(
    string id,
    string signatureIdentity,
    string anchor,
    string section,
    string declaration
);

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
