namespace Inno.Core.Serialization.Tests;

internal sealed class ExactHost : ISerializable
{
    [SerializableProperty]
    public ExactLeaf? item { get; set; }
}
