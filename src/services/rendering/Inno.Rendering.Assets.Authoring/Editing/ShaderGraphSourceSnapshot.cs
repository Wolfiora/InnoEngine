using Inno.Core.Graphs;
using Inno.Extensibility.Types;

namespace Inno.Rendering.Assets.Authoring;

/// <summary>
/// Contains detached graph source state, not a compiled or live runtime asset.
/// </summary>
[StableTypeID("fb1d9ba2-d37c-59f1-a7ef-17feeb3beb0e")]
public sealed class ShaderGraphSourceSnapshot
{
    private readonly GraphDocument m_document;

    internal ShaderGraphSourceSnapshot(
        GraphDocument document,
        string hash,
        bool readOnly
    ) {
        m_document = document;
        this.contentHash = hash;
        this.isReadOnly = readOnly;
    }

    /// <summary>
    /// Gets a detached copy of authored graph records.
    /// </summary>
    public GraphDocument document => m_document.Clone();

    /// <summary>
    /// Gets the source fingerprint used to reject external-edit conflicts.
    /// </summary>
    public string contentHash { get; }

    /// <summary>
    /// Gets whether the source belongs to an immutable installation mount.
    /// </summary>
    public bool isReadOnly { get; }
}
