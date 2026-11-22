using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Testing.Source;

namespace Inno.Build.Cli.Integration.Tests;

public sealed class NativeDependencyDescriptionTests
{
    [Theory]
    [InlineData("windows-x64", "editor,player,shader-tools")]
    [InlineData("macos-arm64", "editor,player,shader-tools")]
    [InlineData("browser-wasm", "player")]
    public async Task ProductDiscoveryUsesTheTargetContributionWithoutPreparingItsNativeSdk(
        string target,
        string products
    ) {
        string root = TestRepository.GetRoot(typeof(NativeDependencyDescriptionTests).Assembly);
        string output = Path.Combine(root, "artifacts", "validation", "native-description", Guid.NewGuid().ToString("N"), "products.json");
        Directory.CreateDirectory(Path.GetDirectoryName(output)!);
        string dotnet = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH")
            ?? Path.Combine(Environment.GetEnvironmentVariable("DOTNET_ROOT")!, OperatingSystem.IsWindows() ? "dotnet.exe" : "dotnet");
        int result = await BuildApplication.RunAsync([
            "native-dependencies",
            "--operation",
            "describe",
            "--engine-root",
            root,
            "--tools-target",
            OperatingSystem.IsWindows() ? "windows-x64" : "macos-arm64",
            "--target",
            target,
            "--dotnet",
            dotnet,
            "--output",
            output
        ], CancellationToken.None);
        Assert.Equal(0, result);
        using JsonDocument description = JsonDocument.Parse(File.ReadAllText(output));
        Assert.Equal(target, description.RootElement.GetProperty("target").GetString());
        Assert.Equal(
            products.Split(','),
            description.RootElement.GetProperty("products").EnumerateArray().Select(static value => value.GetString())
        );
    }
}
