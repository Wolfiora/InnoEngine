using System;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureArrayRootConverter : SerializationConverter<CaptureArrayRoot>
{
    public override void Write(
        SerializationWriter writer,
        CaptureArrayRoot value
    ) {
        writer.Write("scope", value.scope);
        writer.WriteObjectArray(
            "rows",
            value.rows,
            (
                child,
                row
            ) =>
            {
                row.writes++;
                value.onWrite?.Invoke();
                child.Write("number", row.number);
                child.Write("mode", value.mode);
                if (value.stateful)
                {
                    child.WritePropertiesData("state", row.state);
                }
            },
            captureKey: (
                row,
                key
            ) =>
            {
                value.keys++;
                value.savedBuilder = key;
                if (value.failKey)
                {
                    throw new InvalidOperationException("Array key failure.");
                }
                if (!value.allowKeys || !key.TryAddOwner(row, row.revision))
                {
                    return false;
                }
                key.AddValue(value.scope);
                key.AddValue(unchecked((ulong)value.mode));
                key.AddValue(value.stateful ? 1UL : 0UL);
                return true;
            }
        );
    }

    public override CaptureArrayRoot Read(SerializationReader reader) => throw new NotSupportedException();
}
