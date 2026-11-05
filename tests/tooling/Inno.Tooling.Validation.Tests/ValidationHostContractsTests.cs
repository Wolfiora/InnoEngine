using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationHostContractsTests
{
    [Fact]
    public async Task ActualTrxDistinguishesCompiledHostExclusionsFromMissingRequiredExecution()
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationHostContractsTests).Assembly));
        string currentHost = OperatingSystem.IsWindows() ? "windows" : OperatingSystem.IsMacOS() ? "macos" : "linux";
        string otherHost = currentHost == "linux" ? "windows" : "linux";
        string source = $$"""
            using System;
            using System.Runtime.Versioning;
            using Xunit;
            namespace Inno.Input.Tests;
            [SupportedOSPlatform("{{otherHost}}")]
            public sealed class HostContractProbe
            {
                [Fact(Skip = "The declared host is unavailable in this matrix cell.")]
                public void UnsupportedHost() { Assert.Fail("This case must not execute on another host."); }
                [Fact]
                [SupportedOSPlatform("{{currentHost}}")]
                public void RequiredHost() { Assert.Equal(2, 1 + 1); }
                [Fact]
                public void ExistingGuard()
                {
                    if (!OperatingSystem.IsOSPlatform("{{otherHost}}")) return;
                    Assert.True(true);
                }
            }
            """;
        const string C_PROBE = "tests/input/Inno.Input.Tests/HostContractProbe.cs";
        repository.Write(C_PROBE, source);
        using (JsonDocument plan = await repository.PlanAsync())
            Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        Assert.Equal(0, await repository.RunAsync());
        using (JsonDocument receipt = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/managed-trx/host-contracts.json"))))
        {
            string[] excluded = receipt.RootElement.GetProperty("tests/input/Inno.Input.Tests/Inno.Input.Tests.csproj")
                .EnumerateArray().Select(static item => item.GetString()!).ToArray();
            Assert.Equal(2, excluded.Length);
            Assert.Contains(excluded, static item => item.Contains("UnsupportedHost", StringComparison.Ordinal));
            Assert.Contains(excluded, static item => item.Contains("ExistingGuard", StringComparison.Ordinal));
            Assert.DoesNotContain(excluded, static item => item.Contains("RequiredHost", StringComparison.Ordinal));
        }

        repository.Write(C_PROBE, source.Replace("public void UnsupportedHost()",
            "[SupportedOSPlatform(\"" + currentHost + "\")]\n    public void UnsupportedHost()", StringComparison.Ordinal));
        using (await repository.PlanAsync()) { }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/required-host"));
        Assert.Contains("Required test was skipped", File.ReadAllText(repository.PathOf("artifacts/required-host/results.json")), StringComparison.Ordinal);

        repository.Write(C_PROBE, source.Replace("[SupportedOSPlatform(\"" + otherHost + "\")]", string.Empty, StringComparison.Ordinal));
        using (await repository.PlanAsync()) { }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/undeclared-skip"));
        Assert.Contains("Required test was skipped", File.ReadAllText(repository.PathOf("artifacts/undeclared-skip/results.json")), StringComparison.Ordinal);
    }
}
