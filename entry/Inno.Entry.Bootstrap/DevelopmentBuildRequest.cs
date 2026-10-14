using System;
using System.Collections.Generic;

namespace Inno.Entry.Bootstrap;

internal static class DevelopmentBuildRequest
{
    internal static string? ReadDotnet(string[] arguments)
    {
        string? selected = null;
        for (int index = 0; index < arguments.Length; index++)
        {
            if (arguments[index] != "--dotnet")
                continue;
            if (selected is not null || index + 1 >= arguments.Length
                || arguments[index + 1].StartsWith("--", StringComparison.Ordinal))
                throw new ArgumentException("The --dotnet option requires one explicit SDK executable.", nameof(arguments));
            selected = arguments[++index];
        }
        return selected;
    }

    internal static string[] Create(
        string[] arguments,
        DevelopmentHost host,
        string root,
        string dotnet,
        string configuration
    ) {
        bool implicitEditor = arguments.Length == 0 || arguments[0].StartsWith("--", StringComparison.Ordinal);
        string command = implicitEditor ? "editor" : arguments[0];
        if (command is not ("editor" or "clean" or "bindings" or "support-pack" or "shader"))
            return arguments;
        if (command == "editor")
            host.RequireEditor();
        var result = new List<string> { command };
        result.AddRange(implicitEditor ? arguments : arguments[1..]);
        AddDefault(result, "--engine-root", root);
        AddDefault(result, "--dotnet", dotnet);
        int scopeIndex = result.IndexOf("--scope");
        bool globalCacheCleanup = command == "clean" && scopeIndex >= 0
            && scopeIndex + 1 < result.Count && result[scopeIndex + 1] == "cache";
        if (!globalCacheCleanup)
            AddDefault(result, "--target", host.target);
        AddDefault(result, "--tools-target", host.target);
        AddDefault(result, "--configuration", command is "editor" or "clean" ? configuration : "Release");
        return result.ToArray();
    }

    private static void AddDefault(
        List<string> arguments,
        string name,
        string value
    ) {
        if (arguments.Contains(name))
            return;
        arguments.Add(name);
        arguments.Add(value);
    }
}
