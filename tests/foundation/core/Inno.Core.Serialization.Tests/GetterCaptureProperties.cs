using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

[SerializationRevision("revision")]
internal sealed class GetterCaptureProperties : IdentityObject, ISerializable
{
    [SerializableProperty]
    public CaptureGetterStruct value = new();

    private ulong? revision => 0;
}
