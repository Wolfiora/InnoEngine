namespace Inno.Core.Serialization.Tests;

internal sealed class PreviousSchemaSample : ISerializable
{
    [SerializableProperty]
    public int changed { get; set; }

    [SerializableProperty(PropertyVisibility.Hide)]
    public string compatible { get; set; } = string.Empty;

    [SerializableProperty]
    public int removed { get; set; }
}
