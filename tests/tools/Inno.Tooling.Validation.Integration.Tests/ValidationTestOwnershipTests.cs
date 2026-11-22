using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Source;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Integration.Tests;

public sealed class ValidationTestOwnershipTests
{
    [Fact]
    public async Task AFailedUnionRetainsSuccessfulOwnersAndRejectsMissingIsolatedReceipts()
    {
        using var repository = new ValidationRepository(
            TestRepository.GetRoot(typeof(ValidationTestOwnershipTests).Assembly)
        );
        const string C_INPUT = "tests/services/input/Inno.Input.Tests/Inno.Input.Tests.csproj";
        const string C_UI = "tests/services/ui/Inno.UI.Tests/Inno.UI.Tests.csproj";
        const string C_PROBE = "tests/services/input/Inno.Input.Tests/UnionFailureProbe.cs";
        repository.Write(C_PROBE, """
            using Xunit;

            namespace Inno.Input.Tests;

            public sealed class UnionFailureProbe
            {
                [Fact]
                public void SharedFailure() => Assert.Fail("Intentional owner failure");

                [Fact]
                [Trait("ProcessIsolation", "Required")]
                public void IsolatedFailure() => Assert.Fail("Intentional isolated owner failure");
            }

            """);
        repository.Write("tests/services/ui/Inno.UI.Tests/UnionSuccessProbe.cs", """
            using Xunit;

            namespace Inno.UI.Tests;

            public sealed class UnionSuccessProbe
            {
                [Fact]
                [Trait("ProcessIsolation", "Required")]
                public void IsolatedSuccess() => Assert.Equal(2, 1 + 1);
            }

            """);
        string failedSource = File.ReadAllText(repository.PathOf(C_PROBE));
        string passingSource = failedSource
            .Replace("Assert.Fail(\"Intentional owner failure\")", "Assert.Equal(2, 1 + 1)", StringComparison.Ordinal)
            .Replace("Assert.Fail(\"Intentional isolated owner failure\")", "Assert.Equal(2, 1 + 1)", StringComparison.Ordinal);
        using (JsonDocument plan = await repository.PlanAsync())
        {
            Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        JsonElement originalUI;
        using (JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json"))))
        {
            JsonElement[] checks = results.RootElement.GetProperty("results").EnumerateArray().ToArray();
            JsonElement fault = Assert.Single(results.RootElement.GetProperty("failures").EnumerateArray());
            Assert.Equal("test-execution", fault.GetProperty("stage").GetString());
            Assert.Equal("managed", fault.GetProperty("owner").GetString());
            JsonElement input = checks.Single(check => check.GetProperty("id").GetString() == "test:" + C_INPUT);
            Assert.Equal("Failed", input.GetProperty("status").GetString());
            Assert.Contains("SharedFailure", input.GetProperty("detail").GetString());
            Assert.Contains("IsolatedFailure", input.GetProperty("detail").GetString());
            originalUI = checks.Single(check => check.GetProperty("id").GetString() == "test:" + C_UI).Clone();
            Assert.Equal("Passed", originalUI.GetProperty("status").GetString());
        }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));

        repository.Write(C_PROBE, passingSource);
        using (await repository.PlanAsync()) { }
        Assert.Equal(0, await repository.RunAsync(output: "artifacts/repaired", evidence: "artifacts/run/results.json"));
        using JsonDocument repaired = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/repaired/results.json")));
        JsonElement ui = repaired.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "test:" + C_UI);
        string expected = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CI")) ? "Reused" : "Passed";
        Assert.True(
            ui.GetProperty("status").GetString() == expected,
            "Expected " + expected + "; original owner evidence: " + originalUI.GetRawText() + "; repaired: " + ui.GetRawText()
        );

        const string C_EXECUTOR = "tests/Invoke-IsolatedTests.ps1";
        string executor = File.ReadAllText(repository.PathOf(C_EXECUTOR));
        repository.Write(
            C_EXECUTOR,
            executor.Replace(
                "Add-CheckResult 'Isolated' $name",
                "if ($project.BaseName -eq 'Inno.UI.Tests') { continue }; Add-CheckResult 'Isolated' $name",
                StringComparison.Ordinal
            )
        );
        repository.Git("add", C_EXECUTOR);
        repository.Git("commit", "-m", "Model an executor losing an isolated receipt");
        using (await repository.PlanAsync()) { }
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/missing-isolation"));
        using JsonDocument incomplete = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/missing-isolation/results.json")));
        JsonElement missing = incomplete.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "test:" + C_UI);
        Assert.Equal("Failed", missing.GetProperty("status").GetString());
        Assert.Contains("Required isolated execution is missing", missing.GetProperty("detail").GetString());

        repository.Write(C_EXECUTOR, executor + "\nexit 17\n");
        repository.Git("add", C_EXECUTOR);
        repository.Git("commit", "-m", "Model a process failure after passing TRX output");
        using (await repository.PlanAsync()) { }
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(
            () => repository.RunAsync(output: "artifacts/process-failure")
        );
        Assert.Contains("exited with 17", failure.Message);
        using JsonDocument process = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/process-failure/results.json")));
        JsonElement[] tests = process.RootElement.GetProperty("results")
            .EnumerateArray()
            .Where(check => check.GetProperty("id").GetString()!.StartsWith("test:", StringComparison.Ordinal))
            .ToArray();
        foreach (JsonElement check in tests)
        {
            string? id = check.GetProperty("id").GetString();
            string expectedStatus = id == "test:" + C_INPUT || id == "test:" + C_UI ? "Passed" : "NotApplicable";
            Assert.Equal(expectedStatus, check.GetProperty("status").GetString());
        }
        Assert.Contains(tests, check => check.GetProperty("id").GetString() == "test:" + C_INPUT);
        Assert.Contains(tests, check => check.GetProperty("id").GetString() == "test:" + C_UI);
        Assert.Equal(
            "test-execution",
            Assert.Single(process.RootElement.GetProperty("failures").EnumerateArray()).GetProperty("stage").GetString()
        );
    }
}
