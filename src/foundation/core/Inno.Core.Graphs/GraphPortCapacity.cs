namespace Inno.Core.Graphs;

/// <summary>
/// Declares how many edges may connect to a graph port.
/// </summary>
public enum GraphPortCapacity
{
    /// <summary>
    /// At most one edge may connect to the port.
    /// </summary>
    Single,
    /// <summary>
    /// Any number of edges may connect to the port.
    /// </summary>
    Multiple
}
