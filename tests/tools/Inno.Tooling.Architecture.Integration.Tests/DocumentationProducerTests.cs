using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Source;

namespace Inno.Tooling.Architecture.Integration.Tests;

public sealed class DocumentationProducerTests
{
    [Theory]
    [InlineData("complete", true)]
    [InlineData("crlf", true)]
    [InlineData("old-revision", false)]
    [InlineData("wrong-tree", false)]
    [InlineData("foreign-source", false)]
    [InlineData("missing-configuration", false)]
    [InlineData("missing-projection", false)]
    [InlineData("missing-source-owner", false)]
    [InlineData("damaged", false)]
    [InlineData("outside", false)]
    [InlineData("invented-projects", false)]
    public async Task CollectsOnlyCompleteMatchingProducerOutputs(
        string scenario,
        bool accept
    ) {
        string engine = TestRepository.GetRoot(typeof(DocumentationProducerTests).Assembly);
        JsonNode fixture = JsonNode.Parse(
            File.ReadAllBytes(
                Path.Combine(
                    engine,
                    "tests/tools/Inno.Tooling.Architecture.Integration.Tests/fixtures/documentation-producer.json"
                )
            )
        )!;
        string root = Directory.CreateTempSubdirectory("InnoProducer-").FullName;
        string host = Path.Combine(root, "artifacts/input/host");
        string output = Path.Combine(root, "artifacts/output");
        Directory.CreateDirectory(host);
        try
        {
            string source = Directory.CreateDirectory(Path.Combine(root, "src/Inno.Probe")).FullName;
            File.WriteAllText(
                Path.Combine(source, "Inno.Probe.csproj"),
                "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup></Project>"
            );
            File.WriteAllText(Path.Combine(source, "Probe.cs"), "internal sealed class Probe {}");
            string inventory = Path.Combine(host, "projects.json");
            await DocumentationReferences.ExecuteAsync(
                ["export-projects", "--engine-root", root, "--output", inventory],
                CancellationToken.None
            );
            if (scenario == "invented-projects")
            {
                File.WriteAllText(inventory, "[]");
            }
            JsonNode producer = fixture["producer"]!;
            string revision = (string)producer["revision"]!;
            string tree = (string)producer["sourceTree"]!;
            string plan = Path.Combine(host, "plan.json");
            File.WriteAllText(plan, fixture["plan"]!.ToJsonString());
            producer["planIdentity"] = Hash(plan);
            producer["projectManifestIdentity"] = Hash(inventory);
            JsonArray captures = producer["captures"]!.AsArray();
            foreach (JsonNode? capture in captures)
            {
                string model = (string)capture!["model"]!;
                if (model == "source-clr" && scenario == "missing-source-owner")
                {
                    fixture[model]!["projects"] = new JsonArray();
                }
                string file = Path.Combine(host, (string)capture["path"]!);
                File.WriteAllText(file, fixture[model]!.ToJsonString() + (scenario == "crlf" ? "\r\n" : "\n"));
                capture["sha256"] = Hash(file);
            }
            switch (scenario)
            {
                case "missing-projection":
                    captures.Remove(captures.Single(item => (string?)item!["model"] == "editor-script"));
                    break;
                case "outside":
                    captures[0]!["path"] = "../source-clr.json";
                    break;
                case "damaged":
                    File.AppendAllText(Path.Combine(host, "source-clr.json"), " ");
                    break;
                case "old-revision":
                    producer["revision"] = new string('f', 40);
                    break;
                case "wrong-tree":
                    producer["sourceTree"] = new string('f', 40);
                    break;
                case "foreign-source":
                    producer["repository"] = "https://example.invalid/foreign";
                    break;
            }
            File.WriteAllText(Path.Combine(host, "producer.json"), producer.ToJsonString());
            string[] configurations = scenario == "missing-configuration" ? ["Debug", "Release"] : ["Debug"];
            Task<int> CollectAsync()
                => DocumentationReferences.ExecuteAsync([
                    "collect-producers",
                    root,
                    Path.GetDirectoryName(host)!,
                    output,
                    revision,
                    tree,
                    ..configurations
                ], CancellationToken.None);
            if (!accept)
            {
                await Assert.ThrowsAsync<InvalidDataException>(CollectAsync);
                return;
            }
            Assert.Equal(0, await CollectAsync());
            JsonNode binding = JsonNode.Parse(File.ReadAllBytes(Path.Combine(output, "reference/engine.lock.json")))!;
            foreach (JsonNode? capture in binding["apiManifests"]!.AsArray())
            {
                string path = Path.Combine(output, (string)capture!["path"]!);
                Assert.DoesNotContain("\r", File.ReadAllText(path));
                Assert.Equal((string?)capture["sha256"], Hash(path));
            }
            Assert.Equal((string?)binding["projectManifestIdentity"], Hash(Path.Combine(output, (string)binding["projectManifest"]!)));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static string Hash(string path) => Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(path)));
}
