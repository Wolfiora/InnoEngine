using System;
using System.IO;

namespace Inno.Core.Serialization.Tests;

internal sealed class RejectCaptureStream : MemoryStream
{
    public override void Write(ReadOnlySpan<byte> buffer) => throw new IOException("The test destination rejects writes.");
}
