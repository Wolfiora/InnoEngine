using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Markdig;
using Xunit;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Integration.Tests;

public abstract class DocumentationApiFixture : IDisposable
{
    protected const string C_TYPE = "T:Inno.Probe.Value";
    protected const string C_MEMBER = "P:Inno.Probe.Value.count";

    protected readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoApiSynchronization", Guid.NewGuid().ToString("N"));

    protected DocumentationApiFixture()
    {
        Bind();
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    protected Task<int> SyncAsync() => SyncAsync(CancellationToken.None);

    protected Task<int> SyncAsync(CancellationToken cancellationToken)
        => DocumentationCommand.ExecuteAsync(["api-sync", "--docs-root", m_root], cancellationToken);

    protected void SetTypeDocumentation(string xml)
    {
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        facts["projects"]![0]!["symbols"]![0]!["documentation"] = xml;
        string text = facts.ToJsonString();
        Write("reference/api/probe.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    protected string Fragment(string id)
    {
        using JsonDocument registry = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(m_root, "reference/api/content.json")));
        string path = registry.RootElement.GetProperty("entries")
            .EnumerateArray()
            .Single(entry => entry.GetProperty("symbol").GetString() == "engine:Inno.Probe:" + id)
            .GetProperty("path")
            .GetString()!;
        return Path.Combine(m_root, path);
    }

    protected void Bind(
        bool member = true,
        string description = "The value contract.",
        string countType = "int"
    ) {
        string inventory = JsonSerializer.Serialize(
            new[] { new { name = "Inno.Probe", path = "src/Inno.Probe.csproj", surfaces = new[] { "source-clr" }, isTest = false } }
        );
        Write("reference/projects/catalog.json", inventory);
        object Symbol(
            string id,
            string kind,
            string signature,
            string? parent
        ) => new
        {
            project = "Inno.Probe",
            id,
            kind,
            signature,
            signatureHash = Hash(signature),
            source = "src/Value.cs",
            line = 1,
            documentation = "<member><summary>" + description + "</summary></member>",
            containingType = parent,
            baseType = (string?)null,
            interfaces = Array.Empty<string>()
        };
        object[] symbols = member
                           ? [
                               Symbol(C_TYPE, "NamedType", "public class Inno.Probe.Value", null),
                               Symbol(C_MEMBER, "Property", "public " + countType + " Inno.Probe.Value.count { get; }", C_TYPE)
                           ]
                           : [Symbol(C_TYPE, "NamedType", "public class Inno.Probe.Value", null)];
        string facts = JsonSerializer.Serialize(new
        {
            model = "source-clr",
            configuration = "Debug",
            target = (string?)null,
            projects = new[] { new { name = "Inno.Probe", project = "src/Inno.Probe.csproj", symbols } }
        });
        Write("reference/api/probe.json", facts);
        Write(
            "reference/engine.lock.json",
            JsonSerializer.Serialize(new
            {
                repository = "https://github.com/Wolfiora/InnoEngine",
                revision = new string('a', 40),
                sourceTree = new string('b', 40),
                toolingRevision = new string('a', 40),
                projectManifest = "reference/projects/catalog.json",
                projectManifestIdentity = Hash(inventory),
                apiManifests = new[]
                {
                    new
                    {
                        path = "reference/api/probe.json",
                        sha256 = Hash(facts),
                        model = "source-clr",
                        configuration = "Debug",
                        sourceRevision = new string('a', 40),
                        sourceTree = new string('b', 40),
                        target = (string?)null
                    }
                }
            })
        );
    }

    protected void Write(
        string relative,
        string text
    ) {
        string path = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, text);
    }

    protected static string Body(string text) => text[(text.IndexOf("\n---", 4, StringComparison.Ordinal) + 4)..];

    protected static string Hash(string text) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
}
