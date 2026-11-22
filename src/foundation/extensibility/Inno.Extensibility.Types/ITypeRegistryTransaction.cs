namespace Inno.Extensibility.Types;

internal interface ITypeRegistryTransaction
{
    /// <summary>
    /// Makes the prepared value active at the owning subsystem's safety point.
    /// </summary>
    void Activate();

    /// <summary>
    /// Finalizes candidate activation and releases temporary transaction state.
    /// </summary>
    void Complete();

    /// <summary>
    /// Restores the state captured before the current transaction began.
    /// </summary>
    void Rollback();
}
