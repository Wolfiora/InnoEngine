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
using Inno.Core.IO;

namespace Inno.Tooling.Documentation.Integration.Tests;

[Collection(nameof(DocumentationApiFixture))]
public sealed class ApiPackageImportTests : DocumentationApiFixture
{
    [Fact]
    public async Task PackageImportPreservesManualProseAndSkipsRevisionOnlyChanges()
    {
        await SyncAsync();
        string member = Fragment(C_MEMBER);
        string manual = File.ReadAllText(member).Replace("source: xml", "source: Ada Example", StringComparison.Ordinal)
                        + "\nRead this value only after publication.\n";
        File.WriteAllText(member, manual);
        Bind(description: "Updated package XML.");
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]));
        Bind();
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Equal(Body(manual), Body(File.ReadAllText(member)));
        Assert.Contains("outdatedFor:", File.ReadAllText(member));
        Assert.Contains("Updated package XML.", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/index.md")));
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        JsonNode next = JsonNode.Parse(File.ReadAllText(package))!;
        next["revision"] = new string('c', 40);
        JsonNode bindingFile = next["files"]!.AsArray().Single(file => file!["path"]!.GetValue<string>() == "reference/engine.lock.json")!;
        string revised = bindingFile["text"]!
            .GetValue<string>()
            .Replace(new string('a', 40), new string('c', 40), StringComparison.Ordinal);
        bindingFile["text"] = revised;
        bindingFile["sha256"] = Hash(revised);
        next["toolingRevision"] = new string('c', 40);
        File.WriteAllText(package, next.ToJsonString());
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
        Assert.Equal(Body(manual), Body(File.ReadAllText(member)));
        Assert.Contains("outdatedFor:", File.ReadAllText(member));
    }

    [Theory]
    [InlineData("tampered")]
    [InlineData("extra-file")]
    [InlineData("escape")]
    [InlineData("mixed-revisions")]
    public async Task InvalidPackagesCannotModifyTheCheckout(string corruption)
    {
        await SyncAsync();
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]);
        JsonNode packet = JsonNode.Parse(File.ReadAllText(package))!;
        JsonArray files = packet["files"]!.AsArray();
        if (corruption == "tampered") files[0]!["text"] = "{}";
        if (corruption == "extra-file")
        {
            files.Add(new JsonObject { ["path"] = "content/en/manual/task.md", ["text"] = "replace", ["sha256"] = Hash("replace") });
        }
        if (corruption == "escape") files[0]!["path"] = "reference/api/../../AGENTS.md";
        if (corruption == "mixed-revisions") packet["revision"] = new string('d', 40);
        File.WriteAllText(package, packet.ToJsonString());
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        string before = File.ReadAllText(Fragment(C_TYPE));
        await Assert.ThrowsAsync<InvalidDataException>(
            () => DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package])
        );
        Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
        Assert.Equal(before, File.ReadAllText(Fragment(C_TYPE)));
    }

    [Fact]
    public async Task ImportWaitsForTheSharedWriterBeforeChangingFactsAndCancellationPreservesInputs()
    {
        await SyncAsync();
        Bind(description: "New import contract.");
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]);
        Bind();
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        byte[] facts = File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json"));
        using (FileLease writer = await FileLease.AcquireAsync(
            Path.Combine(m_root, "artifacts/api-sync/writer.lock"),
            Timeout.InfiniteTimeSpan
        )) {
            using var cancellation = new CancellationTokenSource();
            Task<int> import = DocumentationCommand.ExecuteAsync(
                ["api-import", "--docs-root", m_root, "--package", package],
                cancellation.Token
            );
            Assert.False(import.IsCompleted);
            Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
            Assert.Equal(facts, File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json")));
            cancellation.Cancel();
            await Assert.ThrowsAnyAsync<OperationCanceledException>(() => import);
            Assert.Equal(facts, File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json")));
        }
        Assert.Equal(0, await DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package]));
        Assert.Contains("New import contract.", File.ReadAllText(Path.Combine(m_root, "content/en/api/engine/probe/index.md")));
    }

    [Fact]
    public async Task ImportRestoresFactsWhenAuthoredContentRejectsSynchronization()
    {
        await SyncAsync();
        Bind(description: "Changed package XML.");
        string package = Path.Combine(m_root, "artifacts/api-package.json");
        await DocumentationCommand.ExecuteAsync(["api-package", "--docs-root", m_root, "--package", package]);
        Bind();
        string member = Fragment(C_MEMBER);
        string invalid = File.ReadAllText(member).Replace("source: xml", "source: \"\"", StringComparison.Ordinal);
        File.WriteAllText(member, invalid);
        byte[] binding = File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json"));
        byte[] facts = File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json"));
        await Assert.ThrowsAsync<InvalidDataException>(
            () => DocumentationCommand.ExecuteAsync(["api-import", "--docs-root", m_root, "--package", package])
        );
        Assert.Equal(binding, File.ReadAllBytes(Path.Combine(m_root, "reference/engine.lock.json")));
        Assert.Equal(facts, File.ReadAllBytes(Path.Combine(m_root, "reference/api/probe.json")));
        Assert.Equal(invalid, File.ReadAllText(member));
        Assert.False(File.Exists(Path.Combine(m_root, "reference/api/export.json")));
    }
}
