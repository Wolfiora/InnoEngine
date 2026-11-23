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
public sealed class ProjectGuideSynchronizationTests : DocumentationApiFixture
{
    [Fact]
    public async Task ProjectGuideBecomesAuthoredOnEditAndTracksSubsequentSourceChanges()
    {
        const string C_PAGE = "content/en/architecture/projects/probe.md";
        Write(
            "reference/projects/content.json",
            JsonSerializer.Serialize(new
            {
                schema = "inno-project-guides/1",
                entries = new[] { new { path = C_PAGE, uid = "project.probe", project = "Inno.Probe" } }
            })
        );
        Write(
            C_PAGE,
            "---\nuid: project.probe\nsymbol: project:Inno.Probe\ntitle: Inno.Probe\nkind: project\nowners: [Inno.Probe]\nsource: xml\n---\n"
        );
        Bind(description: "The value\ncontract.");
        await SyncAsync();
        string path = Path.Combine(m_root, C_PAGE);
        Assert.Contains("The value contract.", File.ReadAllText(path));
        string first = File.ReadAllText(path);
        await SyncAsync();
        Assert.Equal(first, File.ReadAllText(path));
        string manual = first.Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
                        + "\nIndependent lifetime guidance.\n";
        File.WriteAllText(path, manual);
        await SyncAsync();
        Assert.Equal(manual, File.ReadAllText(path));
        string cache = Path.Combine(m_root, "artifacts/project-query");
        DocumentationResult initial = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using (DocumentationSnapshot initialSnapshot = await DocumentationStore.OpenAsync(cache, initial.snapshot))
        {
            Assert.Contains(
                "[!AUTHORED]",
                (await initialSnapshot.ExportCatalogAsync()).pages.Single(page => page.id == "project.probe").markdown
            );
        }
        string original = File.ReadAllText(path);
        Bind(description: "The value\r\ncontract.");
        await SyncAsync();
        Assert.Equal(original, File.ReadAllText(path));
        Bind(description: "Changed XML contract.");
        await SyncAsync();
        Assert.Equal(Body(manual), Body(File.ReadAllText(path)));
        Assert.Contains("outdatedFor:", File.ReadAllText(path));
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        File.AppendAllText(path, "\nUpdated explanation.\n");
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationPageInfo page = (await snapshot.ExportCatalogAsync()).pages.Single(page => page.id == "project.probe");
        Assert.Contains("[!AUTHORED]", page.markdown);
        Assert.Contains("Independent lifetime guidance.", page.markdown);
        Assert.Contains("src/Inno.Probe.csproj", page.markdown);
    }

    [Fact]
    public async Task ProjectGuideRegistryCannotClaimUnrelatedManualPages()
    {
        Write("content/en/manual/keep.md", "Preserve this unrelated guide.");
        Write(
            "reference/projects/content.json",
            JsonSerializer.Serialize(new
            {
                schema = "inno-project-guides/1",
                entries = new[] { new { path = "content/en/manual/keep.md", uid = "manual.keep", project = "Inno.Probe" } }
            })
        );
        await Assert.ThrowsAsync<InvalidDataException>(() => SyncAsync(CancellationToken.None));
        Assert.Equal("Preserve this unrelated guide.", File.ReadAllText(Path.Combine(m_root, "content/en/manual/keep.md")));
    }

