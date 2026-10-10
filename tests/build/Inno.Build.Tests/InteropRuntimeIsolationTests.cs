using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class InteropRuntimeIsolationTests
{
    [Fact]
    public async Task ExternalConsumerBuildPreparesItsIsolatedRuntimeRestore()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        IReadOnlyDictionary<string, string?> environment = DotNetSdkEnvironment.Create(host);
        string owner = Path.Combine(Path.GetTempPath(), "InnoInteropConsumerTests");
        string fixture = PathBoundary.Resolve(owner, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        string restore = Path.Combine(fixture, "restore") + Path.DirectorySeparatorChar;
        string dependency = Path.Combine(root, "backends", "Sdl3", "runtime", "Inno.Adapter.Platform.Sdl3", "Inno.Adapter.Platform.Sdl3.csproj");
        string project = Path.Combine(fixture, "Consumer.csproj");
        await File.WriteAllTextAsync(project, $"""
            <Project Sdk="Microsoft.NET.Sdk">
              <PropertyGroup>
                <TargetFramework>net9.0</TargetFramework>
                <ImplicitUsings>disable</ImplicitUsings>
              </PropertyGroup>
              <ItemGroup>
                <ProjectReference Include="{SecurityElement.Escape(dependency)}" />
              </ItemGroup>
            </Project>
            """);
        await File.WriteAllTextAsync(Path.Combine(fixture, "Consumer.cs"), "public sealed class Consumer { }");
        try
        {
            Assert.False(File.Exists(Path.Combine(restore, "project.assets.json")));
            using StringWriter output = new();
            using StringWriter error = new();
            Exception? failure = await Record.ExceptionAsync(() => ToolchainEnvironment.RunAsync(host,
                ["build", project, "-m:1", "-nodeReuse:false", "--disable-build-servers",
                    "-p:UseSharedCompilation=false", "-p:RestoreUseStaticGraphEvaluation=false",
                    "-p:InnoInteropRuntimeRestore=" + restore, "-p:DebugType=None", "-p:DebugSymbols=false",
                    "-p:InnoNativeBuildFingerprint=" + Path.GetFileName(fixture)],
                fixture, CancellationToken.None, environment, output, error));
            Assert.True(failure is null, output.ToString() + error.ToString() + failure);
            Assert.True(File.Exists(Path.Combine(restore, "project.assets.json")));
            Assert.True(File.Exists(Path.Combine(fixture, "bin", "Debug", "net9.0", "Consumer.dll")));
        }
        finally
        {
            Directory.Delete(PathBoundary.RequireUnlinkedPath(owner, fixture), true);
        }
    }

    [Theory]
    [InlineData("backends/Sdl3/runtime/Inno.Adapter.Platform.Sdl3/Inno.Adapter.Platform.Sdl3.csproj")]
    [InlineData("src/composition/editor/presentation/Inno.Editor.ImGui/Inno.Editor.ImGui.csproj")]
    public async Task SdkAddedRuntimeReferencesRetainConsumerIsolation(string relativeProject)
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", Path.Combine(root, relativeProject), "-nologo", "-nodeReuse:false",
                "-t:IncludeTransitiveProjectReferences", "-getItem:ProjectReference",
                "-p:InnoNativeTarget=windows-x64", "-p:DebugType=None", "-p:DebugSymbols=false"],
            root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement reference = document.RootElement.GetProperty("Items").GetProperty("ProjectReference")
            .EnumerateArray().Single(item => item.GetProperty("Filename").GetString() == "BGCS.Runtime");
        string metadata = reference.GetProperty("AdditionalProperties").GetString()!;
        string profile = metadata.Split(';').Last(value => value.StartsWith("ArtifactsPath=", StringComparison.Ordinal))[14..];
        Assert.StartsWith(Path.Combine(root, "artifacts", "managed", "interop").Replace('\\', '/'), profile.Replace('\\', '/'));
        Assert.Contains("/windows-x64/", profile.Replace('\\', '/'));
        Assert.Contains("d-None-false", profile);
        Assert.Contains("MSBuildProjectExtensionsPath=", metadata);
    }

    [Fact]
    public async Task RestoreRetainsTheReferencedRuntimeArtifactProfile()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string project = Path.Combine(root, "backends", "Text", "native", "Inno.Native.Text", "Inno.Native.Text.csproj");
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        IReadOnlyDictionary<string, string?> environment = DotNetSdkEnvironment.Create(host);
        string fingerprint = "restore-contract-" + Guid.NewGuid().ToString("N");
        string[] properties = ["-p:Configuration=Release", "-p:InnoNativeTarget=windows-x64",
            "-p:InnoNativeBuildFingerprint=" + fingerprint, "-p:DebugType=None", "-p:DebugSymbols=false"];
        string referenceOutput = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", project, "-nologo", "-nodeReuse:false", "-getItem:ProjectReference", .. properties],
            root, CancellationToken.None, environment);
        using JsonDocument document = JsonDocument.Parse(referenceOutput);
        JsonElement reference = document.RootElement.GetProperty("Items").GetProperty("ProjectReference")
            .EnumerateArray().Single(item => item.GetProperty("Filename").GetString() == "BGCS.Runtime");
        string propertiesMetadata = reference.GetProperty("AdditionalProperties").GetString()!;
        string profile = propertiesMetadata.Split(';')
            .Single(value => value.StartsWith("ArtifactsPath=", StringComparison.Ordinal))[14..];
        string restore = propertiesMetadata.Split(';')
            .Single(value => value.StartsWith("MSBuildProjectExtensionsPath=", StringComparison.Ordinal))[29..];
        Assert.NotEqual(profile.TrimEnd('/', '\\'), restore.TrimEnd('/', '\\'));

        await ToolchainEnvironment.RunAsync(host, ["restore", project, "-nodeReuse:false", .. properties],
            root, CancellationToken.None, environment);

        Assert.True(File.Exists(Path.Combine(restore, "project.assets.json")));
        string assets = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", reference.GetProperty("FullPath").GetString()!, "-nologo", "-nodeReuse:false",
                "-getProperty:ProjectAssetsFile", "-p:UseArtifactsOutput=true", "-p:ArtifactsPath=" + profile,
                "-p:MSBuildProjectExtensionsPath=" + restore], root, CancellationToken.None, environment);
        Assert.Equal(Path.GetFullPath(Path.Combine(restore, "project.assets.json")), Path.GetFullPath(assets.Trim()));
    }

    [Fact]
    public async Task ReferencedRuntimeSeparatesSdkTargetAbiAndCompilationProfiles()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string project = Path.Combine(root, "backends", "Text", "native", "Inno.Native.Text", "Inno.Native.Text.csproj");
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        IReadOnlyDictionary<string, string?> environment = DotNetSdkEnvironment.Create(host);
        string baseline = await Artifacts("None", "windows-x64", "", false, false);
        Assert.StartsWith(Path.Combine(root, "artifacts").Replace('\\', '/'), baseline.Replace('\\', '/'));
        Assert.NotEqual(baseline, await Artifacts("portable", "windows-x64", "", false, false));
        Assert.NotEqual(baseline, await Artifacts("None", "browser-wasm", "", false, false));
        Assert.NotEqual(baseline, await Artifacts("None", "windows-x64", "selected-abi", false, false));
        Assert.NotEqual(await Artifacts("None", "windows-x64", "", true, false),
            await Artifacts("None", "windows-x64", "", false, true));

        async Task<string> Artifacts(
            string debugType,
            string target,
            string fingerprint,
            bool aot,
            bool selfContained
        ) {
            string output = await ToolchainEnvironment.CaptureOutputAsync(host,
                ["msbuild", project, "-nologo", "-nodeReuse:false", "-getItem:ProjectReference",
                    "-p:Configuration=Release", "-p:DebugType=" + debugType, "-p:InnoNativeTarget=" + target,
                    "-p:InnoNativeBuildFingerprint=" + fingerprint, "-p:PublishAot=" + aot,
                    "-p:SelfContained=" + selfContained], root, CancellationToken.None, environment);
            using JsonDocument document = JsonDocument.Parse(output);
            JsonElement reference = document.RootElement.GetProperty("Items").GetProperty("ProjectReference")
                .EnumerateArray().Single(item => item.GetProperty("Filename").GetString() == "BGCS.Runtime");
            string properties = reference.GetProperty("AdditionalProperties").GetString()!;
            Assert.Contains("UseArtifactsOutput=true", properties);
            return properties.Split(';').Single(value => value.StartsWith("ArtifactsPath=", StringComparison.Ordinal))[14..];
        }
    }
}
