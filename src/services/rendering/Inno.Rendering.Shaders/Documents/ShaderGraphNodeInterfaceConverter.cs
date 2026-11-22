using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderGraphNodeInterfaceConverter : SerializationConverter<ShaderGraphNodeInterface>
{
    /// <summary>
    /// Writes the complete value through the configured serialization contract.
    /// </summary>
    /// <param name="writer">
    /// The writer that receives the serialized representation.
    /// </param>
    /// <param name="value">
    /// The concrete value read or transformed by this operation.
    /// </param>
    public override void Write(
        SerializationWriter writer,
        ShaderGraphNodeInterface value
    ) {
        writer.Write("displayName", value.displayName);
        writer.Write("createPath", value.createPath);
        writer.Write("createOrder", value.createOrder);
        writer.Write("kind", value.kind);
        writer.Write("effect", value.effect);
        writer.Write("role", value.role);
        writer.Write("inputs", value.inputs);
        writer.Write("outputs", value.outputs);
    }

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated shader graph node interface that represents the completed operation.
    /// </returns>
    public override ShaderGraphNodeInterface Read(SerializationReader reader)
        => new()
    {
        displayName = reader.Read<string>("displayName"),
        createPath = reader.Read<string>("createPath"),
        createOrder = reader.Read<int>("createOrder"),
        kind = reader.Read<ShaderGraphNodeKind>("kind"),
        effect = reader.Read<ShaderGraphNodeEffect>("effect"),
        role = reader.Read<string>("role"),
        inputs = reader.Read<ShaderGraphNodePortDefinition[]>("inputs"),
        outputs = reader.Read<ShaderGraphNodePortDefinition[]>("outputs")
    };
}
