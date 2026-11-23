using System;
using Inno.Core.Execution;

namespace Inno.Audio;

/// <summary>
/// Converts arbitrary host-owned content into backend-neutral emitter and listener snapshots.
/// </summary>
public abstract class AudioContentProvider : IDisposable
{
    private bool m_disposed;

    /// <summary>
    /// Submits current immutable audio snapshots on the host control thread.
    /// </summary>
    /// <param name="context">
    /// Update-scoped content and snapshot collector.
    /// </param>
    public abstract void Submit(AudioContentProviderContext context);

    /// <summary>
    /// Releases generation-scoped provider state.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Owned work is still draining. The generation owner must retain this provider and retry disposal.
    /// </exception>
    public void Dispose()
    {
        if (m_disposed)
        {
            return;
        }
        try
        {
            Dispose(true);
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            m_disposed = true;
            throw;
        }
        m_disposed = true;
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases managed generation-scoped state.
    /// </summary>
    /// <param name="disposing">
    /// Always <see langword="true"/> for explicit disposal.
    /// </param>
    protected virtual void Dispose(bool disposing) {}
}
