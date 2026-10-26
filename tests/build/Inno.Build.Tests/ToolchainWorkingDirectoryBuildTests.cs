using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;
using Inno.Build.Windows.Host;
using Inno.Build.WindowsX64;
using Inno.Core.IO;
using Inno.Testing.Source;

namespace Inno.Build.Tests;

public sealed class ToolchainWorkingDirectoryBuildTests
{
    [Fact]
    public async Task ProcessTempDirectoriesSelectCompatibleCMakeWorkspaces()
    {
        string repository = TestRepository.GetRoot(typeof(ToolchainWorkingDirectoryBuildTests).Assembly);
        string root = Path.Combine(Path.GetTempPath(), "InnoExecutionPathTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        try
        {
            string dotnet = ToolchainEnvironment.ResolveExecutable("dotnet");
            using var timeout = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            string cmake = OperatingSystem.IsWindows()
                ? (await new WindowsX64NativeToolchainProvider(dotnet).ResolveAsync(new(repository, "debug"),
                    BuildTestProducts.WindowsHost(), "windows-x64", timeout.Token)).ResolveExecutable("cmake")
                : ToolchainEnvironment.ResolveExecutable("cmake");
            string project = Path.Combine(root, "Runner.csproj");
            await File.WriteAllTextAsync(project, $$"""
                <Project Sdk="Microsoft.NET.Sdk">
                  <PropertyGroup>
                    <TargetFramework>net10.0</TargetFramework>
                    <LangVersion>14.0</LangVersion>
                    <OutputType>Exe</OutputType>
                    <ImplicitUsings>disable</ImplicitUsings>
                    <Nullable>enable</Nullable>
                  </PropertyGroup>
                  <ItemGroup>
                    <Reference Include="Inno.Build.Toolchains" HintPath="{{SecurityElement.Escape(typeof(ToolchainWorkingDirectory).Assembly.Location)}}" />
                    <Reference Include="Inno.Core.IO" HintPath="{{SecurityElement.Escape(typeof(FileLease).Assembly.Location)}}" />
                    <Reference Include="Inno.Build.Windows.Host" HintPath="{{SecurityElement.Escape(typeof(WindowsToolchainWorkspaceProvider).Assembly.Location)}}" />
                  </ItemGroup>
                </Project>
                """);
            await File.WriteAllTextAsync(Path.Combine(root, "Program.cs"), """
                using System;
                using System.Collections.Generic;
                using System.IO;
                using System.Threading;
                using Inno.Build.Toolchains;
                using Inno.Build.Windows.Host;

                Directory.CreateDirectory(args[0]);
                File.WriteAllText(Path.Combine(args[0], "InnoEngine.sln"), "fixture");
                Directory.CreateDirectory(Path.Combine(args[0], "build"));
                File.WriteAllText(Path.Combine(args[0], "build/fixture.csproj"), "<Project />");
                string source = Path.Combine(args[0], "Source");
                Directory.CreateDirectory(source);
                File.WriteAllText(Path.Combine(source, "CMakeLists.txt"),
                    "cmake_minimum_required(VERSION 3.20)\nproject(ExecutionPathFixture NONE)\n"
                    + "configure_file(input.txt result.txt COPYONLY)\n");
                File.WriteAllText(Path.Combine(source, "input.txt"), args[3]);
                var owner = new NativeComponentDescriptor("fixture", "native/fixture.csproj", "build/fixture.csproj");
                IToolchainWorkspaceProvider workspaces = OperatingSystem.IsWindows()
                    ? new WindowsToolchainWorkspaceProvider() : new PhysicalToolchainWorkspaceProvider();
                var tools = new NativeToolchainSelection("fixture-x64", new BuildHostDescriptor("Fixture", "x64", new BuildHostServices(workspaces)),
                    new Dictionary<string, string> { ["cmake"] = args[1] }, new Dictionary<string, string>(), [], [], new NativeLinkPlan(".native"), false);
                var context = new NativeBuildContext(args[0], "release").WithToolchain(tools)
                    .WithComponentOptions(owner, new(NativeLibraryKind.Shared));
                var recipe = new NativeBuildRecipe(owner, "fixture", "fixture-x64",
                    new[] { NativeBuildInput.FromPath(args[0], source) }, new[] { "fixture" });
                string? execution = null;
                using NativeBuildProduct product = await NativeArtifactPublisher.PublishAsync(context, recipe,
                    async (
                        buildContext,
                        output,
                        cancellation
                    ) => {
                        execution = buildContext.GetNativeBuildRoot(owner);
                        string build = await NativeCMakeExecutor.BuildAsync(buildContext, owner, source, null, [], cancellation);
                        File.Copy(Path.Combine(build, "result.txt"), Path.Combine(output, "result.txt"));
                    });
                bool hit = args[4] == "hit";
                if (File.ReadAllText(product.files[0]) != args[3] || (execution is null) != hit)
                    return 1;
                if (hit && context.statistics.nativeProcesses != 0)
                    return 2;
                File.WriteAllText(args[2], execution ?? "cache");
                return 0;
                """);
            var environment = DotNetSdkEnvironment.Create(dotnet);
            await ToolchainEnvironment.RunAsync(dotnet,
                ["build", project, "--disable-build-servers", "-m:1", "-nr:false", "-p:UseSharedCompilation=false"],
                repository, timeout.Token, environment);
            string physical = Path.Combine(root, "Workspace with spaces", new string('x', 70), new string('y', 70), new string('z', 70));
            string? firstExecution = null;
            (string value, string temporary, bool hit)[] calls =
            [
                ("first", "first-temp", false),
                ("second", "second-temp", false),
                ("third", "first-temp", false),
                ("third", "second-temp", true)
            ];
            foreach (var call in calls)
            {
                string temporary = Path.Combine(root, call.temporary);
                Directory.CreateDirectory(temporary);
                var processEnvironment = new Dictionary<string, string?>(environment, StringComparer.OrdinalIgnoreCase)
                {
                    ["TEMP"] = temporary,
                    ["TMP"] = temporary,
                    ["TMPDIR"] = temporary
                };
                string report = Path.Combine(root, call.value + (call.hit ? "-hit" : string.Empty) + ".txt");
                await ToolchainEnvironment.RunAsync(dotnet,
                    [Path.Combine(root, "bin/Debug/net10.0/Runner.dll"), physical, cmake, report, call.value, call.hit ? "hit" : "build"],
                    repository, timeout.Token, processEnvironment);
                string current = await File.ReadAllTextAsync(report);
                if (call.hit)
                    Assert.Equal("cache", current);
                else if (call.value == "first")
                    firstExecution = current;
                else if (call.value == "second" && OperatingSystem.IsWindows())
                    Assert.NotEqual(firstExecution, current);
                else
                    Assert.Equal(firstExecution, current);
            }
        }
        finally
        {
            Directory.Delete(root, recursive: true);
        }
    }
}
