using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Inno.Core.Logging;
using Inno.Extensibility.Modules;
using Xunit;

namespace Inno.Core.Logging.Tests;

public sealed class InlineLogDeliveryTests
{
    [Fact]
    public void NestedEntriesReachEverySinkInQueueOrder()
    {
        using var router = new LogRouter(deliveryMode: LogDeliveryMode.Inline);
        var received = new ConcurrentQueue<string>();
        router.RegisterSink(new CallbackSink(entry =>
        {
            if (entry.message == "first")
                router.Dispatch(CreateEntry("nested"));
        }));
        router.RegisterSink(new CallbackSink(entry => received.Enqueue(entry.message)));

        router.Dispatch(CreateEntry("first"));
        router.Flush();

        Assert.Equal(["first", "nested"], received.AsEnumerable());
    }

    [Fact]
    public void LongNestedChainsDoNotReenterDelivery()
    {
        using var router = new LogRouter(deliveryMode: LogDeliveryMode.Inline);
        int received = 0;
        int depth = 0;
        int maximumDepth = 0;
        router.RegisterSink(new CallbackSink(_ =>
        {
            maximumDepth = Math.Max(maximumDepth, ++depth);
            if (++received < 10_000)
                router.Dispatch(CreateEntry("next"));
            depth--;
        }));

        router.Dispatch(CreateEntry("first"));

        Assert.Equal(10_000, received);
        Assert.Equal(1, maximumDepth);
    }

    [Fact]
    public async Task FlushWaitsForAnEntryAlreadyInsideReceive()
    {
        using var router = new LogRouter(deliveryMode: LogDeliveryMode.Inline);
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        using var flushing = new ManualResetEventSlim();
        router.RegisterSink(new CallbackSink(_ =>
        {
            entered.Set();
            Assert.True(release.Wait(TimeSpan.FromSeconds(10)));
        }));
        Task producer = Task.Run(() => router.Dispatch(CreateEntry("blocked")));
        Assert.True(entered.Wait(TimeSpan.FromSeconds(5)));
        Task flush = Task.Run(() =>
        {
            flushing.Set();
            router.Flush();
        });
        try
        {
            Assert.True(flushing.Wait(TimeSpan.FromSeconds(5)));
            Task pending = Task.Delay(100);
            Assert.Same(pending, await Task.WhenAny(flush, pending));
        }
        finally
        {
            release.Set();
            await Task.WhenAll(producer, flush).WaitAsync(TimeSpan.FromSeconds(5));
        }
    }

    [Fact]
    public async Task ConcurrentProducersNeverInvokeASinkConcurrently()
    {
        using var router = new LogRouter(deliveryMode: LogDeliveryMode.Inline);
        int active = 0;
        int overlap = 0;
        int received = 0;
        router.RegisterSink(new CallbackSink(_ =>
        {
            if (Interlocked.Increment(ref active) != 1)
                Interlocked.Increment(ref overlap);
            Thread.SpinWait(2_000);
            Interlocked.Increment(ref received);
            Interlocked.Decrement(ref active);
        }));

        await Task.WhenAll(Enumerable.Range(0, 8).Select(_ => Task.Run(() =>
        {
            for (int entry = 0; entry < 64; entry++)
                router.Dispatch(CreateEntry("parallel"));
        })));
        router.Flush();

        Assert.Equal(512, received);
        Assert.Equal(0, overlap);
    }

    [Fact]
    public async Task DisposalWaitsForActiveDeliveryBeforeReleasingTheSink()
    {
        using var router = new LogRouter(deliveryMode: LogDeliveryMode.Inline);
        using var entered = new ManualResetEventSlim();
        using var release = new ManualResetEventSlim();
        using var disposing = new ManualResetEventSlim();
        bool released = false;
        Exception? deliveryFailure = null;
        router.sinkFailed += (
            _,
            exception
        ) => deliveryFailure = exception;
        router.RegisterSink(new DisposableCallbackSink(_ =>
        {
            entered.Set();
            Assert.True(release.Wait(TimeSpan.FromSeconds(10)));
            Assert.False(released);
        }, () => released = true));
        Task producer = Task.Run(() => router.Dispatch(CreateEntry("blocked")));
        Assert.True(entered.Wait(TimeSpan.FromSeconds(5)));
        Task disposal = Task.Run(() =>
        {
            disposing.Set();
            router.Dispose();
        });
        try
        {
            Assert.True(disposing.Wait(TimeSpan.FromSeconds(5)));
            Task pending = Task.Delay(100);
            Assert.Same(pending, await Task.WhenAny(disposal, pending));
        }
        finally
        {
            release.Set();
            await Task.WhenAll(producer, disposal).WaitAsync(TimeSpan.FromSeconds(5));
        }
        Assert.True(released);
        Assert.Null(deliveryFailure);
        Assert.Throws<ObjectDisposedException>(() => router.Dispatch(CreateEntry("after-disposal")));
    }

    [Theory]
    [InlineData(LogDeliveryMode.Inline)]
    [InlineData(LogDeliveryMode.Background)]
    public void DeliveryCallbacksCannotFlushOrDisposeTheirOwnRouter(LogDeliveryMode mode)
    {
        using var router = new LogRouter(deliveryMode: mode);
        router.RegisterSink(new CallbackSink(_ =>
        {
            Assert.Throws<InvalidOperationException>(router.Flush);
            Assert.Throws<InvalidOperationException>(router.Dispose);
        }));
        Exception? failure = null;
        router.sinkFailed += (
            _,
            exception
        ) => failure = exception;

        router.Dispatch(CreateEntry("callback"));
        router.Flush();

        Assert.Null(failure);
    }

    private static LogEntry CreateEntry(string message) => new(
        LogLevel.Info, AssemblyDomain.InnoInternal, AssemblyScope.Runtime,
        "inline-test", message, string.Empty, 0, string.Empty, LogSessionId.none);

    private sealed class CallbackSink(Action<LogEntry> callback) : ILogSink
    {
        public void Receive(LogEntry entry) => callback(entry);
    }

    private sealed class DisposableCallbackSink(
        Action<LogEntry> receive,
        Action dispose
    ) : ILogSink, IDisposable
    {
        public void Receive(LogEntry entry) => receive(entry);

        public void Dispose() => dispose();
    }
}
