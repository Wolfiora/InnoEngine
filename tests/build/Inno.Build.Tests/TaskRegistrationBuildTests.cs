using System;
using System.IO;
using System.Security;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Build.Tests;

public sealed class TaskRegistrationBuildTests
{
    [Fact]
    public async Task PreEvaluatedProjectsBindPrivatePublishersToTheWorkerTaskRegistry()
    {
        string repository = TestRepository.GetRoot(typeof(TaskRegistrationBuildTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoTaskRegistration", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string project = Path.Combine(root, "Runner.csproj");
            string fixture = Path.Combine(root, "Consumer.proj");
            string report = Path.Combine(root, "result.json");
            await File.WriteAllTextAsync(project, """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                    <LangVersion>14.0</LangVersion>
                    <OutputType>Exe</OutputType>
                    <ImplicitUsings>disable</ImplicitUsings>
                    <Nullable>enable</Nullable>
                  </PropertyGroup>
                  <ItemGroup>
                    <Reference Include="Microsoft.Build" HintPath="$(MSBuildBinPath)/Microsoft.Build.dll" />
                    <Reference Include="Microsoft.Build.Framework" HintPath="$(MSBuildBinPath)/Microsoft.Build.Framework.dll" />
                    <Reference Include="Microsoft.Build.Utilities.Core" HintPath="$(MSBuildBinPath)/Microsoft.Build.Utilities.Core.dll" />
                  </ItemGroup>
                </Project>
                """);
            await File.WriteAllTextAsync(fixture, $$"""
                <Project>
                  <Import Project="{{SecurityElement.Escape(Path.Combine(repository, "Directory.Build.props"))}}" />
                  <Import Project="{{SecurityElement.Escape(Path.Combine(repository, "build/msbuild/Inno.Build.Tasks.targets"))}}" />
                </Project>
                """);
            await File.WriteAllTextAsync(Path.Combine(root, "Program.cs"), """
                using System;
                using System.Collections.Concurrent;
                using System.Collections.Generic;
                using System.IO;
                using System.Text.Json;
                using System.Threading;
                using Microsoft.Build.Evaluation;
                using Microsoft.Build.Execution;
                using Microsoft.Build.Framework;
                using Microsoft.Build.Logging;

                Environment.SetEnvironmentVariable("MSBUILD_EXE_PATH", Path.Combine(args[1], "MSBuild.dll"));
                Environment.SetEnvironmentVariable("MSBuildSDKsPath", Path.Combine(args[1], "Sdks"));
                var reports = new List<object>();
                for (int build = 0; build < 2; build++)
                {
                    var properties = new Dictionary<string, string>
                    {
                        ["Configuration"] = "Debug",
                        ["InnoTaskArtifacts"] = Path.Combine(Path.GetDirectoryName(args[0])!, "tasks"),
                        ["ArtifactsPath"] = Path.Combine(Path.GetDirectoryName(args[0])!, "managed"),
                        ["UseSharedCompilation"] = "false",
                        ["BuildProjectReferences"] = "false",
                        ["BuildingInsideVisualStudio"] = "true"
                    };
                    using var projects = new ProjectCollection(properties);
                    ProjectInstance instance = projects.LoadProject(args[0]).CreateProjectInstance();
                    string evaluatedIdentity = instance.GetPropertyValue("InnoTaskLoadId");
                    var copies = new PrivateCopyLogger();
                    var parameters = new BuildParameters(projects)
                    {
                        MaxNodeCount = 2,
                        EnableNodeReuse = false,
                        DisableInProcNode = true,
                        Loggers = new ILogger[] { new ConsoleLogger(LoggerVerbosity.Minimal), copies }
                    };
                    using var manager = new BuildManager();
                    var request = new BuildRequestData(
                        instance,
                        new[] { "PrepareInnoTaskPublisher" },
                        null,
                        BuildRequestDataFlags.ProvideProjectStateAfterBuild
                    );
                    manager.BeginBuild(parameters);
                    try
                    {
                        BuildResult result = manager.PendBuildRequest(request).Execute();
                        if (result.OverallResult != BuildResultCode.Success)
                            return 1;
                        if (copies.count != 0)
                            return 3;
                        ProjectInstance completed = result.ProjectStateAfterBuild!;
                        string foundationProject = Path.Combine(args[3], "src/foundation/core/Inno.Core.IO/Inno.Core.IO.csproj");
                        string foundationAssembly = projects.LoadProject(foundationProject).GetPropertyValue("TargetPath");
                        using (FileStream writable = File.Open(foundationAssembly, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            if (writable.Length == 0)
                                return 2;
                        }
                        reports.Add(new
                        {
                            evaluatedIdentity,
                            registeredIdentity = completed.GetPropertyValue("InnoTaskLoadId"),
                            publisher = completed.GetPropertyValue("InnoTaskPublisherAssembly"),
                            privateCopyTasks = copies.count
                        });
                    }
                    finally
                    {
                        manager.EndBuild();
                    }
                }
                File.WriteAllText(args[2], JsonSerializer.Serialize(reports));
                return 0;

                /// <summary>Observes operation copies that must not become reusable project outputs.</summary>
                public sealed class PrivateCopyLogger : ILogger
                {
                    private readonly ConcurrentDictionary<(int, int, int), byte> m_targets = new();
                    private int m_count;

                    /// <summary>Gets the observed SDK Copy task count in private preparation targets.</summary>
                    public int count => m_count;
                    /// <inheritdoc />
                    public LoggerVerbosity Verbosity { get; set; } = LoggerVerbosity.Diagnostic;
                    /// <inheritdoc />
                    public string? Parameters { get; set; }

                    /// <inheritdoc />
                    public void Initialize(IEventSource source)
                    {
                        source.TargetStarted += (
                            sender,
                            target
                        ) => {
                            if (target.TargetName is "PrepareInnoTaskPublisher" or "PrepareInnoBuildTaskHost")
                            {
                                BuildEventContext context = target.BuildEventContext!;
                                m_targets.TryAdd((context.NodeId, context.ProjectContextId, context.TargetId), 0);
                            }
                        };
                        source.TaskStarted += (
                            sender,
                            task
                        ) => {
                            BuildEventContext context = task.BuildEventContext!;
                            if ((task.TaskName == "Copy" || task.TaskName.EndsWith(".Copy", StringComparison.Ordinal))
                                && m_targets.ContainsKey((context.NodeId, context.ProjectContextId, context.TargetId)))
                                Interlocked.Increment(ref m_count);
                        };
                    }

                    /// <inheritdoc />
                    public void Shutdown() { }
                }
                """);
            string dotnet = ToolchainEnvironment.ResolveExecutable("dotnet");
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(10));
            var environment = DotNetSdkEnvironment.Create(dotnet);
            await ToolchainEnvironment.RunAsync(dotnet,
                ["build", project, "--disable-build-servers", "-m:1", "-nr:false", "-p:UseSharedCompilation=false"],
                repository, timeout.Token, environment);
            string sdk = (await ToolchainEnvironment.CaptureOutputAsync(dotnet,
                ["msbuild", project, "-nologo", "-getProperty:MSBuildBinPath"],
                repository, timeout.Token, environment)).Trim();
            await ToolchainEnvironment.RunAsync(dotnet,
                [Path.Combine(root, "bin/Debug/net10.0/Runner.dll"), fixture, sdk, report, repository],
                repository, timeout.Token, environment);
            using JsonDocument results = JsonDocument.Parse(await File.ReadAllTextAsync(report));
            Assert.Equal(2, results.RootElement.GetArrayLength());
            string? previous = null;
            foreach (JsonElement result in results.RootElement.EnumerateArray())
            {
                string identity = result.GetProperty("registeredIdentity").GetString()!;
                string publisher = Path.GetFullPath(result.GetProperty("publisher").GetString()!);
                Assert.True(Guid.TryParseExact(identity, "N", out _));
                Assert.NotEqual(result.GetProperty("evaluatedIdentity").GetString(), identity);
                Assert.NotEqual(previous, identity);
                Assert.True(File.Exists(publisher), publisher);
                Assert.Equal(0, result.GetProperty("privateCopyTasks").GetInt32());
                Assert.Equal(identity, Directory.GetParent(Path.GetDirectoryName(publisher)!)!.Name);
                previous = identity;
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
