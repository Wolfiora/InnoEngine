using System;
using System.IO;
using System.Linq;

namespace Inno.Tooling.Validation;

internal static class ValidationCiContracts
{
    internal const string C_RULE_LOADER = "tests/ci/Test-AgentRules.ps1";
    private const string C_DIRECTORY = "tests/ci/";

    internal static bool Owns(string path)
        => path.StartsWith(C_DIRECTORY, StringComparison.Ordinal)
            && !path[C_DIRECTORY.Length..].Contains('/')
            && path.EndsWith(".Tests.ps1", StringComparison.Ordinal);

    internal static string[] Discover(string root)
        => Directory.GetFiles(Path.Combine(root, "tests", "ci"))
            .Where(path => Owns(Path.GetRelativePath(root, path).Replace('\\', '/')))
            .Order(StringComparer.Ordinal).ToArray();
}
