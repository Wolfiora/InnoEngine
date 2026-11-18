using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class BackendPrebuiltArchiveTests : IDisposable
{
    private readonly string m_root = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoBackendPackages", Guid.NewGuid().ToString("N"));
    private string Inventory => Path.Combine(m_root, "inventory.json");
    private string Archive => Path.Combine(m_root, "platform.zip");

    public BackendPrebuiltArchiveTests() => Directory.CreateDirectory(m_root);

    [Fact]
    public async Task RepackingOneHundredRevisionsIncludesOnlyTheSelectedCurrentFiles()
    {
        for (int revision = 0; revision < 100; revision++)
        {
            string path = $"library-{revision}.dll";
            File.WriteAllText(Path.Combine(m_root, path), revision.ToString());
            Select(path);
            BackendPrebuiltArchive.Create(m_root, Inventory, Archive);
            using ZipArchive zip = ZipFile.OpenRead(Archive);
            Assert.Equal(new[] { "manifest.json", path }, zip.Entries.Select(entry => entry.FullName));
        }
        string digest = Digest(Archive);
        BackendPrebuiltArchive.Create(m_root, Inventory, Archive);
        Assert.Equal(digest, Digest(Archive));
        string output = Path.Combine(m_root, "consumer");
        await BackendPrebuiltArchive.ExtractAsync(Archive, digest, "fixture-target", output);
        Assert.Equal("99", File.ReadAllText(Path.Combine(output, "library-99.dll")));
        Assert.Equal(2, Directory.GetFiles(output).Length);
        await Assert.ThrowsAnyAsync<IOException>(() => BackendPrebuiltArchive.ExtractAsync(Archive, digest, "fixture-target", output));
        Assert.Equal("99", File.ReadAllText(Path.Combine(output, "library-99.dll")));
    }

    [Fact]
    public async Task ChangedInputsAndExtraArchiveEntriesCannotReplaceCertifiedOutputs()
    {
        string payload = Path.Combine(m_root, "library.dll");
        File.WriteAllText(payload, "certified");
        Select("library.dll");
        BackendPrebuiltArchive.Create(m_root, Inventory, Archive);
        string digest = Digest(Archive);
        File.WriteAllText(payload, "unexpected change");
        Assert.Throws<InvalidDataException>(() => BackendPrebuiltArchive.Create(m_root, Inventory, Archive));
        Assert.Equal(digest, Digest(Archive));
        string output = Path.Combine(m_root, "consumer");
        await Assert.ThrowsAsync<InvalidDataException>(() => BackendPrebuiltArchive.ExtractAsync(Archive, digest, "other-target", output));
        using (ZipArchive zip = ZipFile.Open(Archive, ZipArchiveMode.Update))
            zip.CreateEntry("undeclared");
        await Assert.ThrowsAsync<InvalidDataException>(() => BackendPrebuiltArchive.ExtractAsync(Archive, digest, "fixture-target", output));
        await Assert.ThrowsAsync<InvalidDataException>(() => BackendPrebuiltArchive.ExtractAsync(Archive, Digest(Archive), "fixture-target", output));
        Assert.False(Directory.Exists(output));
    }

    private void Select(string path)
    {
        string digest = Digest(Path.Combine(m_root, path));
        var inventory = new { version = 1, target = "fixture-target", files = new[] { new { owner = "Fixture", configuration = "Debug", input = digest, path, sha256 = digest } } };
        File.WriteAllBytes(Inventory, JsonSerializer.SerializeToUtf8Bytes(inventory));
    }

    private static string Digest(string path)
    {
        using FileStream stream = File.OpenRead(path);
        return Convert.ToHexStringLower(SHA256.HashData(stream));
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);
}
