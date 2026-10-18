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
    private RenderViewport ResolvePrimaryPresentationViewport(RenderPresentationSize size)
    {
        var fullViewport = new RenderViewport(0, 0, size.width, size.height);
        if (m_primaryPresentationViewportProvider is null)
            return fullViewport;
        try
        {
            RenderViewport viewport = m_primaryPresentationViewportProvider(size);
            if ((long)viewport.x + viewport.width > size.width
                || (long)viewport.y + viewport.height > size.height)
            {
                throw new InvalidOperationException(
                    "The configured viewport extends beyond the primary presentation surface.");
            }
            return viewport;
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            PublishFrameIssue(new Diagnostic(
                "RENDER_PRESENTATION_VIEWPORT_FAILED",
                $"The primary presentation viewport was invalid and the complete surface was used: {exception.Message}",
                DiagnosticSeverity.Error,
                "RenderRuntime"));
            return fullViewport;
        }
    }

    private RenderOutputInput CreatePrimaryOutputInput(InputSnapshot input)
    {
        if (m_primaryPresentationSize is not RenderPresentationSize physicalSize
            || m_primaryPresentationViewport is not RenderViewport viewport)
            return RenderOutputInput.suspended;
        Vector2 pointerPosition = input.mousePosition;
        if (m_primaryInputSurfaceSizeProvider is not null)
        {
            RenderPresentationSize? inputSize = m_primaryInputSurfaceSizeProvider();
            if (inputSize is not RenderPresentationSize logicalSize)
                return RenderOutputInput.suspended;
            if (!logicalSize.isValid)
                throw new InvalidOperationException("The primary input surface has an invalid logical size.");
            pointerPosition = new Vector2(
                pointerPosition.x * physicalSize.width / logicalSize.width,
                pointerPosition.y * physicalSize.height / logicalSize.height);
        }
        Vector2 pointer = pointerPosition - new Vector2(viewport.x, viewport.y);
        return new RenderOutputInput(
            pointer, pointer.x >= 0f && pointer.y >= 0f && pointer.x < viewport.width && pointer.y < viewport.height,
            input.scrollDelta, input.modifiers, input.keysPressed, input.keysReleased,
            input.mouseButtonsPressed, input.mouseButtonsReleased, input.textInput);
    }

    private void AddPrimaryPresentationBackground(RenderGraphBuilder graph)
    {
        if (m_primaryPresentationSize is not RenderPresentationSize size
            || m_primaryPresentationViewport is not RenderViewport viewport)
            return;
        if (viewport.x == 0 && viewport.y == 0 && viewport.width == size.width && viewport.height == size.height)
        {
            return;
        }

        graph.AddRasterPass(
                "Primary Presentation Background",
                S_PRESENTATION_BACKGROUND_PHASE,
                0,
                static (
                    _,
                    _
                ) => { })
            .ClearPresentationTarget(S_PRESENTATION_BACKGROUND_COLOR)
            .HasSideEffect();
    }

    private void BuildComposition(
        RenderGraphBuilder graph,
        CompositionRequest composition,
        ref int requestIndex
    ) {
        if (composition.target.kind == RenderTargetKind.Backbuffer && !m_primaryPresentationSize.HasValue)
            return;
        try
        {
            using RenderGraphMutationScope mutation = graph.BeginMutationScope();
            using RenderGraphNameScope names = graph.BeginNameScope(
                $"Composition {composition.name}");
            var layers = new List<RenderTextureHandle>(composition.layers.Length);
            for (int index = 0; index < composition.layers.Length; index++)
            {
                RenderTextureHandle color = graph.CreateTexture(
                    $"Model Layer {index + 1}",
                    new RenderTextureDescriptor(
                        composition.viewport.width, composition.viewport.height,
                        composition.format,
                        RenderTextureUsage.ColorAttachment | RenderTextureUsage.Sampled));
                RenderRequest layer = composition.layers[index];
                var localLayer = new RenderRequest(layer.name, layer.target,
                    new RenderViewport(0, 0, composition.viewport.width, composition.viewport.height),
                    layer.pipeline, layer.data, layer.priority);
                if (!TryBuildRequest(graph, localLayer, requestIndex++,
                        preservePresentationTarget: false, outputOverride: color))
                    return;
                layers.Add(color);
            }
            RenderTextureHandle output = composition.target.kind == RenderTargetKind.Texture
                ? targets.Import(graph, composition.target.texture
                    ?? throw new InvalidOperationException("A texture output requires a RenderTexture."))
                : default;
            m_compositor.AddPasses(graph, composition.name, layers, output,
                composition.viewport, composition.format);
            if (output.isValid)
                graph.MarkOutput(output);
            RenderGraphValidationResult validation = graph.Validate();
            if (!validation.isValid)
            {
                PublishGraphDiagnostics(validation.diagnostics, composition.name);
                return;
            }
            mutation.Commit();
        }
        catch (Exception pendingRetirement) when (RetirementPendingException.Find(pendingRetirement) is not null)
        {
            throw;
        }
        catch (Exception exception)
        {
            PublishFrameIssue(new Diagnostic(
                "RENDER_OUTPUT_COMPOSITION_FAILED",
                $"Output composition '{composition.name}' was isolated after failure: {exception}",
                DiagnosticSeverity.Error,
                composition.name));
        }
    }

}
