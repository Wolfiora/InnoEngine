using Inno.Testing.Source;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NuGetBindingDependencyTests
{
    [Theory]
    [InlineData("backends/ImGui/native/Inno.Native.ImGui/Inno.Native.ImGui.csproj")]
    [InlineData("backends/Sdl3/native/Inno.Native.Sdl3/Inno.Native.Sdl3.csproj")]
    [InlineData("backends/ImGui/runtime/Inno.Adapter.Presentation.ImGui.Sdl3/Inno.Adapter.Presentation.ImGui.Sdl3.csproj")]
    public async Task RuntimeConsumptionUsesAnExactPackageWithoutARepositorySourceReference(string project)
    {
        string root = TestRepository.GetRoot(typeof(NuGetBindingDependencyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", Path.Combine(root, project), "-nologo", "-nodeReuse:false", "-p:DesignTimeBuild=true",
                "-getItem:PackageReference,ProjectReference", "-getProperty:InnoBgcsPackageVersion"],
            root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement items = document.RootElement.GetProperty("Items");
        JsonElement runtime = items.GetProperty("PackageReference").EnumerateArray()
            .Single(static item => item.GetProperty("Identity").GetString() == "BGCS.Runtime");
        string version = document.RootElement.GetProperty("Properties").GetProperty("InnoBgcsPackageVersion").GetString()!;
        Assert.Equal("[" + version + "]", runtime.GetProperty("Version").GetString());
        Assert.DoesNotContain(items.GetProperty("ProjectReference").EnumerateArray(), static item =>
            item.GetProperty("Identity").GetString()!.Contains("BGCS", StringComparison.Ordinal));
        Assert.False(File.Exists(Path.Combine(root, "build", "msbuild", "Inno.Dependencies.props")));
    }

    [Fact]
    public async Task RestoredRuntimePackageMatchesItsLockedContentIdentity()
    {
        string root = TestRepository.GetRoot(typeof(NuGetBindingDependencyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string workspace = Path.Combine(Path.GetTempPath(), "InnoNuGetIdentity", Guid.NewGuid().ToString("N"));
        try
        {
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            string output = await ToolchainEnvironment.CaptureOutputAsync(host,
                ["msbuild", Path.Combine(root, "backends", "ImGui", "native", "Inno.Native.ImGui", "Inno.Native.ImGui.csproj"),
                    "-t:Restore", "-p:RestoreLockedMode=true", "-p:ArtifactsPath=" + workspace,
                    "-getProperty:ProjectAssetsFile,NuGetLockFilePath,TargetFramework", "-nologo", "-nr:false", "-v:quiet"],
                root, cancellation.Token, DotNetSdkEnvironment.Create(host));
            using JsonDocument evaluation = JsonDocument.Parse(output);
            JsonElement properties = evaluation.RootElement.GetProperty("Properties");
            using JsonDocument assets = JsonDocument.Parse(File.ReadAllBytes(properties.GetProperty("ProjectAssetsFile").GetString()!));
            using JsonDocument locked = JsonDocument.Parse(File.ReadAllBytes(properties.GetProperty("NuGetLockFilePath").GetString()!));
            JsonProperty runtime = assets.RootElement.GetProperty("libraries").EnumerateObject()
                .Single(static library => library.Name.StartsWith("BGCS.Runtime/", StringComparison.Ordinal));
            JsonElement lockEntry = locked.RootElement.GetProperty("dependencies")
                .GetProperty(properties.GetProperty("TargetFramework").GetString()!).GetProperty("BGCS.Runtime");
            Assert.Equal(runtime.Name["BGCS.Runtime/".Length..], lockEntry.GetProperty("resolved").GetString());
            Assert.Equal(runtime.Value.GetProperty("sha512").GetString(), lockEntry.GetProperty("contentHash").GetString());
        }
        finally
        {
            if (Directory.Exists(workspace))
                Directory.Delete(workspace, recursive: true);
        }
    }

    [Theory]
    [InlineData("", "", "packages.lock.json")]
    [InlineData("windows-x64", "", "packages.lock.json")]
    [InlineData("browser-wasm", "", "packages.lock.json")]
    [InlineData("macos-arm64", "", "packages.lock.json")]
    [InlineData("windows-x64", "win-x64", "packages.rid-win-x64.lock.json")]
    [InlineData("browser-wasm", "browser-wasm", "packages.rid-browser-wasm.link.lock.json")]
    public async Task OnlyActualRuntimeIdentitySeparatesOrdinaryLockFiles(
        string target,
        string runtime,
        string expectedFile
    ) {
        string root = TestRepository.GetRoot(typeof(NuGetBindingDependencyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string project = Path.Combine(root, "src", "foundation", "core", "Inno.Core.IO", "Inno.Core.IO.csproj");
        string selected = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", project, "-nologo", "-nodeReuse:false", "-p:DesignTimeBuild=true",
                "-p:InnoNativeTarget=" + target, "-p:RuntimeIdentifier=" + runtime, "-getProperty:NuGetLockFilePath"],
            root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        Assert.Equal(expectedFile, Path.GetFileName(selected.Trim()));
    }
}