    [Fact]
    public async Task NewProjectsReceiveSynchronizedGuidesBesideTheirOwnerKind()
    {
        const string C_AUTHORED = "content/en/contributing/testing/projects/authored.md";
        const string C_AUTHORED_TEXT = "---\nuid: project.authored\ntitle: Authored\nkind: project\nowners:\n  - Inno.Authored.Tests\n---\n# Authored\n";
        WriteGuideRegistry();
        Write(C_AUTHORED, C_AUTHORED_TEXT);
        SetProjects(
            ("Inno.Probe", "src/Inno.Probe.csproj"),
            ("Inno.Probe.Tests", "tests/Inno.Probe.Tests/Inno.Probe.Tests.csproj"),
            ("Inno.Adapter.Probe", "backends/Probe/Inno.Adapter.Probe.csproj"),
            ("Inno.Platform.Probe", "platforms/Probe/Inno.Platform.Probe.csproj"),
            ("Inno.Authored.Tests", "tests/Inno.Authored.Tests/Inno.Authored.Tests.csproj")
        );
        await SyncAsync();
        string source = File.ReadAllText(Path.Combine(m_root, "content/en/architecture/projects/inno-probe.md"));
        Assert.Contains(
            "uid: project.inno-probe\nsymbol: project:Inno.Probe\ntitle: Inno.Probe\nkind: project\nowners:\n- Inno.Probe\n",
            source
        );
        Assert.Contains("source: xml", source);
        Assert.Contains("The value contract.", source);
        Assert.Contains(
            "source: xml",
            File.ReadAllText(Path.Combine(m_root, "content/en/contributing/testing/projects/inno-probe-tests.md"))
        );
        Assert.Contains("source: xml", File.ReadAllText(Path.Combine(m_root, "content/en/backends/projects/inno-adapter-probe.md")));
        string[] expected = ["content/en/architecture/projects/inno-probe.md", "content/en/backends/projects/inno-adapter-probe.md",
            "content/en/contributing/testing/projects/inno-probe-tests.md"];
        Assert.Equal(expected, GuideRegistry());
        Assert.Equal(C_AUTHORED_TEXT, File.ReadAllText(Path.Combine(m_root, C_AUTHORED)));
        Assert.Single(
            Directory.GetFiles(Path.Combine(m_root, "content/en/contributing/testing/projects")),
            path => path.EndsWith("inno-probe-tests.md", StringComparison.Ordinal)
        );
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
        string cache = Path.Combine(m_root, "artifacts/project-query");
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, m_root, cache);
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        Assert.Contains(
            "[!SYNCED]",
            (await snapshot.ExportCatalogAsync()).pages.Single(page => page.id == "project.inno-probe-tests").markdown
        );
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RemovedProjectsDeleteSynchronizedGuidesAndPreserveAuthoredGuides(bool manual)
    {
        const string C_PAGE = "content/en/architecture/projects/inno-retired.md";
        WriteGuideRegistry();
        SetProjects(("Inno.Probe", "src/Inno.Probe.csproj"), ("Inno.Retired", "tools/Inno.Retired/Inno.Retired.csproj"));
        await SyncAsync();
        string path = Path.Combine(m_root, C_PAGE);
        if (manual)
        {
            File.WriteAllText(
                path,
                File.ReadAllText(path).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal) + "\nRetained guidance.\n"
            );
        }
        SetProjects(("Inno.Probe", "src/Inno.Probe.csproj"));
        await SyncAsync();
        Assert.Equal(manual, File.Exists(path));
        Assert.Equal(manual, GuideRegistry().Contains(C_PAGE));
        if (manual) Assert.Contains("Retained guidance.", File.ReadAllText(path));
        Assert.Contains("content/en/architecture/projects/inno-probe.md", GuideRegistry());
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-check", "--docs-root", m_root]));
    }

    [Fact]
    public async Task NewProjectGuidesCannotOverwriteUnregisteredFiles()
    {
        const string C_PAGE = "content/en/architecture/projects/inno-probe.md";
        WriteGuideRegistry();
        Write(C_PAGE, "Preserve this unregistered page.");
        await Assert.ThrowsAsync<InvalidDataException>(() => SyncAsync(CancellationToken.None));
        Assert.Equal("Preserve this unregistered page.", File.ReadAllText(Path.Combine(m_root, C_PAGE)));
        Assert.Empty(GuideRegistry());
    }

    [Fact]
    public async Task CheckoutsWithoutAGuideRegistryDoNotManageProjectGuides()
    {
        await SyncAsync();
        Assert.False(File.Exists(Path.Combine(m_root, "reference/projects/content.json")));
        Assert.False(Directory.Exists(Path.Combine(m_root, "content/en/architecture/projects")));
    }

    private void WriteGuideRegistry()
        => Write(
            "reference/projects/content.json",
            JsonSerializer.Serialize(new { schema = "inno-project-guides/1", entries = Array.Empty<object>() })
        );

    private string[] GuideRegistry()
    {
        using JsonDocument registry = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(m_root, "reference/projects/content.json")));
        return registry.RootElement
            .GetProperty("entries")
            .EnumerateArray()
            .Select(entry => entry.GetProperty("path").GetString()!)
            .ToArray();
    }

    private void SetProjects(params (string name, string path)[] projects)
    {
        string inventory = JsonSerializer.Serialize(
            projects.Select(project => new
            {
                project.name,
                project.path,
                surfaces = new[] { "source-clr" },
                isTest = project.path.StartsWith("tests/", StringComparison.Ordinal)
            })
        );
        Write("reference/projects/catalog.json", inventory);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(m_root, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Hash(inventory);
        Write("reference/engine.lock.json", binding.ToJsonString());
    }
}
