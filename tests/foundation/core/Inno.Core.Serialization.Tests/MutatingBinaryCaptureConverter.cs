using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class MutatingBinaryCaptureConverter : SerializationConverter<MutatingBinaryCapture>
{
    public override void Write(
        SerializationWriter writer,
        MutatingBinaryCapture value
    ) {
        writer.Write("bytes", value.bytes);
        value.bytes[0] = 2;
    }

    public override MutatingBinaryCapture Read(SerializationReader reader) => new() { bytes = reader.Read<byte[]>("bytes") };
}
