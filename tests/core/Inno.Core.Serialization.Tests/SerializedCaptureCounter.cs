using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

[SerializationRevision("revision")]
internal sealed class SerializedCaptureCounter : IdentityObject, ISerializable
{
    [SerializableProperty(PropertyVisibility.Serialize)]
    public ulong revision => 0;
}
