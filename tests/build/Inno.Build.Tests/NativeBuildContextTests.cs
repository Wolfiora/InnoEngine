using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Toolchains;
using Inno.Build.Toolchains.Host;
using Xunit;

namespace Inno.Build.Tests;

public sealed class NativeBuildContextTests : IDisposable
{
    private readonly string m_root = Path.Combine(Path.GetTempPath(), "InnoNativeBuildTests", Guid.NewGuid().ToString("N"));

    public void Dispose()
    {
        if (Directory.Exists(m_root))
            Directory.Delete(m_root, recursive: true);
    }

    [Fact]
    public void SelectedCheckoutOwnsIntermediatesEvenWhenTheToolRunsFromAnotherCheckout()
    {
        CreateCheckout();
        string owner = Path.Combine(m_root, "build", "toolchains", "Inno.Build.Toolchains");
        Directory.CreateDirectory(owner);
        File.WriteAllText(Path.Combine(owner, "Inno.Build.Toolchains.csproj"), "<Project />");
        var context = new NativeBuildContext(m_root, "release");

        Assert.Equal(Path.GetFullPath(m_root), context.engineRoot);
        Assert.Equal("release", context.configuration);
        Assert.Equal(Path.Combine(owner, "obj", "native"), context.GetNativeBuildRoot(typeof(ToolchainEnvironment).Assembly));
        Assert.NotEqual(ToolchainEnvironment.FindRepoRoot(), context.engineRoot);
        Assert.False(Directory.Exists(Path.Combine(owner, "obj")));
    }

    [Fact]
    public void ContextRejectsMissingCheckoutAndUnsupportedConfiguration()
    {
        Assert.Throws<DirectoryNotFoundException>(() => new NativeBuildContext(m_root, "release"));
        Assert.Throws<ArgumentException>(() => new NativeBuildContext(string.Empty, "release"));
        CreateCheckout();
        Assert.Throws<ArgumentException>(() => new NativeBuildContext(m_root, "Release"));
        Assert.Throws<ArgumentException>(() => new NativeBuildContext(m_root, "custom"));
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task CanceledBuildDoesNotStartAComponentOrCreateIntermediates(bool editor)
    {
        CreateCheckout();
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        var context = new NativeBuildContext(m_root, "debug");

        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => editor
            ? HostNativeBuild.BuildEditorAsync(context, cancellation.Token)
            : HostNativeBuild.BuildRuntimeAsync(context, cancellation.Token));

        Assert.Single(Directory.EnumerateFileSystemEntries(m_root));
    }

    [Fact]
    public async Task MissingNativeSourcesAreReportedFromTheSelectedCheckout()
    {
        CreateCheckout();
        var context = new NativeBuildContext(m_root, "release");

        DirectoryNotFoundException failure = await Assert.ThrowsAsync<DirectoryNotFoundException>(
            () => HostNativeBuild.BuildRuntimeAsync(context));

        Assert.Contains(Path.Combine(m_root, "extern"), failure.Message, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain(ToolchainEnvironment.FindRepoRoot(), failure.Message, StringComparison.OrdinalIgnoreCase);
    }

    private void CreateCheckout()
    {
        Directory.CreateDirectory(m_root);
        File.WriteAllText(Path.Combine(m_root, "InnoEngine.sln"), string.Empty);
    }
}
