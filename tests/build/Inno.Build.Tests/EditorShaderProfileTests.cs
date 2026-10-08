using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class EditorShaderProfileTests
{
    [Fact]
    public async Task SdkAddedReferencesRetainTheProductTargetAndShaderProfile()
    {
        string root = ToolchainEnvironment.FindRepoRoot();
        string profile = Path.Combine(root, "platforms", "Windows", "build", "Inno.Build.Windows", "EditorProduct.props");
        string backend = Path.Combine(root, "backends", "Sdl3", "runtime",
            "Inno.Adapter.Platform.Sdl3", "Inno.Adapter.Platform.Sdl3.csproj");
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", backend, "-nologo", "-nodeReuse:false", "-t:IncludeTransitiveProjectReferences",
                "-getItem:ProjectReference", "-p:InnoProductBuildProperties=" + profile,
                "-p:InnoNativeTarget=windows-x64"], root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        using JsonDocument document = JsonDocument.Parse(output);
        JsonElement references = document.RootElement.GetProperty("Items").GetProperty("ProjectReference");
        Assert.Contains(references.EnumerateArray(), reference => reference.GetProperty("Filename").GetString() == "BGCS.Runtime");
        Assert.All(references.EnumerateArray().Where(reference =>
            !reference.TryGetProperty("OutputItemType", out JsonElement kind) || kind.GetString() != "Analyzer"), reference =>
        {
            string properties = reference.GetProperty("AdditionalProperties").GetString()!;
            Assert.Contains("InnoNativeTarget=windows-x64", properties);
            Assert.Contains("InnoProductBuildProperties=" + profile, properties);
        });
        Assert.All(references.EnumerateArray().Where(reference =>
            reference.TryGetProperty("OutputItemType", out JsonElement kind) && kind.GetString() == "Analyzer"), reference =>
        {
            Assert.Equal("all", reference.GetProperty("PrivateAssets").GetString());
            string removed = reference.GetProperty("GlobalPropertiesToRemove").GetString()!;
            Assert.Contains("InnoNativeTarget", removed);
            Assert.Contains("InnoProductBuildProperties", removed);
            Assert.Contains("RuntimeIdentifier", removed);
        });
    }

    [Theory]
    [InlineData("Windows", "windows-x64", 4)]
    [InlineData("MacOS", "macos-arm64", 3)]
    [InlineData("Browser", "browser-wasm", 1)]
    public async Task SharedCompositionShadersConsumeExplicitProductFacts(
        string platform,
        string shaderPlatform,
        int apiCount
    ) {
        string root = ToolchainEnvironment.FindRepoRoot();
        string profile = Path.Combine(root, "platforms", platform, "build", "Inno.Build." + platform, "ProductBuild.props");
        string backend = Path.Combine(root, "backends", "Bgfx", "runtime",
            "Inno.Adapter.Rendering.Bgfx", "Inno.Adapter.Rendering.Bgfx.csproj");
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", backend, "-nologo", "-nodeReuse:false", "-getProperty:InnoBgfxShaderTarget",
                "-getItem:_CompositionShaderApi", "-p:InnoProductBuildProperties=" + profile,
                "-p:InnoNativeTarget=fixture-new-cpu"], root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        using JsonDocument document = JsonDocument.Parse(output);
        Assert.Equal(shaderPlatform, document.RootElement.GetProperty("Properties")
            .GetProperty("InnoBgfxShaderTarget").GetString());
        Assert.Equal(apiCount, document.RootElement.GetProperty("Items").GetProperty("_CompositionShaderApi").GetArrayLength());
    }

    [Theory]
    [InlineData("Windows", "windows-x64", "fixture-new-cpu", 4)]
    [InlineData("MacOS", "macos-arm64", "fixture-new-cpu", 3)]
    public async Task BackendConsumesAProductProfileWithoutSelectingAKnownNativeTarget(
        string platform,
        string shaderPlatform,
        string nativeTarget,
        int apiCount
    ) {
        string root = ToolchainEnvironment.FindRepoRoot();
        string profile = Path.Combine(root, "platforms", platform, "build", "Inno.Build." + platform, "EditorProduct.props");
        string backend = Path.Combine(root, "backends", "ImGui", "runtime",
            "Inno.Adapter.Presentation.ImGui.Bgfx", "Inno.Adapter.Presentation.ImGui.Bgfx.csproj");
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", backend, "-nologo", "-nodeReuse:false", "-getProperty:InnoImGuiShaderTarget",
                "-getItem:_ImGuiShaderApi,ProjectReference", "-p:InnoProductBuildProperties=" + profile,
                "-p:InnoNativeTarget=" + nativeTarget], root, CancellationToken.None, DotNetSdkEnvironment.Create(host));
        using JsonDocument document = JsonDocument.Parse(output);
        Assert.Equal(shaderPlatform, document.RootElement.GetProperty("Properties")
            .GetProperty("InnoImGuiShaderTarget").GetString());
        JsonElement items = document.RootElement.GetProperty("Items");
        Assert.Equal(apiCount, items.GetProperty("_ImGuiShaderApi").GetArrayLength());
        Assert.All(items.GetProperty("ProjectReference").EnumerateArray()
            .Where(reference => !reference.TryGetProperty("OutputItemType", out JsonElement kind)
                || kind.GetString() != "Analyzer"), reference =>
            Assert.Contains("InnoProductBuildProperties=" + profile,
                reference.GetProperty("AdditionalProperties").GetString()));
    }
}
