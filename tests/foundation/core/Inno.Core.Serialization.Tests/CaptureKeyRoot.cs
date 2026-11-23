using System;
using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureKeyRoot : IdentityObject, ISerializable
{
    internal ulong? revision = 0;
    internal int number;
    internal string? text;
    internal bool allowKey = true;
    internal bool ignoreDecline;
    internal bool failKey;
    internal bool failWrite;
    internal int writeCalls;
    internal int keyCalls;
    internal ISerializable? properties;
    internal CaptureConvertedValue? converted;
    internal Action? onWrite;
    internal Action<SerializationCaptureKeyBuilder>? onKey;
    internal SerializationCaptureKeyBuilder? savedBuilder;

    internal void Touch() => revision++;
}
