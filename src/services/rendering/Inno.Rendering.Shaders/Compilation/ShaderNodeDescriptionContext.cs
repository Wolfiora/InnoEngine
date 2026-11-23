using System;
using Inno.Core.Graphs;
using Inno.Core.Serialization;

namespace Inno.Rendering.Shaders;

/// <summary>
/// Provides invocation-scoped property decoding through the complete owner serialization context.
/// </summary>
public sealed class ShaderNodeDescriptionContext
{
    private readonly GraphNodeRecord m_node;
    private readonly SerializationRegistry m_serialization;
    private readonly SerializationContext m_context;

    internal ShaderNodeDescriptionContext(
        GraphNodeRecord node,
        SerializationRegistry serialization,
        SerializationContext context,
        ShaderSourceModuleAnalysis? sourceModule,
        string implementationID,
        ShaderIrStageInput? stageInput
    ) {
        m_node = node;
        m_serialization = serialization;
        m_context = context;
        this.sourceModule = sourceModule;
        this.implementationID = implementationID;
        this.stageInput = stageInput;
    }

    /// <summary>
    /// Gets the node's stable identity, never its editor runtime handle.
    /// </summary>
    public GraphNodeID nodeID => m_node.id;

    /// <summary>
    /// Gets the node definition's stable identity.
    /// </summary>
    public string definitionID => m_node.definitionID;

    /// <summary>
    /// Gets a frozen module resolved by the asset owner; null means unassigned or unavailable.
    /// </summary>
    public ShaderSourceModuleAnalysis? sourceModule { get; }

    /// <summary>
    /// Gets the exact implementation key chosen by the target.
    /// </summary>
    public string implementationID { get; }

    /// <summary>
    /// Gets the target-assigned stage input for this node, when applicable.
    /// </summary>
    public ShaderIrStageInput? stageInput { get; }

    /// <summary>
    /// Reads a property using the owner's converter generation and complete reference resolver context.
    /// </summary>
    /// <typeparam name="T">
    /// Declared property type supported by the native serializer.
    /// </typeparam>
    /// <param name="id">
    /// Stable property identity.
    /// </param>
    /// <param name="defaultValue">
    /// Value used only when the property is absent, never after corrupt-data failure.
    /// </param>
    /// <returns>
    /// The deserialized value or the supplied absent-property default.
    /// </returns>
    public T Read<T>(
        string id,
        T defaultValue
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        return m_node.TryGetValue(id, out GraphSerializedValue? value)
               ? m_serialization.Decode(value!.data.Span, reader => reader.Read<T>("value"), m_context)
               : defaultValue;
    }
}
