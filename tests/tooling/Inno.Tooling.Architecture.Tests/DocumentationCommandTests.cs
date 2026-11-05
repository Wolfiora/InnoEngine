using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class DocumentationCommandTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDocumentationTests", Guid.NewGuid().ToString("N"));
    private const string C_ENTRY = "docs/modules/Inno.Probe.md";
    private const string C_SYMBOL = "M:Inno.Probe.Read(System.String)";

    public DocumentationCommandTests()
    {
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00");
        Write("tools/Inno.Probe/Inno.Probe.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\" />");
        Write("docs/modules/README.md", "[Probe](Inno.Probe.md)");
        Write(C_ENTRY, "# Inno.Probe\n\n## 使用契约\n\n借用资源由调用方释放，不能跨线程缓存。\n\n<!-- api-include: Inno.Probe/api.md -->\n");
        string anchor = Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(C_SYMBOL)))[..16];
        Write("docs/modules/Inno.Probe/api.md", "# Probe API\n\n## Read\n\n<!-- api: " + C_SYMBOL + " " + new string('a', 64)
            + " -->\n\n| 声明 | 行为 | 约束 | 失败 |\n| --- | --- | --- | --- |\n| <a id=\"api-" + anchor
            + "\"></a>`Read(string key)` | Returns the current value. | Caller owns the lease. | Throws on missing key. |\n");
    }

    public void Dispose()
    {
        // The fixture owns this unique root; all awaited commands have released their leases.
        Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public async Task SymbolReturnsDeclarationAndUsageContractWithoutAnSdk()
    {
        using JsonDocument result = await RunAsync("symbol", "--project", "Inno.Probe", "--id", C_SYMBOL);
        string text = result.RootElement.GetRawText();
        Assert.Contains("Read(string key)", text);
        Assert.Contains("Caller owns the lease", text);
        Assert.Contains("不能跨线程缓存", text);
        Assert.Equal("Unverified", result.RootElement.GetProperty("sourceApiStatus").GetString());
        Assert.Contains("tools/Inno.Probe/Inno.Probe.csproj", DocumentationCommand.GetProjectDocuments(m_root).Values);
        string anchor = "api-" + Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(C_SYMBOL)))[..16];
        using JsonDocument anchored = await RunAsync("read", "--page", "docs/modules/Inno.Probe/api.md", "--section", anchor);
        Assert.Contains("Caller owns the lease", anchored.RootElement.GetRawText());
        Assert.Contains("不能跨线程缓存", anchored.RootElement.GetRawText());
    }

    [Fact]
    public async Task ProjectOverviewRanksAheadOfAQueryTutorialMentioningTheSameIdentifier()
    {
        Write(C_ENTRY, File.ReadAllText(Path.Combine(m_root, C_ENTRY)).Replace("# Inno.Probe\n", "# Inno.Probe\n\nProbeStyle owns internal colors.\n", StringComparison.Ordinal));
        Write("docs/README.md", "# Docs\n\n## Queries\nUse docs search --query ProbeStyle.\n");
        using JsonDocument result = await RunAsync("search", "--query", "ProbeStyle");
        Assert.Equal(C_ENTRY, result.RootElement.GetProperty("hits")[0].GetProperty("page").GetString());
    }

    [Fact]
    public async Task SameLengthAndTimestampEditsInvalidateTheSnapshot()
    {
        using JsonDocument before = await RunAsync("search", "--query", "借用资源");
        string path = Path.Combine(m_root, C_ENTRY);
        DateTime modified = File.GetLastWriteTimeUtc(path);
        File.WriteAllText(path, File.ReadAllText(path).Replace("借用资源", "冻结资源", StringComparison.Ordinal));
        File.SetLastWriteTimeUtc(path, modified);
        using JsonDocument after = await RunAsync("search", "--query", "冻结资源");
        Assert.NotEqual(before.RootElement.GetProperty("snapshot").GetString(), after.RootElement.GetProperty("snapshot").GetString());
        Assert.Contains("冻结资源", after.RootElement.GetRawText());
    }

    [Fact]
    public async Task SearchHasExplicitEmptyResultsAndOverloadsRequireExactIds()
    {
        using JsonDocument empty = await RunAsync("search", "--query", "NoSuchConcept");
        Assert.Equal("Empty", empty.RootElement.GetProperty("status").GetString());
        using JsonDocument ambiguous = await RunAsync("symbol", "--project", "Inno.Probe", "--id", "Read");
        Assert.Equal("Ambiguous", ambiguous.RootElement.GetProperty("status").GetString());
        await Assert.ThrowsAsync<ArgumentException>(() => RunAsync("symbol", "--project", "Inno.Probe", "--id", "Missing"));
    }

    [Fact]
    public async Task ReferenceLinksFencesTablesAndDuplicateHeadingsRemainDistinct()
    {
        Write("docs/guide.md", "# Guide\n\n[Probe][project]\n\n[project]: modules/Inno.Probe.md\n\n```text\n# Hidden\n<!-- api-include: ../../outside.md -->\n```\n\n## Repeated\nFirst\n\n## Repeated\nSecond\n");
        using JsonDocument first = await RunAsync("read", "--page", "docs/guide.md", "--section", "repeated");
        Assert.Contains("First", first.RootElement.GetRawText());
        Assert.DoesNotContain("Second", first.RootElement.GetRawText());
        using JsonDocument search = await RunAsync("search", "--query", "Repeated");
        Assert.Equal(2, search.RootElement.GetProperty("hits").GetArrayLength());
    }

    [Fact]
    public async Task InclusionCycleOrEscapeCannotReplaceACompleteIndex()
    {
        using JsonDocument initial = await RunAsync("index");
        string index = Path.Combine(m_root, "artifacts/documentation/index.json");
        byte[] previous = File.ReadAllBytes(index);
        Write("docs/modules/Inno.Probe/api.md", "<!-- api-include: ../Inno.Probe.md -->");
        await Assert.ThrowsAsync<InvalidDataException>(() => RunAsync("index"));
        Assert.Equal(previous, File.ReadAllBytes(index));
        Write("docs/modules/Inno.Probe/api.md", "<!-- api-include: ../../../../outside.md -->");
        await Assert.ThrowsAsync<IOException>(() => RunAsync("index"));
        Assert.Equal(previous, File.ReadAllBytes(index));
    }

    [Fact]
    public async Task MovesAndDeletionUpdateTheIndexAndRejectMissingPages()
    {
        Write("docs/old.md", "# Movable\nUniqueMarker");
        using JsonDocument before = await RunAsync("search", "--query", "UniqueMarker");
        File.Move(Path.Combine(m_root, "docs/old.md"), Path.Combine(m_root, "docs/new.md"));
        using JsonDocument moved = await RunAsync("search", "--query", "UniqueMarker");
        Assert.Contains("docs/new.md", moved.RootElement.GetRawText());
        Assert.DoesNotContain("docs/old.md", moved.RootElement.GetRawText());
        File.Delete(Path.Combine(m_root, "docs/new.md"));
        await Assert.ThrowsAsync<FileNotFoundException>(() => RunAsync("read", "--page", "docs/new.md"));
        await Assert.ThrowsAsync<IOException>(() => RunAsync("read", "--page", ".agents/rulebook/coding.md"));
    }

    [Fact]
    public async Task CorruptPageCacheIsRebuiltAndCancellationPreservesTheIndex()
    {
        using JsonDocument initial = await RunAsync("index");
        string output = Path.Combine(m_root, "artifacts/documentation");
        foreach (string cache in Directory.GetFiles(Path.Combine(output, "pages"), "*.json"))
            File.WriteAllText(cache, "{invalid");
        using JsonDocument repaired = await RunAsync("index");
        Assert.Equal(initial.RootElement.GetProperty("snapshot").GetString(), repaired.RootElement.GetProperty("snapshot").GetString());
        byte[] previous = File.ReadAllBytes(Path.Combine(output, "index.json"));
        using var canceled = new CancellationTokenSource();
        canceled.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => DocumentationCommand.ExecuteAsync(["index", "--engine-root", m_root], canceled.Token));
        Assert.Equal(previous, File.ReadAllBytes(Path.Combine(output, "index.json")));
    }

    [Fact]
    public async Task ConcurrentIndexCommandsPublishOneConsistentSnapshot()
    {
        using var output = new StringWriter(CultureInfo.InvariantCulture);
        TextWriter previous = Console.Out;
        try
        {
            Console.SetOut(output);
            await Task.WhenAll(Enumerable.Range(0, 3).Select(_ => Task.Run(() =>
                DocumentationCommand.ExecuteAsync(["index", "--engine-root", m_root, "--format", "json"]))));
        }
        finally { Console.SetOut(previous); }
        using JsonDocument index = JsonDocument.Parse(File.ReadAllText(Path.Combine(m_root, "artifacts/documentation/index.json")));
        Assert.Equal(3, index.RootElement.GetProperty("entries").EnumerateObject().Count());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task LargeSectionsCanBeReadWithoutLosingAnyBytes(bool hasChildren)
    {
        string source = "# Contract\n\n" + string.Concat(Enumerable.Repeat("Owned by caller; release only after the final consumer.\n", 700));
        if (hasChildren)
            source += "\n## Cancellation\nCancellation does not transfer ownership.\n";
        Write("docs/large.md", source);
        var recovered = new StringBuilder();
        int offset = 0;
        do
        {
            using JsonDocument page = await RunAsync("read", "--page", "docs/large.md", "--section", "contract", "--offset",
                offset.ToString(CultureInfo.InvariantCulture));
            recovered.Append(page.RootElement.GetProperty("hits")[0].GetProperty("text").GetString());
            JsonElement next = page.RootElement.GetProperty("continuations");
            if (next.GetArrayLength() == 0)
                break;
            int position = next[0].GetProperty("offset").GetInt32();
            Assert.True(position > offset);
            offset = position;
        } while (true);
        Assert.Equal(source, recovered.ToString());
    }

    [Fact]
    public async Task LargePageNavigationRetainsPreambleAndEveryHeadingLevel()
    {
        Write("docs/large.md", "Preamble contract.\n\n# First\nOwner contract.\n\n### Deep\n"
            + new string('x', 18000) + "\n\n# Second\nRetirement contract.\n");
        using JsonDocument page = await RunAsync("read", "--page", "docs/large.md");
        Assert.Equal("Navigation", page.RootElement.GetProperty("status").GetString());
        Assert.Contains("Preamble contract", page.RootElement.GetProperty("hits")[0].GetProperty("text").GetString());
        string?[] sections = page.RootElement.GetProperty("continuations").EnumerateArray()
            .Select(item => item.GetProperty("section").GetString()).ToArray();
        Assert.Equal(new[] { "first", "deep", "second" }, sections);
        using JsonDocument first = await RunAsync("read", "--page", "docs/large.md", "--section", "first");
        Assert.Contains("Owner contract", first.RootElement.GetProperty("hits")[0].GetProperty("text").GetString());
    }

    [Fact]
    public async Task ModifiedDerivedCacheCannotForgeTheDocumentContract()
    {
        using JsonDocument initial = await RunAsync("index");
        string output = Path.Combine(m_root, "artifacts/documentation");
        foreach (string cache in Directory.GetFiles(Path.Combine(output, "pages"), "*.json"))
        {
            JsonNode payload = JsonNode.Parse(File.ReadAllText(cache))!;
            JsonArray symbols = payload["page"]!["symbols"]!.AsArray();
            if (symbols.Count == 0)
                continue;
            symbols[0]!["declaration"] = "Forged permission to dispose borrowed resources.";
            File.WriteAllText(cache, payload.ToJsonString());
        }
        using JsonDocument result = await RunAsync("symbol", "--project", "Inno.Probe", "--id", C_SYMBOL);
        Assert.Contains("Caller owns the lease", result.RootElement.GetRawText());
        Assert.DoesNotContain("Forged permission", result.RootElement.GetRawText());
    }

    [Fact]
    public async Task OrphanApiAndDuplicateProjectsCannotProduceAnIndex()
    {
        Write("docs/orphan.md", File.ReadAllText(Path.Combine(m_root, "docs/modules/Inno.Probe/api.md")));
        await Assert.ThrowsAsync<InvalidDataException>(() => RunAsync("index"));
        File.Delete(Path.Combine(m_root, "docs/orphan.md"));
        Write("tools/duplicate/Inno.Probe.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\" />");
        await Assert.ThrowsAnyAsync<ArgumentException>(() => RunAsync("index"));
    }

    [Fact]
    public async Task MovingTheProjectChangesOwnershipIdentityWithoutAnSdk()
    {
        using JsonDocument before = await RunAsync("index");
        Directory.CreateDirectory(Path.Combine(m_root, "tools/moved"));
        File.Move(Path.Combine(m_root, "tools/Inno.Probe/Inno.Probe.csproj"), Path.Combine(m_root, "tools/moved/Inno.Probe.csproj"));
        using JsonDocument after = await RunAsync("index");
        Assert.NotEqual(before.RootElement.GetProperty("snapshot").GetString(), after.RootElement.GetProperty("snapshot").GetString());
    }

    private async Task<JsonDocument> RunAsync(params string[] arguments)
    {
        using var output = new StringWriter(CultureInfo.InvariantCulture);
        TextWriter previous = Console.Out;
        try
        {
            Console.SetOut(output);
            Assert.Equal(0, await DocumentationCommand.ExecuteAsync([..arguments, "--engine-root", m_root, "--format", "json"]));
            return JsonDocument.Parse(output.ToString());
        }
        finally { Console.SetOut(previous); }
    }

    private void Write(
        string relative,
        string text
    ) {
        string path = Path.Combine(m_root, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);
        File.WriteAllText(path, text);
    }
}
