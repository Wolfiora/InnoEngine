using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Integration.Tests;

[Collection(nameof(DocumentationSnapshotFixture))]
public sealed class DocumentationSnapshotTests : DocumentationSnapshotFixture
{
    [Fact]
    public async Task RestoredImmutablePayloadsAcquireLocalOwnershipBeforeOpening()
    {
        DocumentationResult first = await IndexAsync();
        string imported = Path.Combine(m_root, "artifacts", "imported");
        string manifest = Path.Combine("generations", first.snapshot!, "manifest.json");
        foreach (string file in Directory.GetFiles(cache, "page.json", SearchOption.AllDirectories)
            .Append(Path.Combine(cache, "current.json"))
            .Append(Path.Combine(cache, manifest))
        ) {
            string destination = Path.Combine(imported, Path.GetRelativePath(cache, file));
            Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
            File.Copy(file, destination);
        }
        byte[] original = File.ReadAllBytes(Path.Combine(imported, manifest));
        DocumentationResult restored = await DocumentationStore.IndexAsync(m_root, source, imported);
        Assert.Equal(first.snapshot, restored.snapshot);
        Assert.Contains("parsed 0", restored.message);
        Assert.Equal(original, File.ReadAllBytes(Path.Combine(imported, manifest)));
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(imported, restored.snapshot);
        Assert.NotEmpty((await snapshot.ExportCatalogAsync()).pages);
        // Confirming an already protected snapshot must not wait on its reading lease.
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        Assert.Equal(restored.snapshot, (await DocumentationStore.IndexAsync(m_root, source, imported, cancellation.Token)).snapshot);
    }

    [Fact]
    public async Task UnclosedFencesCannotReplaceThePublishedSnapshot()
    {
        DocumentationResult previous = await IndexAsync();
        Write("content/en/fence.md", Page("fence", "Fence", "~~~cs\nint value = 1;\n"));
        InvalidDataException error = await Assert.ThrowsAsync<InvalidDataException>(() => IndexAsync());
        Assert.Contains("unclosed code fence", error.Message);
        using DocumentationSnapshot retained = await DocumentationStore.OpenAsync(cache, previous.snapshot);
        Assert.Equal(previous.snapshot, (await retained.ExportCatalogAsync()).snapshot);
    }

