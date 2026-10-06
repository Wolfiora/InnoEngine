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
    /// Registers frame-final work without transferring frame ownership.
    /// </summary>
    /// <param name="contributor">
    /// Contributor invoked after all user pipeline requests.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when the instance is already registered.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Retirement has started or generation cleanup has faulted the owner.
    /// </exception>
    public void RegisterContributor(IRenderFrameGraphContributor contributor)
    {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(contributor);
        lock (m_contributorLock)
        {
            if (m_contributors.Contains(contributor))
                throw new ArgumentException("Frame graph contributor is already registered.", nameof(contributor));
            m_contributors.Add(contributor);
            m_contributorSnapshot = new RenderContributorSnapshot(m_contributors);
        }
    }

    /// <summary>
    /// Stops invoking a previously registered frame-final contributor.
    /// </summary>
    /// <param name="contributor">
    /// Contributor to remove.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when registration existed and was removed.
    /// </returns>
    public bool UnregisterContributor(IRenderFrameGraphContributor contributor)
    {
        ArgumentNullException.ThrowIfNull(contributor);
        lock (m_contributorLock)
        {
            if (!m_contributors.Remove(contributor))
                return false;
            m_contributorSnapshot = new RenderContributorSnapshot(m_contributors);
            return true;
        }
    }

    private void PrepareContributors()
    {
        m_scratch.contributors.Clear();
        foreach (RenderContributorSnapshot.Entry entry in m_frameContributors.entries)
        {
            try
            {
                entry.contributor.PrepareFrame(m_frameIndex);
                m_scratch.contributors.Add(entry);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                PublishContributorFailure(entry.contributor, "prepare", exception);
            }
        }
    }

    private void AddContributors(
        RenderGraphBuilder graph,
        IReadOnlyList<RenderContributorSnapshot.Entry> contributors
    ) {
        for (int index = 0; index < contributors.Count; index++)
        {
            RenderContributorSnapshot.Entry entry = contributors[index];
            IRenderFrameGraphContributor contributor = entry.contributor;
            using RenderGraphMutationScope mutation = graph.BeginMutationScope();
            using RenderGraphNameScope names = graph.BeginNameScope(entry.scopeName);
            try
            {
                contributor.AddRenderPasses(graph, m_frameIndex);
                RenderGraphValidationResult validation = graph.Validate();
                if (!validation.isValid)
                {
                    PublishGraphDiagnostics(validation.diagnostics, contributor.GetType().Name);
                    continue;
                }
                mutation.Commit();
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                PublishContributorFailure(contributor, "graph build", exception);
            }
        }
    }

}
