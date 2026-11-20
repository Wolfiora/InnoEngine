using Inno.Testing.Validation;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Inno.Tooling.Validation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationFailureExecutionTests
{
    [Fact]
    public async Task FailedContractBlocksItsDependentButIndependentContextStillExecutes()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Independent policy executed'");
        repository.Write("tests/ci/Failure.Tests.ps1", "Write-Output ('x' * 20000)\nthrow 'Original contract failure'");
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
        string detail = results.Single(result => result.GetProperty("id").GetString() == "contract-failure")
            .GetProperty("detail").GetString()!;
        Assert.True(detail.Length < 5000, "An owner receipt must not duplicate the complete process log.");
        string log = repository.PathOf("artifacts/run/Failure.Tests.log");
        Assert.Contains(log, detail);
        Assert.Contains(new string('x', 20000), File.ReadAllText(log));
        Assert.Equal("Passed", results.Single(result => result.GetProperty("id").GetString() == "independent-policy").GetProperty("status").GetString());
        JsonElement blocked = results.Single(result => result.GetProperty("id").GetString() == "dependent-policy");
        Assert.Equal("Blocked", blocked.GetProperty("status").GetString());
        Assert.Contains("contract-failure (Failed)", blocked.GetProperty("detail").GetString());
        Assert.True(evidence.RootElement.GetProperty("completed").GetBoolean());
    }

}
