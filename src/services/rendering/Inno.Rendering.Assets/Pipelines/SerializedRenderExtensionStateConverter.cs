using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;
using Inno.Rendering;

namespace Inno.Rendering.Assets;

[Inno.Extensibility.Types.StableTypeId("a1b58cb9-3923-52f8-aa05-e9f282e5bdbd")]
internal sealed class SerializedRenderExtensionStateConverter : SerializationConverter<SerializedRenderExtensionState>
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
        SerializedRenderExtensionState value
    ) {
        writer.Write("stableTypeId", value.stableTypeId);
        writer.Write("propertyData", value.propertyData ?? []);
        writer.Write("dependencies", value.dependencies ?? []);
        if (writer.context.TryGet(out AssetDependencyCollection? dependencies) && dependencies is not null)
            foreach (AssetDependency dependency in value.dependencies ?? [])
                dependencies.Add(dependency);
    }

    /// <summary>
    /// Reconstructs a complete value through the configured serialization contract.
    /// </summary>
    /// <param name="reader">
    /// The reader that supplies the serialized representation.
    /// </param>
    /// <returns>
    /// The validated serialized render extension state that represents the completed operation.
    /// </returns>
    public override SerializedRenderExtensionState Read(SerializationReader reader) => new()
    {
        stableTypeId = reader.Read<System.Guid>("stableTypeId"),
        propertyData = reader.Read<byte[]>("propertyData"),
        dependencies = reader.Read<AssetDependency[]>("dependencies")
    };
}

