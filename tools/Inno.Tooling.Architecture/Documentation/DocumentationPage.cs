using System.Collections.Generic;

namespace Inno.Tooling.Architecture.Documentation;

internal sealed record DocumentationPage(
    string path,
    string identity,
    string title,
    string text,
    bool hasApi,
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
