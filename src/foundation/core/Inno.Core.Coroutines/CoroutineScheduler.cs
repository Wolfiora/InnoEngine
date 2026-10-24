using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Inno.Core.Coroutines;

/// <summary>
/// Serializes coroutine execution and owns reverse-order retirement of admitted enumerators.
/// </summary>
public sealed class CoroutineScheduler : IDisposable
{
    private readonly ConcurrentQueue<Command> m_commands = new();
    private readonly ConcurrentDictionary<long, byte> m_liveIds = new();
    private readonly Dictionary<long, CoroutineState> m_states = new();
    private readonly List<long> m_active = new();
    private readonly Lock m_tickGate = new();
    private readonly WeakReference<CoroutineScheduler> m_selfRef;
    private long m_nextId;
    private int m_disposed;
    private double m_now;
    private ulong m_frame;
    private bool m_ticking;
    private bool m_applyingCommands;
    private bool m_needsPruning;
    private int m_operationDepth;
    private List<Exception>? m_failures;

    /// <summary>
    /// Creates an empty scheduler whose lifetime is owned by the caller.
    /// </summary>
    public CoroutineScheduler()
    {
        m_selfRef = new WeakReference<CoroutineScheduler>(this);
    }

    /// <summary>
    /// Starts a coroutine.
    /// </summary>
    /// <param name="routine">
    /// Enumerator exclusively transferred to the scheduler after successful admission.
    /// </param>
    /// <returns>
    /// Coroutine handle.
    /// </returns>
    public CoroutineHandle StartCoroutine(IEnumerator routine)
    {
        return StartCoroutine(null, routine);
    }

