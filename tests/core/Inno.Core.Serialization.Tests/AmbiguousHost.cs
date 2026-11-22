namespace Inno.Core.Serialization.Tests;

internal sealed class AmbiguousHost : ISerializable
{
    [SerializableProperty]
    public AmbiguousValue? item { get; set; }
}
