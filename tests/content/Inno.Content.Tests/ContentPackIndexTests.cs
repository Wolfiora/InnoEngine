using System;
using Xunit;

namespace Inno.Content.Tests;

public sealed class ContentPackIndexTests
{
    private static readonly string Hash = new('A', 64);

    [Fact]
    public void IndexCopiesSortsAndRejectsCollisionsAndFileDirectoryConflicts()
    {
        ContentEntry[] entries = [Entry("b/file", 2), Entry("a", 1)];
        ContentPackIndex index = new(entries);
        entries[0] = Entry("changed", 7);
        Assert.Equal("a", index.entries[0].key.value);
        Assert.Equal(3, index.totalLength);
        Assert.Throws<ArgumentException>(() => new ContentPackIndex([Entry("a", 1), Entry("A", 2)]));
        Assert.Throws<ArgumentException>(() => new ContentPackIndex([Entry("a", 1), Entry("a/b", 2)]));
        Assert.Throws<ArgumentException>(() => new ContentPackIndex([Entry("content.index", 1)]));
    }

    private static ContentEntry Entry(
        string key,
        long length
    ) => new(new ContentKey(key), length, Hash);
}
