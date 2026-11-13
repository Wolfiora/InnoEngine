using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
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
        string cache = Path.Combine(m_root, "artifacts", "query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo page = Assert.Single(catalog.pages.Where(page => page.kind == "api"));
        Assert.Contains("public long Inno.Probe.Value.count", page.markdown);
        Assert.Contains("Read the count only after publication.", page.markdown);
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

    private Task<int> Sync() => Sync(CancellationToken.None);

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
