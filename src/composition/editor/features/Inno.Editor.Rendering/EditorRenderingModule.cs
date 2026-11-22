using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Inno.Core.Identity;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.Extensibility.Types;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Runtime;
using Inno.Scripting.Api;

using EngineColor = Inno.Core.Mathematics.Color;

namespace Inno.Editor.Rendering;

/// <summary>
/// Composes reloadable rendering-model contributors while retaining only opaque presentation outputs.
/// </summary>
[EditorModule("rendering.viewports", order: 175)]
public sealed class EditorRenderingModule : EditorModule
{
    private const int C_RENDERING_STATISTICS_ORDER = 100;
    private const int C_VIEWPORT_STATISTICS_ORDER = 200;

    private readonly Dictionary<string, EditorViewportNavigationState> m_navigationStates = new(StringComparer.Ordinal);
    private readonly Dictionary<string, ContentReadScope> m_contentScopes = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> m_compositionErrors = new(StringComparer.Ordinal);
    private readonly Dictionary<string, string> m_controllerIDs = new(StringComparer.Ordinal);

    private readonly Dictionary<string, EditorViewportManipulationSpace> m_manipulationSpaces =
        new(StringComparer.Ordinal);

    private readonly Dictionary<string, EditorViewportPresentation> m_presentations =
        new(StringComparer.Ordinal);

    private readonly Dictionary<string, RenderOutputInput> m_outputInputs = new(StringComparer.Ordinal);
    private readonly Dictionary<string, RenderOutputRoute> m_outputRoutes = new(StringComparer.Ordinal);
    private readonly IEditorRenderingHost m_host;
    private readonly EditorInteractions m_interactions;
    private readonly EditorViewportContributorRegistry m_contributors;
    private readonly EditorGizmoProviderRegistry m_gizmos;
    private EditorContext? m_context;

    /// <summary>
    /// Creates the composition host around stable rendering and interaction services.
    /// </summary>
    /// <param name="host">
    /// Host-owned target and opaque texture bridge.
    /// </param>
    /// <param name="interactions">
    /// Shared Editor interaction and selection service.
    /// </param>
    /// <param name="types">
    /// The type catalog that owns viewport contributor generations.
    /// </param>
    [ScriptingApiIgnore]
    public EditorRenderingModule(
        IEditorRenderingHost host,
        EditorInteractions interactions,
        TypeCatalog types
    ) {
        m_host = host ?? throw new ArgumentNullException(nameof(host));
        m_interactions = interactions ?? throw new ArgumentNullException(nameof(interactions));
        m_contributors = new EditorViewportContributorRegistry(types ?? throw new ArgumentNullException(nameof(types)));
        m_gizmos = new EditorGizmoProviderRegistry(types);
    }

    /// <summary>
    /// Collects Editor-only icons and bounds for the current Scene viewport.
    /// </summary>
    /// <param name="viewportID">
    /// The Scene viewport identity.
    /// </param>
    /// <param name="pixelWidth">
    /// The viewport width.
    /// </param>
    /// <param name="pixelHeight">
    /// The viewport height.
    /// </param>
    /// <returns>
    /// Transient gizmo primitives for the submitted frame.
    /// </returns>
    [ScriptingApiIgnore]
    public EditorGizmoFrame CollectGizmos(
        string viewportID,
        int pixelWidth,
        int pixelHeight
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        var frame = new EditorGizmoFrame();
        if (!m_contentScopes.TryGetValue(viewportID, out ContentReadScope? content))
        {
            return frame;
        }
        RuntimeIdentity? selected = (m_interactions.selection.selectedTarget as IdentityObject)?
            .identity.runtimeIdentity;
        var context = new EditorGizmoContext(content, selected, pixelWidth, pixelHeight);
        foreach (EditorGizmoProviderRegistry.Registration provider in m_gizmos.providers.registrations)
        {
            provider.provider.Collect(context, frame);
        }
        return frame;
    }

