namespace Inno.Core.Graphs;

/// <summary>
/// Identifies one endpoint of a graph edge.
/// </summary>
public readonly record struct GraphEndpoint
{
    /// <summary>
    /// Creates a graph edge endpoint.
    /// </summary>
    /// <param name="nodeID">
    /// Owning node identifier.
    /// </param>
    /// <param name="portID">
    /// Port identifier within the node definition.
    /// </param>
    public GraphEndpoint(
        GraphNodeID nodeID,
        GraphPortID portID
    ) {
        this.nodeID = nodeID;
        this.portID = portID;
    }

    /// <summary>
    /// Gets the owning node identifier.
    /// </summary>
    public GraphNodeID nodeID { get; }

    /// <summary>
    /// Gets the node-local port identifier.
    /// </summary>
    public GraphPortID portID { get; }
}
