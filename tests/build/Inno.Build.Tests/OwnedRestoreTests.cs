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
using Microsoft.Build.Utilities;
using Xunit;

namespace Inno.Build.Tests;

public sealed class OwnedRestoreTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoOwnedRestore", Guid.NewGuid().ToString("N"));
    private readonly string m_project;

    public OwnedRestoreTests()
    {
        Directory.CreateDirectory(m_root);
        m_project = Path.Combine(m_root, "Fixture.csproj");
        File.WriteAllText(m_project, "<Project />");
    }

    public void Dispose() => Directory.Delete(m_root, recursive: true);

    [Fact]
    public async System.Threading.Tasks.Task WritersYieldAndNeverEnterRestoreTogether()
    {
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        var firstEngine = new RecordingEngine(() =>
        {
            entered.Set();
            Assert.True(release.Wait(TimeSpan.FromSeconds(10)));
            return true;
        });
        var secondEngine = new RecordingEngine(static () => true);
        Task<bool> first = System.Threading.Tasks.Task.Run(Create(firstEngine).Execute);
        Assert.True(entered.Wait(TimeSpan.FromSeconds(10)));
        Task<bool> second = System.Threading.Tasks.Task.Run(Create(secondEngine).Execute);
        try
        {
            Assert.True(SpinWait.SpinUntil(() => secondEngine.yielded, TimeSpan.FromSeconds(10)));
            Assert.Equal(0, secondEngine.builds);
        }
        finally
        {
            release.Set();
        }
        Assert.True(await first);
        Assert.True(await second);
        Assert.True(File.Exists(Path.Combine(m_root, "write.lock")));
    }

    [Fact]
    public async System.Threading.Tasks.Task CancellationDoesNotEnterRestoreOrRemoveAnotherOwnersLock()
    {
        var engine = new RecordingEngine(static () => true);
        RestoreOwnedProjectTask canceled = Create(engine);
        using (FileLease lease = await FileLease.AcquireAsync(Path.Combine(m_root, "write.lock"), TimeSpan.FromSeconds(5)))
        {
            Task<bool> pending = System.Threading.Tasks.Task.Run(canceled.Execute);
            Assert.True(SpinWait.SpinUntil(() => engine.yielded, TimeSpan.FromSeconds(10)));
            canceled.Cancel();
            Assert.False(await pending);
            Assert.Equal(0, engine.builds);
            Assert.True(engine.reacquired);
        }
        canceled.Cancel();
        Assert.True(Create(new RecordingEngine(static () => true)).Execute());
    }

    [Fact]
    public void EmptyProjectSelectionFailsBeforeTakingOwnership()
    {
        var engine = new RecordingEngine(static () => true);
        RestoreOwnedProjectTask task = Create(engine);
        task.Projects = [];
        Assert.False(task.Execute());
        Assert.Equal(0, engine.builds);
        Assert.False(engine.yielded);
        Assert.False(File.Exists(Path.Combine(m_root, "write.lock")));
    }

    [Fact]
    public void ExactPropertiesAreForwardedAndFailureReleasesOwnership()
    {
        var engine = new RecordingEngine(static () => false);
        RestoreOwnedProjectTask task = Create(engine);
        task.Properties = ["Configuration=Release", "DefineConstants=A%3BB"];
        Assert.False(task.Execute());
        Assert.Equal("Release", engine.properties!["Configuration"]);
        Assert.Equal("A;B", engine.properties["DefineConstants"]);
        Assert.Equal("fixture-x64", engine.properties["InnoNativeTarget"]);
        Assert.True(Create(new RecordingEngine(static () => true)).Execute());
    }

    private RestoreOwnedProjectTask Create(RecordingEngine engine)
    {
        var project = new TaskItem(m_project);
        project.SetMetadata("AdditionalProperties", "Configuration=Debug;InnoNativeTarget=fixture-x64");
        return new RestoreOwnedProjectTask
        {
            BuildEngine = engine,
            Projects = [project],
            LockPath = Path.Combine(m_root, "write.lock")
        };
    }

    private sealed class RecordingEngine(Func<bool> restore) : IBuildEngine3
    {
        private volatile bool m_yielded;
        private int m_builds;
        internal bool yielded => m_yielded;
        internal int builds => m_builds;
        internal bool reacquired { get; private set; }
        internal IDictionary? properties { get; private set; }
        public bool ContinueOnError => false;
        public int LineNumberOfTaskNode => 0;
        public int ColumnNumberOfTaskNode => 0;
        public string ProjectFileOfTaskNode => string.Empty;
        public bool IsRunningMultipleNodes => true;
        public void Yield() => m_yielded = true;
        public void Reacquire() => reacquired = true;
        public void LogErrorEvent(BuildErrorEventArgs error) { }
        public void LogWarningEvent(BuildWarningEventArgs warning) { }
        public void LogMessageEvent(BuildMessageEventArgs message) { }
        public void LogCustomEvent(CustomBuildEventArgs custom) { }

        public BuildEngineResult BuildProjectFilesInParallel(
            string[] projects,
            string[] targets,
            IDictionary[] globals,
            IList<string>[] removed,
            string[] versions,
            bool returnTargetOutputs
        ) {
            Assert.True(reacquired);
            Assert.Equal("Restore", Assert.Single(targets));
            properties = globals[0];
            Interlocked.Increment(ref m_builds);
            return new BuildEngineResult(restore(), []);
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
