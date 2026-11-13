using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Inno.Tooling.Documentation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationDocumentationTests
{
    [Fact]
    public async Task ProjectedReferenceChangesSelectTheirCompilerAndIgnoreUnrelatedBatchOwners()
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the actual projection hosts");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        docs.Write("content/en/script.md", ValidationDocumentationFixture.Page("Example", "```csharp\nint value = 1;\n```\n", "runtime-script"));
        string root = repository.PathOf("artifacts/docs-source");
        JsonNode inventory = JsonNode.Parse(File.ReadAllBytes(Path.Combine(root, "reference/projects/catalog.json")))!;
        inventory.AsArray().Single(owner => owner!["name"]!.GetValue<string>() == "Example")!["surfaces"]!.AsArray().Add("runtime-script");
        byte[] inventoryBytes = JsonSerializer.SerializeToUtf8Bytes(inventory);
        docs.Write("reference/projects/catalog.json", Encoding.UTF8.GetString(inventoryBytes));
        JsonNode binding = JsonNode.Parse(File.ReadAllBytes(Path.Combine(root, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Convert.ToHexStringLower(SHA256.HashData(inventoryBytes));
        binding["apiManifests"]!.AsArray().Add(JsonSerializer.SerializeToNode(new
        {
            path = "reference/api/scripts.json", model = "runtime-script", configuration = "Debug",
            sourceRevision = new string('a', 40), sourceTree = new string('b', 40), sha256 = ""
        }));
        BindProjection("before");
        DocumentationResult baseline = await docs.IndexAsync();
        BindProjection("after");
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        JsonElement check = plan.RootElement.GetProperty("checks").EnumerateArray()
            .Single(candidate => candidate.GetProperty("id").GetString() == "documentation-examples:native");
        Assert.Contains("build:entry/InnoEngine.Build/InnoEngine.Build.csproj", check.GetProperty("prerequisites").EnumerateArray().Select(static value => value.GetString()));
        Assert.Contains("reference/api/scripts.json", check.GetProperty("documentation").GetProperty("references").EnumerateArray().Select(static value => value.GetString()));
        string identity = check.GetProperty("documentation").GetProperty("contentIdentity").GetString()!;
        JsonNode sourceFacts = JsonNode.Parse(File.ReadAllBytes(Path.Combine(root, "reference/api/facts.json")))!;
        sourceFacts["projects"]!.AsArray().Single(owner => owner!["name"]!.GetValue<string>() == "Other")!["symbols"]!.AsArray()
            .Add(JsonSerializer.SerializeToNode(new { id = "T:Other.Added" }));
        byte[] factsBytes = JsonSerializer.SerializeToUtf8Bytes(sourceFacts);
        docs.Write("reference/api/facts.json", Encoding.UTF8.GetString(factsBytes));
        binding["apiManifests"]![0]!["sha256"] = Convert.ToHexStringLower(SHA256.HashData(factsBytes));
        docs.Write("reference/engine.lock.json", binding.ToJsonString());
        using JsonDocument unrelated = await docs.PlanAsync(await docs.IndexAsync(), baseline);
        JsonElement same = unrelated.RootElement.GetProperty("checks").EnumerateArray()
            .Single(candidate => candidate.GetProperty("id").GetString() == "documentation-examples:native");
        Assert.Equal(identity, same.GetProperty("documentation").GetProperty("contentIdentity").GetString());

        void BindProjection(string signature)
        {
            byte[] facts = JsonSerializer.SerializeToUtf8Bytes(new
            {
                model = "runtime-script", configuration = "Debug", projects = new[]
                {
                    new { project = "src/Example/Example.csproj", name = "Example", symbols = new[] { new { id = "T:Logical.Example", signature } } }
                }
            });
            docs.Write("reference/api/scripts.json", Encoding.UTF8.GetString(facts));
            binding["apiManifests"]![1]!["sha256"] = Convert.ToHexStringLower(SHA256.HashData(facts));
            docs.Write("reference/engine.lock.json", binding.ToJsonString());
        }
    }

    [Theory]
    [InlineData(".agents/records/acceptance/native-reload.json", false)]
    [InlineData(".agents/records/plans/work.md", false)]
    [InlineData(".agents/skills/inno-change/SKILL.md", false)]
    [InlineData(".agents/rulebook/coverage.json", true)]
    public async Task AgentTextChangesKeepProductAndInstructionEvidenceSeparate(
        string path,
        bool policyInput
    ) {
        using var repository = new ValidationRepository();
        repository.Write("tests/Native.Tests/Native.Tests.csproj", ValidationRepository.Project(
            "<PropertyGroup><IsTestProject>true</IsTestProject><InnoValidationLayer>native</InnoValidationLayer><InnoTestNativeProduct>editor</InnoTestNativeProduct></PropertyGroup>"));
        repository.Write(path, policyInput ? "{\"units\":[],\"review\":\"before\"}" : "before");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare agent document and native sentinel");
        repository.Write(path, policyInput ? "{\"units\":[],\"review\":\"after!\"}" : "after!");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.False(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build");
        Assert.Contains(checks, check => check.GetProperty("id").GetString() == "agent-documents");
        Assert.DoesNotContain(checks, check => check.GetProperty("id").GetString() == "documentation");
        JsonElement instructions = checks.Single(check => check.GetProperty("id").GetString() == "instructions");
        Assert.Equal(policyInput, instructions.GetProperty("inputs").EnumerateArray().Any(input => input.GetString() == path));
        Assert.DoesNotContain(plan.RootElement.GetProperty("inputSets").GetProperty("tooling").EnumerateArray(), input => input.GetString() == path);
    }

    [Fact]
    public async Task MalformedRuleRegistryCannotProduceAPlan()
    {
        using var repository = new ValidationRepository();
        repository.Write(".agents/rulebook/coverage.json", "not a registry");
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.PlanAsync());
        Assert.Contains(".agents/rulebook/coverage.json", failure.Message);
        Assert.IsAssignableFrom<JsonException>(failure.InnerException);
        Assert.False(File.Exists(repository.PathOf("artifacts/plan/plan.json")));
    }

    [Fact]
    public async Task CanonicalFragmentOwnersSelectOnlyTheirDeclaredExampleContext()
    {
        using var repository = new ValidationRepository();
        DocumentationResult snapshot = await PrepareDocumentationAsync(repository);
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }");
        using JsonDocument plan = await repository.PlanAsync(documentationCache: repository.PathOf("artifacts/documentation"), documentationSnapshot: snapshot.snapshot);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.Contains(checks, check => check.GetProperty("kind").GetString() == "build"
            && check.GetProperty("owner").GetString() == "src/Example/Example.csproj");
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build"
            && check.GetProperty("owner").GetString() == "src/Other/Other.csproj");
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "wiki");
        JsonElement examples = checks.Single(check => check.GetProperty("kind").GetString() == "documentation-examples");
        Assert.Equal("task-fragment", Assert.Single(examples.GetProperty("documentation").GetProperty("pages").EnumerateArray()).GetString());
        JsonElement api = checks.Single(check => check.GetProperty("kind").GetString() == "documentation-api");
        Assert.Equal("src/Example/Example.csproj", Assert.Single(api.GetProperty("projects").EnumerateArray()).GetString());
        Assert.False(plan.RootElement.GetProperty("contexts").TryGetProperty("native", out _));
        File.Delete(repository.PathOf("artifacts/docs-source/content/en/random/behavior.md"));
        using JsonDocument retained = await repository.PlanAsync(documentationCache: repository.PathOf("artifacts/documentation"), documentationSnapshot: snapshot.snapshot);
        Assert.Equal(snapshot.snapshot, retained.RootElement.GetProperty("documentation").GetProperty("snapshot").GetString());
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationStore.IndexAsync(repository.PathOf("."),
            repository.PathOf("artifacts/docs-source"), repository.PathOf("artifacts/documentation")));
    }

    [Fact]
    public async Task SharedHostAndScriptExamplesUseOnePreparedContextWithoutPromotingUnrelatedExamples()
    {
        using var repository = new ValidationRepository();
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Build/InnoEngine.Build.csproj", ValidationRepository.Project());
        repository.Write("src/Independent/Independent.csproj", ValidationRepository.Project());
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>editor</InnoProductId><InnoDevelopmentTarget>fixture-target</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare independently prepared example owners");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        docs.Write("content/en/script.md", ValidationDocumentationFixture.Page("Example", "```csharp\nint value = 1;\n```\n", "runtime-script"));
        docs.Write("content/en/host.md", ValidationDocumentationFixture.Page("Example", "```csharp\nint value = 1;\n```\n", "host")
            .Replace("uid: task-example", "uid: task-host", StringComparison.Ordinal));
        docs.Write("content/en/shared.md", ValidationDocumentationFixture.Page("Other", "```csharp\nint value = 1;\n```\n", "host")
            .Replace("exampleProjects: [Other]", "exampleProjects: [Other, Example]", StringComparison.Ordinal));
        docs.Write("content/en/independent.md", ValidationDocumentationFixture.Page("Independent", "```csharp\nint value = 1;\n```\n", "host"));
        using JsonDocument plan = await docs.PlanAsync(await docs.IndexAsync(), baseline);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement native = checks.Single(check => check.GetProperty("id").GetString() == "documentation-examples:native");
        Assert.Equal(new[] { "task-example", "task-host", "task-other" }, native.GetProperty("documentation").GetProperty("pages").EnumerateArray().Select(page => page.GetString()).Order().ToArray());
        JsonElement managed = checks.Single(check => check.GetProperty("id").GetString() == "documentation-examples:managed");
        Assert.Equal("task-independent", Assert.Single(managed.GetProperty("documentation").GetProperty("pages").EnumerateArray()).GetString());
        foreach (string owner in new[] { "Example", "Other", "Independent" })
            Assert.Equal(owner == "Independent" ? "managed" : "native", checks.Single(check => check.GetProperty("id").GetString() == $"build:src/{owner}/{owner}.csproj").GetProperty("context").GetString());
        var preceding = new System.Collections.Generic.HashSet<string>(StringComparer.Ordinal);
        foreach (JsonElement check in checks)
        {
            if (check.GetProperty("exclusion").ValueKind == JsonValueKind.Null)
                Assert.All(check.GetProperty("prerequisites").EnumerateArray(), prerequisite => Assert.Contains(prerequisite.GetString()!, preceding));
            preceding.Add(check.GetProperty("id").GetString()!);
        }
    }

    [Fact]
    public async Task EmptyForeignProductSurfaceDoesNotClaimForeignExecutionOrRequireItsBuild()
    {
        using var repository = new ValidationRepository();
        const string C_FOREIGN = "platforms/Foreign/player/Foreign.Player/Foreign.Player.csproj";
        repository.Write("Product.props", "<Project />");
        repository.Write("entry/InnoEngine.Editor/InnoEngine.Editor.csproj", ValidationRepository.Project(
            "<PropertyGroup><InnoDevelopmentTarget>host-fixture</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
            + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"));
        repository.Write(C_FOREIGN, ValidationRepository.Project(
            "<PropertyGroup><InnoProductId>player</InnoProductId><InnoProductTarget>foreign-fixture</InnoProductTarget></PropertyGroup>"));
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare an internal foreign product entry");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        repository.Write("platforms/Foreign/player/Foreign.Player/Entry.cs", "namespace Foreign; internal static class Entry { }");
        repository.Write("src/Example/Changed.cs", "namespace Example; internal class Changed { }");
        using JsonDocument plan = await docs.PlanAsync(await docs.IndexAsync());
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement source = checks.Single(check => check.GetProperty("id").GetString() == "documentation-api:source:default");
        Assert.Equal(C_FOREIGN, Assert.Single(source.GetProperty("projects").EnumerateArray()).GetString());
        Assert.Equal("documentation", Assert.Single(source.GetProperty("prerequisites").EnumerateArray()).GetString());
        Assert.DoesNotContain("@" + C_FOREIGN, source.GetProperty("inputs").EnumerateArray().Select(value => value.GetString()));
        Assert.Contains(C_FOREIGN, source.GetProperty("inputs").EnumerateArray().Select(value => value.GetString()));
        JsonElement foreignBuild = checks.Single(check => check.GetProperty("id").GetString() == "build:" + C_FOREIGN);
        Assert.Contains("requires its own supported target", foreignBuild.GetProperty("exclusion").GetString());
        Assert.Contains(checks, check => check.GetProperty("id").GetString() == "test:tests/Example.Tests/Example.Tests.csproj"
            && check.GetProperty("exclusion").ValueKind == JsonValueKind.Null);
    }

    [Fact]
    public async Task EngineChangesDoNotRequireADocumentationCandidateOrReadAnOldWikiTree()
    {
        using var repository = new ValidationRepository();
        repository.Write("docs/Example.md", "# Example\nOld guidance cannot certify the external candidate.");
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }");
        using JsonDocument plan = await repository.PlanAsync();
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString()!.StartsWith("documentation", StringComparison.Ordinal));
        Assert.Contains(checks, check => check.GetProperty("kind").GetString() == "build");
        Assert.Contains(checks, check => check.GetProperty("kind").GetString() == "api");
        await Assert.ThrowsAsync<ArgumentException>(() => repository.PlanAsync(documentationCache: repository.PathOf("artifacts/documentation")));
    }

    [Fact]
    public async Task CorruptPreparedReferenceCannotReachValidationExecution()
    {
        using var repository = new ValidationRepository();
        var documentation = new ValidationDocumentationFixture(repository);
        documentation.Initialize();
        DocumentationResult indexed = await documentation.IndexAsync();
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }");
        using JsonDocument plan = await documentation.PlanAsync(indexed);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(documentation.cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        string identity = catalog.references.Single(reference => reference.model == "source-clr").identity;
        File.WriteAllText(Path.Combine(documentation.cache, "references", identity, "reference.json"), "damaged immutable payload");
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
        Assert.Contains("integrity mismatch", failure.Message);
        Assert.False(File.Exists(repository.PathOf("artifacts/run/results.json")));
    }

    [Fact]
    public async Task UnrelatedProsePreservesSelectedSourceReferenceInputs()
    {
        using var repository = new ValidationRepository();
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }");
        using JsonDocument first = await docs.PlanAsync(baseline);
        docs.Write("content/en/other-guide.md", ValidationDocumentationFixture.Page("Other", "Updated usage guidance."));
        DocumentationResult current = await docs.IndexAsync();
        using JsonDocument next = await docs.PlanAsync(current, baseline);
        JsonElement InitialApi(JsonDocument plan) => plan.RootElement.GetProperty("checks").EnumerateArray()
            .Single(check => check.GetProperty("kind").GetString() == "documentation-api");
        Assert.Equal(InitialApi(first).GetRawText(), InitialApi(next).GetRawText());
        Assert.DoesNotContain(next.RootElement.GetProperty("checks").EnumerateArray(), check => check.GetProperty("id").GetString() == "build:src/Other/Other.csproj");
        Assert.NotEqual(baseline.snapshot, current.snapshot);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RemovingARequiredCaptureDoesNotBecomeAnEmptySuccessfulSelection(bool changedSource)
    {
        using var repository = new ValidationRepository();
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        DocumentationResult baseline = await docs.IndexAsync();
        JsonNode facts = JsonNode.Parse(File.ReadAllBytes(repository.PathOf("artifacts/docs-source/reference/api/facts.json")))!;
        JsonArray owners = facts["projects"]!.AsArray();
        owners.Remove(owners.Single(owner => owner!["name"]!.GetValue<string>() == "Example"));
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(facts);
        File.WriteAllBytes(repository.PathOf("artifacts/docs-source/reference/api/facts.json"), bytes);
        JsonNode binding = JsonNode.Parse(File.ReadAllBytes(repository.PathOf("artifacts/docs-source/reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Convert.ToHexStringLower(SHA256.HashData(bytes));
        docs.Write("reference/engine.lock.json", binding.ToJsonString());
        DocumentationResult current = await docs.IndexAsync();
        if (changedSource)
        {
            repository.Write("src/Example/Changed.cs", "namespace Example; /// <summary>A captured source contract.</summary>\npublic class Changed { }");
            repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Fixture policy passed'");
            repository.Write("tests/ci/CapturePolicy.Tests.ps1", "Write-Output 'Fixture compilation policy passed'");
        }
        using JsonDocument plan = await docs.PlanAsync(current, baseline);
        JsonElement[] checks = plan.RootElement.GetProperty("checks").EnumerateArray().ToArray();
        JsonElement missing = checks.Single(check => check.GetProperty("id").GetString() == "documentation-api-missing:src/Example/Example.csproj");
        Assert.Equal("documentation-missing", missing.GetProperty("kind").GetString());
        Assert.Equal(JsonValueKind.Null, missing.GetProperty("exclusion").ValueKind);
        if (changedSource)
        {
            JsonElement build = checks.Single(check => check.GetProperty("id").GetString() == "build:src/Example/Example.csproj");
            Assert.Equal(build.GetProperty("context").GetString(), missing.GetProperty("context").GetString());
            Assert.Contains(missing.GetProperty("prerequisites").EnumerateArray(), prerequisite => prerequisite.GetString() == build.GetProperty("id").GetString());
            Assert.True(Array.IndexOf(checks, build) < Array.IndexOf(checks, missing));
            Assert.Equal("src/Example/Example.csproj", Assert.Single(missing.GetProperty("projects").EnumerateArray()).GetString());
            // Exercise the real executor and compiler with a finite fixture policy. Repository-wide
            // architecture and test discovery are separate contracts, not inputs to this capture test.
            JsonNode execution = JsonNode.Parse(plan.RootElement.GetRawText())!;
            JsonArray selected = new();
            foreach (JsonElement check in checks.Where(check => new[] { "instructions", "architecture", "documentation", build.GetProperty("id").GetString(), missing.GetProperty("id").GetString() }.Contains(check.GetProperty("id").GetString())))
            {
                JsonNode item = JsonNode.Parse(check.GetRawText())!;
                if (item["id"]!.GetValue<string>() == "architecture")
                    item["kind"] = "ci-contracts";
                selected.Add(item);
            }
            execution["checks"] = selected;
            repository.Write("artifacts/plan/plan.json", execution.ToJsonString());
            await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync());
            using JsonDocument evidence = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/run/results.json")));
            JsonElement[] results = evidence.RootElement.GetProperty("results").EnumerateArray().ToArray();
            Assert.True(results.Single(result => result.GetProperty("id").GetString() == build.GetProperty("id").GetString()).GetProperty("status").GetString() == "Passed", evidence.RootElement.GetRawText());
            Assert.Equal("Blocked", results.Single(result => result.GetProperty("id").GetString() == missing.GetProperty("id").GetString()).GetProperty("status").GetString());
            using JsonDocument captured = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/run/documentation-captures/Example.json")));
            Assert.Equal("Unverified", captured.RootElement.GetProperty("sourceApiStatus").GetString());
            Assert.Contains(captured.RootElement.GetProperty("projects")[0].GetProperty("symbols").EnumerateArray(), symbol => symbol.GetProperty("id").GetString() == "T:Example.Changed");
        }
        else
        {
            Assert.DoesNotContain(checks, check => check.GetProperty("kind").GetString() == "build");
            Assert.Equal(JsonValueKind.Null, missing.GetProperty("projects").ValueKind);
        }
    }

    [Fact]
    public async Task UnknownExecutableAgentInputIsNotExemptedAsDocumentation()
    {
        using var repository = new ValidationRepository();
        repository.Write(".agents/check.ps1", "throw 'unknown executable input'");
        using JsonDocument plan = await repository.PlanAsync();
        Assert.True(plan.RootElement.GetProperty("full").GetBoolean());
        Assert.Contains(plan.RootElement.GetProperty("diagnostics").EnumerateArray(), item => item.GetString()!.Contains(".agents/check.ps1", StringComparison.Ordinal));
    }
    private static async Task<DocumentationResult> PrepareDocumentationAsync(ValidationRepository repository)
    {
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        docs.Write("content/en/Example.md", "---\nuid: project-example\nkind: project\nowners: [Example]\napiSurface: source-clr\n---\n# Example\n\nRead the declared contract.\n\n<!-- api-include: random/behavior.md -->\n");
        docs.Write("content/en/random/behavior.md", "---\nuid: task-fragment\nkind: api\nowners: [Example]\napiSurface: source-clr\nexampleMode: host\nexampleProjects: [Example]\n---\n# Behavior\n\nDeclare a local value.\n\n```csharp\nint value = 1;\n```\n");
        return await docs.IndexAsync();
    }
}
