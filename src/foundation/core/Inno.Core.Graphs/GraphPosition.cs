namespace Inno.Core.Graphs;

/// <summary>
/// Stores a graph-space position independently from any editor UI framework.
/// </summary>
public readonly record struct GraphPosition
{
    /// <summary>
    /// Creates a graph-space position.
    /// </summary>
    /// <param name="x">
    /// Horizontal graph-space coordinate.
    /// </param>
    /// <param name="y">
    /// Vertical graph-space coordinate.
    /// </param>
    public GraphPosition(
        float x,
        float y
    ) {
        this.x = x;
        this.y = y;
    }

    /// <summary>
    /// Gets the horizontal graph-space coordinate.
    /// </summary>
    public float x { get; }

    /// <summary>
    /// Gets the vertical graph-space coordinate.
    /// </summary>
    public float y { get; }
}
