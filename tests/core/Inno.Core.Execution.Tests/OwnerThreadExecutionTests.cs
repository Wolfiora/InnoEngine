using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Inno.Core.Execution.Tests;

public sealed class OwnerThreadExecutionTests
{
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
}
