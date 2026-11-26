using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Inno.Testing.Validation;

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
            {
                manifest.Write(
                    "<package><metadata><id>Inno.Fixture.Dependency</id><version>1.0.0</version><authors>Fixture</authors><description>Restored input ownership fixture.</description></metadata></package>"
                );
            }
            using var props = new StreamWriter(archive.CreateEntry("buildTransitive/Inno.Fixture.Dependency.props").Open());
            props.Write("<Project><PropertyGroup><FixtureMarker>before</FixtureMarker></PropertyGroup></Project>");
        }
        using var repository = new ValidationRepository();
        repository.Write(
            "Directory.Build.props",
            "<Project><PropertyGroup><RestoreSources>"
            + SecurityElement.Escape(feed) + "</RestoreSources><RestorePackagesPath>"
            + SecurityElement.Escape(packages) + "</RestorePackagesPath></PropertyGroup></Project>"
        );
        repository.Write(
            "src/Example/Example.csproj",
            ValidationRepository.Project(
                "<ItemGroup><PackageReference Include=\"Inno.Fixture.Dependency\" Version=\"1.0.0\" /></ItemGroup>"
            )
        );
        repository.Write("src/Example/Contract.cs", "namespace Example; public static class Contract { public static int Value => 1; }");
        repository.Write("AGENTS.md", "# Policy passed\n");
        using JsonDocument cold = await repository.PlanAsync(profile: "full");
        JsonObject accepted = SelectCompilation(cold);
        repository.Write("artifacts/cold.json", accepted.ToJsonString());
        Assert.Equal(
            0,
            await ValidationCommand.ExecuteAsync([
                "run",
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--plan",
                repository.PathOf("artifacts/cold.json"),
                "--output",
                repository.PathOf("artifacts/cold")
            ])
        );
        using JsonDocument restored = await repository.PlanAsync(profile: "full");
        JsonObject current = SelectCompilation(restored);
        Assert.Equal(accepted["environment"]!.GetValue<string>(), current["environment"]!.GetValue<string>());
        Assert.Contains(
            restored.RootElement.GetProperty("contexts")
                .GetProperty("managed")
                .GetProperty("sdkGraph")
                .EnumerateArray()
                .SelectMany(static project => project.GetProperty("Imports").EnumerateArray()),
            static import => import.GetString()!.EndsWith("Inno.Fixture.Dependency.props", StringComparison.Ordinal)
        );
        repository.Write("artifacts/restored.json", current.ToJsonString());
        string imported = Path.Combine(packages, "inno.fixture.dependency", "1.0.0", "buildTransitive", "Inno.Fixture.Dependency.props");
        DateTime timestamp = File.GetLastWriteTimeUtc(imported);
        File.WriteAllText(imported, "<Project><PropertyGroup><FixtureMarker>edited</FixtureMarker></PropertyGroup></Project>");
        File.SetLastWriteTimeUtc(imported, timestamp);
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(
            repository.PathOf("artifacts/cold/results.json"),
            repository.PathOf("artifacts/restored.json")
        ));
        static JsonObject SelectCompilation(JsonDocument generated)
        {
            JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
            JsonObject policy = plan["checks"]!
                .AsArray()
                .Single(check => check!["id"]!.GetValue<string>() == "instructions")!
                .DeepClone()
                .AsObject();
            JsonObject build = plan["checks"]!
                .AsArray()
                .Single(check => check!["id"]!.GetValue<string>() == "build:src/Example/Example.csproj")!
                .DeepClone()
                .AsObject();
            build["prerequisites"] = new JsonArray("instructions");
            plan["checks"] = new JsonArray(policy, build);
            plan["shards"] = new JsonArray(new JsonObject
            {
                ["id"] = "managed",
                ["checks"] = new JsonArray("instructions", build["id"]!.GetValue<string>()),
                ["prerequisites"] = new JsonArray()
            });
            return plan;
        }
    }

    [Fact]
    public async Task FrozenPlanBindsAnotherCheckoutWithoutChangingItsIdentity()
    {
        using var producer = new ValidationRepository();
        producer.Write("AGENTS.md", "# Actual policy execution in the consumer checkout\n");
        producer.Git("add", ".");
        producer.Git("commit", "-m", "Portable policy fixture");
        using JsonDocument generated = await producer.PlanAsync();
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonNode policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!.DeepClone();
        plan["checks"] = new JsonArray(policy);
        plan["shards"] = new JsonArray(new JsonObject
        {
            ["id"] = "architecture",
            ["checks"] = new JsonArray("instructions"),
            ["prerequisites"] = new JsonArray()
        });
        using var consumer = new ValidationRepository(producer.PathOf("."));
        // Git checkout filters may change line endings. This transfer promises identical input bytes, not only a commit.
        foreach (JsonProperty input in generated.RootElement.GetProperty("inputs").EnumerateObject())
        {
            if (!input.Name.StartsWith('@') && File.Exists(producer.PathOf(input.Name)))
            {
                consumer.Write(input.Name, File.ReadAllText(producer.PathOf(input.Name)));
            }
        }
        consumer.Write("artifacts/frozen.json", plan.ToJsonString());
        int exit = await ValidationCommand.ExecuteAsync([
            "run",
            "--engine-root",
            consumer.PathOf("."),
            "--dotnet",
            consumer.dotnet,
            "--plan",
            consumer.PathOf("artifacts/frozen.json"),
            "--shard",
            "architecture",
            "--output",
            consumer.PathOf("artifacts/receipt")
        ]);
        Assert.Equal(0, exit);
        using JsonDocument bound = JsonDocument.Parse(File.ReadAllBytes(consumer.PathOf("artifacts/receipt/execution-plan.json")));
        Assert.Equal(consumer.PathOf("."), bound.RootElement.GetProperty("root").GetString());
        Assert.DoesNotContain(producer.PathOf("."), bound.RootElement.GetRawText());
        using JsonDocument receipt = JsonDocument.Parse(File.ReadAllBytes(consumer.PathOf("artifacts/receipt/results.json")));
        Assert.True(receipt.RootElement.GetProperty("completed").GetBoolean());
    }

    [Fact]
    public async Task IndependentManagedExecutionRecordsExcludedNativeChecksWithoutOpeningTheirSdkContext()
    {
        using var repository = new ValidationRepository();
        repository.Write(
            "tests/Native/Native.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><IsTestProject>true</IsTestProject><InnoTestNativeProduct>editor</InnoTestNativeProduct><InnoValidationLayer>native</InnoValidationLayer></PropertyGroup>"
            )
        );
        repository.Write("AGENTS.md", "# Managed change\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare a separately deployed native test");
        using JsonDocument generated = await repository.PlanAsync(profile: "fast", backendStore: "artifacts/b");
        repository.Write("artifacts/packages.json", "[]");
        string frozen = repository.PathOf("artifacts/bound.json");
        Assert.Equal(
            0,
            await ValidationCommand.ExecuteAsync([
                "bind-backends",
                "--packages",
                repository.PathOf("artifacts/packages.json"),
                "--plan",
                repository.PathOf("artifacts/plan/plan.json"),
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--output",
                frozen
            ])
        );
        JsonObject plan = JsonNode.Parse(File.ReadAllText(frozen))!.AsObject();
        Assert.Null(plan["contexts"]!["native"]);
        JsonNode excluded = plan["checks"]!
            .AsArray()
            .Single(check => check!["id"]!.GetValue<string>() == "test:tests/Native/Native.csproj")!;
        Assert.NotNull(excluded["exclusion"]);
        JsonNode policy = plan["checks"]!.AsArray().Single(check => check!["id"]!.GetValue<string>() == "instructions")!;
        plan["checks"] = new JsonArray(policy.DeepClone(), excluded.DeepClone());
        repository.Write("artifacts/selected.json", plan.ToJsonString());
        Assert.Equal(
            0,
            await ValidationCommand.ExecuteAsync([
                "run",
                "--engine-root",
                repository.PathOf("."),
                "--dotnet",
                repository.dotnet,
                "--plan",
                repository.PathOf("artifacts/selected.json"),
                "--output",
                repository.PathOf("artifacts/executed")
            ])
        );
        using JsonDocument evidence = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/executed/results.json")));
        Assert.Equal("NotApplicable", evidence.RootElement.GetProperty("results")[1].GetProperty("status").GetString());
    }

    [Fact]
    public async Task IndependentShardReceiptsRequireCompleteExactCandidateCoverage()
    {
        using var repository = new ValidationRepository();
        repository.Write("AGENTS.md", "# Actual policy execution\n");
        using JsonDocument generated = await repository.PlanAsync();
        JsonObject plan = JsonNode.Parse(generated.RootElement.GetRawText())!.AsObject();
        JsonObject policy = plan["checks"]!
            .AsArray()
            .Single(check => check!["id"]!.GetValue<string>() == "instructions")!
            .DeepClone()
            .AsObject();
        JsonObject dependent = policy.DeepClone().AsObject();
        dependent["id"] = "dependent-policy";
        dependent["context"] = "managed";
        dependent["prerequisites"] = new JsonArray("instructions");
        plan["checks"] = new JsonArray(policy, dependent);
        plan["shards"] = new JsonArray(
            new JsonObject { ["id"] = "architecture", ["checks"] = new JsonArray("instructions"), ["prerequisites"] = new JsonArray() },
            new JsonObject
            {
                ["id"] = "managed",
                ["checks"] = new JsonArray("dependent-policy"),
                ["prerequisites"] = new JsonArray("architecture")
            }
        );
        repository.Write("artifacts/frozen.json", plan.ToJsonString());
        string frozen = repository.PathOf("artifacts/frozen.json");
        await ExecuteAsync("run", "--shard", "architecture", "--output", repository.PathOf("artifacts/receipts/architecture"));
        Assert.Equal("architecture", Read("artifacts/receipts/architecture/results.json")["shard"]!.GetValue<string>());
        await Assert.ThrowsAsync<InvalidDataException>(
            () => ExecuteAsync("run", "--shard", "managed", "--output", repository.PathOf("artifacts/missing"))
        );
        await ExecuteAsync(
            "run",
            "--shard",
            "managed",
            "--prerequisites",
            repository.PathOf("artifacts/receipts"),
            "--output",
            repository.PathOf("artifacts/receipts/managed")
        );
        JsonObject receipt = Read("artifacts/receipts/managed/results.json");
        Assert.Single(receipt["results"]!.AsArray());
        Assert.Equal("Passed", receipt["results"]![0]!["status"]!.GetValue<string>());
        await ExecuteAsync(
            "join",
            "--prerequisites",
            repository.PathOf("artifacts/receipts"),
            "--output",
            repository.PathOf("artifacts/joined")
        );
        JsonObject joined = Read("artifacts/joined/results.json");
        Assert.True(joined["completed"]!.GetValue<bool>());
        Assert.Null(joined["shard"]);
        Assert.Equal(2, joined["results"]!.AsArray().Count);

        foreach (string damage in new[] {
            "commit",
            "environment",
            "planIdentity",
            "shard",
            "failed",
            "missing",
            "duplicate",
            "old-format",
            "run-failure"
        }) {
            JsonObject changed = receipt.DeepClone().AsObject();
            if (damage == "old-format")
            {
                changed.Remove("version");
            }
            else if (damage == "run-failure")
            {
                changed["failures"] = new JsonArray(
                    new JsonObject { ["stage"] = "test-execution", ["detail"] = "Shared test process exited with 1." }
                );
            }
            else if (damage == "failed")
            {
                changed["results"]![0]!["status"] = "Failed";
            }
            else if (damage == "missing")
            {
                changed["results"] = new JsonArray();
            }
            else if (damage == "duplicate")
            {
                changed["results"]!.AsArray().Add(changed["results"]![0]!.DeepClone());
            }
            else
            {
                changed[damage] = "foreign";
            }
            repository.Write("artifacts/receipts/managed/results.json", changed.ToJsonString());
            await Assert.ThrowsAsync<InvalidDataException>(() => ExecuteAsync(
                "join",
                "--prerequisites",
                repository.PathOf("artifacts/receipts"),
                "--output",
                repository.PathOf("artifacts/rejected/" + damage)
            ));
        }

        Task<int> ExecuteAsync(
            string operation,
            params string[] arguments
        ) => ValidationCommand.ExecuteAsync([
            operation,
            "--engine-root",
            repository.PathOf("."),
            "--dotnet",
            repository.dotnet,
            "--plan",
            frozen,
            ..arguments
        ]);

        JsonObject Read(string relative) => JsonNode.Parse(File.ReadAllText(repository.PathOf(relative)))!.AsObject();
    }
}
