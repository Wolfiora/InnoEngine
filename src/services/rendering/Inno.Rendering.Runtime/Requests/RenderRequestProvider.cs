using System;
using Inno.Core.Execution;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Produces arbitrary render requests without prescribing a scene or rendering model.
/// </summary>
[StableTypeID("5c4302ea-8da5-5627-8fbe-f34ee7b2aec1")]
public abstract class RenderRequestProvider : IDisposable
{
    private bool m_disposed;

    /// <summary>
    /// Submits zero or more requests for the current frame.
    /// </summary>
    /// <param name="context">
    /// Frame-scoped provider context.
    /// </param>
    public abstract void Submit(RenderRequestProviderContext context);

    /// <summary>
    /// Releases generation-scoped provider state.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Provider work is still active. The owner must retain this instance and retry before releasing dependencies.
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
    /// Always true for explicit disposal.
    /// </param>
    /// <exception cref="RetirementPendingException">
    /// Retirement cannot complete yet; subsequent calls resume this hook with its remaining owned resources.
    /// </exception>
    protected virtual void Dispose(bool disposing) {}
}
