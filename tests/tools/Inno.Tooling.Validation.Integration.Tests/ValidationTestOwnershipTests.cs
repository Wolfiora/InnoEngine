using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Integration.Tests;

public sealed class ValidationTestOwnershipTests
{
    [Fact]
    public async Task RealTestFailureReachesTheFinalCheckWithItsOriginalMessage()
    {
        using ValidationRepository repository = await ManagedValidationFixture.CreateAsync();
        repository.Write("tests/services/input/Inno.Input.Tests/Probe.cs", """
            using Xunit;

            namespace Inno.Input.Tests;

            public sealed class Probe
            {
                [Fact]
                public void ReportsTheOriginalFailure() => Assert.Fail("Intentional engine test failure");
            }

            """);
        using JsonDocument selected = await repository.PlanAsync();
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument results = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        Assert.True(
            results.RootElement.GetProperty("failures").GetArrayLength() > 0,
            string.Join(
                Environment.NewLine,
                results.RootElement.GetProperty("results")
                    .EnumerateArray()
                    .Where(item => item.GetProperty("status").GetString() == "Failed")
                    .Select(item => item.GetProperty("detail").GetString())
            )
        );
        Assert.Equal(
            "test-execution",
            Assert.Single(results.RootElement.GetProperty("failures").EnumerateArray()).GetProperty("stage").GetString()
        );
        JsonElement failed = results.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Single(item => item.GetProperty("id").GetString() == "test:" + ManagedValidationFixture.C_TEST);
        Assert.Equal("Failed", failed.GetProperty("status").GetString());
        Assert.Contains("Intentional engine test failure", failed.GetProperty("detail").GetString());
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));
    }
}
