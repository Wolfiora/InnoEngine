using System;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Tooling.Documentation;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class DocumentationReferenceTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDocumentationReference", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public async Task ResolvedUnionSupportsIndependentSelectedSourceOwners()
    {
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00");
        foreach (string name in new[] { "First", "Second" })
        {
            Write("src/" + name + "/" + name + ".csproj", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                  <Target Name="RejectRepeatedResolution" BeforeTargets="ResolveReferences" Condition="Exists('no-resolve')">
                    <Error Text="The union already owns its resolved reference snapshot." />
                  </Target>
                </Project>
                """);
            Write("src/" + name + "/Probe.cs", "namespace " + name + ";\n/// <summary>Supplies this source owner's public contract.</summary>\npublic sealed class Probe { }\n");
        }
        Write("projects.slnx", "<Solution><Project Path=\"src/First/First.csproj\" /><Project Path=\"src/Second/Second.csproj\" /></Solution>");
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        var start = new ProcessStartInfo(host) { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "restore", Path.Combine(m_root, "projects.slnx"), "--nologo", "-v:quiet", "-m:1", "-nr:false", "--disable-parallel" })
            start.ArgumentList.Add(argument);
        using (Process process = Process.Start(start)!)
        {
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
        }
        string[] projects = ["src/First/First.csproj", "src/Second/Second.csproj"];
        string facts = Path.Combine(m_root, "facts.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(["--engine-root", m_root, "--dotnet", host, "--output", facts,
            ..projects.SelectMany(static project => new[] { "--project", project })]));
        string graph = await ManagedProjectGraphCapture.CaptureAsync(m_root, host, projects.Select(project => Path.Combine(m_root, project)).ToArray(),
            new Dictionary<string, string> { ["Configuration"] = "Debug" }, resolveReferences: true);
        string graphPath = Path.Combine(m_root, "resolved.json");
        await File.WriteAllTextAsync(graphPath, graph);
        foreach (string name in new[] { "First", "Second" }) Write("src/" + name + "/no-resolve", "frozen");
        foreach (string project in projects)
        {
            string output = Path.Combine(m_root, Path.GetFileNameWithoutExtension(project) + "-comparison.json");
            Assert.Equal(0, await DocumentationReferences.ValidateApiAsync(["--engine-root", m_root, "--dotnet", host,
                "--reference", facts, "--project", project, "--sdk-snapshot", graphPath, "--output", output]));
            using JsonDocument report = JsonDocument.Parse(File.ReadAllBytes(output));
            Assert.Equal(project, Assert.Single(report.RootElement.GetProperty("projects").EnumerateArray()).GetProperty("project").GetString());
        }
    }

    [Fact]
    public async Task ApiExtractionNeedsNoWikiAndDoesNotBuildTheSelectedProduct()
    {
        const string C_PROJECT = "src/Inno.Probe/Inno.Probe.csproj";
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00");
        Write(C_PROJECT, """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup><TargetFramework>net10.0</TargetFramework><Nullable>enable</Nullable></PropertyGroup>
              <Target Name="RejectProductBuild" BeforeTargets="Build"><Error Text="Product build must never run during API extraction." /></Target>
              <Target Name="RejectRepeatedReferenceCapture" BeforeTargets="ResolveReferences" Condition="Exists('no-resolve')">
                <Error Text="A prepared reference graph must not invoke SDK reference resolution again." />
              </Target>
            </Project>
            """);
        Write("src/Inno.Probe/Probe.cs", """
            namespace Inno.Probe;
            /// <summary>Reads borrowed text without owning the caller's data.</summary>
            public class Probe
            {
                /// <summary>Reads the supplied optional text.</summary>
                /// <param name="text">Borrowed text.</param>
                /// <returns>The original text.</returns>
                public string? Read(string? text = null) => text;
                /// <summary>Reads an integer.</summary>
                /// <param name="value">An integer value.</param>
                /// <returns>The unchanged value.</returns>
                public int Read(int value) => value;
                /// <summary>Runs at the extension boundary.</summary>
                protected virtual void OnRead() { }
            }
            """);
        string host = Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));
        var start = new ProcessStartInfo(host)
        {
            UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true
        };
        foreach (string argument in new[] { "restore", Path.Combine(m_root, C_PROJECT), "--nologo", "-v:quiet" })
            start.ArgumentList.Add(argument);
        using (Process process = Process.Start(start)!)
        {
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
        }
        string output = Path.Combine(m_root, "reference.json");
        string[] arguments = ["--engine-root", m_root, "--dotnet", host, "--project", C_PROJECT, "--output", output];
        Assert.Equal(0, DocumentationReferences.ExportApi(arguments));
        Assert.False(Directory.Exists(Path.Combine(m_root, "docs")));
        using JsonDocument inventory = JsonDocument.Parse(File.ReadAllBytes(output));
        Assert.Equal("source-clr", inventory.RootElement.GetProperty("model").GetString());
        Assert.Equal("Unverified", inventory.RootElement.GetProperty("sourceApiStatus").GetString());
        JsonElement[] symbols = inventory.RootElement.GetProperty("projects")[0].GetProperty("symbols").EnumerateArray().ToArray();
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "M:Inno.Probe.Probe.Read(System.String)"
            && symbol.GetProperty("signature").GetString()!.Contains("string? text = null", StringComparison.Ordinal)
            && symbol.GetProperty("documentation").GetString()!.Contains("Borrowed text", StringComparison.Ordinal));
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "M:Inno.Probe.Probe.Read(System.Int32)");
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "M:Inno.Probe.Probe.OnRead");
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "T:Inno.Probe.Probe"
            && symbol.GetProperty("baseType").GetString() == "object"
            && symbol.GetProperty("kind").GetString() == "NamedType");
        Assert.Contains(symbols, symbol => symbol.GetProperty("id").GetString() == "M:Inno.Probe.Probe.OnRead"
            && symbol.GetProperty("containingType").GetString() == "T:Inno.Probe.Probe");
        string comparison = Path.Combine(m_root, "comparison.json");
        string[] comparisonArguments = ["--engine-root", m_root, "--dotnet", host, "--reference", output, "--output", comparison];
        Assert.Equal(0, await DocumentationReferences.ValidateApiAsync(comparisonArguments));
        using (JsonDocument matched = JsonDocument.Parse(File.ReadAllBytes(comparison)))
        {
            Assert.Equal("Passed", matched.RootElement.GetProperty("status").GetString());
            Assert.Equal("Unverified", matched.RootElement.GetProperty("sourceApiStatus").GetString());
        }
        string graphPath = Path.Combine(m_root, "resolved-graph.json");
        string graph = await ManagedProjectGraphCapture.CaptureAsync(m_root, host, [Path.Combine(m_root, C_PROJECT)],
            new Dictionary<string, string> { ["Configuration"] = "Debug" }, resolveReferences: true);
        await File.WriteAllTextAsync(graphPath, graph);
        Write("src/Inno.Probe/no-resolve", "Reference resolution has already completed for this execution boundary.");
        Assert.Equal(0, await DocumentationReferences.ValidateApiAsync([..comparisonArguments, "--sdk-snapshot", graphPath]));
        JsonNode wrongContext = JsonNode.Parse(graph)!;
        wrongContext[0]!["GlobalProperties"]!["Configuration"] = "Release";
        await File.WriteAllTextAsync(graphPath, wrongContext.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationReferences.ValidateApiAsync([..comparisonArguments, "--sdk-snapshot", graphPath]));
        JsonNode missingOwner = JsonNode.Parse(graph)!;
        missingOwner[0]!["Path"] = Path.Combine(m_root, "src/Inno.Other/Inno.Other.csproj");
        await File.WriteAllTextAsync(graphPath, missingOwner.ToJsonString());
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationReferences.ValidateApiAsync([..comparisonArguments, "--sdk-snapshot", graphPath]));
        await File.WriteAllTextAsync(graphPath, "[]");
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationReferences.ValidateApiAsync([..comparisonArguments, "--sdk-snapshot", graphPath]));
        File.Delete(Path.Combine(m_root, "src/Inno.Probe/no-resolve"));
        const string C_UNRELATED = "src/Inno.Unrelated/Inno.Unrelated.csproj";
        Write(C_UNRELATED, "<Project Sdk=\"Missing.Unrelated.Sdk\" />");
        JsonNode combined = JsonNode.Parse(File.ReadAllBytes(output))!;
        combined["projects"]!.AsArray().Add(JsonSerializer.SerializeToNode(new
        {
            project = C_UNRELATED, name = "Inno.Unrelated", symbols = Array.Empty<object>()
        }));
        byte[] frozen = JsonSerializer.SerializeToUtf8Bytes(combined);
        string cache = Path.Combine(m_root, "artifacts", "documentation");
        string documentation = PrepareDocumentation(frozen, C_PROJECT, C_UNRELATED);
        DocumentationResult indexed = await DocumentationStore.IndexAsync(m_root, documentation, cache);
        Directory.Move(documentation, documentation + "-unavailable");
        string[] preparedArguments = ["--engine-root", m_root, "--dotnet", host, "--cache-root", cache,
            "--snapshot", indexed.snapshot!, "--reference", "reference/api/probe.json", "--project", C_PROJECT,
            "--output", comparison];
        Assert.Equal(0, await DocumentationReferences.ValidateApiAsync(preparedArguments));
        using (JsonDocument matched = JsonDocument.Parse(File.ReadAllBytes(comparison)))
        {
            Assert.Equal(indexed.snapshot, matched.RootElement.GetProperty("documentationSnapshot").GetString());
            Assert.Equal(new string('b', 40), matched.RootElement.GetProperty("documentationSourceTree").GetString());
            JsonElement owner = Assert.Single(matched.RootElement.GetProperty("projects").EnumerateArray());
            Assert.Equal(C_PROJECT, owner.GetProperty("project").GetString());
            Assert.Equal(64, owner.GetProperty("referenceIdentity").GetString()!.Length);
        }
        byte[] protectedResult = File.ReadAllBytes(comparison);
        string[] unknownOwner = preparedArguments.Select(argument => argument == C_PROJECT ? "src/Missing.csproj" : argument).ToArray();
        await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationReferences.ValidateApiAsync(unknownOwner));
        Assert.Equal(protectedResult, File.ReadAllBytes(comparison));
        string[] overwriteCache = preparedArguments.Select(argument => argument == comparison ? Path.Combine(cache, "current.json") : argument).ToArray();
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationReferences.ValidateApiAsync(overwriteCache));
        string source = File.ReadAllText(Path.Combine(m_root, "src/Inno.Probe/Probe.cs"));
        Write("src/Inno.Probe/Probe.cs", source.Replace("Borrowed text.", "Caller must release the text lease.", StringComparison.Ordinal));
        Assert.Equal(1, await DocumentationReferences.ValidateApiAsync(comparisonArguments));
        Assert.Contains("Declaration, XML, constant, source or type relationships changed", File.ReadAllText(comparison));
        Write("src/Inno.Probe/Probe.cs", "\n\n" + source);
        Assert.Equal(0, await DocumentationReferences.ValidateApiAsync(comparisonArguments));
        // Omitting a current overload is a real coverage failure, even when every retained signature matches.
        byte[] fullReference = File.ReadAllBytes(output);
        JsonNode omitted = JsonNode.Parse(fullReference)!;
        omitted["projects"]![0]!["symbols"]!.AsArray().RemoveAt(1);
        File.WriteAllText(output, omitted.ToJsonString());
        Assert.Equal(1, await DocumentationReferences.ValidateApiAsync(comparisonArguments));
        Assert.Contains("Missing current declaration", File.ReadAllText(comparison));
        File.WriteAllBytes(output, fullReference);
        byte[] firstFailure = File.ReadAllBytes(comparison);
        await Assert.ThrowsAsync<OperationCanceledException>(() => DocumentationReferences.ValidateApiAsync(comparisonArguments, new CancellationToken(true)));
        Assert.Equal(firstFailure, File.ReadAllBytes(comparison));
        byte[] previous = File.ReadAllBytes(output);
        Write("src/Inno.Probe/Probe.cs", "namespace Inno.Probe; public class Probe { public MissingContract Read() => null; }");
        Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(arguments));
        Assert.Equal(previous, File.ReadAllBytes(output));
    }

    [Theory]
    [InlineData("[]")]
    [InlineData("[{\"project\":\"../escape.csproj\",\"name\":\"Escape\",\"symbols\":[]}]")]
    [InlineData("[{\"project\":\"src/./Probe.csproj\",\"name\":\"Probe\",\"symbols\":[]}]")]
    [InlineData("[{\"project\":\"src/Probe.csproj\",\"name\":\"Probe\",\"symbols\":[]},{\"project\":\"src/Probe.csproj\",\"name\":\"Probe\",\"symbols\":[]}]")]
    [InlineData("[{\"project\":\"src/Probe.csproj\",\"name\":\"Probe\",\"symbols\":[{\"id\":\"T:Probe\"},{\"id\":\"T:Probe\"}]}]")]
    public async Task InvalidReferenceDiscoveryCannotReplaceEvidence(string projects)
    {
        Write("reference.json", "{\"model\":\"source-clr\",\"configuration\":\"Debug\",\"projects\":" + projects + "}");
        Write("report.json", "first failure");
        Exception? failure = await Record.ExceptionAsync(() => DocumentationReferences.ValidateApiAsync([
            "--engine-root", m_root, "--dotnet", "not-invoked", "--reference", Path.Combine(m_root, "reference.json"),
            "--output", Path.Combine(m_root, "report.json")]));
        Assert.NotNull(failure);
        Assert.True(failure is InvalidDataException or IOException, failure.ToString());
        Assert.Equal("first failure", File.ReadAllText(Path.Combine(m_root, "report.json")));
    }

    [Theory]
    [InlineData("Release", null)]
    [InlineData("Debug", "windows-x64")]
    public async Task DifferentEvaluatedContextsCannotShareOneBatch(
        string configuration,
        string? target
    ) {
        const string C_PROJECTS = "[{\"project\":\"src/Probe.csproj\",\"name\":\"Probe\",\"symbols\":[]}]";
        Write("first.json", "{\"model\":\"source-clr\",\"configuration\":\"Debug\",\"projects\":" + C_PROJECTS + "}");
        Write("second.json", "{\"model\":\"source-clr\",\"configuration\":" + JsonSerializer.Serialize(configuration)
            + ",\"target\":" + JsonSerializer.Serialize(target) + ",\"projects\":" + C_PROJECTS + "}");
        Write("report.json", "original result");
        InvalidDataException failure = await Assert.ThrowsAsync<InvalidDataException>(() => DocumentationReferences.ValidateApiAsync([
            "--engine-root", m_root, "--dotnet", "not-invoked", "--reference", Path.Combine(m_root, "first.json"),
            "--reference", Path.Combine(m_root, "second.json"), "--output", Path.Combine(m_root, "report.json")]));
        Assert.Contains("separate evaluated batches", failure.Message);
        Assert.Equal("original result", File.ReadAllText(Path.Combine(m_root, "report.json")));
    }

    [Theory]
    [InlineData("--cache-root")]
    [InlineData("--snapshot")]
    public async Task IncompleteSnapshotSelectionDoesNotOpenLocalFiles(string option)
    {
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationReferences.ValidateApiAsync([
            "--engine-root", m_root, "--dotnet", "not-invoked", "--reference", "absent.json", "--output", "report.json",
            option, "missing"]));
    }

    [Fact]
    public async Task RetiringSnapshotsPreservesReferencePayloadsForActiveReaders()
    {
        const string C_SELECTED = "src/Inno.Probe/Inno.Probe.csproj";
        const string C_UNRELATED = "src/Inno.Unrelated/Inno.Unrelated.csproj";
        byte[] facts = JsonSerializer.SerializeToUtf8Bytes(new
        {
            model = "source-clr", configuration = "Debug", projects = new[]
            {
                new { project = C_SELECTED, name = "Inno.Probe", symbols = Array.Empty<object>() }
            }
        });
        string documentation = PrepareDocumentation(facts, C_SELECTED, C_UNRELATED);
        string cache = Path.Combine(m_root, "artifacts", "documentation");
        DocumentationResult first = await DocumentationStore.IndexAsync(m_root, documentation, cache);
        string payload = Path.Combine(cache, "references", Convert.ToHexStringLower(SHA256.HashData(facts)));
        using (DocumentationSnapshot reader = await DocumentationStore.OpenAsync(cache, first.snapshot))
        {
            JsonNode changed = JsonNode.Parse(facts)!;
            changed["sourceApiStatus"] = "Unverified";
            PrepareDocumentation(JsonSerializer.SerializeToUtf8Bytes(changed), C_SELECTED, C_UNRELATED);
            DocumentationResult second = await DocumentationStore.IndexAsync(m_root, documentation, cache);
            Assert.NotEqual(first.snapshot, second.snapshot);
            BuildCacheCleanupPlan retirement = BuildCacheCleanupPlan.Create(m_root,
                BuildCacheOwnerCatalog.ReadOwners(m_root), new(0, 0), retireAll: true);
            BuildCacheCleanupResult result = await retirement.ExecuteAsync();
            Assert.Empty(result.failures);
            Assert.DoesNotContain(payload, result.retired);
            Assert.Equal(facts, await reader.ReadReferenceAsync("reference/api/probe.json"));
        }
        BuildCacheCleanupResult released = await BuildCacheCleanupPlan.Create(m_root,
            BuildCacheOwnerCatalog.ReadOwners(m_root), new(0, 0), retireAll: true).ExecuteAsync();
        Assert.Empty(released.failures);
        Assert.Contains(payload, released.retired);
        using DocumentationSnapshot current = await DocumentationStore.OpenAsync(cache);
        Assert.NotEqual(facts, await current.ReadReferenceAsync("reference/api/probe.json"));
    }

    [Fact]
    public void ExportUsesTheActualSourceInventoryWithoutInferringApiSurfaces()
    {
        Write("src/Inno.Probe/Inno.Probe.csproj", "<Project Sdk=\"Microsoft.NET.Sdk\" />");
        Write("tests/Inno.Probe.Tests/Inno.Probe.Tests.csproj", "<Project><PropertyGroup><IsTestProject>true</IsTestProject></PropertyGroup></Project>");
        Write("src/Inno.Probe/obj/Ignored.csproj", "<Project />");
        Write("extern/ThirdParty/ThirdParty.csproj", "<Project />");
        string output = Path.Combine(m_root, "output", "projects.json");
        Assert.Equal(0, DocumentationReferences.ExportProjects(["--engine-root", m_root, "--output", output]));
        using JsonDocument inventory = JsonDocument.Parse(File.ReadAllBytes(output));
        Assert.Equal(2, inventory.RootElement.GetArrayLength());
        Assert.Equal("Inno.Probe", inventory.RootElement[0].GetProperty("name").GetString());
        Assert.Empty(inventory.RootElement[0].GetProperty("surfaces").EnumerateArray());
        Assert.True(inventory.RootElement[1].GetProperty("isTest").GetBoolean());
        byte[] original = File.ReadAllBytes(output);
        Assert.DoesNotContain((byte)'\r', original);
        DocumentationReferences.ExportProjects(["--engine-root", m_root, "--output", output]);
        Assert.Equal(original, File.ReadAllBytes(output));
    }

    [Fact]
    public void DuplicateProjectIdentityCannotReplaceExistingFacts()
    {
        Write("src/one/Inno.Probe.csproj", "<Project />");
        Write("tools/two/Inno.Probe.csproj", "<Project />");
        string output = Path.Combine(m_root, "projects.json");
        File.WriteAllText(output, "previous evidence");
        Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportProjects(["--engine-root", m_root, "--output", output]));
        Assert.Equal("previous evidence", File.ReadAllText(output));
    }

    [Fact]
    public void EmptyDiscoveryCannotBecomeAValidManifest()
    {
        Directory.CreateDirectory(m_root);
        string output = Path.Combine(m_root, "projects.json");
        Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportProjects(["--engine-root", m_root, "--output", output]));
        Assert.False(File.Exists(output));
    }

    private string PrepareDocumentation(
        byte[] facts,
        string selected,
        string unrelated
    ) {
        byte[] projects = JsonSerializer.SerializeToUtf8Bytes(new[]
        {
            new { name = "Inno.Probe", path = selected, entry = "probe", surfaces = new[] { "source-clr" } },
            new { name = "Inno.Unrelated", path = unrelated, entry = "unrelated", surfaces = new[] { "source-clr" } }
        });
        string revision = new('a', 40);
        string tree = new('b', 40);
        Write("documentation/reference/projects/catalog.json", Encoding.UTF8.GetString(projects));
        Write("documentation/reference/api/probe.json", Encoding.UTF8.GetString(facts));
        Write("documentation/reference/engine.lock.json", JsonSerializer.Serialize(new
        {
            repository = "https://github.com/Wolfiora/InnoEngine", revision, sourceTree = tree, toolingRevision = revision,
            projectManifest = "reference/projects/catalog.json", projectManifestIdentity = Convert.ToHexStringLower(SHA256.HashData(projects)),
            apiManifests = new[] { new
            {
                path = "reference/api/probe.json", sha256 = Convert.ToHexStringLower(SHA256.HashData(facts)), model = "source-clr",
                configuration = "Debug", sourceRevision = revision, sourceTree = tree
            } }
        }));
        Write("documentation/content/en/probe.md", "---\nuid: probe\nkind: project\nowners: [Inno.Probe]\n---\n# Probe\n\nUse the declared source contract.\n");
        return Path.Combine(m_root, "documentation");
    }

    private void Write(
        string path,
        string text
    ) {
        string target = Path.Combine(m_root, path);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        File.WriteAllText(target, text);
    }
}
