namespace Inno.Core.Serialization.Tests;

internal sealed class MissingClassHost : ISerializable
{
    [SerializableProperty]
    public MissingChild? child { get; set; }
}
