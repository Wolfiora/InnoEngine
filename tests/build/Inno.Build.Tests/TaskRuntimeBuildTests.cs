using System.Security.Cryptography;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        File.WriteAllText(m_project, "<Project><ItemGroup><Compile Include=\"source.cs\" /></ItemGroup></Project>");
        File.WriteAllText(Path.Combine(m_root, "source.cs"), "class A {}");
        File.WriteAllText(Path.Combine(m_root, "Inno.Build.Tasks.dll"), "prepared-runtime");
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
        Task<bool> firstRun = Task.Factory.StartNew(first.Execute,
            CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        Assert.True(entered.Wait(TimeSpan.FromSeconds(10)));
        Task<bool> secondRun = Task.Factory.StartNew(second.Execute,
            CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        try
        {
            Assert.True(SpinWait.SpinUntil(() => secondEngine.yielded, TimeSpan.FromSeconds(10)));
            Assert.Equal(0, secondEngine.builds);
        }
        finally
        {
            release.Set();
        }
        Assert.True(await firstRun, string.Join("\n", firstEngine.errors.Select(static error => error.Message)));
        Assert.True(await secondRun, string.Join("\n", secondEngine.errors.Select(static error => error.Message)));
        Assert.True(firstEngine.reacquired && secondEngine.reacquired);
        Assert.Single(first.TargetOutputs);
        Assert.Equal(Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(Path.Combine(m_root, "Inno.Build.Tasks.dll")))),
            first.TargetOutputs[0].GetMetadata("FileHash"));
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
            Task<bool> execution = Task.Factory.StartNew(canceled.Execute,
                CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            try
            {
                Assert.True(SpinWait.SpinUntil(() => engine.yielded || execution.IsCompleted, TimeSpan.FromSeconds(10)),
                    string.Join("\n", engine.errors.Select(static error => error.Message)));
                Assert.True(engine.yielded, string.Join("\n", engine.errors.Select(static error => error.Message)));
                canceled.Cancel();
                Assert.False(await execution);
                Assert.Equal(0, engine.builds);
                Assert.True(engine.reacquired);
            }
            finally
            {
                canceled.Cancel();
                await execution;
            }
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

    [Fact]
    public void ReuseIsLimitedToOneBuildAndRejectsSameLengthSameTimeSourceChanges()
    {
        var engine = new RecordingEngine(static () => true);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        var reused = CreateTask(engine);
        Assert.True(reused.Execute(), string.Join("\n", engine.errors));
        Assert.Equal(1, engine.builds);
        Assert.Contains(Path.Combine(m_root, "hosts"), reused.TargetOutputs[0].ItemSpec);
        string source = Path.Combine(m_root, "source.cs");
        DateTime timestamp = File.GetLastWriteTimeUtc(source);
        File.WriteAllText(source, "class B {}");
        File.SetLastWriteTimeUtc(source, timestamp);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(2, engine.builds);
        var nextBuild = new RecordingEngine(static () => true);
        Assert.True(CreateTask(nextBuild).Execute(), string.Join("\n", nextBuild.errors));
        Assert.Equal(1, nextBuild.builds);
    }

    [Fact]
    public void CorruptedImmutableRuntimeIsRepairedBeforeReuse()
    {
        var engine = new RecordingEngine(static () => true);
        var first = CreateTask(engine);
        Assert.True(first.Execute(), string.Join("\n", engine.errors));
        File.WriteAllText(first.TargetOutputs[0].ItemSpec, "corrupted");
        var second = CreateTask(engine);
        Assert.True(second.Execute(), string.Join("\n", engine.errors));
        Assert.Equal(2, engine.builds);
        Assert.Equal("prepared-runtime", File.ReadAllText(second.TargetOutputs[0].ItemSpec));
    }

    [Fact]
    public void InputsChangingDuringCompilationNeverEnterTheBuildRegistry()
    {
        var engine = new RecordingEngine(() =>
        {
            File.WriteAllText(Path.Combine(m_root, "source.cs"), "class B {}");
            return true;
        });
        Assert.False(CreateTask(engine).Execute());
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(2, engine.builds);
    }

    private BuildTaskRuntimeTask CreateTask(RecordingEngine engine)
    {
        engine.outputPath = Path.Combine(m_root, "Inno.Build.Tasks.dll");
        return new()
        {
        BuildEngine = engine,
        ProjectFile = m_project,
        ArtifactsDirectory = Path.Combine(m_root, "bootstrap"),
        LoadDirectory = Path.Combine(m_root, "loads", Guid.NewGuid().ToString("N"), "Debug"),
        Targets = ["Restore", "PublishInnoBuildTaskHost"],
        Properties = ["Configuration=Debug", "BuildProjectReferences=true"],
        RemoveProperties = ["RuntimeIdentifier", "PublishAot"]
        };
    }

    private sealed class RecordingEngine : IBuildEngine4
    {
        private readonly Func<bool> m_build;
        private readonly ConcurrentDictionary<object, object> m_registry = new();
        internal string outputPath = string.Empty;
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
            TaskItem output = new(outputPath);
            output.SetMetadata("FileHash", Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(outputPath))));
            output.SetMetadata("RelativePath", "Inno.Build.Tasks.dll");
            return new(m_build(), [new Dictionary<string, ITaskItem[]> { [targets[^1]] = [output] }]);
        }

        public object GetRegisteredTaskObject(
            object key,
            RegisteredTaskObjectLifetime lifetime
        ) => m_registry.TryGetValue(key, out object? value) ? value : null!;
        public void RegisterTaskObject(
            object key,
            object value,
            RegisteredTaskObjectLifetime lifetime,
            bool allowEarlyCollection
        ) => m_registry[key] = value;
        public object UnregisterTaskObject(
            object key,
            RegisteredTaskObjectLifetime lifetime
        ) => m_registry.TryRemove(key, out object? value) ? value : null!;

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
