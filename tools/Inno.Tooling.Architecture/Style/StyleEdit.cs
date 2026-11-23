namespace Inno.Tooling.Architecture;

// Replaces the half-open character range [start, end) of the original text.
internal sealed record StyleEdit(
    int start,
    int end,
    string text
);
