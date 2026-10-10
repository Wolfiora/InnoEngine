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
    /// Captures snapshots and binds service façades.
    /// </summary>
    /// <param name="frame">
    /// The variable frame timing.
    /// </param>
    protected override void OnBeginFrame(RuntimeFrame frame)
    {
        m_frameInput = InputExecutionContext.TryGet(out IInputService? input)
            ? input!.snapshot : InputSnapshot.empty;
        OwnFrameScope(EnterExecutionScope());
    }
    /// <summary>
    /// Opens resources required for this frame's output.
    /// </summary>
    /// <param name="frame">
    /// The variable frame timing.
    /// </param>
    protected override void OnPrepareOutput(RuntimeFrame frame) => BeginRendering(frame.unscaledDeltaTime);
    /// <summary>
    /// Collects output commands without executing managed code on a native realtime callback.
    /// </summary>
    /// <param name="frame">
    /// The variable frame timing.
    /// </param>
    protected override void OnProduceOutput(RuntimeFrame frame) => CollectRendering(frame.unscaledDeltaTime);
    /// <summary>
    /// Submits output and closes output-specific temporary resources.
    /// </summary>
    /// <param name="frame">
    /// The variable frame timing.
    /// </param>
    protected override void OnCompleteOutput(RuntimeFrame frame) => CompleteRendering(frame.unscaledDeltaTime);

    /// <summary>
    /// Prepares frame-scoped state before render requests are submitted.
    /// </summary>
    /// <param name="deltaTime">
    /// The elapsed frame time in seconds.
    /// </param>
    private void BeginRendering(float deltaTime)
    {
        EnsureActive();
        PruneRetiredGenerations();
        EnsureRequestProviders();
        m_currentFrameIssues.Clear();
        m_device.BeginFrame();
        m_frameOpen = true;
        lock (m_contributorLock)
            m_frameContributors = m_contributorSnapshot;
        try
        {
            m_resourceService.BeginFrame(m_frameIndex);
            m_uploads.BeginFrame(m_frameIndex);
            targets.PrepareFrame();
            m_primaryPresentationSize = m_device.primaryPresentationSize;
            if (m_primaryPresentationSize is RenderPresentationSize size && !size.isValid)
                throw new InvalidOperationException("The render device reported an invalid primary presentation extent.");
            m_primaryPresentationViewport = m_primaryPresentationSize is RenderPresentationSize available
                ? ResolvePrimaryPresentationViewport(available) : null;
            lock (m_requestLock)
            {
                m_currentRequests.Clear();
                m_currentRequests.AddRange(m_pendingRequests);
                m_pendingRequests.Clear();
                m_currentCompositions.Clear();
                m_currentCompositions.AddRange(m_pendingCompositions);
                m_pendingCompositions.Clear();
                m_acceptingCurrentFrame = true;
            }
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            m_frameContributors = RenderContributorSnapshot.empty;
            m_scratch.Clear();
            throw;
        }
        catch
        {
            try
            {
                _ = m_device.EndFrame();
            }
            finally
            {
                m_resourceService.EndMutation();
                m_frameOpen = false;
                m_frameContributors = RenderContributorSnapshot.empty;
                m_scratch.Clear();
            }
            throw;
        }
    }

    /// <summary>
    /// Submits render work for the current frame.
    /// </summary>
    /// <param name="deltaTime">
    /// The elapsed frame time in seconds.
    /// </param>
    private void CollectRendering(float deltaTime)
    {
        if (!m_frameOpen || m_requestProviders is null)
            return;

        using IDisposable executionScope = EnterExecutionScope();
        using ContentReadScope content = GetFrameContentScope();
        InputSnapshot input = m_inputSnapshotProvider?.Invoke() ?? m_frameInput;
        RenderOutputInput outputInput = CreatePrimaryOutputInput(input);
        var context = new RenderRequestProviderContext(
            this,
            content,
            m_device.capabilities,
            m_primaryPresentationSize,
            m_primaryPresentationViewport,
            m_frameIndex,
            deltaTime,
            this,
            outputInput);
        if (m_primaryModelOutputEnabled && m_primaryPresentationViewport is RenderViewport viewport)
            CollectModels(new RenderOutputSession("primary", content, viewport,
                m_frameIndex, deltaTime, this, outputInput, m_primaryRoute));
        foreach (RenderExtensionRegistry.RequestProviderEntry entry in m_requestProviders.providers)
        {
            try
            {
                entry.provider.Submit(context);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                PublishFrameIssue(new Diagnostic(
                    "RENDER_REQUEST_PROVIDER_FAILED",
                    $"Render request provider '{entry.id}' was isolated after failure: {exception}",
                    DiagnosticSeverity.Error,
                    entry.id));
            }
        }
    }

    private ContentReadScope GetFrameContentScope()
    {
        if (m_contentScopeProvider is null)
            return ContentReadScope.empty;
        try
        {
            return m_contentScopeProvider()
                ?? throw new InvalidOperationException("The host content-scope provider returned null.");
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            PublishFrameIssue(new Diagnostic(
                "RENDER_CONTENT_SCOPE_FAILED",
                $"The host content scope failed and an empty scope was used for this frame: {exception}",
                DiagnosticSeverity.Error,
                "RenderRuntime"));
            return ContentReadScope.empty;
        }
    }

    /// <summary>
    /// Completes the current render frame and releases frame-scoped state.
    /// </summary>
    /// <param name="deltaTime">
    /// The elapsed frame time in seconds.
    /// </param>
    private void CompleteRendering(float deltaTime)
    {
        _ = deltaTime;
        if (!m_frameOpen)
            return;

        int executedViewCount = 0;
        int culledPassCount = 0;
        int graphCompileCount = 0;
        bool retirementPending = false;
        try
        {
            lock (m_requestLock)
                m_acceptingCurrentFrame = false;

            RenderGraphBuilder graph = CreateGraph();
            AddPrimaryPresentationBackground(graph);
            PrepareContributors();
            CompleteViewContentFrame();
            int requestIndex = 0;
            RenderPresentationComposer presentation = m_scratch.presentation;
            foreach (RenderRequest request in m_currentRequests)
                m_scratch.scheduled.Add(new ScheduledWork(m_scratch.scheduled.Count, request.priority, request.name, request, null));
            foreach (CompositionRequest composition in m_currentCompositions)
                m_scratch.scheduled.Add(new ScheduledWork(m_scratch.scheduled.Count, composition.priority, composition.name, null, composition));
            m_scratch.scheduled.Sort(static (
                left,
                right
            ) =>
            {
                int priority = left.priority.CompareTo(right.priority);
                if (priority != 0)
                    return priority;
                int name = StringComparer.Ordinal.Compare(left.name, right.name);
                return name != 0 ? name : left.order.CompareTo(right.order);
            });
            foreach (ScheduledWork work in m_scratch.scheduled)
            {
                if (work.composition is CompositionRequest composition)
                {
                    BuildComposition(graph, composition, ref requestIndex);
                    continue;
                }
                RenderRequest request = work.request!;
                bool preservePresentationTarget = presentation.MustPreserve(request);
                if (TryBuildRequest(graph, request, requestIndex++, preservePresentationTarget))
                    presentation.Commit(request);
            }

            AddContributors(graph, m_scratch.contributors);
            graphCompileCount++;
            RenderGraphCompileResult result = graph.Compile();
            culledPassCount = result.culledPassCount;
            PublishGraphDiagnostics(result.diagnostics, "Frame");
            if (result.graph is not null)
            {
                executedViewCount = result.graph.passes.Count;
                if (executedViewCount != 0)
                {
                    m_resourceService.EndMutation();
                    m_device.Execute(result.graph, m_frameIndex);
                    targets.MarkWrittenOutputs(result.graph);
                    m_resourceService.BeginMutation();
                }
            }
            m_resourceService.SweepUnused();
            m_uploads.SweepUnused();
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            retirementPending = true;
            throw;
        }
        finally
        {
            m_scratch.Clear();
            m_frameContributors = RenderContributorSnapshot.empty;
            if (!retirementPending)
                EndRenderingFrame(executedViewCount, culledPassCount, graphCompileCount);
        }
    }

    private void EndRenderingFrame(
        int executedViewCount,
        int culledPassCount,
        int graphCompileCount
    ) {
        RenderDeviceFrameCounters counters = m_device.frameCounters;
        RenderDeviceAllocationCounters? allocations = m_device.allocationCounters;
        m_resourceService.EndMutation();
        try
        {
            _ = m_device.EndFrame();
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch
        {
            CompleteFrameState();
            throw;
        }
        CompleteFrameState();

        void CompleteFrameState()
        {
            m_frameOpen = false;
            ReconcileFrameIssues();
            m_uploads.EndFrame();
            m_currentRequests.Clear();
            m_currentCompositions.Clear();
            m_frameIndex++;
            m_graphicsSettings.frameStatistics = new RenderFrameStatistics(
                m_frameIndex, executedViewCount, counters.drawCount, counters.dispatchCount, culledPassCount,
                allocations, graphCompileCount);
        }
    }

    private void CompleteViewContentFrame()
    {
        foreach (RenderExtensionRegistry.ContentSourceEntry entry in m_extensions.extensions.sources.sources)
        {
            if (entry.source is not IViewContentFrameSource source)
                continue;
            try
            {
                source.CompleteFrame(m_frameIndex);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                PublishFrameIssue(new Diagnostic("RENDER_VIEW_CONTENT_FRAME_FAILED",
                    $"View content source '{entry.id}' failed to complete its frame: {exception}",
                    DiagnosticSeverity.Error, entry.id));
            }
        }
    }

    private RenderGraphBuilder CreateGraph()
    {
        m_graphGeneration++;
        if (m_graphGeneration == 0)
            m_graphGeneration++;
        return new RenderGraphBuilder(m_graphGeneration, m_device.capabilities);
    }

    private sealed class ViewContentSink : IViewContentSink
    {
        private readonly List<ViewContentItem> m_items = [];

        internal IReadOnlyList<ViewContentItem> items => m_items;

        /// <summary>
        /// Submits validated work to the active backend for ordered processing.
        /// </summary>
        /// <param name="item">
        /// The stored item associated with the validated handle.
        /// </param>
public void Submit(ViewContentItem item)
        {
            ArgumentNullException.ThrowIfNull(item);
            m_items.Add(item);
        }
    }

}
