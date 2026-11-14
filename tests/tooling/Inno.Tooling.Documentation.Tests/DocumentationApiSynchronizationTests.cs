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

public sealed class DocumentationApiSynchronizationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoApiSynchronization", Guid.NewGuid().ToString("N"));
    private const string C_TYPE = "T:Inno.Probe.Value";
    private const string C_MEMBER = "P:Inno.Probe.Value.count";

    public DocumentationApiSynchronizationTests() => Bind();

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Theory]
    [InlineData("T:Inno.Probe.Value`1", "public class Inno.Probe.Value<TValue>", "Value<TValue>")]
    [InlineData("T:Inno.Probe.Value`2", "public class Inno.Probe.Value<TTarget, TArgument>", "Value<TTarget, TArgument>")]
    [InlineData("T:Inno.Probe.Outer`1.Value`1", "public class Inno.Probe.Outer<TOwner>.Value<TValue>", "Value<TValue>")]
    [InlineData("T:Inno.Probe.Outer`1.Value", "public class Inno.Probe.Outer<TOwner>.Value", "Value")]
    [InlineData("T:Inno.Probe.class`1", "public class Inno.Probe.@class<TValue>", "@class<TValue>")]
    [InlineData("T:Inno.Probe.Factory`2", "public delegate TResult Inno.Probe.Factory<TValue, TResult>(TValue value)", "Factory<TValue, TResult>")]
    public async Task GenericTitlesUseDeclaredNamesAndRenderAsText(
        string id,
        string signature,
        string expected
    ) {
        SetTypes((id, signature));
        await Sync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single((await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api"));
        Assert.Equal(expected, page.title);
        Assert.Contains("<h1>" + System.Net.WebUtility.HtmlEncode(expected) + "</h1>", Markdig.Markdown.ToHtml(page.markdown));
        Assert.Contains(expected, (await snapshot.QueryAsync(new("symbol", id, "Inno.Probe"))).hits.Single().text);
    }

    [Fact]
    public async Task GenericParameterRenamesPreserveAddressesAndManualProseWithReadableDisambiguation()
    {
        const string C_ONE = "T:Inno.Probe.Value`1";
        const string C_TWO = "T:Inno.Probe.Value`2";
        const string C_OTHER = "T:Other.Value`1";
        SetTypes((C_TYPE, "public class Inno.Probe.Value"),
            (C_ONE, "public class Inno.Probe.Value<TValue>"),
            (C_TWO, "public class Inno.Probe.Value<TTarget, TArgument>"),
            (C_OTHER, "public class Other.Value<TValue>"));
        await Sync();
        string[] paths = new[] { C_TYPE, C_ONE, C_TWO, C_OTHER }.Select(Fragment).ToArray();
        Assert.Equal(4, paths.Distinct().Count());
        string manual = File.ReadAllText(paths[2]).Replace("source: xml", "source: manual", StringComparison.Ordinal)
            + "\nKeep the target alive while the operation runs.\n";
        File.WriteAllText(paths[2], manual);
        SetTypes((C_TYPE, "public class Inno.Probe.Value"),
            (C_ONE, "public class Inno.Probe.Value<TValue>"),
            (C_TWO, "public class Inno.Probe.Value<TOwner, TArgument>"),
            (C_OTHER, "public class Other.Value<TValue>"));
        await Sync();
        Assert.Equal(paths, new[] { C_TYPE, C_ONE, C_TWO, C_OTHER }.Select(Fragment));
        Assert.Equal(manual, File.ReadAllText(paths[2]));
        string navigation = File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/index.md"));
        Assert.Contains("[Inno.Probe.Value&lt;TValue&gt;]", navigation);
        Assert.Contains("[Other.Value&lt;TValue&gt;]", navigation);
        Assert.Contains("[Value&lt;TOwner, TArgument&gt;]", navigation);
        Assert.DoesNotContain("Value1", navigation);
        Assert.DoesNotContain("Value2", navigation);
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = (await snapshot.ExportCatalogAsync()).pages.Single(page => page.symbols.ContainsKey(C_TWO));
        Assert.Equal("Value<TOwner, TArgument>", page.title);
        Assert.Contains("Keep the target alive", page.markdown);
        Assert.Equal(manual, File.ReadAllText(paths[2]));
    }

    [Fact]
    public async Task SynchronizationIsDeterministicAndNeverTouchesManualGuides()
    {
        Write("content/en/manual/task.md", "A hand-written guide without source metadata.");
        await Sync();
        string[] paths = Directory.GetFiles(Path.Combine(m_root, "content/en/api"), "*.md", SearchOption.AllDirectories)
            .Where(path => File.ReadAllText(path).Contains("symbol:", StringComparison.Ordinal)).ToArray();
        Assert.Equal(2, paths.Length);
        Assert.All(paths, path => Assert.Contains("source: xml", File.ReadAllText(path)));
        DateTime[] times = paths.Select(File.GetLastWriteTimeUtc).ToArray();
        await Sync();
        Assert.Equal(times, paths.Select(File.GetLastWriteTimeUtc));
        Assert.Equal("A hand-written guide without source metadata.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/task.md")));
    }

    [Fact]
    public async Task ManualMemberSurvivesXmlChangesWhileItsSiblingUpdates()
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        string manual = File.ReadAllText(member).Replace("source: xml", "source: manual", StringComparison.Ordinal)
            + "\nUse this count after the owner has published the snapshot.\n";
        File.WriteAllText(member, manual);
        Bind(description: "Updated source documentation.");
        await Sync();
        Assert.Equal(manual, File.ReadAllText(member));
        Assert.Contains("Updated source documentation.", File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task MissingSourceRemainsGeneratorOwned()
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        File.WriteAllText(member, File.ReadAllText(member).Replace("source: xml\n", "", StringComparison.Ordinal));
        Bind(description: "A fresh description.");
        await Sync();
        Assert.Contains("source: xml", File.ReadAllText(member));
        Assert.Contains("A fresh description.", File.ReadAllText(member));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RemovalDeletesXmlAndPreservesManualProse(bool manual)
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        if (manual) File.WriteAllText(member, File.ReadAllText(member).Replace("source: xml", "source: manual", StringComparison.Ordinal));
        string before = File.ReadAllText(member);
        Bind(member: false);
        await Sync();
        Assert.Equal(manual, File.Exists(member));
        if (manual) Assert.Equal(before, File.ReadAllText(member));
    }

    [Fact]
    public async Task UnknownSourceCannotOverwriteAnyContent()
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        string invalid = File.ReadAllText(member).Replace("source: xml", "source: manul", StringComparison.Ordinal);
        File.WriteAllText(member, invalid);
        Bind(description: "New XML.");
        await Assert.ThrowsAsync<InvalidDataException>(Sync);
        Assert.Equal(invalid, File.ReadAllText(member));
    }

    [Fact]
    public async Task CorruptCaptureAndPrecancellationCannotChangeContent()
    {
        await Sync();
        string type = File.ReadAllText(Fragment(C_TYPE));
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => Sync(new CancellationToken(true)));
        File.AppendAllText(Path.Combine(m_root, "reference/api/probe.json"), " ");
        await Assert.ThrowsAsync<InvalidDataException>(Sync);
        Assert.Equal(type, File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task RegisteredPathCannotEscapeApiOwnership()
    {
        await Sync();
        string path = Path.Combine(m_root, "reference/api/content.json");
        JsonNode registry = JsonNode.Parse(File.ReadAllText(path))!;
        registry["entries"]![0]!["path"] = "content/en/manual/task.md";
        Write("content/en/manual/task.md", "Preserve this independent guide.");
        File.WriteAllText(path, registry.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(Sync);
        Assert.Equal("Preserve this independent guide.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/task.md")));
    }

    [Fact]
    public async Task UnregisteredFilesAreNotSilentlyClaimed()
    {
        Write("content/en/api/engine/probe/value/index.md", "Unregistered authored prose.");
        await Assert.ThrowsAsync<InvalidDataException>(Sync);
        Assert.Equal("Unregistered authored prose.", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/value/index.md")));
    }

    [Fact]
    public async Task AnApiPrefixCannotAuthorizeTraversalIntoManualContent()
    {
        await Sync();
        string path = Path.Combine(m_root, "reference/api/content.json");
        JsonNode registry = JsonNode.Parse(File.ReadAllText(path))!;
        registry["entries"]![0]!["path"] = "content/en/api/engine/../../manual/task.md";
        Write("content/en/manual/task.md", "Preserve the independent guide.");
        File.WriteAllText(path, registry.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(Sync);
        Assert.Equal("Preserve the independent guide.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/task.md")));
    }

    [Fact]
    public async Task EditorialWarningsDoNotFailButStaleGeneratedContentDoes()
    {
        await Sync();
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        Bind(description: "Changed XML must be synchronized.");
        Assert.Equal(1, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        await Sync();
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
    }

    [Fact]
    public async Task MissingProjectCaptureCannotBePresentedAsACompleteApiSet()
    {
        await Sync();
        string path = Path.Combine(m_root, "reference/projects/catalog.json");
        JsonArray inventory = JsonNode.Parse(File.ReadAllText(path))!.AsArray();
        inventory.Add(new JsonObject { ["name"] = "Inno.Missing", ["path"] = "src/Inno.Missing.csproj", ["isTest"] = false });
        string text = inventory.ToJsonString();
        File.WriteAllText(path, text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(Sync);
        Assert.Contains("Inno.Missing", failure.Message);
    }

    [Theory]
    [InlineData("Runtime")]
    [InlineData("Editor")]
    [InlineData("Authoring")]
    public async Task DeclaredScopeIsSeparateFromCompilationSurfaceAndHasOneCanonicalBody(string scope)
    {
        JsonNode host = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        foreach (string surface in new[] { "runtime-script", "editor-script" })
        {
            JsonNode projected = host.DeepClone();
            projected["model"] = surface;
            foreach (JsonNode? symbol in projected["projects"]![0]!["symbols"]!.AsArray()) symbol!["scriptingScope"] = scope;
            string text = projected.ToJsonString();
            string file = "reference/api/" + surface + ".json";
            Write(file, text);
            JsonNode capture = binding["apiManifests"]![0]!.DeepClone();
            capture["path"] = file;
            capture["sha256"] = Hash(text);
            capture["model"] = surface;
            binding["apiManifests"]!.AsArray().Add(capture);
        }
        JsonNode inventory = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/projects/catalog.json")))!;
        inventory[0]!["surfaces"] = new JsonArray("source-clr", "runtime-script", "editor-script");
        Write("reference/projects/catalog.json", inventory.ToJsonString());
        binding["projectManifestIdentity"] = Hash(inventory.ToJsonString());
        Write("reference/engine.lock.json", binding.ToJsonString());
        await Sync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo script = Assert.Single(catalog.pages.Where(page => page.kind == "api" && page.scope == scope && page.symbols.ContainsKey(C_MEMBER)));
        Assert.Contains("scripting/" + scope.ToLowerInvariant(), script.path);
        Assert.Contains("## V", catalog.pages.Single(page => page.path == "content/en/api/scripting/" + scope.ToLowerInvariant() + "/index.md").markdown);
        foreach (string surface in new[] { "runtime-script", "editor-script" })
        {
            DocumentationHit hit = Assert.Single((await snapshot.QueryAsync(new("symbol", C_MEMBER, "Inno.Probe", surface, scope: scope))).hits);
            Assert.Equal(script.id, hit.pageId);
            Assert.Equal(scope, hit.scope);
            Assert.Equal(new[] { "editor-script", "runtime-script" }, hit.surfaces);
        }
        Assert.Empty((await snapshot.QueryAsync(new("symbol", C_MEMBER, "Inno.Probe", scope: scope == "Editor" ? "Runtime" : "Editor"))).hits);
    }

    [Fact]
    public async Task WebsiteCatalogAndSymbolReadsShareComposedDeclarationsAndManualProse()
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        string manual = File.ReadAllText(member).Replace("source: xml", "source: manual", StringComparison.Ordinal)
            + "\nRead the count only after publication.\n";
        File.WriteAllText(member, manual);
        Bind(description: "Updated XML.", countType: "long");
        await Sync();
        Write("content/en/manual/ownership.md", "---\nuid: manual.ownership\ntitle: Ownership and retirement\nkind: explanation\n---\n\n# Ownership and retirement\n\n## Release\n\nKeep the owner alive until all borrows end.\n");
        string typePath = Fragment(C_TYPE);
        File.WriteAllText(typePath, File.ReadAllText(typePath).Replace("kind: api", "kind: api\ncontracts: [manual.ownership#release]\nexampleMode: host\nexampleProjects: [Inno.Probe]", StringComparison.Ordinal));
        string cache = Path.Combine(m_root, "artifacts", "query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo page = Assert.Single(catalog.pages.Where(page => page.kind == "api" && page.symbols.ContainsKey(C_MEMBER)));
        Assert.Contains("public long Inno.Probe.Value.count", page.markdown);
        Assert.NotNull(page.examples);
        Assert.Equal("host", page.examples.mode);
        Assert.Contains("Read the count only after publication.", page.markdown);
        Assert.Contains("## Required contracts", page.markdown);
        Assert.Contains("[Ownership and retirement](../../../../manual/ownership.md#release)", page.markdown);
        Assert.Contains("[Source declaration](https://github.com/Wolfiora/InnoEngine/blob/", page.markdown);
        Assert.DoesNotContain("Edit this explanation", page.markdown);
        Assert.Contains("reference/api/content.json", catalog.sourceFiles.Keys);
        Assert.Contains(Path.GetRelativePath(m_root, member).Replace('\\', '/'), catalog.sourceFiles.Keys);
        DocumentationResult result = await snapshot.QueryAsync(new("symbol", C_MEMBER, "Inno.Probe", "source-clr"));
        DocumentationHit hit = Assert.Single(result.hits);
        Assert.Equal("manual", hit.contentSourceKind);
        Assert.Contains("public long Inno.Probe.Value.count", hit.text);
        Assert.Contains("Read the count only after publication.", hit.text);
        Assert.Equal(manual, File.ReadAllText(member));
        Assert.Contains(hit.text, Assert.Single((await snapshot.QueryAsync(new("read", hit.contentSource))).hits).text);
        Directory.Move(Path.Combine(m_root, "content"), Path.Combine(m_root, "unavailable-content"));
        Assert.Equal(hit.text, Assert.Single((await snapshot.QueryAsync(new("symbol", C_MEMBER, "Inno.Probe"))).hits).text);
    }

    [Fact]
    public async Task AllTargetDeclarationsRemainFactsWhileAuthoredExamplesRemainExecutable()
    {
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        JsonNode variant = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        variant["target"] = "other-target";
        const string C_VARIANT = "public long Inno.Probe.Value.count { get; }";
        variant["projects"]![0]!["symbols"]![1]!["signature"] = C_VARIANT;
        variant["projects"]![0]!["symbols"]![1]!["signatureHash"] = Hash(C_VARIANT);
        string text = variant.ToJsonString();
        Write("reference/api/variant.json", text);
        JsonNode capture = binding["apiManifests"]![0]!.DeepClone();
        capture["path"] = "reference/api/variant.json";
        capture["target"] = "other-target";
        capture["sha256"] = Hash(text);
        binding["apiManifests"]!.AsArray().Add(capture);
        Write("reference/engine.lock.json", binding.ToJsonString());
        await Sync();
        string member = Fragment(C_MEMBER);
        File.WriteAllText(member, File.ReadAllText(member).Replace("source: xml", "source: manual", StringComparison.Ordinal)
            + "\n```csharp\nvar count = value.count;\n```\n");
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single((await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api" && page.symbols.ContainsKey(C_MEMBER)));
        Assert.Equal(2, page.codeBlocks.Count(block => block.declarationSymbols.Count != 0));
        Assert.Equal("var count = value.count;", Assert.Single(page.codeBlocks.Where(block => block.declarationSymbols.Count == 0)).text);
        Assert.Contains(C_VARIANT, page.symbols[C_MEMBER].code);
    }

    [Theory]
    [InlineData("xml", true)]
    [InlineData("manual", false)]
    [InlineData("", false)]
    public async Task SourceNoticeRequiresExplicitXmlAndSourceLinkEndsThePage(string source, bool notice)
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        string original = File.ReadAllText(member);
        File.WriteAllText(member, original.Replace("source: xml", source.Length == 0 ? "" : "source: " + source, StringComparison.Ordinal));
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single((await snapshot.ExportCatalogAsync()).pages.Where(page => page.symbols.ContainsKey(C_MEMBER)));
        Assert.Equal(notice, page.markdown.Contains("> [!TIP]", StringComparison.Ordinal));
        if (notice) Assert.True(page.markdown.IndexOf("> [!TIP]", StringComparison.Ordinal) < page.markdown.IndexOf("## Required contracts", StringComparison.Ordinal));
        Assert.DoesNotContain("Available in:", page.markdown);
        Assert.DoesNotContain("XML-synced", page.markdown);
        Assert.DoesNotContain("Edit this explanation", page.markdown);
        Assert.Contains("<!-- api-source -->\n[Source declaration](https://github.com/", page.markdown);
        Assert.EndsWith(")", page.markdown.TrimEnd());
    }

    private Task<int> Sync() => Sync(CancellationToken.None);

    [Fact]
    public async Task ElementsAreIndependentPagesWithNestedContainmentAndOverloadLinks()
    {
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        JsonArray symbols = facts["projects"]![0]!["symbols"]!.AsArray();
        const string C_NESTED = "T:Inno.Probe.Value.Child`1";
        const string C_DEEP = "T:Inno.Probe.Value.Child`1.State";
        Add(C_NESTED, "NamedType", "public struct Inno.Probe.Value.Child<TItem>", C_TYPE);
        Add(C_DEEP, "NamedType", "public enum Inno.Probe.Value.Child<TItem>.State", C_NESTED);
        Add("F:Inno.Probe.Value.Child`1.State.Ready", "Field", "public const Inno.Probe.Value.Child<TItem>.State Ready = 0", C_DEEP);
        Add("M:Inno.Probe.Value.Read(System.Int32)", "Method", "public void Inno.Probe.Value.Read(int value)", C_TYPE);
        Add("M:Inno.Probe.Value.Read(System.String)", "Method", "public void Inno.Probe.Value.Read(string value)", C_TYPE);
        SaveFacts(facts);
        await Sync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        var pages = (await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api").ToArray();
        Assert.Equal(7, pages.Length);
        Assert.All(pages, page => Assert.Single(page.symbols));
        DocumentationPageInfo outer = pages.Single(page => page.symbols.ContainsKey(C_TYPE));
        Assert.Contains("| Kind | Element | Description |", outer.markdown);
        Assert.Contains("| Nested types |", outer.markdown);
        Assert.Contains("Value.Child&lt;TItem&gt;", outer.markdown);
        Assert.Contains("Value.Read(System.Int32)", outer.markdown);
        Assert.Contains("Value.Read(System.String)", outer.markdown);
        DocumentationPageInfo nested = pages.Single(page => page.symbols.ContainsKey(C_NESTED));
        Assert.Equal("Value.Child<TItem>", nested.title);
        Assert.Contains("Value.Child&lt;TItem&gt;.State", nested.markdown);
        DocumentationPageInfo field = pages.Single(page => page.title.EndsWith(".Ready", StringComparison.Ordinal));
        Assert.Equal("Value.Child<TItem>.State.Ready", field.title);
        Assert.Contains(pages.Single(page => page.symbols.ContainsKey(C_DEEP)).id, field.contracts);
        Assert.DoesNotContain("_members", field.path);

        void Add(string id, string kind, string signature, string parent)
        {
            JsonNode symbol = symbols[0]!.DeepClone();
            symbol["id"] = id; symbol["kind"] = kind; symbol["signature"] = signature;
            symbol["signatureHash"] = Hash(signature); symbol["containingType"] = parent;
            symbols.Add(symbol);
        }
    }

    [Fact]
    public async Task MovingLegacyMembersRepairsIncomingAnchorsAndManualRelativeLinks()
    {
        await Sync();
        string current = Fragment(C_MEMBER);
        string legacy = Path.Combine(Path.GetDirectoryName(current)!, "_members/count.md");
        Directory.CreateDirectory(Path.GetDirectoryName(legacy)!);
        string text = File.ReadAllText(current).Replace("source: xml", "source: manual", StringComparison.Ordinal)
            + "\nKeep the original explanation. [Owner](../index.md).\n";
        File.WriteAllText(legacy, text);
        File.Delete(current);
        string registryPath = Path.Combine(m_root, "reference/api/content.json");
        JsonNode registry = JsonNode.Parse(File.ReadAllText(registryPath))!;
        registry["entries"]!.AsArray().Single(entry => entry!["symbol"]!.GetValue<string>().EndsWith(C_MEMBER, StringComparison.Ordinal))!["path"]
            = Path.GetRelativePath(m_root, legacy).Replace('\\', '/');
        File.WriteAllText(registryPath, registry.ToJsonString());
        Write("content/en/manual/count.md", "---\nuid: manual.count\ntitle: Count\nkind: how-to\n---\n\n# Count\n\n"
            + "[Old member](../api/engine/probe/value/_members/count.md) and [exact member](../api/engine/probe/value/index.md#api-" + Hash(C_MEMBER)[..16] + ").\n");
        await Sync();
        Assert.False(File.Exists(legacy));
        Assert.Contains("Keep the original explanation. [Owner](index.md).", File.ReadAllText(current));
        Assert.Contains("source: manual", File.ReadAllText(current));
        string guide = File.ReadAllText(Path.Combine(m_root, "content/en/manual/count.md"));
        Assert.DoesNotContain("_members", guide);
        Assert.DoesNotContain("index.md#api-", guide);
        string cache = Path.Combine(m_root, "artifacts/query");
        await DocumentationStore.IndexAsync(m_root, m_root, cache);
        string after = File.ReadAllText(current);
        await Sync();
        Assert.Equal(after, File.ReadAllText(current));
    }

    private void SaveFacts(JsonNode facts)
    {
        string text = facts.ToJsonString();
        Write("reference/api/probe.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    [Fact]
    public async Task XmlRefreshPreservesAuthoredContractAndExampleAssociations()
    {
        await Sync();
        string type = Fragment(C_TYPE);
        string text = File.ReadAllText(type).Replace("kind: api", "contracts: [manual.ownership#release]\naliases: [persistent value]\nexampleMode: host\nexampleProjects: [Inno.Probe]\nkind: api", StringComparison.Ordinal);
        File.WriteAllText(type, text);
        Bind(description: "Fresh XML still needs shared ownership guidance.");
        await Sync();
        string updated = File.ReadAllText(type);
        Assert.Contains("manual.ownership#release", updated);
        Assert.Contains("persistent value", updated);
        Assert.Contains("exampleMode: host", updated);
        Assert.Contains("exampleProjects:", updated);
        Assert.Contains("Fresh XML still needs", updated);
        await Sync();
        Assert.Equal(updated, File.ReadAllText(type));
    }

    [Fact]
    public async Task XmlRenderingPreservesFormulaListsAndExecutableExampleBoundaries()
    {
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        facts["projects"]![0]!["symbols"]![0]!["documentation"] = """
            <member><summary>Owns a value.</summary><remarks>
            A formula: <code>left + (right - left) * amount</code>.
            <list type="bullet"><item><description>Retain the owner.</description></item>
            <item><description>Release the lease.</description></item></list>
            </remarks><example><code>
                var value = 1;
                System.Console.WriteLine(value);
            </code></example></member>
            """;
        string text = facts.ToJsonString();
        Write("reference/api/probe.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
        await Sync();
        string prose = File.ReadAllText(Fragment(C_TYPE));
        Assert.Contains("`left + (right - left) * amount`", prose);
        Assert.Contains("- Retain the owner.", prose);
        Assert.Contains("- Release the lease.", prose);
        Assert.Contains("```csharp\nvar value = 1;\nSystem.Console.WriteLine(value);\n```", prose);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RuntimeAvailabilityInEditorRequiresTheSameActualProjection(bool mismatch)
    {
        JsonNode host = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        foreach (string surface in new[] { "runtime-script", "editor-script" })
        {
            JsonNode projected = host.DeepClone();
            projected["model"] = surface;
            projected["projectionIdentities"] = new JsonObject
            {
                ["Inno.ScriptApi.Runtime"] = new string(mismatch && surface == "editor-script" ? 'f' : 'e', 64),
                ["Inno.ScriptApi.Editor"] = new string('d', 64)
            };
            foreach (JsonNode? symbol in projected["projects"]![0]!["symbols"]!.AsArray()) symbol!["scriptingScope"] = "Runtime";
            if (surface == "editor-script") projected["projects"]![0]!["symbols"] = new JsonArray();
            string file = "reference/api/" + surface + ".json";
            string text = projected.ToJsonString();
            Write(file, text);
            JsonNode capture = binding["apiManifests"]![0]!.DeepClone();
            capture["path"] = file;
            capture["sha256"] = Hash(text);
            capture["model"] = surface;
            binding["apiManifests"]!.AsArray().Add(capture);
        }
        JsonNode inventory = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/projects/catalog.json")))!;
        inventory[0]!["surfaces"] = new JsonArray("source-clr", "runtime-script", "editor-script");
        Write("reference/projects/catalog.json", inventory.ToJsonString());
        binding["projectManifestIdentity"] = Hash(inventory.ToJsonString());
        Write("reference/engine.lock.json", binding.ToJsonString());
        if (mismatch)
        {
            InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(Sync);
            Assert.Contains("different Runtime projection", failure.Message);
            return;
        }
        await Sync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationHit hit = Assert.Single((await snapshot.QueryAsync(new("symbol", C_MEMBER, "Inno.Probe", "editor-script", scope: "Runtime"))).hits);
        Assert.Equal(new[] { "editor-script", "runtime-script" }, hit.surfaces);
    }

    [Fact]
    public async Task PackageImportPreservesManualProseAndSkipsRevisionOnlyChanges()
    {
        await Sync();
        string member = Fragment(C_MEMBER);
        string manual = File.ReadAllText(member).Replace("source: xml", "source: manual", StringComparison.Ordinal)
            + "\nRead this value only after publication.\n";
        File.WriteAllText(member, manual);
        Bind(description: "Updated package XML.");
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]));
        Bind();
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Equal(manual, File.ReadAllText(member));
        Assert.Contains("Updated package XML.", File.ReadAllText(Fragment(C_TYPE)));
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        JsonNode next = JsonNode.Parse(File.ReadAllText(package))!;
        next["revision"] = new string('c', 40);
        JsonNode bindingFile = next["files"]!.AsArray().Single(file => file!["path"]!.GetValue<string>() == "reference/engine.lock.json")!;
        string revised = bindingFile["text"]!.GetValue<string>().Replace(new string('a', 40), new string('c', 40), StringComparison.Ordinal);
        bindingFile["text"] = revised;
        bindingFile["sha256"] = Hash(revised);
        next["toolingRevision"] = new string('c', 40);
        File.WriteAllText(package, next.ToJsonString());
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
        Assert.Equal(manual, File.ReadAllText(member));
    }

    [Theory]
    [InlineData("tampered")]
    [InlineData("extra-file")]
    [InlineData("escape")]
    [InlineData("mixed-revisions")]
    public async Task InvalidPackagesCannotModifyTheCheckout(string corruption)
    {
        await Sync();
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]);
        JsonNode packet = JsonNode.Parse(File.ReadAllText(package))!;
        JsonArray files = packet["files"]!.AsArray();
        if (corruption == "tampered") files[0]!["text"] = "{}";
        if (corruption == "extra-file") files.Add(new JsonObject { ["path"] = "content/en/manual/task.md", ["text"] = "replace", ["sha256"] = Hash("replace") });
        if (corruption == "escape") files[0]!["path"] = "reference/api/../../AGENTS.md";
        if (corruption == "mixed-revisions") packet["revision"] = new string('d', 40);
        File.WriteAllText(package, packet.ToJsonString());
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        string before = File.ReadAllText(Fragment(C_TYPE));
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
        Assert.Equal(before, File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task ImportWaitsForTheSharedWriterBeforeChangingFactsAndCancellationPreservesInputs()
    {
        await Sync();
        Bind(description: "New import contract.");
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]);
        Bind();
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        byte[] facts = File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json"));
        using (FileLease writer = await FileLease.AcquireAsync(Path.Combine(m_root, "artifacts/api-sync/writer.lock"), Timeout.InfiniteTimeSpan))
        {
            using var cancellation = new CancellationTokenSource();
            Task<int> import = DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package], cancellation.Token);
            Assert.False(import.IsCompleted);
            Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
            Assert.Equal(facts, File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json")));
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => import);
            Assert.Equal(facts, File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json")));
        }
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Contains("New import contract.", File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task ImportRestoresFactsWhenAuthoredContentRejectsSynchronization()
    {
        await Sync();
        Bind(description: "Changed package XML.");
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]);
        Bind();
        string member = Fragment(C_MEMBER);
        string invalid = File.ReadAllText(member).Replace("source: xml", "source: manul", StringComparison.Ordinal);
        File.WriteAllText(member, invalid);
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        byte[] facts = File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json"));
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
        Assert.Equal(facts, File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json")));
        Assert.Equal(invalid, File.ReadAllText(member));
        Assert.False(File.Exists(Path.Combine(m_root, "reference/api/export.json")));
    }

    [Fact]
    public async Task NestedXmlListsAndFollowingContractsSurviveComposition()
    {
        SetTypeDocumentation("""
            <member><summary>Owns a value.</summary><remarks>Before the list.
            <list type="bullet"><item><description><b>Borrow</b> the value.
            <para>Do not release it.</para><list type="number">
            <item><description>Check <c>IsAlive</c>.</description></item>
            <item><description>Use the <i>current</i> generation.</description></item>
            </list></description></item><item><description>Retain the owner.</description></item></list>
            <para>After the list, observe failure.</para></remarks></member>
            """);
        await Sync();
        string prose = File.ReadAllText(Fragment(C_TYPE));
        Assert.Contains("- **Borrow** the value.", prose);
        Assert.Contains("  Do not release it.", prose);
        Assert.Contains("  1. Check `IsAlive`.", prose);
        Assert.Contains("  1. Use the *current* generation.", prose);
        Assert.Contains("\n\nAfter the list, observe failure.", prose);
    }

    [Theory]
    [InlineData("<member><summary><unknown>Keep this constraint.</unknown></summary></member>")]
    [InlineData("<member><summary><list type=\"table\"><item>Keep this constraint.</item></list></summary></member>")]
    [InlineData("<!DOCTYPE member [<!ENTITY injected SYSTEM 'file:///missing'>]><member><summary>&injected;</summary></member>")]
    public async Task InvalidXmlCannotDiscardConstraintsOrReplaceLastGoodProse(string xml)
    {
        await Sync();
        string previous = File.ReadAllText(Fragment(C_TYPE));
        SetTypeDocumentation(xml);
        Exception? failure = await Record.ExceptionAsync(Sync);
        Assert.NotNull(failure);
        Assert.True(failure is InvalidDataException or System.Xml.XmlException);
        Assert.Equal(previous, File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task XmlExampleFencesPreserveIndentationAndMarkupAsCode()
    {
        SetTypeDocumentation("""
            <member><summary>Owns a value.</summary><example><code>
                if (ready)
                {
                    string marker = "``` &lt;script&gt;";
                }
            </code><para>Observe failure after this example.</para></example></member>
            """);
        await Sync();
        string prose = File.ReadAllText(Fragment(C_TYPE));
        Assert.Contains("````csharp\nif (ready)\n{\n    string marker = \"``` <script>\";\n}\n````", prose);
        Assert.Contains("Observe failure after this example.", prose);
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
        await Sync();
        JsonNode registry = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/content.json")))!;
        Assert.Equal(2, registry["entries"]!.AsArray().Count);
        Assert.DoesNotContain("Excluded.Probe", registry.ToJsonString());
    }

    private void SetTypes(params (string id, string signature)[] types)
    {
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        JsonNode prototype = facts["projects"]![0]!["symbols"]![0]!.DeepClone();
        var symbols = new JsonArray();
        foreach (var type in types)
        {
            JsonNode symbol = prototype.DeepClone();
            symbol["id"] = type.id;
            symbol["signature"] = type.signature;
            symbol["signatureHash"] = Hash(type.signature);
            symbols.Add(symbol);
        }
        facts["projects"]![0]!["symbols"] = symbols;
        string text = facts.ToJsonString();
        Write("reference/api/probe.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    private void SetTypeDocumentation(string xml)
    {
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        facts["projects"]![0]!["symbols"]![0]!["documentation"] = xml;
        string text = facts.ToJsonString();
        Write("reference/api/probe.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    private Task<int> Sync(CancellationToken cancellation) => DocumentationCommand.ExecuteAsync(["api-sync", "--docs-root", m_root], cancellation);

    private string Fragment(string id)
    {
        using JsonDocument registry = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(m_root, "reference/api/content.json")));
        string path = registry.RootElement.GetProperty("entries").EnumerateArray()
            .Single(entry => entry.GetProperty("symbol").GetString() == "engine:Inno.Probe:" + id).GetProperty("path").GetString()!;
        return Path.Combine(m_root, path);
    }

    private void Bind(bool member = true, string description = "The value contract.", string countType = "int")
    {
        string inventory = JsonSerializer.Serialize(new[] { new { name = "Inno.Probe", path = "src/Inno.Probe.csproj", surfaces = new[] { "source-clr" }, isTest = false } });
        Write("reference/projects/catalog.json", inventory);
        object Symbol(string id, string kind, string signature, string? parent) => new
        {
            project = "Inno.Probe", id, kind, signature, signatureHash = Hash(signature), source = "src/Value.cs", line = 1,
            documentation = "<member><summary>" + description + "</summary></member>", containingType = parent,
            baseType = (string?)null, interfaces = Array.Empty<string>()
        };
        object[] symbols = member
            ? [Symbol(C_TYPE, "NamedType", "public class Inno.Probe.Value", null), Symbol(C_MEMBER, "Property", "public " + countType + " Inno.Probe.Value.count { get; }", C_TYPE)]
            : [Symbol(C_TYPE, "NamedType", "public class Inno.Probe.Value", null)];
        string facts = JsonSerializer.Serialize(new { model = "source-clr", configuration = "Debug", target = (string?)null,
            projects = new[] { new { name = "Inno.Probe", project = "src/Inno.Probe.csproj", symbols } } });
        Write("reference/api/probe.json", facts);
        Write("reference/engine.lock.json", JsonSerializer.Serialize(new
        {
            repository = "https://github.com/Wolfiora/InnoEngine", revision = new string('a', 40), sourceTree = new string('b', 40),
            toolingRevision = new string('a', 40), projectManifest = "reference/projects/catalog.json", projectManifestIdentity = Hash(inventory),
            apiManifests = new[] { new { path = "reference/api/probe.json", sha256 = Hash(facts), model = "source-clr", configuration = "Debug",
                sourceRevision = new string('a', 40), sourceTree = new string('b', 40), target = (string?)null } }
        }));
    }

    private void Write(string relative, string text)
    {
        string path = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, text);
    }

    private static string Hash(string text) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text)));
}
