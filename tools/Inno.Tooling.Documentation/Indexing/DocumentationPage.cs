using System.Collections.Generic;

namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationMetadata(
    string uid,
    string kind,
    string[] owners,
    string[] aliases,
    string[] contracts,
    string? apiSurface,
    DocumentationExampleContext? examples
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
    IReadOnlyList<string> links,
    IReadOnlyList<DocumentationCodeBlockInfo> codeBlocks
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
    string declaration,
    IReadOnlyList<string> code
);
