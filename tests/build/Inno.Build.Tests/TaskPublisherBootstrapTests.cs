using Inno.Testing.Source;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Build.Logging;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class TaskPublisherBootstrapTests
{
    [Theory]
    [InlineData("Debug")]
    [InlineData("Release")]
    public async Task ProjectGraphSharesCompilationButKeepsPrivateLoadsAndFreshBuildOwnership(string configuration)
    {
        string repository = TestRepository.GetRoot(typeof(TaskPublisherBootstrapTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoPublisherTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.WriteAllText(Path.Combine(root, "Directory.Build.props"),
                "<Project><Import Project=\"" + SecurityElement.Escape(Path.Combine(repository, "Directory.Build.props"))
                + "\" /></Project>");
            File.WriteAllText(Path.Combine(root, "Directory.Build.targets"), $$"""
                <Project>
                  <Import Project="{{SecurityElement.Escape(Path.Combine(repository, "Directory.Build.targets"))}}" />
                  <Target Name="ReportPublisher" AfterTargets="Build" DependsOnTargets="PrepareInnoBuildTaskHost">
                    <Message Importance="high" Text="INNO-PUBLISHER-PROBE|$(MSBuildProjectName)|$(InnoTaskBootstrapDirectory)|$(InnoTaskPublisherAssembly)" />
                  </Target>
                </Project>
                """);
            foreach (string name in new[] { "Root", "Left", "Right", "Shared" })
            {
                string directory = Path.Combine(root, name);
                Directory.CreateDirectory(directory);
                string[] dependencies = name switch
                {
                    "Root" => ["Left", "Right"],
                    "Left" or "Right" => ["Shared"],
                    _ => []
                };
                string references = string.Join(string.Empty, dependencies.Select(static name =>
                    $"<ProjectReference Include=\"../{name}/{name}.csproj\" />"));
                if (name == "Root")
                    references += "<ProjectReference Include=\"" + SecurityElement.Escape(Path.Combine(repository,
                        "src/foundation/core/Inno.Core.IO/Inno.Core.IO.csproj")) + "\" />";
                File.WriteAllText(Path.Combine(directory, name + ".csproj"),
                    "<Project Sdk=\"Microsoft.NET.Sdk\"><PropertyGroup><TargetFramework>net10.0</TargetFramework>"
                    + "<ImplicitUsings>disable</ImplicitUsings><BuildingInsideVisualStudio>false</BuildingInsideVisualStudio></PropertyGroup>"
                    + "<ItemDefinitionGroup><ProjectReference><AdditionalProperties>BuildProjectReferences=true;BuildingInsideVisualStudio=false</AdditionalProperties>"
                    + "</ProjectReference></ItemDefinitionGroup><ItemGroup>" + references + "</ItemGroup></Project>");
            }
            var first = await Build(root, configuration);
            string firstOwner = Validate(first.output, first.binlog);
            var second = await Build(root, configuration);
            string secondOwner = Validate(second.output, second.binlog);
            Assert.NotEqual(firstOwner, secondOwner);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Theory]
    [InlineData("net10.0-browser", "browser-wasm")]
    [InlineData("net10.0", "win-x64")]
    public async Task ProductPublishPropertiesDoNotReachFoundationBootstrap(
        string framework,
        string runtimeIdentifier
    ) {
        string repository = TestRepository.GetRoot(typeof(TaskPublisherBootstrapTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoBootstrapTargetTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string project = Path.Combine(root, "Probe.proj");
            File.WriteAllText(project, $$"""
                <Project>
                  <PropertyGroup><Configuration>Debug</Configuration></PropertyGroup>
                  <Import Project="{{SecurityElement.Escape(Path.Combine(repository, "Directory.Build.props"))}}" />
                  <Import Project="{{SecurityElement.Escape(Path.Combine(repository, "Directory.Build.targets"))}}" />
                  <Target Name="Probe" DependsOnTargets="PrepareInnoTaskPublisher" />
                </Project>
                """);
            string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            string binlog = Path.Combine(root, "probe.binlog");
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            using var output = new StringWriter();
            using var errors = new StringWriter();
            Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(dotnet,
                ["msbuild", project, "-nologo", "-t:Probe", "-nr:false", "-bl:" + binlog,
                    "-p:InnoTaskArtifacts=" + Path.Combine(root, "tasks"), "-p:ArtifactsPath=" + Path.Combine(root, "caller"),
                    "-p:TargetFramework=" + framework, "-p:RuntimeIdentifier=" + runtimeIdentifier,
                    "-p:PublishAot=true", "-p:PublishSingleFile=true", "-p:SelfContained=true", "-p:WasmBuildNative=true"],
                repository, timeout.Token, DotNetSdkEnvironment.Create(dotnet), output, errors));
            Assert.True(failure is null, $"{failure}\n{output}\n{errors}");
            int requests = 0;
            var events = new BinaryLogReplayEventSource();
            events.ProjectStarted += (
                _,
                item
            ) => {
                if (Path.GetFileNameWithoutExtension(item.ProjectFile) != "Inno.Core.IO"
                    || item.TargetNames != "Restore;Build")
                    return;
                requests++;
                Assert.NotNull(item.GlobalProperties);
                foreach (string property in new[] { "RuntimeIdentifier", "TargetFramework", "PublishAot", "PublishSingleFile", "SelfContained", "WasmBuildNative" })
                    Assert.DoesNotContain(property, item.GlobalProperties!.Keys);
                Assert.Equal(Path.Combine(root, "caller", ".inno-task-foundation").Replace('\\', '/'),
                    item.GlobalProperties!["ArtifactsPath"].Replace('\\', '/'));
            };
            events.Replay(binlog);
            Assert.True(requests > 0, "No neutral Foundation bootstrap was inspected.");
            Assert.False(Directory.Exists(Path.Combine(root, "caller", "bin")));
            Assert.False(Directory.Exists(Path.Combine(root, "caller", "obj")));
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static string Validate(
        string output,
        string binlog
    ) {
        string[][] reports = output.Split('\n').Select(static line => line.Trim())
            .Where(static line => line.StartsWith("INNO-PUBLISHER-PROBE|", StringComparison.Ordinal))
            .Select(static line => line.Split('|')).ToArray();
        Assert.True(reports.Length == 4, output);
        Assert.Equal(4, reports.Select(static report => report[1]).Distinct().Count());
        Assert.Equal(4, reports.Select(static report => report[3]).Distinct().Count());
        string owner = Assert.Single(reports.Select(static report => report[2]).Distinct());
        // Incremental Build targets can report TargetPath without executing the compiler.
        int compilations = 0;
        var events = new BinaryLogReplayEventSource();
        int sharedFoundationRequests = 0;
        int bootstrapFoundationRequests = 0;
        events.ProjectStarted += (
            _,
            project
        ) => {
            if (Path.GetFileNameWithoutExtension(project.ProjectFile) != "Inno.Core.IO"
                || project.GlobalProperties is not { } globals
                || !globals.TryGetValue("ArtifactsPath", out string? artifacts))
                return;
            if (string.Equals(Path.GetFileName(artifacts), ".inno-task-foundation", StringComparison.Ordinal))
            {
                bootstrapFoundationRequests++;
                Assert.DoesNotContain("InnoNativeTarget", globals.Keys);
                Assert.DoesNotContain("BuildingInsideVisualStudio", globals.Keys);
                return;
            }
            if (!string.IsNullOrEmpty(project.TargetNames)
                || !string.Equals(Path.GetFileName(artifacts), "managed", StringComparison.Ordinal))
                return;
            sharedFoundationRequests++;
            Assert.True(globals.TryGetValue("InnoNativeTarget", out string? target));
            Assert.Equal("fixture-target", target);
            Assert.True(globals.TryGetValue("BuildingInsideVisualStudio", out string? insideIde));
            Assert.Equal("false", insideIde);
        };
        events.TaskStarted += (
            _,
            task
        ) => {
            if (task.TaskName == "Csc" && Path.GetFileNameWithoutExtension(task.ProjectFile) == "Inno.Build.TaskHosting")
                compilations++;
        };
        events.Replay(binlog);
        Assert.True(sharedFoundationRequests > 0, "The real shared Foundation graph was not exercised.");
        Assert.True(bootstrapFoundationRequests > 0, "The neutral bootstrap output owner was not exercised.");
        Assert.True(compilations == 1, $"Expected one bootstrap Csc task, observed {compilations}.\n{output}");
        string[] runtimeReports = output.Split('\n').Select(static line => line.Trim())
            .Where(static line => line.StartsWith("INNO-TASK-RUNTIME ", StringComparison.Ordinal)).ToArray();
        Assert.Equal(4, runtimeReports.Length);
        Assert.Single(runtimeReports.Select(static line => line[(line.LastIndexOf(' ') + 1)..]).Distinct());
        Assert.InRange(runtimeReports.Count(static line => line.StartsWith("INNO-TASK-RUNTIME prepared ", StringComparison.Ordinal)), 0, 1);
        foreach (string[] report in reports)
            Assert.True(File.Exists(report[3]), "The private publisher was not retained for its loading process: " + report[3]);
        return owner;
    }

    private static async Task<(string output, string binlog)> Build(
        string root,
        string configuration
    ) {
        string dotnet = Path.Combine(Path.GetFullPath("../../..", RuntimeEnvironment.GetRuntimeDirectory()),
            OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        var start = new ProcessStartInfo(dotnet)
        {
            WorkingDirectory = root,
            UseShellExecute = false,
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true
        };
        string binlog = Path.Combine(root, Guid.NewGuid().ToString("N") + ".binlog");
        foreach (string argument in new[]
        {
            "build", "Root/Root.csproj", "--configuration", configuration, "--disable-build-servers", "--nologo", "-m:2", "-nodeReuse:false",
            "-p:InnoNativeTarget=fixture-target",
            "-p:InnoTaskArtifacts=" + Path.Combine(root, "tasks"),
            "-p:ArtifactsPath=" + Path.Combine(root, "managed"),
            "-bl:" + binlog
        })
            start.ArgumentList.Add(argument);
        using Process process = Process.Start(start) ?? throw new InvalidOperationException("MSBuild did not start.");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> errors = process.StandardError.ReadToEndAsync();
        using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(10));
        try
        {
            await process.WaitForExitAsync(timeout.Token);
        }
        catch
        {
            if (!process.HasExited)
                process.Kill(entireProcessTree: true);
            await process.WaitForExitAsync();
            await output;
            await errors;
            throw;
        }
        string diagnostics = await output + await errors;
        Assert.True(process.ExitCode == 0, diagnostics);
        return (diagnostics, binlog);
    }
}
