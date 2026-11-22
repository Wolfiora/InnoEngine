using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderGraphInputSettingsConverter : SerializationConverter<ShaderGraphInputSettings>
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
        ShaderGraphInputSettings value
    ) {
        writer.Write("id", value.id);
        writer.Write("type", value.type);
        writer.Write("kind", value.kind);
        writer.Write("semantic", value.semantic);
        writer.Write("location", value.location);
    }

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated shader graph input settings that represents the completed operation.
    /// </returns>
    public override ShaderGraphInputSettings Read(SerializationReader reader)
        => new()
    {
        id = reader.Read<string>("id"),
        type = reader.Read<ShaderGraphType>("type"),
        kind = reader.Read<ShaderIrInputKind>("kind"),
        semantic = reader.Read<string>("semantic"),
        location = reader.Read<int>("location")
    };
}
