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

public sealed class HostRuntimeClosureBuildTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task RuntimeDiscoveryReceivesPrivateDependenciesWithoutExposingTheirCompileAssets(bool exposeClosure)
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoHostClosure", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        root = new PhysicalPathResolver().Resolve(root);
        string engine = TestRepository.GetRoot(typeof(HostRuntimeClosureBuildTests).Assembly);
        string dotnet = ToolchainEnvironment.ResolveExecutable("dotnet");
        string artifacts = Path.Combine(root, "managed");
        try
        {
            string extension = WriteProject(root, "Inno.Fixture.Extension", string.Empty,
                "namespace Inno.Fixture.Extension; public sealed class Marker { }");
            string library = WriteProject(root, "Inno.Fixture.Host",
                $"<PropertyGroup><InnoExposeRuntimeClosure>{exposeClosure.ToString().ToLowerInvariant()}</InnoExposeRuntimeClosure></PropertyGroup>"
                + $"<ItemGroup><ProjectReference Include=\"{Escape(extension)}\" PrivateAssets=\"compile\" /></ItemGroup>",
                "namespace Inno.Fixture.Host; public sealed class Marker { }");
            string adapter = Path.Combine(engine, "backends", "DotNet", "runtime", "Inno.Adapter.Modules.DotNet",
                "Inno.Adapter.Modules.DotNet.csproj");
            string application = WriteProject(root, "Host",
                "<PropertyGroup><OutputType>Exe</OutputType></PropertyGroup>"
                + $"<ItemGroup><ProjectReference Include=\"{Escape(library)}\" />"
                + $"<ProjectReference Include=\"{Escape(adapter)}\" /></ItemGroup>",
                """
                using System.Linq;
                using Inno.Adapter.Modules.DotNet;
                internal static class Program
                {
                    private static int Main()
                    {
                        using var source = new DotNetAssemblyCatalogSource(typeof(Inno.Fixture.Host.Marker).Assembly);
                        return source.GetAssemblies().Any(assembly => assembly.GetName().Name == "Inno.Fixture.Extension") ? 0 : 13;
                    }
                }
                """);
            string[] properties = ["-p:ArtifactsPath=" + artifacts, "-p:UseSharedCompilation=false"];
            await ToolchainEnvironment.RunAsync(dotnet,
                ["build", application, "-c", "Release", "--disable-build-servers", "-m:1", "-nr:false", ..properties],
                root, CancellationToken.None, DotNetSdkEnvironment.Create(dotnet));
            string executable = (await ToolchainEnvironment.CaptureOutputAsync(dotnet,
                ["msbuild", application, "-nologo", "-m:1", "-nr:false", "-p:Configuration=Release",
                    "-getProperty:TargetPath", ..properties], root, CancellationToken.None,
                DotNetSdkEnvironment.Create(dotnet))).Trim();
            string output = Path.GetDirectoryName(executable)!;
            Assert.Equal(exposeClosure, File.Exists(Path.Combine(output, "Inno.Fixture.Extension.dll")));
            Assert.Equal(exposeClosure, File.Exists(Path.Combine(output, "Inno.Fixture.Host.deps.json")));
            if (exposeClosure)
                await ToolchainEnvironment.RunAsync(dotnet, [executable], root, CancellationToken.None,
                    DotNetSdkEnvironment.Create(dotnet));
            else
            {
                InvalidOperationException unavailable = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                    ToolchainEnvironment.RunAsync(dotnet, [executable], root, CancellationToken.None,
                        DotNetSdkEnvironment.Create(dotnet)));
                Assert.EndsWith("exit code 13.", unavailable.Message);
            }

            File.WriteAllText(Path.Combine(Path.GetDirectoryName(application)!, "ForbiddenCompile.cs"),
                "internal sealed class ForbiddenCompile { private readonly Inno.Fixture.Extension.Marker m_value = new(); }");
            string compilerLog = Path.Combine(root, "negative-compile.log");
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                ToolchainEnvironment.RunAsync(dotnet,
                    ["build", application, "-c", "Release", "--no-restore", "--disable-build-servers", "-m:1", "-nr:false",
                        "-flp:logfile=" + compilerLog + ";verbosity=normal", ..properties],
                    root, CancellationToken.None, DotNetSdkEnvironment.Create(dotnet)));
            Assert.Contains("CS0234", File.ReadAllText(compilerLog));
        }
        finally
        {
            if (Directory.Exists(root))
                Directory.Delete(root, recursive: true);
        }
    }

    private static string WriteProject(
        string root,
        string name,
        string items,
        string source
    ) {
        string directory = Path.Combine(root, name);
        Directory.CreateDirectory(directory);
        string project = Path.Combine(directory, name + ".csproj");
        string engine = TestRepository.GetRoot(typeof(HostRuntimeClosureBuildTests).Assembly);
        File.WriteAllText(project, $"""
            <Project>
              <Import Project="Sdk.props" Sdk="Microsoft.NET.Sdk" />
              <PropertyGroup>
                <TargetFramework>net10.0</TargetFramework>
                <LangVersion>14.0</LangVersion>
                <ImplicitUsings>disable</ImplicitUsings>
                <Nullable>enable</Nullable>
                <IsTestSupportProject>true</IsTestSupportProject>
                <InnoAssemblyDomain>InnoInternal</InnoAssemblyDomain>
                <InnoAssemblyScope>Editor</InnoAssemblyScope>
              </PropertyGroup>
              {items}
              <Import Project="{Escape(Path.Combine(engine, "Directory.Build.targets"))}" />
              <Import Project="Sdk.targets" Sdk="Microsoft.NET.Sdk" />
            </Project>
            """);
        File.WriteAllText(Path.Combine(directory, "Program.cs"), source);
        return project;
    }

    private static string Escape(string path) => SecurityElement.Escape(path)!;
}
