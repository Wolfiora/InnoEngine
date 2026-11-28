using System;
using System.Threading.Tasks;

namespace Inno.Core.Coroutines;

/// <summary>
/// Waits until a task is completed.
/// </summary>
/// <param name="task">
/// The task used to initialize this instance.
/// </param>
public sealed class WaitForTask(Task task) : YieldInstruction
{
    private readonly Func<double, ulong, bool> m_wait = (
        _,
        _
    ) => !task.IsCompleted;

    /// <summary>
    /// Gets the awaited task.
    /// </summary>
    public Task task { get; } = task ?? throw new ArgumentNullException(nameof(task));

    /// <inheritdoc/>
    protected internal override Func<double, ulong, bool> CreateWaiter() => m_wait;
}
