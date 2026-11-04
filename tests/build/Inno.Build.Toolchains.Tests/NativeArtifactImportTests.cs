using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class NativeArtifactImportTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeImport", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("fixture", "component/Fixture.csproj", "component/Fixture.csproj");

    [Fact]
    public async Task ImportedImmutableContentIsVerifiedAndRegisteredWithoutCopyingSourceOwnerState()
    {
        string producer = Prepare("producer");
        string consumer = Prepare("consumer");
        using NativeBuildProduct source = await PublishAsync(producer, ProduceAsync);
        string imported = Import(source, producer, consumer);
        Assert.False(File.Exists(imported + ".read.lock"));
        Assert.Empty(BuildCacheOwnerCatalog.ReadOwners(consumer));
        using NativeBuildProduct selected = await PublishAsync(consumer, static (_, _, _) => throw new InvalidOperationException("A valid import must not compile again."));
        Assert.Equal(source.fingerprint, selected.fingerprint);
        Assert.Equal("result", File.ReadAllText(selected.files.Single()));
        Assert.Contains(BuildCacheOwnerCatalog.ReadOwners(consumer), owner => owner.id == "native");
        Assert.True(File.Exists(imported + ".read.lock"));
        Assert.Single(Directory.GetFiles(Path.Combine(consumer, "artifacts", "native"), "*.cache.json"));
    }

    [Theory]
    [InlineData("same-length")]
    [InlineData("missing")]
    [InlineData("additional")]
    [InlineData("manifest-escape")]
    [InlineData("wrong-identity")]
    public async Task DamagedImportedContentsCannotBypassTheRealProducer(string damage)
    {
        string producer = Prepare("producer");
        string consumer = Prepare("consumer");
        using NativeBuildProduct source = await PublishAsync(producer, ProduceAsync);
        string imported = Import(source, producer, consumer);
        string output = Path.Combine(imported, "Outputs", "result.fixture");
        if (damage == "same-length")
        {
            DateTime timestamp = File.GetLastWriteTimeUtc(output);
            File.WriteAllText(output, "broken");
            File.SetLastWriteTimeUtc(output, timestamp);
        }
        else if (damage == "missing")
            File.Delete(output);
        else if (damage == "additional")
            File.WriteAllText(Path.Combine(imported, "Outputs", "unexpected"), "extra");
        else
            File.WriteAllText(Path.Combine(imported, "generation-manifest.json"), System.Text.Json.JsonSerializer.Serialize(new
            {
                fingerprint = damage == "wrong-identity" ? new string('0', 64) : source.fingerprint,
                files = new System.Collections.Generic.Dictionary<string, string> { ["../escape"] = new string('0', 64) }
            }));
        await Assert.ThrowsAsync<InvalidOperationException>(() => PublishAsync(consumer,
            static (_, _, _) => throw new InvalidOperationException("Retain the first real producer failure.")));
        Assert.Empty(Directory.GetFiles(Path.Combine(consumer, "artifacts", "native"), "*.current"));
        using NativeBuildProduct repaired = await PublishAsync(consumer, ProduceAsync);
        Assert.True(BuildArtifactManifest.IsComplete(repaired.directory, source.fingerprint, ["Outputs", "Metadata"]));
        Assert.Equal("result", File.ReadAllText(repaired.files.Single()));
        Assert.False(File.Exists(Path.Combine(consumer, "escape")));
    }

    [Fact]
    public async Task TransportExcludesUnadoptedCandidatesAndReimportsWithoutProducerState()
    {
        string producer = Prepare("producer");
        string consumer = Prepare("consumer");
        string unrelated = Prepare("unrelated");
        File.WriteAllText(Path.Combine(unrelated, "Source", "source.h"), "another source");
        using NativeBuildProduct source = await PublishAsync(producer, ProduceAsync);
        using NativeBuildProduct other = await PublishAsync(unrelated, ProduceAsync);
        _ = Import(other, unrelated, producer);
        string archive = Path.Combine(producer, "artifacts", "transfer.zip");
        Assert.Equal(1, await NativeArtifactTransfer.ExportAsync(producer, archive));
        using (ZipArchive zip = ZipFile.OpenRead(archive))
            Assert.All(zip.Entries, entry => Assert.DoesNotContain(other.fingerprint, entry.FullName));
        Assert.Equal(1, await NativeArtifactTransfer.ImportAsync(consumer, archive));
        Assert.DoesNotContain(BuildCacheOwnerCatalog.ReadOwners(consumer), owner => owner.id == "native");
        Assert.Empty(Directory.GetFiles(Path.Combine(consumer, "artifacts", "native"), "*.cache.json"));
        using NativeBuildProduct selected = await PublishAsync(consumer, static (_, _, _) => throw new InvalidOperationException("The imported candidate must be reusable."));
        Assert.Equal(source.fingerprint, selected.fingerprint);
        Assert.Equal(0, await NativeArtifactTransfer.ImportAsync(consumer, archive));
    }

    [Theory]
    [InlineData("escape")]
    [InlineData("tamper")]
    [InlineData("missing")]
    [InlineData("link")]
    public async Task TransportRejectsIncompleteOrUnsafeArchivesBeforeInstallingAnyCandidate(string damage)
    {
        string producer = Prepare("producer");
        string consumer = Prepare("consumer");
        using NativeBuildProduct source = await PublishAsync(producer, ProduceAsync);
        string archive = Path.Combine(producer, "artifacts", "transfer.zip");
        await NativeArtifactTransfer.ExportAsync(producer, archive);
        using (ZipArchive zip = ZipFile.Open(archive, ZipArchiveMode.Update))
        {
            ZipArchiveEntry output = zip.Entries.Single(entry => entry.FullName.EndsWith("result.fixture", StringComparison.Ordinal));
            if (damage == "missing")
                output.Delete();
            else if (damage == "tamper")
            {
                using var writer = new StreamWriter(output.Open());
                writer.Write("broken");
            }
            else if (damage == "link")
                output.ExternalAttributes = 0xa000 << 16;
            else
            {
                using var writer = new StreamWriter(zip.CreateEntry("../escape").Open());
                writer.Write("escape");
            }
        }
        await Assert.ThrowsAsync<InvalidDataException>(() => NativeArtifactTransfer.ImportAsync(consumer, archive));
        Assert.False(Directory.Exists(Path.Combine(consumer, "artifacts", "native")));
        Assert.False(File.Exists(Path.Combine(consumer, "escape")));
    }

    [Fact]
    public async Task CanceledExportPreservesThePreviousTransport()
    {
        string producer = Prepare("producer");
        using NativeBuildProduct source = await PublishAsync(producer, ProduceAsync);
        string archive = Path.Combine(producer, "artifacts", "transfer.zip");
        await NativeArtifactTransfer.ExportAsync(producer, archive);
        byte[] before = File.ReadAllBytes(archive);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => NativeArtifactTransfer.ExportAsync(producer, archive, cancellation.Token));
        Assert.Equal(before, File.ReadAllBytes(archive));
        Assert.Empty(Directory.GetFiles(Path.GetDirectoryName(archive)!, "*.staging-*"));
    }

    private string Prepare(string name)
    {
        string root = Path.Combine(m_root, name);
        Directory.CreateDirectory(Path.Combine(root, "component"));
        Directory.CreateDirectory(Path.Combine(root, "Source"));
        File.WriteAllText(Path.Combine(root, "component", "Fixture.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(root, "Source", "source.h"), "source");
        File.WriteAllText(Path.Combine(root, "InnoEngine.sln"), string.Empty);
        return root;
    }

    private Task<NativeBuildProduct> PublishAsync(string root, Func<NativeBuildContext, string, CancellationToken, Task> produce)
        => NativeArtifactPublisher.PublishAsync(new(root, "debug"), new(m_owner, "fixture", "fixture-target",
            [NativeBuildInput.FromPath(root, "Source")], ["debug", "fixture-recipe"]), produce);

    private static Task ProduceAsync(NativeBuildContext context, string directory, CancellationToken cancellation)
        => File.WriteAllTextAsync(Path.Combine(directory, "result.fixture"), "result", cancellation);

    private static string Import(NativeBuildProduct source, string producer, string consumer)
    {
        string destination = Path.Combine(consumer, Path.GetRelativePath(producer, source.directory));
        foreach (string file in Directory.GetFiles(source.directory, "*", SearchOption.AllDirectories))
        {
            string target = Path.Combine(destination, Path.GetRelativePath(source.directory, file));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!);
            File.Copy(file, target);
        }
        return destination;
    }

    public void Dispose()
    {
        string parent = Path.GetFullPath(Path.Combine(Path.GetTempPath(), "InnoNativeImport")) + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(m_root).StartsWith(parent, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The import fixture escaped its owner.");
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
