using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

using Inno.Core.Execution;
using Inno.Extensibility.Modules;

namespace Inno.Core.Logging;

/// <summary>
/// Owns one host's bounded log queue, filtering policy, and sink collection.
/// </summary>
public sealed class LogRouter : IDisposable
{
    private static readonly ExecutionSlot<LogRouter> S_CURRENT_SCOPE = new("router");

    private readonly List<ILogSink> m_sinks = [];
    private readonly Lock m_sinksLock = new();
    private readonly ConcurrentQueue<WorkItem> m_queue = new();
    private readonly object m_queueSync = new();
    private readonly object m_inlineDeliverySync = new();
    private readonly object m_disposalSync = new();
    private readonly int m_queueCapacity;
    private readonly int m_drainBudget;
    private readonly SemaphoreSlim m_signal = new(0);
    private readonly Thread? m_worker;
    private volatile bool m_running = true;
    private volatile LogLevel m_minimumLevel = LogLevel.Debug;
    private volatile bool m_disposed;
    private int m_deliveryThreadId;
    private ILogSink[] m_sinkSnapshot = [];

    /// <summary>
    /// Occurs after a failing sink has been quarantined from this router.
    /// </summary>
    public event Action<ILogSink, Exception>? sinkFailed;

    /// <summary>
    /// Creates an isolated logging router with an explicit delivery policy.
    /// </summary>
    /// <param name="queueCapacity">
    /// Maximum pending entries and flush barriers before explicit rejection.
    /// </param>
    /// <param name="drainBudget">
    /// Maximum work items delivered using one sink snapshot.
    /// </param>
    /// <param name="deliveryMode">
    /// Selects worker or inline delivery independently of the platform.
    /// </param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// A capacity or budget is not positive, or the delivery policy is not a defined mode.
    /// </exception>
    public LogRouter(
        int queueCapacity = 65536,
        int drainBudget = 4096,
        LogDeliveryMode deliveryMode = LogDeliveryMode.Background
    ) {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(queueCapacity);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(drainBudget);
        if (!Enum.IsDefined(deliveryMode))
            throw new ArgumentOutOfRangeException(nameof(deliveryMode));
        m_queueCapacity = queueCapacity;
        m_drainBudget = drainBudget;
        if (deliveryMode == LogDeliveryMode.Background)
        {
            m_worker = new Thread(ProcessQueue)
            {
                IsBackground = true,
                Name = $"Inno.LogRouter.{Guid.NewGuid():N}"
            };
            m_worker.Start();
        }
    }

    internal static LogRouter current => S_CURRENT_SCOPE.current;

