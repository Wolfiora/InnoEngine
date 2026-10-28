using Inno.Testing.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Tests;

public sealed class StaticGraphRestoreTests
{
    [Fact]
    public async Task DevelopmentEntryRestoresItsDirectProductReferenceOwners()
    {
        string root = TestRepository.GetRoot(typeof(StaticGraphRestoreTests).Assembly);
        string owner = Path.Combine(Path.GetTempPath(), "InnoEntryRestoreTests");
        string fixture = PathBoundary.Resolve(owner, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        try
        {
            string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
                ?? ToolchainEnvironment.ResolveExecutable("dotnet");
            string entry = Path.Combine(root, "entry", "InnoEngine.Editor", "InnoEngine.Editor.csproj");
            string artifacts = Path.Combine(fixture, "managed");
            string[] common = ["-p:Configuration=Debug", "-p:ArtifactsPath=" + artifacts, "-nodeReuse:false"];
            using JsonDocument route = await EvaluateAsync(host, entry, common);
            JsonElement properties = route.RootElement.GetProperty("Properties");
            string product = properties.GetProperty("InnoProductRestoreProject").GetString()!;
            Assert.True(File.Exists(product));
            string[] productProperties = [.. common,
                "-p:InnoNativeTarget=" + properties.GetProperty("InnoNativeTarget").GetString(),
                "-p:InnoToolTarget=" + properties.GetProperty("InnoToolTarget").GetString(),
                "-p:InnoProductBuildProperties=" + properties.GetProperty("InnoProductBuildProperties").GetString()];
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            await ToolchainEnvironment.RunAsync(host,
                ["msbuild", entry, "-t:RestoreProductTargetGraph", "-v:quiet", .. common],
                root, cancellation.Token, DotNetSdkEnvironment.Create(host));

            var references = new HashSet<string>(StringComparer.Ordinal);
            foreach (JsonElement reference in route.RootElement.GetProperty("Items").GetProperty("ProjectReference").EnumerateArray())
                references.Add(reference.GetProperty("FullPath").GetString()!);
            using JsonDocument selectedProduct = await EvaluateAsync(host, product, productProperties);
            foreach (JsonElement reference in selectedProduct.RootElement.GetProperty("Items").GetProperty("ProjectReference").EnumerateArray())
            {
                if (!reference.TryGetProperty("PrivateAssets", out JsonElement privateAssets) || string.IsNullOrEmpty(privateAssets.GetString()))
                    references.Add(reference.GetProperty("FullPath").GetString()!);
            }
            Assert.NotEmpty(references);
            foreach (string reference in references)
            {
                using JsonDocument evaluated = await EvaluateAsync(host, reference, productProperties);
                string assets = evaluated.RootElement.GetProperty("Properties").GetProperty("ProjectAssetsFile").GetString()!;
                Assert.Equal(Path.GetFullPath(assets), PathBoundary.RequireContained(artifacts, assets));
                Assert.True(File.Exists(assets), $"The entry did not restore its selected reference owner: {assets}");
            }
        }
        finally
        {
            Directory.Delete(PathBoundary.RequireUnlinkedPath(owner, fixture), recursive: true);
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task ProductRestoreAcceptsLargeInheritedProperties(bool inheritSolution)
    {
        string root = TestRepository.GetRoot(typeof(StaticGraphRestoreTests).Assembly);
        string host = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        IReadOnlyDictionary<string, string?> environment = DotNetSdkEnvironment.Create(host);
        string owner = Path.Combine(Path.GetTempPath(), "InnoStaticGraphRestoreTests");
        string fixture = PathBoundary.Resolve(owner, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(fixture);
        string project = Path.Combine(root, "platforms", "WindowsX64", "editor", "Inno.Editor.WindowsX64", "Inno.Editor.WindowsX64.csproj");
        string driver = Path.Combine(fixture, "Restore.proj");
        string inheritedMetadata = new('R', 65536);
        try
        {
            await File.WriteAllTextAsync(driver, $"""
                <Project>
                  <Target Name="Verify">
                    <MSBuild Projects="{SecurityElement.Escape(project)}"
                             Targets="RestoreProductTargetGraph"
                             Properties="InnoRestoreFixtureMetadata={inheritedMetadata};SolutionPath={(inheritSolution ? SecurityElement.Escape(Path.Combine(root, "InnoEngine.sln")) : string.Empty)}"
                             BuildInParallel="false" />
                  </Target>
                </Project>
                """);
            using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(3));
            await ToolchainEnvironment.RunAsync(host,
                ["msbuild", driver, "-t:Verify", "-m:1", "-nodeReuse:false", "-nologo", "-v:minimal"],
                root, cancellation.Token, environment);
        }
        finally
        {
            Directory.Delete(PathBoundary.RequireUnlinkedPath(owner, fixture), recursive: true);
        }
    }

    private static async Task<JsonDocument> EvaluateAsync(
        string host,
        string project,
        IReadOnlyList<string> properties
    ) {
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(1));
        using var output = new StringWriter();
        using var error = new StringWriter();
        await ToolchainEnvironment.RunAsync(host,
            ["msbuild", project, "-nologo", .. properties,
                "-getProperty:ProjectAssetsFile,InnoProductRestoreProject,InnoNativeTarget,InnoToolTarget,InnoProductBuildProperties",
                "-getItem:ProjectReference"],
            Path.GetDirectoryName(project)!, cancellation.Token, DotNetSdkEnvironment.Create(host), output, error);
        return JsonDocument.Parse(output.ToString());
    }
}
