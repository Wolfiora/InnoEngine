using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Inno.Tooling.Validation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationExecutionTests
{
    [Fact]
    public async Task UnrelatedCliPayloadDoesNotInvalidateTheValidationToolEnvironment()
    {
        using var repository = new ValidationRepository();
        string unrelated = Path.Combine(Path.GetDirectoryName(typeof(ValidationCommand).Assembly.Location)!,
            "UnrelatedDocumentationPayload-" + Guid.NewGuid().ToString("N") + ".dll");
        try
        {
            File.WriteAllText(unrelated, "first");
            using JsonDocument initial = await repository.PlanAsync();
            DateTime timestamp = File.GetLastWriteTimeUtc(unrelated);
            File.WriteAllText(unrelated, "other");
            File.SetLastWriteTimeUtc(unrelated, timestamp);
            using JsonDocument changed = await repository.PlanAsync();
            Assert.Equal(initial.RootElement.GetProperty("environment").GetString(), changed.RootElement.GetProperty("environment").GetString());
        }
        finally
        {
            File.Delete(unrelated);
        }
    }

    [Fact]
    public async Task ToolSourceInputsFollowEvaluatedReferencesAndPolicyIncludesItsReview()
    {
        using var repository = new ValidationRepository();
        const string C_TOOL = "tools/Inno.Tooling.Validation/Inno.Tooling.Validation.csproj";
        const string C_DEPENDENCY = "src/Example/ToolDependency.cs";
        const string C_UNRELATED = "tools/Inno.Unrelated/Unrelated.cs";
        const string C_REVIEW = ".agents/records/decisions/translation.md";
        repository.Write(C_TOOL, ValidationRepository.Project("<ItemGroup><ProjectReference Include=\"../../src/Example/Example.csproj\" /></ItemGroup>"));
        repository.Write(C_DEPENDENCY, "namespace Example; internal class ToolDependency { }");
        repository.Write("tools/Inno.Unrelated/Inno.Unrelated.csproj", ValidationRepository.Project());
        repository.Write(C_UNRELATED, "namespace Unrelated; internal class Unrelated { }");
        repository.Write(C_REVIEW, "# Reviewed translation\n");
        repository.Write(".agents/rulebook/coverage.json", """
            { "units": [{ "translation": { "review": ".agents/records/decisions/translation.md" } }] }
            """);
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare actual tooling dependencies");
        using JsonDocument plan = await repository.PlanAsync();
        string?[] inputs = plan.RootElement.GetProperty("inputSets").GetProperty("tooling").EnumerateArray()
            .Select(static value => value.GetString()).ToArray();
        Assert.Contains(C_TOOL, inputs);
        Assert.Contains(C_DEPENDENCY, inputs);
        Assert.DoesNotContain(C_UNRELATED, inputs);
        Assert.DoesNotContain(C_REVIEW, inputs);
        JsonElement policy = plan.RootElement.GetProperty("checks").EnumerateArray().Single(check => check.GetProperty("id").GetString() == "instructions");
        Assert.Contains(C_REVIEW, policy.GetProperty("inputs").EnumerateArray().Select(static value => value.GetString()));
    }

    [Fact]
    public async Task FailedContractBlocksItsDependentButIndependentContextStillExecutes()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Independent policy executed'");
        repository.Write("tests/ci/Failure.Tests.ps1", "throw 'Original contract failure'");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare independent verification scripts");
        using JsonDocument generated = await repository.PlanAsync();
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonNode policy = plan["checks"]!.AsArray().Single(check => check!["kind"]!.GetValue<string>() == "instructions")!;
        JsonObject failure = policy.DeepClone().AsObject();
        failure["id"] = "contract-failure";
        failure["kind"] = "ci-contracts";
        failure["inputs"] = new JsonArray("tests/ci/Failure.Tests.ps1");
        JsonObject independent = policy.DeepClone().AsObject();
        independent["id"] = "independent-policy";
        independent["context"] = "managed";
        JsonObject dependent = independent.DeepClone().AsObject();
        dependent["id"] = "dependent-policy";
        dependent["prerequisites"] = new JsonArray("contract-failure");
        plan["checks"] = new JsonArray(failure, independent, dependent);
        repository.Write("artifacts/plan/plan.json", plan.ToJsonString());

        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        using JsonDocument evidence = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/run/results.json")));
        JsonElement[] results = evidence.RootElement.GetProperty("results").EnumerateArray().ToArray();
        Assert.Equal("Failed", results.Single(result => result.GetProperty("id").GetString() == "contract-failure").GetProperty("status").GetString());
        Assert.Contains("Original contract failure", results.Single(result => result.GetProperty("id").GetString() == "contract-failure").GetProperty("detail").GetString());
        Assert.Equal("Passed", results.Single(result => result.GetProperty("id").GetString() == "independent-policy").GetProperty("status").GetString());
        JsonElement blocked = results.Single(result => result.GetProperty("id").GetString() == "dependent-policy");
        Assert.Equal("Blocked", blocked.GetProperty("status").GetString());
        Assert.Contains("contract-failure (Failed)", blocked.GetProperty("detail").GetString());
        Assert.True(evidence.RootElement.GetProperty("completed").GetBoolean());
    }

    [Fact]
    public async Task CompilationDeclaresItsPolicyPrerequisitesInThePlan()
    {
        using var repository = new ValidationRepository();
        repository.Write("src/Example/Changed.cs", "namespace Example; public sealed class Changed { }");
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement[] builds = plan.RootElement.GetProperty("checks").EnumerateArray()
            .Where(check => check.GetProperty("kind").GetString() == "build").ToArray();
        Assert.NotEmpty(builds);
        foreach (JsonElement build in builds)
        {
            string?[] prerequisites = build.GetProperty("prerequisites").EnumerateArray().Select(static value => value.GetString()).ToArray();
            Assert.Contains("instructions", prerequisites);
            Assert.Contains("architecture", prerequisites);
            Assert.DoesNotContain("documentation", prerequisites);
        }
    }
}
