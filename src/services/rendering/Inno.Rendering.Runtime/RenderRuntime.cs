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

/// <summary>
/// Owns the sole graphics frame boundary and executes model-neutral render requests.
/// </summary>
public sealed partial class RenderRuntime : RuntimeSubsystem, IRenderRequestSink, IViewContentCollector
{
    private const int C_MAX_QUEUED_REQUESTS = 4096;
    private static readonly RenderPhaseId S_PRESENTATION_BACKGROUND_PHASE = new(
        "inno.runtime.presentation-background");
    private static readonly RenderClearColor S_PRESENTATION_BACKGROUND_COLOR = new(0f, 0f, 0f, 1f);

    private readonly object m_requestLock = new();
    private readonly object m_contributorLock = new();
    private readonly IRenderDevice m_device;
    private readonly IDiagnosticReporter m_diagnostics;
    private readonly Func<ContentReadScope>? m_contentScopeProvider;
    private readonly Func<InputSnapshot>? m_inputSnapshotProvider;
    private readonly Func<RenderPresentationSize?>? m_primaryInputSurfaceSizeProvider;
    private readonly Func<RenderPresentationSize, RenderViewport>? m_primaryPresentationViewportProvider;
    private readonly RenderResourceService m_resourceService;
    private readonly RenderFrameUploadService m_uploads;
    private readonly RenderLayerCompositor m_compositor;
    private readonly RenderExtensionRegistry m_extensions;
    private readonly GraphicsSettingsState m_graphicsSettings;
    private readonly Dictionary<RenderPipelineAsset, GenerationCacheEntry> m_generations = [];
    private readonly List<RenderPipelineAsset> m_retiredAssets = [];
    private readonly List<RenderRequest> m_pendingRequests = [];
    private readonly List<RenderRequest> m_currentRequests = [];
    private readonly List<CompositionRequest> m_pendingCompositions = [];
    private readonly List<CompositionRequest> m_currentCompositions = [];
    private readonly List<IRenderFrameGraphContributor> m_contributors = [];
    private readonly RenderFrameScratch m_scratch = new();
    private RenderContributorSnapshot m_contributorSnapshot = RenderContributorSnapshot.empty;
    private RenderContributorSnapshot m_frameContributors = RenderContributorSnapshot.empty;
    private long m_prunedTypeCacheVersion = -1;
    private readonly Dictionary<FrameIssueKey, Diagnostic> m_previousFrameIssues = [];
    private readonly Dictionary<FrameIssueKey, Diagnostic> m_currentFrameIssues = [];
    private ulong m_frameIndex;
    private uint m_graphGeneration;
    private RenderExtensionRegistry.RequestProviderGeneration? m_requestProviders;
    private bool m_acceptingCurrentFrame;
    private bool m_disposed;
    private RenderRetirementQueue? m_retirement;
    private bool m_frameOpen;
    private RenderRuntimeReloadSession? m_reloadSession;
    private RenderPresentationSize? m_primaryPresentationSize;
    private RenderViewport? m_primaryPresentationViewport;
    private RenderOutputRoute? m_primaryRoute;
    private bool m_primaryModelOutputEnabled = true;
    private InputSnapshot m_frameInput = InputSnapshot.empty;

    /// <summary>
    /// Creates a render runtime without installing any concrete pipeline.
    /// </summary>
    /// <param name="device">
    /// Active backend-neutral device.
    /// </param>
    /// <param name="types">
    /// The type catalog that owns render extension generations.
    /// </param>
    /// <param name="diagnostics">
    /// Owner-bound structured diagnostic producer.
    /// </param>
    /// <param name="contributors">
    /// Optional frame-final contributors such as an ImGui backend.
    /// </param>
    /// <param name="targetArtifacts">
    /// Provider of immutable target artifacts. Editor hosts may provide an asynchronous authoring-backed
    /// implementation, while Player hosts provide a source-free deployment implementation.
    /// </param>
    /// <param name="contentScopeProvider">
    /// Optional host callback that supplies explicit current-frame content without coupling Rendering to Scene or documents.
    /// </param>
    /// <param name="primaryPresentationViewportProvider">
    /// Optional host callback that selects the content region within the primary presentation surface.
    /// The complete surface is used when omitted.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The contributor sequence contains a null entry or the same contributor more than once.
    /// No rendering generation is registered in this case.
    /// </exception>
    /// <param name="resourceLimits">
    /// Finite native cache and asynchronous readback limits; omitted values use the engine defaults.
    /// </param>
    /// <param name="inputSnapshotProvider">
    /// Optional host callback that reads the completed session input snapshot when output is collected.
    /// </param>
    /// <param name="primaryInputSurfaceSizeProvider">
    /// Optional logical input extent used for pointer scaling; returns null while input has no surface.
    /// </param>
    /// <param name="compositionProgramProvider">
    /// Backend-specific program provider used when the host composites multiple render layers.
    /// </param>
    public RenderRuntime(
        TypeCatalog types,
        IRenderDevice device,
        IDiagnosticReporter diagnostics,
        IEnumerable<IRenderFrameGraphContributor>? contributors = null,
        IRenderTargetArtifactProvider? targetArtifacts = null,
        Func<ContentReadScope>? contentScopeProvider = null,
        Func<RenderPresentationSize, RenderViewport>? primaryPresentationViewportProvider = null,
        RenderResourceLimits? resourceLimits = null,
        Func<InputSnapshot>? inputSnapshotProvider = null,
        Func<RenderPresentationSize?>? primaryInputSurfaceSizeProvider = null,
        IRenderLayerCompositionProgramProvider? compositionProgramProvider = null
    ) {
        ArgumentNullException.ThrowIfNull(types);
        resourceLimits ??= new RenderResourceLimits();
        resourceLimits.Validate();
        m_device = device ?? throw new ArgumentNullException(nameof(device));
        m_diagnostics = diagnostics ?? throw new ArgumentNullException(nameof(diagnostics));
        if (contributors is not null)
        {
            foreach (IRenderFrameGraphContributor contributor in contributors)
            {
                if (contributor is null || m_contributors.Contains(contributor))
                    throw new ArgumentException("Initial contributors must contain unique non-null instances.", nameof(contributors));
                m_contributors.Add(contributor);
            }
        }
        m_contributorSnapshot = new RenderContributorSnapshot(m_contributors);
        m_contentScopeProvider = contentScopeProvider;
        m_inputSnapshotProvider = inputSnapshotProvider;
        m_primaryInputSurfaceSizeProvider = primaryInputSurfaceSizeProvider;
        m_primaryPresentationViewportProvider = primaryPresentationViewportProvider;
        m_extensions = new RenderExtensionRegistry(types);
        m_graphicsSettings = new GraphicsSettingsState(m_device.capabilities);
        m_resourceService = new RenderResourceService(
            m_device,
            m_diagnostics,
            targetArtifacts,
            resourceLimits);
        m_uploads = new RenderFrameUploadService(m_device, resourceLimits);
        m_compositor = new RenderLayerCompositor(m_device, compositionProgramProvider);
        targets = new RenderTargetStore(device, resourceLimits.targets);
    }

