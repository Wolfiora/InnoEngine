using System;
using System.IO;
using Xunit;

namespace Inno.Tooling.Architecture.Tests;

[Collection("Architecture diagnostics")]
public sealed class AgentDocumentTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoAgentDocuments", Guid.NewGuid().ToString("N"));

    public AgentDocumentTests()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        foreach (string category in new[] { "rulebook", "records", "records/plans", "records/decisions", "records/acceptance", "records/history" })
        {
            string directory = Path.Combine(m_root, ".agents", category);
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "README.md"), "# Category\n");
        }
    }

    [Fact]
    public void AgentLinksUseParsedHeadingsReferencesAndExplicitAnchorsWithoutReadingCodeLinks()
    {
        File.WriteAllText(Path.Combine(m_root, "AGENTS.md"), "# Entry\n\n[Guide][guide]\n\n[guide]: .agents/rulebook/README.md#hello-world\n\n[Explicit](.agents/rulebook/README.md#contract)\n\n~~~text\n[missing](missing.md)\n~~~\n");
        File.WriteAllText(Path.Combine(m_root, ".agents/rulebook/README.md"), "# Hello *World*\n\n<a id=\"contract\"></a>\n");
        Assert.Equal(0, ArchitectureCommandFixture.Run(m_root, "--agent-documents-only").exitCode);
        File.AppendAllText(Path.Combine(m_root, "AGENTS.md"), "\n[Broken](.agents/rulebook/README.md#absent)\n");
        var rejected = ArchitectureCommandFixture.Run(m_root, "--agent-documents-only");
        Assert.Equal(1, rejected.exitCode);
        Assert.Contains("missing anchor", rejected.output);
        File.AppendAllText(Path.Combine(m_root, "AGENTS.md"), "\n[Missing](absent.md)\n");
        Assert.Contains("missing path", ArchitectureCommandFixture.Run(m_root, "--agent-documents-only").output);
    }

    [Theory]
    [InlineData("--wiki")]
    [InlineData("--wiki-only")]
    [InlineData("--wiki-contracts-only")]
    [InlineData("--documentation-only")]
    public void RetiredProductDocumentationEntrypointsAreRejected(string option)
        => Assert.Throws<ArgumentException>(() => ArchitectureValidator.Execute([m_root, option]));

    public void Dispose() => Directory.Delete(m_root, recursive: true);
}
