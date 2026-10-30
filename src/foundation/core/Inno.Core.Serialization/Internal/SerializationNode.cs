using System;
using System.Collections.Generic;
using System.Threading;

namespace Inno.Core.Serialization;

internal abstract record SerializationNode
{
    private string? m_capturePath;

    internal string? capturePath => Volatile.Read(ref m_capturePath);

    internal void RememberPath(string path)
    {
        if (this is not NullSerializationNode)
            Interlocked.CompareExchange(ref m_capturePath, path, null);
    }
}

internal sealed record NullSerializationNode : SerializationNode
{
    internal static NullSerializationNode instance { get; } = new();

    private NullSerializationNode()
    {
    }
}

internal sealed record ScalarSerializationNode(object value) : SerializationNode;

internal sealed record BinarySerializationNode(byte[] value) : SerializationNode
{
    internal PropertyDataCapture? properties { get; init; }
}

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

internal sealed record ArraySerializationNode : SerializationNode
{
    internal ArraySerializationNode() : this([]) { }

    internal ArraySerializationNode(List<SerializationNode> values) => this.values = values;

    internal List<SerializationNode> values { get; }
}

internal sealed record MapSerializationNode : SerializationNode
{
    internal List<KeyValuePair<SerializationNode, SerializationNode>> values { get; } = [];
}
