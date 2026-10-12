namespace Inno.Build.TaskHosting;

internal sealed record TaskRuntimeRequestSnapshot(
    string key,
    bool needsRestore
);