    /// <summary>
    /// Gets whether the current extension generation contributes to one viewport purpose.
    /// </summary>
    /// <param name="kind">
    /// Open viewport purpose.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when at least one contributor is registered.
    /// </returns>
    public bool HasContributors(EditorViewportKindID kind) => kind.isValid && m_contributors.contributors.byKind.ContainsKey(kind);

    /// <summary>
    /// Supplies viewport-local input captured by the output panel for this frame.
    /// </summary>
    /// <param name="viewportID">
    /// Stable output identity.
    /// </param>
    /// <param name="input">
    /// Physical-pixel input snapshot.
    /// </param>
    public void SetOutputInput(
        string viewportID,
        RenderOutputInput input
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        m_outputInputs[viewportID] = input ?? throw new ArgumentNullException(nameof(input));
    }

    /// <summary>
    /// Sets the exact model contributor order for an output with several applicable models.
    /// </summary>
    /// <param name="viewportID">
    /// Stable output identity.
    /// </param>
    /// <param name="route">
    /// Explicit contributor route, or null for single-model selection.
    /// </param>
    public void SetOutputRoute(
        string viewportID,
        RenderOutputRoute? route
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        if (route is null)
        {
            m_outputRoutes.Remove(viewportID);
        }
        else
        {
            m_outputRoutes[viewportID] = route;
        }
    }

    /// <summary>
    /// Gets the most recent isolated contribution or composition failure for one stable viewport.
    /// </summary>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <returns>
    /// The failure message, or null when no current failure exists.
    /// </returns>
    public string? GetCompositionError(string viewportID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        return m_compositionErrors.GetValueOrDefault(viewportID);
    }

    /// <summary>
    /// Gets the host-owned neutral navigation state for one stable Editor viewport.
    /// </summary>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <returns>
    /// The reusable navigation state owned by the Editor host.
    /// </returns>
    [ScriptingApiIgnore]
    public EditorViewportNavigationState GetNavigationState(string viewportID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        if (!m_navigationStates.TryGetValue(viewportID, out EditorViewportNavigationState? state))
        {
            state = new EditorViewportNavigationState();
            m_navigationStates.Add(viewportID, state);
        }
        return state;
    }

    /// <summary>
    /// Sets the explicit ordered host content visible to one Editor viewport.
    /// </summary>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="content">
    /// Current frame-safe content scope.
    /// </param>
    [ScriptingApiIgnore]
    public void SetContentScope(
        string viewportID,
        ContentReadScope content
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        ArgumentNullException.ThrowIfNull(content);
        if (m_contentScopes.Remove(viewportID, out ContentReadScope? previous))
        {
            previous.Dispose();
        }
        m_contentScopes[viewportID] = content ?? throw new ArgumentNullException(nameof(content));
    }

    /// <summary>
    /// Queries the selected controller's navigation contract before viewport input is processed.
    /// </summary>
    /// <param name="kind">
    /// Open viewport purpose.
    /// </param>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="pixelWidth">
    /// Positive target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Positive target height.
    /// </param>
    /// <param name="profile">
    /// Receives the selected controller profile.
    /// </param>
    /// <returns>
    /// True when a contributor returned a usable navigation profile and became the controller.
    /// </returns>
    public bool TryConfigureNavigation(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        out EditorViewportNavigationProfile profile
    ) {
        profile = EditorViewportNavigationProfile.disabled;
        if (!TryCreateContext(kind, viewportID, pixelWidth, pixelHeight, out EditorViewportContext? context))
        {
            return false;
        }
        var failures = new List<string>();
        EditorViewportContributorRegistry.Registration[] contributors = GetApplicableContributors(context!, failures);
        foreach (EditorViewportContributorRegistry.Registration registration in contributors
            .OrderByDescending(static value => value.attribute.controllerPriority)
            .ThenBy(static value => value.attribute.id, StringComparer.Ordinal)
        ) {
            try
            {
                EditorViewportNavigationProfile candidate = registration.contributor.ConfigureNavigation(context!)
                    ?? throw new InvalidOperationException("Viewport contributor returned a null navigation profile.");
                if (!candidate.id.isValid)
                {
                    continue;
                }
                profile = candidate;
                m_controllerIDs[viewportID] = registration.attribute.id;
                SetCompositionFailures(viewportID, failures);
                return true;
            }
            catch (Exception exception)
            {
                failures.Add(
                    $"Viewport contributor '{registration.attribute.id}' navigation failed: {exception.Message}"
                );
            }
        }
        m_controllerIDs.Remove(viewportID);
        SetCompositionFailures(viewportID, failures);
        return false;
    }

