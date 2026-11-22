namespace Inno.Tooling.Validation;

internal sealed record ValidationShard(
    string id,
    string[] checks,
    string[] prerequisites
);
