using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml.Linq;
using Inno.Tooling.Documentation;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationDocumentationInputTests
{
    [Fact]
    public async Task EmptyForeignSurfaceRetainsSourceProofWithoutRestoringOrBuildingTheProduct()
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
        repository.Git("commit", "-m", "Declare the foreign source-only product boundary");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        repository.Write("platforms/Foreign/player/Foreign.Player/Entry.cs", "namespace Foreign; internal static class Entry { }");
        // Retain the fixture's existing behavioral consumer while this test executes only
        // the source-inspection obligations, just as the foreign-boundary planning fixture does.
        repository.Write("src/Example/Changed.cs", "namespace Example; internal sealed class Changed { }");
        using JsonDocument planned = await docs.PlanAsync(await docs.IndexAsync());
        JsonNode execution = JsonNode.Parse(planned.RootElement.GetRawText())!;
        execution["checks"] = new JsonArray(planned.RootElement.GetProperty("checks").EnumerateArray()
            .Where(check => check.GetProperty("id").GetString() is "documentation" or "documentation-api:source:default")
            .Select(check => JsonNode.Parse(check.GetRawText())).ToArray());
        repository.Write("artifacts/plan/plan.json", execution.ToJsonString());
        Assert.Equal(0, await repository.RunAsync());
        using JsonDocument result = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/run/results.json")));
        JsonElement source = result.RootElement.GetProperty("results").EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "documentation-api:source:default");
        Assert.Equal("Passed", source.GetProperty("status").GetString());
        JsonElement proof = Assert.Single(source.GetProperty("documentationInputs").EnumerateArray());
        Assert.Equal(C_FOREIGN, proof.GetProperty("project").GetString());
        Assert.Empty(proof.GetProperty("packages").EnumerateObject());
        Assert.Matches("^[a-f0-9]{64}$", proof.GetProperty("sources").GetProperty(C_FOREIGN).GetString()!);
        Assert.Empty(Directory.EnumerateFiles(repository.PathOf("."), "project.assets.json", SearchOption.AllDirectories));
        Assert.Empty(Directory.EnumerateFiles(repository.PathOf("."), "Foreign.Player.dll", SearchOption.AllDirectories));
    }

    [Fact]
    public async Task SuccessfulSourceComparisonRetainsSeparateOwnerAndPackageInputs()
    {
        using var repository = new ValidationRepository();
        const string C_EXAMPLE = "src/Example/Example.csproj";
        const string C_OTHER = "src/Other/Other.csproj";
        string feed = repository.PathOf("artifacts/package-feed");
        string packages = repository.PathOf("artifacts/packages");
        Directory.CreateDirectory(feed);
        using (ZipArchive package = ZipFile.Open(Path.Combine(feed, "Inno.ProofFixture.1.0.0.nupkg"), ZipArchiveMode.Create))
        {
            using (var manifest = new StreamWriter(package.CreateEntry("Inno.ProofFixture.nuspec").Open()))
                manifest.Write("<package><metadata><id>Inno.ProofFixture</id><version>1.0.0</version>"
                    + "<authors>InnoEngine</authors><description>Owned dependency evidence fixture.</description></metadata></package>");
            using var content = new StreamWriter(package.CreateEntry("content/proof.txt").Open());
            content.Write("first");
        }
        repository.Write(C_EXAMPLE, ValidationRepository.Project(
            new XElement("PropertyGroup", new XElement("RestoreSources", feed), new XElement("RestorePackagesPath", packages),
                new XElement("NuGetAudit", false)).ToString()
            + "<ItemGroup><PackageReference Include=\"Inno.ProofFixture\" Version=\"1.0.0\" /></ItemGroup>"));
        repository.Write("tests/ci/Test-AgentRules.ps1", "Write-Output 'Fixture instructions passed'\n");
        repository.Write("tests/ci/Policy.Tests.ps1", "Write-Output 'Fixture compilation policy passed'\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare actual package and finite compilation policy");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        docs.Write("content/en/example-task.md", ValidationDocumentationFixture.Page("Example",
            "Keep an ordinary managed value alive.\n\n```csharp\nSystem.GC.KeepAlive(1);\n```\n", "host"));
        DocumentationResult indexed = await docs.IndexAsync();
        int run = 0;
        repository.Write("src/Example/Implementation.cs", "namespace Example; internal sealed class Implementation { }\n");
        repository.Write("src/Other/Implementation.cs", "namespace Other; internal sealed class Implementation { }\n");
        using JsonDocument first = await ExecuteAsync();
        JsonElement[] initial = Proofs(first);
        Assert.Equal(2, initial.Length);
        JsonElement example = initial.Single(proof => proof.GetProperty("project").GetString() == C_EXAMPLE);
        Assert.Equal(C_EXAMPLE, Assert.Single(example.GetProperty("owners").EnumerateArray()).GetString());
        Assert.DoesNotContain(C_OTHER, example.GetProperty("owners").EnumerateArray().Select(owner => owner.GetString()));
        Assert.Matches("^[a-f0-9]{64}$", example.GetProperty("sourceIdentity").GetString()!);
        Assert.Matches("^[a-f0-9]{64}$", example.GetProperty("packages").GetProperty("inno.prooffixture/1.0.0").GetString()!);
        Assert.Equal("Debug", example.GetProperty("configuration").GetString());
        Assert.Equal(first.RootElement.GetProperty("environment").GetString(), example.GetProperty("environment").GetString());
        JsonElement exampleExecution = first.RootElement.GetProperty("results").EnumerateArray()
            .Single(result => result.GetProperty("id").GetString() == "documentation-examples:managed");
        Assert.Equal("Passed", exampleExecution.GetProperty("status").GetString());
        JsonElement exampleInputs = Assert.Single(exampleExecution.GetProperty("documentationInputs").EnumerateArray());
        Assert.Equal(C_EXAMPLE, exampleInputs.GetProperty("project").GetString());
        Assert.Equal(example.GetProperty("packages").GetRawText(), exampleInputs.GetProperty("packages").GetRawText());
        Assert.Equal(example.GetProperty("sources").GetRawText(), exampleInputs.GetProperty("sources").GetRawText());

        repository.Write("src/Other/Implementation.cs", "namespace Other; internal sealed class Implementation { internal int value; }\n");
        using JsonDocument next = await ExecuteAsync();
        JsonElement[] changed = Proofs(next);
        Assert.Equal(example.GetProperty("sourceIdentity").GetString(),
            changed.Single(proof => proof.GetProperty("project").GetString() == C_EXAMPLE).GetProperty("sourceIdentity").GetString());
        Assert.NotEqual(initial.Single(proof => proof.GetProperty("project").GetString() == C_OTHER).GetProperty("sourceIdentity").GetString(),
            changed.Single(proof => proof.GetProperty("project").GetString() == C_OTHER).GetProperty("sourceIdentity").GetString());
        string evidence = repository.PathOf("artifacts/source-input-run-2/results.json");
        Assert.Equal(0, await repository.EvidenceAsync(evidence));
        string restored = Path.Combine(packages, "inno.prooffixture", "1.0.0", "content", "proof.txt");
        DateTime timestamp = File.GetLastWriteTimeUtc(restored);
        File.WriteAllText(restored, "other");
        File.SetLastWriteTimeUtc(restored, timestamp);
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.EvidenceAsync(evidence));

        async Task<JsonDocument> ExecuteAsync()
        {
            using JsonDocument planned = await docs.PlanAsync(indexed);
            JsonNode execution = JsonNode.Parse(planned.RootElement.GetRawText())!;
            var selected = new JsonArray();
            foreach (JsonElement check in planned.RootElement.GetProperty("checks").EnumerateArray())
            {
                string kind = check.GetProperty("kind").GetString()!;
                if (kind is not ("instructions" or "architecture" or "documentation" or "documentation-api" or "documentation-examples")
                    && !(kind == "build" && check.GetProperty("owner").GetString() is C_EXAMPLE or C_OTHER))
                    continue;
                JsonNode item = JsonNode.Parse(check.GetRawText())!;
                // The source-input protocol uses real compilation and comparison. Repository-wide
                // architecture policy is verified independently, not synthesized by this small fixture.
                if (kind == "architecture")
                    item["kind"] = "ci-contracts";
                selected.Add(item);
            }
            execution["checks"] = selected;
            repository.Write("artifacts/plan/plan.json", execution.ToJsonString());
            string output = "artifacts/source-input-run-" + ++run;
            await repository.RunAsync(output: output);
            using JsonDocument comparison = JsonDocument.Parse(File.ReadAllBytes(
                repository.PathOf(output + "/documentation-api-managed-default.json")));
            foreach (JsonElement owner in comparison.RootElement.GetProperty("projects").EnumerateArray())
                Assert.Equal(planned.RootElement.GetProperty("documentationSources")
                    .GetProperty("reference/api/facts.json\0" + owner.GetProperty("name").GetString()).GetString(),
                    owner.GetProperty("referenceIdentity").GetString());
            return JsonDocument.Parse(File.ReadAllBytes(repository.PathOf(output + "/results.json")));
        }

        static JsonElement[] Proofs(JsonDocument evidence)
        {
            Assert.True(evidence.RootElement.GetProperty("completed").GetBoolean());
            JsonElement comparison = Assert.Single(evidence.RootElement.GetProperty("results").EnumerateArray()
                .Where(result => result.GetProperty("id").GetString()!.StartsWith("documentation-api:", StringComparison.Ordinal)));
            Assert.Equal("Passed", comparison.GetProperty("status").GetString());
            return comparison.GetProperty("documentationInputs").EnumerateArray().ToArray();
        }
    }
}
