using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class NearestBaseConverter : SerializationConverter<NearestBase>
{
    public override void Write(
        SerializationWriter writer,
        NearestBase value
    ) => writer.Write("value", value.value);

    public override NearestBase Read(SerializationReader reader)
        => new NearestLeaf { value = reader.Read<int>("value"), selectedBy = "base" };
}
