using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

[SerializationRevision("revision")]
internal sealed class ConvertedCaptureProperties : IdentityObject, ISerializable
{
    [SerializableProperty]
    public CaptureConvertedValue value = new() { number = 1 };

    private ulong? revision => 0;
}
