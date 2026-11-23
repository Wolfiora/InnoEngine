using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderGraphNodePortDefinitionConverter : SerializationConverter<ShaderGraphNodePortDefinition>
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
        ShaderGraphNodePortDefinition value
    ) {
        writer.Write("id", value.id);
        writer.Write("type", value.type);
        writer.Write("required", value.required);
    }

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated shader graph node port definition that represents the completed operation.
    /// </returns>
    public override ShaderGraphNodePortDefinition Read(SerializationReader reader)
        => new()
    {
        id = reader.Read<string>("id"),
        type = reader.Read<ShaderGraphType>("type"),
        required = reader.Read<bool>("required")
    };
}
