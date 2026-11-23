namespace Inno.Core.Serialization.Tests;

internal sealed class CycleHost : ISerializable
{
    [SerializableProperty]
    public CycleValue? value { get; set; }
}
