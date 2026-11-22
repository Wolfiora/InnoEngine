using Inno.Core.Identity;

namespace Inno.Core.Serialization.Tests;

internal sealed class UntrackedCaptureProperties : IdentityObject, ISerializable
{
    [SerializableProperty]
    public int number;
}
