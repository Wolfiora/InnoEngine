using System.Collections.Generic;
using Inno.Core.Graphs;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Reports neutral interaction intents for the feature controller to validate and apply through History.
/// </summary>
public readonly struct GraphCanvasPointerResult
{
    internal GraphCanvasPointerResult(
        GraphNodeId? nodePressed,
        IReadOnlyDictionary<GraphNodeId, GraphPosition>? movedNodes,
        GraphEndpoint? output,
        GraphEndpoint? input,
        GraphEndpoint? releasedOutput
    ) {
        this.nodePressed = nodePressed;
        this.movedNodes = movedNodes;
        this.output = output;
        this.input = input;
        this.releasedOutput = releasedOutput;
    }

    /// <summary>
    /// Gets the pressed node header, or null when no header was pressed.
    /// </summary>
    public GraphNodeId? nodePressed { get; }

    /// <summary>
    /// Gets completed movement positions, or null when no data edit was requested.
    /// </summary>
    public IReadOnlyDictionary<GraphNodeId, GraphPosition>? movedNodes { get; }

    /// <summary>
    /// Gets a requested connection's output endpoint; its domain type still requires validation.
    /// </summary>
    public GraphEndpoint? output { get; }

    /// <summary>
    /// Gets a requested connection's input endpoint, paired with the output endpoint.
    /// </summary>
    public GraphEndpoint? input { get; }

    /// <summary>
    /// Gets an output released over empty canvas, allowing the feature to offer a creation workflow.
    /// </summary>
    public GraphEndpoint? releasedOutput { get; }
}
