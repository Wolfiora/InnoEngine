namespace Inno.Build.Toolchains;

internal sealed record ManagedBuildInputCapture(
    string fingerprint,
    string[] declarations
);
