using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.TaskHosting;
using Inno.Core.IO;
using Microsoft.Build.Framework;
using TaskItem = Microsoft.Build.Utilities.TaskItem;
using Xunit;

namespace Inno.Build.Tests;

public sealed class TaskRuntimeBuildTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoTaskRuntime", Guid.NewGuid().ToString("N"));
    private readonly string m_project;

    public TaskRuntimeBuildTests()
    {
        Directory.CreateDirectory(m_root);
        m_project = Path.Combine(m_root, "Runtime.csproj");
        File.WriteAllText(m_project, "<Project />");
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Fact]
    public async Task SharedBuildsYieldWhileWaitingAndSerializeWrites()
    {
        using ManualResetEventSlim entered = new();
        using ManualResetEventSlim release = new();
        RecordingEngine firstEngine = new(() => {
            entered.Set();
            Assert.True(release.Wait(TimeSpan.FromSeconds(10)));
            return true;
        });
        RecordingEngine secondEngine = new(() => true);
        BuildTaskRuntimeTask first = CreateTask(firstEngine);
        BuildTaskRuntimeTask second = CreateTask(secondEngine);
        Task<bool> firstRun = Task.Run(first.Execute);
        Assert.True(entered.Wait(TimeSpan.FromSeconds(10)));
        Task<bool> secondRun = Task.Run(second.Execute);
        try
        {
            Assert.True(SpinWait.SpinUntil(() => secondEngine.yielded, TimeSpan.FromSeconds(10)));
            Assert.Equal(0, secondEngine.builds);
        }
        finally
        {
            release.Set();
        }
        Assert.True(await firstRun);
        Assert.True(await secondRun);
        Assert.True(firstEngine.reacquired && secondEngine.reacquired);
        Assert.Single(first.TargetOutputs);
        Assert.Equal("0123", first.TargetOutputs[0].GetMetadata("FileHash"));
        Assert.Equal("Debug", firstEngine.properties!["Configuration"]);
        Assert.Equal(Path.Combine(m_root, "bootstrap"), firstEngine.properties["ArtifactsPath"]);
        Assert.Contains("RuntimeIdentifier", firstEngine.removed!);
    }

    [Fact]
    public async Task CancellationReleasesWaitAndAllowsTheNextBuild()
    {
        RecordingEngine engine = new(() => true);
        BuildTaskRuntimeTask canceled = CreateTask(engine);
        using (FileLease lease = await FileLease.AcquireAsync(
            Path.Combine(m_root, "bootstrap", "build.lock"), TimeSpan.FromSeconds(10)))
        {
            Task<bool> execution = Task.Run(canceled.Execute);
            Assert.True(SpinWait.SpinUntil(() => engine.yielded, TimeSpan.FromSeconds(10)));
            canceled.Cancel();
            Assert.False(await execution);
            Assert.Equal(0, engine.builds);
            Assert.True(engine.reacquired);
        }
        canceled.Cancel();
        Assert.True(CreateTask(new(() => true)).Execute());
    }

    [Fact]
    public void FailedBuildDoesNotRetainSharedOwnership()
    {
        Assert.False(CreateTask(new(() => false)).Execute());
        Assert.True(CreateTask(new(() => true)).Execute());
    }

    [Theory]
    [InlineData("ArtifactsPath=other")]
    [InlineData("Configuration=Debug;Configuration=Release")]
    public void InvalidPropertiesFailBeforeCreatingSharedOutput(string declarations)
    {
        BuildTaskRuntimeTask task = CreateTask(new(() => true));
        task.Properties = declarations.Split(';');
        Assert.False(task.Execute());
        Assert.False(Directory.Exists(Path.Combine(m_root, "bootstrap")));
    }

    private BuildTaskRuntimeTask CreateTask(RecordingEngine engine) => new()
    {
        BuildEngine = engine,
        ProjectFile = m_project,
        ArtifactsDirectory = Path.Combine(m_root, "bootstrap"),
        Targets = ["Restore", "PublishInnoBuildTaskHost"],
        Properties = ["Configuration=Debug", "BuildProjectReferences=true"],
        RemoveProperties = ["RuntimeIdentifier", "PublishAot"]
    };

    private sealed class RecordingEngine : IBuildEngine3
    {
        private readonly Func<bool> m_build;
        private int m_builds;
        private volatile bool m_yielded;
        internal RecordingEngine(Func<bool> build) => m_build = build;
        internal int builds => m_builds;
        internal bool yielded => m_yielded;
        internal bool reacquired { get; private set; }
        internal IDictionary? properties { get; private set; }
        internal IList<string>? removed { get; private set; }
        internal ConcurrentBag<BuildErrorEventArgs> errors { get; } = [];
        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode => string.Empty;
        public bool IsRunningMultipleNodes => true;
        public void Yield() => m_yielded = true;
        public void Reacquire() => reacquired = true;
        public void LogErrorEvent(BuildErrorEventArgs error) => errors.Add(error);
        public void LogWarningEvent(BuildWarningEventArgs warning) { }
        public void LogMessageEvent(BuildMessageEventArgs message) { }
        public void LogCustomEvent(CustomBuildEventArgs custom) { }
        public BuildEngineResult BuildProjectFilesInParallel(
            string[] projects,
            string[] targets,
            IDictionary[] globalProperties,
            IList<string>[] removeGlobalProperties,
            string[] toolsVersions,
            bool returnTargetOutputs
        ) {
            Assert.True(reacquired);
            Interlocked.Increment(ref m_builds);
            properties = globalProperties[0];
            removed = removeGlobalProperties[0];
            TaskItem output = new("Inno.Build.Tasks.dll");
            output.SetMetadata("FileHash", "0123");
            return new(m_build(), [new Dictionary<string, ITaskItem[]> { [targets[^1]] = [output] }]);
        }

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
    }
}
