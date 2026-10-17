using System;
using System.Collections.Generic;

namespace Inno.Build.Cli;

internal sealed class BuildCommandLine
{
    private readonly Dictionary<string, string> m_values = new(StringComparer.Ordinal);

    internal static BuildCommandLine Parse(IReadOnlyList<string> arguments)
    {
        var result = new BuildCommandLine();
        for (int index = 0; index < arguments.Count; index++)
        {
            string key = arguments[index];
            if (!key.StartsWith("--", StringComparison.Ordinal))
                throw new ArgumentException($"Expected '--name value'; incomplete option '{key}'.");
            bool flag = key is "--clean" or "--dry-run";
            string value = "true";
            if (!flag)
            {
                if (++index >= arguments.Count || arguments[index].StartsWith("--", StringComparison.Ordinal))
                    throw new ArgumentException($"Missing value for '{key}'.");
                value = arguments[index];
            }
            if (!result.m_values.TryAdd(key[2..], value))
                throw new ArgumentException($"Duplicate option '{key}'.");
        }
        return result;
    }

    internal bool Has(string name) => m_values.ContainsKey(name);

    internal void Validate(params string[] allowed)
    {
        var names = new HashSet<string>(allowed, StringComparer.Ordinal);
        foreach (string key in m_values.Keys)
            if (!names.Contains(key))
                throw new ArgumentException($"Unknown option '--{key}'.");
    }

    internal string Read(
        string name,
        string defaultValue
    ) => m_values.GetValueOrDefault(name, defaultValue);

    internal string Require(string name)
        => m_values.TryGetValue(name, out string? value) && !string.IsNullOrWhiteSpace(value)
            ? value : throw new ArgumentException($"Required option '--{name}' is missing.");
}
