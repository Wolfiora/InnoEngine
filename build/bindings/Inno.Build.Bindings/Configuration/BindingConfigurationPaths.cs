using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using BGCS.Configuration;
using BGCS.Cpp2C.Configuration;

namespace Inno.Build.Bindings;

internal static class BindingConfigurationPaths
{
    private static readonly Regex Variables = new("%([A-Za-z_][A-Za-z0-9_]*)%", RegexOptions.CultureInvariant);

    internal static void Apply(
        CsCodeGeneratorConfig config,
        IReadOnlyDictionary<string, string>? environment
    ) {
        config.entryFiles = config.entryFiles.Select(path => Expand(path, environment)!).ToList();
        config.allowedHeaders = config.allowedHeaders.Select(path => Expand(path, environment)!).ToList();
        config.includeFolders = config.includeFolders.Select(path => Expand(path, environment)!).ToList();
        config.systemIncludeFolders = config.systemIncludeFolders.Select(path => Expand(path, environment)!).ToList();
        config.outputPath = Expand(config.outputPath, environment)!;
        config.targetSysRoot = Expand(config.targetSysRoot, environment);
        config.compilerPath = Expand(config.compilerPath, environment);
    }

    internal static void Apply(
        Cpp2CGeneratorConfig config,
        IReadOnlyDictionary<string, string>? environment
    ) {
        config.entryFiles = config.entryFiles.Select(path => Expand(path, environment)!).ToList();
        config.allowedHeaders = config.allowedHeaders.Select(path => Expand(path, environment)!).ToList();
        config.includeFolders = config.includeFolders.Select(path => Expand(path, environment)!).ToList();
        config.systemIncludeFolders = config.systemIncludeFolders.Select(path => Expand(path, environment)!).ToList();
        config.outputPath = Expand(config.outputPath, environment)!;
        config.targetSysRoot = Expand(config.targetSysRoot, environment);
        config.compilerPath = Expand(config.compilerPath, environment);
    }

    internal static string? Expand(
        string? path,
        IReadOnlyDictionary<string, string>? environment
    ) {
        if (path is null)
            return null;
        return Variables.Replace(path, match =>
        {
            string name = match.Groups[1].Value;
            if (environment is null || !environment.TryGetValue(name, out string? value) || string.IsNullOrWhiteSpace(value))
                throw new InvalidOperationException($"Binding path requires '{name}' from the operation's frozen native toolchain.");
            if (Variables.IsMatch(value))
                throw new InvalidOperationException($"Frozen native toolchain value '{name}' contains an unresolved path variable.");
            return value;
        });
    }
}
