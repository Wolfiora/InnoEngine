namespace Inno.Core.Serialization.Tests;

internal sealed class ConvertedChildHost : ISerializable
{
    [SerializableProperty]
    public ConvertedChild? child { get; set; }
}
