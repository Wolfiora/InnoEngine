using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Validation;
using Inno.Tooling.Documentation;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationDocumentationInputTests
{
    [Fact]
    public async Task EmptyForeignSurfaceRetainsSourceProofWithoutRestoringOrBuildingTheProduct()
    {
        using var repository = new ValidationRepository();
        const string C_FOREIGN = "src/Foreign/Foreign.Player/Foreign.Player.csproj";
        repository.Write("Product.props", "<Project />");
        repository.Write(
            "entry/InnoEngine.Editor/InnoEngine.Editor.csproj",
            ValidationRepository.Project(
                "<PropertyGroup><InnoDevelopmentTarget>host-fixture</InnoDevelopmentTarget><InnoDevelopmentEditorBuildProperties>"
                + repository.PathOf("Product.props") + "</InnoDevelopmentEditorBuildProperties></PropertyGroup>"
            )
        );
        repository.Write(
            C_FOREIGN,
            ValidationRepository.Project(
                "<PropertyGroup><InnoProductId>player</InnoProductId><InnoProductTarget>foreign-fixture</InnoProductTarget></PropertyGroup>"
            )
        );
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the foreign source-only product boundary");
        var docs = new ValidationDocumentationFixture(repository);
        docs.Initialize();
        repository.Write("src/Foreign/Foreign.Player/Entry.cs", "namespace Foreign; internal static class Entry { }");
        // Retain the fixture's existing behavioral consumer while this test executes only
        // the source-inspection obligations, just as the foreign-boundary planning fixture does.
        repository.Write("src/Example/Changed.cs", "namespace Example; internal sealed class Changed { }");
        using JsonDocument planned = await docs.PlanAsync(await docs.IndexAsync());
        JsonNode execution = JsonNode.Parse(planned.RootElement.GetRawText())!;
        execution["checks"] = new JsonArray(
            planned.RootElement.GetProperty("checks")
                .EnumerateArray()
                .Where(check => check.GetProperty("id").GetString() is "documentation" or "documentation-api:source:default")
                .Select(check => JsonNode.Parse(check.GetRawText()))
                .ToArray()
        );
        repository.Write("artifacts/plan/plan.json", execution.ToJsonString());
        Assert.Equal(0, await repository.RunAsync());
        using JsonDocument result = JsonDocument.Parse(File.ReadAllBytes(repository.PathOf("artifacts/run/results.json")));
        JsonElement source = result.RootElement.GetProperty("results")
            .EnumerateArray()
            .Single(check => check.GetProperty("id").GetString() == "documentation-api:source:default");
        Assert.Equal("Passed", source.GetProperty("status").GetString());
        JsonElement proof = Assert.Single(source.GetProperty("documentationInputs").EnumerateArray());
        Assert.Equal(C_FOREIGN, proof.GetProperty("project").GetString());
        Assert.Empty(proof.GetProperty("packages").EnumerateObject());
        Assert.Matches("^[a-f0-9]{64}$", proof.GetProperty("sources").GetProperty(C_FOREIGN).GetString()!);
        Assert.Empty(Directory.EnumerateFiles(repository.PathOf("."), "project.assets.json", SearchOption.AllDirectories));
        Assert.Empty(Directory.EnumerateFiles(repository.PathOf("."), "Foreign.Player.dll", SearchOption.AllDirectories));
    }
}
