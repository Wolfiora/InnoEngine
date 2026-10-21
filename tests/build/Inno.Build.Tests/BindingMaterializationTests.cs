using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Inno.Build.TaskHosting;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;
using Microsoft.Build.Framework;

namespace Inno.Build.Tests;

public sealed class BindingMaterializationTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoBindingConsumption", Guid.NewGuid().ToString("N"));
    private readonly RecordingBuildEngine m_engine = new();

    public BindingMaterializationTests()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "artifact", "Generated"));
        Directory.CreateDirectory(Path.Combine(m_root, "artifact", "Native"));
        File.WriteAllText(Path.Combine(m_root, "artifact", "Generated", "Bindings.cs"), "public static class Binding { }");
        BuildArtifactManifest.Write(Path.Combine(m_root, "artifact"), "selected", ["Native", "Generated"]);
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Fact]
    public async Task ConsumerCopySurvivesSourceRetirementAndUnchangedCopyKeepsTimestamp()
    {
        MaterializeBindingsTask first = CreateTask();
        Assert.True(first.Execute());
        string contents = File.ReadAllText(first.MaterializedSource);
        DateTime timestamp = DateTime.UtcNow.AddDays(-1);
        File.SetLastWriteTimeUtc(first.MaterializedSource, timestamp);
        timestamp = File.GetLastWriteTimeUtc(first.MaterializedSource);
        MaterializeBindingsTask second = CreateTask();
        Assert.True(second.Execute());
        Assert.Equal(timestamp, File.GetLastWriteTimeUtc(second.MaterializedSource));
        using (BuildArtifactWriteLease retirement = await BuildArtifactWriteLease.AcquireAsync(first.ArtifactDirectory))
            Directory.Delete(first.ArtifactDirectory, recursive: true);
        Assert.Equal(contents, File.ReadAllText(first.MaterializedSource));
    }

    [Fact]
    public void DamagedArtifactDoesNotReplaceAnExistingConsumerCopy()
    {
        MaterializeBindingsTask first = CreateTask();
        Assert.True(first.Execute());
        string original = File.ReadAllText(first.MaterializedSource);
        File.WriteAllText(first.Source, "invalid");
        MaterializeBindingsTask damaged = CreateTask();
        Assert.False(damaged.Execute());
        Assert.Empty(damaged.MaterializedSource);
        Assert.Equal(original, File.ReadAllText(first.MaterializedSource));
        Assert.NotEmpty(m_engine.errors);
    }

    [Fact]
    public void RelativeArtifactOwnerIsRejectedBeforeCreatingAConsumerDirectory()
    {
        MaterializeBindingsTask task = CreateTask();
        task.ArtifactDirectory = "relative-owner";
        Assert.False(task.Execute());
        Assert.False(Directory.Exists(task.IntermediateDirectory));
    }

    [Fact]
    public async Task CancellationDoesNotDisturbAnExclusivePublisher()
    {
        MaterializeBindingsTask task = CreateTask();
        using BuildArtifactWriteLease publisher = await BuildArtifactWriteLease.AcquireAsync(task.ArtifactDirectory);
        Task<bool> waiting = Task.Run(task.Execute);
        task.Cancel();
        Assert.False(await waiting);
        Assert.False(Directory.Exists(task.IntermediateDirectory));
        Assert.True(BuildArtifactManifest.IsComplete(task.ArtifactDirectory, task.Fingerprint, ["Native", "Generated"]));
    }

    [Fact]
    public async Task HostConsumerYieldsItsNodeAndWaitsForThePublicationOwner()
    {
        MaterializeBindingsTask task = CreateHostTask();
        using FileLease publisher = await FileLease.AcquireAsync(HostPublicationLock, TimeSpan.FromSeconds(5));
        Task<bool> waiting = Task.Run(task.Execute);
        try
        {
            await m_engine.yielded.Task.WaitAsync(TimeSpan.FromSeconds(5));
            await Task.Delay(TimeSpan.FromSeconds(32));
            Assert.False(waiting.IsCompleted);
            Assert.False(Directory.Exists(task.IntermediateDirectory));
            File.WriteAllText(task.Source, "public static class PublishedBinding { }");
        }
        finally
        {
            publisher.Dispose();
            await waiting.WaitAsync(TimeSpan.FromSeconds(10));
        }
        Assert.True(await waiting.WaitAsync(TimeSpan.FromSeconds(10)));
        Assert.True(m_engine.reacquired);
        Assert.Equal(File.ReadAllText(task.Source), File.ReadAllText(task.MaterializedSource));
        Assert.Empty(m_engine.errors);
    }

    [Fact]
    public async Task CancelingAHostConsumerRetainsThePublisherAndPreviousConsumerCopy()
    {
        MaterializeBindingsTask task = CreateHostTask();
        Directory.CreateDirectory(Path.Combine(task.IntermediateDirectory, "bindings"));
        string previous = Path.Combine(task.IntermediateDirectory, "bindings", "Bindings.cs");
        File.WriteAllText(previous, "previous");
        using FileLease publisher = await FileLease.AcquireAsync(HostPublicationLock, TimeSpan.FromSeconds(5));
        Task<bool> waiting = Task.Run(task.Execute);
        await m_engine.yielded.Task.WaitAsync(TimeSpan.FromSeconds(5));
        task.Cancel();
        Assert.False(await waiting.WaitAsync(TimeSpan.FromSeconds(10)));
        Assert.True(m_engine.reacquired);
        Assert.Empty(task.MaterializedSource);
        Assert.Equal("previous", File.ReadAllText(previous));
        Assert.Equal("public static class HostBinding { }", File.ReadAllText(task.Source));
        await Assert.ThrowsAsync<TimeoutException>(async () =>
        {
            using FileLease conflicting = await FileLease.AcquireAsync(HostPublicationLock, TimeSpan.FromMilliseconds(100));
        });
    }

    [Fact]
    public async Task ReacquiringAFailedBuildNodeReleasesThePublicationOwner()
    {
        MaterializeBindingsTask task = CreateHostTask();
        m_engine.failReacquire = true;
        Assert.False(task.Execute());
        Assert.False(Directory.Exists(task.IntermediateDirectory));
        using FileLease next = await FileLease.AcquireAsync(HostPublicationLock, TimeSpan.FromSeconds(5));
        Assert.NotEmpty(m_engine.errors);
    }

    private string HostPublicationLock => Path.Combine(m_root, "component", "obj", "bindings", "generation.lock");

    private MaterializeBindingsTask CreateHostTask()
    {
        MaterializeBindingsTask task = CreateTask();
        task.ArtifactDirectory = string.Empty;
        task.Source = Path.Combine(m_root, "component", "Generated", "Bindings.cs");
        Directory.CreateDirectory(Path.GetDirectoryName(task.Source)!);
        File.WriteAllText(task.Source, "public static class HostBinding { }");
        return task;
    }

    private MaterializeBindingsTask CreateTask() => new()
    {
        Source = Path.Combine(m_root, "artifact", "Generated", "Bindings.cs"),
        ArtifactDirectory = Path.Combine(m_root, "artifact"),
        Fingerprint = "selected",
        ComponentProject = Path.Combine(m_root, "component", "Component.csproj"),
        IntermediateDirectory = Path.Combine(m_root, "sdk", "obj"),
        BuildEngine = m_engine
    };

    private sealed class RecordingBuildEngine : IBuildEngine3
    {
        internal List<BuildErrorEventArgs> errors { get; } = [];
        internal TaskCompletionSource yielded { get; } = new(TaskCreationOptions.RunContinuationsAsynchronously);
        internal bool reacquired { get; private set; }
        internal bool failReacquire { get; set; }
        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode => string.Empty;
        public bool IsRunningMultipleNodes => true;
        public void Yield() => yielded.TrySetResult();
        public void Reacquire()
        {
            if (failReacquire)
                throw new InvalidOperationException("The build node is unavailable.");
            reacquired = true;
        }
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
        public bool BuildProjectFile(
            string project,
            string[] targets,
            IDictionary properties,
            IDictionary outputs,
            string toolsVersion
        ) => throw new NotSupportedException();

        public bool BuildProjectFilesInParallel(
            string[] projects,
            string[] targets,
            IDictionary[] properties,
            IDictionary[] outputs,
            string[] toolsVersions,
            bool useResultsCache,
            bool unloadProjects
        ) => throw new NotSupportedException();

        public BuildEngineResult BuildProjectFilesInParallel(
            string[] projects,
            string[] targets,
            IDictionary[] properties,
            IList<string>[] removedProperties,
            string[] toolsVersions,
            bool returnTargetOutputs
        ) => throw new NotSupportedException();
    }
}
