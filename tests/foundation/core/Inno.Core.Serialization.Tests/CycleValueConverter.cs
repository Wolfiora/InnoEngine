using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class CycleValueConverter : SerializationConverter<CycleValue>
{
    public override void Write(
        SerializationWriter writer,
        CycleValue value
    ) => writer.Write("next", value.next);

    public override CycleValue Read(SerializationReader reader) => new() { next = reader.Read<CycleValue?>("next") };
}
