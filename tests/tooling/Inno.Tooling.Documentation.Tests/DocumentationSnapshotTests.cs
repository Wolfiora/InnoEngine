using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Documentation.Tests;

public sealed class DocumentationSnapshotTests : IDisposable
{
    private const string C_SYMBOL = "M:Inno.Probe.Read(System.String)";
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDocumentationSnapshots", Guid.NewGuid().ToString("N"));
    private string source => Path.Combine(m_root, "source");
    private string cache => Path.Combine(m_root, "artifacts", "documentation");

    public DocumentationSnapshotTests()
    {
        byte[] projects = JsonSerializer.SerializeToUtf8Bytes(new[]
        {
            new { name = "Inno.Probe", path = "src/Inno.Probe.csproj", entry = "probe", surfaces = new[] { "host-runtime", "runtime-script" } }
        });
        Write("reference/projects/catalog.json", Encoding.UTF8.GetString(projects));
        Write("reference/engine.lock.json", JsonSerializer.Serialize(new
        {
            repository = "https://github.com/Wolfiora/InnoEngine", revision = new string('a', 40), sourceTree = new string('b', 40),
            projectManifest = "reference/projects/catalog.json", projectManifestIdentity = Hash(projects), toolingRevision = new string('a', 40)
        }));
        Write("content/en/probe.md", Page("probe", "Probe", "## Ownership\n\nBorrow the lease; the owner releases it after the final consumer.\n",
            "kind: project\nowners: [Inno.Probe]\naliases: [ProbeStyle, 控制台颜色]\n"));
        Write("content/en/api.md", Api("probe-api", "host-runtime"));
    }

    public void Dispose()
    {
        // All test-owned sessions are disposed before removing this uniquely allocated fixture boundary.
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public async Task PreparedQueriesNeedNeitherSourceCheckoutNorAnSdkAndNeverRewriteTheIndex()
    {
        DocumentationResult indexed = await IndexAsync();
        Dictionary<string, string> before = Files();
        Directory.Move(source, Path.Combine(m_root, "unavailable-source"));
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationResult result = await snapshot.QueryAsync(new("search", "ProbeStyle"));
        Assert.Equal("Found", result.status);
        Assert.Equal("probe", result.hits[0].pageId);
        Assert.All(result.hits, hit => Assert.Equal("Excerpt", hit.completeness));
        DocumentationResult read = await snapshot.QueryAsync(new("read", "probe", section: "ownership"));
        Assert.Contains("final consumer", read.hits.Single().text);
        Assert.Equal("Unverified", read.sourceApiStatus);
        Assert.Equal(before.OrderBy(pair => pair.Key), Files().OrderBy(pair => pair.Key));
    }

    [Fact]
    public async Task RefreshDetectsSameLengthTimestampPreservingEditsWhileOldReadersStayConsistent()
    {
        DocumentationResult indexed = await IndexAsync();
        using DocumentationSnapshot old = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        string path = Path.Combine(source, "content/en/probe.md");
        DateTime timestamp = File.GetLastWriteTimeUtc(path);
        File.WriteAllText(path, File.ReadAllText(path).Replace("Borrow", "Retain", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, timestamp);
        DocumentationResult updated = await IndexAsync();
        Assert.NotEqual(indexed.snapshot, updated.snapshot);
        using DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache);
        Assert.Contains("Borrow", (await old.QueryAsync(new("read", "probe"))).hits.Single().text);
        Assert.Contains("Retain", (await current.QueryAsync(new("read", "probe"))).hits.Single().text);
    }

    [Fact]
    public async Task ConfirmingUnchangedContentDoesNotWaitForOrRewriteAnOpenGeneration()
    {
        DocumentationResult initial = await IndexAsync();
        using DocumentationSnapshot open = await DocumentationStore.OpenAsync(cache);
        string manifest = Path.Combine(cache, "generations", initial.snapshot, "manifest.json");
        DateTime timestamp = File.GetLastWriteTimeUtc(manifest);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        DocumentationResult confirmed = await DocumentationStore.IndexAsync(m_root, source, cache, cancellation.Token);
        Assert.Equal(initial.snapshot, confirmed.snapshot);
        Assert.Equal(timestamp, File.GetLastWriteTimeUtc(manifest));
    }

    [Fact]
    public async Task ExactSymbolsReturnExplicitContractRelationshipsAndSurfaceAmbiguity()
    {
        await IndexAsync();
        using (DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache))
        {
            DocumentationResult symbol = await snapshot.QueryAsync(new("symbol", C_SYMBOL, "Inno.Probe", "host-runtime"));
            Assert.Equal("Partial", symbol.status);
            Assert.Contains("Read(string key)", symbol.hits.Single().text);
            Assert.Contains(symbol.continuations, next => next.pageId == "probe" && next.section == "ownership");
            DocumentationResult resource = await snapshot.QueryAsync(new("resource", symbol.hits.Single().resource));
            Assert.Equal(symbol.hits.Single().text, resource.hits.Single().text);
        }
        Write("content/en/script.md", Api("probe-script", "runtime-script"));
        await IndexAsync();
        using DocumentationSnapshot updated = await DocumentationStore.OpenAsync(cache);
        DocumentationResult ambiguous = await updated.QueryAsync(new("symbol", C_SYMBOL, "Inno.Probe"));
        Assert.Equal("Ambiguous", ambiguous.status);
        Assert.Equal(2, ambiguous.hits.Count);
        Assert.Equal("Ambiguous", (await updated.QueryAsync(new("symbol", "Read", "Inno.Probe"))).status);
    }

