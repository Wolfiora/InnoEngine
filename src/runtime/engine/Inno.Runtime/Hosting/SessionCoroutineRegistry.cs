using System;
using Inno.Core.Coroutines;
using Inno.Extensibility.Types;

namespace Inno.Runtime;

internal sealed class SessionCoroutineRegistry(
    TypeCatalog types,
    RuntimeSession session
) : TypeRegistry<CoroutineScheduler>(types)
{
    internal CoroutineScheduler scheduler => current;

    protected override CoroutineScheduler Build(TypeCacheSnapshot types) => new();

    protected override void DisposeSnapshot(CoroutineScheduler snapshot)
    {
        using IDisposable scope = session.EnterExecutionScope();
        using IDisposable admission = SessionCoroutines.Suspend();
        snapshot.Dispose();
    }
}
