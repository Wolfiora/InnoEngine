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
    /// Submits validated work to the active backend for ordered processing.
    /// </summary>
    /// <param name="request">
    /// The validated immutable request that defines this operation.
    /// </param>
    /// <exception cref="InvalidOperationException">
    /// The bounded frame queue has reached capacity, retirement has started, or generation cleanup has faulted
    /// the owner. The request was not accepted.
    /// </exception>
    /// <exception cref="ObjectDisposedException">
    /// The rendering owner has retired.
    /// </exception>
    public void Submit(RenderRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        lock (m_requestLock)
        {
            EnsureActive();
            if (m_currentRequests.Count + m_pendingRequests.Count
                + m_currentCompositions.Count + m_pendingCompositions.Count >= C_MAX_QUEUED_REQUESTS)
                throw new InvalidOperationException("The rendering request queue is at capacity.");
            (m_acceptingCurrentFrame ? m_currentRequests : m_pendingRequests).Add(request);
        }
    }

    /// <summary>
    /// Submits one or more independently rendered model layers for premultiplied-alpha output composition.
    /// </summary>
    /// <param name="name">
    /// Stable frame-local composition identity.
    /// </param>
    /// <param name="target">
    /// Final host-owned output target.
    /// </param>
    /// <param name="viewport">
    /// Output viewport in physical pixels.
    /// </param>
    /// <param name="format">
    /// Shared sampled color format required by every layer.
    /// </param>
    /// <param name="layers">
    /// Ordered requests, one per rendering model.
    /// </param>
    /// <param name="priority">
    /// Ascending scheduling priority relative to ordinary requests.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The layer set or format is invalid.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The bounded frame queue is full or retirement has started.
    /// </exception>
    public void SubmitComposition(
        string name,
        RenderTarget target,
        RenderViewport viewport,
        RenderTextureFormat format,
        IReadOnlyList<RenderRequest> layers,
        int priority = 0
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentNullException.ThrowIfNull(layers);
        if (layers.Count == 0 || layers.Any(static layer => layer is null))
            throw new ArgumentException("A composition requires at least one non-null model layer.", nameof(layers));
        if (layers.Any(layer => layer.viewport != viewport || layer.target != target))
            throw new ArgumentException("All composition layers must name the same final target and viewport.", nameof(layers));
        if (!m_device.capabilities.SupportsRenderTarget(format)
            || !m_device.capabilities.SupportsSampled(format))
            throw new ArgumentException($"The device cannot sample and render model layers in '{format}'.", nameof(format));
        var composition = new CompositionRequest(name, target, viewport, format,
            layers.ToArray(), priority);
        lock (m_requestLock)
        {
            EnsureActive();
            if (m_currentRequests.Count + m_pendingRequests.Count
                + m_currentCompositions.Count + m_pendingCompositions.Count >= C_MAX_QUEUED_REQUESTS)
                throw new InvalidOperationException("The rendering request queue is at capacity.");
            (m_acceptingCurrentFrame ? m_currentCompositions : m_pendingCompositions).Add(composition);
        }
    }

    private bool TryBuildRequest(
        RenderGraphBuilder graph,
        RenderRequest request,
        int requestIndex,
        bool preservePresentationTarget,
        RenderTextureHandle outputOverride = default
    ) {
        if (request.target.kind == RenderTargetKind.Backbuffer && !outputOverride.isValid && !m_primaryPresentationSize.HasValue)
            return false;
        RenderPipelineAsset? asset = request.pipeline ?? m_graphicsSettings.defaultPipeline;
        if (asset is null)
        {
            PublishFrameIssue(new Diagnostic(
                "RENDER_PIPELINE_UNAVAILABLE",
                $"Render request '{request.name}' has no pipeline. The frame and UI remain active.",
                DiagnosticSeverity.Warning,
                request.name));
            return false;
        }

        if (!TryGetGeneration(asset, out RenderPipelineGeneration? generation))
            return false;

        try
        {
            using RenderGraphMutationScope mutation = graph.BeginMutationScope();
            using RenderGraphNameScope names = graph.BeginNameScope(
                $"Request[{requestIndex}] {request.name}");
            RenderTextureHandle outputTexture = outputOverride.isValid
                ? outputOverride
                : request.target.kind == RenderTargetKind.Texture
                ? targets.Import(
                    graph,
                    request.target.texture
                        ?? throw new InvalidOperationException("A texture target requires a RenderTexture."))
                : default;
            if (outputTexture.isValid && !outputOverride.isValid)
                graph.MarkOutput(outputTexture);

            var context = new RenderPipelineContext(
                request,
                asset,
                graph,
                m_device.capabilities,
                new RenderResourceMap(),
                m_diagnostics,
                m_resourceService,
                m_uploads,
                m_frameIndex,
                preservePresentationTarget,
                outputTexture);
            generation!.pipeline.Build(context);
            AddFeatures(asset, generation.features, context);
            RenderGraphValidationResult validation = graph.Validate();
            if (!validation.isValid)
            {
                PublishGraphDiagnostics(validation.diagnostics, request.name);
                return false;
            }
            mutation.Commit();
            return true;
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            PublishFrameIssue(new Diagnostic(
                "RENDER_REQUEST_FAILED",
                $"Render request '{request.name}' was isolated after failure: {exception}",
                DiagnosticSeverity.Error,
                request.name));
            return false;
        }
    }

    private void EnsureRequestProviders()
    {
        m_requestProviders = m_extensions.extensions.providers;
    }

    private sealed record CompositionRequest(
        string name,
        RenderTarget target,
        RenderViewport viewport,
        RenderTextureFormat format,
        RenderRequest[] layers,
        int priority
    );

    private readonly record struct ScheduledWork(
        int order,
        int priority,
        string name,
        RenderRequest? request,
        CompositionRequest? composition
    );

}
