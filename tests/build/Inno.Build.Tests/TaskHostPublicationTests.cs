using System;
using System.Collections;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Inno.Build.TaskHosting;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Xunit;

namespace Inno.Build.Tests;

public sealed class TaskHostPublicationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoTaskPublication", Guid.NewGuid().ToString("N"));
    private readonly RecordingBuildEngine m_engine = new();

    public TaskHostPublicationTests()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "input"));
        File.WriteAllBytes(Path.Combine(m_root, "input", "Inno.Build.Tasks.dll"), [1, 2, 3, 4]);
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Fact]
    public void RepeatedPublicationReusesOneVerifiedRuntimeWithoutChangingItsFiles()
    {
        PublishTaskHostTask first = CreateTask();
        Assert.True(first.Execute());
        DateTime timestamp = File.GetLastWriteTimeUtc(first.PublishedAssembly);
        PublishTaskHostTask second = CreateTask();
        Assert.True(second.Execute());
        Assert.Equal(first.PublishedAssembly, second.PublishedAssembly);
        Assert.Equal(timestamp, File.GetLastWriteTimeUtc(second.PublishedAssembly));
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "hosts")));
        Assert.Empty(m_engine.errors);
    }

    [Fact]
    public async System.Threading.Tasks.Task ConcurrentReadersShareTheCompleteContentIdentity()
    {
        PublishTaskHostTask first = CreateTask();
        PublishTaskHostTask second = CreateTask();
        bool[] results = await System.Threading.Tasks.Task.WhenAll(
            System.Threading.Tasks.Task.Run(first.Execute), System.Threading.Tasks.Task.Run(second.Execute));
        Assert.All(results, result => Assert.True(result));
        Assert.Equal(first.PublishedAssembly, second.PublishedAssembly);
        Assert.Single(Directory.GetDirectories(Path.Combine(m_root, "hosts")));
        Assert.Equal(2, Directory.GetDirectories(Path.Combine(m_root, "loads")).Length);
    }

    [Fact]
    public void ChangedSelectedSourceCannotPublishOrReplaceAnExistingRuntime()
    {
        PublishTaskHostTask first = CreateTask();
        Assert.True(first.Execute());
        PublishTaskHostTask frozen = CreateTask();
        File.WriteAllBytes(Path.Combine(m_root, "input", "Inno.Build.Tasks.dll"), [4, 3, 2, 1]);
        Assert.False(frozen.Execute());
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, File.ReadAllBytes(first.PublishedAssembly));
        Assert.Single(m_engine.errors);
    }

    [Fact]
    public void CacheValidationDetectsSameLengthAndTimestampTamperingAndExtraPayload()
    {
        PublishTaskHostTask first = CreateTask();
        Assert.True(first.Execute());
        DateTime timestamp = File.GetLastWriteTimeUtc(first.PublishedAssembly);
        File.WriteAllBytes(first.PublishedAssembly, [4, 3, 2, 1]);
        File.SetLastWriteTimeUtc(first.PublishedAssembly, timestamp);
        File.WriteAllText(Path.Combine(Path.GetDirectoryName(first.PublishedAssembly)!, "unselected.dll"), "extra");
        PublishTaskHostTask repaired = CreateTask();
        Assert.True(repaired.Execute());
        Assert.Equal(new byte[] { 1, 2, 3, 4 }, File.ReadAllBytes(repaired.PublishedAssembly));
        Assert.Single(Directory.GetFiles(Path.GetDirectoryName(repaired.PublishedAssembly)!));
    }

    [Fact]
    public void CanceledBootstrapNeverCreatesACacheOrReader()
    {
        PublishTaskHostTask task = CreateTask();
        task.Cancel();
        Assert.False(task.Execute());
        Assert.False(Directory.Exists(Path.Combine(m_root, "hosts")));
        Assert.False(Directory.Exists(Path.Combine(m_root, "loads")));
    }

    [Fact]
    public void RetirementKeepsLiveReadersAndReleasesDeadReadersAndTheirUnreferencedRuntime()
    {
        PublishTaskHostTask old = CreateTask();
        Assert.True(old.Execute());
        PublishTaskHostTask live = CreateTask();
        Assert.True(live.Execute());
        MarkDead(old);
        File.WriteAllBytes(Path.Combine(m_root, "input", "Inno.Build.Tasks.dll"), [5, 6, 7, 8]);
        Assert.True(CreateTask().Execute());
        Assert.False(Directory.Exists(Path.GetDirectoryName(old.LoadDirectory)!));
        Assert.True(File.Exists(old.PublishedAssembly));
        MarkDead(live);
        Assert.True(CreateTask().Execute());
        Assert.False(File.Exists(old.PublishedAssembly));
    }

    [Theory]
    [InlineData("../escaped.dll")]
    [InlineData("C:/escaped.dll")]
    [InlineData("nested//escaped.dll")]
    public void InvalidInputIdentityFailsBeforeCreatingOutput(string relative)
    {
        PublishTaskHostTask task = CreateTask();
        task.InputFiles[0].SetMetadata("RelativePath", relative);
        Assert.False(task.Execute());
        Assert.False(Directory.Exists(Path.Combine(m_root, "hosts")));
    }

    [Fact]
    public void BootstrapRetirementPreservesCurrentProcessAndRemovesOnlyConfirmedDeadOwners()
    {
        PublishTaskHostTask previous = CreateTask();
        previous.PublisherDirectory = Path.Combine(m_root, "publishers", "previous", "Debug");
        Directory.CreateDirectory(previous.PublisherDirectory);
        Assert.True(previous.Execute());
        string owners = Path.Combine(Path.GetDirectoryName(previous.PublisherDirectory)!, "owners");
        foreach (string marker in Directory.GetFiles(owners, "*.pid"))
            File.WriteAllText(marker, int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture));

        PublishTaskHostTask current = CreateTask();
        current.PublisherDirectory = Path.Combine(m_root, "publishers", "current", "Debug");
        Directory.CreateDirectory(current.PublisherDirectory);
        Assert.True(current.Execute());
        Assert.False(Directory.Exists(Path.GetDirectoryName(previous.PublisherDirectory)!));
        Assert.True(Directory.Exists(current.PublisherDirectory));
    }

    private PublishTaskHostTask CreateTask()
    {
        string source = Path.Combine(m_root, "input", "Inno.Build.Tasks.dll");
        TaskItem file = new(source);
        file.SetMetadata("RelativePath", "Inno.Build.Tasks.dll");
        file.SetMetadata("FileHash", Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(source))));
        return new PublishTaskHostTask
        {
            InputFiles = [file], CacheDirectory = Path.Combine(m_root, "hosts"),
            LoadDirectory = Path.Combine(m_root, "loads", Guid.NewGuid().ToString("N"), "Debug"),
            BuildEngine = m_engine
        };
    }

    private static void MarkDead(PublishTaskHostTask task)
    {
        string owners = Path.Combine(Path.GetDirectoryName(task.LoadDirectory)!, "owners");
        foreach (string marker in Directory.GetFiles(owners, "*.pid"))
            File.WriteAllText(marker, int.MaxValue.ToString(System.Globalization.CultureInfo.InvariantCulture));
    }

    private sealed class RecordingBuildEngine : IBuildEngine
    {
        internal ConcurrentBag<BuildErrorEventArgs> errors { get; } = [];
        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode => string.Empty;
        public void LogErrorEvent(BuildErrorEventArgs error) => errors.Add(error);
        public void LogWarningEvent(BuildWarningEventArgs warning) { }
        public void LogMessageEvent(BuildMessageEventArgs message) { }
        public void LogCustomEvent(CustomBuildEventArgs custom) { }
        public bool BuildProjectFile(
            string project,
            string[] targets,
            IDictionary properties,
            IDictionary outputs
        ) => throw new NotSupportedException();
    }
}
