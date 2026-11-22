using System;
using System.IO;
using System.Linq;

namespace Inno.Tooling.Validation;

internal static class ValidationCiContracts
{
    internal const string C_RULE_LOADER = "tests/ci/Test-AgentRules.ps1";
    internal const string C_RULE_EXAMPLES = "tests/ci/Test-AgentRules.examples.json";
    private const string C_DIRECTORY = "tests/ci/";

    internal const string C_GATE = "tests/ci/Invoke-ValidationGate.ps1";

    internal static readonly string[] GateScripts = [C_GATE, "tests/ci/Invoke-ValidationGate.Tests.ps1"];

    internal static readonly string[] GateContracts =
    [
        "tests/ci/Invoke-ValidationGate.Tests.ps1",
        "tests/ci/Join-ValidationCi.Tests.ps1",
        "tests/ci/Test-ValidationReport.Tests.ps1",
        "tests/ci/Write-ValidationSummary.Tests.ps1",
        "tests/ci/ValidationIncident.Tests.ps1"
    ];

    internal static bool Owns(string path)
        => path == C_GATE || path.StartsWith(C_DIRECTORY, StringComparison.Ordinal)
            && !path[C_DIRECTORY.Length..].Contains('/')
            && path.EndsWith(".Tests.ps1", StringComparison.Ordinal);

    internal static string[] Discover(string root)
        => Directory.GetFiles(Path.Combine(root, "tests", "ci"))
            .Where(path => path.EndsWith(".Tests.ps1", StringComparison.Ordinal))
            .Order(StringComparer.Ordinal).ToArray();
}
