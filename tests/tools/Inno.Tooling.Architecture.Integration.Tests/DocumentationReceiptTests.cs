using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Source;

namespace Inno.Tooling.Architecture.Integration.Tests;

public sealed class DocumentationReceiptTests
{
    [Theory]
    [MemberData(nameof(Cases))]
    public async Task ReconcilesOnlyTheDeclaredReport(
        string kind,
        string name
    ) {
        JsonNode fixture = ReadFixture(kind);
        JsonNode test = fixture["cases"]!.AsArray().Single(item => (string?)item!["name"] == name)!;
        JsonNode state = fixture["bases"]![(string)test["base"]!]!.DeepClone();
        foreach (JsonNode? change in test["changes"]!.AsArray())
        {
            JsonArray path = change!["path"]!.AsArray();
            JsonNode parent = state;
            foreach (JsonNode? part in path.SkipLast(1))
            {
                parent = parent is JsonArray ? parent[(int)part!]! : parent[(string)part!]!;
            }
            JsonNode? value = change["value"]?.DeepClone();
            if (parent is JsonArray array)
            {
                array[(int)path[^1]!] = value;
            }
            else if (change.AsObject().ContainsKey("value"))
            {
                parent[(string)path[^1]!] = value;
            }
            else
            {
                parent.AsObject().Remove((string)path[^1]!);
            }
        }
        JsonNode plan = state["plan"]!;
        plan["documentation"] = state["context"]!.DeepClone();
        plan["checks"]!
            .AsArray()
            .Add(
                new JsonObject { ["id"] = "documentation", ["kind"] = "documentation", ["documentation"] = state["context"]!.DeepClone() }
            );
        DirectoryInfo root = Directory.CreateTempSubdirectory("InnoReceipts-");
        try
        {
            string planFile = Write("plan.json", plan);
            string results = Write("results.json", state["evidence"]!);
            Write("documentation-catalog.json", state["catalog"]!);
            string report = kind == "source" ? "documentation-api-managed-default.json" : "managed-documentation-examples.json";
            if ((bool?)state["missing"] != true)
            {
                Write(report, state["report"]!);
            }
            string output = Path.Combine(root.FullName, "certificate.json");
            Task<int> operation = DocumentationReferences.ExecuteAsync(
                ["certify", planFile, results, root.FullName, output],
                CancellationToken.None
            );
            if (!(bool)test["accepted"]!)
            {
                Assert.NotNull(await Record.ExceptionAsync(() => operation));
                Assert.False(File.Exists(output));
                return;
            }
            Assert.Equal(0, await operation);
            JsonNode receipt = Assert.Single(
                JsonNode.Parse(File.ReadAllBytes(output))![kind == "source" ? "sourceApi" : "sourceExamples"]!.AsArray()
            )!;
            Assert.Equal(
                Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(Path.Combine(root.FullName, report)))),
                (string?)receipt["reportIdentity"]
            );
            if (kind == "source")
            {
                Assert.Equal("Probe", (string?)receipt["projects"]![0]);
                if (plan["checks"]![0]!["inputs"]!.AsArray().Count > 0)
                {
                    Assert.Equal(new string('a', 64), (string?)Assert.Single(receipt["inputs"]!.AsArray())!["packages"]!["probe/1.0.0"]);
                }
            }
            else
            {
                JsonNode page = receipt["pages"]![0]!;
                Assert.Single(page["blocks"]!.AsArray());
                Assert.Equal("Probe", (string?)page["projects"]![0]);
            }
        }
        finally
        {
            root.Delete(recursive: true);
        }

        string Write(
            string file,
            JsonNode value
        ) {
            string path = Path.Combine(root.FullName, file);
            File.WriteAllText(path, value.ToJsonString());
            return path;
        }
    }

    public static IEnumerable<object[]> Cases()
    {
        foreach (string kind in new[] { "source", "example" })
        {
            foreach (JsonNode? item in ReadFixture(kind)["cases"]!.AsArray())
            {
                yield return [kind, (string)item!["name"]!];
            }
        }
    }

    private static JsonNode ReadFixture(string kind)
        => JsonNode.Parse(
            File.ReadAllBytes(
                Path.Combine(
                    TestRepository.GetRoot(typeof(DocumentationReceiptTests).Assembly),
                    $"tests/tools/Inno.Tooling.Architecture.Integration.Tests/fixtures/documentation-{kind}-receipts.json"
                )
            )
        )!;
}
