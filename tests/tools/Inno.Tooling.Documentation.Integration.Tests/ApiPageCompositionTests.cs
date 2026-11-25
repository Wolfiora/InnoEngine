using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Xml;
using Markdig;
using Xunit;

namespace Inno.Tooling.Documentation.Integration.Tests;

[Collection(nameof(DocumentationApiFixture))]
public sealed class ApiPageCompositionTests : DocumentationApiFixture
{
    [Theory]
    [InlineData("T:Inno.Probe.Value`1", "public class Inno.Probe.Value<TValue>", "Value<TValue>")]
    [InlineData("T:Inno.Probe.Value`2", "public class Inno.Probe.Value<TTarget, TArgument>", "Value<TTarget, TArgument>")]
    [InlineData("T:Inno.Probe.Outer`1.Value`1", "public class Inno.Probe.Outer<TOwner>.Value<TValue>", "Value<TValue>")]
    [InlineData("T:Inno.Probe.Outer`1.Value", "public class Inno.Probe.Outer<TOwner>.Value", "Value")]
    [InlineData("T:Inno.Probe.class`1", "public class Inno.Probe.@class<TValue>", "@class<TValue>")]
    [InlineData(
        "T:Inno.Probe.Factory`2",
        "public delegate TResult Inno.Probe.Factory<TValue, TResult>(TValue value)",
        "Factory<TValue, TResult>"
    )]
    public async Task GenericTitlesUseDeclaredNamesAndRenderAsText(
        string id,
        string signature,
        string expected
    ) {
        SetTypes((id, signature));
        await SyncAsync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single((await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api"));
        Assert.Equal(expected, page.title);
        Assert.Contains("<h1>" + WebUtility.HtmlEncode(expected) + "</h1>", Markdown.ToHtml(page.markdown));
        Assert.Contains(expected, (await snapshot.QueryAsync(new DocumentationRequest("symbol", id, "Inno.Probe"))).hits.Single().text);
    }

    [Fact]
    public async Task GenericParameterRenamesPreserveAddressesAndManualProseWithReadableDisambiguation()
    {
        const string C_ONE = "T:Inno.Probe.Value`1";
        const string C_TWO = "T:Inno.Probe.Value`2";
        const string C_OTHER = "T:Other.Value`1";
        SetTypes(
            (C_TYPE, "public class Inno.Probe.Value"),
            (C_ONE, "public class Inno.Probe.Value<TValue>"),
            (C_TWO, "public class Inno.Probe.Value<TTarget, TArgument>"),
            (C_OTHER, "public class Other.Value<TValue>")
        );
        await SyncAsync();
        string[] paths = new[] { C_TYPE, C_ONE, C_TWO, C_OTHER }.Select(Fragment).ToArray();
        Assert.Equal(4, paths.Distinct().Count());
        string manual = File.ReadAllText(paths[2]).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
                        + "\nKeep the target alive while the operation runs.\n";
        File.WriteAllText(paths[2], manual);
        SetTypes(
            (C_TYPE, "public class Inno.Probe.Value"),
            (C_ONE, "public class Inno.Probe.Value<TValue>"),
            (C_TWO, "public class Inno.Probe.Value<TOwner, TArgument>"),
            (C_OTHER, "public class Other.Value<TValue>")
        );
        await SyncAsync();
        Assert.Equal(paths, new[] { C_TYPE, C_ONE, C_TWO, C_OTHER }.Select(Fragment));
        Assert.Equal(Body(manual), Body(File.ReadAllText(paths[2])));
        Assert.Contains("outdatedFor:", File.ReadAllText(paths[2]));
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
        Assert.Equal(Body(manual), Body(File.ReadAllText(paths[2])));
        Assert.Contains("outdatedFor:", File.ReadAllText(paths[2]));
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
            foreach (JsonNode? symbol in projected["projects"]![0]!["symbols"]!.AsArray())
            {
                symbol!["scriptingScope"] = scope;
            }
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
        await SyncAsync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo script = Assert.Single(
            catalog.pages.Where(page => page.kind == "api" && page.scope == scope && page.symbols.ContainsKey(C_MEMBER))
        );
        Assert.Contains("scripting/" + scope.ToLowerInvariant(), script.path);
        string scopeDirectory = "content/en/api/scripting/" + scope.ToLowerInvariant();
        DocumentationPageInfo scopePage = catalog.pages.Single(page => page.path == scopeDirectory + "/index.md");
        Assert.Contains("[Probe](_projects/probe/index.md)", scopePage.markdown);
        DocumentationPageInfo projectPage = catalog.pages.Single(page => page.path == scopeDirectory + "/_projects/probe/index.md");
        Assert.Contains("## Types", projectPage.markdown);
        Assert.Contains("| [Value]", projectPage.markdown);
        DocumentationPageInfo typePage = Assert.Single(
            catalog.pages.Where(page => page.kind == "api" && page.scope == scope && page.symbols.ContainsKey(C_TYPE))
        );
        string typeLink = Path.GetRelativePath(Path.GetDirectoryName(projectPage.path)!, typePage.path).Replace('\\', '/');
        Assert.Contains("](" + typeLink + ")", projectPage.markdown);
        Assert.DoesNotContain("/_projects/", script.path);
        foreach (string surface in new[] { "runtime-script", "editor-script" })
        {
            DocumentationHit hit = Assert.Single(
                (await snapshot.QueryAsync(new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe", surface, scope: scope))).hits
            );
            Assert.Equal(script.id, hit.pageId);
            Assert.Equal(scope, hit.scope);
            Assert.Equal(new[] { "editor-script", "runtime-script" }, hit.surfaces);
        }
        Assert.Empty(
            (await snapshot.QueryAsync(
                new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe", scope: scope == "Editor" ? "Runtime" : "Editor")
            )).hits
        );
    }

    [Fact]
    public async Task WebsiteCatalogAndSymbolReadsShareComposedDeclarationsAndManualProse()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string manual = File.ReadAllText(member).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
                        + "\nRead the count only after publication.\n";
        File.WriteAllText(member, manual);
        Bind(description: "Updated XML.", countType: "long");
        await SyncAsync();
        Write(
            "content/en/manual/ownership.md",
            "---\nuid: manual.ownership\ntitle: Ownership and retirement\nkind: explanation\n---\n\n# Ownership and retirement\n\n## Release\n\nKeep the owner alive until all borrows end.\n"
        );
        string typePath = Fragment(C_TYPE);
        File.WriteAllText(
            typePath,
            File.ReadAllText(typePath)
                .Replace(
                    "contracts: []",
                    "contracts: [manual.ownership#release]\nexampleMode: host\nexampleProjects: [Inno.Probe]",
                    StringComparison.Ordinal
                )
        );
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
        DocumentationResult result = await snapshot.QueryAsync(new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe", "source-clr"));
        DocumentationHit hit = Assert.Single(result.hits);
        Assert.Equal("Ada Example", hit.contentSourceKind);
        Assert.Contains("public long Inno.Probe.Value.count", hit.text);
        Assert.Contains("Read the count only after publication.", hit.text);
        Assert.Equal(Body(manual), Body(File.ReadAllText(member)));
        Assert.Contains("outdatedFor:", File.ReadAllText(member));
        Assert.Contains(
            hit.text,
            Assert.Single((await snapshot.QueryAsync(new DocumentationRequest("read", hit.contentSource))).hits).text
        );
        Directory.Move(Path.Combine(m_root, "content"), Path.Combine(m_root, "unavailable-content"));
        Assert.Equal(
            hit.text,
            Assert.Single((await snapshot.QueryAsync(new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe"))).hits).text
        );
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
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        File.WriteAllText(
            member,
            File.ReadAllText(member).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
            + "\n```csharp\nvar count = value.count;\n```\n"
        );
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single(
            (await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api" && page.symbols.ContainsKey(C_MEMBER))
        );
        Assert.Equal(2, page.codeBlocks.Count(block => block.declarationSymbols.Count != 0));
        Assert.Equal("var count = value.count;", Assert.Single(page.codeBlocks.Where(block => block.declarationSymbols.Count == 0)).text);
        Assert.Contains(C_VARIANT, page.symbols[C_MEMBER].code);
    }

    [Theory]
    [InlineData("xml", true)]
    [InlineData("Ada Example", false)]
    [InlineData("", true)]
    public async Task EditorialNoticeFollowsOwnershipAndSourceLinkEndsThePage(
        string source,
        bool notice
    ) {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string original = File.ReadAllText(member);
        File.WriteAllText(
            member,
            original.Replace("source: xml", source.Length == 0 ? string.Empty : "source: " + source, StringComparison.Ordinal)
        );
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single(
            (await snapshot.ExportCatalogAsync()).pages.Where(page => page.symbols.ContainsKey(C_MEMBER))
        );
        Assert.Equal(notice, page.markdown.Contains("> [!SYNCED]", StringComparison.Ordinal));
        if (notice)
        {
            Assert.True(
                page.markdown.IndexOf("> [!SYNCED]", StringComparison.Ordinal) < page.markdown.IndexOf(
                    "## Required contracts",
                    StringComparison.Ordinal
                )
            );
        }
        Assert.DoesNotContain("Available in:", page.markdown);
        Assert.DoesNotContain("XML-synced", page.markdown);
        Assert.DoesNotContain("Edit this explanation", page.markdown);
        Assert.Contains("<!-- api-source -->\n[Source declaration](https://github.com/", page.markdown);
        Assert.EndsWith(")", page.markdown.TrimEnd());
    }

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
        await SyncAsync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo[] pages = (await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api").ToArray();
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

        void Add(
            string id,
            string kind,
            string signature,
            string parent
        ) {
            JsonNode symbol = symbols[0]!.DeepClone();
            symbol["id"] = id;
            symbol["kind"] = kind;
            symbol["signature"] = signature;
            symbol["signatureHash"] = Hash(signature);
            symbol["containingType"] = parent;
            symbols.Add(symbol);
        }
    }

    [Fact]
    public async Task MovingLegacyMembersRepairsIncomingAnchorsAndManualRelativeLinks()
    {
        await SyncAsync();
        string current = Fragment(C_MEMBER);
        string legacy = Path.Combine(Path.GetDirectoryName(current)!, "_members/count.md");
        Directory.CreateDirectory(Path.GetDirectoryName(legacy)!);
        string text = File.ReadAllText(current).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
                      + "\nKeep the original explanation. [Owner](../index.md).\n";
        File.WriteAllText(legacy, text);
        File.Delete(current);
        string registryPath = Path.Combine(m_root, "reference/api/content.json");
        JsonNode registry = JsonNode.Parse(File.ReadAllText(registryPath))!;
        registry["entries"]!
            .AsArray()
            .Single(
                entry => entry!["symbol"]!.GetValue<string>().EndsWith(C_MEMBER, StringComparison.Ordinal)
            )!["path"] = Path.GetRelativePath(m_root, legacy).Replace('\\', '/');
        File.WriteAllText(registryPath, registry.ToJsonString());
        Write(
            "content/en/manual/count.md",
            "---\nuid: manual.count\ntitle: Count\nkind: how-to\n---\n\n# Count\n\n"
            + "[Old member](../api/engine/probe/value/_members/count.md) and [exact member](../api/engine/probe/value/index.md#api-"
            + Hash(C_MEMBER)[..16]
            + ").\n"
        );
        await SyncAsync();
        Assert.False(File.Exists(legacy));
        Assert.Contains("Keep the original explanation. [Owner](index.md).", File.ReadAllText(current));
        Assert.Contains("source: Ada Example", File.ReadAllText(current));
        string guide = File.ReadAllText(Path.Combine(m_root, "content/en/manual/count.md"));
        Assert.DoesNotContain("_members", guide);
        Assert.DoesNotContain("index.md#api-", guide);
        string cache = Path.Combine(m_root, "artifacts/query");
        await DocumentationStore.IndexAsync(m_root, m_root, cache);
        string after = File.ReadAllText(current);
        await SyncAsync();
        Assert.Equal(after, File.ReadAllText(current));
    }

    [Fact]
    public async Task XmlRefreshPreservesAuthoredContractAndExampleAssociations()
    {
        await SyncAsync();
        string type = Fragment(C_TYPE);
        string text = File.ReadAllText(type)
            .Replace(
                "contracts: []",
                "contracts: [manual.ownership#release]\naliases: [persistent value]\nexampleMode: host\nexampleProjects: [Inno.Probe]",
                StringComparison.Ordinal
            );
        File.WriteAllText(type, text);
        Bind(description: "Fresh XML still needs shared ownership guidance.");
        await SyncAsync();
        string updated = File.ReadAllText(type);
        Assert.Contains("manual.ownership#release", updated);
        Assert.Contains("persistent value", updated);
        Assert.Contains("exampleMode: host", updated);
        Assert.Contains("exampleProjects:", updated);
        Assert.DoesNotContain("Fresh XML still needs", updated);
        Assert.Contains("Fresh XML still needs", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/index.md")));
        await SyncAsync();
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
        await SyncAsync();
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
            foreach (JsonNode? symbol in projected["projects"]![0]!["symbols"]!.AsArray())
            {
                symbol!["scriptingScope"] = "Runtime";
            }
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
            InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(SyncAsync);
            Assert.Contains("different Runtime projection", failure.Message);
            return;
        }
        await SyncAsync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationHit hit = Assert.Single(
            (await snapshot.QueryAsync(new DocumentationRequest("symbol", C_MEMBER, "Inno.Probe", "editor-script", scope: "Runtime"))).hits
        );
        Assert.Equal(new[] { "editor-script", "runtime-script" }, hit.surfaces);
    }

    [Fact]
    public async Task XmlSummaryAndSectionsHaveTheSameReadingOrderInMarkdownAndQueries()
    {
        SetTypeDocumentation("""
            <member><remarks>Keep the owner alive.</remarks><summary>Owns a value.<para>Release it when finished.</para></summary>
            <typeparam name="T">The owned type.</typeparam><returns>The current value.</returns>
            <example><code>var value = Read();</code></example></member>
            """);
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        facts["projects"]![0]!["symbols"]![0]!["baseType"] = "System.Object";
        SaveFacts(facts);
        await SyncAsync();
        string prose = File.ReadAllText(Fragment(C_TYPE));
        Assert.DoesNotContain("Owns a value.", prose);
        Assert.DoesNotContain("Release it when finished.", prose);
        Assert.Contains("## Remarks\n\nKeep the owner alive.", prose);
        Assert.Contains("## Type parameters", prose);
        Assert.Contains("## Returns\n\nThe current value.", prose);
        Assert.Contains("## Example", prose);
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single(
            (await snapshot.ExportCatalogAsync()).pages.Where(page => page.symbols.ContainsKey(C_TYPE))
        );
        Assert.Contains("# Value\n\n> - Owns a value.\n>   \n>   Release it when finished.", page.markdown);
        Assert.Contains("> - Owns", page.markdown);
        Assert.True(
            page.markdown.IndexOf("> [!SYNCED]", StringComparison.Ordinal) < page.markdown.IndexOf("# Value", StringComparison.Ordinal)
        );
        Assert.Contains("**API:** engine  \n**Inherits:** `System.Object`  \n**Project:** `Inno.Probe`", page.markdown);
        Assert.True(
            page.markdown.IndexOf(
                "## Declaration",
                StringComparison.Ordinal
            ) < page.markdown.IndexOf("## Remarks", StringComparison.Ordinal)
        );
        Assert.DoesNotContain("#### Remarks", page.markdown);
        DocumentationHit hit = Assert.Single((await snapshot.QueryAsync(new DocumentationRequest("symbol", C_TYPE, "Inno.Probe"))).hits);
        Assert.Contains("Owns a value.", hit.text);
        Assert.Contains("Keep the owner alive.", hit.text);
        Assert.Equal(1, page.markdown.Split("Owns a value.", StringSplitOptions.None).Length - 1);
    }

    [Fact]
    public async Task PartialSummariesRemainSeparateQuotedItemsWithoutSplittingTheirParagraphs()
    {
        SetTypeDocumentation("""
            <member><summary>First declaration.<para>Still the first summary.</para></summary>
            <summary>Second declaration with <c>Value</c>.</summary><remarks>Shared lifetime.</remarks></member>
            """);
        await SyncAsync();
        string cache = Path.Combine(m_root, "artifacts/query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = Assert.Single(
            (await snapshot.ExportCatalogAsync()).pages.Where(page => page.symbols.ContainsKey(C_TYPE))
        );
        Assert.Contains(
            "> - First declaration.\n>   \n>   Still the first summary.\n> \n> - Second declaration with `Value`.",
            page.markdown
        );
        string html = Markdown.ToHtml(page.markdown);
        string summaryHtml = html[html.IndexOf("<blockquote>\n<ul>", StringComparison.Ordinal)..];
        summaryHtml = summaryHtml[..summaryHtml.IndexOf("</blockquote>", StringComparison.Ordinal)];
        Assert.Equal(2, summaryHtml.Split("<li>", StringSplitOptions.None).Length - 1);
        Assert.Contains("<blockquote>\n<ul>", html);
        Assert.Contains("## Remarks\n\nShared lifetime.", File.ReadAllText(Fragment(C_TYPE)));
        Assert.DoesNotContain("First declaration.", File.ReadAllText(Fragment(C_TYPE)));
        DocumentationHit hit = Assert.Single((await snapshot.QueryAsync(new DocumentationRequest("symbol", C_TYPE, "Inno.Probe"))).hits);
        Assert.Contains("> - Second declaration with `Value`.", hit.text);
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
        await SyncAsync();
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
        await SyncAsync();
        string previous = File.ReadAllText(Fragment(C_TYPE));
        SetTypeDocumentation(xml);
        Exception? failure = await Record.ExceptionAsync(SyncAsync);
        Assert.NotNull(failure);
        Assert.True(failure is InvalidDataException or XmlException);
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
        await SyncAsync();
        string prose = File.ReadAllText(Fragment(C_TYPE));
        Assert.Contains("````csharp\nif (ready)\n{\n    string marker = \"``` <script>\";\n}\n````", prose);
        Assert.Contains("Observe failure after this example.", prose);
    }

    private void SaveFacts(JsonNode facts)
    {
        string text = facts.ToJsonString();
        Write("reference/api/probe.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["apiManifests"]![0]!["sha256"] = Hash(text);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }

    private void SetTypes(params (string id, string signature)[] types)
    {
        JsonNode facts = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/api/probe.json")))!;
        JsonNode prototype = facts["projects"]![0]!["symbols"]![0]!.DeepClone();
        var symbols = new JsonArray();
        foreach ((string id, string signature) type in types)
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
}
