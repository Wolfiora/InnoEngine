using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Validation;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationProductInputTests
{
    [Fact]
    public async Task ProductChecksStartFromAnEmptyEngineOwnedProject()
    {
        using var repository = new ValidationRepository();
        // Stop at the external product boundary: this regression certifies input preparation, not an Editor journey.
        repository.Write("tests/Invoke-EditorSmoke.ps1", "throw 'Product input reached the product boundary.'\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the product boundary");
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }\n");
        using JsonDocument generated = await repository.PlanAsync();
        JsonNode plan = JsonNode.Parse(generated.RootElement.GetRawText())!;
        JsonNode build = plan["checks"]!.AsArray().Single(check => (
            check!["kind"]!.GetValue<string>() == "build"
            && check["owner"]!.GetValue<string>() == "src/Example/Example.csproj"
        ))!.DeepClone();
        build["prerequisites"] = new JsonArray();
        plan["checks"] = new JsonArray(
            build,
            JsonSerializer.SerializeToNode(new
            {
                id = "product-probe",
                kind = "products",
                owner = "src/Example/Example.csproj",
                reason = "Exercise the engine-owned product-input boundary.",
                environment = "fixture",
                prerequisites = new[] { build["id"]!.GetValue<string>() },
                inputs = new[] { "@src/Example/Example.csproj" },
                reusable = false,
                exclusion = (string?)null,
                context = "managed",
                layer = "product"
            })
        );
        JsonNode project = plan["contexts"]!["managed"]!["sdkGraph"]!.AsArray().Single(snapshot => (
            repository.BindWorkspace(snapshot!["Path"]!.GetValue<string>()) == repository.PathOf("src/Example/Example.csproj")
        ))!;
        project["Properties"]!["InnoValidationBackendPattern"] = "Declared fixture renderer";
        repository.Write("artifacts/plan/plan.json", plan.ToJsonString());
        string artifacts = repository.BindWorkspace(plan["artifacts"]!.GetValue<string>());

        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/probe"));

        string output = repository.PathOf("artifacts/probe");
        using JsonDocument evidence = JsonDocument.Parse(File.ReadAllText(Path.Combine(output, "results.json")));
        JsonElement result = evidence.RootElement
            .GetProperty("results")
            .EnumerateArray()
            .Single(result => result.GetProperty("id").GetString() == "product-probe");
        Assert.Equal("Failed", result.GetProperty("status").GetString());
        string detail = result.GetProperty("detail").GetString()!;
        Assert.True(detail.Contains("Product input reached the product boundary", StringComparison.Ordinal), detail);
        string source = Path.Combine(output, "product-probe", "Source");
        Assert.Equal(Path.Combine(source, "Assets"), Assert.Single(Directory.GetFileSystemEntries(source)));
        Assert.Empty(Directory.GetFileSystemEntries(Path.Combine(source, "Assets")));
        Assert.False(Directory.Exists(Path.Combine(artifacts, "consumer-inputs")));
    }
}
