namespace Inno.Runtime.Contracts;

/// <summary>
/// Constructs a subsystem after its declaration has passed scope and dependency checks.
/// An attributed implementation in a deployed module is discoverable in Editor and Player.
/// </summary>
public interface IRuntimeSubsystemActivator
{
    /// <summary>
    /// Creates a subsystem owned exclusively by the supplied construction scope.
    /// </summary>
    /// <param name="context">
    /// Stable foundation services and a private factory lifetime. Register partial allocations and work here before callbacks can fail.
    /// </param>
    /// <returns>
    /// A new unattached runtime subsystem; after returning, its lifecycle belongs to the pipeline.
    /// </returns>
    IRuntimeSubsystem Create(RuntimeSubsystemContext context);
}
