namespace Inno.Core.Serialization.Tests;

internal sealed class NearestHost : ISerializable
{
    [SerializableProperty]
    public NearestLeaf? item { get; set; }
}
