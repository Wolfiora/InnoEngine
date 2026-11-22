using System;
using Inno.Core.Execution;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

[StableTypeID("0f145d08-b261-54bf-92aa-5ee6605d819e")]
internal static class GraphicsSettingsExecutionContext
{
    private static readonly ExecutionSlot<GraphicsSettingsState> CURRENT = new("state");

    internal static GraphicsSettingsState current
        => currentOrNull
           ?? throw new InvalidOperationException("No rendering runtime is bound to the current execution context.");

    internal static GraphicsSettingsState? currentOrNull => CURRENT.TryGet(out GraphicsSettingsState? state) ? state : null;

    internal static IDisposable Enter(GraphicsSettingsState state)
    {
        ArgumentNullException.ThrowIfNull(state);
        return CURRENT.Enter(state);
    }
}
