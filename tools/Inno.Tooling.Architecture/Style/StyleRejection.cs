namespace Inno.Tooling.Architecture;

// A fix that was discarded because its result did not preserve the program text.
internal sealed record StyleRejection(
    string rule,
    string path,
    string reason
);
