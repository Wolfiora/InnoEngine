using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class NativeInputBatchTests : IDisposable
{
    private readonly string m_parent = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoNativeInputBatch");
    private readonly string m_root;
    private readonly string m_source;

    public NativeInputBatchTests()
    {
        m_root = Path.Combine(m_parent, Guid.NewGuid().ToString("N"));
        m_source = Path.Combine(m_root, "Source");
        Directory.CreateDirectory(m_source);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        for (int index = 0; index < 64; index++)
            File.WriteAllText(Path.Combine(m_source, index.ToString("D2") + ".h"), new string((char)('A' + index % 26), 16 * 1024));
    }

    [Fact]
    public void AliasesShareReadsWhileFreshPhasesObserveAllBytes()
    {
        var context = new NativeBuildContext(m_root, "debug");
        NativeBuildInput[] inputs = [new("first", m_source), new("second", m_source)];
        string initial = NativeBuildFingerprint.Create(context, ["fixture"], inputs);
        Assert.Equal(initial, NativeBuildFingerprint.Create(["fixture"], inputs.Reverse()));
        NativeBuildPhaseStatistics first = Assert.Single(context.statistics.phases);
        Assert.Equal(64, first.files);
        Assert.Equal(64 * 16 * 1024, first.bytes);
        Assert.Equal(64, first.reusedReads);
        string changed = Path.Combine(m_source, "31.h");
        DateTime timestamp = File.GetLastWriteTimeUtc(changed);
        File.WriteAllText(changed, new string('!', 16 * 1024));
        File.SetLastWriteTimeUtc(changed, timestamp);
        NativeBuildContext verification = context.BeginInputVerification("after-write");
        string current = NativeBuildFingerprint.Create(verification, ["fixture"], inputs);
        Assert.NotEqual(initial, current);
        Assert.Equal(current, NativeBuildFingerprint.Create(verification, ["fixture"], inputs));
        NativeBuildPhaseStatistics fresh = context.statistics.phases.Single(phase => phase.phase == "after-write");
        Assert.Equal(64, fresh.files);
        Assert.Equal(64 * 16 * 1024, fresh.bytes);
        Assert.Equal(192, fresh.reusedReads);
    }

    [Fact]
    public async Task FailedBatchDrainsReadersAndDoesNotFreezePartialContent()
    {
        var context = new NativeBuildContext(m_root, "debug");
        NativeBuildInput[] inputs = [new("source", m_source)];
        using (var locked = new FileStream(Path.Combine(m_source, "63.h"), FileMode.Open, FileAccess.ReadWrite, FileShare.None))
            await Assert.ThrowsAnyAsync<IOException>(() => Task.Run(() => NativeBuildFingerprint.Create(context, [], inputs)));
        foreach (string path in Directory.EnumerateFiles(m_source))
        {
            using var exclusive = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            exclusive.WriteByte((byte)'!');
        }
        Assert.Equal(NativeBuildFingerprint.Create([], inputs), NativeBuildFingerprint.Create(context, [], inputs));
    }

    [Fact]
    public void CancellationAfterInventoryCannotReturnSuccessfulPartialSnapshot()
    {
        var context = new NativeBuildContext(m_root, "debug");
        NativeBuildInput[] inputs = [new("source", m_source)];
        using var cancellation = new CancellationTokenSource();
        Assert.ThrowsAny<OperationCanceledException>(() =>
            NativeBuildFingerprint.Create(context, [], CancelAfterInventory(), cancellation.Token));
        foreach (string path in Directory.EnumerateFiles(m_source))
        {
            using var exclusive = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            exclusive.WriteByte((byte)'!');
        }
        Assert.Equal(NativeBuildFingerprint.Create([], inputs), NativeBuildFingerprint.Create(context, [], inputs));

        IEnumerable<NativeBuildInput> CancelAfterInventory()
        {
            yield return inputs[0];
            cancellation.Cancel();
        }
    }

    public void Dispose()
    {
        if (!Path.GetFullPath(m_root).StartsWith(m_parent + Path.DirectorySeparatorChar, StringComparison.Ordinal))
            throw new InvalidOperationException("The native input fixture escaped its owner.");
        Directory.Delete(m_root, recursive: true);
    }
}
