namespace Inno.Tooling.Architecture;

// A `SuppressMessage("InnoStyle", ...)` attribute that hides the findings of one rule inside one declaration (EX-01).
internal sealed record StyleSuppression(
    string rule,
    string path,
    int line,
    string justification
);
