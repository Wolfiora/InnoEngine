namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationProject(
    string name,
    string path,
    string? entry,
    string[] surfaces
);