    [Fact]
    public async Task ReferencePayloadsRemainReadableAfterCheckoutChangesAndShareImmutableStorage()
    {
        const string C_REFERENCE = "reference/api/probe.json";
        byte[] firstBytes = BindReference(C_REFERENCE, "initial");
        DocumentationResult first = await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, first.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationReferenceInfo reference = Assert.Single(catalog.references, item => item.model == "host-runtime");
        Assert.Equal(new[] { "Inno.Probe" }, reference.projects);
        Assert.Equal(Hash(firstBytes), reference.identity);
        Assert.Equal(firstBytes, await snapshot.ReadReferenceAsync(C_REFERENCE));
        string payload = Path.Combine(cache, "references", reference.identity, "reference.json");
        DateTime timestamp = File.GetLastWriteTimeUtc(payload);
        Write("content/en/task.md", Page("task", "A new task", "Read the existing API contracts."));
        DocumentationResult second = await IndexAsync();
        Assert.NotEqual(first.snapshot, second.snapshot);
        Assert.Equal(timestamp, File.GetLastWriteTimeUtc(payload));
        Assert.Equal(2, Directory.GetDirectories(Path.Combine(cache, "references")).Length);
        BindReference(C_REFERENCE, "updated");
        DocumentationResult third = await IndexAsync();
        using DocumentationSnapshot refreshed = await DocumentationStore.OpenAsync(cache, third.snapshot);
        Assert.Equal(firstBytes, await snapshot.ReadReferenceAsync(C_REFERENCE));
        Assert.NotEqual(firstBytes, await refreshed.ReadReferenceAsync(C_REFERENCE));
        Dictionary<string, string> before = Files();
        Directory.Move(source, Path.Combine(m_root, "unavailable-reference-source"));
        byte[][] readers = await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => snapshot.ReadReferenceAsync(C_REFERENCE)));
        Assert.All(readers, bytes => Assert.Equal(firstBytes, bytes));
        Assert.Equal(before.OrderBy(static pair => pair.Key), Files().OrderBy(static pair => pair.Key));
        await Assert.ThrowsAsync<ArgumentException>(() => snapshot.ReadReferenceAsync("../escape.json"));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => snapshot.ReadReferenceAsync(C_REFERENCE, new CancellationToken(true))
        );
    }

    [Fact]
    public async Task CorruptOrMismatchedReferencesCannotBecomePublishedFacts()
    {
        const string C_REFERENCE = "reference/api/probe.json";
        byte[] expected = BindReference(C_REFERENCE, "initial");
        DocumentationResult first = await IndexAsync();
        byte[] pointer = File.ReadAllBytes(Path.Combine(cache, "current.json"));
        string input = Path.Combine(source, C_REFERENCE);
        DateTime timestamp = File.GetLastWriteTimeUtc(input);
        File.WriteAllBytes(
            input,
            Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(expected).Replace("initial", "changed", StringComparison.Ordinal))
        );
        File.SetLastWriteTimeUtc(input, timestamp);
        await Assert.ThrowsAsync<InvalidDataException>(IndexAsync);
        Assert.Equal(pointer, File.ReadAllBytes(Path.Combine(cache, "current.json")));
        File.WriteAllBytes(input, expected);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, first.snapshot);
        string payload = Path.Combine(cache, "references", Hash(expected), "reference.json");
        File.WriteAllText(payload, "corrupt");
        await Assert.ThrowsAsync<InvalidDataException>(() => snapshot.ReadReferenceAsync(C_REFERENCE));
        await IndexAsync();
        Assert.Equal(expected, await snapshot.ReadReferenceAsync(C_REFERENCE));
        using DocumentationSnapshot mismatch = await DocumentationStore.OpenAsync(cache, first.snapshot, new string('c', 40));
        await Assert.ThrowsAsync<InvalidOperationException>(() => mismatch.ReadReferenceAsync(C_REFERENCE));
    }

    [Theory]
    [InlineData("target")]
    [InlineData("configuration")]
    [InlineData("model")]
    [InlineData("sourceRevision")]
    [InlineData("duplicate")]
    [InlineData("escape")]
    public async Task InvalidReferenceBindingsFailBeforePublication(string changed)
    {
        BindReference("reference/api/probe.json", "initial");
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/engine.lock.json")))!;
        JsonArray references = binding["apiManifests"]!.AsArray();
        if (changed == "duplicate")
        {
            references.Add(references[0]!.DeepClone());
        }
        else if (changed == "escape")
        {
            references[0]!["path"] = "../outside.json";
        }
        else
        {
            references[0]![changed] = "invalid";
        }
        Write("reference/engine.lock.json", binding.ToJsonString());
        if (changed == "escape")
        {
            await Assert.ThrowsAnyAsync<IOException>(IndexAsync);
        }
        else
        {
            await Assert.ThrowsAsync<InvalidDataException>(IndexAsync);
        }
        Assert.False(File.Exists(Path.Combine(cache, "current.json")));
    }

    [Theory]
    [InlineData("<script>alert('unexpected')</script>")]
    [InlineData("A <img src=x onerror=alert(1)> side effect.")]
    [InlineData("<a id=\"api-deadbeef\" onclick=\"alert(1)\"></a>")]
    [InlineData("<!-- harmless comment --><iframe src=\"https://example.com\"></iframe>")]
    public async Task ExecutableOrPresentationHtmlCannotReplaceThePublishedSnapshot(string html)
    {
        DocumentationResult initial = await IndexAsync();
        Write("content/en/unsafe.md", Page("unsafe", "Unsafe", html));
        await Assert.ThrowsAsync<InvalidDataException>(() => IndexAsync());
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        Assert.Equal(initial.snapshot, snapshot.identity);
        Assert.Equal("Missing", (await snapshot.QueryAsync(new DocumentationRequest("read", "unsafe"))).status);
    }

    [Theory]
    [InlineData("[Missing](absent.md)", typeof(InvalidDataException))]
    [InlineData("[Missing](probe.md#absent)", typeof(InvalidDataException))]
    [InlineData("[Escape](../../../outside.md)", typeof(IOException))]
    [InlineData("[Executable](javascript:alert)", typeof(InvalidDataException))]
    public async Task BrokenOrUnsafeLinksCannotPublishANewGeneration(
        string link,
        Type failureType
    ) {
        DocumentationResult original = await IndexAsync();
        Write("content/en/broken.md", Page("broken", "Broken", link));
        Exception? failure = await Record.ExceptionAsync(() => IndexAsync());
        Assert.NotNull(failure);
        Assert.Equal(failureType, failure.GetType());
        using DocumentationSnapshot retained = await DocumentationStore.OpenAsync(cache);
        Assert.Equal(original.snapshot, retained.identity);
    }

    [Fact]
    public async Task ReferenceLinksAndEncodedHeadingTargetsUseCanonicalParsing()
    {
        Write(
            "content/en/links.md",
            Page(
                "links",
                "Links",
                "[Ownership][target]\n\n[target]: probe.md#ownership\n\n[Self](#links)\n\n[API](api.md#api-"
                + Hash(Encoding.UTF8.GetBytes(C_SYMBOL))[..16]
                + ")\n"
            )
        );
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationPageInfo page = (await snapshot.ExportCatalogAsync()).pages.Single(page => page.id == "links");
        Assert.Contains("probe.md#ownership", page.links);
        Assert.Contains("#links", page.links);
    }

    [Fact]
    public async Task PreparedQueriesNeedNeitherSourceCheckoutNorAnSdkAndNeverRewriteTheIndex()
    {
        DocumentationResult indexed = await IndexAsync();
        Dictionary<string, string> before = Files();
        Directory.Move(source, Path.Combine(m_root, "unavailable-source"));
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationResult result = await snapshot.QueryAsync(new DocumentationRequest("search", "ProbeStyle"));
        Assert.Equal("Found", result.status);
        Assert.Equal("probe", result.hits[0].pageId);
        Assert.All(result.hits, hit => Assert.Equal("Excerpt", hit.completeness));
        DocumentationResult read = await snapshot.QueryAsync(new DocumentationRequest("read", "probe", section: "ownership"));
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
        Assert.Contains("Borrow", (await old.QueryAsync(new DocumentationRequest("read", "probe"))).hits.Single().text);
        Assert.Contains("Retain", (await current.QueryAsync(new DocumentationRequest("read", "probe"))).hits.Single().text);
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

    [Theory]
    [InlineData(false, "parsed 0, reused 2")]
    [InlineData(true, "parsed 2, reused 0")]
    public async Task QueryChangesReuseParsedPagesButParserChangesRequireFreshParsing(
        bool changeParser,
        string expectedWork
    ) {
        DocumentationResult initial = await IndexAsync();
        string manifestPath = Path.Combine(cache, "generations", initial.snapshot, "manifest.json");
        JsonNode manifest = JsonNode.Parse(File.ReadAllText(manifestPath))!;
        manifest["schema"] = "prior-query-implementation";
        if (changeParser)
        {
            manifest["parserSchema"] = "prior-parser-implementation";
        }
        manifest["identity"] = string.Empty;
        string previousIdentity = Hash(JsonSerializer.SerializeToUtf8Bytes(manifest));
        manifest["identity"] = previousIdentity;
        string previousDirectory = Path.Combine(cache, "generations", previousIdentity);
        Directory.CreateDirectory(previousDirectory);
        File.WriteAllBytes(Path.Combine(previousDirectory, "manifest.json"), JsonSerializer.SerializeToUtf8Bytes(manifest));
        File.WriteAllBytes(Path.Combine(cache, "leases", previousIdentity + ".lock"), []);
        File.WriteAllBytes(Path.Combine(cache, "current.json"), JsonSerializer.SerializeToUtf8Bytes(previousIdentity));
        string[] payloads = Directory.GetFiles(Path.Combine(cache, "pages"), "page.json", SearchOption.AllDirectories);
        Dictionary<string, DateTime> timestamps = payloads.ToDictionary(
            static path => path,
            File.GetLastWriteTimeUtc,
            StringComparer.Ordinal
        );

        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationStore.OpenAsync(cache));
        DocumentationResult refreshed = await IndexAsync();
        Assert.Contains(expectedWork, refreshed.message);
        Assert.Equal(initial.snapshot, refreshed.snapshot);
        if (!changeParser)
        {
            Assert.All(payloads, path => Assert.Equal(timestamps[path], File.GetLastWriteTimeUtc(path)));
        }
        using DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Found", (await current.QueryAsync(new DocumentationRequest("read", "probe"))).status);
    }

    [Fact]
    public async Task CorruptionFailsReadsAndExplicitRefreshRepairsOnlyTheOwnedPayload()
    {
        DocumentationResult initial = await IndexAsync();
        string[] payloads = Directory.GetFiles(Path.Combine(cache, "pages"), "page.json", SearchOption.AllDirectories);
        foreach (string payload in payloads)
        {
            File.WriteAllText(payload, "{corrupt");
        }
        using (DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache))
        {
            await Assert.ThrowsAsync<InvalidDataException>(() => snapshot.QueryAsync(new DocumentationRequest("read", "probe")));
        }
        DocumentationResult repaired = await IndexAsync();
        Assert.Equal(initial.snapshot, repaired.snapshot);
        using DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Found", (await current.QueryAsync(new DocumentationRequest("read", "probe"))).status);
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
        {
            Assert.Equal("content/en/new.md", (await current.QueryAsync(new DocumentationRequest("read", "movable"))).hits.Single().page);
        }
        File.Delete(Path.Combine(source, "content/en/new.md"));
        await IndexAsync();
        using DocumentationSnapshot deleted = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Missing", (await deleted.QueryAsync(new DocumentationRequest("read", "movable"))).status);
        Assert.Equal("Found", (await before.QueryAsync(new DocumentationRequest("read", "movable"))).status);
    }

    [Fact]
    public async Task FirstConcurrentQueriesShareSearchStateWithoutPublishingOrRetainingCancellation()
    {
        await IndexAsync();
        Dictionary<string, string> before = Files();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() =>
            snapshot.QueryAsync(new DocumentationRequest("search", "ownership"), cancellation.Token));

        var start = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        Task<DocumentationResult>[] queries = Enumerable.Range(0, 12).Select(index => Task.Run(async () =>
        {
            await start.Task;
            return await snapshot.QueryAsync(
                (index % 3) switch
                {
                    0 => new DocumentationRequest("search", "ownership"),
                    1 => new DocumentationRequest("symbol", C_SYMBOL, "Inno.Probe", "host-runtime"),
                    _ => new DocumentationRequest("read", "probe", section: "ownership")
                }
            );
        })).ToArray();
        start.SetResult();
        DocumentationResult[] results = await Task.WhenAll(queries);
        for (int index = 0; index < results.Length; index++)
        {
            DocumentationResult result = results[index];
            Assert.Equal(snapshot.identity, result.snapshot);
            if (index % 3 == 1)
            {
                Assert.Equal(C_SYMBOL, Assert.Single(result.hits).symbol);
                Assert.Equal("probe", Assert.Single(result.continuations).pageId);
                Assert.Equal("ownership", result.continuations[0].section);
            }
            else
            {
                Assert.Equal("Found", result.status);
                Assert.Contains(result.hits, hit => hit.pageId == "probe" && hit.section == "ownership");
            }
        }
        Assert.Equal(before.OrderBy(pair => pair.Key), Files().OrderBy(pair => pair.Key));
    }

    [Fact]
    public async Task ConcurrentWritersAndReadersPublishConsistentGenerations()
    {
        for (int index = 0; index < 24; index++)
        {
            Write($"content/en/task-{index}.md", Page($"task-{index}", $"Task {index}", $"Original content {index}.\n"));
        }
        DocumentationResult[] results = await Task.WhenAll(Enumerable.Range(0, 3).Select(_ => IndexAsync()));
        Assert.Single(results.Select(result => result.snapshot).Distinct());
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult[] reads = await Task.WhenAll(
            Enumerable.Range(0, 10).Select(_ => snapshot.QueryAsync(new DocumentationRequest("read", "probe")))
        );
        Assert.All(reads, result => Assert.Equal(results[0].snapshot, result.snapshot));
        byte[] pointer = File.ReadAllBytes(Path.Combine(cache, "current.json"));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => DocumentationStore.IndexAsync(m_root, source, cache, cancellation.Token)
        );
        Assert.Equal(pointer, File.ReadAllBytes(Path.Combine(cache, "current.json")));
    }

    [Fact]
    public async Task CancelingPreparationWithAProtectedPageRetainsThePublishedGeneration()
    {
        DocumentationResult initial = await IndexAsync();
        string pointer = Path.Combine(cache, "current.json");
        byte[] selected = File.ReadAllBytes(pointer);
        string[] payloads = Directory.GetFiles(Path.Combine(cache, "pages"), "page.json", SearchOption.AllDirectories);
        string protectedPage = payloads.Single(
            path => JsonNode.Parse(File.ReadAllText(path))!["path"]!.GetValue<string>() == "content/en/probe.md"
        );
        string key = Path.GetFileName(Path.GetDirectoryName(protectedPage)!);
        using DocumentationSnapshot reader = await DocumentationStore.OpenAsync(cache);
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(30));
        for (int index = 0; index < 24; index++)
        {
            Write($"content/en/new-{index}.md", Page($"new-{index}", $"New {index}", "New generation content.\n"));
        }
        using (await FileLease.AcquireAsync(Path.Combine(cache, "leases", key + ".lock"), Timeout.InfiniteTimeSpan, cancellation.Token))
        {
            Task<DocumentationResult> preparation = DocumentationStore.IndexAsync(m_root, source, cache, cancellation.Token);
            try
            {
                while (Directory.GetFiles(Path.Combine(cache, "pages"), "page.json", SearchOption.AllDirectories).Length == payloads.Length)
                {
                    Assert.False(preparation.IsCompleted, "Preparation ended before publishing any independent page payload.");
                    await Task.Delay(10, cancellation.Token);
                }
                Assert.False(preparation.IsCompleted);
                cancellation.Cancel();
                await Assert.ThrowsAnyAsync<OperationCanceledException>(() => preparation);
                Assert.Equal(selected, File.ReadAllBytes(pointer));
                DocumentationResult retained = await reader.QueryAsync(new DocumentationRequest("read", "probe-api"));
                Assert.Equal("Partial", retained.status);
                Assert.Equal(initial.snapshot, retained.snapshot);
                Assert.Contains(retained.continuations, item => item.pageId == "probe" && item.section == "ownership");
            }
            finally
            {
                cancellation.Cancel();
                try
                {
                    await preparation;
                }
                catch (OperationCanceledException)
                {
                    // Drain every worker before releasing the protected page.
                }
            }
        }
        DocumentationResult resumed = await IndexAsync();
        Assert.NotEqual(initial.snapshot, resumed.snapshot);
        using DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache);
        Assert.Equal(26, (await current.ExportCatalogAsync()).pages.Count);
        Assert.Equal("Missing", (await reader.QueryAsync(new DocumentationRequest("read", "new-23"))).status);
        Assert.Equal("Found", (await current.QueryAsync(new DocumentationRequest("read", "new-23"))).status);
    }

    [Theory]
    [InlineData("exampleMode: guessed\nexampleProjects: [Inno.Probe]\n")]
    [InlineData("exampleMode: host\n")]
    [InlineData("exampleProjects: [Inno.Probe]\n")]
    [InlineData("exampleMode: host\nexampleProjects: [Inno.Missing]\n")]
    public async Task InvalidExampleContextsCannotReplaceThePublishedSnapshot(string metadata)
    {
        await IndexAsync();
        byte[] pointer = File.ReadAllBytes(Path.Combine(cache, "current.json"));
        Write(
            "content/en/example.md",
            Page("example", "Example", "```csharp\npublic class Example { }\n```\n", "kind: tutorial\n" + metadata)
        );
        await Assert.ThrowsAsync<InvalidDataException>(() => IndexAsync());
        Assert.Equal(pointer, File.ReadAllBytes(Path.Combine(cache, "current.json")));
    }

    [Fact]
    public async Task OrphanSymbolsAndDuplicateProjectIdentitiesCannotReplaceThePublishedGeneration()
    {
        DocumentationResult original = await IndexAsync();
        string api = File.ReadAllText(Path.Combine(source, "content/en/api.md"));
        Write("content/en/api.md", api.Replace("owners: [Inno.Probe]", "owners: []", StringComparison.Ordinal));
        await Assert.ThrowsAsync<InvalidDataException>(() => IndexAsync());
        Write("content/en/api.md", api);
        JsonNode inventory = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/projects/catalog.json")))!;
        inventory.AsArray().Add(inventory[0]!.DeepClone());
        SetProjectInventory(inventory);
        await Assert.ThrowsAnyAsync<ArgumentException>(() => IndexAsync());
        using DocumentationSnapshot unchanged = await DocumentationStore.OpenAsync(cache);
        Assert.Equal(original.snapshot, unchanged.identity);
    }

    [Fact]
    public async Task AProjectMoveChangesTheSnapshotThroughItsExplicitInventoryWithoutSdkWork()
    {
        DocumentationResult previous = await IndexAsync();
        JsonNode projects = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/projects/catalog.json")))!;
        projects[0]!["path"] = "src/moved/Inno.Probe.csproj";
        SetProjectInventory(projects);
        DocumentationResult moved = await IndexAsync();
        Assert.NotEqual(previous.snapshot, moved.snapshot);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, moved.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        byte[] bytes = await snapshot.ReadReferenceAsync("reference/projects/catalog.json");
        Assert.Contains("src/moved/Inno.Probe.csproj", Encoding.UTF8.GetString(bytes));
        Assert.Equal("probe", catalog.projects["Inno.Probe"]);
    }

    private void SetProjectInventory(JsonNode projects)
    {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(projects);
        Write("reference/projects/catalog.json", Encoding.UTF8.GetString(bytes));
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Hash(bytes);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    private byte[] BindReference(
        string path,
        string description
    ) {
        byte[] bytes = JsonSerializer.SerializeToUtf8Bytes(new
        {
            model = "host-runtime",
            configuration = "Debug",
            target = (string?)null,
            projects = new[]
            {
                new { name = "Inno.Probe", project = "src/Inno.Probe.csproj", symbols = Array.Empty<object>(), description }
            }
        });
        Write(path, Encoding.UTF8.GetString(bytes));
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/engine.lock.json")))!;
        binding["apiManifests"] = JsonSerializer.SerializeToNode(new[]
        {
            new
            {
                path,
                sha256 = Hash(bytes),
                model = "host-runtime",
                configuration = "Debug",
                target = (string?)null,
                sourceRevision = new string('a', 40),
                sourceTree = new string('b', 40)
            }
        });
        Write("reference/engine.lock.json", binding.ToJsonString());
        return bytes;
    }
}
