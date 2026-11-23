using System;
using Inno.Core.Diagnostics;

namespace Inno.Core.Graphs;

/// <summary>
/// Reports one graph validation problem using stable document identifiers.
/// </summary>
public sealed class GraphDiagnostic
{
    /// <summary>
    /// Creates a graph diagnostic.
    /// </summary>
    /// <param name="code">
    /// Stable machine-readable diagnostic code.
    /// </param>
    /// <param name="message">
    /// Artist-facing diagnostic text.
    /// </param>
    /// <param name="severity">
    /// Diagnostic impact.
    /// </param>
    /// <param name="nodeID">
    /// Optional related node identifier.
    /// </param>
    /// <param name="edgeID">
    /// Optional related edge identifier.
    /// </param>
    public GraphDiagnostic(
        string code,
        string message,
        DiagnosticSeverity severity,
        GraphNodeID? nodeID = null,
        GraphEdgeID? edgeID = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        this.code = code;
        this.message = message;
        this.severity = severity;
        this.nodeID = nodeID;
        this.edgeID = edgeID;
    }

    /// <summary>
    /// Gets the stable machine-readable diagnostic code.
    /// </summary>
    public string code { get; }

    /// <summary>
    /// Gets the artist-facing diagnostic text.
    /// </summary>
    public string message { get; }

    /// <summary>
    /// Gets the diagnostic impact.
    /// </summary>
    public DiagnosticSeverity severity { get; }

    /// <summary>
    /// Gets the related node identifier, if any.
    /// </summary>
    public GraphNodeID? nodeID { get; }

    /// <summary>
    /// Gets the related edge identifier, if any.
    /// </summary>
    public GraphEdgeID? edgeID { get; }
}
