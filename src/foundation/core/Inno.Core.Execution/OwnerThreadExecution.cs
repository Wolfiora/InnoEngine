using System;
using System.Collections.Concurrent;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Core.Execution;

/// <summary>
/// Runs asynchronous application work while returning its continuations to the calling thread.
/// </summary>
public static class OwnerThreadExecution
{
    /// <summary>
    /// Pumps continuations until the owned operation completes, then restores the previous context.
    /// </summary>
    /// <typeparam name="TResult">
    /// The result produced by the operation.
    /// </typeparam>
    /// <param name="operation">
    /// Work that owns and retires every task it starts before returning. Async void work is not supported.
    /// </param>
    /// <returns>
    /// The completed operation result. Operation failures and cancellation propagate unchanged.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// The operation is null.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The operation returned a null task.
    /// </exception>
    /// <remarks>
    /// This is a blocking host boundary. Callback-driven hosts use their platform synchronization
    /// context instead. Work that explicitly suppresses context capture is responsible for returning
    /// to the owner before accessing thread-affine resources.
    /// </remarks>
    public static TResult Run<TResult>(Func<Task<TResult>> operation)
    {
        ArgumentNullException.ThrowIfNull(operation);
        SynchronizationContext? previous = SynchronizationContext.Current;
        using var context = new OwnerContext();
        SynchronizationContext.SetSynchronizationContext(context);
        try
        {
            Task<TResult> completion = operation()
                ?? throw new InvalidOperationException("The owned operation returned a null task.");
            context.Pump(completion);
            return completion.GetAwaiter().GetResult();
        }
        finally
        {
            SynchronizationContext.SetSynchronizationContext(previous);
        }
    }

    private sealed class OwnerContext : SynchronizationContext, IDisposable
    {
        private readonly BlockingCollection<Action> m_callbacks = new();
        private readonly int m_threadId = Environment.CurrentManagedThreadId;

        /// <inheritdoc />
        public override void Post(
            SendOrPostCallback callback,
            object? state
        ) {
            ArgumentNullException.ThrowIfNull(callback);
            m_callbacks.Add(() => callback(state));
        }

        /// <inheritdoc />
        public override void Send(
            SendOrPostCallback callback,
            object? state
        ) {
            ArgumentNullException.ThrowIfNull(callback);
            if (Environment.CurrentManagedThreadId == m_threadId)
            {
                callback(state);
                return;
            }
            ExceptionDispatchInfo? failure = null;
            using var completed = new ManualResetEventSlim();
            Post(_ =>
            {
                try
                {
                    callback(state);
                }
                catch (Exception exception)
                {
                    failure = ExceptionDispatchInfo.Capture(exception);
                }
                finally
                {
                    completed.Set();
                }
            }, null);
            completed.Wait();
            failure?.Throw();
        }

        /// <inheritdoc />
        public override SynchronizationContext CreateCopy() => this;

        /// <inheritdoc />
        public void Dispose() => m_callbacks.Dispose();

        internal void Pump(Task completion)
        {
            Task notification = completion.ContinueWith(_ => m_callbacks.Add(static () => { }),
                CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            while (!notification.IsCompleted || m_callbacks.Count != 0)
                m_callbacks.Take()();
        }
    }
}
