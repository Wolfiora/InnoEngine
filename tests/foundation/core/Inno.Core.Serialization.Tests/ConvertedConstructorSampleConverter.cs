using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class ConvertedConstructorSampleConverter : SerializationConverter<ConvertedConstructorSample>
{
    public override void Write(
        SerializationWriter writer,
        ConvertedConstructorSample value
    ) => writer.Write("value", value.value);

    public override ConvertedConstructorSample Read(SerializationReader reader) => new(reader.Read<int>("value"));

    public override void Restore(
        SerializationReader reader,
        ConvertedConstructorSample target
    ) => target.value = reader.Read<int>("value");
}
