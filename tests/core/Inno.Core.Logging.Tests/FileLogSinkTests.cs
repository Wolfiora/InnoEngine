using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Inno.Core.Logging;
using Inno.Extensibility.Modules;
using Xunit;

namespace Inno.Core.Logging.Tests;

public sealed class FileLogSinkTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoFileLogTests", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Theory]
    [InlineData(LogDeliveryMode.Inline)]
    [InlineData(LogDeliveryMode.Background)]
    public void RouterFlushMakesCompletedFileWritesObservable(LogDeliveryMode mode)
    {
        using var router = new LogRouter(deliveryMode: mode);
        using var sink = new FileLogSink(m_root);
        router.RegisterSink(sink);
        for (int index = 0; index < 100; index++)
            router.Dispatch(CreateEntry("visible-" + index));

        router.Flush();

        string[] lines = ReadLines(Assert.Single(Directory.GetFiles(m_root, "*.log")));
        Assert.Equal(100, lines.Length);
        Assert.Contains("visible-99", lines[^1]);
        router.UnregisterSink(sink);
    }

    [Fact]
    public void RapidRotationRetainsTheCurrentEntryAndHonorsTheByteLimit()
    {
        using var sink = new FileLogSink(m_root, maxFileSizeBytes: 512, maxFiles: 3);
        for (int index = 0; index < 120; index++)
            sink.Receive(CreateEntry($"entry-{index:D3}-" + new string('★', 80)));

        string[] files = Directory.GetFiles(m_root, "*.log");
        Assert.InRange(files.Length, 1, 3);
        Assert.All(files, file => Assert.InRange(new FileInfo(file).Length, 1L, 512L));
        string combined = string.Concat(files.Select(ReadContent));
        Assert.Contains("entry-119-", combined);
        Assert.DoesNotContain("entry-000-", combined);
    }

    [Fact]
    public void OversizedEntriesRemainCompleteAndRotateBeforeTheNextEntry()
    {
        using var sink = new FileLogSink(m_root, maxFileSizeBytes: 128, maxFiles: 2);
        string oversized = new('x', 2_000);
        sink.Receive(CreateEntry(oversized));
        sink.Receive(CreateEntry("next-entry"));

        string[] files = Directory.GetFiles(m_root, "*.log");
        Assert.Equal(2, files.Length);
        Assert.Single(files, file => new FileInfo(file).Length > 128);
        Assert.Contains(oversized, string.Concat(files.Select(ReadContent)));
        Assert.Contains("next-entry", string.Concat(files.Select(ReadContent)));
    }

    [Fact]
    public void MultipleSinksOwnDistinctFilesInTheSameDirectory()
    {
        FileLogSink[] sinks = Enumerable.Range(0, 40).Select(_ => new FileLogSink(m_root, maxFiles: 64)).ToArray();
        try
        {
            for (int index = 0; index < sinks.Length; index++)
                sinks[index].Receive(CreateEntry("sink-" + index));

            string[] files = Directory.GetFiles(m_root, "*.log");
            Assert.Equal(sinks.Length, files.Length);
            Assert.All(files, file => Assert.Single(ReadLines(file)));
        }
        finally
        {
            foreach (FileLogSink sink in sinks)
                sink.Dispose();
        }
    }

    [Fact]
    public void RetentionPreservesAnActiveWriterAndRemovesItAfterClosure()
    {
        using var first = new FileLogSink(m_root, maxFileSizeBytes: 512, maxFiles: 1);
        first.Receive(CreateEntry("first-active"));
        string firstFile = Assert.Single(Directory.GetFiles(m_root, "*.log"));
        using var second = new FileLogSink(m_root, maxFileSizeBytes: 512, maxFiles: 1);
        second.Receive(CreateEntry(new string('x', 2_000)));
        first.Receive(CreateEntry("still-active"));

        Assert.Equal(2, Directory.GetFiles(m_root, "*.log").Length);
        Assert.Contains("still-active", ReadContent(firstFile));

        first.Dispose();
        second.Receive(CreateEntry("second-rotated"));

        string remaining = ReadContent(Assert.Single(Directory.GetFiles(m_root, "*.log")));
        Assert.Contains("second-rotated", remaining);
        Assert.False(File.Exists(firstFile));
    }

    [Fact]
    public void ConcurrentReceiversWriteWholeEntriesExactlyOnce()
    {
        using var sink = new FileLogSink(m_root);
        Parallel.For(0, 256, index => sink.Receive(CreateEntry("concurrent-" + index)));

        string[] lines = ReadLines(Assert.Single(Directory.GetFiles(m_root, "*.log")));
        Assert.Equal(256, lines.Length);
        Assert.Equal(256, lines.Distinct(StringComparer.Ordinal).Count());
    }

    [Theory]
    [InlineData(0L, 3)]
    [InlineData(-1L, 3)]
    [InlineData(512L, 0)]
    [InlineData(512L, -1)]
    public void InvalidLimitsFailBeforeCreatingFiles(
        long size,
        int count
    ) {
        Assert.Throws<ArgumentOutOfRangeException>(() => new FileLogSink(m_root, size, count));
        Assert.False(Directory.Exists(m_root));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public void InvalidDirectoryFailsAtConstruction(string? directory)
        => Assert.ThrowsAny<ArgumentException>(() => new FileLogSink(directory!));

    [Fact]
    public void FileBlockingTheDirectoryIsReportedWithoutChangingItsContent()
    {
        Directory.CreateDirectory(m_root);
        string blocked = Path.Combine(m_root, "blocked");
        File.WriteAllText(blocked, "preserve");

        Assert.ThrowsAny<IOException>(() => new FileLogSink(blocked));
        Assert.Equal("preserve", ReadContent(blocked));
    }

    [Fact]
    public void DisposedSinkRejectsWritesAndRepeatedDisposalIsSafe()
    {
        var sink = new FileLogSink(m_root);
        sink.Receive(CreateEntry("before-disposal"));
        sink.Dispose();
        sink.Dispose();

        Assert.Throws<ObjectDisposedException>(() => sink.Receive(CreateEntry("after-disposal")));
        string content = ReadContent(Assert.Single(Directory.GetFiles(m_root, "*.log")));
        Assert.Contains("before-disposal", content);
        Assert.DoesNotContain("after-disposal", content);
    }

    private static string[] ReadLines(string file) => ReadContent(file).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    private static string ReadContent(string file)
    {
        using var reader = new StreamReader(new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        return reader.ReadToEnd();
    }

    private static LogEntry CreateEntry(string message) => new(
        LogLevel.Info, AssemblyDomain.InnoInternal, AssemblyScope.Runtime,
        "file-test", message, string.Empty, 0, string.Empty, LogSessionId.none);
}
