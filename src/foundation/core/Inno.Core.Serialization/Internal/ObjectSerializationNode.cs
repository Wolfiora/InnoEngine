using System;
using System.Collections.Generic;
using System.Threading;

namespace Inno.Core.Serialization;

internal sealed record ObjectSerializationNode : SerializationNode
{
    private byte[]? m_encodedCapture;
    private int m_encodingTooLarge;

    internal SerializationCaptureKey? captureKey { get; set; }
    internal OrderedDictionary<string, SerializationNode> values { get; } = new(StringComparer.Ordinal);
    internal byte[]? encodedCapture => Volatile.Read(ref m_encodedCapture);
    internal bool encodingTooLarge => Volatile.Read(ref m_encodingTooLarge) != 0;

    internal byte[] PublishEncodedCapture(byte[] data) => Interlocked.CompareExchange(ref m_encodedCapture, data, null) ?? data;

    internal void RejectEncodedCapture() => Volatile.Write(ref m_encodingTooLarge, 1);
}
