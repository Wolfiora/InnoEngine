using System;
using System.IO;
using System.Text.Json;
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
