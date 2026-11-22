using System;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Inno.References;
using Inno.Rendering.Runtime;

namespace Inno.Editor.Rendering;

/// <summary>
/// Supplies frame-only Editor interaction and output dimensions to a contributor.
/// </summary>
public sealed class EditorViewportContext
{
    internal EditorViewportContext(
        EditorContext editor,
        EditorInteractions interactions,
        EditorViewportKindID kind,
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        EditorViewportNavigationState navigation,
        ContentReadScope content,
        EditorViewportPresentation presentation,
        IViewContentCollector viewContent,
        ulong frameIndex,
        RenderOutputInput input
    ) {
        this.editor = editor;
        this.interactions = interactions;
        this.kind = kind;
        this.viewportID = viewportID;
        this.pixelWidth = pixelWidth;
        this.pixelHeight = pixelHeight;
        this.navigation = navigation;
        this.content = content;
        this.presentation = presentation;
        this.viewContent = viewContent;
        this.frameIndex = frameIndex;
        this.input = input;
    }

    /// <summary>
    /// Gets the current Editor context.
    /// </summary>
    public EditorContext editor { get; }

    /// <summary>
    /// Gets the shared Editor interaction and selection service.
    /// </summary>
    public EditorInteractions interactions { get; }

    /// <summary>
    /// Gets the open viewport purpose.
    /// </summary>
    public EditorViewportKindID kind { get; }

    /// <summary>
    /// Gets the stable panel viewport identity.
    /// </summary>
    public string viewportID { get; }

    /// <summary>
    /// Gets target width in physical pixels.
    /// </summary>
    public int pixelWidth { get; }

    /// <summary>
    /// Gets target height in physical pixels.
    /// </summary>
    public int pixelHeight { get; }

    /// <summary>
    /// Gets host-owned neutral navigation state that the selected controller can map to its camera model.
    /// </summary>
    public EditorViewportNavigationState navigation { get; }

    /// <summary>
    /// Gets the explicit ordered host content visible to this viewport.
    /// </summary>
    public ContentReadScope content { get; }

    /// <summary>
    /// Gets host-selected presentation preferences for this viewport.
    /// </summary>
    public EditorViewportPresentation presentation { get; }

    /// <summary>
    /// Gets the active generation's model-independent world-content collector.
    /// </summary>
    public IViewContentCollector viewContent { get; }

    /// <summary>
    /// Gets the frame index shared by all Editor views in this output frame.
    /// </summary>
    public ulong frameIndex { get; }

    /// <summary>
    /// Gets input located in this viewport's physical pixels.
    /// </summary>
    public RenderOutputInput input { get; }

    internal EditorViewportContext ForLayer(RenderOutputLayer layer)
    {
        ArgumentNullException.ThrowIfNull(layer);
        return new EditorViewportContext(
            editor,
            interactions,
            kind,
            viewportID,
            pixelWidth,
            pixelHeight,
            navigation,
            content,
            presentation,
            new SelectedViewContentCollector(viewContent, layer.sourceIDs),
            frameIndex,
            input
        );
    }
}
