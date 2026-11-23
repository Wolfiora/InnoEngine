namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationSearchEntry(
    string pageId,
    string section,
    string title,
    string body,
    string[] tokens
);
