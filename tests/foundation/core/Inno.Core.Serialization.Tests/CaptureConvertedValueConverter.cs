using System.Collections.Generic;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureConvertedValueConverter : SerializationConverter<CaptureConvertedValue>
{
    internal static readonly List<string> EVENTS = [];
    internal static int calls;

    public CaptureConvertedValueConverter()
    {
        EVENTS.Add("construct");
    }

    public override void Write(
        SerializationWriter writer,
        CaptureConvertedValue value
    ) {
        EVENTS.Add("convert-write");
        writer.Write("number", value.number);
        writer.Write("call", ++calls);
    }

    public override CaptureConvertedValue Read(SerializationReader reader) => new() { number = reader.Read<int>("number") };
}
