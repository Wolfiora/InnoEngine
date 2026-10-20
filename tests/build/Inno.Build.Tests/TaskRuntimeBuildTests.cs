using System.Security.Cryptography;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
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

    /// <summary>
    /// Restores the selected target before publishing a runtime whose existing assets omit that target.
    /// </summary>
    [Fact]
    public void MissingSelectedTargetRequiresRestoreBeforePublication()
    {
        string assets = Path.Combine(m_root, "assets.json");
        File.WriteAllText(m_project, """
            <Project><PropertyGroup>
              <TargetFramework>net10.0</TargetFramework>
              <TargetFrameworkMoniker>.NETCoreApp,Version=v10.0</TargetFrameworkMoniker>
              <ProjectAssetsFile>assets.json</ProjectAssetsFile>
            </PropertyGroup></Project>
            """);
        File.WriteAllText(assets, """{"targets":{},"libraries":{},"packageFolders":{}}""");
        var engine = new RecordingEngine(() => true, () => File.WriteAllText(assets,
            """{"targets":{"net10.0":{}},"libraries":{},"packageFolders":{}}"""));
        BuildTaskRuntimeTask task = CreateTask(engine);
        task.Targets = ["PublishInnoBuildTaskHost"];
        Assert.True(task.Execute(), string.Join("\n", engine.errors.Select(static error => error.Message)));
        Assert.EndsWith(":Restore", engine.requests[0], StringComparison.Ordinal);
        Assert.Equal(1, engine.builds);
        Assert.Single(task.TargetOutputs);
    }

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
            Path.Combine(m_root, "bootstrap") + ".lock", TimeSpan.FromSeconds(10)))
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
    public void ValidPublicationIsReusedAcrossBuildsAndRejectsSameLengthSameTimeSourceChanges()
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
        Assert.Equal(0, nextBuild.builds);
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

    [Theory]
    [InlineData(false, 1)]
    [InlineData(true, 2)]
    public void BootstrapOwnershipIsForwardedWithoutHidingDerivedCompilerInputs(
        bool affectsCompilation,
        int expectedBuilds
    ) {
        if (affectsCompilation)
            File.WriteAllText(m_project, "<Project><PropertyGroup><DefineConstants>$(InnoTaskBootstrapOwner)</DefineConstants>"
                + "</PropertyGroup><ItemGroup><Compile Include=\"source.cs\" /></ItemGroup></Project>");
        var engine = new RecordingEngine(static () => true);
        foreach (string owner in new[] { new string('a', 32), new string('b', 32) })
        {
            BuildTaskRuntimeTask task = CreateTask(engine);
            task.Properties = [..task.Properties, "InnoTaskBootstrapOwner=" + owner];
            task.RemoveProperties = [..task.RemoveProperties, "InnoTaskBootstrapOwner"];
            Assert.True(task.Execute(), string.Join("\n", engine.errors));
            if (affectsCompilation || owner[0] == 'a')
            {
                Assert.Equal(owner, engine.properties!["InnoTaskBootstrapOwner"]);
                Assert.DoesNotContain("InnoTaskBootstrapOwner", engine.removed!);
            }
        }
        Assert.Equal(expectedBuilds, engine.builds);
    }

    [Theory]
    [InlineData("AdditionalFiles")]
    [InlineData("EditorConfigFiles")]
    [InlineData("GlobalAnalyzerConfigFiles")]
    [InlineData("AdditionalCompileInputs")]
    public void CompilerInputChangesCannotReuseThePreviousRuntime(string itemType)
    {
        string input = Path.Combine(m_root, "compiler-input.txt");
        File.WriteAllText(input, "value=one");
        File.WriteAllText(m_project,
            $"<Project><ItemGroup><Compile Include=\"source.cs\" /><{itemType} Include=\"compiler-input.txt\" /></ItemGroup></Project>");
        var engine = new RecordingEngine(static () => true);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(1, engine.builds);
        DateTime timestamp = File.GetLastWriteTimeUtc(input);
        File.WriteAllText(input, "value=two");
        File.SetLastWriteTimeUtc(input, timestamp);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(2, engine.builds);
    }

    [Theory]
    [InlineData("compile")]
    [InlineData("runtime")]
    [InlineData("analyzers")]
    public void ResolvedPackageBytesCannotReuseThePreviousRuntime(string assetGroup)
    {
        string packageRoot = Path.Combine(m_root, "packages");
        string packageDirectory = Path.Combine(packageRoot, "fixture", "1.0.0");
        string relative = assetGroup == "analyzers" ? "analyzers/dotnet/cs/Fixture.dll" : "lib/net10.0/Fixture.dll";
        string assembly = Path.Combine(packageDirectory, relative);
        Directory.CreateDirectory(Path.GetDirectoryName(assembly)!);
        File.WriteAllText(assembly, "package-one");
        string unrelated = Path.Combine(packageDirectory, "lib", "net8.0", "Fixture.dll");
        Directory.CreateDirectory(Path.GetDirectoryName(unrelated)!);
        File.WriteAllText(unrelated, "unselected-one");
        var package = new Dictionary<string, object> { ["type"] = "package" };
        if (assetGroup != "analyzers")
            package[assetGroup] = new Dictionary<string, object> { [relative] = new { } };
        string assets = Path.Combine(m_root, "project.assets.json");
        File.WriteAllText(assets, JsonSerializer.Serialize(new
        {
            targets = new Dictionary<string, object>
            {
                ["net10.0"] = new Dictionary<string, object>
                {
                    ["Fixture/1.0.0"] = package
                }
            },
            libraries = new Dictionary<string, object>
            {
                ["Fixture/1.0.0"] = new { type = "package", path = "fixture/1.0.0", files = new[] { relative, "lib/net8.0/Fixture.dll" } }
            },
            packageFolders = new Dictionary<string, object> { [packageRoot] = new { } }
        }));
        File.WriteAllText(m_project,
            "<Project><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
            + "<ProjectAssetsFile>" + assets + "</ProjectAssetsFile></PropertyGroup>"
            + "<ItemGroup><Compile Include=\"source.cs\" /></ItemGroup></Project>");
        var engine = new RecordingEngine(static () => true);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        File.WriteAllText(unrelated, "unselected-two");
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(1, engine.builds);
        DateTime timestamp = File.GetLastWriteTimeUtc(assembly);
        File.WriteAllText(assembly, "package-two");
        File.SetLastWriteTimeUtc(assembly, timestamp);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(2, engine.builds);
        File.Delete(assembly);
        Assert.False(CreateTask(engine).Execute());
        Assert.Equal(2, engine.builds);
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

    [Fact]
    public void DeclaredIndependentRestoresFinishBeforeCompilationAndAreNotRepeatedOnReuse()
    {
        string dependency = Path.Combine(m_root, "Dependency.csproj");
        File.WriteAllText(dependency, "<Project />");
        File.WriteAllText(m_project, "<Project><ItemGroup><Compile Include=\"source.cs\" />"
            + "<InnoBuildInputRestoreProject Include=\"Dependency.csproj\">"
            + "<AdditionalProperties>OwnedRestore=true</AdditionalProperties></InnoBuildInputRestoreProject>"
            + "</ItemGroup></Project>");
        var engine = new RecordingEngine(static () => true);
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(new[] { m_project + ":Restore", dependency + ":Restore", m_project + ":PublishInnoBuildTaskHost" }, engine.requests);
        Assert.DoesNotContain("InnoInteropRuntimeRestore", engine.properties!.Keys.Cast<string>());
        Assert.True(CreateTask(engine).Execute(), string.Join("\n", engine.errors));
        Assert.Equal(3, engine.requests.Count);
        Assert.Equal(1, engine.builds);
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
        private readonly Action? m_restore;

        internal RecordingEngine(
            Func<bool> build,
            Action? restore = null
        ) {
            m_build = build;
            m_restore = restore;
        }
        internal int builds => m_builds;
        internal bool yielded => m_yielded;
        internal bool reacquired { get; private set; }
        internal IDictionary? properties { get; private set; }
        internal IList<string>? removed { get; private set; }
        internal ConcurrentBag<BuildErrorEventArgs> errors { get; } = [];
        internal List<string> requests { get; } = [];
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
            requests.Add(projects[0] + ":" + string.Join(";", targets));
            if (returnTargetOutputs)
                Interlocked.Increment(ref m_builds);
            else if (targets.Contains("Restore", StringComparer.Ordinal))
                m_restore?.Invoke();
            properties = globalProperties[0];
            removed = removeGlobalProperties[0];
            TaskItem output = new(outputPath);
            output.SetMetadata("FileHash", Convert.ToHexString(SHA256.HashData(File.ReadAllBytes(outputPath))));
            output.SetMetadata("RelativePath", "Inno.Build.Tasks.dll");
            return new(!returnTargetOutputs || m_build(), [new Dictionary<string, ITaskItem[]> { [targets[^1]] = [output] }]);
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
