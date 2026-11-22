using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Documentation.Integration.Tests;

[Collection(nameof(DocumentationSnapshotFixture))]
public sealed class DocumentationSearchTests : DocumentationSnapshotFixture
{
    [Fact]
    public async Task AMemberNameDoesNotPolluteEveryOtherSectionOnItsTypePage()
    {
        Write(
            "content/en/api.md",
            Page(
                "probe-api",
                "Probe API",
                "## ReadOnly\n\n<!-- api: M:Inno.Probe.ReadOnly " + new string('a', 64)
                + " -->\n\n```csharp\npublic void ReadOnly();\n```\n\nRead the current value.\n"
                + "\n## WriteOnly\n\n<!-- api: M:Inno.Probe.WriteOnly " + new string('b', 64)
                + " -->\n\n```csharp\npublic void WriteOnly();\n```\n\nWrite a replacement value.\n",
                "kind: api\nowners: [Inno.Probe]\napiSurface: host-runtime\n"
            )
        );
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult result = await snapshot.QueryAsync(new DocumentationRequest("search", "WriteOnly"));
        Assert.Equal("writeonly", Assert.Single(result.hits).section);
        DocumentationResult exact = await snapshot.QueryAsync(new DocumentationRequest("search", "M:Inno.Probe.WriteOnly"));
        Assert.Equal("M:Inno.Probe.WriteOnly", Assert.Single(exact.hits).symbol);
    }

