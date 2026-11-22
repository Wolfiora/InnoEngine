using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Rendering.Shaders;

internal sealed class ShaderGraphStageSettingsConverter : SerializationConverter<ShaderGraphStageSettings>
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
        ShaderGraphStageSettings value
    ) {
        writer.Write("stage", value.stage);
        writer.Write("outputs", value.outputs);
        writer.Write("threadsX", value.threadsX);
        writer.Write("threadsY", value.threadsY);
        writer.Write("threadsZ", value.threadsZ);
    }

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated shader graph stage settings that represents the completed operation.
    /// </returns>
    public override ShaderGraphStageSettings Read(SerializationReader reader)
        => new()
    {
        stage = reader.Read<ShaderStage>("stage"),
        outputs = reader.Read<ShaderGraphOutput[]>("outputs"),
        threadsX = reader.Read<int>("threadsX"),
        threadsY = reader.Read<int>("threadsY"),
        threadsZ = reader.Read<int>("threadsZ")
    };
}
