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
        DocumentationPageInfo script = Assert.Single(catalog.pages.Where(page => page.kind == "api" && page.scope == scope));
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
        File.WriteAllText(typePath, File.ReadAllText(typePath).Replace("kind: api", "kind: api\ncontracts: [manual.ownership#release]", StringComparison.Ordinal));
        string cache = Path.Combine(m_root, "artifacts", "query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo page = Assert.Single(catalog.pages.Where(page => page.kind == "api"));
        Assert.Contains("public long Inno.Probe.Value.count", page.markdown);
        Assert.Contains("Read the count only after publication.", page.markdown);
        Assert.Contains("## Required contracts", page.markdown);
        Assert.Contains("[Ownership and retirement](../../../../manual/ownership.md#release)", page.markdown);
        Assert.Contains("https://github.com/Wolfiora/InnoEngine.Docs/edit/draft/" + Path.GetRelativePath(m_root, member).Replace('\\', '/'), page.markdown);
        Assert.Contains("reference/api/content.json", catalog.sourceFiles.Keys);
        Assert.Contains(Path.GetRelativePath(m_root, member).Replace('\\', '/'), catalog.sourceFiles.Keys);
        DocumentationResult result = await snapshot.QueryAsync(new("symbol", C_MEMBER, "Inno.Probe", "source-clr"));
        DocumentationHit hit = Assert.Single(result.hits);
        Assert.Equal("manual", hit.contentSourceKind);
        Assert.Contains("public long Inno.Probe.Value.count", hit.text);
        Assert.Contains("Read the count only after publication.", hit.text);
        Assert.Equal(manual, File.ReadAllText(member));
        Assert.Equal(hit.text, Assert.Single((await snapshot.QueryAsync(new("read", hit.contentSource))).hits).text);
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
        DocumentationPageInfo page = Assert.Single((await snapshot.ExportCatalogAsync()).pages.Where(page => page.kind == "api"));
        Assert.Equal(3, page.codeBlocks.Count(block => block.declarationSymbols.Count != 0));
        Assert.Equal("var count = value.count;", Assert.Single(page.codeBlocks.Where(block => block.declarationSymbols.Count == 0)).text);
        Assert.Contains(C_VARIANT, page.symbols[C_MEMBER].code);
    }

    private Task<int> Sync() => Sync(CancellationToken.None);

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
