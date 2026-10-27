using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Testing.Source;
using Microsoft.Build.Logging;
using Xunit;

namespace Inno.Build.Tests;

public sealed class CompilerToolBuildScopeTests
{
    [Fact]
    public async Task AnalyzerRuntimeAndTransitiveEdgesShareOneCompilerToolBuild()
    {
        string repository = TestRepository.GetRoot(typeof(CompilerToolBuildScopeTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoCompilerToolScope", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            File.Copy(Path.Combine(repository, "global.json"), Path.Combine(root, "global.json"));
            foreach (string suffix in new[] { "props", "targets" })
                File.WriteAllText(Path.Combine(root, "Directory.Build." + suffix),
                    "<Project><Import Project=\"" + SecurityElement.Escape(Path.Combine(repository, "Directory.Build." + suffix)) + "\" /></Project>");
            WriteProject(root, "Tool", """
                <Project>
                  <PropertyGroup><IsRoslynComponent>true</IsRoslynComponent></PropertyGroup>
                  <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
                  <PropertyGroup><TargetFramework>netstandard2.0</TargetFramework></PropertyGroup>
                  <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
                </Project>
                """);
            WriteProject(root, "Runtime", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                  <ItemGroup><ProjectReference Include="../Tool/Tool.csproj" AdditionalProperties="BuildProjectReferences=true" PrivateAssets="compile" /></ItemGroup>
                </Project>
                """);
            WriteProject(root, "Analyzer", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                  <ItemGroup><ProjectReference Include="../Tool/Tool.csproj" OutputItemType="Analyzer" ReferenceOutputAssembly="false" PrivateAssets="all" /></ItemGroup>
                </Project>
                """);
            WriteProject(root, "Root", """
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup><TargetFramework>net10.0</TargetFramework></PropertyGroup>
                  <ItemGroup>
                    <ProjectReference Include="../Runtime/Runtime.csproj" />
                    <ProjectReference Include="../Analyzer/Analyzer.csproj" />
                  </ItemGroup>
                </Project>
                """);
            string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            for (int run = 0; run < 2; run++)
            {
                string binlog = Path.Combine(root, run + ".binlog");
                await ToolchainEnvironment.RunAsync(dotnet,
                    ["build", "Root/Root.csproj", "--nologo", "-m:4", "-nodeReuse:false",
                        "-p:ArtifactsPath=" + Path.Combine(root, "managed"), "-p:InnoNativeTarget=fixture-target", "-bl:" + binlog],
                    root, timeout.Token, DotNetSdkEnvironment.Create(dotnet));
                int compilations = 0;
                var identities = new HashSet<string>(StringComparer.Ordinal);
                var events = new BinaryLogReplayEventSource();
                events.ProjectStarted += (
                    _,
                    project
                ) => {
                    if (Path.GetFileName(project.ProjectFile) != "Tool.csproj" || !string.IsNullOrEmpty(project.TargetNames))
                        return;
                    Assert.NotNull(project.GlobalProperties);
                    Assert.DoesNotContain("InnoNativeTarget", project.GlobalProperties!.Keys);
                    Assert.DoesNotContain("BuildProjectReferences", project.GlobalProperties.Keys);
                    Assert.DoesNotContain("InnoTaskBootstrapOwner", project.GlobalProperties.Keys);
                    identities.Add(string.Join(";", project.GlobalProperties.OrderBy(static pair => pair.Key)
                        .Select(static pair => pair.Key + "=" + pair.Value)));
                };
                events.TaskStarted += (
                    _,
                    task
                ) => {
                    if (task.TaskName == "Csc" && Path.GetFileName(task.ProjectFile) == "Tool.csproj")
                        compilations++;
                };
                events.Replay(binlog);
                Assert.Single(identities);
                Assert.Equal(run == 0 ? 1 : 0, compilations);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }

    private static void WriteProject(
        string root,
        string name,
        string source
    ) {
        string directory = Path.Combine(root, name);
        Directory.CreateDirectory(directory);
        File.WriteAllText(Path.Combine(directory, name + ".csproj"), source);
        File.WriteAllText(Path.Combine(directory, name + ".cs"), "public sealed class " + name + "Marker { }");
    }
}
