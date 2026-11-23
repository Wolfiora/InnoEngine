using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class GenericBoxConverter<T> : SerializationConverter<GenericBox<T>>
{
    public override void Write(
        SerializationWriter writer,
        GenericBox<T> value
    ) => writer.Write("value", value.value);

    public override GenericBox<T> Read(SerializationReader reader) => new(reader.Read<T>("value"));
}