    /// <summary>
    /// Sets presentation preferences supplied to every contributor for one Editor viewport.
    /// </summary>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="presentation">
    /// Current host-owned presentation preferences.
    /// </param>
    [ScriptingApiIgnore]
    public void SetPresentation(
        string viewportID,
        EditorViewportPresentation presentation
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        m_presentations[viewportID] = presentation;
    }

    /// <summary>
    /// Tries to get the manipulation space from the selected controller's latest contribution.
    /// </summary>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="space">
    /// Receives the latest exact view/projection contract.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when the selected controller supplied a manipulation space.
    /// </returns>
    [ScriptingApiIgnore]
    public bool TryGetManipulationSpace(
        string viewportID,
        out EditorViewportManipulationSpace space
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        return m_manipulationSpaces.TryGetValue(viewportID, out space);
    }

    /// <summary>
    /// Draws toolbar controls owned by the selected model controller.
    /// </summary>
    /// <param name="kind">
    /// Open viewport purpose.
    /// </param>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="pixelWidth">
    /// Current target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Current target height.
    /// </param>
    public void DrawControllerToolbar(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight
    ) {
        if (!TryCreateContext(kind, viewportID, pixelWidth, pixelHeight, out EditorViewportContext? context))
        {
            return;
        }
        var failures = new List<string>();
        EditorViewportContributorRegistry.Registration[] contributors = GetApplicableContributors(context!, failures);
        EditorViewportContributorRegistry.Registration? registration = SelectController(viewportID, contributors);
        if (registration is null)
        {
            SetCompositionFailures(viewportID, failures);
            return;
        }
        try
        {
            registration.contributor.DrawToolbar(context!);
        }
        catch (Exception exception)
        {
            failures.Add($"Viewport contributor '{registration.attribute.id}' toolbar failed: {exception.Message}");
        }
        SetCompositionFailures(viewportID, failures);
    }

