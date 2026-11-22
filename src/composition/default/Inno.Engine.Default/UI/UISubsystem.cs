using Inno.Runtime.Contracts;
using Inno.UI.Runtime;

namespace Inno.Engine.Default;

internal static class UISubsystem
{
    [RuntimeSubsystemRegistration("inno.runtime.ui")]
    internal static IRuntimeSubsystemFactory Create(EngineSessionComposition context)
        => new UIRuntimeFactory(_ => new UIRuntime(
            context.adapters.ui.CreateBackend(context.selection.ui),
            context.artifacts
        ));
}
