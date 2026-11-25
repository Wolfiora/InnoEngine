namespace Inno.Core.Serialization.Tests;

internal sealed class MissingConstructorSample : ISerializable
{
    public MissingConstructorSample(int value)
    {
        this.value = value;
    }

    [SerializableProperty]
    public int value { get; set; }
}
