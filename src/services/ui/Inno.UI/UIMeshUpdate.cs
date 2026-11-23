using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Inno.UI;

/// <summary>
/// Publishes one immutable mesh generation to a rendering plugin.
/// </summary>
public sealed class UIMeshUpdate
{
    private readonly IReadOnlyList<UIVertex> m_vertices;
    private readonly IReadOnlyList<uint> m_indices;

    internal UIMeshUpdate(
        UIMeshHandle mesh,
        ulong revision,
        UIVertex[] vertices,
        uint[] indices
    ) {
        this.mesh = mesh;
        this.revision = revision;
        m_vertices = new ReadOnlyCollection<UIVertex>(vertices);
        m_indices = new ReadOnlyCollection<uint>(indices);
    }

    /// <summary>
    /// Gets the generation-scoped mesh handle.
    /// </summary>
    public UIMeshHandle mesh { get; }

    /// <summary>
    /// Gets the monotonically increasing mesh content revision.
    /// </summary>
    public ulong revision { get; }

    /// <summary>
    /// Gets immutable local-space vertices.
    /// </summary>
    public IReadOnlyList<UIVertex> vertices => m_vertices;

    /// <summary>
    /// Gets immutable triangle indices.
    /// </summary>
    public IReadOnlyList<uint> indices => m_indices;
}
