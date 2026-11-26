using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Testing.Source;

namespace Inno.Tooling.Validation.Integration.Tests;

public sealed class HostedValidationReceiptTests
{
    [Fact]
    public async Task ReplaysFailedAndRetriedHostedRunsWithoutNetworkAccess()
    {
        string engine = TestRepository.GetRoot(typeof(HostedValidationReceiptTests).Assembly);
        string fixture = Path.Combine(engine, "tests/tools/Inno.Tooling.Validation.Integration.Tests/fixtures/hosted_receipts.py");
        await ToolchainEnvironment.RunAsync(
            OperatingSystem.IsWindows() ? "python" : "python3",
            ["-B", fixture, Path.Combine(engine, ".github/scripts")],
            engine
        );
    }
}
