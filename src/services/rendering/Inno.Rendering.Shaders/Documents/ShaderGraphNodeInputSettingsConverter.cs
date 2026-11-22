using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderGraphNodeInputSettingsConverter : SerializationConverter<ShaderGraphNodeInputSettings>
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
        ShaderGraphNodeInputSettings value
    ) => writer.Write("ports", value.ports);

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated shader graph node input settings that represents the completed operation.
    /// </returns>
    public override ShaderGraphNodeInputSettings Read(SerializationReader reader)
        => new()
    {
        ports = reader.Read<ShaderGraphNodePortDefinition[]>("ports")
    };
}
