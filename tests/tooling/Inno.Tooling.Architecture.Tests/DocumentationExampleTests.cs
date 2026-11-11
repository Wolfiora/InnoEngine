using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
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
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "editor", "--runtime-reference", runtime]));
        Assert.Contains("Blocked", File.ReadAllText(output));
        Assert.Equal(0, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "editor", "--runtime-reference", runtime, "--editor-reference", editor]));
        string evidence = File.ReadAllText(output);
        Assert.Contains(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(runtime))), evidence);
        Assert.Contains(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(editor))), evidence);
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "host-leak", "--runtime-reference", runtime]));
        Assert.Contains("CS0103", File.ReadAllText(output));
        Assert.Equal(1, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "runtime", "--runtime-reference", unrelated]));
        Assert.Contains("Expected the Inno.ScriptApi.Runtime", File.ReadAllText(output));
        await Assert.ThrowsAsync<ArgumentException>(() => DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "editor", "--editor-reference", editor]));
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
