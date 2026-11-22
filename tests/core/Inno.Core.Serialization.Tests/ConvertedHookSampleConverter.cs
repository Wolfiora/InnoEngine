using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class ConvertedHookSampleConverter : SerializationConverter<ConvertedHookSample>
{
    public override void Write(
        SerializationWriter writer,
        ConvertedHookSample value
    ) => writer.Write("value", value.value);

    public override ConvertedHookSample Read(SerializationReader reader) => new() { value = reader.Read<int>("value") };

    public override void Restore(
        SerializationReader reader,
        ConvertedHookSample target
    ) => target.value = reader.Read<int>("value");
}
