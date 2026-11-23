using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Tooling.Documentation.Integration.Tests;

[Collection(nameof(DocumentationSnapshotFixture))]
public sealed class DocumentationCatalogTests : DocumentationSnapshotFixture
{
    [Fact]
    public async Task PlatformInventoryDoesNotRequireOrExposeImplementationGuides()
    {
        JsonNode projects = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/projects/catalog.json")))!;
        JsonNode platform = projects[0]!.DeepClone();
        platform["name"] = "Inno.Platform.Probe";
        platform["path"] = "platforms/Probe/Inno.Platform.Probe.csproj";
        platform["entry"] = null;
        projects.AsArray().Add(platform);
        string text = projects.ToJsonString();
        Write("reference/projects/catalog.json", text);
        JsonNode binding = JsonNode.Parse(File.ReadAllText(Path.Combine(source, "reference/engine.lock.json")))!;
        binding["projectManifestIdentity"] = Hash(Encoding.UTF8.GetBytes(text));
        Write("reference/engine.lock.json", binding.ToJsonString());
        DocumentationResult indexed = await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, indexed.snapshot);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        Assert.Contains("Inno.Probe", catalog.projects.Keys);
        Assert.DoesNotContain("Inno.Platform.Probe", catalog.projects.Keys);
        Write(
            "content/en/platform.md",
            Page(
                "platform",
                "Platform implementation",
                "Platform-specific internals.",
                "kind: project\nowners: [Inno.Platform.Probe]\n"
            )
        );
        await Assert.ThrowsAsync<InvalidDataException>(IndexAsync);
    }

    [Fact]
    public async Task CatalogUsesThePreparedSnapshotAndReportsMissingProjectCoverage()
    {
        await IndexAsync();
        using DocumentationSnapshot initial = await DocumentationStore.OpenAsync(cache);
        DocumentationCatalogReport first = await initial.ExportCatalogAsync();
        Assert.Equal("probe", first.projects["Inno.Probe"]);
        DocumentationPageInfo page = Assert.Single(first.pages, item => item.id == "probe");
        Assert.Contains("ownership", page.sections.Keys);
        Assert.Equal("Unverified", first.sourceApiStatus);
        DocumentationSymbolInfo symbol = Assert.Single(first.pages, item => item.id == "probe-api").symbols[C_SYMBOL];
        Assert.Equal(new string('a', 64), symbol.signatureIdentity);
        Assert.Contains("Read(string key)", symbol.code);
        Directory.Move(source, Path.Combine(m_root, "unavailable-source"));
        DocumentationCatalogReport repeated = await initial.ExportCatalogAsync();
        Assert.Equal(first.contentIdentity, repeated.contentIdentity);
        Assert.Equal(first.pages.Select(item => item.contentIdentity), repeated.pages.Select(item => item.contentIdentity));
        using DocumentationSnapshot mismatch = await DocumentationStore.OpenAsync(cache, expectedSourceTree: new string('c', 40));
        await Assert.ThrowsAsync<InvalidOperationException>(() => mismatch.ExportCatalogAsync());
    }

    [Fact]
    public async Task CatalogPreservesSectionDeclarationsWithoutTreatingExamplesAsCertification()
    {
        const string C_DECLARATION = "public string? Read(string key = \"default\");";
        Write(
            "content/en/api.md",
            Page(
                "probe-api",
                "Probe API",
                "## Read\n\n<!-- api: " + C_SYMBOL + " " + Hash(Encoding.UTF8.GetBytes(C_DECLARATION))
                + " -->\n\n```csharp\n" + C_DECLARATION + "\n```\n\nThe caller retains ownership.\n"
                + "\n## Other task\n\n```csharp\nUnrelatedExample();\n```\n",
                "kind: api\nowners: [Inno.Probe]\napiSurface: host-runtime\ncontracts: [probe#ownership]\n"
            )
        );
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        DocumentationPageInfo page = Assert.Single(catalog.pages, page => page.id == "probe-api");
        DocumentationSymbolInfo symbol = page.symbols[C_SYMBOL];
        Assert.Equal("public string? Read(string key = \"default\");", Assert.Single(symbol.code));
        Assert.Equal(2, page.codeBlocks.Count);
        Assert.Equal(C_SYMBOL, Assert.Single(page.codeBlocks[0].declarationSymbols));
        Assert.Equal(C_DECLARATION, page.codeBlocks[0].text);
        Assert.Empty(page.codeBlocks[1].declarationSymbols);
        Assert.Equal("UnrelatedExample();", page.codeBlocks[1].text);
        Assert.Equal("other-task", page.codeBlocks[1].section);
        Assert.Equal("csharp", page.codeBlocks[1].language);
        Assert.True(page.codeBlocks[1].line > page.codeBlocks[0].line);
        Assert.Equal("Unverified", catalog.sourceApiStatus);
        DocumentationResult result = await snapshot.QueryAsync(new DocumentationRequest("symbol", C_SYMBOL, "Inno.Probe", "host-runtime"));
        Assert.Contains("caller retains ownership", Assert.Single(result.hits).text);
        Assert.DoesNotContain("UnrelatedExample", Assert.Single(result.hits).text);
        Assert.Single(result.continuations);
    }

    [Fact]
    public async Task CatalogRetainsExplicitScriptContextWithoutPreparingItsReferences()
    {
        Write(
            "content/en/example.md",
            Page(
                "example",
                "Example",
                "```csharp\npublic class Example { }\n```\n",
                "kind: tutorial\nexampleMode: editor-script\nexampleProjects: [Inno.Probe]\n"
            )
        );
        await IndexAsync();
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache);
        DocumentationPageInfo page = Assert.Single((await snapshot.ExportCatalogAsync()).pages, page => page.id == "example");
        Assert.Equal("editor-script", page.examples!.mode);
        Assert.Equal("Inno.Probe", Assert.Single(page.examples.projects));
        Assert.Single(page.codeBlocks);
    }
}
