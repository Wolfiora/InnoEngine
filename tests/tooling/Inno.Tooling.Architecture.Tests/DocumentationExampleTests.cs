using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Tooling.Documentation;
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
        Assert.Equal(0, await DocumentationExamples.ExecuteAsync([..Arguments(index.snapshot, output), "--page", "working"]));
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
