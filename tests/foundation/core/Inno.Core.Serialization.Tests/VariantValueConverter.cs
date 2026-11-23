using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class VariantValueConverter : SerializationConverter<IVariantValue<object>>
{
    public override void Write(
        SerializationWriter writer,
        IVariantValue<object> value
    ) => writer.Write("value", (string)value.value);

    public override IVariantValue<object> Read(SerializationReader reader)
        => new VariantValue(reader.Read<string>("value"));
}
