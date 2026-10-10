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
    /// Sets the explicit model composition route for the primary output at a frame boundary.
    /// </summary>
    /// <param name="route">
    /// Ordered model identities, or null for automatic single-model selection.
    /// </param>
    public void SetPrimaryRoute(RenderOutputRoute? route)
    {
        EnsureActive();
        if (m_frameOpen)
            throw new InvalidOperationException("Output routes can only change at a frame boundary.");
        m_primaryRoute = route;
    }

    /// <summary>
    /// Enables or disables model rendering to the host's primary backbuffer.
    /// Editor hosts disable this because their Game and Scene sessions own offscreen outputs.
    /// </summary>
    /// <param name="enabled">
    /// Whether the primary backbuffer is a model output.
    /// </param>
    public void SetPrimaryModelOutputEnabled(bool enabled)
    {
        EnsureActive();
        if (m_frameOpen)
            throw new InvalidOperationException("Primary output ownership can only change at a frame boundary.");
        m_primaryModelOutputEnabled = enabled;

    }

    private void CollectModels(RenderOutputSession session)
    {
        try
        {
            List<RenderExtensionRegistry.RenderModelEntry> applicable = m_scratch.applicableModels;
            foreach (RenderExtensionRegistry.RenderModelEntry entry in m_extensions.extensions.models.models)
            {
                try
                {
                    if (entry.model.CanRender(session))
                        applicable.Add(entry);
                }
                catch (Exception exception)
                {
                    PublishFrameIssue(new Diagnostic("RENDER_MODEL_ACCEPT_FAILED",
                        $"Render model '{entry.id}' failed to inspect output content: {exception}",
                        DiagnosticSeverity.Error, entry.id));
                }
            }
            if (applicable.Count == 0)
            {
                PublishModelDiagnostic("No rendering model accepts the primary output content.");
                return;
            }
            List<RenderExtensionRegistry.RenderModelEntry> ordered = m_scratch.orderedModels;
            RenderOutputRoute? route = session.route;
            if (route is not null)
            {
                if (route.layers.Count != applicable.Count)
                {
                    PublishModelDiagnostic("The primary output route must name every applicable rendering model exactly once.");
                    return;
                }
                foreach (RenderOutputLayer layer in route.layers)
                {
                    int match = -1;
                    for (int candidate = 0; candidate < applicable.Count; candidate++)
                    {
                        if (applicable[candidate].id == layer.modelId)
                        {
                            match = candidate;
                            break;
                        }
                    }
                    if (match < 0)
                    {
                        PublishModelDiagnostic("The primary output route must name every applicable rendering model exactly once.");
                        return;
                    }
                    ordered.Add(applicable[match]);
                }
            }
            else if (applicable.Count > 1)
            {
                PublishModelDiagnostic("Multiple rendering models accept the primary output; configure a RenderOutputRoute: "
                    + string.Join(", ", applicable.Select(static entry => entry.id)));
                return;
            }
            if (route is null)
                ordered.AddRange(applicable);

            List<RenderRequest> requests = m_scratch.modelRequests;
            RenderTextureFormat? format = null;
            for (int index = 0; index < ordered.Count; index++)
            {
                RenderExtensionRegistry.RenderModelEntry entry = ordered[index];
                try
                {
                    RenderOutputSession modelSession = route is null
                        ? session
                        : session.ForLayer(route.layers[index]);
                    RenderModelOutput model = entry.model.Build(modelSession);
                    if (format is RenderTextureFormat selected && selected != model.targetFormat)
                    {
                        PublishModelDiagnostic($"Output models require different target formats: '{selected}' and '{model.targetFormat}'.");
                        return;
                    }
                    format ??= model.targetFormat;
                    requests.Add(new RenderRequest(model.name, RenderTarget.backbuffer, session.viewport,
                        model.pipeline, model.data, 1000 + index));
                }
                catch (Exception exception)
                {
                    PublishFrameIssue(new Diagnostic("RENDER_MODEL_BUILD_FAILED",
                        $"Render model '{entry.id}' failed to build output: {exception}",
                        DiagnosticSeverity.Error, entry.id));
                    return;
                }
            }
            if (requests.Count == 1 && m_device.primaryPresentationEncodesSrgb)
                Submit(requests[0]);
            else
                SubmitComposition($"Output:{session.id}", RenderTarget.backbuffer,
                    session.viewport, format!.Value, requests, priority: 1000);
        }
        finally
        {
            m_scratch.applicableModels.Clear();
            m_scratch.orderedModels.Clear();
            m_scratch.modelRequests.Clear();
        }
    }

}
