using Inno.Testing.Source;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Scripting.Compiler.Tests;

/// <summary>
/// Verifies that generated script projects and nested solutions retain their own build inputs.
/// </summary>
public sealed class IdeProjectIsolationTests
{
    /// <summary>
    /// Builds both generated script projects without importing unrelated repository configuration.
    /// </summary>
    /// <returns>
    /// Completion after the SDK restores and builds the generated solution with hostile ambient build files present.
    /// </returns>
    [Fact]
    public async Task GeneratedProjectsIgnoreAmbientBuildConfiguration()
    {
        using var fixture = new CompilerFixture();
        fixture.Write("Counter.cs", "public sealed class Counter { public int count { get; set => field = value; } = 3; }");
        Assert.True(fixture.Compile().success);
        fixture.compiler.GenerateProjectFiles();
        foreach (string kind in new[] { "props", "targets" })
        {
            File.WriteAllText(Path.Combine(fixture.projectRoot, "Directory.Build." + kind), $$"""
                <Project>
                  <Target Name="RejectAmbient{{kind}}" BeforeTargets="PrepareForBuild">
                    <Error Text="An unrelated Directory.Build.{{kind}} entered the script project." />
                  </Target>
                </Project>
                """);
        }
        DotNetSdkDescriptor sdk = await ResolveSdk(Path.Combine(fixture.projectRoot, "Inno.GameScripts.csproj"));
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "build", "InnoProject.sln", "--disable-build-servers", "-m:1", "-nr:false", "-v:quiet"],
            fixture.projectRoot, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
    }

    /// <summary>
    /// Restores a solution beneath the checkout without replacing its graph with the engine solution.
    /// </summary>
    /// <returns>
    /// Completion after the nested project's own assets file exists following a normal SDK solution restore.
    /// </returns>
    [Fact]
    public async Task NestedSolutionRestoresOnlyItsOwnProjects()
    {
        string repository = TestRepository.GetRoot(typeof(IdeProjectIsolationTests).Assembly);
        using ProcessOwnedDirectory owner = await ProcessOwnedDirectory.CreateAsync(
            Path.Combine(repository, "artifacts", "acceptance", "nested-solution-tests"));
        string project = Path.Combine(owner.directory, "Nested.csproj");
        File.WriteAllText(project, """
            <Project>
              <PropertyGroup>
                <ImportDirectoryBuildProps>false</ImportDirectoryBuildProps>
                <ImportDirectoryBuildTargets>false</ImportDirectoryBuildTargets>
              </PropertyGroup>
              <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <LangVersion>14.0</LangVersion>
                <ImplicitUsings>disable</ImplicitUsings>
              </PropertyGroup>
              <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
            </Project>
            """);
        DotNetSdkDescriptor sdk = await ResolveSdk(project);
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "new", "sln", "--format", "sln", "--name", "Nested"],
            owner.directory, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "sln", "Nested.sln", "add", "Nested.csproj"],
            owner.directory, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
        await ToolchainEnvironment.RunAsync(sdk.hostPath,
            [sdk.cliPath, "restore", "Nested.sln", "--disable-build-servers", "-m:1", "-nr:false"],
            owner.directory, CancellationToken.None, DotNetSdkEnvironment.Create(sdk.hostPath));
        Assert.True(File.Exists(Path.Combine(owner.directory, "obj", "project.assets.json")));
    }

    private static ValueTask<DotNetSdkDescriptor> ResolveSdk(string project)
    {
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        return DotNetSdkResolver.ResolveAsync(host, project);
    }
}
