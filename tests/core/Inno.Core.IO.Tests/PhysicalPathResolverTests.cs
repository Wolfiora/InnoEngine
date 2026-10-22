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
        Assert.Equal(file, resolver.Resolve(file));
        Assert.Equal(file, resolver.Resolve(file));
        Assert.Throws<IOException>(() => PathBoundary.RequireContained(Path.Combine(m_root, "Owned"), resolver.Resolve(file)));
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
