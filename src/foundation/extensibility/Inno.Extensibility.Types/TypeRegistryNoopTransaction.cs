namespace Inno.Extensibility.Types;

internal sealed class TypeRegistryNoopTransaction : ITypeRegistryTransaction
{
    /// <summary>
    /// Makes the prepared value active at the owning subsystem's safety point.
    /// </summary>
    public void Activate() {}

    /// <summary>
    /// Finalizes candidate activation and releases temporary transaction state.
    /// </summary>
    public void Complete() {}

    /// <summary>
    /// Restores the state captured before the current transaction began.
    /// </summary>
    public void Rollback() {}

    internal static TypeRegistryNoopTransaction instance { get; } = new();
}
