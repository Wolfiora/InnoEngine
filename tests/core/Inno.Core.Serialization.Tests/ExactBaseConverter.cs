using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class ExactBaseConverter : SerializationConverter<ExactBase>
{
    public override void Write(
        SerializationWriter writer,
        ExactBase value
    ) => writer.Write("value", value.value);

    public override ExactBase Read(SerializationReader reader)
        => new ExactLeaf { value = reader.Read<int>("value"), selectedBy = "base" };
}
