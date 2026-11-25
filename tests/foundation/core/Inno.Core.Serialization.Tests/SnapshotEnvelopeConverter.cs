using System;
using System.Collections.Generic;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class SnapshotEnvelopeConverter : SerializationConverter<SnapshotEnvelope>
{
    public override void Write(
        SerializationWriter writer,
        SnapshotEnvelope value
    ) {
        value.writeCalls++;
        value.observedTypeGeneration = writer.typeSnapshot.version;
        foreach (KeyValuePair<string, int> pair in value.fields)
        {
            writer.Write(pair.Key, pair.Value);
        }
        if (value.duplicate)
        {
            writer.Write("duplicate", 1);
            writer.Write("duplicate", 2);
        }
        if (!value.omitState)
        {
            writer.WritePropertiesData("state", value.state);
            writer.WriteObjectArray("items", value.items, static (
                child,
                number
            ) => child.Write("value", number));
        }
        if (value.fail)
        {
            throw new InvalidOperationException("Capture rejected by its source converter.");
        }
    }

    public override SnapshotEnvelope Read(SerializationReader reader) => throw new NotSupportedException();
}
