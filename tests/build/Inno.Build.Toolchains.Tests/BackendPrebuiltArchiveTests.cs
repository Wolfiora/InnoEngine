using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.IO;

namespace Inno.Build.Toolchains.Tests;

public sealed class BackendPrebuiltArchiveTests : IDisposable
{
    private readonly string m_root = Path.Combine(
        new PhysicalPathResolver().Resolve(Path.GetTempPath()),
        "InnoBackendPackages",
        Guid.NewGuid().ToString("N")
    );

    public BackendPrebuiltArchiveTests()
    {
        Directory.CreateDirectory(m_root);
    }

    [Fact]
    public async Task RepackingOneHundredRevisionsIncludesOnlyTheSelectedCurrentFiles()
    {
        for (int revision = 0; revision < 100; revision++)
        {
            string path = $"library-{revision}.dll";
            File.WriteAllText(Path.Combine(m_root, path), revision.ToString());
            Select(path);
            BackendPrebuiltArchive.Create(m_root, inventory, archive);
            using ZipArchive zip = ZipFile.OpenRead(archive);
            Assert.Equal(new[] { "manifest.json", path }, zip.Entries.Select(entry => entry.FullName));
        }
        string digest = Digest(archive);
        BackendPrebuiltArchive.Create(m_root, inventory, archive);
        Assert.Equal(digest, Digest(archive));
        string output = Path.Combine(m_root, "consumer");
        await BackendPrebuiltArchive.ExtractAsync(archive, digest, "fixture-target", output);
        Assert.Equal("99", File.ReadAllText(Path.Combine(output, "library-99.dll")));
        Assert.Equal(3, Directory.GetFiles(output).Length);
        Assert.Contains(digest, File.ReadAllText(Path.Combine(output, "receipt.json")));
        await Assert.ThrowsAnyAsync<IOException>(() => BackendPrebuiltArchive.ExtractAsync(archive, digest, "fixture-target", output));
        Assert.Equal("99", File.ReadAllText(Path.Combine(output, "library-99.dll")));
    }

    [Fact]
    public async Task ChangedInputsAndExtraArchiveEntriesCannotReplaceCertifiedOutputs()
    {
        string payload = Path.Combine(m_root, "library.dll");
        File.WriteAllText(payload, "certified");
        Select("library.dll");
        BackendPrebuiltArchive.Create(m_root, inventory, archive);
        string digest = Digest(archive);
        File.WriteAllText(payload, "unexpected change");
        Assert.Throws<InvalidDataException>(() => BackendPrebuiltArchive.Create(m_root, inventory, archive));
        Assert.Equal(digest, Digest(archive));
        string output = Path.Combine(m_root, "consumer");
        await Assert.ThrowsAsync<InvalidDataException>(() => BackendPrebuiltArchive.ExtractAsync(archive, digest, "other-target", output));
        using (ZipArchive zip = ZipFile.Open(archive, ZipArchiveMode.Update))
        {
            zip.CreateEntry("undeclared");
        }
        await Assert.ThrowsAsync<InvalidDataException>(
            () => BackendPrebuiltArchive.ExtractAsync(archive, digest, "fixture-target", output)
        );
        await Assert.ThrowsAsync<InvalidDataException>(
            () => BackendPrebuiltArchive.ExtractAsync(archive, Digest(archive), "fixture-target", output)
        );
        Assert.False(Directory.Exists(output));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    private string inventory => Path.Combine(m_root, "inventory.json");
    private string archive => Path.Combine(m_root, "platform.zip");

    private void Select(string path)
    {
        string digest = Digest(Path.Combine(m_root, path));
        var manifest = new
        {
            version = 1,
            target = "fixture-target",
            files = new[] { new { owner = "Fixture", configuration = "Debug", input = digest, path, sha256 = digest } }
        };
        File.WriteAllBytes(inventory, JsonSerializer.SerializeToUtf8Bytes(manifest));
    }

    private static string Digest(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Convert.ToHexStringLower(SHA256.HashData(stream));
    }
}
