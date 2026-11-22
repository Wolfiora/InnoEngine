namespace Inno.Core.Serialization.Tests;

internal sealed class MissingSetterSample : ISerializable
{
    [SerializableProperty]
    public int value { get; } = 1;
}