    [Fact]
    public async Task EmptyMissingMismatchAndCancellationRemainDistinct()
    {
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Empty", (await snapshot.QueryAsync(new("search", "NoSuchConcept"))).status);
        Assert.Equal("Missing", (await snapshot.QueryAsync(new("read", "../../private.md"))).status);
        Assert.Equal("Missing", (await snapshot.QueryAsync(new("symbol", "M:Missing"))).status);
        Assert.Equal("Found", (await snapshot.QueryAsync(new("search", "控制台颜色"))).status);
        using DocumentationSnapshot mismatched = await DocumentationStore.OpenAsync(cache, expectedSourceTree: new string('c', 40));
        Assert.Equal("Mismatched", (await mismatched.QueryAsync(new("read", "probe"))).status);
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => snapshot.QueryAsync(new("read", "probe"), canceled.Token));
        Assert.Equal("Found", (await snapshot.QueryAsync(new("read", "probe"))).status);
    }

    [Fact]
    public async Task CorruptionFailsReadsAndExplicitRefreshRepairsOnlyTheOwnedPayload()
    {
        DocumentationResult initial = await IndexAsync();
        string[] payloads = Directory.GetFiles(Path.Combine(cache, "pages"), "page.json", SearchOption.AllDirectories);
        foreach (string payload in payloads)
            File.WriteAllText(payload, "{corrupt");
        using (DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache))
            await Assert.ThrowsAsync<InvalidDataException>(() => snapshot.QueryAsync(new("read", "probe")));
        DocumentationResult repaired = await IndexAsync();
        Assert.Equal(initial.snapshot, repaired.snapshot);
        using DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Found", (await current.QueryAsync(new("read", "probe"))).status);
    }

    [Theory]
    [InlineData("<!-- api-include: ../../../../outside.md -->", typeof(IOException))]
    [InlineData("<!-- api-include: api.md -->", typeof(InvalidDataException))]
    [InlineData("<!-- api-include: missing.md -->", typeof(InvalidDataException))]
    public async Task InvalidInclusionsNeverReplaceTheLastCompleteGeneration(
        string include,
        Type expectedFailure
    ) {
        await IndexAsync();
        byte[] pointer = File.ReadAllBytes(Path.Combine(cache, "current.json"));
        Write("content/en/api.md", Api("probe-api", "host-runtime") + "\n" + include + "\n");
        await Assert.ThrowsAsync(expectedFailure, async () => await IndexAsync());
        Assert.Equal(pointer, File.ReadAllBytes(Path.Combine(cache, "current.json")));
    }

    [Fact]
    public async Task MovesKeepPageIdentityAndDeletesInvalidateOnlyTheNewSnapshot()
    {
        Write("content/en/old.md", Page("movable", "Movable", "UniqueMarker\n"));
        DocumentationResult initial = await IndexAsync();
        using DocumentationSnapshot before = await DocumentationStore.OpenAsync(cache);
        File.Move(Path.Combine(source, "content/en/old.md"), Path.Combine(source, "content/en/new.md"));
        DocumentationResult moved = await IndexAsync();
        Assert.NotEqual(initial.snapshot, moved.snapshot);
        using (DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache))
            Assert.Equal("content/en/new.md", (await current.QueryAsync(new("read", "movable"))).hits.Single().page);
        File.Delete(Path.Combine(source, "content/en/new.md"));
        await IndexAsync();
        using DocumentationSnapshot deleted = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Missing", (await deleted.QueryAsync(new("read", "movable"))).status);
        Assert.Equal("Found", (await before.QueryAsync(new("read", "movable"))).status);
    }

    [Fact]
    public async Task LargeSectionsRoundTripWithoutLosingTextOrSurrogatePairs()
    {
        string body = "## Long contract\n\n" + string.Concat(Enumerable.Repeat("The caller retains ownership until every consumer exits. 🦊\n", 700));
        Write("content/en/long.md", Page("long", "Long page", body));
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        var recovered = new StringBuilder();
        int offset = 0;
        do
        {
            DocumentationResult read = await snapshot.QueryAsync(new("read", "long", section: "long-contract", offset: offset));
            recovered.Append(read.hits.Single().text);
            if (read.continuations.Count == 0)
                break;
            int next = read.continuations.Single().offset;
            Assert.True(next > offset);
            offset = next;
        } while (true);
        Assert.Equal(body, recovered.ToString());
    }

    [Fact]
    public async Task ConcurrentWritersAndReadersPublishConsistentGenerations()
    {
        DocumentationResult[] results = await Task.WhenAll(Enumerable.Range(0, 3).Select(_ => IndexAsync()));
        Assert.Single(results.Select(result => result.snapshot).Distinct());
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult[] reads = await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => snapshot.QueryAsync(new("read", "probe"))));
        Assert.All(reads, result => Assert.Equal(results[0].snapshot, result.snapshot));
        byte[] pointer = File.ReadAllBytes(Path.Combine(cache, "current.json"));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => DocumentationStore.IndexAsync(m_root, source, cache, cancellation.Token));
        Assert.Equal(pointer, File.ReadAllBytes(Path.Combine(cache, "current.json")));
    }

    private Task<DocumentationResult> IndexAsync() => DocumentationStore.IndexAsync(m_root, source, cache);

    [Fact]
    public async Task TaskHeadingsRankAboveTutorialsThatMerelyDemonstrateTheQuery()
    {
        Write("content/en/task.md", Page("z-task", "Customize Console **presentation**", "Inspect the panel style owner.\n"));
        Write("content/en/query.md", Page("a-query", "Search documentation", "Example: search Console presentation.\n"));
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult result = await snapshot.QueryAsync(new("search", "Console presentation"));
        Assert.Equal("z-task", result.hits[0].pageId);
        Assert.Equal("customize-console-presentation", result.hits[0].section);
    }

    [Fact]
    public async Task StructuredPaginationAndExactSymbolSearchNeedNoConsoleParsing()
    {
        Write("content/en/second.md", Page("second", "Ownership details", "Borrow the lease; observe the ownership contract.\n"));
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult first = await snapshot.QueryAsync(new("search", "ownership", limit: 1));
        Assert.NotNull(first.nextRequest);
        DocumentationResult second = await snapshot.QueryAsync(first.nextRequest!);
        Assert.NotEqual(first.hits.Single().resource, second.hits.Single().resource);
        DocumentationResult exact = await snapshot.QueryAsync(new("search", C_SYMBOL));
        Assert.Equal(C_SYMBOL, exact.hits.Single().symbol);
        Assert.Equal("Excerpt", exact.hits.Single().completeness);
        Assert.Equal("Exact qualified symbol", exact.hits.Single().reason);
    }

    private void Write(
        string path,
        string text
    ) {
        string target = Path.Combine(source, path);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        File.WriteAllText(target, text, new UTF8Encoding(false));
    }

    private Dictionary<string, string> Files() => Directory.GetFiles(cache, "*", SearchOption.AllDirectories)
        .Where(static path => !path.EndsWith(".lock", StringComparison.Ordinal))
        .ToDictionary(path => Path.GetRelativePath(cache, path), path => Hash(File.ReadAllBytes(path)), StringComparer.Ordinal);

    private static string Hash(byte[] bytes) => Convert.ToHexStringLower(SHA256.HashData(bytes));

    private static string Page(
        string uid,
        string title,
        string body,
        string metadata = "kind: explanation\n"
    ) => "---\nuid: " + uid + "\ntitle: " + title + "\n" + metadata + "---\n# " + title + "\n\n" + body;

    private static string Api(
        string uid,
        string surface
    ) => Page(uid, "Probe API", "## Read\n\n<!-- api: " + C_SYMBOL + " " + new string('a', 64)
        + " -->\n\n| Declaration | Contract |\n| --- | --- |\n| <a id=\"api-" + Hash(Encoding.UTF8.GetBytes(C_SYMBOL))[..16]
        + "\"></a>`Read(string key)` | Returns the value; caller owns the lease. |\n",
        "kind: api\nowners: [Inno.Probe]\napiSurface: " + surface + "\ncontracts: [probe#ownership]\n");
}
