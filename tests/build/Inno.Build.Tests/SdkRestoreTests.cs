using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Inno.Testing.Source;
using Xunit;

namespace Inno.Build.Tests;

/// <summary>
/// Exercises standard SDK dependency resolution and the development entry's product routing.
/// </summary>
public sealed class SdkRestoreTests
{
    /// <summary>
    /// Resolves the requested SDK target without publishing dependency manifests into source directories.
    /// </summary>
    /// <param name="target">
    /// The native product label, independent of NuGet's runtime identifier.
    /// </param>
    /// <param name="runtime">
    /// The actual SDK runtime identifier, or an empty value for a neutral restore.
    /// </param>
    /// <param name="aheadOfTime">
    /// Whether the SDK must resolve its NativeAOT compiler package.
    /// </param>
    /// <param name="trimmed">
    /// Whether the SDK must resolve its trimming tool package.
    /// </param>
    /// <returns>
    /// Completion after SDK assets, tool dependencies and source ownership have been verified.
    /// </returns>
    [Theory]
    [InlineData("", "", false, false)]
    [InlineData("windows-x64", "win-x64", false, false)]
    [InlineData("macos-arm64", "osx-arm64", false, false)]
    [InlineData("browser-wasm", "", false, false)]
    [InlineData("windows-x64", "win-x64", true, true)]
    [InlineData("windows-x64", "win-x64", false, true)]
    public async Task StandardRestoreResolvesSdkDependenciesWithoutWritingSourceManifests(
        string target,
        string runtime,
        bool aheadOfTime,
        bool trimmed
    ) {
        string root = TestRepository.GetRoot(typeof(SdkRestoreTests).Assembly);
        string project = Path.Combine(root, "src", "foundation", "core", "Inno.Core.IO", "Inno.Core.IO.csproj");
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        string workspace = Path.Combine(Path.GetTempPath(), "InnoSdkRestore", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(workspace);
        workspace = new PhysicalPathResolver().Resolve(workspace);
        try
        {
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            string output = await ToolchainEnvironment.CaptureOutputAsync(host,
                ["msbuild", project, "-t:Restore", "-p:ArtifactsPath=" + workspace,
                    "-p:InnoNativeTarget=" + target, "-p:RuntimeIdentifier=" + runtime,
                    "-p:PublishAot=" + aheadOfTime, "-p:PublishTrimmed=" + trimmed,
                    "-getProperty:ProjectAssetsFile", "-nologo", "-nr:false", "-v:quiet"],
                root, cancellation.Token, DotNetSdkEnvironment.Create(host));
            string assetsPath = output.Trim();
            Assert.StartsWith(Path.GetFullPath(workspace) + Path.DirectorySeparatorChar,
                Path.GetFullPath(assetsPath), StringComparison.Ordinal);
            using JsonDocument assets = JsonDocument.Parse(File.ReadAllBytes(assetsPath));
            JsonProperty[] resolved = assets.RootElement.GetProperty("targets").EnumerateObject().ToArray();
            Assert.NotEmpty(resolved);
            if (runtime.Length != 0)
                Assert.Contains(resolved, entry => entry.Name.EndsWith("/" + runtime, StringComparison.Ordinal));
            if (aheadOfTime)
                Assert.Contains(assets.RootElement.GetProperty("libraries").EnumerateObject(),
                    static entry => entry.Name.StartsWith("Microsoft.DotNet.ILCompiler/", StringComparison.Ordinal));
            if (trimmed)
                Assert.Contains(assets.RootElement.GetProperty("libraries").EnumerateObject(),
                    static entry => entry.Name.StartsWith("Microsoft.NET.ILLink.Tasks/", StringComparison.Ordinal));
            Assert.Empty(Directory.EnumerateFiles(Path.GetDirectoryName(project)!, "packages*.lock.json"));
            Assert.Empty(Directory.EnumerateFiles(workspace, "packages*.lock.json", SearchOption.AllDirectories));
        }
        finally
        {
            if (Directory.Exists(workspace))
                Directory.Delete(workspace, recursive: true);
        }
    }

    /// <summary>
    /// Preserves each supported SDK host's explicit product properties on runtime project references.
    /// </summary>
    /// <param name="sdkHost">
    /// The SDK host identity used to evaluate the development entry.
    /// </param>
    /// <param name="expectedTarget">
    /// The native target owned by the selected host product.
    /// </param>
    /// <returns>
    /// Completion after the SDK has evaluated the expected product and reference context.
    /// </returns>
    [Theory]
    [InlineData("win-x64", "windows-x64")]
    [InlineData("osx-arm64", "macos-arm64")]
    public async Task EditorHostRoutingPreservesTheProductReferenceContext(
        string sdkHost,
        string expectedTarget
    ) {
        string root = TestRepository.GetRoot(typeof(SdkRestoreTests).Assembly);
        string host = ToolchainEnvironment.ResolveExecutable("dotnet");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(60));
        string output = await ToolchainEnvironment.CaptureOutputAsync(host,
            ["msbuild", Path.Combine(root, "entry", "InnoEngine.Editor", "InnoEngine.Editor.csproj"),
                "-p:NETCoreSdkRuntimeIdentifier=" + sdkHost,
                "-getProperty:InnoNativeTarget,InnoProductBuildProperties", "-getItem:ProjectReference", "-nologo", "-nr:false"],
            root, cancellation.Token, DotNetSdkEnvironment.Create(host));
        using JsonDocument evaluation = JsonDocument.Parse(output);
        JsonElement properties = evaluation.RootElement.GetProperty("Properties");
        Assert.Equal(expectedTarget, properties.GetProperty("InnoNativeTarget").GetString());
        string product = properties.GetProperty("InnoProductBuildProperties").GetString()!;
        Assert.True(File.Exists(product));
        foreach (JsonElement reference in evaluation.RootElement.GetProperty("Items").GetProperty("ProjectReference").EnumerateArray())
        {
            if (reference.TryGetProperty("OutputItemType", out JsonElement role) && role.GetString() == "Analyzer")
                continue;
            string context = reference.GetProperty("AdditionalProperties").GetString()!;
            Assert.Contains("InnoNativeTarget=" + expectedTarget, context);
            Assert.Contains("InnoProductBuildProperties=" + product, context);
        }
    }
}
