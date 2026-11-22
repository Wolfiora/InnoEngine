namespace Inno.Core.Serialization.Tests;

internal sealed class ConvertedConstructorSample : ISerializable
{
    public ConvertedConstructorSample(int value)
    {
        this.value = value;
    }

    public int value { get; set; }
}
