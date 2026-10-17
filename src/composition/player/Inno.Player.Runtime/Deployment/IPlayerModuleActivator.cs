using Inno.Extensibility.Modules;
using Inno.Runtime;

namespace Inno.Player.Runtime;

/// <summary>
/// Activates a validated logical code deployment through the common module transaction boundary.
/// </summary>
public interface IPlayerModuleActivator
{
    /// <summary>
    /// Verifies the code closure and publishes its module contributions atomically.
    /// </summary>
    /// <param name="modules">
    /// The engine-owned module catalog and generation coordinator.
    /// </param>
    /// <param name="deployment">
    /// The frozen logical deployment, independent of DLL directories or managed runtime technology.
    /// </param>
    /// <exception cref="System.IO.InvalidDataException">
    /// The runtime manifest does not match the linked code closure.
    /// </exception>
    void Activate(
        ModuleHost modules,
        GameCodeDeployment deployment
    );
}
