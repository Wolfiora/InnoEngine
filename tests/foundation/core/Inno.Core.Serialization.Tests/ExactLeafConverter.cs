using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class ExactLeafConverter : SerializationConverter<ExactLeaf>
{
    public override void Write(
        SerializationWriter writer,
        ExactLeaf value
    ) => writer.Write("value", value.value);

    public override ExactLeaf Read(SerializationReader reader)
        => new() { value = reader.Read<int>("value"), selectedBy = "exact" };
}
