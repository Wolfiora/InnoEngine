using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderGraphTypeConverter : SerializationConverter<ShaderGraphType>
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
        ShaderGraphType value
    ) {
        writer.Write("id", value.id);
        writer.Write("element", value.element);
        writer.Write("length", value.length);
        writer.Write("fieldNames", value.fieldNames);
        writer.Write("fieldTypes", value.fieldTypes);
        writer.Write("isStorage", value.isStorage);
        writer.Write("isImage", value.isImage);
        writer.Write("storageElement", value.storageElement);
        writer.Write("access", value.access);
        writer.Write("format", value.format);
        writer.Write("dimension", value.dimension);
        writer.Write("isArray", value.isArray);
    }

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated shader graph type that represents the completed operation.
    /// </returns>
    public override ShaderGraphType Read(SerializationReader reader)
        => new()
    {
        id = reader.Read<string>("id"),
        element = reader.Read<ShaderGraphType?>("element"),
        length = reader.Read<int>("length"),
        fieldNames = reader.Read<string[]>("fieldNames"),
        fieldTypes = reader.Read<ShaderGraphType[]>("fieldTypes"),
        isStorage = reader.Read<bool>("isStorage"),
        isImage = reader.Read<bool>("isImage"),
        storageElement = reader.Read<ShaderGraphType?>("storageElement"),
        access = reader.Read<RenderStorageAccess>("access"),
        format = reader.Read<RenderTextureFormat>("format"),
        dimension = reader.Read<RenderTextureDimension>("dimension"),
        isArray = reader.Read<bool>("isArray")
    };
}
