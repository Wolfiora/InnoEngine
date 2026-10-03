using System;
using System.Threading;
using System.Threading.Tasks;
using Inno.Shell;
using Xunit;

namespace Inno.Input.Tests;

public sealed class ShellFrameDriverTests
{
    [Fact]
    public async Task PollingFramesKeepCallingThreadAndStopAtCallbackBoundary()
    {
        int owner = Environment.CurrentManagedThreadId;
        int frames = 0;
        var driver = new PollingShellFrameDriver();
        await driver.RunAsync(() =>
        {
            Assert.Equal(owner, Environment.CurrentManagedThreadId);
            return ++frames < 3;
        }, CancellationToken.None);
        Assert.Equal(3, frames);
        Assert.True(driver.allowsBlockingPacing);
    }

    [Fact]
    public async Task ScheduledFramesWaitBeforeAdvancingAndStopWithoutSchedulingAgain()
    {
        var opportunity = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        int frames = 0;
        int scheduled = 0;
        var driver = new ScheduledShellFrameDriver(_ =>
        {
            scheduled++;
            return new ValueTask(opportunity.Task);
        });
        Task run = driver.RunAsync(() =>
        {
            frames++;
            return false;
        }, CancellationToken.None).AsTask();
        Assert.Equal(0, frames);
        Assert.Equal(1, scheduled);
        opportunity.SetResult();
        await run;
        Assert.Equal(1, frames);
        Assert.Equal(1, scheduled);
        Assert.False(driver.allowsBlockingPacing);
    }

    [Fact]
    public async Task CancellationDuringSchedulingPreventsTheNextFrame()
    {
        using var cancellation = new CancellationTokenSource();
        var opportunity = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
        int frames = 0;
        var driver = new ScheduledShellFrameDriver(token => new ValueTask(opportunity.Task.WaitAsync(token)));
        Task run = driver.RunAsync(() =>
        {
            frames++;
            return true;
        }, cancellation.Token).AsTask();
        cancellation.Cancel();
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => run);
        Assert.Equal(0, frames);
    }

    [Fact]
    public async Task SchedulingAndFrameFailuresPropagateToTheShellOwner()
    {
        var expected = new InvalidOperationException("frame failed");
        var driver = new ScheduledShellFrameDriver(_ => ValueTask.CompletedTask);
        Exception actual = await Assert.ThrowsAsync<InvalidOperationException>(
            () => driver.RunAsync(() => throw expected, CancellationToken.None).AsTask());
        Assert.Same(expected, actual);
        Assert.Throws<InvalidOperationException>(
            () => new PollingShellFrameDriver().RunAsync(() => throw expected, CancellationToken.None));
    }
}
