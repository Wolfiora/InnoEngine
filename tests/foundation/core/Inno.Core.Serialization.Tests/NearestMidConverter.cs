using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class NearestMidConverter : SerializationConverter<NearestMid>
{
    public override void Write(
        SerializationWriter writer,
        NearestMid value
    ) => writer.Write("value", value.value);

    public override NearestMid Read(SerializationReader reader)
        => new NearestLeaf { value = reader.Read<int>("value"), selectedBy = "mid" };
}
