using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Inno.Build.TaskHosting;
using Inno.Build.Toolchains;
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

    private MaterializeBindingsTask CreateTask() => new()
    {
        Source = Path.Combine(m_root, "artifact", "Generated", "Bindings.cs"),
        ArtifactDirectory = Path.Combine(m_root, "artifact"),
        Fingerprint = "selected",
        ComponentProject = Path.Combine(m_root, "component", "Component.csproj"),
        IntermediateDirectory = Path.Combine(m_root, "sdk", "obj"),
        BuildEngine = m_engine
    };

    private sealed class RecordingBuildEngine : IBuildEngine
    {
        internal List<BuildErrorEventArgs> errors { get; } = [];
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
