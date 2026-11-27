using System;
using Inno.Core.Coroutines;
using Inno.Extensibility.Types;

namespace Inno.Runtime;

internal sealed class SessionCoroutineRegistry(
    TypeCatalog types,
    RuntimeSession session,
    CoroutineScheduler scheduler
) : TypeRegistry<TypeCacheSnapshot>(types)
{
    /// <inheritdoc/>
    protected override TypeCacheSnapshot Build(TypeCacheSnapshot types) => types;

    /// <inheritdoc/>
    protected override void OnActivationCompleted(
        TypeCacheSnapshot? previous,
        TypeCacheSnapshot candidate
    ) {
        if (previous is null) return;
        using IDisposable scope = session.EnterExecutionScope();
        using IDisposable admission = SessionCoroutines.Suspend();
        scheduler.StopCoroutinesUsing(type => previous.ContainsAssembly(type.Assembly) && !candidate.ContainsAssembly(type.Assembly));
    }

    internal void EnsureRegistered() => _ = current;
}
