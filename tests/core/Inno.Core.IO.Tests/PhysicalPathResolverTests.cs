using System;
using System.IO;
using Xunit;

namespace Inno.Core.IO.Tests;

public sealed class PhysicalPathResolverTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoPhysicalPaths", Guid.NewGuid().ToString("N"));

    [Fact]
    public void ReadingResolutionDoesNotCreateMissingEntriesOrGrantOwnership()
    {
        Directory.CreateDirectory(m_root);
        string file = Path.Combine(m_root, "input.bin");
        File.WriteAllText(file, "input");
        var resolver = new PhysicalPathResolver();
        string resolved = resolver.Resolve(file);
        Assert.True(Path.IsPathFullyQualified(resolved));
        Assert.Equal("input", File.ReadAllText(resolved));
        Assert.Equal(resolved, resolver.Resolve(file));
        Assert.Equal(resolved, new PhysicalPathResolver().Resolve(resolved));
        for (string? entry = resolved; entry is not null; entry = Path.GetDirectoryName(entry))
            Assert.False(File.GetAttributes(entry).HasFlag(FileAttributes.ReparsePoint), entry);
        string owner = Path.Combine(Path.GetDirectoryName(resolved)!, "Owned");
        Assert.Throws<IOException>(() => PathBoundary.RequireContained(owner, resolved));
        Assert.False(Directory.Exists(owner));
        string missing = Path.Combine(m_root, "missing.bin");
        Assert.Throws<FileNotFoundException>(() => resolver.Resolve(missing));
        Assert.False(File.Exists(missing));
        Assert.Throws<ArgumentException>(() => resolver.Resolve(string.Empty));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }
}
