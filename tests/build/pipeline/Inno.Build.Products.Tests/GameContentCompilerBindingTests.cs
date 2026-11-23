using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Build.Managed;

namespace Inno.Build.Products.Tests;

public sealed class GameContentCompilerBindingTests
{
    [Fact]
    public void BindingRejectsMissingOrMismatchedComponentsBeforeAnyWork()
    {
        var packager = new FixturePackager();
        var compiler = new FixtureCompiler(packager.id);
        var binding = new GameBuildTargetBinding(packager, compiler);
        Assert.Same(packager, binding.packager);
        Assert.Same(compiler, binding.compiler);
        Assert.Throws<ArgumentNullException>(() => new GameBuildTargetBinding(null!, compiler));
        Assert.Throws<ArgumentNullException>(() => new GameBuildTargetBinding(packager, null!));
        Assert.Throws<ArgumentException>(
            () => new GameBuildTargetBinding(packager, new FixtureCompiler(new BuildTargetID("other-target")))
        );
    }

    [Fact]
    public async Task ContentCompilerCanBeReplacedWithoutChangingPlatformPackaging()
    {
        var packager = new FixturePackager();
        var first = new FixtureCompiler(packager.id);
        var second = new FixtureCompiler(packager.id);
        var original = new GameBuildTargetBinding(packager, first);
        var replacement = new GameBuildTargetBinding(packager, second);
        await original.compiler.CompileAsync(null!);
        await replacement.compiler.CompileAsync(null!);
        Assert.Equal(1, first.calls);
        Assert.Equal(1, second.calls);
        Assert.Same(original.packager, replacement.packager);
        Assert.Equal(0, packager.calls);
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => second.CompileAsync(null!, cancellation.Token).AsTask());
        Assert.Equal(1, second.calls);
    }

    private sealed class FixtureCompiler(BuildTargetID target) : IGameContentCompiler
    {
        public BuildTargetID target { get; } = target;

        public ValueTask CompileAsync(
            GameBuildContentContext context,
            CancellationToken cancellationToken = default
        ) {
            cancellationToken.ThrowIfCancellationRequested();
            calls++;
            return ValueTask.CompletedTask;
        }

        internal int calls { get; private set; }
    }

    private sealed class FixturePackager : IGameBuildTarget
    {
        public BuildTargetID id => new("fixture-platform");
        public ManagedDeploymentID defaultManagedDeployment => ManagedDeploymentID.coreClr;
        public string runtimeIdentifier => "fixture-rid";
        public string displayName => "Fixture";

        public void Validate(string directory) => throw new InvalidOperationException("Binding must not inspect output.");

        public ValueTask<string> PackageAsync(
            GameBuildPackageContext context,
            CancellationToken cancellationToken = default
        ) {
            calls++;
            throw new InvalidOperationException("Content compilation must not call platform packaging.");
        }

        internal int calls { get; private set; }
    }
}
