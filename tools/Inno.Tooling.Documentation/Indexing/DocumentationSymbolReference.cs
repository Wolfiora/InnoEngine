namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationSymbolReference(
    string project,
    string surface,
    string id,
    string pageId,
    string anchor,
    string[]? surfaces = null
);
