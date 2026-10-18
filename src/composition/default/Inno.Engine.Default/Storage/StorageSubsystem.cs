using Inno.Runtime.Contracts;
using Inno.Storage.Runtime;

namespace Inno.Engine.Default;

internal static class StorageSubsystem
{
    [RuntimeSubsystemRegistration("inno.runtime.storage")]
    internal static IRuntimeSubsystemFactory Create(EngineSessionComposition context)
        => new StorageRuntimeFactory(_ => context.createStorage());
}
