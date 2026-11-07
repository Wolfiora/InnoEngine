using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Validation.Tests;

public sealed class ValidationConsumerInputTests
{
    [Fact]
    public async Task ProductInputsUseVerifiedCommitObjectsAcrossRunsAndRepairCorruption()
    {
        using var repository = new ValidationRepository();
        using var consumer = new ValidationRepository();
        string origin = "https://github.com/validation/" + Path.GetFileName(Path.TrimEndingDirectorySeparator(consumer.PathOf("."))) + ".git";
        consumer.Git("remote", "add", "origin", origin);
        const string C_SCENE = "FlappyBird/Assets/FlappyBird/FlappyBird.iscene";
        consumer.Write(C_SCENE, "pinned scene content");
        consumer.Git("add", C_SCENE);
        consumer.Git("commit", "-m", "Pin the consumer scene");
        string commit = consumer.Git("rev-parse", "HEAD");
        consumer.Write(C_SCENE, "uncommitted consumer changes must remain private");
        repository.Write("tests/fixtures/editor-ui/consumer.json", JsonSerializer.Serialize(new { repository = origin, commit, project = "FlappyBird" }));
        // Stop at the external product boundary: this regression certifies input preparation, not an Editor journey.
        repository.Write("tests/Invoke-EditorSmoke.ps1", "throw 'Consumer input reached the product boundary.'\n");
        repository.Git("add", ".");
        repository.Git("commit", "-m", "Declare the isolated product input protocol");
        repository.Write("src/Example/Changed.cs", "namespace Example; public class Changed { }\n");
        using JsonDocument generated = await repository.PlanAsync();
        JsonNode plan = JsonNode.Parse(generated.RootElement.GetRawText())!;
        JsonNode build = plan["checks"]!.AsArray().Single(check => check!["kind"]!.GetValue<string>() == "build"
            && check["owner"]!.GetValue<string>() == "src/Example/Example.csproj")!.DeepClone();
        build["prerequisites"] = new JsonArray();
        plan["checks"] = new JsonArray(build, JsonSerializer.SerializeToNode(new
        {
            id = "consumer-probe", kind = "products", owner = "src/Example/Example.csproj",
            reason = "Exercise the declared external product-input boundary.", environment = "fixture",
            prerequisites = new[] { build["id"]!.GetValue<string>() }, inputs = new[] { "@src/Example/Example.csproj" },
            reusable = false, exclusion = (string?)null, context = "managed", layer = "product"
        }));
        JsonNode project = plan["contexts"]!["managed"]!["sdkGraph"]!.AsArray().Single(snapshot =>
            snapshot!["Path"]!.GetValue<string>() == repository.PathOf("src/Example/Example.csproj"))!;
        project["Properties"]!["InnoValidationBackendPattern"] = "Declared fixture renderer";
        repository.Write("artifacts/plan/plan.json", plan.ToJsonString());
        string artifacts = plan["artifacts"]!.GetValue<string>();

        await RunProbeAsync("first");
        string cache = Assert.Single(Directory.GetDirectories(Path.Combine(artifacts, "consumer-inputs")));
        Assert.Equal(commit, consumer.Git("-C", cache, "rev-parse", commit + "^{commit}"));
        await RunProbeAsync("reused");
        Assert.False(File.Exists(repository.PathOf("artifacts/reused/consumer-probe/consumer-fetch.log")));
        string stored = Directory.GetFiles(Path.Combine(cache, "objects", "pack"), "*.pack").SingleOrDefault()
            ?? Path.Combine(cache, "objects", commit[..2], commit[2..]);
        Assert.True(File.Exists(stored));
        File.SetAttributes(stored, FileAttributes.Normal);
        using (FileStream corrupt = File.OpenWrite(stored))
            corrupt.WriteByte(0);
        await RunProbeAsync("repaired");
        Assert.Single(Directory.GetDirectories(Path.Combine(artifacts, "consumer-inputs"), "*.rejected-*"));
        Assert.True(File.Exists(repository.PathOf("artifacts/repaired/consumer-probe/consumer-fetch.log")));
        Assert.Equal("uncommitted consumer changes must remain private", File.ReadAllText(consumer.PathOf(C_SCENE)));
        consumer.Git("fsck", "--full", "--strict");
        consumer.Git("-C", cache, "remote", "set-url", "origin", "https://github.com/validation/wrong.git");
        await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/wrong-origin"));
        Assert.Contains("different repository or storage owner", File.ReadAllText(repository.PathOf("artifacts/wrong-origin/results.json")));

        async Task RunProbeAsync(string name)
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => repository.RunAsync(output: "artifacts/" + name));
            string output = repository.PathOf("artifacts/" + name);
            using JsonDocument evidence = JsonDocument.Parse(File.ReadAllText(Path.Combine(output, "results.json")));
            JsonElement result = evidence.RootElement.GetProperty("results").EnumerateArray().Single(result => result.GetProperty("id").GetString() == "consumer-probe");
            Assert.Equal("Failed", result.GetProperty("status").GetString());
            string detail = result.GetProperty("detail").GetString()!;
            Assert.True(detail.Contains("Consumer input reached the product boundary", StringComparison.Ordinal), detail);
            Assert.Equal("pinned scene content", File.ReadAllText(Path.Combine(output, "consumer-probe", "Source", "Assets", "FlappyBird", "FlappyBird.iscene")));
        }
    }
}
