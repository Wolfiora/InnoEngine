using System;

namespace Inno.Entry.Bootstrap;

internal sealed record DevelopmentToolDefinition(
    string cacheName,
    string project,
    string assemblyName,
    string applicationType
) {
    internal static DevelopmentToolDefinition Build { get; } = new("cli",
        "build/cli/Inno.Build.Cli/Inno.Build.Cli.csproj", "Inno.Build.Cli", "Inno.Build.Cli.BuildApplication");
    internal static DevelopmentToolDefinition Maintenance { get; } = new("maintenance",
        "build/toolchains/Inno.Build.Toolchains/Inno.Build.Toolchains.csproj", "Inno.Build.Toolchains", "Inno.Build.Toolchains.BuildMaintenanceApplication");

    internal static DevelopmentToolDefinition Select(string id)
        => id switch
        {
            "cli" => Build,
            "maintenance" => Maintenance,
            _ => throw new ArgumentException("Unknown development tool.", nameof(id))
        };
}
