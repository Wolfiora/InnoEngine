using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Inno.Core.Diagnostics;
using Inno.Core.Graphs;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Freezes one target-selected graph region and its externally resolved, neutral compilation inputs.
/// </summary>
public sealed class ShaderGraphLoweringRequest
{
    /// <summary>
    /// Captures a region without retaining an asset object, source resolver, node provider or UI selection.
    /// </summary>
    /// <param name="graph">
    /// Nodes and edges in this stage region; canvas positions are not compilation inputs.
    /// </param>
    /// <param name="outputs">
    /// Named region results mapped to stable output endpoints.
    /// </param>
    /// <param name="implementationID">
    /// Exact source implementation key selected by the target.
    /// </param>
    /// <param name="sourceModules">
    /// Already analyzed source modules keyed by their source node identity.
    /// </param>
    /// <param name="stageInputs">
    /// Target-assigned stage inputs keyed by their input node identity.
    /// </param>
    public ShaderGraphLoweringRequest(
        GraphDocument graph,
        IReadOnlyDictionary<string, GraphEndpoint> outputs,
        string implementationID,
        IReadOnlyDictionary<GraphNodeID, ShaderSourceModuleAnalysis>? sourceModules = null,
        IReadOnlyDictionary<GraphNodeID, ShaderIrStageInput>? stageInputs = null
    ) {
        ArgumentNullException.ThrowIfNull(graph);
        ArgumentNullException.ThrowIfNull(outputs);
        ArgumentException.ThrowIfNullOrWhiteSpace(implementationID);
        foreach (string name in outputs.Keys)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
        }
        this.graph = graph.Clone();
        this.outputs = new ReadOnlyDictionary<string, GraphEndpoint>(
            new Dictionary<string, GraphEndpoint>(outputs, StringComparer.Ordinal)
        );
        this.implementationID = implementationID;
        this.sourceModules = new ReadOnlyDictionary<GraphNodeID, ShaderSourceModuleAnalysis>(
            new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis>(
                sourceModules ?? new Dictionary<GraphNodeID, ShaderSourceModuleAnalysis>()
            )
        );
        this.stageInputs = new ReadOnlyDictionary<GraphNodeID, ShaderIrStageInput>(
            new Dictionary<GraphNodeID, ShaderIrStageInput>(stageInputs ?? new Dictionary<GraphNodeID, ShaderIrStageInput>())
        );
    }

    /// <summary>
    /// Gets named region outputs by stable graph endpoint.
    /// </summary>
    public IReadOnlyDictionary<string, GraphEndpoint> outputs { get; }

    /// <summary>
    /// Gets the exact implementation key selected by the target.
    /// </summary>
    public string implementationID { get; }

    /// <summary>
    /// Gets frozen source modules, including unavailable/failed modules for accurate diagnostics.
    /// </summary>
    public IReadOnlyDictionary<GraphNodeID, ShaderSourceModuleAnalysis> sourceModules { get; }

    /// <summary>
    /// Gets target-assigned input descriptors; they contain no native expressions.
    /// </summary>
    public IReadOnlyDictionary<GraphNodeID, ShaderIrStageInput> stageInputs { get; }

    internal GraphDocument graph { get; }
}

/// <summary>
/// Identifies a graph compilation problem without retaining a node or compiler instance.
/// </summary>
/// <param name="code">
/// Stable diagnostic identity.
/// </param>
/// <param name="severity">
/// Diagnostic severity.
/// </param>
/// <param name="message">
/// Human-readable explanation.
/// </param>
/// <param name="nodeID">
/// Affected node identity when known.
/// </param>
/// <param name="portID">
/// Affected semantic port when known.
/// </param>
public sealed record ShaderGraphDiagnostic(
    string code,
    DiagnosticSeverity severity,
    string message,
    GraphNodeID? nodeID = null,
    string? portID = null
);
