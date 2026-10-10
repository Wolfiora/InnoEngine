using Inno.References;
using Inno.Runtime.Contracts;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Extensibility.Types;
using Inno.Rendering;
using Inno.Input;
using Inno.Core.Mathematics;
using Inno.Rendering.Assets;

namespace Inno.Rendering.Runtime;

sealed partial class RenderRuntime
{
    /// <summary>
    /// Detaches this feature and releases generation-scoped state.
    /// </summary>
    private void ReleaseRendering()
    {
        if (m_disposed)
            return;
        if (m_retirement is null)
        {
            m_retirement = new RenderRetirementQueue();
            m_retirement.Add(() => m_reloadSession?.Rollback());
            m_retirement.Add(RetireGenerations);
            m_retirement.Add(m_extensions.Dispose);
            m_retirement.Add(() =>
            {
                m_graphicsSettings.Clear();
                m_requestProviders = null;
                lock (m_requestLock)
                {
                    m_acceptingCurrentFrame = false;
                    m_pendingRequests.Clear();
                    m_currentRequests.Clear();
                    m_pendingCompositions.Clear();
                    m_currentCompositions.Clear();
                }
                lock (m_contributorLock)
                {
                    m_contributors.Clear();
                    m_contributorSnapshot = RenderContributorSnapshot.empty;
                    m_frameContributors = RenderContributorSnapshot.empty;
                }
                m_scratch.Clear();
            });
            m_retirement.Add(() =>
            {
                if (!m_frameOpen)
                {
                    m_device.BeginFrame();
                    m_frameOpen = true;
                }
            });
            m_retirement.Add(targets.Dispose);
            m_retirement.Add(m_compositor.Dispose);
            m_retirement.Add(m_uploads.Dispose);
            m_retirement.Add(m_resourceService.Dispose);
            m_retirement.Add(() =>
            {
                if (m_frameOpen)
                    _ = m_device.EndFrame();
                m_frameOpen = false;
            });
            m_retirement.Add(m_uploads.EndFrame);
        }
        try
        {
            m_extensions.Retire(m_retirement);
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
    }

    private void RetireGenerations()
    {
        var resources = new RenderRetirementQueue();
        foreach (GenerationCacheEntry entry in m_generations.Values)
        {
            if (entry.lastGood is not null)
                resources.Add(entry.lastGood.Dispose);
        }
        try
        {
            m_extensions.Retire(resources);
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            m_generations.Clear();
            throw;
        }
        m_generations.Clear();
    }

    private void PruneRetiredAssets()
    {
        try
        {
            foreach ((RenderPipelineAsset asset, GenerationCacheEntry entry) in m_generations)
            {
                if (entry.hasAssetOwner && !ReferenceEquals(entry.assetIdentity.Resolve<RenderPipelineAsset>(), asset))
                    m_retiredAssets.Add(asset);
            }
            foreach (RenderPipelineAsset asset in m_retiredAssets)
            {
                m_extensions.Retire(m_generations[asset].lastGood);
                m_generations.Remove(asset);
            }
        }
        finally
        {
            m_retiredAssets.Clear();
        }
    }

}
