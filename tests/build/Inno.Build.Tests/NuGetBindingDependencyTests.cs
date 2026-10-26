using Inno.Testing.Source;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
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

    /// <summary>
    /// Verifies the restored runtime version, signed archive and NuGet content identity without a source manifest.
    /// </summary>
    /// <returns>
    /// Completion after the official restore output and installed package metadata agree.
    /// </returns>
    [Fact]
    public async Task RestoredRuntimePackageMatchesItsDeclaredVersionAndNuGetContentIdentity()
    {
        string root = TestRepository.GetRoot(typeof(NuGetBindingDependencyTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string workspace = Path.Combine(Path.GetTempPath(), "InnoNuGetIdentity", Guid.NewGuid().ToString("N"));
        try
        {
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            string output = await ToolchainEnvironment.CaptureOutputAsync(host,
                ["msbuild", Path.Combine(root, "backends", "ImGui", "native", "Inno.Native.ImGui", "Inno.Native.ImGui.csproj"),
                    "-t:Restore", "-p:ArtifactsPath=" + workspace,
                    "-getProperty:ProjectAssetsFile,InnoBgcsPackageVersion", "-nologo", "-nr:false", "-v:quiet"],
                root, cancellation.Token, DotNetSdkEnvironment.Create(host));
            using JsonDocument evaluation = JsonDocument.Parse(output);
            JsonElement properties = evaluation.RootElement.GetProperty("Properties");
            using JsonDocument assets = JsonDocument.Parse(File.ReadAllBytes(properties.GetProperty("ProjectAssetsFile").GetString()!));
            JsonProperty runtime = assets.RootElement.GetProperty("libraries").EnumerateObject()
                .Single(static library => library.Name.StartsWith("BGCS.Runtime/", StringComparison.Ordinal));
            string version = properties.GetProperty("InnoBgcsPackageVersion").GetString()!;
            Assert.Equal("BGCS.Runtime/" + version, runtime.Name);
            string packagePath = runtime.Value.GetProperty("path").GetString()!;
            string archive = assets.RootElement.GetProperty("packageFolders").EnumerateObject()
                .Select(folder => Path.Combine(folder.Name, packagePath, "bgcs.runtime." + version + ".nupkg"))
                .Single(File.Exists);
            await using FileStream package = File.OpenRead(archive);
            string archiveHash = Convert.ToBase64String(await SHA512.HashDataAsync(package));
            Assert.Equal(File.ReadAllText(archive + ".sha512").Trim(), archiveHash);
            // NuGet's content identity excludes signature metadata from the signed archive.
            using JsonDocument metadata = JsonDocument.Parse(File.ReadAllBytes(
                Path.Combine(Path.GetDirectoryName(archive)!, ".nupkg.metadata")));
            Assert.Equal(runtime.Value.GetProperty("sha512").GetString(),
                metadata.RootElement.GetProperty("contentHash").GetString());
        }
        finally
        {
            if (Directory.Exists(workspace))
                Directory.Delete(workspace, recursive: true);
        }
    }

}
