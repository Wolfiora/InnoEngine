using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Source;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Integration.Tests;

public sealed class ValidationTestOwnershipTests
{
    [Fact]
    public async Task RealTestFailureReachesTheFinalCheckWithItsOriginalMessage()
    {
        using var repository = new ValidationRepository();
        string source = TestRepository.GetRoot(typeof(ValidationTestOwnershipTests).Assembly);
        const string C_LOGGER = "tests/support/Inno.Testing.Discovery.TestLogger/";
        foreach (string path in new[]
        {
            "tools/ci/Invoke-IsolatedTests.ps1",
            C_LOGGER + "DiscoveryLogger.cs",
            C_LOGGER + "Inno.Testing.Discovery.TestLogger.csproj"
        }
        ) {
            repository.Write(path, File.ReadAllText(Path.Combine(source, path)));
        }
        const string C_PROJECT = "tests/Example.Tests/Example.Tests.csproj";
        repository.Write(
            C_PROJECT,
            ValidationRepository.Project("""
            <PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>domain</InnoValidationLayer></PropertyGroup>
            <ItemGroup>
              <ProjectReference Include="../support/Inno.Testing.Discovery.TestLogger/Inno.Testing.Discovery.TestLogger.csproj" ReferenceOutputAssembly="false" />
              <PackageReference Include="Microsoft.NET.Test.Sdk" Version="17.12.0" />
              <PackageReference Include="xunit" Version="2.6.6" />
              <PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
            </ItemGroup>
            """)
        );
        repository.Write("tools/ci/Test-AgentRules.ps1", "Write-Output 'Fixture policy passed'");
        repository.Write("tests/ci/Fixture.Tests.ps1", "Write-Output 'Fixture ready'");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Register the minimal test host");
        repository.Write("tests/Example.Tests/Probe.cs", """
            using Xunit;
            public sealed class Probe
            {
                [Fact]
                public void ReportsTheOriginalFailure() => Assert.Fail("Intentional engine test failure");
            }
            """);
        using JsonDocument selected = await repository.PlanAsync();
        JsonObject plan = JsonNode.Parse(selected.RootElement.GetRawText())!.AsObject();
        // The fixture exercises the actual test runner with a passing repository policy prerequisite.
        JsonNode architecture = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "architecture")!;
        architecture["kind"] = "ci-contracts";
        architecture["projects"] = new JsonArray("tests/ci/Fixture.Tests.ps1");
        repository.Write("artifacts/plan/plan.json", plan.ToJsonString());
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
            .Single(item => item.GetProperty("id").GetString() == "test:" + C_PROJECT);
        Assert.Equal("Failed", failed.GetProperty("status").GetString());
        Assert.Contains("Intentional engine test failure", failed.GetProperty("detail").GetString());
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(repository.PathOf("artifacts/run/results.json")));
    }
}
