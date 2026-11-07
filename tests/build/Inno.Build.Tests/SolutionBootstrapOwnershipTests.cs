using Inno.Testing.Source;
using System;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class SolutionBootstrapOwnershipTests
{
    [Fact]
    public async Task OwnedProductCleanDoesNotPrepareNativeArtifacts()
    {
        string repository = TestRepository.GetRoot(typeof(SolutionBootstrapOwnershipTests).Assembly);
        string root = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoProductCleanTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string tasks = Path.Combine(root, "tasks");
            string artifacts = Path.Combine(root, "managed");
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));
            using var output = new StringWriter();
            using var error = new StringWriter();
            Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(host,
                ["clean", Path.Combine(repository, "entry/InnoEngine.Editor/InnoEngine.Editor.csproj"),
                    "--artifacts-path", artifacts, "-p:InnoTaskArtifacts=" + tasks, "-p:InnoPrepareProductNative=false",
                    "--disable-build-servers", "-m:1", "-nr:false"],
                repository, cancellation.Token, DotNetSdkEnvironment.Create(host), output, error));
            Assert.True(failure is null, $"{failure}\n{output}\n{error}");
            Assert.False(Directory.Exists(tasks), output.ToString());
            Assert.Empty(Directory.GetFiles(artifacts, "*.dll", SearchOption.AllDirectories));
            Assert.DoesNotContain("INNO-NATIVE", output.ToString(), StringComparison.Ordinal);
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task BootstrapClaimsAreSharedWithinOneProcessButCannotCrossExitedProcesses()
    {
        string repository = TestRepository.GetRoot(typeof(SolutionBootstrapOwnershipTests).Assembly);
        string root = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoBootstrapLifetimeTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string project = Path.Combine(root, "Probe.proj");
            await File.WriteAllTextAsync(project, $$"""
                <Project>
                  <UsingTask TaskName="Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner" TaskFactory="RoslynCodeTaskFactory"
                             AssemblyFile="$(MSBuildToolsPath)/Microsoft.Build.Tasks.Core.dll">
                    <Task><Code Type="Class" Language="cs" Source="{{SecurityElement.Escape(Path.Combine(repository, "build/tasks/Inno.Build.TaskHosting/TaskRuntimeBootstrapOwner.cs"))}}" /></Task>
                  </UsingTask>
                  <Target Name="Clean">
                    <Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner Root="$(MSBuildProjectDirectory)/publishers" Identity="$(InnoTaskBootstrapOwner)"
                        FoundationAssembly="{{SecurityElement.Escape(typeof(ProcessOwnedDirectory).Assembly.Location)}}" />
                  </Target>
                  <Target Name="Build" DependsOnTargets="Clean">
                    <Inno.Build.TaskHosting.TaskRuntimeBootstrapOwner Root="$(MSBuildProjectDirectory)/publishers" Identity="$(InnoTaskBootstrapOwner)"
                        FoundationAssembly="{{SecurityElement.Escape(typeof(ProcessOwnedDirectory).Assembly.Location)}}" />
                  </Target>
                </Project>
                """);
            string identity = Guid.NewGuid().ToString("N");
            await InvokeAsync(identity);
            Exception expired = await Assert.ThrowsAnyAsync<Exception>(() => InvokeAsync(identity));
            Assert.Contains("exited or unverifiable operation claim", expired.Message, StringComparison.Ordinal);
            await InvokeAsync(Guid.NewGuid().ToString("N"));

            async Task InvokeAsync(string owner)
            {
                string host = ToolchainEnvironment.ResolveExecutable("dotnet");
                using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                using var output = new StringWriter();
                using var error = new StringWriter();
                Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(host,
                    ["msbuild", project, "-t:Build", "-nr:false", "-p:InnoTaskBootstrapOwner=" + owner],
                    root, cancellation.Token, DotNetSdkEnvironment.Create(host), output, error));
                if (failure is not null)
                    throw new InvalidOperationException(output.ToString() + error, failure);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    [Fact]
    public async Task SolutionSharesOneBootstrapOwnerAndRenewsItForTheNextBuild()
    {
        string root = TestRepository.GetRoot(typeof(SolutionBootstrapOwnershipTests).Assembly);
        string fixture = Path.Combine(Path.GetTempPath(), "InnoSolutionOwnerTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        try
        {
            File.Copy(Path.Combine(root, "Directory.Solution.targets"), Path.Combine(fixture, "Directory.Solution.targets"));
            foreach (string name in new[] { "First", "Second" })
                await File.WriteAllTextAsync(Path.Combine(fixture, name + ".csproj"), """
                    <Project DefaultTargets="Build">
                      <Target Name="Build">
                        <Error Condition="'$(InnoTaskBootstrapOwner)' == ''" Text="Missing solution build owner." />
                        <WriteLinesToFile File="$(MSBuildProjectDirectory)/$(MSBuildProjectName).owner"
                                          Lines="$(InnoTaskBootstrapOwner)" Overwrite="true" />
                      </Target>
                    </Project>
                    """);
            string solution = Path.Combine(fixture, "InnoEngine.sln");
            await File.WriteAllTextAsync(solution, """
                Microsoft Visual Studio Solution File, Format Version 12.00
                Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "First", "First.csproj", "{BF56473A-8C5D-4E17-8A0A-67289A364EB0}"
                EndProject
                Project("{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}") = "Second", "Second.csproj", "{4DA642A3-4E80-4CBC-9E57-22F5BA4356D0}"
                EndProject
                Global
                  GlobalSection(SolutionConfigurationPlatforms) = preSolution
                    Debug|Any CPU = Debug|Any CPU
                  EndGlobalSection
                  GlobalSection(ProjectConfigurationPlatforms) = postSolution
                    {BF56473A-8C5D-4E17-8A0A-67289A364EB0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                    {BF56473A-8C5D-4E17-8A0A-67289A364EB0}.Debug|Any CPU.Build.0 = Debug|Any CPU
                    {4DA642A3-4E80-4CBC-9E57-22F5BA4356D0}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
                    {4DA642A3-4E80-4CBC-9E57-22F5BA4356D0}.Debug|Any CPU.Build.0 = Debug|Any CPU
                  EndGlobalSection
                EndGlobal
                """);
            string host = ToolchainEnvironment.ResolveExecutable("dotnet");
            string? previous = null;
            for (int build = 0; build < 2; build++)
            {
                using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));
                using var output = new StringWriter();
                using var error = new StringWriter();
                Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(
                    host, ["msbuild", solution, "-m:2", "-nodeReuse:false", "-v:quiet"],
                    fixture, cancellation.Token, DotNetSdkEnvironment.Create(host), output, error));
                Assert.True(failure is null, $"{failure}{Environment.NewLine}{output}{Environment.NewLine}{error}");
                string first = File.ReadAllText(Path.Combine(fixture, "First.owner")).Trim();
                Assert.True(Guid.TryParseExact(first, "N", out _));
                Assert.Equal(first, File.ReadAllText(Path.Combine(fixture, "Second.owner")).Trim());
                Assert.NotEqual(previous, first);
                previous = first;
            }
        }
        finally
        {
            Directory.Delete(fixture, recursive: true);
        }
    }
}
