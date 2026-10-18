using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;

namespace Inno.Scripting.Compiler;

internal static class ScriptSourceLocations
{
    internal static ImmutableArray<KeyValuePair<string, string>> CreatePathMap(IReadOnlyList<ScriptSourceInput> sources)
        => sources.Select(static source => new KeyValuePair<string, string>(
                Path.GetDirectoryName(source.sourcePath)!,
                source.assetPath.source.value + "::./"
                    + (Path.GetDirectoryName(source.assetPath.localPath) ?? string.Empty).Replace('\\', '/')))
            .Distinct()
            .OrderByDescending(static mapping => mapping.Key.Length)
            .ThenBy(static mapping => mapping.Key, StringComparer.Ordinal)
            .ToImmutableArray();

    internal static string CreateProjectPathMap(IReadOnlyList<ScriptSourceInput> sources)
    {
        string value = string.Join(",", CreatePathMap(sources).Select(static mapping =>
            EscapeSeparators(mapping.Key) + "=" + EscapeSeparators(mapping.Value)));
        return value.Replace("%", "%25", StringComparison.Ordinal)
            .Replace("$", "%24", StringComparison.Ordinal)
            .Replace("@", "%40", StringComparison.Ordinal)
            .Replace(";", "%3B", StringComparison.Ordinal);
    }

    private static string EscapeSeparators(string value)
        => value.Replace(",", ",,", StringComparison.Ordinal).Replace("=", "==", StringComparison.Ordinal);
}
