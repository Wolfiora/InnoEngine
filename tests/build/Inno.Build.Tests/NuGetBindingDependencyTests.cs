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
        string root = ToolchainEnvironment.FindRepoRoot();
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
    public void RestoredRuntimePackageMatchesItsLockedContentIdentity()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        using JsonDocument assets = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root,
            "tests", "build", "Inno.Build.Tests", "obj", "project.assets.json")));
        using JsonDocument locked = JsonDocument.Parse(File.ReadAllBytes(Path.Combine(root,
            "tests", "build", "Inno.Build.Tests", "packages.lock.json")));
        JsonProperty runtime = assets.RootElement.GetProperty("libraries").EnumerateObject()
            .Single(static library => library.Name.StartsWith("BGCS.Runtime/", StringComparison.Ordinal));
        JsonElement lockEntry = locked.RootElement.GetProperty("dependencies").GetProperty("net9.0").GetProperty("BGCS.Runtime");
        Assert.Equal(runtime.Name["BGCS.Runtime/".Length..], lockEntry.GetProperty("resolved").GetString());
        Assert.Equal(runtime.Value.GetProperty("sha512").GetString(), lockEntry.GetProperty("contentHash").GetString());
    }

    [Theory]
    [InlineData("", "", "packages.lock.json")]
    [InlineData("windows-x64", "", "packages.windows-x64.lock.json")]
    [InlineData("windows-x64", "win-x64", "packages.windows-x64.win-x64.lock.json")]
    [InlineData("browser-wasm", "browser-wasm", "packages.browser-wasm.browser-wasm.lock.json")]
    public async Task TargetRestoresUseIndependentLockFiles(
        string target,
        string runtime,
        string expectedFile
    ) {
        string root = ToolchainEnvironment.FindRepoRoot();
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string project = Path.Combine(root, "src", "foundation", "core", "Inno.Core.IO", "Inno.Core.IO.csproj");
        string selected = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", project, "-nologo", "-nodeReuse:false", "-p:DesignTimeBuild=true",
                "-p:InnoNativeTarget=" + target, "-p:RuntimeIdentifier=" + runtime, "-getProperty:NuGetLockFilePath"],
            root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        Assert.Equal(expectedFile, Path.GetFileName(selected.Trim()));
    }
}