    /// <summary>
    /// Builds, composes, submits, and returns one host-owned offscreen viewport.
    /// </summary>
    /// <param name="kind">
    /// Open viewport purpose.
    /// </param>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="pixelWidth">
    /// Positive target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Positive target height.
    /// </param>
    /// <param name="output">
    /// Receives the opaque viewport output.
    /// </param>
    /// <returns>
    /// <see langword="true"/> when at least one model contribution was accepted.
    /// </returns>
    public bool TrySubmit(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        out EditorViewportOutput output
    ) {
        output = default;
        if (!TryCreateContext(kind, viewportID, pixelWidth, pixelHeight, out EditorViewportContext? context))
        {
            return false;
        }
        var failures = new List<string>();
        EditorViewportContributorRegistry.Registration[] contributors = GetApplicableContributors(context!, failures);
        if (contributors.Length == 0)
        {
            PublishViewportStatistics(kind, viewportID, pixelWidth, pixelHeight, "Unavailable", contributorIDs: "None");
            m_host.Release(viewportID);
            m_controllerIDs.Remove(viewportID);
            m_manipulationSpaces.Remove(viewportID);
            failures.Add("No rendering-model contributor accepts the current viewport content.");
            SetCompositionFailures(viewportID, failures);
            return false;
        }

        RenderOutputRoute? route = null;
        if (contributors.Length > 1)
        {
            if (!m_outputRoutes.TryGetValue(viewportID, out route)
                || route.layers.Count != contributors.Length
                || route.layers.Any(layer => contributors.All(entry => entry.attribute.id != layer.modelID))
            ) {
                failures.Add(
                    "Multiple rendering models accept this output; configure a RenderOutputRoute naming every contributor exactly once: "
                    + string.Join(", ", contributors.Select(static entry => entry.attribute.id))
                );
                HandleViewportFailure(kind, viewportID, pixelWidth, pixelHeight, contributors, failures);
                return false;
            }
            contributors = route.layers
                .Select(layer => contributors.Single(entry => entry.attribute.id == layer.modelID))
                .ToArray();
        }
        var accepted = new List<AcceptedContribution>(contributors.Length);
        RenderTextureFormat? targetFormat = null;
        for (int index = 0; index < contributors.Length; index++)
        {
            EditorViewportContributorRegistry.Registration registration = contributors[index];
            try
            {
                EditorViewportContext modelContext = route is null
                                                     ? context!
                                                     : context!.ForLayer(route.layers[index]);
                EditorViewportContribution contribution = registration.contributor.Build(modelContext)
                    ?? throw new InvalidOperationException("Viewport contributor returned a null contribution.");
                if (targetFormat is RenderTextureFormat selectedFormat
                    && contribution.targetFormat != selectedFormat
                ) {
                    failures.Add(
                        $"Viewport contributor '{registration.attribute.id}' requested target format "
                        + $"'{contribution.targetFormat}' while the composition uses '{selectedFormat}'."
                    );
                    continue;
                }
                targetFormat ??= contribution.targetFormat;
                accepted.Add(new AcceptedContribution(registration, contribution));
            }
            catch (Exception exception)
            {
                failures.Add($"Viewport contributor '{registration.attribute.id}' failed: {exception.Message}");
            }
        }
        if (accepted.Count == 0)
        {
            HandleViewportFailure(kind, viewportID, pixelWidth, pixelHeight, contributors, failures);
            return false;
        }
        if (accepted.Count != contributors.Length)
        {
            HandleViewportFailure(kind, viewportID, pixelWidth, pixelHeight, contributors, failures);
            return false;
        }

        try
        {
            output = m_host.Submit(
                new EditorViewportComposition(
                    viewportID,
                    pixelWidth,
                    pixelHeight,
                    targetFormat!.Value,
                    accepted.Select((
                        value,
                        index
                    ) => new EditorViewportLayer(
                        value.registration.attribute.id,
                        value.contribution.pipeline,
                        value.contribution.data,
                        route is null ? value.registration.attribute.order : index
                    ))
                )
            );
            EditorViewportContributorRegistry.Registration? controller = SelectController(viewportID, contributors);
            EditorViewportContribution? controllerContribution = controller is null
                ? null
                : accepted.FirstOrDefault(value => ReferenceEquals(value.registration, controller))?.contribution;
            if (controllerContribution?.manipulationSpace is EditorViewportManipulationSpace manipulationSpace)
            {
                m_manipulationSpaces[viewportID] = manipulationSpace;
            }
            else
            {
                m_manipulationSpaces.Remove(viewportID);
            }
            PublishViewportStatistics(
                kind,
                viewportID,
                pixelWidth,
                pixelHeight,
                output.isReady ? "Ready" : "Preparing",
                string.Join(", ", accepted.Select(static value => value.registration.attribute.id)),
                accepted
            );
            SetCompositionFailures(viewportID, failures);
            return true;
        }
        catch (Exception exception)
        {
            failures.Add($"Viewport composition failed: {exception.Message}");
            HandleViewportFailure(kind, viewportID, pixelWidth, pixelHeight, contributors, failures);
            return false;
        }
    }

