using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Managed;
using Inno.Build.Managed.DotNet;
using Inno.Build.Toolchains;
using Xunit;

namespace Inno.Build.Tests;

public sealed class ManagedDeploymentTests
{
    [Fact]
    public void DotNetProvidersConsumeAnOpenFrozenTargetSelection()
    {
        string[] targets = ["fixture-runtime"];
        var workspaces = new PhysicalToolchainWorkspaceProvider();
        IManagedDeploymentCompiler[] compilers =
        [
            new CoreClrDeploymentCompiler("dotnet", targets, workspaces),
            new NativeAotDeploymentCompiler("dotnet", targets, workspaces),
            new MonoWasmDeploymentCompiler("dotnet", targets, workspaces, false),
            new MonoWasmDeploymentCompiler("dotnet", targets, workspaces, true)
        ];
        targets[0] = "changed";
        var catalog = new ManagedDeploymentCatalog(compilers);
        Assert.Equal(4, catalog.GetSupportedDeployments("fixture-runtime").Count);
        Assert.Empty(catalog.GetSupportedDeployments("changed"));
        foreach (IManagedDeploymentCompiler compiler in compilers)
            Assert.Same(compiler, catalog.Resolve(compiler.id, "fixture-runtime"));
    }

    [Fact]
    public void OpenProviderCatalogDispatchesCustomIdentityWithoutPlatformBranches()
    {
        var compiler = new CustomCompiler();
        var catalog = new ManagedDeploymentCatalog([compiler]);
        Assert.Same(compiler, catalog.Resolve(new ManagedDeploymentId("custom-runtime"), "custom-target"));
        Assert.Throws<InvalidOperationException>(() => catalog.Resolve(compiler.id, "unsupported"));
        Assert.Throws<InvalidOperationException>(() => catalog.Resolve(new ManagedDeploymentId("absent"), "custom-target"));
        Assert.Throws<ArgumentException>(() => new ManagedDeploymentCatalog([compiler, compiler]));
    }

    [Fact]
    public void CapabilityInputsAndPublicationArtifactsAreImmutable()
    {
        string[] targets = ["custom-target"];
        var capabilities = new ManagedDeploymentCapabilities(targets,
            dynamicCode: false, aheadOfTime: true, nativeStaticLinking: true);
        targets[0] = "replaced";
        Assert.Equal("custom-target", Assert.Single(capabilities.runtimeIdentifiers));
        string[] files = ["game.wasm"];
        var result = new ManagedDeploymentResult(new ManagedDeploymentId("custom-runtime"),
            Path.GetTempPath(), "selected-sdk", files);
        files[0] = "replaced.wasm";
        Assert.Equal("game.wasm", Assert.Single(result.files));
        Assert.Throws<ArgumentException>(() => new ManagedDeploymentResult(result.deployment,
            result.outputDirectory, result.sdkIdentity, ["../outside"]));
    }

    private sealed class CustomCompiler : IManagedDeploymentCompiler
    {
        public ManagedDeploymentId id => new("custom-runtime");
        public ManagedDeploymentCapabilities capabilities { get; } = new(
            ["custom-target"], dynamicCode: false, aheadOfTime: true, nativeStaticLinking: true);

        public ValueTask<ManagedDeploymentResult> CompileAsync(
            ManagedDeploymentRequest request,
            CancellationToken cancellationToken = default
        ) => throw new InvalidOperationException("This contract test resolves providers without publishing code.");
    }
}
