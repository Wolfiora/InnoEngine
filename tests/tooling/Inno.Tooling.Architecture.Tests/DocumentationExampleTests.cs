using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Inno.Tooling.Documentation;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class DocumentationExampleTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoDocumentationExamples", Guid.NewGuid().ToString("N"));
    private string cache => Path.Combine(m_root, "cache");
    private string source => Path.Combine(m_root, "documentation");
    private string host => Path.GetFullPath(Path.Combine(RuntimeEnvironment.GetRuntimeDirectory(), "..", "..", "..",
        OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet"));

    public DocumentationExampleTests()
    {
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00");
        Write("src/Inno.Probe/Inno.Probe.csproj", """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup><TargetFramework>net10.0</TargetFramework><Nullable>enable</Nullable></PropertyGroup>
              <Target Name="RejectRepeatedBuild" BeforeTargets="Build" Condition="Exists('no-build')">
                <Error Text="Example validation must not build its product." />
              </Target>
              <Target Name="RejectRepeatedResolution" BeforeTargets="ResolveReferences" Condition="Exists('no-resolve')">
                <Error Text="A prepared graph must not resolve its example references again." />
              </Target>
            </Project>
            """);
        Write("src/Inno.Probe/Probe.cs", "namespace Inno.Probe; public static class Probe { public static int Read() => 42; }");
        byte[] projects = JsonSerializer.SerializeToUtf8Bytes(new[]
        {
            new { name = "Inno.Probe", path = "src/Inno.Probe/Inno.Probe.csproj", surfaces = Array.Empty<string>() }
        });
        Write("documentation/reference/projects.json", Encoding.UTF8.GetString(projects));
        Write("documentation/reference/engine.lock.json", JsonSerializer.Serialize(new
        {
            repository = "https://github.com/Wolfiora/InnoEngine", revision = new string('a', 40), sourceTree = new string('b', 40),
            projectManifest = "reference/projects.json", projectManifestIdentity = Convert.ToHexStringLower(SHA256.HashData(projects)),
            toolingRevision = new string('a', 40)
        }));
    }

    [Fact]
    public async Task ExplicitReferencesCompileIndependentGroupsAndMissingScriptContextStaysBlocked()
    {
        var start = new ProcessStartInfo(host)
        {
            UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true
        };
        foreach (string argument in new[] { "build", Path.Combine(m_root, "src/Inno.Probe/Inno.Probe.csproj"), "-v:quiet" })
            start.ArgumentList.Add(argument);
        using (Process process = Process.Start(start)!)
        {
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
        }
        Write("src/Inno.Probe/no-build", "guard");
        Page("working", "host", "public class Example { public int Read() => Inno.Probe.Probe.Read(); }");
        Page("broken", "host", "public class Example { public int Read() => Inno.Probe.Probe.Missing(); }");
        Page("script", "editor-script", "public class ScriptExample { }");
        Page("unspecified", null, "public class Unspecified { }");
        DocumentationResult index = await DocumentationStore.IndexAsync(m_root, source, cache);
        string output = Path.Combine(m_root, "results.json");
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync(Arguments(index.snapshot, output)));
        using JsonDocument report = JsonDocument.Parse(File.ReadAllBytes(output));
        var results = report.RootElement.GetProperty("results").EnumerateArray()
            .ToDictionary(static group => group.GetProperty("page").GetString()!);
        Assert.Equal("Passed", results["working"].GetProperty("status").GetString());
        Assert.NotEmpty(results["working"].GetProperty("references").EnumerateObject());
        Assert.Equal("Failed", results["broken"].GetProperty("status").GetString());
        Assert.Equal("Blocked", results["script"].GetProperty("status").GetString());
        Assert.Contains("Logical script examples", results["script"].GetProperty("errors")[0].GetString());
        Assert.Equal("Blocked", results["unspecified"].GetProperty("status").GetString());
        Assert.Equal("Unverified", report.RootElement.GetProperty("sourceApiStatus").GetString());
        string graphPath = Path.Combine(m_root, "resolved-graph.json");
        string graph = await ManagedProjectGraphCapture.CaptureAsync(m_root, host, [Path.Combine(m_root, "src/Inno.Probe/Inno.Probe.csproj")],
            new Dictionary<string, string> { ["Configuration"] = "Debug" }, resolveReferences: true);
        await File.WriteAllTextAsync(graphPath, graph);
        Write("src/Inno.Probe/no-resolve", "References are frozen for this execution boundary.");
        Assert.Equal(0, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "working", "--sdk-snapshot", graphPath]));
    }

    [Fact]
    public async Task EmptyDiscoveryCancellationAndUnknownPagesCannotProduceGreenEvidence()
    {
        Page("empty", null, null);
        DocumentationResult index = await DocumentationStore.IndexAsync(m_root, source, cache);
        string output = Path.Combine(m_root, "results.json");
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync(Arguments(index.snapshot, output)));
        using JsonDocument report = JsonDocument.Parse(File.ReadAllBytes(output));
        Assert.Equal("Blocked", report.RootElement.GetProperty("status").GetString());
        byte[] previous = File.ReadAllBytes(output);
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationExamples.ExecuteAsync(
            [..Arguments(index.snapshot, output), "--page", "missing"]));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => DocumentationExamples.ExecuteAsync(
            Arguments(index.snapshot, output), cancellation.Token));
        Assert.Equal(previous, File.ReadAllBytes(output));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public async Task HostExamplesRejectUnreferencedAssembliesAndRespectMarkdownFences()
    {
        string directory = Path.Combine(m_root, "src/Inno.Probe/bin/Debug/net10.0");
        Directory.CreateDirectory(directory);
        File.Copy(EmitReference("Inno.Probe", "namespace Inno.Probe; public static class Probe { public static int Read() => 42; }"), Path.Combine(directory, "Inno.Probe.dll"));
        File.WriteAllText(Path.Combine(directory, "Inno.Probe.xml"), "<p></th>");
        File.Copy(EmitReference("Foreign", "public static class Foreign { public static int Read() => 7; }"), Path.Combine(directory, "Foreign.dll"));
        var start = new ProcessStartInfo(host) { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "restore", Path.Combine(m_root, "src/Inno.Probe/Inno.Probe.csproj"), "--disable-parallel", "-m:1", "-nr:false", "--disable-build-servers" }) start.ArgumentList.Add(argument);
        using (Process process = Process.Start(start)!)
        {
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
        }
        Page("valid", "host", "public static class Example { public static int Read() => Inno.Probe.Probe.Read(); public static int Count(params System.ReadOnlySpan<int> values) => values.Length; }");
        Page("foreign", "host", "public static class Example { public static int Read() => Foreign.Read(); }");
        Page("tilde", "host", "int value = missing;");
        Page("long-fence", "host", "int value = missing;");
        foreach (string id in new[] { "tilde", "long-fence" })
        {
            string path = Path.Combine(source, "content/en/" + id + ".md");
            File.WriteAllText(path, File.ReadAllText(path).Replace("```", id == "tilde" ? "~~~" : "````", StringComparison.Ordinal));
        }
        Page("text", "host", "int value = 1;");
        File.AppendAllText(Path.Combine(source, "content/en/text.md"), "\n````text\n```csharp\nint value = missing;\n```\n````\n");
        DocumentationResult index = await DocumentationStore.IndexAsync(m_root, source, cache);
        string output = Path.Combine(m_root, "fences.json");
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync(Arguments(index.snapshot, output)));
        using JsonDocument report = JsonDocument.Parse(File.ReadAllBytes(output));
        var results = report.RootElement.GetProperty("results").EnumerateArray().ToDictionary(static item => item.GetProperty("page").GetString()!);
        foreach (string id in new[] { "valid", "text" }) Assert.Equal("Passed", results[id].GetProperty("status").GetString());
        foreach (string id in new[] { "foreign", "tilde", "long-fence" }) Assert.Equal("Failed", results[id].GetProperty("status").GetString());
        Assert.Contains("CS0103", results["foreign"].GetProperty("errors")[0].GetString());
    }

    [Fact]
    public async Task LogicalExamplesRetainBothScriptSurfacesAndNeverBorrowHostImplementation()
    {
        var start = new ProcessStartInfo(host) { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "restore", Path.Combine(m_root, "src/Inno.Probe/Inno.Probe.csproj"), "--disable-parallel", "-m:1", "-nr:false", "--disable-build-servers" }) start.ArgumentList.Add(argument);
        using (Process process = Process.Start(start)!)
        {
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
        }
        string runtime = EmitReference("Inno.ScriptApi.Runtime", "namespace InnoEngine.Probe; public static class Value { public static int Read() => 7; }");
        string editor = EmitReference("Inno.ScriptApi.Editor", "namespace InnoEditor.Probe; public static class Action { public static int Read() => 9; }");
        string unrelated = EmitReference("Unrelated", "public class Unrelated { }");
        Page("runtime", "runtime-script", "public class Example { public int Read() => InnoEngine.Probe.Value.Read(); }");
        Page("editor", "editor-script", "public class Example { public int Read() => InnoEngine.Probe.Value.Read() + InnoEditor.Probe.Action.Read(); }");
        Page("host-leak", "runtime-script", "public class Example { public int Read() => Inno.Probe.Probe.Read(); }");
        DocumentationResult index = await DocumentationStore.IndexAsync(m_root, source, cache);
        string output = Path.Combine(m_root, "scripts.json");
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "runtime"]));
        Assert.Contains("Blocked", File.ReadAllText(output));
        int runtimeResult = await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "runtime", "--runtime-reference", runtime]);
        Assert.True(runtimeResult == 0, File.ReadAllText(output));
        await AssertReceiptAsync(index.snapshot, output, "runtime");
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "editor", "--runtime-reference", runtime]));
        Assert.Contains("Blocked", File.ReadAllText(output));
        Assert.Equal(0, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "editor", "--runtime-reference", runtime, "--editor-reference", editor]));
        using (JsonDocument evidence = JsonDocument.Parse(File.ReadAllBytes(output)))
        {
            JsonElement references = evidence.RootElement.GetProperty("results")[0].GetProperty("references");
            Assert.Equal(Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(runtime))), references.GetProperty("Inno.ScriptApi.Runtime").GetString());
            Assert.Equal(Convert.ToHexStringLower(SHA256.HashData(File.ReadAllBytes(editor))), references.GetProperty("Inno.ScriptApi.Editor").GetString());
        }
        await AssertReceiptAsync(index.snapshot, output, "editor");
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "host-leak", "--runtime-reference", runtime]));
        Assert.Contains("CS0103", File.ReadAllText(output));
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "runtime", "--runtime-reference", unrelated]));
        Assert.Contains("Expected the Inno.ScriptApi.Runtime", File.ReadAllText(output));
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "editor", "--editor-reference", editor]));
    }

    [Fact]
    public async Task ScriptReferenceCaptureUsesActualMembersOriginsAndQualifiedSurfaces()
    {
        Write("src/Inno.Probe/Probe.cs", "namespace Inno.Probe; public static class Probe { public static int Read(int value) => value; public static int Read(string value) => value.Length; } public static class EditorProbe { public static int Inspect() => 1; } public sealed class Owned { private Owned() { } }");
        var start = new ProcessStartInfo(host) { UseShellExecute = false, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "restore", Path.Combine(m_root, "src/Inno.Probe/Inno.Probe.csproj"), "--disable-parallel", "-nr:false", "-v:quiet" })
            start.ArgumentList.Add(argument);
        using (Process process = Process.Start(start)!)
        {
            Task<string> stdout = process.StandardOutput.ReadToEndAsync();
            Task<string> stderr = process.StandardError.ReadToEndAsync();
            await process.WaitForExitAsync();
            Assert.True(process.ExitCode == 0, await stdout + await stderr);
        }
        string facts = Path.Combine(m_root, "host.json");
        Assert.Equal(0, DocumentationReferences.ExportApi(["--engine-root", m_root, "--dotnet", host,
            "--project", "src/Inno.Probe/Inno.Probe.csproj", "--output", facts]));
        string runtime = EmitReference("Inno.ScriptApi.Runtime", "namespace Logical; public static class Renamed { public static int Read(int value) => value; public static int Read(string value) => value.Length; private static int Hidden() => 0; } public sealed class Owned { }");
        string editor = EmitReference("Inno.ScriptApi.Editor", "namespace Logical; public static class Tools { public static int Inspect() => 1; }");
        const string C_XML = """
            <doc><assembly><name>ASSEMBLY</name></assembly>
            <provenance format="inno-script-documentation/2"><type id="T:Logical.Renamed" source="T:Inno.Probe.Probe" assembly="Inno.Probe" /></provenance>
            <members><member name="T:Logical.Renamed"><summary>Uses the logical caller contract.</summary></member>
            <member name="M:Logical.Renamed.Read(System.Int32)"><summary>Reads the supplied value.</summary><param name="value">The value.</param><returns>The supplied value.</returns></member>
            <member name="M:Logical.Renamed.HostOnly"><summary>Must not become a projected declaration.</summary></member></members></doc>
            """;
        File.WriteAllText(Path.ChangeExtension(runtime, ".xml"), C_XML.Replace("ASSEMBLY", "Inno.ScriptApi.Runtime", StringComparison.Ordinal)
            .Replace("</provenance>", "<type id=\"T:Logical.Owned\" source=\"T:Inno.Probe.Owned\" assembly=\"Inno.Probe\" /></provenance>", StringComparison.Ordinal));
        File.WriteAllText(Path.ChangeExtension(editor, ".xml"), C_XML.Replace("ASSEMBLY", "Inno.ScriptApi.Editor", StringComparison.Ordinal)
            .Replace("Logical.Renamed", "Logical.Tools", StringComparison.Ordinal).Replace("T:Inno.Probe.Probe", "T:Inno.Probe.EditorProbe", StringComparison.Ordinal));
        string output = Path.Combine(m_root, "projected.json");
        string[] arguments = ["--engine-root", m_root, "--dotnet", host, "--source-reference", facts,
            "--runtime-reference", runtime, "--editor-reference", editor, "--output", output,
            "--framework-project", "src/Inno.Probe/Inno.Probe.csproj"];
        Assert.Equal(0, DocumentationReferences.ExportApi([..arguments, "--surface", "runtime-script"]));
        byte[] runtimeFacts = File.ReadAllBytes(output);
        using (JsonDocument captured = JsonDocument.Parse(File.ReadAllBytes(output)))
        {
            Assert.Equal("runtime-script", captured.RootElement.GetProperty("model").GetString());
            Assert.Single(captured.RootElement.GetProperty("projectionIdentities").EnumerateObject());
            JsonElement project = captured.RootElement.GetProperty("projects")[0];
            Assert.Equal("Inno.Probe", project.GetProperty("name").GetString());
            var symbols = project.GetProperty("symbols").EnumerateArray().ToDictionary(static symbol => symbol.GetProperty("id").GetString()!);
            Assert.Contains("M:Logical.Renamed.Read(System.Int32)", symbols.Keys);
            Assert.Contains("M:Logical.Renamed.Read(System.String)", symbols.Keys);
            Assert.DoesNotContain(symbols.Keys, static id => id.Contains("Hidden", StringComparison.Ordinal) || id.Contains("HostOnly", StringComparison.Ordinal));
            Assert.Contains("supplied value", symbols["M:Logical.Renamed.Read(System.Int32)"].GetProperty("documentation").GetString());
            Assert.True(symbols["M:Logical.Owned.#ctor"].GetProperty("projectionOnly").GetBoolean());
            Assert.Contains("not a supported construction path", symbols["M:Logical.Owned.#ctor"].GetProperty("documentation").GetString());
        }
        Assert.Equal(0, DocumentationReferences.ExportApi([..arguments, "--surface", "editor-script"]));
        using (JsonDocument captured = JsonDocument.Parse(File.ReadAllBytes(output)))
        {
            Assert.Equal(2, captured.RootElement.GetProperty("projectionIdentities").EnumerateObject().Count());
            Assert.Contains(captured.RootElement.GetProperty("projects")[0].GetProperty("symbols").EnumerateArray(),
                static symbol => symbol.GetProperty("id").GetString() == "M:Logical.Tools.Inspect");
        }
        Page("projected-contract", "runtime-script", "public static class Example { public static int Read() => Logical.Renamed.Read(7); }");
        Bind(runtimeFacts);
        DocumentationResult prepared = await DocumentationStore.IndexAsync(m_root, source, cache);
        string comparison = Path.Combine(m_root, "comparison.json");
        Assert.Equal(0, await DocumentationExamples.ExecuteAsync([..Arguments(prepared.snapshot, comparison), "--runtime-reference", runtime]));
        using (JsonDocument report = JsonDocument.Parse(File.ReadAllBytes(comparison)))
        {
            Assert.Equal("Passed", report.RootElement.GetProperty("scriptReferences")[0].GetProperty("status").GetString());
        }
        await AssertReceiptAsync(prepared.snapshot, comparison, "projected-contract");
        JsonNode changedFacts = JsonNode.Parse(runtimeFacts)!;
        changedFacts["projects"]![0]!["symbols"]![0]!["signature"] = "public int ChangedContract";
        Bind(JsonSerializer.SerializeToUtf8Bytes(changedFacts));
        DocumentationResult changed = await DocumentationStore.IndexAsync(m_root, source, cache);
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(changed.snapshot, comparison), "--runtime-reference", runtime]));
        using (JsonDocument report = JsonDocument.Parse(File.ReadAllBytes(comparison)))
        {
            Assert.Equal("Passed", report.RootElement.GetProperty("results")[0].GetProperty("status").GetString());
            Assert.Equal("Failed", report.RootElement.GetProperty("scriptReferences")[0].GetProperty("status").GetString());
        }
        byte[] previous = File.ReadAllBytes(output);
        JsonNode conflicting = JsonNode.Parse(File.ReadAllBytes(facts))!;
        JsonNode selectedMember = conflicting["projects"]![0]!["symbols"]!.AsArray()
            .Single(value => value!["id"]!.GetValue<string>() == "M:Inno.Probe.Probe.Read(System.Int32)")!;
        selectedMember["documentation"] = "<member><summary>A conflicting captured contract.</summary></member>";
        string conflictPath = Path.Combine(m_root, "conflicting-source.json");
        File.WriteAllBytes(conflictPath, JsonSerializer.SerializeToUtf8Bytes(conflicting));
        Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi(
            [..arguments, "--source-reference", conflictPath, "--surface", "runtime-script"]));
        Assert.Equal(previous, File.ReadAllBytes(output));
        File.WriteAllText(Path.ChangeExtension(runtime, ".xml"), C_XML.Replace("T:Inno.Probe.Probe", "T:Missing.Owner", StringComparison.Ordinal));
        Assert.Throws<InvalidDataException>(() => DocumentationReferences.ExportApi([..arguments, "--surface", "runtime-script"]));
        Assert.Equal(previous, File.ReadAllBytes(output));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.ThrowsAny<OperationCanceledException>(() => DocumentationReferences.ExportApi([..arguments, "--surface", "runtime-script"], cancellation.Token));
        Assert.Equal(previous, File.ReadAllBytes(output));

        void Bind(byte[] projectedFacts)
        {
            byte[] inventory = JsonSerializer.SerializeToUtf8Bytes(new[]
            {
                new { name = "Inno.Probe", path = "src/Inno.Probe/Inno.Probe.csproj", surfaces = new[] { "source-clr", "runtime-script" } }
            });
            byte[] hostFacts = File.ReadAllBytes(facts);
            Write("documentation/reference/projects.json", Encoding.UTF8.GetString(inventory));
            Write("documentation/reference/host.json", Encoding.UTF8.GetString(hostFacts));
            Write("documentation/reference/runtime.json", Encoding.UTF8.GetString(projectedFacts));
            Write("documentation/reference/engine.lock.json", JsonSerializer.Serialize(new
            {
                repository = "https://github.com/Wolfiora/InnoEngine", revision = new string('a', 40), sourceTree = new string('b', 40),
                projectManifest = "reference/projects.json", projectManifestIdentity = Convert.ToHexStringLower(SHA256.HashData(inventory)),
                toolingRevision = new string('a', 40), apiManifests = new[]
                {
                    new { path = "reference/host.json", sha256 = Convert.ToHexStringLower(SHA256.HashData(hostFacts)), model = "source-clr", configuration = "Debug", sourceRevision = new string('a', 40), sourceTree = new string('b', 40) },
                    new { path = "reference/runtime.json", sha256 = Convert.ToHexStringLower(SHA256.HashData(projectedFacts)), model = "runtime-script", configuration = "Debug", sourceRevision = new string('a', 40), sourceTree = new string('b', 40) }
                }
            }));
        }
    }

    private async Task AssertReceiptAsync(string snapshotId, string reportPath, string pageId)
    {
        using DocumentationSnapshot snapshot = await DocumentationStore.OpenAsync(cache, snapshotId);
        DocumentationCatalogReport catalog = await snapshot.ExportCatalogAsync();
        Write("receipt/documentation-catalog.json", JsonSerializer.Serialize(catalog));
        File.Copy(reportPath, Path.Combine(m_root, "receipt/managed-documentation-examples.json"), overwrite: true);
        Write("receipt/check.ps1", """
            param([string]$Repository, [string]$Page)
            $ErrorActionPreference = 'Stop'
            $catalog = Get-Content "$PSScriptRoot/documentation-catalog.json" -Raw | ConvertFrom-Json
            $hash = 'a' * 64
            $owner = 'src/Inno.Probe/Inno.Probe.csproj'
            $plan = @{ environment = $hash; checks = @(@{ id = 'documentation-examples:managed'; kind = 'documentation-examples'; context = 'managed'
                exclusion = $null; projects = @($owner); documentation = @{ pages = @($Page); configuration = 'Debug'; target = $null } }) }
            $evidence = @{ results = @(@{ id = 'documentation-examples:managed'; status = 'Passed'; documentationInputs = @(@{
                project = $owner; owners = @($owner); sourceIdentity = $hash; environment = $hash; configuration = 'Debug'; target = $null
                packages = @{}; sources = @{ $owner = (Get-FileHash "$PSScriptRoot/../$owner" -Algorithm SHA256).Hash.ToLowerInvariant() } }) }) }
            $pairing = @{ documentationSnapshot = $catalog.snapshot; contentIdentity = $catalog.contentIdentity; engineTree = $catalog.sourceTree }
            $receipt = @(& "$Repository/tests/ci/Get-DocumentationExampleReceipt.ps1" -Plan $plan -Evidence $evidence -Pairing $pairing -Directory $PSScriptRoot)
            if ($receipt.Count -ne 1 -or $receipt[0].pages.Count -ne 1 -or $receipt[0].pages[0].page -cne $Page) {
                throw 'The real compiler report lost its selected example during CI reconciliation.'
            }
            "Reconciled $Page against the actual example compiler report."
            """);
        var start = new ProcessStartInfo("pwsh") { UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
        foreach (string argument in new[] { "-NoProfile", "-File", Path.Combine(m_root, "receipt/check.ps1"),
                     TestRepository.GetRoot(typeof(DocumentationExampleTests).Assembly), pageId })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start)!;
        Task<string> stdout = process.StandardOutput.ReadToEndAsync();
        Task<string> stderr = process.StandardError.ReadToEndAsync();
        await process.WaitForExitAsync();
        Assert.True(process.ExitCode == 0, await stdout + await stderr);
    }

    private string EmitReference(string name, string sourceText)
    {
        string output = Path.Combine(m_root, name + ".dll");
        string[] references = File.ReadAllLines(Path.Combine(AppContext.BaseDirectory, "DocumentationFrameworkReferences.txt"));
        Assert.NotEmpty(references);
        var compilation = CSharpCompilation.Create(name, [CSharpSyntaxTree.ParseText(sourceText)],
            references.Select(static path => MetadataReference.CreateFromFile(path)),
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        Assert.True(compilation.Emit(output).Success);
        return output;
    }

    private string[] Arguments(
        string snapshot,
        string output
    ) => ["--engine-root", m_root, "--cache-root", cache, "--snapshot", snapshot, "--dotnet", host, "--output", output];

    private void Page(
        string id,
        string? mode,
        string? code
    ) => Write("documentation/content/en/" + id + ".md", "---\nuid: " + id + "\nkind: tutorial\n"
        + (mode is null ? "" : "exampleMode: " + mode + "\nexampleProjects: [Inno.Probe]\n")
        + "---\n# " + id + "\n\nExample contract.\n" + (code is null ? "" : "\n```csharp\n" + code + "\n```\n"));

    private void Write(
        string path,
        string text
    ) {
        string file = Path.Combine(m_root, path);
        Directory.CreateDirectory(Path.GetDirectoryName(file)!);
        File.WriteAllText(file, text);
    }
}
