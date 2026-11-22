namespace Inno.Core.Serialization.Tests;

internal sealed class ConcurrentCaptureValue(int count) : ISerializable
{
    internal int count { get; } = count;
}
