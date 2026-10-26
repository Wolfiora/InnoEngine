using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Toolchains;

namespace Inno.Build.Tests;

public sealed class ProductNativeCacheLookupTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeBatch", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("fixture", "native/fixture.csproj", "build/fixture.csproj");
    private int m_builds;

    public ProductNativeCacheLookupTests()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "build"));
        File.WriteAllText(Path.Combine(m_root, "build", "fixture.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(m_root, "shared.input"), "shared");
    }

    [Fact]
    public async Task CompleteHitUsesOneFreshUnionAndDoesNotInvokeProducers()
    {
        ProductNativeBuildPlan plan = Plan();
        using (await new ComponentNativeProductExecutor().BuildAsync(Context(), plan, CancellationToken.None)) { }
        Assert.Equal(2, m_builds);
        NativeBuildContext hot = Context();
        using ProductNativeBuildResult result = await new ComponentNativeProductExecutor().BuildAsync(hot, plan, CancellationToken.None);
        Assert.Equal(2, m_builds);
        NativeBuildPhaseStatistics phase = Assert.Single(hot.statistics.phases.Where(static phase => phase.phase == "native-product-lock"));
        Assert.Equal(1, phase.files);
        Assert.True(phase.reusedReads > 0);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task DamagedMemberFallsBackWithoutReturningPartialProducts()
    {
        ProductNativeBuildPlan plan = Plan();
        using (ProductNativeBuildResult initial = await new ComponentNativeProductExecutor().BuildAsync(Context(), plan, CancellationToken.None))
            File.WriteAllText(initial[1].files[0], "damaged");
        using ProductNativeBuildResult repaired = await new ComponentNativeProductExecutor().BuildAsync(Context(), plan, CancellationToken.None);
        Assert.Equal(3, m_builds);
        Assert.All(repaired, static product => Assert.Equal("complete", File.ReadAllText(product.files[0])));
    }

    [Fact]
    public async Task ConcurrentHotRequestsShareReadOwnershipWithoutWorkspaceDeadlock()
    {
        ProductNativeBuildPlan plan = Plan();
        using (await new ComponentNativeProductExecutor().BuildAsync(Context(), plan, CancellationToken.None)) { }
        using var cancellation = new CancellationTokenSource(TimeSpan.FromSeconds(10));
        ProductNativeBuildResult[] results = await Task.WhenAll(new ComponentNativeProductExecutor().BuildAsync(Context(), plan, cancellation.Token), new ComponentNativeProductExecutor().BuildAsync(Context(), plan, cancellation.Token));
        foreach (ProductNativeBuildResult result in results)
            result.Dispose();
        Assert.Equal(2, m_builds);
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private NativeBuildContext Context() => new NativeBuildContext(m_root, "release").WithToolchain(new("fixture-x64",
        new BuildHostDescriptor("Fixture", "x64"), new Dictionary<string, string> { ["compiler"] = Environment.ProcessPath! },
        new Dictionary<string, string>(), [], [], new NativeLinkPlan(".dll"), false));

    private ProductNativeBuildPlan Plan() => new("fixture", new[] { "first", "second" }.Select(id => new ProductNativeBuildStep(
        id, m_owner, new(NativeLibraryKind.Shared), [],
        (
            context,
            dependencies,
            cancellation
        ) => NativeArtifactPublisher.PublishAsync(context, Recipe(context, id), async (
            @scoped,
            output,
            token
        ) => {
            Interlocked.Increment(ref m_builds);
            await File.WriteAllTextAsync(Path.Combine(output, "result.native"), "complete", token);
        }, cancellation), context => Recipe(context, id), static (
            relative,
            target
        ) => relative)).ToArray());

    private NativeBuildRecipe Recipe(
        NativeBuildContext context,
        string id
    ) => new(m_owner, id, "fixture-x64", [NativeBuildInput.FromPath(m_root, Path.Combine(m_root, "shared.input"))], [id]);
}