    /// <summary>
    /// Forwards a normalized click to the selected model controller.
    /// </summary>
    /// <param name="kind">
    /// Open viewport purpose.
    /// </param>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="pixelWidth">
    /// Current target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Current target height.
    /// </param>
    /// <param name="x">
    /// Normalized horizontal position.
    /// </param>
    /// <param name="y">
    /// Normalized vertical position.
    /// </param>
    /// <param name="button">
    /// Platform-independent pointer button index.
    /// </param>
    public void HandlePointer(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        float x,
        float y,
        int button
    ) {
        if (!TryCreateContext(kind, viewportID, pixelWidth, pixelHeight, out EditorViewportContext? context))
        {
            return;
        }
        var failures = new List<string>();
        EditorViewportContributorRegistry.Registration[] contributors = GetApplicableContributors(context!, failures);
        EditorViewportContributorRegistry.Registration? registration = SelectController(viewportID, contributors);
        if (registration is null)
        {
            SetCompositionFailures(viewportID, failures);
            return;
        }
        try
        {
            registration.contributor.HandlePointer(new EditorViewportPointerContext(context!, x, y, button));
        }
        catch (Exception exception)
        {
            failures.Add(
                $"Viewport contributor '{registration.attribute.id}' pointer handler failed: {exception.Message}"
            );
        }
        SetCompositionFailures(viewportID, failures);
    }

    /// <summary>
    /// Draws a ready output in the current panel.
    /// </summary>
    /// <param name="output">
    /// Opaque output returned by <see cref="TrySubmit"/>.
    /// </param>
    /// <param name="logicalSize">
    /// Destination size in logical UI pixels.
    /// </param>
    public void Draw(
        EditorViewportOutput output,
        Vector2 logicalSize
    ) => m_host.Draw(output, logicalSize);

