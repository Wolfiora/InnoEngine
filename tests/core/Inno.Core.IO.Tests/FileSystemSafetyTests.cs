using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Core.IO.Tests;

public sealed class FileSystemSafetyTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoCoreIoTests", Guid.NewGuid().ToString("N"));

    private readonly ITestOutputHelper m_output;

    public FileSystemSafetyTests(ITestOutputHelper output)
    {
        m_output = output;
        Directory.CreateDirectory(m_root);
    }

    [Fact]
    public void WriteAllBytesReplacesTheCompletePayloadWithoutLeavingStagingFiles()
    {
        string destination = Path.Combine(m_root, "Settings.Project.inno");
        File.WriteAllText(destination, "previous");

        AtomicFile.WriteAllBytes(destination, "current"u8);

        Assert.Equal("current", File.ReadAllText(destination));
        Assert.Empty(Directory.EnumerateFiles(m_root).Where(path => path.Contains(".staging-", StringComparison.Ordinal)));
    }

    [Fact]
    public void InstallRequiresASameDirectoryCandidateAndPreservesTheDestinationOnFailure()
    {
        string candidateDirectory = Path.Combine(m_root, "candidate");
        Directory.CreateDirectory(candidateDirectory);
        string candidate = Path.Combine(candidateDirectory, "document.inno");
        string destination = Path.Combine(m_root, "document.inno");
        File.WriteAllText(candidate, "candidate");
        File.WriteAllText(destination, "current");

        Assert.Throws<ArgumentException>(() => AtomicFile.Install(candidate, destination));

        Assert.Equal("current", File.ReadAllText(destination));
        Assert.Equal("candidate", File.ReadAllText(candidate));
    }

    [Fact]
    public void InstallReplacesAnExistingFileWithTheCompleteCandidate()
    {
        string candidate = Path.Combine(m_root, "document.staging");
        string destination = Path.Combine(m_root, "document.inno");
        File.WriteAllText(candidate, "candidate");
        File.WriteAllText(destination, "current");

        AtomicFile.Install(candidate, destination);

        Assert.False(File.Exists(candidate));
        Assert.Equal("candidate", File.ReadAllText(destination));
    }

    [Fact]
    public void PathBoundaryRejectsAbsoluteAndParentTraversalPaths()
    {
        Assert.Throws<ArgumentException>(() => PathBoundary.Resolve(m_root, Path.Combine(m_root, "absolute")));
        Assert.Throws<IOException>(() => PathBoundary.Resolve(m_root, "../escape"));
        Assert.Equal(Path.Combine(m_root, "Assets", "item.inno"), PathBoundary.Resolve(m_root, "Assets/item.inno"));
    }

    [Fact]
    public void OwnedTreeEnumerationIncludesNestedFilesAndRejectsMissingRoots()
    {
        string nested = Path.Combine(m_root, "nested");
        Directory.CreateDirectory(nested);
        string file = Path.Combine(nested, "value.txt");
        File.WriteAllText(file, "owned");

        Assert.Equal(file, Assert.Single(PathBoundary.EnumerateFiles(m_root)));
        Assert.Throws<DirectoryNotFoundException>(() => PathBoundary.EnumerateFiles(Path.Combine(m_root, "missing")).ToArray());
    }

    [Fact]
    public void OwnershipRejectsCaseAliasesIndependentlyOfVolumeSemantics()
    {
        string owned = Path.Combine(m_root, "Owned");
        string sibling = Path.Combine(m_root, "owned", "value.bin");
        Assert.Throws<IOException>(() => PathBoundary.RequireContained(owned, sibling));
        Assert.Throws<IOException>(() => PathBoundary.RequireUnlinkedPath(owned, sibling));
        Assert.Throws<IOException>(() => PathBoundary.Resolve(owned, "../owned/value.bin"));
        Assert.Equal(Path.Combine(owned, "value.bin"), PathBoundary.RequireUnlinkedPath(owned, Path.Combine(owned, "value.bin")));
    }

    [Fact]
    public void AtomicFileRejectsDifferentlySpelledParentBeforeRenaming()
    {
        string parent = Path.Combine(m_root, "Owned");
        Directory.CreateDirectory(parent);
        string candidate = Path.Combine(parent, "candidate.bin");
        File.WriteAllText(candidate, "candidate");
        Assert.Throws<ArgumentException>(() => AtomicFile.Install(candidate, Path.Combine(m_root, "owned", "target.bin")));
        Assert.Equal("candidate", File.ReadAllText(candidate));
    }

    [Fact]
    public void DirectoryInstallReplacesACompleteTreeWithoutLeavingBackupDirectories()
    {
        string candidate = Path.Combine(m_root, "candidate");
        string destination = Path.Combine(m_root, "destination");
        Directory.CreateDirectory(candidate);
        Directory.CreateDirectory(destination);
        File.WriteAllText(Path.Combine(candidate, "current.txt"), "current", Encoding.UTF8);
        File.WriteAllText(Path.Combine(destination, "previous.txt"), "previous", Encoding.UTF8);

        AtomicDirectory.Install(candidate, destination);

        Assert.False(Directory.Exists(candidate));
        Assert.False(File.Exists(Path.Combine(destination, "previous.txt")));
        Assert.Equal("current", File.ReadAllText(Path.Combine(destination, "current.txt"), Encoding.UTF8));
        Assert.Empty(Directory.EnumerateDirectories(m_root).Where(path => path.Contains(".backup-", StringComparison.Ordinal)));
    }

    /// <summary>
    /// Rejects canceled installation before creating parents or modifying either owned tree.
    /// </summary>
    /// <param name="existing">
    /// Whether a previous destination is already present.
    /// </param>
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void DirectoryInstallPriorCancellationPreservesBothOwners(bool existing)
    {
        string candidate = Path.Combine(m_root, "candidate");
        string destination = Path.Combine(m_root, "publication", "destination");
        Directory.CreateDirectory(candidate);
        File.WriteAllText(Path.Combine(candidate, "current.txt"), "current");
        if (existing)
        {
            Directory.CreateDirectory(destination);
            File.WriteAllText(Path.Combine(destination, "previous.txt"), "previous");
        }
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();

        Assert.ThrowsAny<OperationCanceledException>(() => AtomicDirectory.Install(candidate, destination, cancellation.Token));

        Assert.Equal("current", File.ReadAllText(Path.Combine(candidate, "current.txt")));
        if (existing)
        {
            Assert.Equal("previous", File.ReadAllText(Path.Combine(destination, "previous.txt")));
        }
        else
        {
            Assert.False(Directory.Exists(Path.GetDirectoryName(destination)));
        }
        Assert.Empty(Directory.EnumerateDirectories(m_root, "*.backup-*", SearchOption.AllDirectories));
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    [Trait("ProcessIsolation", "Required")]
    public void DirectoryInstallCancellationRestoresThePreviousTreeDuringABlockedRename()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        string candidate = Directory.CreateDirectory(Path.Combine(m_root, "candidate")).FullName;
        string destination = Directory.CreateDirectory(Path.Combine(m_root, "destination")).FullName;
        string file = Path.Combine(candidate, "value.txt");
        File.WriteAllText(file, "current");
        File.WriteAllText(Path.Combine(destination, "previous.txt"), "previous");
        using FileStream reader = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var cancellation = new CancellationTokenSource();
        bool evacuated = false;
        ObserveBlockedRename(
            () =>
            {
                evacuated = !Directory.Exists(destination);
                cancellation.Cancel();
            },
            () =>
            {
                OperationCanceledException failure = Assert.ThrowsAny<OperationCanceledException>(
                    () => AtomicDirectory.Install(candidate, destination, cancellation.Token)
                );
                Assert.Equal(cancellation.Token, failure.CancellationToken);
            }
        );
        Assert.True(evacuated);
        Assert.Equal("previous", File.ReadAllText(Path.Combine(destination, "previous.txt")));
        Assert.Equal("current", File.ReadAllText(file));
        Assert.Empty(Directory.EnumerateDirectories(m_root, "*.backup-*"));
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    [Trait("ProcessIsolation", "Required")]
    public void DirectoryPublicationObservesCancellationAfterARealSharingFailure() => VerifyBlockedCancellation(false);

    [Fact]
    [SupportedOSPlatform("windows")]
    [Trait("ProcessIsolation", "Required")]
    public void DirectoryPublicationObservesPendingCancellationAtTheDeadline() => VerifyBlockedCancellation(true);

    [SupportedOSPlatform("windows")]
    private void VerifyBlockedCancellation(bool expireWindow)
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        string candidate = Directory.CreateDirectory(Path.Combine(m_root, "candidate")).FullName;
        string destination = Path.Combine(m_root, "published");
        string file = Path.Combine(candidate, "value.txt");
        File.WriteAllText(file, "complete");
        using FileStream reader = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var cancellation = new CancellationTokenSource();
        ObserveBlockedRename(
            () =>
            {
                cancellation.Cancel();
                if (expireWindow)
                {
                    Thread.Sleep(TimeSpan.FromMilliseconds(2100));
                }
            },
            () =>
            {
                OperationCanceledException failure = Assert.ThrowsAny<OperationCanceledException>(
                    () => AtomicDirectory.Publish(candidate, destination, cancellation.Token)
                );
                Assert.Equal(cancellation.Token, failure.CancellationToken);
            }
        );
        Assert.Equal("complete", File.ReadAllText(file));
        Assert.False(Directory.Exists(destination));
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    [Trait("ProcessIsolation", "Required")]
    public void DirectoryPublicationToleratesATemporaryWindowsReaderWithoutDeleteSharing()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        string candidate = Directory.CreateDirectory(Path.Combine(m_root, "candidate")).FullName;
        string destination = Path.Combine(m_root, "published");
        string file = Path.Combine(candidate, "value.txt");
        File.WriteAllText(file, "complete");
        using FileStream reader = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        using var cancellation = new CancellationTokenSource();
        ObserveBlockedRename(reader.Dispose, () => AtomicDirectory.Publish(candidate, destination, cancellation.Token));
        cancellation.Cancel();
        Assert.Equal("complete", File.ReadAllText(Path.Combine(destination, "value.txt")));
        Assert.False(Directory.Exists(candidate));
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public void PersistentReaderPreservesCandidateAndReturnsWindowsError()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }
        string candidate = Directory.CreateDirectory(Path.Combine(m_root, "candidate")).FullName;
        string destination = Path.Combine(m_root, "published");
        string file = Path.Combine(candidate, "value.txt");
        File.WriteAllText(file, "complete");
        using FileStream reader = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.Read);
        Exception? failure = Record.Exception(() => AtomicDirectory.Publish(candidate, destination));
        Assert.True(failure is IOException or UnauthorizedAccessException && IsSharingError(failure));
        Assert.False(Directory.Exists(destination));
        Assert.Equal("complete", File.ReadAllText(file));
    }

    [Fact]
    public void DirectoryPublicationPreservesAnExistingOwner()
    {
        string candidate = Directory.CreateDirectory(Path.Combine(m_root, "candidate")).FullName;
        string destination = Directory.CreateDirectory(Path.Combine(m_root, "published")).FullName;
        File.WriteAllText(Path.Combine(candidate, "value.txt"), "complete");
        File.WriteAllText(Path.Combine(destination, "previous.txt"), "previous");
        Assert.ThrowsAny<IOException>(() => AtomicDirectory.Publish(candidate, destination));
        Assert.Equal("previous", File.ReadAllText(Path.Combine(destination, "previous.txt")));
        Assert.Equal("complete", File.ReadAllText(Path.Combine(candidate, "value.txt")));
    }

    private static bool IsSharingError(Exception exception)
        => exception.HResult is unchecked((int)0x8007_0005) or unchecked((int)0x8007_0020) or unchecked((int)0x8007_0021);

    private void ObserveBlockedRename(
        Action onBlocked,
        Action operation
    ) {
        // The isolated test intercepts the actual Windows denial before the production catch filter runs.
        int thread = Environment.CurrentManagedThreadId;
        Stopwatch clock = Stopwatch.StartNew();
        long blocked = -1;
        long resumed = -1;
        int hresult = 0;
        void Observe(
            object? sender,
            FirstChanceExceptionEventArgs args
        ) {
            if (Environment.CurrentManagedThreadId != thread || blocked >= 0 || !IsSharingError(args.Exception))
            {
                return;
            }
            blocked = clock.ElapsedMilliseconds;
            hresult = args.Exception.HResult;
            onBlocked();
            resumed = clock.ElapsedMilliseconds;
        }
        AppDomain.CurrentDomain.FirstChanceException += Observe;
        try
        {
            operation();
        }
        finally
        {
            AppDomain.CurrentDomain.FirstChanceException -= Observe;
            m_output.WriteLine(
                $"Rename worker started at 0 ms, denied at {blocked} ms, intervention completed at {resumed} ms, exited at {clock.ElapsedMilliseconds} ms, HRESULT 0x{hresult:X8}."
            );
        }
        Assert.True(blocked >= 0, "Expected an actual Windows sharing failure on the publishing thread.");
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public void DirectoryCleanupFailurePreservesTheCompleteCommittedCandidate()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        string candidate = Path.Combine(m_root, "candidate");
        string destination = Path.Combine(m_root, "destination");
        Directory.CreateDirectory(candidate);
        Directory.CreateDirectory(destination);
        File.WriteAllText(Path.Combine(candidate, "current.txt"), "current");
        string lockedFile = Path.Combine(destination, "locked.txt");
        File.WriteAllText(lockedFile, "previous");
        File.SetAttributes(lockedFile, FileAttributes.ReadOnly);
        try
        {
            DirectoryCleanupException failure = Assert.Throws<DirectoryCleanupException>(
                () => AtomicDirectory.Install(candidate, destination)
            );

            Assert.Contains("installed", failure.Message, StringComparison.OrdinalIgnoreCase);
            Assert.Equal("current", File.ReadAllText(Path.Combine(destination, "current.txt")));
            Assert.False(Directory.Exists(candidate));
            string backup = Assert.Single(Directory.EnumerateDirectories(m_root, "destination.backup-*"));
            Assert.Equal(destination, failure.installedDirectory);
            Assert.Equal(backup, failure.backupDirectory);
            Assert.Equal("previous", File.ReadAllText(Path.Combine(backup, "locked.txt")));
        }
        finally
        {
            foreach (string retained in Directory.EnumerateFiles(m_root, "locked.txt", SearchOption.AllDirectories))
            {
                File.SetAttributes(retained, FileAttributes.Normal);
            }
        }
    }

    [Fact]
    public void DirectoryInstallRejectsOverlappingTreesBeforeChangingEitherOwner()
    {
        string destination = Path.Combine(m_root, "destination");
        string candidate = Path.Combine(destination, "candidate");
        Directory.CreateDirectory(candidate);
        File.WriteAllText(Path.Combine(candidate, "current.txt"), "current");
        File.WriteAllText(Path.Combine(destination, "previous.txt"), "previous");

        Assert.Throws<ArgumentException>(() => AtomicDirectory.Install(candidate, destination));
        Assert.Throws<ArgumentException>(() => AtomicDirectory.Install(destination, candidate));
        Assert.Throws<ArgumentException>(() => AtomicDirectory.Install(destination, destination));

        Assert.Equal("current", File.ReadAllText(Path.Combine(candidate, "current.txt")));
        Assert.Equal("previous", File.ReadAllText(Path.Combine(destination, "previous.txt")));
        Assert.Empty(Directory.EnumerateDirectories(m_root, "*.backup-*", SearchOption.AllDirectories));
    }

    [Fact]
    [SupportedOSPlatform("windows")]
    public void DirectoryInstallRestoresThePreviousTreeWhenTheCandidateCannotMove()
    {
        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        string candidate = Path.Combine(m_root, "candidate");
        string destination = Path.Combine(m_root, "destination");
        Directory.CreateDirectory(candidate);
        Directory.CreateDirectory(destination);
        string candidateFile = Path.Combine(candidate, "current.txt");
        File.WriteAllText(candidateFile, "current");
        File.WriteAllText(Path.Combine(destination, "previous.txt"), "previous");
        using var held = new FileStream(candidateFile, FileMode.Open, FileAccess.Read, FileShare.Read);

        Exception? failure = Record.Exception(() => AtomicDirectory.Install(candidate, destination));

        Assert.True(failure is IOException or UnauthorizedAccessException);
        Assert.Equal("previous", File.ReadAllText(Path.Combine(destination, "previous.txt")));
        Assert.Equal("current", File.ReadAllText(candidateFile));
        Assert.Empty(Directory.EnumerateDirectories(m_root, "*.backup-*"));
    }

    [Fact]
    public void PathBoundaryAcceptsChildrenOfAVolumeRoot()
    {
        string root = Path.GetPathRoot(m_root)!;
        string relative = Path.Combine("InnoBoundaryProbe", "value.bin");
        string child = Path.Combine(root, relative);

        Assert.Equal(child, PathBoundary.Resolve(root, relative));
        Assert.Equal(child, PathBoundary.RequireContained(root, child));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
        {
            Directory.Delete(m_root, recursive: true);
        }
    }
}
