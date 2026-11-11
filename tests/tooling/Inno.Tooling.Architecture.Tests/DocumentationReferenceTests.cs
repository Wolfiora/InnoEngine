using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
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
    public async Task ApiExtractionNeedsNoWikiAndDoesNotBuildTheSelectedProduct()
    {
        const string C_PROJECT = "src/Inno.Probe/Inno.Probe.csproj";
        Write("InnoEngine.sln", "Microsoft Visual Studio Solution File, Format Version 12.00");
        Write(C_PROJECT, """
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup><TargetFramework>net10.0</TargetFramework><Nullable>enable</Nullable></PropertyGroup>
              <Target Name="RejectProductBuild" BeforeTargets="Build"><Error Text="Product build must never run during API extraction." /></Target>
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
        Assert.Equal(0, DocumentationReferences.ValidateApi(comparisonArguments));
        using (JsonDocument matched = JsonDocument.Parse(File.ReadAllBytes(comparison)))
        {
            Assert.Equal("Passed", matched.RootElement.GetProperty("status").GetString());
            Assert.Equal("Unverified", matched.RootElement.GetProperty("sourceApiStatus").GetString());
        }
        string source = File.ReadAllText(Path.Combine(m_root, "src/Inno.Probe/Probe.cs"));
        Write("src/Inno.Probe/Probe.cs", source.Replace("Borrowed text.", "Caller must release the text lease.", StringComparison.Ordinal));
        Assert.Equal(1, DocumentationReferences.ValidateApi(comparisonArguments));
        Assert.Contains("Declaration, XML, constant, source or type relationships changed", File.ReadAllText(comparison));
        Write("src/Inno.Probe/Probe.cs", "\n\n" + source);
        Assert.Equal(0, DocumentationReferences.ValidateApi(comparisonArguments));
        // Omitting a current overload is a real coverage failure, even when every retained signature matches.
        byte[] fullReference = File.ReadAllBytes(output);
        JsonNode omitted = JsonNode.Parse(fullReference)!;
        omitted["projects"]![0]!["symbols"]!.AsArray().RemoveAt(1);
        File.WriteAllText(output, omitted.ToJsonString());
        Assert.Equal(1, DocumentationReferences.ValidateApi(comparisonArguments));
        Assert.Contains("Missing current declaration", File.ReadAllText(comparison));
        File.WriteAllBytes(output, fullReference);
        byte[] firstFailure = File.ReadAllBytes(comparison);
        Assert.Throws<OperationCanceledException>(() => DocumentationReferences.ValidateApi(comparisonArguments, new CancellationToken(true)));
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
    public void InvalidReferenceDiscoveryCannotReplaceEvidence(string projects)
    {
        Write("reference.json", "{\"model\":\"source-clr\",\"configuration\":\"Debug\",\"projects\":" + projects + "}");
        Write("report.json", "first failure");
        Exception? failure = Record.Exception(() => DocumentationReferences.ValidateApi([
            "--engine-root", m_root, "--dotnet", "not-invoked", "--reference", Path.Combine(m_root, "reference.json"),
            "--output", Path.Combine(m_root, "report.json")]));
        Assert.NotNull(failure);
        Assert.True(failure is InvalidDataException or IOException, failure.ToString());
        Assert.Equal("first failure", File.ReadAllText(Path.Combine(m_root, "report.json")));
    }

    [Theory]
    [InlineData("Release", null)]
    [InlineData("Debug", "windows-x64")]
    public void DifferentEvaluatedContextsCannotShareOneBatch(
        string configuration,
        string? target
    ) {
        const string C_PROJECTS = "[{\"project\":\"src/Probe.csproj\",\"name\":\"Probe\",\"symbols\":[]}]";
        Write("first.json", "{\"model\":\"source-clr\",\"configuration\":\"Debug\",\"projects\":" + C_PROJECTS + "}");
        Write("second.json", "{\"model\":\"source-clr\",\"configuration\":" + JsonSerializer.Serialize(configuration)
            + ",\"target\":" + JsonSerializer.Serialize(target) + ",\"projects\":" + C_PROJECTS + "}");
        Write("report.json", "original result");
        InvalidDataException failure = Assert.Throws<InvalidDataException>(() => DocumentationReferences.ValidateApi([
            "--engine-root", m_root, "--dotnet", "not-invoked", "--reference", Path.Combine(m_root, "first.json"),
            "--reference", Path.Combine(m_root, "second.json"), "--output", Path.Combine(m_root, "report.json")]));
        Assert.Contains("separate evaluated batches", failure.Message);
        Assert.Equal("original result", File.ReadAllText(Path.Combine(m_root, "report.json")));
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

    private void Write(
        string path,
        string text
    ) {
        string target = Path.Combine(m_root, path);
        Directory.CreateDirectory(Path.GetDirectoryName(target)!);
        File.WriteAllText(target, text);
    }
}
