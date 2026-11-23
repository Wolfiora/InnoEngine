using Inno.Core.Graphs;
using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Supplies neutral authoring data to a domain target without exposing an adapter or live asset.
/// </summary>
public sealed class ShaderTargetContext
{
    internal ShaderTargetContext(
        GraphDocument document,
        SerializationRegistry serialization,
        SerializationContext references
    ) {
        this.document = document;
        this.serialization = serialization;
        this.references = references;
    }

    /// <summary>
    /// Gets a private document copy; expansion cannot modify the authored source.
    /// </summary>
    public GraphDocument document { get; }

    /// <summary>
    /// Gets the current owner converter registry, valid only during expansion.
    /// </summary>
    public SerializationRegistry serialization { get; }

    /// <summary>
    /// Gets the complete owner reference context used for generated resource defaults.
    /// </summary>
    public SerializationContext references { get; }
}
