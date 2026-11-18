using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Inno.Testing.Validation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationShardExecutionTests
{
    [Fact]
    public async Task RestoreKeepsTheToolEnvironmentStableWhileChangedPackageBytesRejectReuse()
    {
        using var dependencies = new ValidationRepository();
        string feed = dependencies.PathOf("feed");
        string packages = dependencies.PathOf("packages");
        Directory.CreateDirectory(feed);
        using (ZipArchive archive = ZipFile.Open(Path.Combine(feed, "Inno.Fixture.Dependency.1.0.0.nupkg"), ZipArchiveMode.Create))
        {
            using (var manifest = new StreamWriter(archive.CreateEntry("Inno.Fixture.Dependency.nuspec").Open()))
                manifest.Write("<package><metadata><id>Inno.Fixture.Dependency</id><version>1.0.0</version><authors>Fixture</authors><description>Restored input ownership fixture.</description></metadata></package>");
            using var props = new StreamWriter(archive.CreateEntry("buildTransitive/Inno.Fixture.Dependency.props").Open());
            props.Write("<Project><PropertyGroup><FixtureMarker>before</FixtureMarker></PropertyGroup></Project>");
        }
        using var repository = new ValidationRepository();
        repository.Write("Directory.Build.props", "<Project><PropertyGroup><RestoreSources>"
            + System.Security.SecurityElement.Escape(feed) + "</RestoreSources><RestorePackagesPath>"
            + System.Security.SecurityElement.Escape(packages) + "</RestorePackagesPath></PropertyGroup></Project>");
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project(
            "<ItemGroup><PackageReference Include=\"Inno.Fixture.Dependency\" Version=\"1.0.0\" /></ItemGroup>"));
        repository.Write("src/Example/Contract.cs", "namespace Example; public static class Contract { public static int Value => 1; }");
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Policy passed'");
        using JsonDocument cold = await repository.PlanAsync(profile: "full");
        JsonObject accepted = SelectCompilation(cold);
        repository.Write("artifacts/cold.json", accepted.ToJsonString());
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["run", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/cold.json"), "--output", repository.PathOf("artifacts/cold")]));
        using JsonDocument restored = await repository.PlanAsync(profile: "full");
        JsonObject current = SelectCompilation(restored);
        Assert.Equal(accepted["environment"]!.GetValue<string>(), current["environment"]!.GetValue<string>());
        Assert.Contains(restored.RootElement.GetProperty("contexts").GetProperty("managed").GetProperty("sdkGraph").EnumerateArray()
            .SelectMany(static project => project.GetProperty("Imports").EnumerateArray()),
            static import => import.GetString()!.EndsWith("Inno.Fixture.Dependency.props", StringComparison.Ordinal));
        repository.Write("artifacts/restored.json", current.ToJsonString());
        Assert.Equal(0, await Resume("artifacts/reused"));
        Assert.False(File.Exists(repository.PathOf("artifacts/reused/managed-build.log")));
        JsonNode reused = JsonNode.Parse(File.ReadAllText(repository.PathOf("artifacts/reused/results.json")))!;
        Assert.Equal("Reused", reused["results"]!.AsArray().Single(result => result!["id"]!.GetValue<string>()
            == "build:src/Example/Example.csproj")!["status"]!.GetValue<string>());

        string imported = Path.Combine(packages, "inno.fixture.dependency", "1.0.0", "buildTransitive", "Inno.Fixture.Dependency.props");
        DateTime timestamp = File.GetLastWriteTimeUtc(imported);
        File.WriteAllText(imported, "<Project><PropertyGroup><FixtureMarker>edited</FixtureMarker></PropertyGroup></Project>");
        File.SetLastWriteTimeUtc(imported, timestamp);
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(
            repository.PathOf("artifacts/reused/results.json"), repository.PathOf("artifacts/restored.json")));
        Assert.Equal(0, await Resume("artifacts/package-changed"));
        Assert.True(File.Exists(repository.PathOf("artifacts/package-changed/managed-build.log")));

        Task<int> Resume(string output) => ValidationCommand.ExecuteAsync(["resume", "--engine-root", repository.PathOf("."),
            "--dotnet", repository.dotnet, "--plan", repository.PathOf("artifacts/restored.json"),
            "--accepted-plan", repository.PathOf("artifacts/cold.json"), "--evidence", repository.PathOf("artifacts/cold/results.json"),
            "--output", repository.PathOf(output)]);

        static JsonObject SelectCompilation(JsonDocument generated)
        {
            JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
            JsonObject policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone().AsObject();
            JsonObject build = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "build:src/Example/Example.csproj")!.DeepClone().AsObject();
            build["prerequisites"] = new JsonArray("instructions");
            plan["checks"] = new JsonArray(policy, build);
            plan["shards"] = new JsonArray(new JsonObject
            {
                ["id"] = "managed", ["checks"] = new JsonArray("instructions", build["id"]!.GetValue<string>()), ["prerequisites"] = new JsonArray()
            });
            return plan;
        }
    }

    [Fact]
    public async Task ChangedExternalSdkInputExecutesCurrentChecksWithoutImportingRetainedOutputs()
    {
        using var sdkInput = new ValidationRepository();
        sdkInput.Write("Environment.props", "<Project><PropertyGroup><FixtureSdk>before</FixtureSdk></PropertyGroup></Project>");
        using var repository = new ValidationRepository();
        repository.Write("Shared.props", "<Project><Import Project=\"" + System.Security.SecurityElement.Escape(sdkInput.PathOf("Environment.props")) + "\" /></Project>");
        repository.Write("tests/ci/Test-AgentRules.ps1", "Add-Content -LiteralPath artifacts/policy-executions.txt -Value executed");
        using JsonDocument before = await repository.PlanAsync(profile: "full");
        JsonObject accepted = SelectPolicy(before);
        repository.Write("artifacts/accepted.json", accepted.ToJsonString());
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["run", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/accepted.json"), "--output", repository.PathOf("artifacts/accepted")]));
        sdkInput.Write("Environment.props", "<Project><PropertyGroup><FixtureSdk>after</FixtureSdk></PropertyGroup></Project>");
        using JsonDocument after = await repository.PlanAsync(profile: "full");
        JsonObject current = SelectPolicy(after);
        Assert.Equal(accepted["tree"]!.GetValue<string>(), current["tree"]!.GetValue<string>());
        Assert.NotEqual(accepted["environment"]!.GetValue<string>(), current["environment"]!.GetValue<string>());
        repository.Write("artifacts/current.json", current.ToJsonString());
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["resume", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/current.json"), "--accepted-plan", repository.PathOf("artifacts/accepted.json"),
            "--evidence", repository.PathOf("artifacts/accepted/results.json"), "--retained-outputs", repository.PathOf("artifacts/unusable-outputs"),
            "--output", repository.PathOf("artifacts/resumed")]));
        Assert.Equal(2, File.ReadAllLines(repository.PathOf("artifacts/policy-executions.txt")).Length);
        JsonNode result = JsonNode.Parse(File.ReadAllText(repository.PathOf("artifacts/resumed/results.json")))!;
        Assert.Equal("Passed", result["results"]![0]!["status"]!.GetValue<string>());

        static JsonObject SelectPolicy(JsonDocument generated)
        {
            JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
            JsonObject policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone().AsObject();
            policy["reusable"] = true;
            plan["checks"] = new JsonArray(policy);
            plan["shards"] = new JsonArray(new JsonObject
            {
                ["id"] = "architecture", ["checks"] = new JsonArray("instructions"), ["prerequisites"] = new JsonArray()
            });
            return plan;
        }
    }

    [Fact]
    public async Task ContinuationRetainsARealCompiledUnionWithoutInvokingBuildAgain()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Policy passed'");
        repository.Write("tests/ci/Compilation.Tests.ps1", "Write-Output 'Compilation fixture policy passed'");
        repository.Write("src/Example/Example.csproj", ValidationRepository.Project("""
            <PropertyGroup><OutputType>Exe</OutputType></PropertyGroup>
            <ItemGroup>
              <AssemblyMetadata Include="Inno.Testing.RepositoryRoot" Value="$([System.IO.Path]::GetFullPath('$(MSBuildProjectDirectory)/../..'))" />
              <AssemblyMetadata Include="Inno.Testing.ArtifactsPath" Value="$(ArtifactsPath)" />
            </ItemGroup>
            """));
        repository.Write("src/Example/TestRepository.cs", File.ReadAllText(Path.Combine(
            TestRepository.GetRoot(typeof(ValidationShardExecutionTests).Assembly), "tests/source/Inno.Testing.Source/TestRepository.cs")));
        repository.Write("src/Example/Program.cs", """
            using System;
            using System.Reflection;
            using Inno.Testing.Source;
            Console.WriteLine(TestRepository.GetRoot(Assembly.GetExecutingAssembly()));
            Console.WriteLine(TestRepository.GetArtifactsPath(Assembly.GetExecutingAssembly()));
            """);
        const string C_EXISTING_PROJECT = "tests/Example.Tests/Example.Tests.csproj";
        repository.Write(C_EXISTING_PROJECT, File.ReadAllText(repository.PathOf(C_EXISTING_PROJECT)).ReplaceLineEndings("\r\n"));
        repository.Write(".gitattributes", "* -text\n");
        // Existing fixture files entered the index before this byte-preserving checkout policy.
        repository.Git("add", "--renormalize", ".");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare portable compilation fixture inputs");
        using JsonDocument generated = await repository.PlanAsync(profile: "full");
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonObject policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone().AsObject();
        JsonObject build = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "build:src/Example/Example.csproj")!.DeepClone().AsObject();
        build["prerequisites"] = new JsonArray("instructions");
        plan["checks"] = new JsonArray(policy, build);
        plan["shards"] = new JsonArray(new JsonObject
        {
            ["id"] = "managed", ["checks"] = new JsonArray("instructions", build["id"]!.GetValue<string>()), ["prerequisites"] = new JsonArray()
        });
        repository.Write("artifacts/accepted.json", plan.ToJsonString());
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["run", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/accepted.json"), "--output", repository.PathOf("artifacts/accepted")]));
        Assert.True(File.Exists(repository.PathOf("artifacts/accepted/managed-build.log")));
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["resume", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/accepted.json"), "--accepted-plan", repository.PathOf("artifacts/accepted.json"),
            "--evidence", repository.PathOf("artifacts/accepted/results.json"), "--output", repository.PathOf("artifacts/resumed")]));
        Assert.False(File.Exists(repository.PathOf("artifacts/resumed/managed-build.log")));
        JsonNode evidence = JsonNode.Parse(File.ReadAllText(repository.PathOf("artifacts/resumed/results.json")))!;
        Assert.Equal("Reused", evidence["results"]!.AsArray().Single(result => result!["id"]!.GetValue<string>() == build["id"]!.GetValue<string>())!["status"]!.GetValue<string>());
        await Assert.ThrowsAsync<InvalidDataException>(() => ValidationCommand.ExecuteAsync(["resume", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/accepted.json"), "--accepted-plan", repository.PathOf("artifacts/accepted.json"),
            "--evidence", repository.PathOf("artifacts/accepted/results.json"), "--retained-outputs", repository.PathOf("artifacts/accepted"),
            "--output", repository.PathOf("artifacts/refuse-overwrite")]));
        Assert.False(File.Exists(repository.PathOf("artifacts/refuse-overwrite/managed-build.log")));
        using var consumer = new ValidationRepository(repository.PathOf("."));
        consumer.Write("artifacts/accepted.json", plan.ToJsonString());
        consumer.Write("artifacts/evidence.json", File.ReadAllText(repository.PathOf("artifacts/accepted/results.json")));
        string originalOutputs = repository.BindWorkspace(plan["artifacts"]!.GetValue<string>());
        foreach (string file in Directory.EnumerateFiles(originalOutputs, "*", SearchOption.AllDirectories))
        {
            string destination = consumer.PathOf("artifacts/retained/" + Path.GetRelativePath(originalOutputs, file));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(file, destination);
        }
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["resume", "--engine-root", consumer.PathOf("."), "--dotnet", consumer.dotnet,
            "--plan", consumer.PathOf("artifacts/accepted.json"), "--accepted-plan", consumer.PathOf("artifacts/accepted.json"),
            "--evidence", consumer.PathOf("artifacts/evidence.json"), "--retained-outputs", consumer.PathOf("artifacts/retained"),
            "--output", consumer.PathOf("artifacts/resumed")]));
        Assert.True(File.Exists(consumer.PathOf("artifacts/resumed/managed-restore-retained.log")));
        Assert.False(File.Exists(consumer.PathOf("artifacts/resumed/managed-build.log")));
        string outputs = consumer.BindWorkspace(plan["artifacts"]!.GetValue<string>());
        string assembly = consumer.BindWorkspace(plan["contexts"]!["managed"]!["sdkGraph"]!.AsArray()
            .Single(node => node!["Path"]!.GetValue<string>().Replace('\\', '/').EndsWith("/src/Example/Example.csproj", StringComparison.Ordinal))!["Properties"]!["TargetPath"]!.GetValue<string>());
        string actual = await ToolchainEnvironment.CaptureOutputAsync(consumer.dotnet, [assembly], consumer.PathOf("."), CancellationToken.None,
            new Dictionary<string, string?>
            {
                ["INNO_TESTING_REPOSITORY_ROOT"] = consumer.PathOf("."),
                ["INNO_TESTING_ARTIFACTS_PATH"] = outputs
            });
        Assert.Equal(new[] { consumer.PathOf("."), outputs }, actual.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
        string originalAssembly = repository.BindWorkspace(plan["contexts"]!["managed"]!["sdkGraph"]!.AsArray()
            .Single(node => node!["Path"]!.GetValue<string>().Replace('\\', '/').EndsWith("/src/Example/Example.csproj", StringComparison.Ordinal))!["Properties"]!["TargetPath"]!.GetValue<string>());
        string independent = await ToolchainEnvironment.CaptureOutputAsync(repository.dotnet, [originalAssembly], repository.PathOf("."), CancellationToken.None,
            new Dictionary<string, string?>
            {
                ["INNO_TESTING_REPOSITORY_ROOT"] = consumer.PathOf("."),
                ["INNO_TESTING_ARTIFACTS_PATH"] = outputs
            });
        Assert.Equal(new[] { repository.PathOf("."), originalOutputs }, independent.Split(['\r', '\n'], StringSplitOptions.RemoveEmptyEntries));
    }

    [Fact]
    public async Task ContinuationExecutesOnlyMissingPolicyAndRejectsForeignEvidence()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", "Add-Content -LiteralPath artifacts/policy-executions.txt -Value executed");
        using JsonDocument generated = await repository.PlanAsync(profile: "full");
        JsonObject accepted = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonObject policy = accepted["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone().AsObject();
        policy["reusable"] = true;
        accepted["checks"] = new JsonArray(policy);
        accepted["profile"] = 2;
        accepted["full"] = false;
        accepted["shards"] = new JsonArray(new JsonObject
        {
            ["id"] = "architecture", ["checks"] = new JsonArray("instructions"), ["prerequisites"] = new JsonArray()
        });
        repository.Write("artifacts/accepted.json", accepted.ToJsonString());
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["run", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/accepted.json"), "--output", repository.PathOf("artifacts/accepted")]));
        JsonObject current = accepted.DeepClone().AsObject();
        current["profile"] = 3;
        current["full"] = true;
        JsonObject missing = policy.DeepClone().AsObject();
        missing["id"] = "missing-policy";
        missing["prerequisites"] = new JsonArray("instructions");
        current["checks"]!.AsArray().Add(missing);
        current["shards"]![0]!["checks"]!.AsArray().Add("missing-policy");
        repository.Write("artifacts/current.json", current.ToJsonString());
        Assert.Equal(0, await Resume("artifacts/accepted/results.json", "artifacts/resumed"));
        JsonNode evidence = JsonNode.Parse(File.ReadAllText(repository.PathOf("artifacts/resumed/results.json")))!;
        Assert.Equal("Reused", evidence["results"]![0]!["status"]!.GetValue<string>());
        Assert.Equal("Passed", evidence["results"]![1]!["status"]!.GetValue<string>());
        Assert.Equal(2, File.ReadAllLines(repository.PathOf("artifacts/policy-executions.txt")).Length);
        JsonNode foreign = JsonNode.Parse(File.ReadAllText(repository.PathOf("artifacts/accepted/results.json")))!;
        foreign["commit"] = new string('f', 40);
        repository.Write("artifacts/foreign.json", foreign.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(() => Resume("artifacts/foreign.json", "artifacts/rejected"));
        Assert.Equal(2, File.ReadAllLines(repository.PathOf("artifacts/policy-executions.txt")).Length);

        Task<int> Resume(
            string evidencePath,
            string output
        ) => ValidationCommand.ExecuteAsync(["resume", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/current.json"), "--accepted-plan", repository.PathOf("artifacts/accepted.json"),
            "--evidence", repository.PathOf(evidencePath), "--output", repository.PathOf(output)]);
    }

    [Fact]
    public async Task CompleteAcceptanceRequiresTheSameEnvironmentInputsAndCheckDefinitions()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Actual policy execution'");
        using JsonDocument generated = await repository.PlanAsync(profile: "full");
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonObject policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone().AsObject();
        plan["checks"] = new JsonArray(policy);
        plan["shards"] = new JsonArray(new JsonObject
        {
            ["id"] = "architecture", ["checks"] = new JsonArray("instructions"), ["prerequisites"] = new JsonArray()
        });
        repository.Write("artifacts/accepted-plan.json", plan.ToJsonString());
        Assert.Equal(0, await ValidationCommand.ExecuteAsync(["run", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
            "--plan", repository.PathOf("artifacts/accepted-plan.json"), "--output", repository.PathOf("artifacts/accepted")]));
        JsonObject receipt = JsonNode.Parse(File.ReadAllText(repository.PathOf("artifacts/accepted/results.json")))!.AsObject();
        foreach (string change in new[] { "none", "environment", "tree", "commit", "checks", "configuration", "profile", "extended", "missing-result" })
        {
            JsonObject current = plan.DeepClone().AsObject();
            if (change is "environment" or "tree" or "commit" or "configuration")
                current[change] = "different";
            else if (change == "checks")
                current["checks"]![0]!["reason"] = "Changed check contract";
            else if (change == "profile")
                current["profile"] = 4;
            else if (change == "extended")
                current["extended"] = true;
            repository.Write("artifacts/current-plan.json", current.ToJsonString());
            JsonObject evidence = receipt.DeepClone().AsObject();
            if (change == "missing-result")
                evidence["results"] = new JsonArray();
            repository.Write("artifacts/evidence.json", evidence.ToJsonString());
            Task<int> Compare() => ValidationCommand.ExecuteAsync(["acceptance", "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
                "--plan", repository.PathOf("artifacts/current-plan.json"), "--accepted-plan", repository.PathOf("artifacts/accepted/plan.json"),
                "--evidence", repository.PathOf("artifacts/evidence.json"), "--output", repository.PathOf("artifacts/comparison/" + change)]);
            if (change == "missing-result")
                await Assert.ThrowsAsync<InvalidDataException>(Compare);
            else
            {
                Assert.Equal(0, await Compare());
                using JsonDocument decision = JsonDocument.Parse(File.ReadAllText(repository.PathOf("artifacts/comparison/" + change + "/acceptance.json")));
                Assert.Equal(change == "none", decision.RootElement.GetProperty("reusable").GetBoolean());
            }
        }
    }

    [Fact]
    public async Task FrozenPlanBindsAnotherCheckoutWithoutChangingItsIdentity()
    {
        using var producer = new ValidationRepository();
        producer.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Actual policy execution in the consumer checkout'");
        producer.Git("add", ".");
        producer.Git("commit", "-m", "Portable policy fixture");
        using JsonDocument generated = await producer.PlanAsync();
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonNode policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone();
        plan["checks"] = new JsonArray(policy);
        plan["shards"] = new JsonArray(new JsonObject
        {
            ["id"] = "architecture", ["checks"] = new JsonArray("instructions"), ["prerequisites"] = new JsonArray()
        });
        using var consumer = new ValidationRepository(producer.PathOf("."));
        // Git checkout filters may change line endings. This transfer promises identical input bytes, not only a commit.
        foreach (JsonProperty input in generated.RootElement.GetProperty("inputs").EnumerateObject())
            if (!input.Name.StartsWith('@') && File.Exists(producer.PathOf(input.Name)))
                consumer.Write(input.Name, File.ReadAllText(producer.PathOf(input.Name)));
        consumer.Write("artifacts/frozen.json", plan.ToJsonString());
        int exit = await ValidationCommand.ExecuteAsync(["run", "--engine-root", consumer.PathOf("."), "--dotnet", consumer.dotnet,
            "--plan", consumer.PathOf("artifacts/frozen.json"), "--shard", "architecture", "--output", consumer.PathOf("artifacts/receipt")]);
        Assert.Equal(0, exit);
        using JsonDocument bound = JsonDocument.Parse(File.ReadAllBytes(consumer.PathOf("artifacts/receipt/execution-plan.json")));
        Assert.Equal(consumer.PathOf("."), bound.RootElement.GetProperty("root").GetString());
        Assert.DoesNotContain(producer.PathOf("."), bound.RootElement.GetRawText());
        using JsonDocument receipt = JsonDocument.Parse(File.ReadAllBytes(consumer.PathOf("artifacts/receipt/results.json")));
        Assert.True(receipt.RootElement.GetProperty("completed").GetBoolean());
    }

    [Fact]
    public async Task IndependentShardReceiptsRequireCompleteExactCandidateCoverage()
    {
        using var repository = new ValidationRepository();
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Actual policy execution'");
        using JsonDocument generated = await repository.PlanAsync();
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonObject policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone().AsObject();
        JsonObject dependent = policy.DeepClone().AsObject();
        dependent["id"] = "dependent-policy";
        dependent["context"] = "managed";
        dependent["prerequisites"] = new JsonArray("instructions");
        plan["checks"] = new JsonArray(policy, dependent);
        plan["shards"] = new JsonArray(
            new JsonObject { ["id"] = "architecture", ["checks"] = new JsonArray("instructions"), ["prerequisites"] = new JsonArray() },
            new JsonObject { ["id"] = "managed", ["checks"] = new JsonArray("dependent-policy"), ["prerequisites"] = new JsonArray("architecture") });
        repository.Write("artifacts/frozen.json", plan.ToJsonString());
        string frozen = repository.PathOf("artifacts/frozen.json");
        await Execute("run", "--shard", "architecture", "--output", repository.PathOf("artifacts/receipts/architecture"));
        Assert.Equal("architecture", Read("artifacts/receipts/architecture/results.json")["shard"]!.GetValue<string>());
        await Assert.ThrowsAsync<InvalidDataException>(() => Execute("run", "--shard", "managed", "--output", repository.PathOf("artifacts/missing")));
        await Execute("run", "--shard", "managed", "--prerequisites", repository.PathOf("artifacts/receipts"),
            "--output", repository.PathOf("artifacts/receipts/managed"));
        JsonObject receipt = Read("artifacts/receipts/managed/results.json");
        Assert.Single(receipt["results"]!.AsArray());
        Assert.Equal("Passed", receipt["results"]![0]!["status"]!.GetValue<string>());
        await Execute("join", "--prerequisites", repository.PathOf("artifacts/receipts"), "--output", repository.PathOf("artifacts/joined"));
        JsonObject joined = Read("artifacts/joined/results.json");
        Assert.True(joined["completed"]!.GetValue<bool>());
        Assert.Null(joined["shard"]);
        Assert.Equal(2, joined["results"]!.AsArray().Count);

        foreach (string damage in new[] { "commit", "environment", "planIdentity", "shard", "failed", "missing", "duplicate" })
        {
            JsonObject changed = receipt.DeepClone().AsObject();
            if (damage == "failed")
                changed["results"]![0]!["status"] = "Failed";
            else if (damage == "missing")
                changed["results"] = new JsonArray();
            else if (damage == "duplicate")
                changed["results"]!.AsArray().Add(changed["results"]![0]!.DeepClone());
            else
                changed[damage] = "foreign";
            repository.Write("artifacts/receipts/managed/results.json", changed.ToJsonString());
            await Assert.ThrowsAsync<InvalidDataException>(() => Execute("join", "--prerequisites", repository.PathOf("artifacts/receipts"),
                "--output", repository.PathOf("artifacts/rejected/" + damage)));
        }

        Task<int> Execute(
            string operation,
            params string[] arguments
        )
            => ValidationCommand.ExecuteAsync([operation, "--engine-root", repository.PathOf("."), "--dotnet", repository.dotnet,
                "--plan", frozen, ..arguments]);

        JsonObject Read(string relative) => JsonNode.Parse(File.ReadAllText(repository.PathOf(relative)))!.AsObject();
    }
}
