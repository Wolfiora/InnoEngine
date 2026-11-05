using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Core.IO;
using Xunit;

namespace Inno.Build.Toolchains.Tests;

public sealed class ProductNativePreparationTests : IDisposable
{
    private readonly string m_root = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoPreparedNative", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("fixture", "component/Fixture.csproj", "component/Fixture.csproj");
    private int m_productions;

    public ProductNativePreparationTests()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "component"));
        Directory.CreateDirectory(Path.Combine(m_root, "Source"));
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
        File.WriteAllText(Path.Combine(m_root, "component", "Fixture.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(m_root, "Source", "source.h"), "first");
        File.WriteAllText(Path.Combine(m_root, "sdk-tool"), "tool-a");
    }

    [Fact]
    public async Task EqualRequestsAcquireIndependentReadersWithoutProducingAgain()
    {
        NativeBuildContext producer = Context();
        ProductNativeBuildPlan plan = Plan();
        string identity = await ProductNativePreparation.FreezeAsync(producer, plan);
        using ProductNativeBuildResult built = await new ComponentNativeProductExecutor().BuildAsync(producer, plan, CancellationToken.None);
        string[][] rows = Assert.IsType<string[][]>(ProductNativePreparation.Capture(producer, plan, built, identity));
        NativeBuildContext consumer = Context();
        Assert.Equal(identity, await ProductNativePreparation.FreezeAsync(consumer, plan));
        using ProductNativeBuildResult reading = Assert.IsType<ProductNativeBuildResult>(await ProductNativePreparation.TryAcquireAsync(consumer, plan, rows, identity));
        built.Dispose();
        Assert.Equal("result", File.ReadAllText(reading[0].files.Single()));
        Assert.Equal(1, m_productions);
        using var cancellation = new CancellationTokenSource();
        Task<BuildArtifactWriteLease> waiting = BuildArtifactWriteLease.AcquireAsync(reading[0].directory,
            Path.Combine(m_root, "artifacts", "native"), cancellation.Token).AsTask();
        Assert.False(waiting.IsCompleted);
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => waiting);
    }

    [Theory]
    [InlineData("source")]
    [InlineData("membership")]
    [InlineData("sdk")]
    [InlineData("configuration")]
    [InlineData("target")]
    public async Task FrozenIdentityChangesWithEveryDeclaredInput(string changed)
    {
        NativeBuildContext before = Context();
        string original = await ProductNativePreparation.FreezeAsync(before, Plan());
        if (changed == "source")
        {
            string path = Path.Combine(m_root, "Source", "source.h");
            DateTime timestamp = File.GetLastWriteTimeUtc(path);
            File.WriteAllText(path, "other");
            File.SetLastWriteTimeUtc(path, timestamp);
        }
        else if (changed == "membership")
            File.WriteAllText(Path.Combine(m_root, "Source", "added.h"), "new");
        else if (changed == "sdk")
            File.WriteAllText(Path.Combine(m_root, "sdk-tool"), "tool-b");
        NativeBuildContext after = Context(changed == "configuration" ? "release" : "debug", changed == "target" ? "other-target" : "fixture-target");
        Assert.NotEqual(original, await ProductNativePreparation.FreezeAsync(after, Plan()));
    }

    [Fact]
    public async Task SourceChangesWhileWaitingForProtectionRejectReuse()
    {
        NativeBuildContext producer = Context();
        ProductNativeBuildPlan plan = Plan();
        string identity = await ProductNativePreparation.FreezeAsync(producer, plan);
        using ProductNativeBuildResult built = await new ComponentNativeProductExecutor().BuildAsync(producer, plan, CancellationToken.None);
        string[][] rows = ProductNativePreparation.Capture(producer, plan, built, identity)!;
        string directory = built[0].directory;
        built.Dispose();
        NativeBuildContext consumer = Context();
        Assert.Equal(identity, await ProductNativePreparation.FreezeAsync(consumer, plan));
        using BuildArtifactWriteLease writer = await BuildArtifactWriteLease.AcquireAsync(directory, Path.Combine(m_root, "artifacts", "native"));
        Task<ProductNativeBuildResult?> pending = ProductNativePreparation.TryAcquireAsync(consumer, plan, rows, identity);
        Assert.False(pending.IsCompleted);
        File.WriteAllText(Path.Combine(m_root, "Source", "source.h"), "other");
        writer.Dispose();
        Assert.Null(await pending);
    }

    [Fact]
    public async Task CorruptedPublicationAndPreCancellationCannotReturnSuccessfulReaders()
    {
        NativeBuildContext producer = Context();
        ProductNativeBuildPlan plan = Plan();
        string identity = await ProductNativePreparation.FreezeAsync(producer, plan);
        using ProductNativeBuildResult built = await new ComponentNativeProductExecutor().BuildAsync(producer, plan, CancellationToken.None);
        string[][] rows = ProductNativePreparation.Capture(producer, plan, built, identity)!;
        string output = built[0].files.Single();
        built.Dispose();
        File.WriteAllText(output, "broken");
        Assert.Null(await ProductNativePreparation.TryAcquireAsync(Context(), plan, rows, identity));
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => ProductNativePreparation.TryAcquireAsync(Context(), plan, rows, identity, cancellation.Token));
    }

    public void Dispose()
    {
        string parent = Path.Combine(new PhysicalPathResolver().Resolve(Path.GetTempPath()), "InnoPreparedNative") + Path.DirectorySeparatorChar;
        if (!Path.GetFullPath(m_root).StartsWith(parent, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("The native fixture escaped its temporary owner.");
        Directory.Delete(m_root, recursive: true);
    }

    private NativeBuildContext Context(
        string configuration = "debug",
        string target = "fixture-target"
    ) => new NativeBuildContext(m_root, configuration).WithToolchain(new NativeToolchainSelection(target,
        new BuildHostDescriptor("Fixture", "fixture-cpu"), new Dictionary<string, string> { ["compiler"] = Path.Combine(m_root, "sdk-tool") },
        new Dictionary<string, string>(), [], [], new NativeLinkPlan(".fixture"), false));

    private ProductNativeBuildPlan Plan()
        => new("fixture-product", [new ProductNativeBuildStep("fixture", m_owner, new NativeComponentBuildOptions(NativeLibraryKind.Shared), [],
            (
                context,
                dependencies,
                cancellation
            ) => NativeArtifactPublisher.PublishAsync(context, Recipe(context), async (
                owned,
                output,
                token
            ) => {
                Interlocked.Increment(ref m_productions);
                await File.WriteAllTextAsync(Path.Combine(output, "result.fixture"), "result", token);
            }, cancellation), Recipe, (
                path,
                target
            ) => path)]);

    private NativeBuildRecipe Recipe(NativeBuildContext context)
        => new(m_owner, "fixture", context.toolchain!.targetId,
            new[] { Path.Combine(m_root, "Source") }.Concat(context.toolchain.inputPaths).Select(path => NativeBuildInput.FromPath(m_root, path)),
            new[] { context.configuration }.Concat(context.toolchain.declarations));
}