    [Fact]
    public async Task ExactSymbolsReturnExplicitContractRelationshipsAndSurfaceAmbiguity()
    {
        await IndexAsync();
        using (DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache))
        {
            DocumentationResult symbol = await snapshot.QueryAsync(
                new DocumentationRequest("symbol", C_SYMBOL, "Inno.Probe", "host-runtime")
            );
            Assert.Equal("Partial", symbol.status);
            Assert.Contains("Read(string key)", symbol.hits.Single().text);
            Assert.Contains(symbol.continuations, next => next.pageId == "probe" && next.section == "ownership");
            DocumentationResult resource = await snapshot.QueryAsync(new DocumentationRequest("resource", symbol.hits.Single().resource));
            Assert.Equal(symbol.hits.Single().text, resource.hits.Single().text);
        }
        Write("content/en/script.md", Api("probe-script", "runtime-script"));
        await IndexAsync();
        using DocumentationSnapshot updated = await DocumentationStore.OpenAsync(cache);
        DocumentationResult ambiguous = await updated.QueryAsync(new DocumentationRequest("symbol", C_SYMBOL, "Inno.Probe"));
        Assert.Equal("Ambiguous", ambiguous.status);
        Assert.Equal(2, ambiguous.hits.Count);
        Assert.Equal("Ambiguous", (await updated.QueryAsync(new DocumentationRequest("symbol", "Read", "Inno.Probe"))).status);
    }

    [Fact]
    public async Task EmptyMissingMismatchAndCancellationRemainDistinct()
    {
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        Assert.Equal("Empty", (await snapshot.QueryAsync(new DocumentationRequest("search", "NoSuchConcept"))).status);
        Assert.Equal("Missing", (await snapshot.QueryAsync(new DocumentationRequest("read", "../../private.md"))).status);
        Assert.Equal("Missing", (await snapshot.QueryAsync(new DocumentationRequest("symbol", "M:Missing"))).status);
        Assert.Equal("Found", (await snapshot.QueryAsync(new DocumentationRequest("search", "控制台颜色"))).status);
        using DocumentationSnapshot mismatched = await DocumentationStore.OpenAsync(cache, expectedSourceTree: new string('c', 40));
        Assert.Equal("Mismatched", (await mismatched.QueryAsync(new DocumentationRequest("read", "probe"))).status);
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => snapshot.QueryAsync(new DocumentationRequest("read", "probe"), canceled.Token)
        );
        Assert.Equal("Found", (await snapshot.QueryAsync(new DocumentationRequest("read", "probe"))).status);
    }

    [Fact]
    public async Task LargeSectionsRoundTripWithoutLosingTextOrSurrogatePairs()
    {
        string body = "## Long contract\n\n"
                      + string.Concat(Enumerable.Repeat("The caller retains ownership until every consumer exits. 🦊\n", 700));
        Write("content/en/long.md", Page("long", "Long page", body));
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        var recovered = new StringBuilder();
        int offset = 0;
        do
        {
            DocumentationResult read = await snapshot.QueryAsync(
                new DocumentationRequest("read", "long", section: "long-contract", offset: offset)
            );
            recovered.Append(read.hits.Single().text);
            if (read.continuations.Count == 0)
            {
                break;
            }
            int next = read.continuations.Single().offset;
            Assert.True(next > offset);
            offset = next;
        }
        while (true);
        Assert.Equal(body, recovered.ToString());
    }

    [Fact]
    public async Task TaskHeadingsRankAboveTutorialsThatMerelyDemonstrateTheQuery()
    {
        Write("content/en/task.md", Page("z-task", "Customize Console **presentation**", "Inspect the panel style owner.\n"));
        Write("content/en/query.md", Page("a-query", "Search documentation", "Example: search Console presentation.\n"));
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult result = await snapshot.QueryAsync(new DocumentationRequest("search", "Console presentation"));
        Assert.Equal("z-task", result.hits[0].pageId);
        Assert.Equal("customize-console-presentation", result.hits[0].section);
    }

    [Fact]
    public async Task TaskHeadingTermsOutrankAssemblyIndexesWithoutAnExactPhrase()
    {
        Write(
            "content/en/probe.md",
            Page(
                "probe",
                "Probe",
                "Read keyboard, pointer and text input in the task guide.\n\n## Ownership\n\nBorrow the lease.\n",
                "kind: project\nowners: [Inno.Probe]\n"
            )
        );
        Write(
            "content/en/task.md",
            Page(
                "z-task",
                "Read keyboard, pointer and text input",
                "Observe one immutable frame.\n",
                "kind: how-to\nowners: [Inno.Probe]\n"
            )
        );
        Write(
            "content/en/index.md",
            Page("a-index", "All members", "Read data. Keyboard values. Pointer fields. Text formatting. Input functions.\n")
        );
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        Dictionary<string, string> before = Files();
        DocumentationResult result = await snapshot.QueryAsync(new DocumentationRequest("search", "read keyboard pointer text input"));
        Assert.Equal("z-task", result.hits[0].pageId);
        Assert.Equal("All query terms in heading", result.hits[0].reason);
        Assert.Equal("Excerpt", result.hits[0].completeness);
        Assert.Contains(result.hits, hit => hit.pageId == "probe");
        Assert.Contains(result.hits, hit => hit.pageId == "a-index");
        Assert.Equal(before.OrderBy(static pair => pair.Key), Files().OrderBy(static pair => pair.Key));
    }

    [Fact]
    public async Task NaturalTaskTiesPreferGuidesWhileIdentifiersKeepReferencePriority()
    {
        const string C_PREFAB_SYMBOL = "M:Inno.Probe.InstantiatePrefab";
        Write(
            "content/en/prefab-api.md",
            Page(
                "a-prefab-api",
                "Prefab API",
                "## InstantiatePrefab\n\n<!-- api: " + C_PREFAB_SYMBOL + " " + new string('a', 64)
                + " -->\n\n```csharp\npublic void InstantiatePrefab();\n```\n\nCreate an owned instance.\n",
                "kind: api\nowners: [Inno.Probe]\napiSurface: host-runtime\n"
            )
        );
        Write(
            "content/en/prefab-task.md",
            Page(
                "z-prefab-task",
                "Instantiate a prefab in its owning scene",
                "Supply the current scene and asset.\n",
                "kind: how-to\nowners: [Inno.Probe]\n"
            )
        );
        Write(
            "content/en/body-only.md",
            Page("a-body-only", "Other task", "Instantiate a prefab.\n", "kind: tutorial\nowners: [Inno.Probe]\n")
        );
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        Dictionary<string, string> before = Files();
        DocumentationResult task = await snapshot.QueryAsync(
            new DocumentationRequest("search", "instantiate prefab", "Inno.Probe", limit: 1)
        );
        Assert.Equal("z-prefab-task", Assert.Single(task.hits).pageId);
        Assert.Equal("Excerpt", task.hits[0].completeness);
        DocumentationResult next = await snapshot.QueryAsync(Assert.IsType<DocumentationRequest>(task.nextRequest));
        Assert.Equal("a-prefab-api", Assert.Single(next.hits).pageId);
        DocumentationResult identifier = await snapshot.QueryAsync(new DocumentationRequest("search", "InstantiatePrefab"));
        Assert.Equal("a-prefab-api", identifier.hits[0].pageId);
        DocumentationResult exact = await snapshot.QueryAsync(new DocumentationRequest("search", C_PREFAB_SYMBOL));
        Assert.Equal(C_PREFAB_SYMBOL, Assert.Single(exact.hits).symbol);
        Assert.Equal(before.OrderBy(static pair => pair.Key), Files().OrderBy(static pair => pair.Key));
    }

    [Fact]
    public async Task StructuredPaginationAndExactSymbolSearchNeedNoConsoleParsing()
    {
        Write("content/en/second.md", Page("second", "Ownership details", "Borrow the lease; observe the ownership contract.\n"));
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationResult first = await snapshot.QueryAsync(new DocumentationRequest("search", "ownership", limit: 1));
        Assert.NotNull(first.nextRequest);
        DocumentationResult second = await snapshot.QueryAsync(first.nextRequest!);
        Assert.NotEqual(first.hits.Single().resource, second.hits.Single().resource);
        DocumentationResult exact = await snapshot.QueryAsync(new DocumentationRequest("search", C_SYMBOL));
        Assert.Equal(C_SYMBOL, exact.hits.Single().symbol);
        Assert.Equal("Excerpt", exact.hits.Single().completeness);
        Assert.Equal("Exact qualified symbol", exact.hits.Single().reason);
    }

    [Fact]
    public async Task CommandReadsRequireExplicitPreparationAndNeverRefreshAuthoringInputs()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationCommand.ExecuteAsync(["index", "--engine-root", m_root]));
        await Assert.ThrowsAsync<ArgumentException>(
            () => DocumentationCommand.ExecuteAsync(["read", "--engine-root", m_root, "--docs-root", source, "--page", "probe"])
        );
        Assert.Equal(
            0,
            await DocumentationCommand.ExecuteAsync(
                ["index", "--engine-root", m_root, "--docs-root", source, "--cache-root", cache, "--format", "json"]
            )
        );
        Dictionary<string, string> prepared = Files();
        Write("content/en/probe.md", "Invalid mutable authoring content cannot silently replace the selected snapshot.");
        Assert.Equal(
            0,
            await DocumentationCommand.ExecuteAsync(
                ["read", "--engine-root", m_root, "--cache-root", cache, "--page", "probe", "--format", "json"]
            )
        );
        Assert.Equal(
            2,
            await DocumentationCommand.ExecuteAsync(
                ["read", "--engine-root", m_root, "--cache-root", cache, "--page", "missing", "--format", "json"]
            )
        );
        Assert.Equal(prepared.OrderBy(static pair => pair.Key), Files().OrderBy(static pair => pair.Key));
    }

    [Fact]
    public async Task LargePageReadsPreservePreambleAndNestedSectionsThroughExplicitContinuation()
    {
        string body = "---\nuid: long-contract\nkind: explanation\n---\nPreamble ownership contract.\n\n# First\nOwner contract.\n\n### Deep\n"
                      + new string('x', 18_000) + "\n\n## Second\nRetirement contract.\n";
        Write("content/en/long.md", body);
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        var text = new StringBuilder();
        var request = new DocumentationRequest("read", "long-contract");
        do
        {
            DocumentationResult result = await snapshot.QueryAsync(request);
            text.Append(Assert.Single(result.hits).text);
            request = result.nextRequest;
        }
        while (request is not null);
        Assert.Equal(body, text.ToString());
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        Assert.Equal(new[] { "first", "deep", "second" }, catalog.pages.Single(page => page.id == "long-contract").sections.Keys);
        Assert.Contains(
            "Retirement contract.",
            (await snapshot.QueryAsync(new DocumentationRequest("read", "long-contract", section: "second"))).hits.Single().text
        );
    }
}