    /// <summary>
    /// Starts a coroutine with an owner token.
    /// </summary>
    /// <param name="owner">
    /// Owner key used by <see cref="StopAllCoroutines(object)"/>.
    /// </param>
    /// <param name="routine">
    /// Enumerator exclusively transferred to the scheduler after successful admission.
    /// </param>
    /// <returns>
    /// Coroutine handle.
    /// </returns>
    public CoroutineHandle StartCoroutine(
        object? owner,
        IEnumerator routine
    ) {
        ArgumentNullException.ThrowIfNull(routine);

        lock (m_tickGate)
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref m_disposed) != 0, this);

            long id = Interlocked.Increment(ref m_nextId);
            m_liveIds[id] = 0;
            m_commands.Enqueue(Command.Start(id, owner, routine));
            return new CoroutineHandle(id, m_selfRef);
        }
    }

    /// <summary>
    /// Requests stopping a coroutine by handle.
    /// </summary>
    /// <param name="handle">
    /// Target coroutine handle.
    /// </param>
    /// <returns>
    /// <see langword="true"/> if the handle is valid.
    /// </returns>
    public bool StopCoroutine(CoroutineHandle handle)
    {
        if (!handle.IsOwnedBy(this) || !handle.isValid || Volatile.Read(ref m_disposed) != 0)
        {
            return false;
        }

        m_commands.Enqueue(Command.Stop(handle.id));
        return true;
    }

    /// <summary>
    /// Stops owned coroutines and disposes their nested enumerators in reverse order.
    /// Reentrant calls defer cleanup of an executing callback until that callback returns.
    /// </summary>
    /// <param name="owner">
    /// Owner key used when starting coroutines.
    /// </param>
    /// <exception cref="AggregateException">
    /// One or more eligible enumerators failed during cleanup.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The scheduler has been disposed.
    /// </exception>
    public void StopAllCoroutines(object owner)
    {
        ObjectDisposedException.ThrowIf(Volatile.Read(ref m_disposed) != 0, this);
        ArgumentNullException.ThrowIfNull(owner);
        lock (m_tickGate)
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref m_disposed) != 0, this);
            m_operationDepth++;
            try
            {
                m_commands.Enqueue(Command.StopByOwner(owner));
                ApplyPendingCommands();
            }
            finally
            {
                CompleteOperation();
            }
        }
    }

    /// <summary>
    /// Requests stopping all active coroutines.
    /// </summary>
    public void StopAllCoroutines()
    {
        if (Volatile.Read(ref m_disposed) != 0)
        {
            return;
        }

        m_commands.Enqueue(Command.StopAll());
    }

    /// <summary>
    /// Closes admission and disposes all active and pending enumerators.
    /// Reentrant disposal retires the executing enumerator after its callback returns.
    /// </summary>
    /// <exception cref="AggregateException">
    /// Cleanup failed after all eligible enumerators were attempted.
    /// </exception>
    public void Dispose()
    {
        lock (m_tickGate)
        {
            if (Interlocked.Exchange(ref m_disposed, 1) != 0)
                return;

            m_operationDepth++;
            try
            {
                ApplyPendingCommands();
                RetireAll();
                PruneInactive();
            }
            finally
            {
                CompleteOperation();
            }
        }
    }

    internal double now => m_now;
    internal ulong frame => m_frame;

    /// <summary>
    /// Advances coroutine execution by one frame.
    /// </summary>
    /// <param name="deltaTime">
    /// Frame delta time in seconds.
    /// </param>
    /// <exception cref="AggregateException">
    /// Execution, a wait predicate, or enumerator cleanup failed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// A callback recursively advances this scheduler.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The scheduler has been disposed.
    /// </exception>
    public void Tick(float deltaTime)
    {
        if (deltaTime < 0f)
        {
            deltaTime = 0f;
        }

        lock (m_tickGate)
        {
            ObjectDisposedException.ThrowIf(Volatile.Read(ref m_disposed) != 0, this);

            if (m_ticking || m_operationDepth != 0)
                throw new InvalidOperationException("A coroutine callback cannot recursively advance its scheduler.");

            m_operationDepth++;
            m_ticking = true;
            try
            {
                m_now += deltaTime;
                m_frame++;
                ApplyPendingCommands();
                TickActiveCoroutines();
            }
            finally
            {
                m_ticking = false;
                PruneInactive();
                CompleteOperation();
            }
        }
    }

    internal bool Contains(CoroutineHandle handle)
    {
        if (handle.IsOwnedBy(this))
        {
            return IsHandleValid(handle.id);
        }

        return handle.isValid;
    }

    internal bool IsHandleValid(long id)
    {
        return Volatile.Read(ref m_disposed) == 0 && id > 0 && m_liveIds.ContainsKey(id);
    }

    private void ApplyPendingCommands()
    {
        // Cleanup callbacks may enqueue commands, but must not recursively drain the queue.
        if (m_applyingCommands)
            return;

        m_applyingCommands = true;
        try
        {
            while (m_commands.TryDequeue(out Command cmd))
            {
                switch (cmd.kind)
                {
                    case CommandKind.Start:
                        if (cmd.routine is null)
                            break;
                        CoroutineState state = new(cmd.id, cmd.owner, cmd.routine);
                        m_states.Add(state.id, state);
                        m_active.Add(state.id);
                        if (Volatile.Read(ref m_disposed) != 0)
                            Retire(state);
                        break;
                    case CommandKind.Stop:
                        if (m_states.TryGetValue(cmd.id, out CoroutineState? stopped))
                            Retire(stopped);
                        break;
                    case CommandKind.StopByOwner:
                        for (int i = 0; i < m_active.Count; i++)
                        {
                            if (m_states.TryGetValue(m_active[i], out CoroutineState? owned)
                                && ReferenceEquals(owned.owner, cmd.owner))
                                Retire(owned);
                        }
                        break;
                    case CommandKind.StopAll:
                        RetireAll();
                        break;
                }
            }
        }
        finally
        {
            m_applyingCommands = false;
            if (!m_ticking)
                PruneInactive();
        }
    }

    private void TickActiveCoroutines()
    {
        // Retirements leave stable slots until the outer dispatch completes. A callback can
        // stop any owner without shifting this cursor or executing another coroutine twice.
        int count = m_active.Count;
        for (int i = 0; i < count; i++)
        {
            if (!m_states.TryGetValue(m_active[i], out CoroutineState? state))
                continue;

            state.executing = true;
            try
            {
                if (!StepCoroutine(state))
                    Retire(state);
            }
            catch (Exception failure)
            {
                RecordFailure(failure);
                Retire(state);
            }
            finally
            {
                state.executing = false;
                if (state.retired)
                    DisposeStack(state);
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private bool StepCoroutine(CoroutineState state)
    {
        if (state.waiter is not null)
        {
            bool waiting = state.waiter.Invoke(this);
            if (state.retired)
                return false;
            if (waiting)
                return true;
            state.waiter = null;
        }

        return AdvanceCoroutine(state);
    }

    private bool AdvanceCoroutine(CoroutineState state)
    {
        while (!state.retired && state.stack.Count != 0)
        {
            IEnumerator current = state.stack.Peek();
            bool moved = current.MoveNext();
            if (state.retired)
                return false;
            if (!moved)
            {
                state.stack.Pop();
                if (!DisposeEnumerator(current))
                    return false;
                continue;
            }

            object? yielded = current.Current;
            if (state.retired)
                return false;
            if (yielded is null)
            {
                ulong targetFrame = m_frame + 1;
                state.waiter = scheduler => scheduler.frame < targetFrame;
                return true;
            }

            if (yielded is IEnumerator nested)
            {
                foreach (IEnumerator ancestor in state.stack)
                {
                    if (ReferenceEquals(ancestor, nested))
                        throw new InvalidOperationException("A coroutine cannot yield an enumerator already on its stack.");
                }
                state.stack.Push(nested);
                continue;
            }

            if (yielded is YieldInstruction instruction)
            {
                state.waiter = instruction.CreateWaiter(m_now, m_frame);
                return true;
            }

            if (yielded is Task task)
            {
                state.waiter = _ => !task.IsCompleted;
                return true;
            }

            if (yielded is CoroutineHandle handle)
            {
                state.waiter = scheduler => scheduler.Contains(handle);
                return true;
            }

            return false;
        }

        return false;
    }

    private void Retire(CoroutineState state)
    {
        if (state.retired)
            return;
        state.retired = true;
        m_needsPruning = true;
        m_states.Remove(state.id);
        m_liveIds.TryRemove(state.id, out _);
        state.waiter = null;
        if (!state.executing)
            DisposeStack(state);
    }

    private void RetireAll()
    {
        for (int i = 0; i < m_active.Count; i++)
        {
            if (m_states.TryGetValue(m_active[i], out CoroutineState? state))
                Retire(state);
        }
    }

    private void DisposeStack(CoroutineState state)
    {
        // Pop before calling user cleanup. A throwing or reentrant Dispose cannot cause
        // the same enumerator to be disposed twice or prevent the parent from retiring.
        state.executing = true;
        try
        {
            while (state.stack.TryPop(out IEnumerator? routine))
                DisposeEnumerator(routine);
        }
        finally
        {
            state.executing = false;
            state.owner = null;
        }
    }

    private bool DisposeEnumerator(IEnumerator routine)
    {
        if (routine is not IDisposable disposable)
            return true;
        try
        {
            disposable.Dispose();
            return true;
        }
        catch (Exception failure)
        {
            RecordFailure(failure);
            return false;
        }
    }

    private void PruneInactive()
    {
        if (!m_needsPruning || m_ticking || m_applyingCommands)
            return;
        int retained = 0;
        for (int i = 0; i < m_active.Count; i++)
        {
            long id = m_active[i];
            if (m_states.ContainsKey(id))
                m_active[retained++] = id;
        }
        if (retained != m_active.Count)
            m_active.RemoveRange(retained, m_active.Count - retained);
        m_needsPruning = false;
    }

    private void RecordFailure(Exception failure) => (m_failures ??= []).Add(failure);

    private void CompleteOperation()
    {
        if (--m_operationDepth != 0 || m_failures is null)
            return;
        List<Exception> failures = m_failures;
        m_failures = null;
        throw new AggregateException("Coroutine execution or retirement failed; all eligible cleanup was attempted.", failures);
    }

    private enum CommandKind
    {
        Start,
        Stop,
        StopByOwner,
        StopAll
    }

    private readonly struct Command(
        CommandKind kind,
        long id,
        object? owner,
        IEnumerator? routine
    ) {
        /// <summary>
        /// Gets the operation kind that determines how this value is interpreted.
        /// </summary>
        public CommandKind kind { get; } = kind;
        /// <summary>
        /// Gets the stable identity used to reference this value across subsystem boundaries.
        /// </summary>
        public long id { get; } = id;
        /// <summary>
        /// Gets the object that owns the coroutine lifetime.
        /// </summary>
        public object? owner { get; } = owner;
        /// <summary>
        /// Gets the root enumerator advanced by this coroutine.
        /// </summary>
        public IEnumerator? routine { get; } = routine;

        /// <summary>
        /// Starts value processing after validating the current state.
        /// </summary>
        /// <param name="id">
        /// The stable identity used to locate the requested value.
        /// </param>
        /// <param name="owner">
        /// The object that owns the resulting lifetime and state.
        /// </param>
        /// <param name="routine">
        /// The root enumerator owned by the scheduler after admission.
        /// </param>
        /// <returns>
        /// The validated command that represents the completed operation.
        /// </returns>
        public static Command Start(
            long id,
            object? owner,
            IEnumerator routine
        ) => new(CommandKind.Start, id, owner, routine);

        /// <summary>
        /// Stops value processing and releases operation-scoped resources.
        /// </summary>
        /// <param name="id">
        /// The stable identity used to locate the requested value.
        /// </param>
        /// <returns>
        /// The validated command that represents the completed operation.
        /// </returns>
        public static Command Stop(long id) => new(CommandKind.Stop, id, null, null);

        /// <summary>
        /// Stops by owner processing and releases operation-scoped resources.
        /// </summary>
        /// <param name="owner">
        /// The object that owns the resulting lifetime and state.
        /// </param>
        /// <returns>
        /// The validated command that represents the completed operation.
        /// </returns>
        public static Command StopByOwner(object owner) => new(CommandKind.StopByOwner, 0, owner, null);

        /// <summary>
        /// Stops all processing and releases operation-scoped resources.
        /// </summary>
        /// <returns>
        /// The validated command that represents the completed operation.
        /// </returns>
        public static Command StopAll() => new(CommandKind.StopAll, 0, null, null);
    }

    private sealed class CoroutineState(
        long id,
        object? owner,
        IEnumerator routine
    ) {
        /// <summary>
        /// Gets the stable identity used to reference this value across subsystem boundaries.
        /// </summary>
        public long id { get; } = id;
        /// <summary>
        /// Gets the object that owns the coroutine lifetime.
        /// </summary>
        public object? owner { get; set; } = owner;
        /// <summary>
        /// Gets the nested enumerator stack retained by this coroutine.
        /// </summary>
        public Stack<IEnumerator> stack { get; } = new([routine]);
        /// <summary>
        /// Gets the current continuation predicate, or null when the coroutine can advance.
        /// </summary>
        public CoroutineWaitDelegate? waiter { get; set; }
        /// <summary>
        /// Gets whether a user callback is borrowing the enumerator stack.
        /// </summary>
        public bool executing { get; set; }
        /// <summary>
        /// Gets whether call admission has closed and the remaining stack must retire.
        /// </summary>
        public bool retired { get; set; }
    }
}
