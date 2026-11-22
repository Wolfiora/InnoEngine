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

namespace Inno.Tooling.Documentation.Integration.Tests;

[Collection(nameof(DocumentationApiFixture))]
public sealed class DocumentationApiSynchronizationTests : DocumentationApiFixture
{
    [Fact]
    public async Task SynchronizationIsDeterministicAndNeverTouchesManualGuides()
    {
        Write("content/en/manual/task.md", "A hand-written guide without source metadata.");
        await SyncAsync();
        string[] paths = Directory.GetFiles(Path.Combine(m_root, "content/en/api"), "*.md", SearchOption.AllDirectories)
            .Where(path => File.ReadAllText(path).Contains("symbol:", StringComparison.Ordinal))
            .ToArray();
        Assert.Equal(2, paths.Length);
        Assert.All(paths, path => Assert.Contains("source: xml", File.ReadAllText(path)));
        DateTime[] times = paths.Select(File.GetLastWriteTimeUtc).ToArray();
        await SyncAsync();
        Assert.Equal(times, paths.Select(File.GetLastWriteTimeUtc));
        Assert.Equal("A hand-written guide without source metadata.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/task.md")));
    }

    [Fact]
    public async Task ManualMemberSurvivesXmlChangesWhileItsSiblingUpdates()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string manual = File.ReadAllText(member).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
                        + "\nUse this count after the owner has published the snapshot.\n";
        File.WriteAllText(member, manual);
        Bind(description: "Updated source documentation.");
        await SyncAsync();
        Assert.Equal(Body(manual), Body(File.ReadAllText(member)));
        Assert.Contains("outdatedFor:", File.ReadAllText(member));
        Assert.True(string.IsNullOrWhiteSpace(Body(File.ReadAllText(Fragment(C_TYPE)))));
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo[] pages = (await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api").ToArray();
        Assert.All(pages, page => Assert.Contains("Updated source documentation.", page.markdown));
        DocumentationPageInfo memberPage = Assert.Single(pages.Where(page => page.symbols.ContainsKey(C_MEMBER)));
        Assert.True(
            memberPage.markdown.IndexOf("> [!OUTDATED]", StringComparison.Ordinal)
            < memberPage.markdown.IndexOf("Updated source documentation.", StringComparison.Ordinal)
        );
        Assert.Contains("Use this count after the owner has published the snapshot.", memberPage.markdown);
        Assert.Contains("Updated source documentation.", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/index.md")));
        Assert.Contains("| Updated source documentation. |", Assert.Single(pages.Where(page => page.symbols.ContainsKey(C_TYPE))).markdown);
    }

    [Fact]
    public async Task EditingADescriptionClearsAutomaticStalenessUntilTheNextSourceChange()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        File.WriteAllText(member, File.ReadAllText(member).Replace("source: xml", "source: Ada", StringComparison.Ordinal));
        Bind(description: "A changed\ncontract.");
        await SyncAsync();
        string path = Path.Combine(m_root, "artifacts/query");
        async Task AssertStatusAsync(string status)
        {
            DocumentationResult index = await DocumentationStore.IndexAsync(m_root, m_root, path);
            using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(path, index.snapshot);
            Assert.Contains(
                "> [!" + status + "]",
                (await snapshot.QueryAsync(new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe"))).hits.Single().text
            );
        }
        await AssertStatusAsync("OUTDATED");
        File.WriteAllText(
            member,
            File.ReadAllText(member).Replace("description: null", "description:\n- An updated explanation.", StringComparison.Ordinal)
        );
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        await AssertStatusAsync("AUTHORED");
        Bind(description: "A changed\r\ncontract.");
        await SyncAsync();
        await AssertStatusAsync("AUTHORED");
        Bind(description: "A newer contract.");
        await SyncAsync();
        await AssertStatusAsync("OUTDATED");
    }

    [Theory]
    [InlineData("maybe")]
    [InlineData("[true]")]
    public async Task InvalidOutdatedMetadataCannotBeOverwritten(string value)
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string invalid = File.ReadAllText(member).Replace("outdated: false", "outdated: " + value, StringComparison.Ordinal);
        File.WriteAllText(member, invalid);
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal(invalid, File.ReadAllText(member));
    }

    [Fact]
    public async Task LegacyStaleApiRetainsOutdatedStatusDuringMigration()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string legacy = File.ReadAllText(member)
            .Replace("sourceFingerprint:", "reviewed:", StringComparison.Ordinal)
            .Replace("source: xml", "source: Ada", StringComparison.Ordinal)
            .Replace("outdated: false", "outdated: true", StringComparison.Ordinal);
        File.WriteAllText(member, legacy);
        await SyncAsync();
        Assert.Contains("outdatedFor:", File.ReadAllText(member));
        Assert.DoesNotContain("reviewed:", File.ReadAllText(member));
    }

    [Fact]
    public async Task MissingSourceRemainsGeneratorOwned()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        File.WriteAllText(member, File.ReadAllText(member).Replace("source: xml\n", string.Empty, StringComparison.Ordinal));
        Bind(description: "A fresh description.");
        await SyncAsync();
        Assert.Contains("source: xml", File.ReadAllText(member));
        Assert.True(string.IsNullOrWhiteSpace(Body(File.ReadAllText(member))));
        Assert.Contains("A fresh description.", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/index.md")));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RemovalDeletesXmlAndPreservesManualProse(bool manual)
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        if (manual)
        {
            File.WriteAllText(member, File.ReadAllText(member).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal));
        }
        string before = File.ReadAllText(member);
        Bind(member: false);
        await SyncAsync();
        Assert.Equal(manual, File.Exists(member));
        if (manual) Assert.Equal(before, File.ReadAllText(member));
    }

    [Fact]
    public async Task BlankAuthorCannotOverwriteAnyContent()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string invalid = File.ReadAllText(member).Replace("source: xml", "source: \"\"", StringComparison.Ordinal);
        File.WriteAllText(member, invalid);
        Bind(description: "New XML.");
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal(invalid, File.ReadAllText(member));
    }

    [Fact]
    public async Task CorruptCaptureAndPrecancellationCannotChangeContent()
    {
        await SyncAsync();
        string type = File.ReadAllText(Fragment(C_TYPE));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => SyncAsync(new CancellationToken(true)));
        File.AppendAllText(Path.Combine(m_root, "reference/api/probe.json"), " ");
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal(type, File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task RegisteredPathCannotEscapeApiOwnership()
    {
        await SyncAsync();
        string path = Path.Combine(m_root, "reference/api/content.json");
        JsonNode registry = JsonNode.Parse(File.ReadAllText(path))!;
        registry["entries"]![0]!["path"] = "content/en/manual/task.md";
        Write("content/en/manual/task.md", "Preserve this independent guide.");
        File.WriteAllText(path, registry.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal("Preserve this independent guide.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/task.md")));
    }

    [Fact]
    public async Task UnregisteredFilesAreNotSilentlyClaimed()
    {
        Write("content/en/api/engine/probe/value/index.md", "Unregistered authored prose.");
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal("Unregistered authored prose.", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/value/index.md")));
    }

    [Fact]
    public async Task AnApiPrefixCannotAuthorizeTraversalIntoManualContent()
    {
        await SyncAsync();
        string path = Path.Combine(m_root, "reference/api/content.json");
        JsonNode registry = JsonNode.Parse(File.ReadAllText(path))!;
        registry["entries"]![0]!["path"] = "content/en/api/engine/../../manual/task.md";
        Write("content/en/manual/task.md", "Preserve the independent guide.");
        File.WriteAllText(path, registry.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal("Preserve the independent guide.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/task.md")));
    }

    [Fact]
    public async Task EditorialWarningsDoNotFailButStaleGeneratedContentDoes()
    {
        await SyncAsync();
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        Bind(description: "Changed XML must be synchronized.");
        Assert.Equal(1, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        await SyncAsync();
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
    }

    [Fact]
    public async Task MissingProjectCaptureCannotBePresentedAsACompleteApiSet()
    {
        await SyncAsync();
        string path = Path.Combine(m_root, "reference/projects/catalog.json");
        JsonArray inventory = JsonNode.Parse(File.ReadAllText(path))!.AsArray();
        inventory.Add(new JsonObject { ["name"] = "Inno.Missing", ["path"] = "src/Inno.Missing.csproj", ["isTest"] = false });
        string text = inventory.ToJsonString();
        File.WriteAllText(path, text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Contains("Inno.Missing", failure.Message);
    }

    [Fact]
    public async Task NamedAuthorAndDescriptionsSurviveSyncAndReachTablesAndQueries()
    {
        await SyncAsync();
        string typePath = Fragment(C_TYPE);
        string memberPath = Fragment(C_MEMBER);
        File.WriteAllText(
            typePath,
            File.ReadAllText(typePath)
                .Replace("description: null", "description: [Custom type introduction, Second type paragraph]", StringComparison.Ordinal)
        );
        string original = File.ReadAllText(memberPath)
            .Replace("source: xml", "source: Ada Lovelace", StringComparison.Ordinal)
            .Replace("description: null", "description: [Custom member introduction, Second member paragraph]", StringComparison.Ordinal);
        string body = "\n\n## Remarks\n\nKeep this author explanation byte for byte.\n";
        File.WriteAllText(memberPath, original[..(original.Length - Body(original).Length)] + body);
        Bind(description: "Changed XML.", countType: "long");
        await SyncAsync();
        string saved = File.ReadAllText(memberPath);
        Assert.Contains("source: Ada Lovelace", saved);
        Assert.Contains("outdatedFor:", saved);
        Assert.Equal(body, Body(saved));
        Assert.True(
            saved.IndexOf("contracts:", StringComparison.Ordinal) < saved.IndexOf("# Authoring metadata", StringComparison.Ordinal)
        );
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo type = Assert.Single(catalog.pages.Where(page => page.symbols.ContainsKey(C_TYPE)));
        DocumentationPageInfo member = Assert.Single(catalog.pages.Where(page => page.symbols.ContainsKey(C_MEMBER)));
        Assert.Contains("> - Custom type introduction", type.markdown);
        Assert.Contains("Custom member introduction<br><br>Second member paragraph", type.markdown);
        Assert.Contains("> - Second member paragraph", member.markdown);
        Assert.Contains("public long", member.markdown);
        Assert.Contains(
            catalog.pages,
            page => page.markdown.Contains("Custom type introduction<br><br>Second type paragraph", StringComparison.Ordinal)
        );
        DocumentationHit hit = Assert.Single((await snapshot.QueryAsync(new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe"))).hits);
        Assert.Contains("Custom member introduction", hit.text);
        Assert.DoesNotContain("Changed XML.", member.markdown);
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("wrong scalar")]
    [InlineData("[null]")]
    public async Task InvalidDescriptionDoesNotOverwriteContent(string value)
    {
        await SyncAsync();
        string path = Fragment(C_TYPE);
        string invalid = File.ReadAllText(path).Replace("description: null", "description: " + value, StringComparison.Ordinal);
        File.WriteAllText(path, invalid);
        await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
        Assert.Equal(invalid, File.ReadAllText(path));
    }

    [Fact]
    public async Task ManualBodySectionsRemainEditableWhileSummaryFollowsSource()
    {
        SetTypeDocumentation("<member><summary>Original summary.</summary><remarks>Original remarks.</remarks></member>");
        await SyncAsync();
        string path = Fragment(C_TYPE);
        string text = File.ReadAllText(path).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal);
        string body = "\n\n## Remarks\n\nMy independently maintained ownership explanation.\n";
        File.WriteAllText(path, text[..(text.Length - Body(text).Length)] + body);
        SetTypeDocumentation("<member><summary>Updated source summary.</summary><remarks>Changed source remarks.</remarks></member>");
        await SyncAsync();
        Assert.Equal(body, Body(File.ReadAllText(path)));
        Assert.DoesNotContain("Updated source summary.", File.ReadAllText(path));
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single(
            (await snapshot.ExportCatalogAsync()).pages.Where(page => page.symbols.ContainsKey(C_TYPE))
        );
        Assert.Contains("# Value\n\n> - Updated source summary.", page.markdown);
        Assert.True(
            page.markdown.IndexOf("> [!OUTDATED]", StringComparison.Ordinal) < page.markdown.IndexOf("# Value", StringComparison.Ordinal)
        );
        Assert.Contains("## Remarks\n\nMy independently maintained ownership explanation.", page.markdown);
        Assert.DoesNotContain("#### Remarks", page.markdown);
        Assert.DoesNotContain("Changed source remarks.", page.markdown);
        Assert.DoesNotContain("Original summary.", page.markdown);
    }

    [Theory]
    [InlineData("backends/Probe/Adapter.csproj", false)]
    [InlineData("platforms/WindowsX64/Probe.csproj", false)]
    [InlineData("tools/Probe.csproj", false)]
    [InlineData("entry/Probe.csproj", false)]
    [InlineData("extern/Probe.csproj", false)]
    [InlineData("tests/fixtures/Probe.csproj", false)]
    [InlineData("tools/Probe.Tests.csproj", true)]
    [InlineData("src/Native.vcxproj", false)]
    public async Task ExcludedOwnersCannotEnterTheRegisteredApiSet(
        string projectPath,
        bool isTest
    ) {
        JsonNode inventory = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/projects/catalog.json")))!;
        JsonNode excluded = inventory[0]!.DeepClone();
        excluded["name"] = "Excluded.Probe";
        excluded["path"] = projectPath;
        excluded["isTest"] = isTest;
        inventory.AsArray().Add(excluded);
        string inventoryText = inventory.ToJsonString();
        Write("reference/projects/catalog.json", inventoryText);
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        JsonNode captured = facts["projects"]![0]!.DeepClone();
        captured["name"] = "Excluded.Probe";
        captured["project"] = projectPath;
        facts["projects"]!.AsArray().Add(captured);
        string factsText = facts.ToJsonString();
        Write("reference/api/probe.json", factsText);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Hash(inventoryText);
        binding["apiManifests"]![0]!["sha256"] = Hash(factsText);
        Write("reference/engine.lock.json", binding.ToJsonString());
        await SyncAsync();
        JsonNode registry = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/content.json")))!;
        Assert.Equal(2, registry["entries"]!.AsArray().Count);
        Assert.DoesNotContain("Excluded.Probe", registry.ToJsonString());
    }
}
