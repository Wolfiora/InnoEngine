namespace Inno.Core.Serialization.Tests;

internal sealed class GenericBoxHost : ISerializable
{
    [SerializableProperty]
    public GenericBox<int>? box { get; set; }
}
