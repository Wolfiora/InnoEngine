using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Core.Execution.Tests;

public sealed class OwnerThreadExecutionTests
{
    [Fact]
    [Trait("ProcessIsolation", "Required")]
    public void WorkerCompletionCannotLeaveTheOwnerWaitingForAnotherCallback()
    {
        Exception? failure = null;
        var owner = new Thread(() =>
        {
            try
            {
                for (int index = 0; index < 4096; index++)
                {
                    int expected = index;
                    Assert.Equal(expected, OwnerThreadExecution.Run(() => Task.Run(() => expected)));
                }
            }
            catch (Exception exception)
            {
                failure = exception;
            }
        }) { IsBackground = true };
        owner.Start();
        Assert.True(owner.Join(TimeSpan.FromSeconds(15)), "A completed worker left the owner blocked on an empty callback queue.");
        Assert.Null(failure);
    }

    [Fact]
    public void AsynchronousContinuationsAndSendReturnToTheCallingThread()
    {
        int owner = Environment.CurrentManagedThreadId;
        SynchronizationContext? original = SynchronizationContext.Current;
        Assert.Equal(42, OwnerThreadExecution.Run(async () =>
        {
            SynchronizationContext context = SynchronizationContext.Current!;
            await Task.Run(() => context.Send(_ => Assert.Equal(owner, Environment.CurrentManagedThreadId), null));
            Assert.Equal(owner, Environment.CurrentManagedThreadId);
            await Task.Delay(10);
            Assert.Equal(owner, Environment.CurrentManagedThreadId);
            return 42;
        }));
        Assert.Same(original, SynchronizationContext.Current);
    }

    [Fact]
    public void CompletedOperationsFailuresAndCancellationRestoreThePreviousContext()
    {
        SynchronizationContext? original = SynchronizationContext.Current;
        Assert.Equal(1, OwnerThreadExecution.Run(() => Task.FromResult(1)));
        var failure = new InvalidOperationException("Expected owner failure.");
        Assert.Same(failure, Assert.Throws<InvalidOperationException>(() =>
            OwnerThreadExecution.Run(() => Task.FromException<int>(failure))));
        Assert.Throws<TaskCanceledException>(() => OwnerThreadExecution.Run(() =>
            Task.FromCanceled<int>(new CancellationToken(canceled: true))));
        Assert.Throws<ArgumentNullException>(() => OwnerThreadExecution.Run<int>(null!));
        Assert.Same(original, SynchronizationContext.Current);
    }

    [Fact]
    public void CompletedOperationsDrainCallbacksPostedByAlreadyQueuedWork()
    {
        int callbacks = 0;
        Assert.Equal(7, OwnerThreadExecution.Run(() =>
        {
            SynchronizationContext context = SynchronizationContext.Current!;
            context.Post(_ =>
            {
                callbacks++;
                context.Post(_ => callbacks++, null);
            }, null);
            return Task.FromResult(7);
        }));
        Assert.Equal(2, callbacks);
    }
}
