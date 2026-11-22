namespace Inno.Core.Graphs;

/// <summary>
/// Declares the direction in which values flow through a graph port.
/// </summary>
public enum GraphPortDirection
{
    /// <summary>
    /// Receives a value from another node.
    /// </summary>
    Input,
    /// <summary>
    /// Produces a value for another node.
    /// </summary>
    Output
}
