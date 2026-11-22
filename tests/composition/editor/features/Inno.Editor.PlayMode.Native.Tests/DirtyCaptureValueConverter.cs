using System;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Editor.PlayMode.Native.Tests;

internal sealed class DirtyCaptureValueConverter : SerializationConverter<DirtyCaptureValue>
{
    public override void Write(
        SerializationWriter writer,
        DirtyCaptureValue value
    ) {
        value.reads++;
        writer.Write("value", value.value);
        if (value.reject)
        {
            throw new InvalidOperationException("The test-owned converter rejected capture.");
        }
    }

    public override DirtyCaptureValue Read(SerializationReader reader) => new() { value = reader.Read<int>("value") };
}
