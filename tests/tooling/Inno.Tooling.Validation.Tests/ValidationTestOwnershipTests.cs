using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationTestOwnershipTests
{
    [Fact]
    public async Task AFailedUnionRetainsSuccessfulOwnersAndRejectsMissingIsolatedReceipts()
    {
        using var repository = new ValidationRepository(TestRepository.GetRoot(typeof(ValidationTestOwnershipTests).Assembly));
        const string C_INPUT = "tests/input/Inno.Input.Tests/Inno.Input.Tests.csproj";
        const string C_UI = "tests/ui/Inno.UI.Tests/Inno.UI.Tests.csproj";
        const string C_PROBE = "tests/input/Inno.Input.Tests/UnionFailureProbe.cs";
        repository.Write(C_PROBE, """
            using Xunit;
            namespace Inno.Input.Tests;
            public sealed class UnionFailureProbe
            {
                [Fact]
                public void SharedFailure() { Assert.Fail("Intentional owner failure"); }
                [Fact, Trait("ProcessIsolation", "Required")]
                public void IsolatedFailure() { Assert.Fail("Intentional isolated owner failure"); }
            }
            """);
        repository.Write("tests/ui/Inno.UI.Tests/UnionSuccessProbe.cs", """
            using Xunit;
            namespace Inno.UI.Tests;
            public sealed class UnionSuccessProbe
            {
                [Fact, Trait("ProcessIsolation", "Required")]
                public void IsolatedSuccess() { Assert.Equal(2, 1 + 1); }
            }
            """);
        string failedSource = File.ReadAllText(repository.PathOf(C_PROBE));
        string passingSource = failedSource.Replace("Assert.Fail(\"Intentional owner failure\")", "Assert.Equal(2, 1 + 1)", StringComparison.Ordinal)
            .Replace("Assert.Fail(\"Intentional isolated owner failure\")", "Assert.Equal(2, 1 + 1)", StringComparison.Ordinal);
        // First restore introduces actual NuGet imports into the SDK graph. Warm that environment before testing exact-input reuse.
        repository.Write(C_PROBE, passingSource);
        using (await repository.PlanAsync()) { }
        Assert.Equal(0, await repository.RunAsync(output: "artifacts/warmup"));
        repository.Write(C_PROBE, failedSource);
        using (JsonDocument plan = await repository.PlanAsync())
            Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        JsonElement originalUi;
        using (JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json"))))
        {
            JsonElement[] checks = results.RootElement.GetProperty("results").EnumerateArray().ToArray();
            Assert.Equal("Failed", checks.Single(check => check.GetProperty("id").GetString() == "test-execution:managed").GetProperty("status").GetString());
            JsonElement input = checks.Single(check => check.GetProperty("id").GetString() == "test:" + C_INPUT);
            Assert.Equal("Failed", input.GetProperty("status").GetString());
            Assert.Contains("SharedFailure", input.GetProperty("detail").GetString());
            Assert.Contains("IsolatedFailure", input.GetProperty("detail").GetString());
            originalUi = checks.Single(check => check.GetProperty("id").GetString() == "test:" + C_UI).Clone();
            Assert.Equal("Passed", originalUi.GetProperty("status").GetString());
        }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));

        repository.Write(C_PROBE, passingSource);
        using (await repository.PlanAsync()) { }
        Assert.Equal(0, await repository.RunAsync(output: "artifacts/repaired", evidence: "artifacts/run/results.json"));
        using JsonDocument repaired = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/repaired/results.json")));
        JsonElement ui = repaired.RootElement.GetProperty("results").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "test:" + C_UI);
        string expected = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ? "Reused" : "Passed";
        Assert.True(ui.GetProperty("status").GetString() == expected,
            "Expected " + expected + "; original owner evidence: " + originalUi.GetRawText() + "; repaired: " + ui.GetRawText());

        const string C_EXECUTOR = "tests/Invoke-IsolatedTests.ps1";
        repository.Write(C_EXECUTOR, File.ReadAllText(repository.PathOf(C_EXECUTOR)).Replace("Add-CheckResult 'Isolated' $name",
            "if ($project.BaseName -eq 'Inno.UI.Tests') { continue }; Add-CheckResult 'Isolated' $name", StringComparison.Ordinal));
        repository.Git("add", C_EXECUTOR);
        repository.Git("commit", "-m", "Model an executor losing an isolated receipt");
        using (await repository.PlanAsync()) { }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/missing-isolation"));
        using JsonDocument incomplete = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/missing-isolation/results.json")));
        JsonElement missing = incomplete.RootElement.GetProperty("results").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "test:" + C_UI);
        Assert.Equal("Failed", missing.GetProperty("status").GetString());
        Assert.Contains("Required isolated execution is missing", missing.GetProperty("detail").GetString());
    }
}
