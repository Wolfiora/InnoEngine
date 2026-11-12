using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.IO;
using Xunit;

namespace Inno.Tooling.Documentation.Tests;

public sealed class DocumentationEvidenceTests : IDisposable
{
    private readonly string m_root = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoDocumentationEvidence", Guid.NewGuid().ToString("N"));
    private string cache => Path.Combine(m_root, "cache");
    private string source => Path.Combine(m_root, "source");

    public void Dispose()
    {
        if (Directory.Exists(m_root)) Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public async Task TrustedSourceEvidenceIsIndependentFromContentFreshnessAndQueryCompleteness()
    {
        JsonNode certificate = await PrepareAsync();
        DocumentationEvidence evidence = await ReadAsync(certificate);
        using DocumentationSnapshot raw = await DocumentationStore.OpenAsync(cache);
        using DocumentationSnapshot verified = await DocumentationStore.OpenAsync(cache, sourceEvidence: evidence);
        Assert.Equal(raw.identity, verified.identity);
        Assert.Equal("Unverified", (await raw.QueryAsync(new("status"))).sourceApiStatus);
        Assert.Equal("Verified", (await verified.ExportCatalogAsync()).sourceApiStatus);
        Assert.Equal(evidence.identity, (await verified.ExportCatalogAsync()).sourceEvidenceIdentity);
        string manifest = Path.Combine(cache, "generations", verified.identity, "manifest.json");
        byte[] original = File.ReadAllBytes(manifest);
        File.WriteAllText(Path.Combine(source, "content/en/probe.md"), "Mutable authoring changes are not read by a prepared session.");
        File.WriteAllText(Path.Combine(m_root, "certificate.json"), "The imported immutable value does not retain a file dependency.");
        DocumentationResult[] results = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => verified.QueryAsync(new("search", "ownership"))));
        Assert.All(results, result =>
        {
            Assert.Equal("Verified", result.sourceApiStatus);
            Assert.Equal(evidence.identity, result.sourceEvidenceIdentity);
            Assert.Equal("Excerpt", Assert.Single(result.hits).completeness);
        });
        Assert.Equal(original, File.ReadAllBytes(manifest));
        Write("content/en/probe.md", "---\nuid: probe\nkind: project\nowners: [Probe]\n---\n# Updated task\n\nOwnership stays explicit.\n");
        await DocumentationStore.IndexAsync(m_root, source, cache);
        using DocumentationSnapshot prose = await DocumentationStore.OpenAsync(cache, sourceEvidence: evidence);
        Assert.NotEqual(raw.identity, prose.identity);
        Assert.Equal("Verified", (await prose.ExportCatalogAsync()).sourceApiStatus);
        using DocumentationSnapshot mismatch = await DocumentationStore.OpenAsync(cache, expectedSourceTree: new string('c', 40), sourceEvidence: evidence);
        Assert.Equal("Mismatched", (await mismatch.QueryAsync(new("status"))).status);
        Assert.Equal("Unverified", (await mismatch.QueryAsync(new("status"))).sourceApiStatus);
    }

    [Theory]
    [InlineData("repository")]
    [InlineData("sourceTree")]
    [InlineData("projectManifestIdentity")]
    [InlineData("identity")]
    [InlineData("path")]
    [InlineData("configuration")]
    [InlineData("target")]
    [InlineData("projects")]
    [InlineData("extra-reference")]
    public async Task MatchingDigestCannotCertifyDifferentSourceOrReferenceCoverage(string change)
    {
        JsonNode certificate = await PrepareAsync();
        if (change == "repository") certificate[change] = "https://example.invalid/other";
        else if (change == "sourceTree") certificate[change] = new string('c', 40);
        else if (change == "projectManifestIdentity") certificate[change] = new string('c', 64);
        else if (change == "extra-reference")
        {
            JsonNode extra = certificate["references"]![0]!.DeepClone();
            extra["path"] = "reference/api/extra.json";
            certificate["references"]!.AsArray().Add(extra);
        }
        else if (change == "projects") certificate["references"]![0]![change] = new JsonArray("Other");
        else certificate["references"]![0]![change] = change == "identity" ? new string('c', 64) : "different";
        DocumentationEvidence evidence = await ReadAsync(certificate);
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationStore.OpenAsync(cache, sourceEvidence: evidence));
        using DocumentationSnapshot retained = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Unverified", (await retained.ExportCatalogAsync()).sourceApiStatus);
    }

    [Theory]
    [InlineData("missing-reference")]
    [InlineData("duplicate-reference")]
    [InlineData("duplicate-project")]
    [InlineData("unsupported-model")]
    [InlineData("missing-producer")]
    [InlineData("bad-producer")]
    [InlineData("wrong-format")]
    public async Task IncompleteOrAmbiguousCertificatesAreRejected(string change)
    {
        JsonNode certificate = await PrepareAsync();
        switch (change)
        {
            case "missing-reference": certificate["references"] = new JsonArray(); break;
            case "duplicate-reference": certificate["references"]!.AsArray().Add(certificate["references"]![0]!.DeepClone()); break;
            case "duplicate-project": certificate["references"]![0]!["projects"] = new JsonArray("Probe", "Probe"); break;
            case "unsupported-model": certificate["references"]![0]!["model"] = "unknown"; break;
            case "missing-producer": certificate["producers"] = new JsonArray(); break;
            case "bad-producer": certificate["producers"]![0]!["receiptIdentity"] = "not-a-digest"; break;
            case "wrong-format": certificate["format"] = "unknown"; break;
        }
        await Assert.ThrowsAsync<InvalidDataException>(() => ReadAsync(certificate));
    }

    [Fact]
    public async Task ImportRejectsTamperingCancellationAndImplicitCliTrust()
    {
        JsonNode certificate = await PrepareAsync();
        DocumentationEvidence evidence = await ReadAsync(certificate);
        string file = Path.Combine(m_root, "certificate.json");
        File.AppendAllText(file, " ");
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationEvidence.ReadAsync(file, evidence.identity));
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => DocumentationEvidence.ReadAsync(file, Hash(File.ReadAllBytes(file)), canceled.Token));
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationCommand.ExecuteAsync(["status", "--cache-root", cache, "--evidence-file", file]));
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationCommand.ExecuteAsync(["index", "--docs-root", source,
            "--evidence-file", file, "--evidence-sha256", evidence.identity]));
    }

    private async Task<JsonNode> PrepareAsync()
    {
        byte[] projects = JsonSerializer.SerializeToUtf8Bytes(new[] { new { name = "Probe", path = "src/Probe.csproj", surfaces = new[] { "source-clr" } } });
        byte[] api = JsonSerializer.SerializeToUtf8Bytes(new { model = "source-clr", configuration = "Debug", target = (string?)null,
            projects = new[] { new { name = "Probe", project = "src/Probe.csproj", symbols = Array.Empty<object>() } } });
        Write("reference/projects/catalog.json", Encoding.UTF8.GetString(projects));
        Write("reference/api/probe.json", Encoding.UTF8.GetString(api));
        Write("reference/engine.lock.json", JsonSerializer.Serialize(new
        {
            repository = "https://github.com/Wolfiora/InnoEngine", revision = new string('a', 40), sourceTree = new string('b', 40),
            toolingRevision = new string('a', 40), projectManifest = "reference/projects/catalog.json", projectManifestIdentity = Hash(projects),
            apiManifests = new[] { new { path = "reference/api/probe.json", sha256 = Hash(api), model = "source-clr", configuration = "Debug",
                target = (string?)null, sourceRevision = new string('a', 40), sourceTree = new string('b', 40) } }
        }));
        Write("content/en/probe.md", "---\nuid: probe\nkind: project\nowners: [Probe]\n---\n# Ownership task\n\nThe caller owns the result.\n");
        await DocumentationStore.IndexAsync(m_root, source, cache);
        return JsonSerializer.SerializeToNode(new
        {
            format = "inno-documentation-source/1", repository = "https://github.com/Wolfiora/InnoEngine", sourceTree = new string('b', 40),
            projectManifestIdentity = Hash(projects),
            references = new[] { new { path = "reference/api/probe.json", identity = Hash(api), model = "source-clr", configuration = "Debug",
                target = (string?)null, projects = new[] { "Probe" } } },
            producers = new[] { new { origin = "fixture://reconciled-local-producer", receiptIdentity = new string('d', 64) } }
        })!;
    }

    private Task<DocumentationEvidence> ReadAsync(JsonNode certificate)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(certificate);
        string file = Path.Combine(m_root, "certificate.json");
        File.WriteAllBytes(file, bytes);
        return DocumentationEvidence.ReadAsync(file, Hash(bytes));
    }

    private void Write(string relative, string content)
    {
        string file = Path.Combine(source, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, content);
    }

    private static string Hash(byte[] bytes) => Convert.ToHexStringLower(SHA256.HashData(bytes));
}
