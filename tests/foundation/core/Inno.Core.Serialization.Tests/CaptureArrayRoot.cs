using System;
using System.Collections.Generic;

namespace Inno.Core.Serialization.Tests;

internal sealed class CaptureArrayRoot : ISerializable
{
    internal List<CaptureArrayRow> rows = [];
    internal Guid scope = Guid.NewGuid();
    internal int mode;
    internal int keys;
    internal bool allowKeys = true;
    internal bool stateful;
    internal bool failKey;
    internal Action? onWrite;
    internal SerializationCaptureKeyBuilder? savedBuilder;
}
