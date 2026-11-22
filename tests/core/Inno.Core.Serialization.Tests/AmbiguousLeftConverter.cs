using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class AmbiguousLeftConverter : SerializationConverter<IAmbiguousLeft>
{
    public override void Write(
        SerializationWriter writer,
        IAmbiguousLeft value
    ) {}

    public override IAmbiguousLeft Read(SerializationReader reader) => new AmbiguousValue();
}