    /// <summary>
    /// Binds this router to the current asynchronous execution context.
    /// </summary>
    /// <returns>
    /// A strict last-in-first-out scope owned by the caller.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this router has been disposed.
    /// </exception>
    public IDisposable EnterScope()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        return S_CURRENT_SCOPE.Enter(this);
    }

    /// <summary>
    /// Registers a sink to receive future entries from this router.
    /// </summary>
    /// <param name="sink">
    /// The sink to register exactly once.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="sink"/> is null.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this router has been disposed.
    /// </exception>
    public void RegisterSink(ILogSink sink)
    {
        ArgumentNullException.ThrowIfNull(sink);
        ObjectDisposedException.ThrowIf(m_disposed, this);
        lock (m_sinksLock)
        {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            if (!m_sinks.Contains(sink))
            {
                m_sinks.Add(sink);
                Volatile.Write(ref m_sinkSnapshot, m_sinks.ToArray());
            }
        }
    }

    /// <summary>
    /// Unregisters a sink so it receives no future entries from this router.
    /// </summary>
    /// <param name="sink">
    /// The sink to remove when registered.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="sink"/> is null.
    /// </exception>
    public void UnregisterSink(ILogSink sink)
    {
        ArgumentNullException.ThrowIfNull(sink);
        lock (m_sinksLock)
        {
            if (m_sinks.Remove(sink))
                Volatile.Write(ref m_sinkSnapshot, m_sinks.ToArray());
        }
    }

    /// <summary>
    /// Sets the lowest severity accepted by this router.
    /// </summary>
    /// <param name="level">
    /// The minimum dispatched severity.
    /// </param>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this router has been disposed.
    /// </exception>
    public void SetMinimumLevel(LogLevel level)
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        m_minimumLevel = level;
    }

    /// <summary>
    /// Creates a category-bound logger for one engine service or extension type.
    /// </summary>
    /// <typeparam name="TOwner">
    /// The type whose assembly ownership and category identify emitted entries.
    /// </typeparam>
    /// <returns>
    /// A stateless logger bound to this router and the supplied owner type.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this router has been disposed.
    /// </exception>
    public Logger CreateLogger<TOwner>()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        Type ownerType = typeof(TOwner);
        return new Logger(
            this,
            ownerType.Assembly.GetInnoAssemblyDomain(),
            ownerType.Assembly.GetInnoAssemblyScope(),
            ownerType.Name);
    }

    /// <summary>
    /// Queues an immutable entry under the configured worker or inline delivery policy.
    /// </summary>
    /// <param name="entry">
    /// The entry to dispatch when it satisfies the current severity policy.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The bounded queue is full and the producer must apply backpressure.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this router has been disposed.
    /// </exception>
    public void Dispatch(LogEntry entry)
    {
        if (!TryDispatch(entry))
            throw new InvalidOperationException("The log queue is full; the producer must apply backpressure.");
    }

    /// <summary>
    /// Attempts to queue an immutable entry, draining it inline when the host has no logging worker.
    /// </summary>
    /// <param name="entry">
    /// The message to accept under the current severity policy.
    /// </param>
    /// <returns>
    /// True when accepted or filtered by policy; false when bounded queue capacity is exhausted.
    /// </returns>
    /// <remarks>
    /// Inline callers serialize delivery. Entries written by a sink callback are queued until
    /// every sink has received the current entry, without recursively invoking callbacks.
    /// </remarks>
    /// <exception cref="ObjectDisposedException">
    /// The router has stopped accepting work.
    /// </exception>
    public bool TryDispatch(LogEntry entry)
    {
        lock (m_queueSync)
        {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            if (!IsEnabled(entry.level))
                return true;
            if (m_queue.Count >= m_queueCapacity)
                return false;
            Enqueue(WorkItem.ForEntry(entry));
        }
        if (m_worker is null)
            DrainInline();
        return true;
    }

    /// <summary>
    /// Blocks until every entry enqueued before this call has reached the current sink snapshot.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// A delivery callback attempts to wait for its own completion, or the queue cannot accept a flush barrier.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// Thrown after this router has been disposed.
    /// </exception>
    public void Flush()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        EnsureOutsideDelivery("flush");
        using var completion = new ManualResetEventSlim(initialState: false);
        lock (m_queueSync)
        {
            ObjectDisposedException.ThrowIf(m_disposed, this);
            if (m_queue.Count >= m_queueCapacity)
                throw new InvalidOperationException("The log queue is full; retry the flush after the worker drains pending entries.");
            Enqueue(WorkItem.ForBarrier(completion));
        }
        if (m_worker is null)
            DrainInline();
        completion.Wait();
    }

    /// <summary>
    /// Drains pending entries, stops the worker, and disposes every sink still owned by this router.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// A delivery callback attempts to dispose its own router.
    /// </exception>
    /// <exception cref="AggregateException">
    /// One or more owned sinks fail to dispose after pending entries have drained.
    /// </exception>
    public void Dispose()
    {
        EnsureOutsideDelivery("dispose");
        lock (m_disposalSync)
            DisposeCore();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal bool IsEnabled(LogLevel level) => level >= m_minimumLevel;

    private void DisposeCore()
    {
        lock (m_queueSync)
        {
            if (m_disposed)
                return;
            m_disposed = true;
            m_running = false;
            m_signal.Release();
        }
        m_worker?.Join();
        if (m_worker is null)
            DrainInline();
        ILogSink[] sinks;
        lock (m_sinksLock)
        {
            sinks = m_sinks.ToArray();
            m_sinks.Clear();
            Volatile.Write(ref m_sinkSnapshot, []);
            sinkFailed = null;
        }
        List<Exception>? failures = null;
        for (int index = 0; index < sinks.Length; index++)
        {
            if (sinks[index] is not IDisposable disposable)
                continue;
            try
            {
                disposable.Dispose();
            }
            catch (Exception exception)
            {
                failures ??= [];
                failures.Add(exception);
            }
        }
        m_signal.Dispose();
        if (failures is not null)
            throw new AggregateException("One or more log sinks failed to dispose.", failures);
    }

    private void ProcessQueue()
    {
        Volatile.Write(ref m_deliveryThreadId, Environment.CurrentManagedThreadId);
        try
        {
            while (true)
            {
                m_signal.Wait();
                DrainQueue();
                if (!m_running && m_queue.IsEmpty)
                    return;
            }
        }
        finally
        {
            Volatile.Write(ref m_deliveryThreadId, 0);
        }
    }

    private void DrainInline()
    {
        lock (m_inlineDeliverySync)
        {
            if (m_deliveryThreadId != 0)
                return;
            Volatile.Write(ref m_deliveryThreadId, Environment.CurrentManagedThreadId);
            try
            {
                while (!m_queue.IsEmpty)
                    DrainQueue();
            }
            finally
            {
                Volatile.Write(ref m_deliveryThreadId, 0);
            }
        }
    }

    private void EnsureOutsideDelivery(string operation)
    {
        if (Volatile.Read(ref m_deliveryThreadId) == Environment.CurrentManagedThreadId)
            throw new InvalidOperationException($"A log delivery callback cannot {operation} its own router.");
    }

    private void Enqueue(WorkItem item)
    {
        m_queue.Enqueue(item);
        if (m_worker is not null)
            m_signal.Release();
    }

    private void DrainQueue()
    {
        ILogSink[] sinks = Volatile.Read(ref m_sinkSnapshot);
        HashSet<ILogSink>? quarantined = null;
        int remaining = m_drainBudget;
        while (remaining-- > 0 && m_queue.TryDequeue(out WorkItem item))
        {
            if (item.completion is ManualResetEventSlim completion)
            {
                completion.Set();
                continue;
            }
            for (int index = 0; index < sinks.Length; index++)
            {
                if (quarantined?.Contains(sinks[index]) == true)
                    continue;
                try
                {
                    sinks[index].Receive(item.entry);
                }
                catch (Exception exception)
                {
                    (quarantined ??= []).Add(sinks[index]);
                    lock (m_sinksLock)
                    {
                        m_sinks.Remove(sinks[index]);
                        Volatile.Write(ref m_sinkSnapshot, m_sinks.ToArray());
                    }
                    ReportSinkFailure(sinks[index], exception);
                }
            }
        }
    }

    private void ReportSinkFailure(
        ILogSink sink,
        Exception exception
    ) {
        Action<ILogSink, Exception>? handlers = sinkFailed;
        if (handlers is null)
        {
            ReportFailureToConsole(
                $"Log sink '{sink.GetType().FullName}' failed and was quarantined: {exception}");
            return;
        }
        foreach (Delegate handler in handlers.GetInvocationList())
        {
            try
            {
                ((Action<ILogSink, Exception>)handler)(sink, exception);
            }
            catch (Exception observerFailure)
            {
                ReportFailureToConsole(
                    $"Log sink failure observer '{handler.Method.DeclaringType?.FullName}' failed: {observerFailure}");
            }
        }
    }

    private static void ReportFailureToConsole(string message)
    {
        try
        {
            Console.Error.WriteLine(message);
        }
        catch (Exception)
        {
            // A failed secondary reporting channel must not terminate delivery to healthy sinks.
        }
    }

    private readonly record struct WorkItem(
        LogEntry entry,
        ManualResetEventSlim? completion
    ) {
        internal static WorkItem ForEntry(LogEntry entry) => new(entry, null);

        internal static WorkItem ForBarrier(ManualResetEventSlim completion) => new(default, completion);
    }

}
