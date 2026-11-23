using System;
using Inno.Core.Serialization.Converters;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureKeyRootConverter : SerializationConverter<CaptureKeyRoot>
{
    public override void Write(
        SerializationWriter writer,
        CaptureKeyRoot value
    ) {
        value.writeCalls++;
        value.onWrite?.Invoke();
        if (value.failWrite)
        {
            throw new InvalidOperationException("Capture writer failure.");
        }
        writer.Write("number", value.number);
        writer.Write("text", value.text);
        writer.Write("context", writer.context.TryGet(out CaptureKeyContext? context) ? context!.value : 0UL);
        if (value.properties is not null)
        {
            writer.WritePropertiesData("properties", value.properties);
        }
        if (value.converted.HasValue)
        {
            writer.Write("converted", value.converted.Value);
        }
    }

    public override CaptureKeyRoot Read(SerializationReader reader) => throw new NotSupportedException();

    protected override bool TryCaptureKey(
        CaptureKeyRoot value,
        SerializationCaptureKeyBuilder key
    ) {
        value.keyCalls++;
        value.savedBuilder = key;
        if (value.failKey)
        {
            throw new InvalidOperationException("Capture key failure.");
        }
        if (!value.allowKey)
        {
            return false;
        }
        key.EnsureCapacity(5);
        bool complete = key.TryAddOwner(value, value.revision);
        key.AddValue(key.context.TryGet(out CaptureKeyContext? context) ? context!.value : 0UL);
        if (value.properties is not null)
        {
            complete &= key.TryAddProperties(value.properties);
        }
        value.onKey?.Invoke(key);
        return complete || value.ignoreDecline;
    }
}
