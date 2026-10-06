using System;
using System.IO;
using Xunit;

namespace Inno.Core.IO.Tests;

public sealed class ByteDocumentStoreTests
{
    [Fact]
    public void ReadOnlyDocumentsOwnBytesAndRejectReplacement()
    {
        byte[] original = [1, 2, 3];
        var store = new ReadOnlyByteDocumentStore("settings", original);
        original[0] = 7;
        byte[] snapshot = store.Read();
        snapshot[1] = 8;
        Assert.Equal(new byte[] { 1, 2, 3 }, store.Read());
        Assert.False(store.canWrite);
        Assert.Throws<NotSupportedException>(() => store.Write([]));
    }

    [Fact]
    public void FilesDistinguishAbsentAndEmptyAndReplaceCompleteDocuments()
    {
        string root = Path.Combine(Path.GetTempPath(), "inno-documents-" + Guid.NewGuid().ToString("N"));
        try
        {
            var store = new FileByteDocumentStore(Path.Combine(root, "settings"));
            Assert.Null(store.Read());
            Assert.False(Directory.Exists(root));
            store.Write([]);
            Assert.Empty(store.Read()!);
            store.Write([3, 4]);
            Assert.Equal(new byte[] { 3, 4 }, store.Read());
            Assert.Single(Directory.GetFiles(root));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, true);
        }
    }
}
