using System;

namespace Inno.Core.Coroutines;

/// <summary>
/// Creates a wait condition each time a coroutine yields this instruction.
/// </summary>
public abstract class YieldInstruction
{
    /// <summary>
    /// Creates the predicate for one suspension. Mutable progress belongs to this invocation.
    /// </summary>
    /// <returns>
    /// A predicate receiving elapsed scheduler seconds and frames since suspension.
    /// The scheduler polls from the next tick until the predicate returns false, the coroutine stops,
    /// or the predicate throws. Stopping releases the predicate without polling it again.
    /// </returns>
    protected internal abstract Func<double, ulong, bool> CreateWaiter();
}
