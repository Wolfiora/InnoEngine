using System;
using System.IO;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Composition;
using Inno.Build.Distribution.Standard;
using Inno.Build.SupportPacks;
using Inno.Testing.Source;

namespace Inno.Build.BrowserWasm32.Integration.Tests;

[SupportedOSPlatform("windows")]
public sealed class BrowserDependencyConsumptionTests
{
    [Fact]
    public async Task MatchingStaticDependenciesProduceARealManagedPlayerSupportPack()
    {
        string root = TestRepository.GetRoot(typeof(BrowserDependencyConsumptionTests).Assembly);
        string manifest = Environment.GetEnvironmentVariable("INNO_TESTING_NATIVE_DEPENDENCIES")
            ?? throw new InvalidOperationException("Browser product acceptance requires an explicit consume-only native selection.");
        Assert.True(File.Exists(manifest), "The selected immutable native dependency manifest must exist.");
        string dotnet = StandardBuildEnvironment.ResolveManagedHost();
        BuildCompositionContext context = StandardBuildEnvironment.Capture(
            AppContext.BaseDirectory,
            new BuildTargetID("windows-x64"),
            dotnet,
            manifest
        );
        BuildDistribution distribution = StandardBuildDistribution.Create(context).build;
        var target = new BuildTargetID("browser-wasm");
        string output = Path.Combine(root, "artifacts", "validation", "browser-support-pack");
        using var cancellation = new CancellationTokenSource(TimeSpan.FromMinutes(20));
        using PlayerSupportPackPublication pack = await distribution.supportPacks.PublishAsync(
            root,
            output,
            target,
            dotnet,
            manifest,
            cancellation.Token
        );
        distribution.platforms.Single(platform => platform.descriptor.id == target).supportPackSource.Validate(pack.directory);
        Assert.True(File.Exists(Path.Combine(pack.directory, "References", "Inno.Player.Runtime.dll")));
        Assert.False(File.Exists(Path.Combine(pack.directory, "References", "Inno.Player.BrowserWasm32.dll")));
        Assert.True(File.Exists(Path.Combine(pack.directory, "PlayerLink", "Player.csproj")));
        Assert.True(File.Exists(Path.Combine(pack.directory, "PlayerLink", "Program.cs")));
        Assert.True(File.Exists(Path.Combine(pack.directory, "PlayerLink", "BrowserWasm32PlayerComposition.cs")));
        Assert.NotEmpty(Directory.GetFiles(Path.Combine(pack.directory, "PlayerLink", "Native"), "*.a", SearchOption.AllDirectories));
    }
}
