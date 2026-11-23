namespace Inno.Core.Serialization.Tests;

// Open: DuplicateKeyDerivedSample derives from it.
internal class DuplicateKeyBaseSample : ISerializable
{
    [SerializableProperty]
    public int value { get; set; }
}
