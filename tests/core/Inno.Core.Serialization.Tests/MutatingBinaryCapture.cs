namespace Inno.Core.Serialization.Tests;

internal sealed class MutatingBinaryCapture : ISerializable
{
    internal byte[] bytes = [1];
}
