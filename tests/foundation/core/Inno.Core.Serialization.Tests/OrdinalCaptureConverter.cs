using System;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class OrdinalCaptureConverter : SerializationConverter<OrdinalCapture>
{
    public override void Write(
        SerializationWriter writer,
        OrdinalCapture value
    ) {
        foreach (string name in value.names)
        {
            writer.Write(name, 7);
        }
    }

    public override OrdinalCapture Read(SerializationReader reader) => throw new NotSupportedException();
}