    /// <summary>
    /// Gets persistent offscreen target services for viewport presentation.
    /// </summary>
    public RenderTargetStore targets { get; }

    /// <summary>
    /// Gets backend-neutral persistent resource resolution for host-owned previews and rendering integrations.
    /// </summary>
    public IRenderResourceService resources => m_resourceService;

    /// <summary>
    /// Gets the generation-scoped collector used by rendering models for world content.
    /// </summary>
    public IViewContentCollector viewContent => this;

    /// <summary>
    /// Gets the monotonic index of the current or most recently completed output frame.
    /// </summary>
    public ulong currentFrameIndex => m_frameIndex;

    /// <summary>
    /// Collects frame requests from rendering models and registered providers.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    /// <returns>
    /// An immutable snapshot of the values selected by the operation.
    /// </returns>
    public IReadOnlyList<ViewContentItem> Collect(ViewContentContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        EnsureActive();
        var sink = new ViewContentSink();
        foreach (RenderExtensionRegistry.ContentSourceEntry entry in m_extensions.extensions.sources.sources)
        {
            if (context.sourceIds is { } sourceIds
                && !sourceIds.Contains(entry.id, StringComparer.Ordinal))
                continue;
            try
            {
                entry.source.Collect(context, sink);
            }
            catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
            {
                throw;
            }
            catch (Exception exception)
            {
                PublishFrameIssue(new Diagnostic(
                    "RENDER_VIEW_CONTENT_SOURCE_FAILED",
                    $"View content source '{entry.id}' was isolated after failure: {exception}",
                    DiagnosticSeverity.Error,
                    entry.id));
            }
        }
        return sink.items;
    }

    /// <summary>
    /// Gets the non-zero rendering-device generation that owns persistent handles.
    /// </summary>
    public uint deviceGeneration => m_device.generation;

    /// <summary>
    /// Gets a detached control-thread snapshot of resource occupancy, high-water marks and rejected admissions.
    /// </summary>
    public RenderResourceStatistics resourceStatistics => m_resourceService.statistics with
    {
        uploadPages = m_uploads.pageCount, uploadResidentBytes = m_uploads.residentBytes,
        uploadPeakBytes = m_uploads.peakResidentBytes, uploadedFrameBytes = m_uploads.frameBytes,
        rejectedUploads = m_uploads.rejectedCount, targets = targets.count, rejectedTargets = targets.rejectedCount
    };

    /// <summary>
    /// Binds this rendering runtime to script-facing graphics APIs for the current asynchronous execution flow.
    /// </summary>
    /// <returns>
    /// A strict last-in-first-out execution scope owned by the caller.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// Thrown when this rendering runtime has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Retirement has started or generation cleanup has faulted the owner.
    /// </exception>
    public IDisposable EnterExecutionScope()
    {
        EnsureActive();
        return GraphicsSettingsExecutionContext.Enter(m_graphicsSettings);
    }

    /// <summary>
    /// Attaches this feature to its owning runtime generation.
    /// </summary>
    protected override void OnStart()
    {
        EnsureActive();
    }

    private void EnsureActive()
    {
        ObjectDisposedException.ThrowIf(m_disposed, this);
        if (m_retirement is not null)
            throw new InvalidOperationException("Rendering retirement has started and cannot accept new work.");
        m_extensions.EnsureHealthy();
    }

    /// <summary>
    /// Releases every runtime rendering generation, persistent target, upload, and GPU resource
    /// owned by this instance.
    /// </summary>
    protected override void OnStop() => ReleaseRendering();

}
