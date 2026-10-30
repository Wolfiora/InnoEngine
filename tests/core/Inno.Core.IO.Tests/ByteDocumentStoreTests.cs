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

    [Fact]
    public void FilesReadTheCompleteDocumentWhileADeleteAccessHandleRemainsOpen()
    {
        string root = Path.Combine(Path.GetTempPath(), "inno-documents-" + Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        string path = Path.Combine(root, "settings");
        byte[] expected = [3, 4, 5, 6];
        try
        {
            File.WriteAllBytes(path, expected);
            using (var renameHandle = new FileStream(
                       path,
                       FileMode.Open,
                       FileAccess.Read,
                       FileShare.Read | FileShare.Delete,
                       4096,
                       FileOptions.DeleteOnClose))
            {
                if (OperatingSystem.IsWindows())
                    Assert.ThrowsAny<IOException>(() => File.ReadAllBytes(path));
                var store = new FileByteDocumentStore(path);
                for (int read = 0; read < 16; read++)
                    Assert.Equal(expected, store.Read());
                Assert.True(File.Exists(path));
            }
            Assert.False(File.Exists(path));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