    /// <summary>
    /// Stops retaining one viewport target.
    /// </summary>
    /// <param name="viewportID">
    /// Stable viewport identity.
    /// </param>
    public void Release(string viewportID)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        m_outputInputs.Remove(viewportID);
        m_outputRoutes.Remove(viewportID);
        m_manipulationSpaces.Remove(viewportID);
        if (m_contentScopes.Remove(viewportID, out ContentReadScope? content))
        {
            content.Dispose();
        }
        m_controllerIDs.Remove(viewportID);
        m_host.Release(viewportID);
    }

    /// <summary>
    /// Initializes this feature when its owning runtime becomes active.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStart(EditorContext context)
    {
        m_context = context;
        _ = m_contributors.contributors;
    }

    /// <summary>
    /// Advances this feature using the current runtime state.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnUpdate(EditorContext context)
    {
        RenderFrameStatistics? statistics = GraphicsSettings.frameStatistics;
        if (statistics is null)
        {
            return;
        }
        var groupID = new EditorStatisticGroupID("inno.rendering.frame");
        context.statistics.Publish(new EditorStatistic[]
        {
            CreateStatistic("frame", "Frame", statistics.frameIndex.ToString(), 0),
            CreateStatistic("views", "Views", statistics.viewCount.ToString(), 10),
            CreateStatistic("draws", "Draws", statistics.drawCount.ToString(), 20),
            CreateStatistic("dispatches", "Dispatches", statistics.dispatchCount.ToString(), 30),
            CreateStatistic("culled-passes", "Culled Passes", statistics.culledPassCount.ToString(), 40)
        });
        if (statistics.allocationCounters is { } allocations)
        {
            context.statistics.Publish(new EditorStatistic[]
            {
                CreateStatistic("allocation-generation", "Allocation Generation", allocations.deviceGeneration.ToString(), 50),
                CreateStatistic("texture-allocations", "Transient Textures (lifetime)", allocations.textureAllocations.ToString(), 60),
                CreateStatistic("buffer-allocations", "Transient Buffers (lifetime)", allocations.bufferAllocations.ToString(), 70),
                CreateStatistic(
                    "framebuffer-allocations",
                    "Transient Framebuffers (lifetime)",
                    allocations.frameBufferAllocations.ToString(),
                    80
                )
            });
        }
        else
        {
            context.statistics.Publish(new[]
            {
                CreateStatistic("allocations-unavailable", "Transient Allocations", "Unavailable", 50)
            });
        }
        return;

        EditorStatistic CreateStatistic(
            string id,
            string label,
            string value,
            int order
        ) => new(
            new EditorStatisticID($"inno.rendering.frame.{id}"),
            groupID,
            "Rendering",
            label,
            value,
            C_RENDERING_STATISTICS_ORDER,
            order
        );
    }

    /// <summary>
    /// Stops this feature before its owning runtime releases the active generation.
    /// </summary>
    /// <param name="context">
    /// The context that supplies state and services for this operation.
    /// </param>
    protected override void OnStop(EditorContext context)
    {
        _ = context;
        m_context = null;
        m_host.ReleaseAll();
        m_navigationStates.Clear();
        ClearContentScopes();
        m_compositionErrors.Clear();
        m_controllerIDs.Clear();
        m_manipulationSpaces.Clear();
        m_presentations.Clear();
        m_outputInputs.Clear();
        m_outputRoutes.Clear();
    }

    /// <summary>
    /// Releases resources retained by this feature after it has stopped.
    /// </summary>
    protected override void OnDispose()
    {
        m_contributors.Dispose();
        m_gizmos.Dispose();
        m_navigationStates.Clear();
        ClearContentScopes();
        m_compositionErrors.Clear();
        m_controllerIDs.Clear();
        m_manipulationSpaces.Clear();
        m_presentations.Clear();
        m_outputInputs.Clear();
        m_outputRoutes.Clear();
        m_host.ReleaseAll();
    }

    private void ClearContentScopes()
    {
        foreach (ContentReadScope scope in m_contentScopes.Values)
        {
            scope.Dispose();
        }
        m_contentScopes.Clear();
    }

    private bool TryCreateContext(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        out EditorViewportContext? context
    ) {
        if (!kind.isValid || m_context is null)
        {
            context = null;
            return false;
        }
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelHeight);
        context = new EditorViewportContext(
            m_context,
            m_interactions,
            kind,
            viewportID,
            pixelWidth,
            pixelHeight,
            GetNavigationState(viewportID),
            m_contentScopes.GetValueOrDefault(viewportID, ContentReadScope.empty),
            m_presentations.GetValueOrDefault(viewportID, new EditorViewportPresentation(EngineColor.DARKGRAY)),
            m_host.viewContent,
            m_host.currentFrameIndex,
            m_outputInputs.GetValueOrDefault(viewportID, RenderOutputInput.empty)
        );
        return true;
    }

    private void PublishViewportStatistics(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        string state,
        string contributorIDs,
        IReadOnlyList<AcceptedContribution>? contributions = null
    ) {
        if (m_context is null)
        {
            return;
        }
        string groupKey = $"inno.rendering.viewport.{viewportID}";
        var groupID = new EditorStatisticGroupID(groupKey);
        string groupName = GetDisplayName(viewportID);
        var statistics = new List<EditorStatistic>
        {
            CreateStatistic("kind", "Kind", kind.value, 0),
            CreateStatistic("contributors", "Contributors", contributorIDs, 10),
            CreateStatistic("state", "State", state, 20),
            CreateStatistic("resolution", "Resolution", $"{pixelWidth} x {pixelHeight}", 30)
        };
        if (contributions is { Count: > 0 })
        {
            string pipeline = string.Join(
                ", ",
                contributions.Select(static contribution => (
                    string.IsNullOrWhiteSpace(contribution.contribution.pipeline?.pipelineTypeId)
                    ? GraphicsSettings.defaultPipeline?.pipelineTypeId ?? "Project Default"
                    : contribution.contribution.pipeline.pipelineTypeId
                ))
            );
            statistics.Add(CreateStatistic("pipelines", "Pipelines", pipeline, 40));
            statistics.Add(
                CreateStatistic("format", "Target Format", contributions[0].contribution.targetFormat.ToString(), 50)
            );
            statistics.Add(
                CreateStatistic(
                    "order",
                    "Composition Order",
                    string.Join(
                        ", ",
                        contributions.Select(static contribution =>
                            contribution.registration.attribute.order.ToString())
                    ),
                    60
                )
            );
        }
        m_context.statistics.Publish(statistics);
        return;

        EditorStatistic CreateStatistic(
            string id,
            string label,
            string value,
            int order
        ) => new(
            new EditorStatisticID($"{groupKey}.{id}"),
            groupID,
            groupName,
            label,
            value,
            C_VIEWPORT_STATISTICS_ORDER,
            order
        );
    }

    private EditorViewportContributorRegistry.Registration[] GetApplicableContributors(
        EditorViewportContext context,
        List<string> failures
    ) {
        if (!m_contributors.contributors.byKind.TryGetValue(
            context.kind,
            out EditorViewportContributorRegistry.Registration[]? registrations
        )) {
            return [];
        }

        var applicable = new List<EditorViewportContributorRegistry.Registration>(registrations.Length);
        foreach (EditorViewportContributorRegistry.Registration registration in registrations)
        {
            try
            {
                if (registration.contributor.CanContribute(context))
                {
                    applicable.Add(registration);
                }
            }
            catch (Exception exception)
            {
                failures.Add(
                    $"Viewport contributor '{registration.attribute.id}' participation check failed: "
                    + exception.Message
                );
            }
        }
        return applicable.ToArray();
    }

    private EditorViewportContributorRegistry.Registration? SelectController(
        string viewportID,
        IReadOnlyList<EditorViewportContributorRegistry.Registration> contributors
    ) {
        if (m_controllerIDs.TryGetValue(viewportID, out string? controllerID))
        {
            EditorViewportContributorRegistry.Registration? selected = contributors.FirstOrDefault(
                contributor => string.Equals(contributor.attribute.id, controllerID, StringComparison.Ordinal)
            );
            if (selected is not null)
            {
                return selected;
            }
        }

        return contributors
            .OrderByDescending(static contributor => contributor.attribute.controllerPriority)
            .ThenBy(static contributor => contributor.attribute.id, StringComparer.Ordinal)
            .FirstOrDefault();
    }

    private void HandleViewportFailure(
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        IReadOnlyList<EditorViewportContributorRegistry.Registration> contributors,
        List<string> failures
    ) {
        PublishViewportStatistics(
            kind,
            viewportID,
            pixelWidth,
            pixelHeight,
            "Failed",
            string.Join(", ", contributors.Select(static contributor => contributor.attribute.id))
        );
        m_host.Release(viewportID);
        m_controllerIDs.Remove(viewportID);
        m_manipulationSpaces.Remove(viewportID);
        SetCompositionFailures(viewportID, failures);
    }

    private void SetCompositionFailures(
        string viewportID,
        IReadOnlyList<string> failures
    ) {
        string[] distinct = failures.Distinct(StringComparer.Ordinal).ToArray();
        if (distinct.Length == 0)
        {
            m_compositionErrors.Remove(viewportID);
        }
        else
        {
            m_compositionErrors[viewportID] = string.Join(Environment.NewLine, distinct);
        }
    }

    private static string GetDisplayName(string identifier)
    {
        char[] characters = identifier.Replace('-', ' ').Replace('_', ' ').ToCharArray();
        bool capitalize = true;
        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == ' ')
            {
                capitalize = true;
                continue;
            }
            if (!capitalize)
            {
                continue;
            }
            characters[i] = char.ToUpperInvariant(characters[i]);
            capitalize = false;
        }
        return new string(characters);
    }

    private sealed record AcceptedContribution(
        EditorViewportContributorRegistry.Registration registration,
        EditorViewportContribution contribution
    );
}
