namespace Inno.Runtime.Contracts;

/// <summary>
/// Creates one runtime subsystem instance for each declared Host or Session owner.
/// </summary>
public interface IRuntimeSubsystemFactory : IRuntimeSubsystemActivator
{
    /// <summary>
    /// Gets stable ordering and dependency metadata without creating runtime state.
    /// </summary>
    RuntimeSubsystemDescriptor descriptor { get; }
}
