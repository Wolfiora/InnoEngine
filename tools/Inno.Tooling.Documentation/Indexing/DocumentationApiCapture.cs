namespace Inno.Tooling.Documentation.Indexing;

internal sealed record DocumentationApiCapture(
    string path,
    string sha256,
    string model,
    string configuration,
    string? target,
    string sourceRevision,
    string sourceTree
);
