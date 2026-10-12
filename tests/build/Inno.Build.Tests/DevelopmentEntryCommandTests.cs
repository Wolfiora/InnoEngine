using System.Threading;
using System.Threading.Tasks;
using Inno.Build.Cli;
using Xunit;

namespace Inno.Build.Tests;

public sealed class DevelopmentEntryCommandTests
{
    [Theory]
    [InlineData("rebuild")]
    [InlineData("engine")]
    [InlineData("editor", "--clean", "true")]
    [InlineData("editor", "--clean", "--clean")]
    [InlineData("editor", "--configuration")]
    [InlineData("editor", "--target", "one", "--target", "two")]
    [InlineData("support-pack", "--clean")]
    [InlineData("editor", "--dry-run")]
    [InlineData("editor", "--unrecognized", "value")]
    public async Task InvalidCommandsFailBeforeSdkDiscoveryOrCleanup(params string[] arguments)
    {
        Assert.Equal(1, await BuildApplication.Run(arguments, CancellationToken.None));
    }

    [Fact]
    public async Task CancellationReturnsBeforeCreatingBuildState()
    {
        using var cancellation = new CancellationTokenSource();
        cancellation.Cancel();
        Assert.Equal(2, await BuildApplication.Run(["editor", "--clean"], cancellation.Token));
    }
}
