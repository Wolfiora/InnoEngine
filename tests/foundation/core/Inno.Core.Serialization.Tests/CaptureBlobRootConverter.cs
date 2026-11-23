using System;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureBlobRootConverter : SerializationConverter<CaptureBlobRoot>
{
    public override void Write(
        SerializationWriter writer,
        CaptureBlobRoot value
    ) => writer.Write("data", value.data);

    public override CaptureBlobRoot Read(SerializationReader reader) => throw new NotSupportedException();

    protected override bool TryCaptureKey(
        CaptureBlobRoot value,
        SerializationCaptureKeyBuilder key
    ) => key.TryAddOwner(value, 0);
}
