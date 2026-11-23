namespace Inno.Core.Graphs;

/// <summary>
/// Stores one typed connection between two graph endpoints.
/// </summary>
public sealed class GraphEdgeRecord
{
    /// <summary>
    /// Creates a graph edge record.
    /// </summary>
    /// <param name="id">
    /// Stable edge identifier within the document.
    /// </param>
    /// <param name="output">
    /// Source endpoint.
    /// </param>
    /// <param name="input">
    /// Destination endpoint.
    /// </param>
    public GraphEdgeRecord(
        GraphEdgeID id,
        GraphEndpoint output,
        GraphEndpoint input
    ) {
        this.id = id;
        this.output = output;
        this.input = input;
    }

    /// <summary>
    /// Gets the stable edge identifier.
    /// </summary>
    public GraphEdgeID id { get; }

    /// <summary>
    /// Gets the source endpoint.
    /// </summary>
    public GraphEndpoint output { get; }

    /// <summary>
    /// Gets the destination endpoint.
    /// </summary>
    public GraphEndpoint input { get; }
}
