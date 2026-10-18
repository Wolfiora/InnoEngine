using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Serialization;
using Inno.Rendering;
using Inno.Rendering.Assets;
using System;
using System.Collections.Generic;

namespace Inno.Rendering.Runtime;

/// <summary>
/// Builds frame-local passes without prescribing a rendering model.
/// </summary>
[Inno.Extensibility.Types.StableTypeId("f3736feb-4238-54b6-9fe9-c40ffb095134")]
public abstract class RenderPipeline : IDisposable
{
    private bool m_disposed;

    /// <summary>
    /// Applies reload-safe pipeline settings to this generation.
    /// </summary>
    /// <param name="state">
    /// Stable type identity and neutral property bytes.
    /// </param>
    /// <param name="settings">
    /// Native context bound to the canonical Pipeline asset's actual owner.
    /// </param>
    public void Configure(
        SerializedRenderExtensionState state,
        RenderExtensionStateContext settings
    ) {
        ArgumentNullException.ThrowIfNull(settings);
        OnConfigure(state, settings);
    }

    /// <summary>
    /// Builds all passes for one request.
    /// </summary>
    /// <param name="context">
    /// Frame-scoped pipeline context.
    /// </param>
    public abstract void Build(RenderPipelineContext context);

    /// <summary>
    /// Releases generation-scoped pipeline state.
    /// </summary>
    /// <exception cref="RetirementPendingException">
    /// Pipeline work is still active. The owner must retain this instance and retry before releasing dependencies.
    /// </exception>
    public void Dispose()
    {
        if (m_disposed)
            return;
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
    /// Reads pipeline-owned settings from neutral state.
    /// </summary>
    /// <param name="state">
    /// Reload-safe extension state.
    /// </param>
    /// <param name="settings">
    /// Native context bound to the canonical Pipeline asset's actual owner.
    /// </param>
    protected virtual void OnConfigure(
        SerializedRenderExtensionState state,
        RenderExtensionStateContext settings
    ) { }

    /// <summary>
    /// Releases managed generation-scoped state.
    /// </summary>
    /// <param name="disposing">
    /// Always true for explicit disposal.
    /// </param>
    /// <exception cref="RetirementPendingException">
    /// Retirement cannot complete yet; subsequent calls resume this hook with its remaining owned resources.
    /// </exception>
    protected virtual void Dispose(bool disposing) { }
}

