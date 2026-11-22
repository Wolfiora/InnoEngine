using System.Threading;

namespace Inno.Core.Serialization;

internal abstract record SerializationNode
{
    private string? m_capturePath;

    internal string? capturePath => Volatile.Read(ref m_capturePath);

    internal void RememberPath(string path)
    {
        if (this is not NullSerializationNode)
        {
            Interlocked.CompareExchange(ref m_capturePath, path, null);
        }
    }
}

internal sealed record ScalarSerializationNode(object value) : SerializationNode;

internal sealed record BinarySerializationNode(byte[] value) : SerializationNode
{
    internal PropertyDataCapture? properties { get; init; }
}
