using Inno.Core.Graphs;

namespace Inno.Editor.Graph;

/// <summary>
/// Contains a neutral copied node fragment suitable for graph clipboard operations.
/// </summary>
public sealed class GraphClipboardData
{
    private readonly GraphDocument m_fragment;

    internal GraphClipboardData(GraphDocument fragment)
    {
        m_fragment = fragment;
    }

    /// <summary>
    /// Gets the copied node count.
    /// </summary>
    public int nodeCount => m_fragment.nodes.Count;

    /// <summary>
    /// Gets the copied connection count whose endpoints both belong to the fragment.
    /// </summary>
    public int edgeCount => m_fragment.edges.Count;

    internal GraphDocument CloneFragment() => m_fragment.Clone();
}
