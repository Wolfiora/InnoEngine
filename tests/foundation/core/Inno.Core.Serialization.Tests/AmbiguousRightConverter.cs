using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class AmbiguousRightConverter : SerializationConverter<IAmbiguousRight>
{
    public override void Write(
        SerializationWriter writer,
        IAmbiguousRight value
    ) {}

    public override IAmbiguousRight Read(SerializationReader reader) => new AmbiguousValue();
}
