using System;
using System.Collections.Generic;
using Inno.Core.Collections;
using Inno.Core.Diagnostics;

namespace Inno.Core.Graphs;

/// <summary>
/// Validates neutral graph topology against generation-scoped node definitions.
/// </summary>
public static class GraphValidator
{
    /// <summary>
    /// Validates node availability, ports, edge types, capacity, required inputs and cycles.
    /// </summary>
    /// <param name="document">
    /// Graph document to validate without modifying it.
    /// </param>
    /// <param name="resolver">
    /// Active node definition resolver.
    /// </param>
    /// <param name="conversion">
    /// Optional directed type conversion policy.
    /// </param>
    /// <param name="allowCycles">
    /// Whether directed cycles are permitted.
    /// </param>
    /// <returns>
    /// A deterministic validation result. Missing definitions remain warnings so documents stay editable.
    /// </returns>
    public static GraphValidationResult Validate(
        GraphDocument document,
        IGraphNodeDefinitionResolver resolver,
        IGraphTypeConversion? conversion = null,
        bool allowCycles = false
    ) {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(resolver);

        List<GraphDiagnostic> diagnostics = [];
        Dictionary<GraphNodeID, IReadOnlyDictionary<GraphPortID, GraphPortDefinition>> portsByNode = [];

        ResolvePorts(document, resolver, portsByNode, diagnostics);
        ValidateEdges(document, portsByNode, conversion, diagnostics);
        ValidateRequiredInputs(document, portsByNode, diagnostics);

        if (!allowCycles)
        {
            ValidateCycles(document, diagnostics);
        }

        return new GraphValidationResult(diagnostics);
    }

    private static void ResolvePorts(
        GraphDocument document,
        IGraphNodeDefinitionResolver resolver,
        Dictionary<GraphNodeID, IReadOnlyDictionary<GraphPortID, GraphPortDefinition>> portsByNode,
        List<GraphDiagnostic> diagnostics
    ) {
        foreach (GraphNodeRecord node in document.nodes)
        {
            if (!resolver.TryResolve(node.definitionID, out GraphNodeDefinition? definition) || definition is null)
            {
                diagnostics.Add(
                    new GraphDiagnostic(
                        "GRAPH_MISSING_NODE",
                        $"Node definition '{node.definitionID}' is not active. The node and its connections were preserved.",
                        DiagnosticSeverity.Warning,
                        node.id
                    )
                );
                continue;
            }

            try
            {
                Dictionary<GraphPortID, GraphPortDefinition> ports = [];
                foreach (GraphPortDefinition port in definition.GetPorts(node))
                {
                    if (!ports.TryAdd(port.id, port))
                    {
                        diagnostics.Add(
                            new GraphDiagnostic(
                                "GRAPH_DUPLICATE_PORT",
                                $"Node definition '{node.definitionID}' returned duplicate port '{port.id}'.",
                                DiagnosticSeverity.Error,
                                node.id
                            )
                        );
                    }
                }

                portsByNode[node.id] = ports;
            }
            catch (Exception exception)
            {
                diagnostics.Add(
                    new GraphDiagnostic(
                        "GRAPH_NODE_RESOLUTION_FAILED",
                        $"Node '{node.id}' could not resolve its ports: {exception.Message}",
                        DiagnosticSeverity.Error,
                        node.id
                    )
                );
            }
        }
    }

    private static void ValidateEdges(
        GraphDocument document,
        IReadOnlyDictionary<GraphNodeID, IReadOnlyDictionary<GraphPortID, GraphPortDefinition>> portsByNode,
        IGraphTypeConversion? conversion,
        List<GraphDiagnostic> diagnostics
    ) {
        Dictionary<GraphEndpoint, int> connectionCounts = [];

        foreach (GraphEdgeRecord edge in document.edges)
        {
            GraphNodeRecord? outputNode = document.FindNode(edge.output.nodeID);
            GraphNodeRecord? inputNode = document.FindNode(edge.input.nodeID);
            if (outputNode is null || inputNode is null)
            {
                diagnostics.Add(
                    new GraphDiagnostic(
                        "GRAPH_MISSING_ENDPOINT_NODE",
                        $"Edge '{edge.id}' references a node that is not present in the document.",
                        DiagnosticSeverity.Error,
                        edgeID: edge.id
                    )
                );
                continue;
            }

            if (!TryGetPort(portsByNode, edge.output, out GraphPortDefinition outputPort)
                || !TryGetPort(portsByNode, edge.input, out GraphPortDefinition inputPort)
            ) {
                if (portsByNode.ContainsKey(edge.output.nodeID) && portsByNode.ContainsKey(edge.input.nodeID))
                {
                    diagnostics.Add(
                        new GraphDiagnostic(
                            "GRAPH_MISSING_ENDPOINT_PORT",
                            $"Edge '{edge.id}' references a port that is not active on its node.",
                            DiagnosticSeverity.Error,
                            edgeID: edge.id
                        )
                    );
                }

                continue;
            }

            if (outputPort.direction != GraphPortDirection.Output || inputPort.direction != GraphPortDirection.Input)
            {
                diagnostics.Add(
                    new GraphDiagnostic(
                        "GRAPH_INVALID_DIRECTION",
                        $"Edge '{edge.id}' must connect an output port to an input port.",
                        DiagnosticSeverity.Error,
                        edgeID: edge.id
                    )
                );
            }

            bool compatible = StringComparer.Ordinal.Equals(outputPort.valueTypeID, inputPort.valueTypeID)
                              || (conversion?.CanConvert(outputPort.valueTypeID, inputPort.valueTypeID) ?? false);
            if (!compatible)
            {
                diagnostics.Add(
                    new GraphDiagnostic(
                        "GRAPH_INCOMPATIBLE_TYPES",
                        $"Edge '{edge.id}' cannot convert '{outputPort.valueTypeID}' to '{inputPort.valueTypeID}'.",
                        DiagnosticSeverity.Error,
                        edgeID: edge.id
                    )
                );
            }

            CountConnection(edge.output, outputPort, connectionCounts, edge.id, diagnostics);
            CountConnection(edge.input, inputPort, connectionCounts, edge.id, diagnostics);
        }
    }

