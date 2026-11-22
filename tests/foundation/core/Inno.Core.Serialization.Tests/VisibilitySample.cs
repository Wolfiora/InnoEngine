namespace Inno.Core.Serialization.Tests;

internal sealed class VisibilitySample : ISerializable
{
    [SerializableProperty]
    public int shown { get; set; }

    [SerializableProperty(PropertyVisibility.Hide)]
    public int hidden { get; set; }

    [SerializableProperty(PropertyVisibility.Readonly)]
    public int readOnly { get; set; }

    [SerializableProperty(PropertyVisibility.Transient)]
    public int transient { get; set; }

    [SerializableProperty(PropertyVisibility.SerializeOnly)]
    public int serializeOnly { get; set; }

    [SerializableProperty(PropertyVisibility.DeserializeOnly)]
    public int deserializeOnly { get; set; }
}
