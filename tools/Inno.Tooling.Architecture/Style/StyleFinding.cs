namespace Inno.Tooling.Architecture;

internal sealed record StyleFinding(
    string rule,
    string path,
    int line,
    string message
);
