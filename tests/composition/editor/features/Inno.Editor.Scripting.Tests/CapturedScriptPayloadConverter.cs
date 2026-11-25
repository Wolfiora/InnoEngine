using System;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;

namespace Inno.Editor.Scripting.Tests;

internal sealed class CapturedScriptPayloadConverter : SerializationConverter<CapturedScriptPayload>
{
    public override void Write(
        SerializationWriter writer,
        CapturedScriptPayload value
    ) => writer.WritePropertiesData("state", value.payload);

    public override CapturedScriptPayload Read(SerializationReader reader)
        => throw new NotSupportedException("This fixture only captures an existing generation.");
}