    private static void CountConnection(
        GraphEndpoint endpoint,
        GraphPortDefinition port,
        Dictionary<GraphEndpoint, int> connectionCounts,
        GraphEdgeID edgeID,
        List<GraphDiagnostic> diagnostics
    ) {
        connectionCounts.TryGetValue(endpoint, out int currentCount);
        currentCount++;
        connectionCounts[endpoint] = currentCount;
        if (port.capacity == GraphPortCapacity.Single && currentCount > 1)
        {
            diagnostics.Add(
                new GraphDiagnostic(
                    "GRAPH_PORT_CAPACITY",
                    $"Port '{endpoint.portID}' accepts only one connection.",
                    DiagnosticSeverity.Error,
                    endpoint.nodeID,
                    edgeID
                )
            );
        }
    }

    private static void ValidateRequiredInputs(
        GraphDocument document,
        IReadOnlyDictionary<GraphNodeID, IReadOnlyDictionary<GraphPortID, GraphPortDefinition>> portsByNode,
        List<GraphDiagnostic> diagnostics
    ) {
        HashSet<GraphEndpoint> connectedInputs = [];
        foreach (GraphEdgeRecord edge in document.edges)
        {
            connectedInputs.Add(edge.input);
        }

        foreach ((GraphNodeID nodeID, IReadOnlyDictionary<GraphPortID, GraphPortDefinition> ports) in portsByNode)
        {
            foreach (GraphPortDefinition port in ports.Values)
            {
                if (port.direction == GraphPortDirection.Input
                    && port.required
                    && !connectedInputs.Contains(new GraphEndpoint(nodeID, port.id))
                ) {
                    diagnostics.Add(
                        new GraphDiagnostic(
                            "GRAPH_REQUIRED_INPUT",
                            $"Required input '{port.displayName}' is not connected.",
                            DiagnosticSeverity.Error,
                            nodeID
                        )
                    );
                }
            }
        }
    }

    private static void ValidateCycles(
        GraphDocument document,
        List<GraphDiagnostic> diagnostics
    ) {
        var graph = new DependencyGraph<GraphNodeID>();
        foreach (GraphNodeRecord node in document.nodes)
        {
            graph.AddNode(node.id);
        }

        foreach (GraphEdgeRecord edge in document.edges)
        {
            if (graph.ContainsNode(edge.output.nodeID) && graph.ContainsNode(edge.input.nodeID))
            {
                graph.AddDependency(edge.input.nodeID, edge.output.nodeID);
            }
        }

        if (graph.TryFindCycle(out IReadOnlyList<GraphNodeID> cycle))
        {
            diagnostics.Add(
                new GraphDiagnostic(
                    "GRAPH_CYCLE",
                    "The graph contains a directed cycle.",
                    DiagnosticSeverity.Error,
                    cycle[0]
                )
            );
        }
    }

    private static bool TryGetPort(
        IReadOnlyDictionary<GraphNodeID, IReadOnlyDictionary<GraphPortID, GraphPortDefinition>> portsByNode,
        GraphEndpoint endpoint,
        out GraphPortDefinition port
    ) {
        if (portsByNode.TryGetValue(endpoint.nodeID, out IReadOnlyDictionary<GraphPortID, GraphPortDefinition>? ports)
            && ports.TryGetValue(endpoint.portID, out GraphPortDefinition? resolved)
        ) {
            port = resolved;
            return true;
        }

        port = null!;
        return false;
    }
}
