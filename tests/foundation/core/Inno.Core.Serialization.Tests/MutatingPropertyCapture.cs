namespace Inno.Core.Serialization.Tests;

internal sealed class MutatingPropertyCapture : ISerializable
{
    private readonly byte[] m_bytes = [1];

    [SerializableProperty(PropertyVisibility.Serialize)]
    public byte[] bytes => m_bytes;

    [SerializableProperty(PropertyVisibility.Serialize)]
    public int mutate => m_bytes[0] = 2;

    internal void Reset() => m_bytes[0] = 1;
}
