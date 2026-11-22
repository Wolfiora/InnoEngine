using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class ConvertedChildConverter : SerializationConverter<ConvertedChild>
{
    public override void Write(
        SerializationWriter writer,
        ConvertedChild value
    ) => writer.Write("value", value.value);

    public override ConvertedChild Read(SerializationReader reader) => new() { value = reader.Read<int>("value") };
}
