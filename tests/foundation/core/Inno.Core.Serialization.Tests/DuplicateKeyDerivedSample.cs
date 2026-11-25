namespace Inno.Core.Serialization.Tests;

internal sealed class DuplicateKeyDerivedSample : DuplicateKeyBaseSample
{
    [SerializableProperty]
    public new int value { get; set; }
}
