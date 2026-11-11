namespace Inno.Core.Coroutines;

internal delegate bool CoroutineWaitDelegate(CoroutineScheduler scheduler);

/// <summary>
/// Base type for the scheduler's built-in coroutine wait instructions.
/// </summary>
/// <remarks>
/// Waiter creation is internal to this assembly. External callers use the supplied instruction types
/// or compose supported yields in an enumerator; this is not an externally implementable extension point.
/// </remarks>
public abstract class YieldInstruction
{
    internal abstract CoroutineWaitDelegate CreateWaiter(
        double now,
        ulong frame
    );
}
