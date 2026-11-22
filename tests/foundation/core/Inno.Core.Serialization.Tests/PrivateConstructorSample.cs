namespace Inno.Core.Serialization.Tests;

internal sealed class PrivateConstructorSample : ISerializable
{
    private PrivateConstructorSample()
    {
        this.wasConstructed = true;
    }

    [SerializableProperty]
    public int value { get; set; }

    public bool wasConstructed { get; }
}
