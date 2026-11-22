using System;

namespace Inno.Extensibility.Types;

internal sealed class TypeRegistryRegistration(
    TypeRegistryCoordinator owner,
    Guid id
) : IDisposable
{
    private bool m_disposed;

    /// <summary>
    /// Releases the resources owned by this instance.
    /// </summary>
    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }
        m_disposed = true;
        owner.Unregister(id);
    }

    internal Guid id { get; } = id;
}
