using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Composition;
using Inno.Build.Toolchains;

namespace Inno.Build.Tests;

public sealed class NativeProductExecutorTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoProductExecutor", Guid.NewGuid().ToString("N"));
    private readonly NativeComponentDescriptor m_owner = new("probe", "component/Native.csproj", "component/Toolchain.csproj");
    private int m_productions;

    [Fact]
    public async Task NewTargetAndBackendUseTheSameExecutorAndStaticLinkageCanUseAStandaloneProducer()
    {
        NativeBuildContext context = Context();
        var plan = new ProductNativeBuildPlan("probe-product", [new ProductNativeBuildStep("probe", m_owner,
            new(NativeLibraryKind.Static), [], Produce, Recipe, static (
                relative,
                target
            ) => relative)]);
        var executor = new SelectedExecutor();
        var contribution = new NativeProductContribution(plan, executor);
        using (ProductNativeBuildResult first = await contribution.BuildAsync(context, CancellationToken.None))
        {
            NativeArtifact artifact = Assert.Single(Assert.Single(first).artifacts);
            Assert.Equal(NativeArtifactRole.StaticArchive, artifact.role);
            Assert.Equal("probe-target", first[0].targetId);
            Assert.Equal("probe-output.data", Path.GetFileName(artifact.file));
        }
        using ProductNativeBuildResult hot = await contribution.BuildAsync(Context(), CancellationToken.None);
        Assert.Equal(1, m_productions);
        Assert.Equal(2, executor.executions);
    }

    [Fact]
    public async Task UnsupportedLinkageIsRejectedBeforeBindingOrStaging()
    {
        NativeBuildContext context = Context();
        string sdk = ToolchainEnvironment.ResolveExecutable("dotnet");
        var selection = new NativeToolchainSelection("probe-target", new BuildHostDescriptor("ProbeHost", "processor"),
            new Dictionary<string, string> { ["sdk"] = sdk }, new Dictionary<string, string>(), [], [],
            new NativeLinkPlan(staticLibraryExtension: ".archive"), false);
        var plan = new ProductNativeBuildPlan("probe-product", [new ProductNativeBuildStep("probe", m_owner,
            new(NativeLibraryKind.Shared), [], Produce, Recipe, static (
                relative,
                target
            ) => relative)]);
        await Assert.ThrowsAsync<NotSupportedException>(() => new ComponentNativeProductExecutor()
            .BuildAsync(context.WithToolchain(selection), plan, CancellationToken.None));
        Assert.Equal(0, m_productions);
        Assert.Equal(0, context.statistics.bindingGenerations);
        Assert.False(Directory.Exists(Path.Combine(m_root, "artifacts")));
    }

    [Fact]
    public async Task IncompleteRoleInventoryIsRejectedBeforePublication()
    {
        NativeBuildContext context = Context();
        await Assert.ThrowsAsync<InvalidDataException>(() => NativeArtifactPublisher.PublishAsync(context, Recipe(context),
            static (
                operation,
                output,
                cancellation
            ) => File.WriteAllTextAsync(Path.Combine(output, "payload"), "payload", cancellation),
            describeOutputs: static (
                operation,
                output
            ) => []));
        Assert.Empty(Directory.GetFiles(Path.Combine(m_root, "artifacts", "native"), "generation-manifest.json", SearchOption.AllDirectories));
    }

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    private NativeBuildContext Context()
    {
        Directory.CreateDirectory(Path.Combine(m_root, "component"));
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), "fixture");
        File.WriteAllText(Path.Combine(m_root, "component", "Native.csproj"), "<Project />");
        File.WriteAllText(Path.Combine(m_root, "component", "Toolchain.csproj"), "<Project />");
        string sdk = Environment.GetEnvironmentVariable("DOTNET_HOST_PATH") ?? ToolchainEnvironment.ResolveExecutable("dotnet");
        var tools = new NativeToolchainSelection("probe-target", new BuildHostDescriptor("ProbeHost", "processor"),
            new Dictionary<string, string> { ["sdk"] = sdk }, new Dictionary<string, string>(), [], [], new NativeLinkPlan(".shared", staticLibraryExtension: ".data"), false);
        return new NativeBuildContext(m_root, "release").WithToolchain(tools);
    }

    private NativeBuildRecipe Recipe(NativeBuildContext context) => new(m_owner, "probe", "probe-target",
        [NativeBuildInput.FromPath(m_root, Path.Combine(m_root, "component", "Native.csproj"))], ["probe-recipe"]);

    private Task<NativeBuildProduct> Produce(
        NativeBuildContext context,
        IReadOnlyDictionary<string, NativeBuildProduct> dependencies,
        CancellationToken cancellation
    ) => NativeArtifactPublisher.PublishAsync(context, Recipe(context), (
        operation,
        output,
        token
    ) => {
        m_productions++;
        return File.WriteAllTextAsync(Path.Combine(output, "probe-output.data"), "archive", token);
    }, cancellation, describeOutputs: static (
        operation,
        output
    ) => [new NativeArtifact("probe-archive", Path.Combine(output, "probe-output.data"), NativeArtifactRole.StaticArchive)]);

    private sealed class SelectedExecutor : INativeProductExecutor
    {
        internal int executions { get; private set; }

        public Task<ProductNativeBuildResult> BuildAsync(
            NativeBuildContext context,
            ProductNativeBuildPlan plan,
            CancellationToken cancellationToken
        ) {
            executions++;
            return new ComponentNativeProductExecutor().BuildAsync(context, plan, cancellationToken);
        }
    }
}
