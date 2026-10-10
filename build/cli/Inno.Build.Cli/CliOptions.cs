using System;
using System.Collections.Generic;

namespace Inno.Build.Cli;

internal sealed class CliOptions
{
    private readonly Dictionary<string, string> m_values = new(StringComparer.Ordinal);

    internal static CliOptions Parse(IReadOnlyList<string> arguments)
    {
        var result = new CliOptions();
        for (int index = 0; index < arguments.Count; index += 2)
        {
            string key = arguments[index];
            if (!key.StartsWith("--", StringComparison.Ordinal) || index + 1 >= arguments.Count)
                throw new ArgumentException($"Expected '--name value'; incomplete option '{key}'.");
            if (!result.m_values.TryAdd(key[2..], arguments[index + 1]))
                throw new ArgumentException($"Duplicate option '{key}'.");
        }
        return result;
    }

    internal string Read(
        string name,
        string defaultValue
    ) => m_values.GetValueOrDefault(name, defaultValue);

    internal string Require(string name)
        => m_values.TryGetValue(name, out string? value) && !string.IsNullOrWhiteSpace(value)
            ? value : throw new ArgumentException($"Required option '--{name}' is missing.");
}
